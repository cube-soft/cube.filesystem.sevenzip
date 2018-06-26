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
        /// <param name="src">Information オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(Information src) : this(src, src.Name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">Information オブジェクト</param>
        /// <param name="pathInArchive">圧縮ファイル中の相対パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(Information src, string pathInArchive) :
            base(src.Source, src.Refreshable)
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

    /* --------------------------------------------------------------------- */
    ///
    /// FileItem
    ///
    /// <summary>
    /// FileItem の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FileItemExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToFileItem
        ///
        /// <summary>
        /// FileItem オブジェクトに変換します。
        /// </summary>
        ///
        /// <param name="src">Information オブジェクト</param>
        ///
        /// <returns>FileItem オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static FileItem ToFileItem(this Information src) =>
            new FileItem(src);

        /* ----------------------------------------------------------------- */
        ///
        /// ToFileItem
        ///
        /// <summary>
        /// FileItem オブジェクトに変換します。
        /// </summary>
        ///
        /// <param name="src">Information オブジェクト</param>
        /// <param name="pathInArchive">圧縮ファイル中の相対パス</param>
        ///
        /// <returns>FileItem オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static FileItem ToFileItem(this Information src, string pathInArchive) =>
            new FileItem(src, pathInArchive);

        #endregion
    }
}
