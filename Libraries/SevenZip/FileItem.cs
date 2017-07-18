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
    public class FileItem : IArchiveItem
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
        /// <param name="info">ファイルまたはディレクトリ情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(FileSystemInfo info)
            : this(info, info.Name) { }

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
        public FileItem(FileSystemInfo info, string pathInArchive)
        {
            _info = info;
            Path = pathInArchive;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// 圧縮ファイル中の相対パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        ///
        /// <summary>
        /// 絶対パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FullName => _info.FullName;

        /* ----------------------------------------------------------------- */
        ///
        /// IsDirectory
        ///
        /// <summary>
        /// ディレクトリかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsDirectory => _info is DirectoryInfo;

        /* ----------------------------------------------------------------- */
        ///
        /// Extension
        ///
        /// <summary>
        /// 拡張子部分を表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Extension => _info.Extension;

        /* ----------------------------------------------------------------- */
        ///
        /// Attributes
        ///
        /// <summary>
        /// 属性を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Attributes => (uint)_info.Attributes;

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        ///
        /// <summary>
        /// 展開後のファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Size
        {
            get
            {
                if (_info is FileInfo file) return file.Length;
                else return 0;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreationTime
        ///
        /// <summary>
        /// 作成日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime CreationTime => _info.CreationTime;

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// 最終更新日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime => _info.LastWriteTime;

        /* ----------------------------------------------------------------- */
        ///
        /// LastAccessTime
        ///
        /// <summary>
        /// 最終アクセス日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastAccessTime => _info.LastAccessTime;

        #endregion

        #region Fields
        private FileSystemInfo _info;
        #endregion
    }
}
