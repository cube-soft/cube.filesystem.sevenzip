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
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamBase
    ///
    /// <summary>
    /// 圧縮ファイルを扱うストリームの基底クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveStreamBase : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamBase
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="baseStream">
        /// ラップ対象となる Stream オブジェクト
        /// </param>
        ///
        /// <param name="disposeStream">
        /// Dispose 時に BaseStream オブジェクトも破棄するかどうかを示す値
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveStreamBase(Stream baseStream, bool disposeStream)
        {
            _dispose = new OnceAction<bool>(Dispose);
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
        /// ラップ対象となる Stream オブジェクトを取得します。
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
        /// ストリームの位置を設定します。
        /// </summary>
        ///
        /// <param name="offset">起点からのオフセット値</param>
        /// <param name="origin">起点となる位置</param>
        /// <param name="result">設定後の位置</param>
        ///
        /// <remarks>
        /// IInStream.Seek(long, SeekOrigin, IntPtr) の実装です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Seek(long offset, SeekOrigin origin, IntPtr result)
        {
            var pos = BaseStream.Seek(offset, origin);
            if (result != IntPtr.Zero) Marshal.WriteInt64(result, pos);
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveStreamBase
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ArchiveStreamBase() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _disposeStream) BaseStream.Dispose();
        }

        #endregion

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly bool _disposeStream = true;
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamReader
    ///
    /// <summary>
    /// 圧縮ファイルの読み込み用ストリームを表すクラスです。
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
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="baseStream">
        /// ラップ対象となる Stream オブジェクト
        /// </param>
        ///
        /// <remarks>
        /// ArchiveStreamReader.Dispose() 実行時に BaseStream.Dispose() が
        /// 実行されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveStreamReader(Stream baseStream) : this(baseStream, true) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="baseStream">
        /// ラップ対象となる Stream オブジェクト
        /// </param>
        ///
        /// <param name="disposeStream">
        /// Dispose 時に BaseStream オブジェクトも破棄するかどうかを示す値
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
        /// データを読み込みます。
        /// </summary>
        ///
        /// <param name="buffer">バッファ</param>
        /// <param name="size">読み込むサイズ</param>
        ///
        /// <returns>実際に読み込まれたサイズ</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int Read(byte[] buffer, uint size) => BaseStream.Read(buffer, 0, (int)size);

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamWriter
    ///
    /// <summary>
    /// 圧縮ファイルの書き込み用ストリームを表すクラスです。
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
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="baseStream">
        /// ラップ対象となる Stream オブジェクト
        /// </param>
        ///
        /// <remarks>
        /// ArchiveStreamWriter.Dispose() 実行時に BaseStream.Dispose() が
        /// 実行されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveStreamWriter(Stream baseStream) : this(baseStream, true) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveStreamWriter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="baseStream">
        /// ラップ対象となる Stream オブジェクト
        /// </param>
        ///
        /// <param name="disposeStream">
        /// Dispose 時に BaseStream オブジェクトも破棄するかどうかを示す値
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
        /// 現在のストリームの長さを設定します。
        /// </summary>
        ///
        /// <param name="size">設定サイズ</param>
        ///
        /// <returns>0 (ゼロ)</returns>
        ///
        /// <remarks>
        /// IOutStream.SetSize(long) の実装です。
        /// </remarks>
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
        /// 現在のストリームにバイトシーケンスを書き込み、書き込んだ
        /// バイト数だけストリームの現在位置を進めます。
        /// </summary>
        ///
        /// <param name="data">書き込み用データ</param>
        /// <param name="size">書き込むバイト数</param>
        /// <param name="result">実際に書き込まれたバイト数</param>
        ///
        /// <returns>0 (ゼロ)</returns>
        ///
        /// <remarks>
        /// IOutStream.Write(byte[], uint, IntPtr) の実装です。
        /// </remarks>
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
}
