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
using System.IO;

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
        /// <param name="format">圧縮フォーマット</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format) : this(format, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveWriter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="format">圧縮フォーマット</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format, Operator io)
        {
            Format = format;
            _lib = new NativeLibrary();
            _io = io;
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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ファイルまたはディレクトリを圧縮ファイルに追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Add(string path)
            => Add(path, _io.GetFileName(path));

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ファイルまたはフォルダを圧縮ファイルに追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Add(string path, string pathInArchive)
        {
            if (_io.Exists(path))
            {
                if (_io.IsDirectory(path)) Add(new DirectoryInfo(path), pathInArchive);
                else Add(new FileInfo(path), pathInArchive);
            }
            else throw new FileNotFoundException(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 圧縮ファイルを作成し保存します。
        /// </summary>
        /// 
        /// <param name="path">保存パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Save(string path) => Save(path, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 圧縮ファイルを作成し保存します。
        /// </summary>
        /// 
        /// <param name="path">保存パス</param>
        /// <param name="password">パスワード</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Save(string path, string password)
            => SaveCore(path, new PasswordQuery(password), null);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 圧縮ファイルを作成し保存します。
        /// </summary>
        /// 
        /// <param name="path">保存パス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="progress">進捗状況報告用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Save(string path, IQuery<string, string> password, IProgress<ArchiveReport> progress)
            => SaveCore(path, new PasswordQuery(password), progress);

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

            if (disposing) _lib?.Dispose();

            _disposed = true;
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ファイルを圧縮ファイルに追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Add(FileInfo info, string pathInArchive)
            => _items.Add(new FileItem(info, pathInArchive));

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ディレクトリを圧縮ファイルに追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Add(DirectoryInfo info, string pathInArchive)
        {
            _items.Add(new FileItem(info, pathInArchive));

            foreach (var child in info.GetFiles())
            {
                Add(child, _io.Combine(pathInArchive, child.Name));
            }

            foreach (var child in info.GetDirectories())
            {
                Add(child, _io.Combine(pathInArchive, child.Name));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveCore
        ///
        /// <summary>
        /// 圧縮ファイルを作成し保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void SaveCore(string path, IQuery<string, string> password, IProgress<ArchiveReport> progress)
        {
            var raw = _lib.GetOutArchive(Format);
            var stream = new ArchiveStreamWriter(_io.Create(path));
            var callback = new ArchiveUpdateCallback(_items, path, _io)
            {
                Password = password,
                Progress = progress,
            };

            try { raw.UpdateItems(stream, (uint)_items.Count, callback); }
            finally
            {
                stream.Dispose();
                SaveResult(path, callback.Result);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveResult
        ///
        /// <summary>
        /// 圧縮後の処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveResult(string path, OperationResult result)
        {
            switch (result)
            {
                case OperationResult.OK:
                    break;
                case OperationResult.UserCancel:
                    throw new UserCancelException();
                default:
                    throw new IOException(result.ToString());
            }
        }

        #region Fields
        private bool _disposed = false;
        private NativeLibrary _lib;
        private Operator _io;
        private IList<FileItem> _items = new List<FileItem>();
        #endregion

        #endregion
    }
}
