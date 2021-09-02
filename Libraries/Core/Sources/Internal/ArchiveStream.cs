/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    #region ArchiveStreamBase

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamBase
    ///
    /// <summary>
    /// Represents the base class for streams that handle archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveStreamBase : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamBase
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveStreamBase class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="baseStream">Target stream.</param>
        /// <param name="disposeStream">
        /// Value indicating whether to discard the BaseStream object when
        /// disposed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveStreamBase(Stream baseStream, bool disposeStream)
        {
            BaseStream = baseStream;
            _disposeStream = disposeStream;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// BaseStream
        ///
        /// <summary>
        /// Gets the target stream.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Stream BaseStream { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Seek
        ///
        /// <summary>
        /// Sets the position of the stream.
        /// The metod implements IInStream.Seek(long, SeekOrigin, IntPtr).
        /// </summary>
        ///
        /// <param name="offset">Offset value from the origin.</param>
        /// <param name="origin">Starting position.</param>
        /// <param name="result">Position after setting.</param>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Seek(long offset, SeekOrigin origin, IntPtr result)
        {
            var pos = BaseStream.Seek(offset, origin);
            if (result != IntPtr.Zero) Marshal.WriteInt64(result, pos);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing && _disposeStream) BaseStream.Dispose();
        }

        #endregion

        #region Fields
        private readonly bool _disposeStream = true;
        #endregion
    }

    #endregion

    #region ArchiveStreamReader

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamReader
    ///
    /// <summary>
    /// Represents a stream for reading an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveStreamReader : ArchiveStreamBase, ISequentialInStream, IInStream
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamReader
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveStreamReader class
        /// with the specified stream. BaseStream is disposed when disposed.
        /// </summary>
        ///
        /// <param name="baseStream">Target stream.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveStreamReader(Stream baseStream) : this(baseStream, true) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamReader
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveStreamReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="baseStream">Target stream.</param>
        /// <param name="disposeStream">
        /// Value indicating whether to discard the BaseStream object when
        /// disposed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveStreamReader(Stream baseStream, bool disposeStream) :
            base(baseStream, disposeStream) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// Reads the data.
        /// </summary>
        ///
        /// <param name="buffer">Buffer.</param>
        /// <param name="size">Size to read.</param>
        ///
        /// <returns>Actual size read.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int Read(byte[] buffer, uint size) => BaseStream.Read(buffer, 0, (int)size);

        #endregion
    }

    #endregion

    #region ArchiveStreamWriter

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamWriter
    ///
    /// <summary>
    /// Represents a stream for writing an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveStreamWriter : ArchiveStreamBase, ISequentialOutStream, IOutStream
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamWriter
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveStreamReader class
        /// with the specified stream. BaseStream is disposed when disposed.
        /// </summary>
        ///
        /// <param name="baseStream">Target stream.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveStreamWriter(Stream baseStream) : this(baseStream, true) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamWriter
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveStreamReader class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="baseStream">Target stream.</param>
        /// <param name="disposeStream">
        /// Value indicating whether to discard the BaseStream object when
        /// disposed.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveStreamWriter(Stream baseStream, bool disposeStream) :
            base(baseStream, disposeStream) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SetSize
        ///
        /// <summary>
        /// Sets the length of the current stream.
        /// The method implements IOutStream.SetSize(long).
        /// </summary>
        ///
        /// <param name="size">Size to set.</param>
        ///
        /// <returns>Zero.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int SetSize(long size)
        {
            BaseStream.SetLength(size);
            return 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Write
        ///
        /// <summary>
        /// Writes a byte sequence to the current stream and advances the
        /// current position of the stream by the number of bytes written.
        /// The method implements IOutStream.Write(byte[], uint, IntPtr).
        /// </summary>
        ///
        /// <param name="data">data for writing</param>
        /// <param name="size">size to write.</param>
        /// <param name="result">written size.</param>
        ///
        /// <returns>Zero.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int Write(byte[] data, uint size, IntPtr result)
        {
            var count = (int)size;
            BaseStream.Write(data, 0, count);
            if (result != IntPtr.Zero) Marshal.WriteInt32(result, count);
            return 0;
        }

        #endregion
    }

    #endregion
}
