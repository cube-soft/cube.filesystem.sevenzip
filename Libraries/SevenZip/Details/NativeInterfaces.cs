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
    /* --------------------------------------------------------------------- */
    ///
    /// ICryptoGetTextPassword
    /// 
    /// <summary>
    /// 圧縮ファイル解凍時にパスワードを伝えるためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000500100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICryptoGetTextPassword
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword
        /// 
        /// <summary>
        /// 圧縮ファイルのパスワードを取得します。
        /// </summary>
        /// 
        /// <param name="password">パスワード</param>
        /// 
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int CryptoGetTextPassword([MarshalAs(UnmanagedType.BStr)] out string password);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ICryptoGetTextPassword2
    /// 
    /// <summary>
    /// 圧縮時にパスワードを伝えるためのコールバック関数を定義した
    /// インターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000500110000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICryptoGetTextPassword2
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword2
        ///
        /// <summary>
        /// 圧縮ファイルのパスワードを取得します。
        /// </summary>
        /// 
        /// <param name="enable">
        /// パスワードを設定するかどうかを示す値 (0 if not)
        /// </param>
        /// <param name="password">パスワード</param>
        /// 
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int CryptoGetTextPassword2(
            ref int enable,
            [MarshalAs(UnmanagedType.BStr)] out string password
        );
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveOpenCallback
    /// 
    /// <summary>
    /// 圧縮ファイルを展開する際のコールバック関数を定義した
    /// インターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveOpenCallback
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// 圧縮ファイルの展開時の合計サイズを取得します。
        /// </summary>
        /// 
        /// <param name="files">ファイル数</param>
        /// <param name="bytes">バイト数</param>
        ///
        /// <remarks>
        /// 7z.dll で null が設定される事が多いため、ref ulong の代わりに
        /// IntPtr を使用しています。非 null 時に値を取得する場合、
        /// Marshal.ReadInt64 を使用して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        void SetTotal(IntPtr /* ref ulong */ files, IntPtr /* ref ulong */ bytes);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// 展開の完了したサイズを取得します。
        /// </summary>
        /// 
        /// <param name="files">ファイル数</param>
        /// <param name="bytes">バイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetCompleted(IntPtr /* ref ulong */ files, IntPtr /* ref ulong */ bytes);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveOpenVolumeCallback
    /// 
    /// <summary>
    /// 分割された圧縮ファイルを展開するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600300000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveOpenVolumeCallback
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// Gets the archive property data.
        /// </summary>
        /// 
        /// <param name="pid">The property identificator.</param>
        /// <param name="value">The property value.</param>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetProperty(ItemPropId pid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// Gets the stream for reading the volume.
        /// </summary>
        /// 
        /// <param name="name">The volume file name.</param>
        /// <param name="stream">Stream pointer for reading.</param>
        /// 
        /// <returns>Zero if Ok</returns>
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetStream(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            [Out, MarshalAs(UnmanagedType.Interface)] out IInStream stream
        );
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveExtractCallback
    /// 
    /// <summary>
    /// 圧縮ファイル中の各ファイルを展開するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600200000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveExtractCallback
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// 展開後のバイト数を通知します。
        /// </summary>
        /// 
        /// <param name="size">バイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetTotal(ulong total);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        /// 
        /// <summary>
        /// 展開の完了したバイト数を通知します。
        /// </summary>
        /// 
        /// <param name="value">バイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetCompleted(ref ulong value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        /// 
        /// <summary>
        /// 展開した内容を保存するためのストリームを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="stream">出力ストリーム</param>
        /// <param name="mode">展開モード</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /// <remarks>
        /// 展開をスキップする場合、OperationResult.OK 以外の値を返します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetStream(
            uint index,
            [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialOutStream stream,
            AskMode mode
        );

        /* ----------------------------------------------------------------- */
        ///
        /// PrepareOperation
        /// 
        /// <summary>
        /// 展開処理の直前に実行されます。
        /// </summary>
        /// 
        /// <param name="mode">展開モード</param>
        ///
        /* ----------------------------------------------------------------- */
        void PrepareOperation(AskMode mode);

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// 処理結果を通知します。
        /// </summary>
        /// 
        /// <param name="result">処理結果</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetOperationResult(OperationResult result);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveUpdateCallback
    /// 
    /// <summary>
    /// 圧縮ファイル中のファイル内容を更新するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600800000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveUpdateCallback
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// 圧縮するファイルの合計バイト数を通知します。
        /// </summary>
        /// 
        /// <param name="size">バイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetTotal(ulong size);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        /// 
        /// <summary>
        /// 圧縮処理の終了したバイト数を通知します。
        /// </summary>
        /// 
        /// <param name="size">バイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetCompleted(ref ulong size);

        /* ----------------------------------------------------------------- */
        ///
        /// GetUpdateItemInfo
        ///
        /// <summary>
        /// 追加する項目に関する情報を取得します。
        /// </summary>
        /// 
        /// <param name="index">インデックス</param>
        /// <param name="newdata">1 if new, 0 if not</param>
        /// <param name="newprop">1 if new, 0 if not</param>
        /// <param name="indexInArchive">-1 if doesn't matter</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetUpdateItemInfo(
            uint index,
            ref int newdata,
            ref int newprop,
            ref uint indexInArchive
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        /// 
        /// <summary>
        /// 各種プロパティを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="pid">プロパティの種類</param>
        /// <param name="value">プロパティの内容</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetProperty(uint index, ItemPropId pid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        /// 
        /// <summary>
        /// ストリームを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="stream">読み込み用ストリーム</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetStream(
            uint index,
            [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialInStream stream
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        /// 
        /// <summary>
        /// 処理結果を設定します。
        /// </summary>
        /// 
        /// <param name="result">処理結果</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetOperationResult(OperationResult result);

        /* ----------------------------------------------------------------- */
        ///
        /// EnumProperties
        ///
        /// <summary>
        /// EnumProperties 7-zip internal function.
        /// </summary>
        /// 
        /// <param name="enumerator">The enumerator pointer.</param>
        /// 
        /* ----------------------------------------------------------------- */
        long EnumProperties(IntPtr enumerator);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ISequentialInStream
    /// 
    /// <summary>
    /// 圧縮ファイルの入力ストリームを処理するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300010000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialInStream
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// Writes data to 7-zip packer
        /// </summary>
        /// 
        /// <param name="data">Array of bytes available for writing</param>
        /// <param name="size">Array size</param>
        /// 
        /// <returns>S_OK if success</returns>
        /// 
        /// <remarks>
        /// If (size > 0) and there are bytes in stream, 
        /// this function must read at least 1 byte.
        /// This function is allowed to read less than "size" bytes.
        /// You must call Read function in loop, if you need exact
        /// amount of data.
        /// </remarks>
        /* ----------------------------------------------------------------- */
        int Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
        );
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IInStream
    /// 
    /// <summary>
    /// 圧縮ファイルの入力ストリームを処理するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300030000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInStream
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// Read routine
        /// </summary>
        /// 
        /// <param name="data">Array of bytes to set</param>
        /// <param name="size">Array size</param>
        /// 
        /// <returns>Zero if Ok</returns>
        /// 
        /* ----------------------------------------------------------------- */
        int Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Seek
        /// 
        /// <summary>
        /// Seek routine
        /// </summary>
        /// 
        /// <param name="offset">Offset value</param>
        /// <param name="origin">Seek origin value</param>
        /// <param name="newpos">New position pointer</param>
        /// 
        /* ----------------------------------------------------------------- */
        void Seek(long offset, SeekOrigin origin, IntPtr newpos);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ISequentialOutStream
    /// 
    /// <summary>
    /// 圧縮ファイルの出力ストリームを処理するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300020000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialOutStream
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Write
        /// 
        /// <summary>
        /// Writes data to unpacked file stream
        /// </summary>
        /// 
        /// <param name="data">Array of bytes available for reading</param>
        /// <param name="size">Array size</param>
        /// <param name="processedSize">Processed data size</param>
        /// 
        /// <returns>S_OK if success</returns>
        /// 
        /// <remarks>
        /// If size != 0, return value is S_OK and (*processedSize == 0),
        /// then there are no more bytes in stream.
        /// If (size > 0) and there are bytes in stream, 
        /// this function must read at least 1 byte.
        /// This function is allowed to rwrite less than "size" bytes.
        /// You must call Write function in loop, if you need exact
        /// amount of data.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize
        );
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IOutStream
    /// 
    /// <summary>
    /// 圧縮ファイルの出力ストリームを処理するためのコールバック関数を
    /// 定義したインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300040000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOutStream
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Write
        /// 
        /// <summary>
        /// Write routine
        /// </summary>
        /// 
        /// <param name="data">Array of bytes to get</param>
        /// <param name="size">Array size</param>
        /// <param name="processedSize">Processed size</param>
        /// 
        /// <returns>Zero if Ok</returns>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Seek
        /// 
        /// <summary>
        /// Seek routine
        /// </summary>
        /// 
        /// <param name="offset">Offset value</param>
        /// <param name="origin">Seek origin value</param>
        /// <param name="newpos">New position pointer</param>
        /// 
        /* ----------------------------------------------------------------- */
        void Seek(long offset, SeekOrigin origin, IntPtr newpos);

        /* ----------------------------------------------------------------- */
        ///
        /// SetSize
        /// 
        /// <summary>
        /// Set size routine
        /// </summary>
        /// 
        /// <param name="size">New size value</param>
        /// 
        /// <returns>Zero if Ok</returns>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int SetSize(long size);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IInArchive
    /// 
    /// <summary>
    /// 既存の圧縮ファイルを処理するためのコールバック関数を定義した
    /// インターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600600000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInArchive
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// Opens archive for reading.
        /// </summary>
        /// 
        /// <param name="stream">Archive file stream</param>
        /// <param name="checkpos">Maximum start position for checking</param>
        /// <param name="callback">Callback for opening archive</param>
        /// 
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Open(
            IInStream stream,
            [In] ref ulong checkpos,
            [MarshalAs(UnmanagedType.Interface)] IArchiveOpenCallback callback
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        /// 
        /// <summary>
        /// Closes the archive.
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        void Close();

        /* ----------------------------------------------------------------- */
        ///
        /// GetNumberOfItems
        /// 
        /// <summary>
        /// Gets the number of files in the archive file table  .          
        /// </summary>
        /// 
        /// <returns>The number of files in the archive</returns>
        /// 
        /* ----------------------------------------------------------------- */
        uint GetNumberOfItems();

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        /// 
        /// <summary>
        /// Retrieves specific property data.
        /// </summary>
        /// 
        /// <param name="index">File index in the archive file table</param>
        /// <param name="pid">Property ID</param>
        /// <param name="value">Property variant value</param>
        /// 
        /* ----------------------------------------------------------------- */
        void GetProperty(uint index, ItemPropId pid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// Extracts files from the opened archive.
        /// </summary>
        /// 
        /// <param name="indexes">
        /// indexes of files to be extracted (must be sorted)
        /// </param>
        /// <param name="count">0xFFFFFFFF means all files</param>
        /// <param name="test">test != 0 means "test files operation"</param>
        /// <param name="callback">Callback for operations handling</param>
        /// 
        /// <returns>0 if success</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Extract(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] indexes,
            uint count,
            int test,
            [MarshalAs(UnmanagedType.Interface)] IArchiveExtractCallback callback
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetArchiveProperty
        /// 
        /// <summary>
        /// Gets archive property data
        /// </summary>
        /// 
        /// <param name="aid">Archive property ID</param>
        /// <param name="value">Property value</param>
        /// 
        /* ----------------------------------------------------------------- */
        void GetArchiveProperty(ArchivePropId aid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetNumberOfProperties
        /// 
        /// <summary>
        /// Gets the number of properties
        /// </summary>
        /// 
        /// <returns>The number of properties</returns>
        /// 
        /* ----------------------------------------------------------------- */
        uint GetNumberOfProperties();

        /* ----------------------------------------------------------------- */
        ///
        /// GetPropertyInfo
        /// 
        /// <summary>
        /// Gets property information
        /// </summary>
        /// 
        /// <param name="index">Item index</param>
        /// <param name="name">Name</param>
        /// <param name="pid">Property identificator</param>
        /// <param name="type">Variant type</param>
        /// 
        /* ----------------------------------------------------------------- */
        void GetPropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropId pid,
            out ushort type
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetNumberOfArchiveProperties
        /// 
        /// <summary>
        /// Gets the number of archive properties
        /// </summary>
        /// 
        /// <returns>The number of archive properties</returns>
        /// 
        /* ----------------------------------------------------------------- */
        uint GetNumberOfArchiveProperties();

        /* ----------------------------------------------------------------- */
        ///
        /// GetArchivePropertyInfo
        /// 
        /// <summary>
        /// Gets the archive property information
        /// </summary>
        /// 
        /// <param name="index">Item index</param>
        /// <param name="name">Name</param>
        /// <param name="pid">Property identificator</param>
        /// <param name="type">Variant type</param>
        /// 
        /* ----------------------------------------------------------------- */
        void GetArchivePropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropId pid,
            out ushort type
        );
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IOutArchive
    /// 
    /// <summary>
    /// 圧縮ファイルを生成するためのコールバック関数を定義した
    /// インターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600A00000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOutArchive
    {
        /* ----------------------------------------------------------------- */
        ///
        /// UpdateItems
        /// 
        /// <summary>
        /// Updates archive items
        /// </summary>
        /// 
        /// <param name="stream">
        /// Stream pointer for writing the archive data
        /// </param>
        /// <param name="count">Number of archive items</param>
        /// <param name="callback">The Callback pointer</param>
        /// 
        /// <returns>Zero if Ok</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int UpdateItems(
            [MarshalAs(UnmanagedType.Interface)] ISequentialOutStream stream,
            uint count,
            [MarshalAs(UnmanagedType.Interface)] IArchiveUpdateCallback callback
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileTimeType
        /// 
        /// <summary>
        /// Gets file time type(?)
        /// </summary>
        /// 
        /// <param name="type">Type pointer</param>
        /// 
        /* ----------------------------------------------------------------- */
        void GetFileTimeType(IntPtr type);
    }
}
