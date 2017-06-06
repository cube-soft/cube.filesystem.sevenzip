using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Cube.FileSystem.SevenZip
{
    public class StreamWrapper : IDisposable
    {
        protected Stream BaseStream;

        protected StreamWrapper(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        public void Dispose()
        {
            BaseStream.Close();
        }

        public virtual void Seek(long offset, uint seekOrigin, IntPtr newPosition)
        {
            long Position = (uint)BaseStream.Seek(offset, (SeekOrigin)seekOrigin);
            if (newPosition != IntPtr.Zero)
                Marshal.WriteInt64(newPosition, Position);
        }
    }

    public class InStreamWrapper : StreamWrapper, ISequentialInStream, IInStream
    {
        public InStreamWrapper(Stream baseStream) : base(baseStream) { }

        public uint Read(byte[] data, uint size)
        {
            return (uint)BaseStream.Read(data, 0, (int)size);
        }
    }

    // Can close base stream after period of inactivity and reopen it when needed.
    // Useful for long opened archives (prevent locking archive file on disk).
    public class InStreamTimedWrapper : StreamWrapper, ISequentialInStream, IInStream
    {
        private string BaseStreamFileName;
        private long BaseStreamLastPosition;
        private Timer CloseTimer;

        private const int KeepAliveInterval = 10 * 1000; // 10 sec

        public InStreamTimedWrapper(Stream baseStream)
          : base(baseStream)
        {
            if ((BaseStream is FileStream) && !BaseStream.CanWrite && BaseStream.CanSeek)
            {
                BaseStreamFileName = ((FileStream)BaseStream).Name;
                CloseTimer = new Timer(new TimerCallback(CloseStream), null, KeepAliveInterval, Timeout.Infinite);
            }
        }

        private void CloseStream(object state)
        {
            if (CloseTimer != null)
            {
                CloseTimer.Dispose();
                CloseTimer = null;
            }

            if (BaseStream != null)
            {
                if (BaseStream.CanSeek)
                    BaseStreamLastPosition = BaseStream.Position;
                BaseStream.Close();
                BaseStream = null;
            }
        }

        protected void ReopenStream()
        {
            if (BaseStream == null)
            {
                if (BaseStreamFileName != null)
                {
                    BaseStream = new FileStream(BaseStreamFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BaseStream.Position = BaseStreamLastPosition;
                    CloseTimer = new Timer(new TimerCallback(CloseStream), null, KeepAliveInterval, Timeout.Infinite);
                }
                else
                    throw new ObjectDisposedException("StreamWrapper");
            }
            else
              if (CloseTimer != null)
                CloseTimer.Change(KeepAliveInterval, Timeout.Infinite);
        }

        /*public int Read(byte[] data, uint size, IntPtr processedSize)
        {
          int Processed = BaseStream.Read(data, 0, (int)size);
          if (processedSize != IntPtr.Zero)
            Marshal.WriteInt32(processedSize, Processed);
          return 0;
        }*/

        public uint Read(byte[] data, uint size)
        {
            ReopenStream();
            return (uint)BaseStream.Read(data, 0, (int)size);
        }

        public override void Seek(long offset, uint seekOrigin, IntPtr newPosition)
        {
            ReopenStream();
            base.Seek(offset, seekOrigin, newPosition);
        }
    }

    public class OutStreamWrapper : StreamWrapper, ISequentialOutStream, IOutStream
    {
        public OutStreamWrapper(Stream baseStream) : base(baseStream) { }

        public int SetSize(long newSize)
        {
            BaseStream.SetLength(newSize);
            return 0;
        }

        public int Write(byte[] data, uint size, IntPtr processedSize)
        {
            BaseStream.Write(data, 0, (int)size);
            if (processedSize != IntPtr.Zero)
                Marshal.WriteInt32(processedSize, (int)size);
            return 0;
        }
    }
}
