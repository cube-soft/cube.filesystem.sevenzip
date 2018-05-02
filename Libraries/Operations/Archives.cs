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

namespace Cube.FileSystem.SevenZip.Archives
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOperator
    ///
    /// <summary>
    /// 圧縮ファイルに関する拡張メソッドを定義するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ArchiveOperator
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを生成します。
        /// </summary>
        ///
        /// <param name="item">圧縮ファイル情報</param>
        /// <param name="root">起点となるディレクトリのパス</param>
        ///
        /// <remarks>
        /// ArchiveItem.IsDirectory が false の場合、何もせずに終了します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void CreateDirectory(this ArchiveItem item, string root) =>
            CreateDirectory(item, root, new IO());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを生成します。
        /// </summary>
        ///
        /// <param name="item">圧縮ファイル情報</param>
        /// <param name="root">起点となるディレクトリのパス</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /// <remarks>
        /// ArchiveItem.IsDirectory が false の場合、何もせずに終了します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void CreateDirectory(this ArchiveItem item, string root, IO io)
        {
            if (!item.IsDirectory) return;
            var path = io.Combine(root, item.FullName);
            if (!io.Exists(path)) io.CreateDirectory(path);
            SetAttributes(item, root, io);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// ArchiveItem オブジェクトの内容に従ってファイルまたは
        /// ディレクトリに属性、作成日時、最終更新日時、最終アクセス日時を
        /// 設定します。
        /// </summary>
        ///
        /// <param name="item">圧縮ファイル情報</param>
        /// <param name="root">起点となるディレクトリのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetAttributes(this ArchiveItem item, string root) =>
            SetAttributes(item, root, new IO());

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// ArchiveItem オブジェクトの内容に従ってファイルまたは
        /// ディレクトリに属性、作成日時、最終更新日時、最終アクセス日時を
        /// 設定します。
        /// </summary>
        ///
        /// <param name="item">圧縮ファイル情報</param>
        /// <param name="root">起点となるディレクトリのパス</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetAttributes(this ArchiveItem item, string root, IO io)
        {
            var path = io.Combine(root, item.FullName);
            if (!io.Exists(path)) return;

            io.SetAttributes(path, item.Attributes);
            SetCreationTime(item, path, io);
            SetLastWriteTime(item, path, io);
            SetLastAccessTime(item, path, io);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetCreationTime
        ///
        /// <summary>
        /// 作成日時を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetCreationTime(this ArchiveItem item, string path, IO io)
        {
            var time = item.CreationTime  != DateTime.MinValue ? item.CreationTime :
                       item.LastWriteTime != DateTime.MinValue ? item.LastWriteTime :
                       item.LastAccessTime;
            if (time != DateTime.MinValue) io.SetCreationTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastWriteTime
        ///
        /// <summary>
        /// 最終更新日時を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetLastWriteTime(this ArchiveItem item, string path, IO io)
        {
            var time = item.LastWriteTime  != DateTime.MinValue ? item.LastWriteTime :
                       item.LastAccessTime != DateTime.MinValue ? item.LastAccessTime :
                       item.CreationTime;
            if (time != DateTime.MinValue) io.SetLastWriteTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastAccessTime
        ///
        /// <summary>
        /// 最終アクセス日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetLastAccessTime(this ArchiveItem item, string path, IO io)
        {
            var time = item.LastAccessTime != DateTime.MinValue ? item.LastAccessTime :
                       item.LastWriteTime  != DateTime.MinValue ? item.LastWriteTime :
                       item.CreationTime;
            if (time != DateTime.MinValue) io.SetLastAccessTime(path, time);
        }

        #endregion
    }
}
