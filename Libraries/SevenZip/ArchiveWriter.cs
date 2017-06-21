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
    /// ArchiveWriter
    /// 
    /// <summary>
    /// 圧縮ファイルを作成するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveWriter : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveWriter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format)
        {
            Format = format;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 圧縮ファイルのフォーマットを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 圧縮するファイル一覧を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IList<System.IO.FileInfo> Items { get; } = new List<System.IO.FileInfo>();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 圧縮ファイルを作成し保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Save(string path, string password)
        {
            using (var lib = new NativeLibrary())
            using (var stream = new ArchiveStreamWriter(System.IO.File.Create(path)))
            {
                var raw = lib.GetOutArchive(Format);
                var callback = new ArchiveUpdateCallback(Items);
                raw.UpdateItems(stream, (uint)Items.Count, callback);
            }
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveWriter
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        // ~ArchiveWriter() {
        //   Dispose(false);
        // }

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
            Dispose(true);
            // GC.SuppressFinalize(this);
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
            if (_disposed) return;

            //if (disposing)
            //{

            //}

            _disposed = true;
        }

        #endregion

        #endregion

        #region Fields
        private bool _disposed = false;
        #endregion
    }
}
