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
using System.IO;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    /// <summary>
    /// 7-zip IArchiveOpenCallback imported interface to handle the opening of an archive.
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveOpenCallback
    {
        // ref ulong replaced with IntPtr because handlers often pass null value
        // read actual value with Marshal.ReadInt64
        /// <summary>
        /// Sets total data size
        /// </summary>
        /// <param name="files">Files pointer</param>
        /// <param name="bytes">Total size in bytes</param>
        void SetTotal(
            IntPtr files,
            IntPtr bytes);

        /// <summary>
        /// Sets completed size
        /// </summary>
        /// <param name="files">Files pointer</param>
        /// <param name="bytes">Completed size in bytes</param>
        void SetCompleted(
            IntPtr files,
            IntPtr bytes);
    }

    /// <summary>
    /// 7-zip ICryptoGetTextPassword imported interface to get the archive password.
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000500100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICryptoGetTextPassword
    {
        /// <summary>
        /// Gets password for the archive
        /// </summary>
        /// <param name="password">Password for the archive</param>
        /// <returns>Zero if everything is OK</returns>
        [PreserveSig]
        int CryptoGetTextPassword(
            [MarshalAs(UnmanagedType.BStr)] out string password);
    }

    /// <summary>
    /// 7-zip ICryptoGetTextPassword2 imported interface for setting the archive password.
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000500110000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICryptoGetTextPassword2
    {
        /// <summary>
        /// Sets password for the archive
        /// </summary>
        /// <param name="passwordIsDefined">Specifies whether archive has a password or not (0 if not)</param>
        /// <param name="password">Password for the archive</param>
        /// <returns>Zero if everything is OK</returns>
        [PreserveSig]
        int CryptoGetTextPassword2(
            ref int passwordIsDefined,
            [MarshalAs(UnmanagedType.BStr)] out string password);
    }

    /// <summary>
    /// 7-zip IArchiveExtractCallback imported interface.
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600200000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveExtractCallback
    {
        /// <summary>
        /// Gives the size of the unpacked archive files
        /// </summary>
        /// <param name="total">Size of the unpacked archive files (in bytes)</param>
        void SetTotal(ulong total);

        /// <summary>
        /// SetCompleted 7-zip function
        /// </summary>
        /// <param name="completeValue"></param>
        void SetCompleted([In] ref ulong completeValue);

        /// <summary>
        /// Gets the stream for file extraction
        /// </summary>
        /// <param name="index">File index in the archive file table</param>
        /// <param name="outStream">Pointer to the stream</param>
        /// <param name="askExtractMode">Extraction mode</param>
        /// <returns>S_OK - OK, S_FALSE - skip this file</returns>
        [PreserveSig]
        int GetStream(
            uint index,
            [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialOutStream outStream,
            AskMode askExtractMode);

        /// <summary>
        /// PrepareOperation 7-zip function
        /// </summary>
        /// <param name="askExtractMode">Ask mode</param>
        void PrepareOperation(AskMode askExtractMode);

        /// <summary>
        /// Sets the operaton result
        /// </summary>
        /// <param name="operationResult">The operation result</param>
        void SetOperationResult(OperationResult operationResult);
    }

    /// <summary>
    /// 7-zip IArchiveUpdateCallback imported interface.
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600800000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveUpdateCallback
    {
        /// <summary>
        /// Gives the size of the unpacked archive files.
        /// </summary>
        /// <param name="total">Size of the unpacked archive files (in bytes)</param>
        void SetTotal(ulong total);

        /// <summary>
        /// SetCompleted 7-zip internal function.
        /// </summary>
        /// <param name="completeValue"></param>
        void SetCompleted([In] ref ulong completeValue);

        /// <summary>
        /// Gets archive update mode.
        /// </summary>
        /// <param name="index">File index</param>
        /// <param name="newData">1 if new, 0 if not</param>
        /// <param name="newProperties">1 if new, 0 if not</param>
        /// <param name="indexInArchive">-1 if doesn't matter</param>
        /// <returns></returns>
        [PreserveSig]
        int GetUpdateItemInfo(
            uint index, ref int newData,
            ref int newProperties, ref uint indexInArchive);

        /// <summary>
        /// Gets the archive item property data.
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="propId">Property identificator</param>
        /// <param name="value">Property value</param>
        /// <returns>Zero if Ok</returns>
        [PreserveSig]
        int GetProperty(uint index, ItemPropId propId, ref PropVariant value);

        /// <summary>
        /// Gets the stream for reading.
        /// </summary>
        /// <param name="index">The item index.</param>
        /// <param name="inStream">The ISequentialInStream pointer for reading.</param>
        /// <returns>Zero if Ok</returns>
        [PreserveSig]
        int GetStream(
            uint index,
            [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialInStream inStream);

        /// <summary>
        /// Sets the result for currently performed operation.
        /// </summary>
        /// <param name="operationResult">The result value.</param>
        void SetOperationResult(OperationResult operationResult);

        /// <summary>
        /// EnumProperties 7-zip internal function.
        /// </summary>
        /// <param name="enumerator">The enumerator pointer.</param>
        /// <returns></returns>
        long EnumProperties(IntPtr enumerator);
    }

    /// <summary>
    /// 7-zip IArchiveOpenVolumeCallback imported interface to handle archive volumes.
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600300000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveOpenVolumeCallback
    {
        /// <summary>
        /// Gets the archive property data.
        /// </summary>
        /// <param name="propId">The property identificator.</param>
        /// <param name="value">The property value.</param>
        [PreserveSig]
        int GetProperty(
            ItemPropId propId, ref PropVariant value);

        /// <summary>
        /// Gets the stream for reading the volume.
        /// </summary>
        /// <param name="name">The volume file name.</param>
        /// <param name="inStream">The IInStream pointer for reading.</param>
        /// <returns>Zero if Ok</returns>
        [PreserveSig]
        int GetStream(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            [Out, MarshalAs(UnmanagedType.Interface)] out IInStream inStream);
    }

    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600400000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInArchiveGetStream
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        ISequentialInStream GetStream(uint index);
    }

    /// <summary>
    /// 7-zip ISequentialInStream imported interface
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300010000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialInStream
    {
        /// <summary>
        /// Writes data to 7-zip packer
        /// </summary>
        /// <param name="data">Array of bytes available for writing</param>
        /// <param name="size">Array size</param>
        /// <returns>S_OK if success</returns>
        /// <remarks>If (size > 0) and there are bytes in stream, 
        /// this function must read at least 1 byte.
        /// This function is allowed to read less than "size" bytes.
        /// You must call Read function in loop, if you need exact amount of data.
        /// </remarks>
        int Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size);
    }

    /// <summary>
    /// 7-zip ISequentialOutStream imported interface
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300020000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialOutStream
    {
        /// <summary>
        /// Writes data to unpacked file stream
        /// </summary>
        /// <param name="data">Array of bytes available for reading</param>
        /// <param name="size">Array size</param>
        /// <param name="processedSize">Processed data size</param>
        /// <returns>S_OK if success</returns>
        /// <remarks>If size != 0, return value is S_OK and (*processedSize == 0),
        ///  then there are no more bytes in stream.
        /// If (size > 0) and there are bytes in stream, 
        /// this function must read at least 1 byte.
        /// This function is allowed to rwrite less than "size" bytes.
        /// You must call Write function in loop, if you need exact amount of data.
        /// </remarks>
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size, IntPtr processedSize);
    }

    /// <summary>
    /// 7-zip IInStream imported interface
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300030000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInStream
    {
        /// <summary>
        /// Read routine
        /// </summary>
        /// <param name="data">Array of bytes to set</param>
        /// <param name="size">Array size</param>
        /// <returns>Zero if Ok</returns>
        int Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
        );

        /// <summary>
        /// Seek routine
        /// </summary>
        /// <param name="offset">Offset value</param>
        /// <param name="origin">Seek origin value</param>
        /// <param name="newPosition">New position pointer</param>
        void Seek(long offset, SeekOrigin origin, IntPtr newPosition);
    }

    /// <summary>
    /// 7-zip IOutStream imported interface
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300040000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOutStream
    {
        /// <summary>
        /// Write routine
        /// </summary>
        /// <param name="data">Array of bytes to get</param>
        /// <param name="size">Array size</param>
        /// <param name="processedSize">Processed size</param>
        /// <returns>Zero if Ok</returns>
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize);

        /// <summary>
        /// Seek routine
        /// </summary>
        /// <param name="offset">Offset value</param>
        /// <param name="seekOrigin">Seek origin value</param>
        /// <param name="newPosition">New position pointer</param>       
        void Seek(
            long offset, SeekOrigin seekOrigin, IntPtr newPosition);

        /// <summary>
        /// Set size routine
        /// </summary>
        /// <param name="newSize">New size value</param>
        /// <returns>Zero if Ok</returns>
        [PreserveSig]
        int SetSize(long newSize);
    }

    /// <summary>
    /// 7-zip essential in archive interface
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600600000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInArchive
    {
        /// <summary>
        /// Opens archive for reading.
        /// </summary>
        /// <param name="stream">Archive file stream</param>
        /// <param name="maxCheckStartPosition">Maximum start position for checking</param>
        /// <param name="openArchiveCallback">Callback for opening archive</param>
        /// <returns></returns>
        [PreserveSig]
        int Open(
            IInStream stream,
            [In] ref ulong maxCheckStartPosition,
            [MarshalAs(UnmanagedType.Interface)] IArchiveOpenCallback openArchiveCallback);

        /// <summary>
        /// Closes the archive.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets the number of files in the archive file table  .          
        /// </summary>
        /// <returns>The number of files in the archive</returns>
        uint GetNumberOfItems();

        /// <summary>
        /// Retrieves specific property data.
        /// </summary>
        /// <param name="index">File index in the archive file table</param>
        /// <param name="propId">Property code</param>
        /// <param name="value">Property variant value</param>
        void GetProperty(
            uint index,
            ItemPropId propId,
            ref PropVariant value); // PropVariant

        /// <summary>
        /// Extracts files from the opened archive.
        /// </summary>
        /// <param name="indexes">indexes of files to be extracted (must be sorted)</param>
        /// <param name="numItems">0xFFFFFFFF means all files</param>
        /// <param name="testMode">testMode != 0 means "test files operation"</param>
        /// <param name="extractCallback">IArchiveExtractCallback for operations handling</param>
        /// <returns>0 if success</returns>
        [PreserveSig]
        int Extract(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] indexes,
            uint numItems,
            int testMode,
            [MarshalAs(UnmanagedType.Interface)] IArchiveExtractCallback extractCallback);

        /// <summary>
        /// Gets archive property data
        /// </summary>
        /// <param name="propId">Archive property identificator</param>
        /// <param name="value">Archive property value</param>
        void GetArchiveProperty(
            ItemPropId propId, // PROPID
            ref PropVariant value); // PropVariant

        /// <summary>
        /// Gets the number of properties
        /// </summary>
        /// <returns>The number of properties</returns>
        uint GetNumberOfProperties();

        /// <summary>
        /// Gets property information
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="name">Name</param>
        /// <param name="propId">Property identificator</param>
        /// <param name="varType">Variant type</param>
        void GetPropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropId propId, // PROPID
            out ushort varType); //VARTYPE

        /// <summary>
        /// Gets the number of archive properties
        /// </summary>
        /// <returns>The number of archive properties</returns>
        uint GetNumberOfArchiveProperties();

        /// <summary>
        /// Gets the archive property information
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="name">Name</param>
        /// <param name="propId">Property identificator</param>
        /// <param name="varType">Variant type</param>
        void GetArchivePropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropId propId, // PROPID
            out ushort varType); //VARTYPE
    }

    /// <summary>
    /// 7-zip essential out archive interface
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600A00000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOutArchive
    {
        /// <summary>
        /// Updates archive items
        /// </summary>
        /// <param name="outStream">The ISequentialOutStream pointer for writing the archive data</param>
        /// <param name="numItems">Number of archive items</param>
        /// <param name="updateCallback">The IArchiveUpdateCallback pointer</param>
        /// <returns>Zero if Ok</returns>
        [PreserveSig]
        int UpdateItems(
            [MarshalAs(UnmanagedType.Interface)] ISequentialOutStream outStream,
            uint numItems,
            [MarshalAs(UnmanagedType.Interface)] IArchiveUpdateCallback updateCallback);

        /// <summary>
        /// Gets file time type(?)
        /// </summary>
        /// <param name="type">Type pointer</param>
        void GetFileTimeType(IntPtr type);
    }

    /// <summary>
    /// 7-zip ISetProperties interface for setting various archive properties
    /// </summary>
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600030000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISetProperties
    {
        /// <summary>
        /// Sets the archive properties
        /// </summary>
        /// <param name="names">The names of the properties</param>
        /// <param name="values">The values of the properties</param>
        /// <param name="numProperties">The properties count</param>
        /// <returns></returns>        
        int SetProperties(IntPtr names, IntPtr values, int numProperties);
    }
}
