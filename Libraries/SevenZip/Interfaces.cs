/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000000050000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IProgress
    {
        void SetTotal(ulong total);

        void SetCompleted([In] ref ulong completeValue);
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IArchiveOpenCallback
    {
        void SetTotal(
            IntPtr files,  // [In] ref ulong files
            IntPtr bytes   // [In] ref ulong bytes
        );

        void SetCompleted(
            IntPtr files,  // [In] ref ulong files
            IntPtr bytes   // [In] ref ulong bytes
        );
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000500100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICryptoGetTextPassword
    {
        [PreserveSig]
        int CryptoGetTextPassword([MarshalAs(UnmanagedType.BStr)] out string password);

        //[return : MarshalAs(UnmanagedType.BStr)]
        //string CryptoGetTextPassword();
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600200000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IArchiveExtractCallback //: IProgress
    {
        void SetTotal(ulong total);

        void SetCompleted([In] ref ulong completeValue);

        [PreserveSig]
        int GetStream(
            uint index,
            [MarshalAs(UnmanagedType.Interface)] out ISequentialOutStream outStream,
            Mode extractMode
        );

        void PrepareOperation(Mode extractMode);

        void SetOperationResult(Result result);
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600300000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IArchiveOpenVolumeCallback
    {
        void GetProperty(ItemPropertyId pid, IntPtr value);

        [PreserveSig]
        int GetStream(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            [MarshalAs(UnmanagedType.Interface)] out IInStream inStream
        );
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600400000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInArchiveGetStream
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        ISequentialInStream GetStream(uint index);
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300010000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISequentialInStream
    {
        uint Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
        );

        //[PreserveSig]
        //int Read(
        //    [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
        //    uint size,
        //    IntPtr processedSize // ref uint processedSize
        //);

        /*
        Out: if size != 0, return_value = S_OK and (*processedSize == 0),
          then there are no more bytes in stream.
        if (size > 0) && there are bytes in stream, 
        this function must read at least 1 byte.
        This function is allowed to read less than number of remaining bytes in stream.
        You must call Read function in loop, if you need exact amount of data
        */
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300020000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISequentialOutStream
    {
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize // ref uint processedSize
        );

        /*
        if (size > 0) this function must write at least 1 byte.
        This function is allowed to write less than "size".
        You must call Write function in loop, if you need to write exact amount of data
        */
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300030000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInStream //: ISequentialInStream
    {
        uint Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
        );

        //[PreserveSig]
        //int Read(
        //    [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
        //    uint size,
        //    IntPtr processedSize // ref uint processedSize
        //);

        //[PreserveSig]
        void Seek(
            long offset,
            uint seekOrigin,
            IntPtr newPosition // ref long newPosition
        );
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300040000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOutStream //: ISequentialOutStream
    {
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize // ref uint processedSize
        );

        //[PreserveSig]
        void Seek(
            long offset,
            uint seekOrigin,
            IntPtr newPosition // ref long newPosition
        );

        [PreserveSig]
        int SetSize(long newSize);
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600600000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    //[AutomationProxy(true)]
    public interface IInArchive
    {
        [PreserveSig]
        int Open(
            IInStream stream,
            /*[MarshalAs(UnmanagedType.U8)]*/ [In] ref ulong maxCheckStartPosition,
            [MarshalAs(UnmanagedType.Interface)] IArchiveOpenCallback openArchiveCallback
        );

        void Close();

        //void GetNumberOfItems([In] ref uint numItem);
        uint GetNumberOfItems();

        void GetProperty(
            uint index,
            ItemPropertyId pid,   // PROPID
            ref PropVariant value // PROPVARIANT
        );

        // indices must be sorted 
        // numItems = 0xFFFFFFFF means all files
        // testMode != 0 means "test files operation"
        [PreserveSig]
        int Extract(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] indices, //[In] ref uint indices,
            uint numItems,
            int testMode,
            [MarshalAs(UnmanagedType.Interface)] IArchiveExtractCallback extractCallback
        );

        void GetArchiveProperty(
            uint pid,             // PROPID
            ref PropVariant value // PROPVARIANT
        );

        //void GetNumberOfProperties([In] ref uint numProperties);
        uint GetNumberOfProperties();

        void GetPropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropertyId pid, // PROPID
            out ushort varType      //VARTYPE
        );

        //void GetNumberOfArchiveProperties([In] ref uint numProperties);
        uint GetNumberOfArchiveProperties();

        void GetArchivePropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] string name,
            ref uint pid,      // PROPID
            ref ushort varType //VARTYPE
        );
    }
}
