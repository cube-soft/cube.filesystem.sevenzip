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
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOpenCallback
    /// 
    /// <summary>
    /// 圧縮ファイルを開く際のコールバック関数群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ArchiveOpenCallback : ArchivePasswordCallback, IArchiveOpenCallback
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveOpenCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="io">入出力用のオブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveOpenCallback(string src, Operator io) : base(src, io) { }

        #endregion

        #region Methods

        #region IArchiveOpenCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// 圧縮ファイルの展開時の合計サイズを取得します。
        /// </summary>
        /// 
        /// <param name="count">ファイル数</param>
        /// <param name="bytes">バイト数</param>
        ///
        /// <remarks>
        /// 7z.dll で null が設定される事が多いため、ref ulong の代わりに
        /// IntPtr を使用しています。非 null 時に値を取得する場合、
        /// Marshal.ReadInt64 を使用して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void SetTotal(IntPtr count, IntPtr bytes)
        {
            if (count != IntPtr.Zero) ArchiveReport.TotalCount = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) ArchiveReport.TotalBytes = Marshal.ReadInt64(bytes);

            Report();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// ストリームの読み込み準備が完了したサイズを取得します。
        /// </summary>
        /// 
        /// <param name="count">ファイル数</param>
        /// <param name="bytes">バイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCompleted(IntPtr count, IntPtr bytes)
        {
            if (count != IntPtr.Zero) ArchiveReport.Count = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) ArchiveReport.Bytes = Marshal.ReadInt64(bytes);

            Report();
            Result = OperationResult.OK;
        }

        #endregion

        #endregion
    }
}
