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
    internal sealed class ArchiveOpenCallback : IArchiveOpenCallback, ICryptoGetTextPassword
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
        public ArchiveOpenCallback(string password)
        {
            Password = password;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// 圧縮ファイルのパスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; }

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
            password = Password;
            return 0;
        }

        #endregion

        #region IArchiveOpenCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// Sets total data size
        /// </summary>
        /// 
        /// <param name="files">Files pointer</param>
        /// <param name="bytes">Total size in bytes</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetTotal(IntPtr files, IntPtr bytes) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// Sets completed size
        /// </summary>
        /// 
        /// <param name="files">Files pointer</param>
        /// <param name="bytes">Completed size in bytes</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCompleted(IntPtr files, IntPtr bytes) { }

        #endregion

        #endregion
    }
}
