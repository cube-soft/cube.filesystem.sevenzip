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
    internal sealed class ArchiveOpenCallback
        : ArchiveCallbackBase, IArchiveOpenCallback, ICryptoGetTextPassword
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
        /* ----------------------------------------------------------------- */
        public ArchiveOpenCallback(string src)
        {
            Source = src;
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
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            if (Password != null)
            {
                var e = new QueryEventArgs<string, string>(Source);
                Password.Request(e);
                var valid = !e.Cancel && !string.IsNullOrEmpty(e.Result);
                password = valid ? e.Result : string.Empty;
                Result = e.Cancel ? OperationResult.UserCancel :
                         valid    ? OperationResult.OK :
                                    OperationResult.WrongPassword;
            }
            else
            {
                password = string.Empty;
                Result = OperationResult.WrongPassword;
            }
            return (int)Result;
        }

        #endregion

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
            if (count != IntPtr.Zero) ProgressReport.TotalCount = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) ProgressReport.TotalBytes = Marshal.ReadInt64(bytes);
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
            if (count != IntPtr.Zero) ProgressReport.Count = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) ProgressReport.Bytes = Marshal.ReadInt64(bytes);
            Progress?.Report(ProgressReport);
            Result = OperationResult.OK;
        }

        #endregion

        #endregion
    }
}
