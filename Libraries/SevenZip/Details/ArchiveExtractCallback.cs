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
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveExtractCallback
    /// 
    /// <summary>
    /// 圧縮ファイルを展開する際のコールバック関数群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ArchiveExtractCallback
        : ArchiveCallbackBase, IArchiveExtractCallback, ICryptoGetTextPassword
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveExtractCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveExtractCallback(string src, long count, long size,
            Func<uint, ISequentialOutStream> destination)
        {
            ProgressReport.FileCount = count;
            ProgressReport.FileSize  = size;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 圧縮ファイルのパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 出力ストリームを取得するオブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public Func<uint, ISequentialOutStream> Destination { get; }

        #endregion

        #region Methods

        #region ICryptoGetTextPassword

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
        /// <returns>0 (ゼロ)</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            Cancel.ThrowIfCancellationRequested();

            var e = new QueryEventArgs<string, string>(Source);
            if (Password != null) Password.Request(e);
            else e.Cancel = true;

            password = !e.Cancel ? e.Result : string.Empty;
            return 0;
        }

        #endregion

        #region IArchiveExtractCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// 展開後のバイト数を通知します。
        /// </summary>
        /// 
        /// <param name="size">展開後のバイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong size)
        {
            Cancel.ThrowIfCancellationRequested();

            _hack = Math.Max((long)size - ProgressReport.FileSize, 0);
            Progress?.Report(ProgressReport);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        /// 
        /// <summary>
        /// 展開の完了したバイトサイズを通知します。
        /// </summary>
        /// 
        /// <param name="size">展開の完了したバイト数</param>
        /// 
        /// <remarks>
        /// IInArchive.Extract を複数回実行する場合、SetTotal および
        /// SetCompleted で取得できる値が Format によって異なります。
        /// 例えば、zip の場合は毎回 Extract に指定したファイルのバイト数を
        /// 表しますが、7z の場合はそれまでに Extract で展開した累積
        /// バイト数となります。ArchiveExtractCallback では Format 毎の
        /// 違いをなくすために正規化しています。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCompleted(ref ulong size)
        {
            Cancel.ThrowIfCancellationRequested();

            var cvt = Math.Max((long)size - _hack, 0);
            ProgressReport.DoneSize = cvt;
            Progress?.Report(ProgressReport);
        }

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
        /// <returns>0 (ゼロ)</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialOutStream stream, AskMode mode)
        {
            Cancel.ThrowIfCancellationRequested();

            stream = (mode == AskMode.Extract) ? Destination(index) : null;
            return 0;
        }

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
        public void PrepareOperation(AskMode mode) { }

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
        public void SetOperationResult(OperationResult result)
        {
            ProgressReport.DoneCount = 1;
            Progress?.Report(ProgressReport);
            Result = result;
        }

        #endregion

        #endregion

        #region Fields
        private long _hack = 0;
        #endregion
    }
}
