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

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileItem
    /// 
    /// <summary>
    /// 圧縮予定ファイルの 1 項目を表すインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileItem : Information
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">ファイルまたはディレクトリのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(string path) : base(path)
        {
            PathInArchive = Name;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">ファイルまたはディレクトリのパス</param>
        /// <param name="pathInArchive">圧縮ファイル中の相対パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(string path, string pathInArchive) : base(path)
        {
            PathInArchive = pathInArchive;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="info">ファイルまたはディレクトリ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(FileSystemInfo info) : this(info, info.Name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="info">ファイルまたはディレクトリ情報</param>
        /// <param name="pathInArchive">圧縮ファイル中の相対パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(FileSystemInfo info, string pathInArchive) : base(info)
        {
            PathInArchive = pathInArchive;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PathInArchive
        ///
        /// <summary>
        /// 圧縮ファイル中の相対パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string PathInArchive { get; }

        #endregion
    }
}
