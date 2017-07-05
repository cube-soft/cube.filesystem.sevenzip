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
using System.ComponentModel;
using System.IO;
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
            _lib = new NativeLibrary();
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
        /// FileCount
        ///
        /// <summary>
        /// 圧縮するファイル数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long FileCount => _items.Count;

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        ///
        /// <summary>
        /// 圧縮の終了したファイル数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long DoneCount { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileSize
        ///
        /// <summary>
        /// 圧縮するファイルの合計バイト数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long FileSize { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneSize
        ///
        /// <summary>
        /// 圧縮の終了したバイト数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long DoneSize { get; private set; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// 進捗状況の通知時に発生するイベントです。
        /// </summary>
        /// 
        /// <remarks>
        /// 通知される値は、展開の完了したバイト数です。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<long> Progress;

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgress
        ///
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnProgress(ValueEventArgs<long> e)
            => Progress?.Invoke(this, e);

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
            => Add(path, Path.GetFileName(path));

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
            if (File.Exists(path)) Add(new FileInfo(path), pathInArchive);
            else if (Directory.Exists(path)) Add(new DirectoryInfo(path), pathInArchive);
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
        /* ----------------------------------------------------------------- */
        public void Save(string path, string password)
        {
            DoneCount = 0;
            DoneSize  = 0;
            FileSize  = 0;

            var raw      = _lib.GetOutArchive(Format);
            var stream   = new ArchiveStreamWriter(File.Create(path));
            var callback = new ArchiveUpdateCallback(_items) { Password = password };

            try
            {
                callback.PropertyChanged += WhenPropertyChanged;
                raw.UpdateItems(stream, (uint)_items.Count, callback);
            }
            finally
            {
                callback.PropertyChanged -= WhenPropertyChanged;
                stream.Dispose();
                SaveResult(path, callback.Result);
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
                Add(child, Path.Combine(pathInArchive, child.Name));
            }

            foreach (var child in info.GetDirectories())
            {
                Add(child, Path.Combine(pathInArchive, child.Name));
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
                default:
                    throw new IOException(result.ToString());
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenNotifyFileSize
        ///
        /// <summary>
        /// NotifyFileSize イベント発生時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void WhenNotifyFileSize(object sender, ValueEventArgs<long> e)
            => FileSize = e.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseProgress
        ///
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void RaiseProgress(object sender, ValueEventArgs<long> e)
            => OnProgress(e);

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPropertyChanged
        ///
        /// <summary>
        /// プロパティ変更時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void WhenPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ac = sender as ArchiveUpdateCallback;
            if (ac == null) return;

            switch (e.PropertyName)
            {
                case nameof(ac.DoneCount):
                    DoneCount = ac.DoneCount;
                    break;
                case nameof(ac.FileSize):
                    FileSize = ac.FileSize;
                    break;
                case nameof(ac.DoneSize):
                    OnProgress(ValueEventArgs.Create(ac.DoneSize));
                    break;
            }
        }

        #region Fields
        private bool _disposed = false;
        private NativeLibrary _lib;
        private IList<FileItem> _items = new List<FileItem>();
        #endregion

        #endregion
    }
}
