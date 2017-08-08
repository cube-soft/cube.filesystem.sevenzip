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
using Cube.FileSystem.SevenZip.Archives;

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
        /// <param name="path">圧縮ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string path) : this(path, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">圧縮ファイルのパス</param>
        /// <param name="password">パスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string path, string password)
            : this(path, password, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">圧縮ファイルのパス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string path, IQuery<string, string> password)
            : this(path, password, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">圧縮ファイルのパス</param>
        /// <param name="password">パスワード</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string path, string password, Operator io)
        {
            Source = path;
            _password = new PasswordQuery(password);
            _io = io;
            Open();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">圧縮ファイルのパス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string path, IQuery<string, string> password, Operator io)
        {
            Source = path;
            _password = new PasswordQuery(password);
            _io = io;
            Open();
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

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 圧縮ファイルのファイル形式を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public Format Format { get; private set; } = Format.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 圧縮ファイルの一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IReadOnlyCollection<ArchiveItem> Items => _items;

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Extracted
        /// 
        /// <summary>
        /// 圧縮ファイルの項目が展開完了した時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<ArchiveItem> Extracted;

        /* ----------------------------------------------------------------- */
        ///
        /// OnExtracted
        /// 
        /// <summary>
        /// Extracted イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnExtracted(ValueEventArgs<ArchiveItem> e)
            => Extracted?.Invoke(this, e);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        /// 
        /// <param name="directory">保存ディレクトリ</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        /// 
        /// <param name="directory">保存ディレクトリ</param>
        /// <param name="progress">進捗報告用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, IProgress<ArchiveReport> progress)
        {
            using (var cb = new ArchiveExtractCallback(Source, directory, _items, _io, _items.Count, -1))
            {
                cb.Password = _password;
                cb.Progress = progress;
                cb.Extracted += (s, e) => OnExtracted(e);
                _raw.Extract(null, uint.MaxValue, 0, cb);
            }
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
        ~ArchiveReader()
        {
            Dispose(false);
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                _7z?.Dispose();
            }

            _disposed = true;
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 圧縮ファイルを開きます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Open()
        {
            Format = Formats.FromFile(Source, _io);
            if (Format == Format.Unknown) throw new NotSupportedException();

            _stream = new ArchiveStreamReader(_io.OpenRead(Source));
            _7z     = new SevenZipLibrary();
            _raw    = _7z.GetInArchive(Format);

            var pos = 32UL * 1024;
            _raw.Open(_stream, ref pos, new ArchiveOpenCallback(Source) { Password = _password });

            _items = new ReadOnlyArchiveList(_raw, Format, Source, _password, _io);
        }

        #region Fields
        private bool _disposed = false;
        private SevenZipLibrary _7z;
        private IInArchive _raw;
        private ArchiveStreamReader _stream;
        private IQuery<string, string> _password;
        private Operator _io;
        private ReadOnlyArchiveList _items;
        #endregion

        #endregion
    }
}
