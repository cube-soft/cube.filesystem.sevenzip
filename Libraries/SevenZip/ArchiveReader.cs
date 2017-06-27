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
    /// ArchiveReader
    /// 
    /// <summary>
    /// 圧縮ファイルを読み込み、展開するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveReader : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveReader() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 圧縮ファイルの一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IReadOnlyCollection<ArchiveItem> Items { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 圧縮ファイルを開きます。
        /// </summary>
        /// 
        /// <param name="path">圧縮ファイルのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Open(string path) => Open(path, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 圧縮ファイルを開きます。
        /// </summary>
        /// 
        /// <param name="path">圧縮ファイルのパス</param>
        /// <param name="password">パスワード</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Open(string path, string password)
        {
            var ext = System.IO.Path.GetExtension(path);
            var fmt = FormatConversions.FromExtension(ext);
            if (fmt == Format.Unknown) throw new NotSupportedException();

            var pos = 32UL * 1024;

            _lib    = new NativeLibrary();
            _stream = new ArchiveStreamReader(System.IO.File.OpenRead(path));
            _raw    = _lib.GetInArchive(fmt);
            _raw.Open(_stream, ref pos, new ArchiveOpenCallback(password));

            Items = new ReadOnlyArchiveCollection(_raw);
        }

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~Archive
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        // ~ArchiveReader() {
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

            if (disposing)
            {
                _raw?.Close();
                _stream?.Dispose();
                _lib?.Dispose();
            }

            _disposed = true;
        }

        #endregion

        #endregion

        #region Fields
        private bool _disposed = false;
        private NativeLibrary _lib;
        private IInArchive _raw;
        private ArchiveStreamReader _stream;
        #endregion
    }
}
