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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        public ArchiveReader(string path, string password) :
            this(path, password, new IO()) { }

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
        public ArchiveReader(string path, IQuery<string, string> password) :
            this(path, password, new IO()) { }

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
        public ArchiveReader(string path, string password, IO io)
        {
            _dispose = new OnceAction<bool>(Dispose);
            Source = path;
            _io = io;
            _password = new PasswordQuery(password);
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
        public ArchiveReader(string path, IQuery<string, string> password, IO io)
        {
            _dispose = new OnceAction<bool>(Dispose);
            Source = path;
            _io = io;
            _password = new PasswordQuery(password);
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
        public IReadOnlyList<ArchiveItem> Items => _items;

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// 展開をスキップするファイル名またはディレクトリ名一覧を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Filters { get; set; }

        #endregion

        #region Events

        #region Extracting

        /* ----------------------------------------------------------------- */
        ///
        /// Extracting
        ///
        /// <summary>
        /// 圧縮ファイル各項目の展開開始時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<ArchiveItem> Extracting;

        /* ----------------------------------------------------------------- */
        ///
        /// OnExtracted
        ///
        /// <summary>
        /// Extracting イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnExtracting(ValueEventArgs<ArchiveItem> e) =>
            Extracting?.Invoke(this, e);

        #endregion

        #region Extracted

        /* ----------------------------------------------------------------- */
        ///
        /// Extracted
        ///
        /// <summary>
        /// 圧縮ファイル各項目の展開完了時に発生するイベントです。
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
        protected virtual void OnExtracted(ValueEventArgs<ArchiveItem> e) =>
            Extracted?.Invoke(this, e);

        #endregion

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
            using (var cb = new ArchiveExtractCallback(Source, directory, _items, _io))
            {
                cb.TotalCount  = _items.Count;
                cb.Password    = _password;
                cb.Progress    = progress;
                cb.Filters     = Filters;
                cb.Extracting += (s, e) => OnExtracting(e);
                cb.Extracted  += (s, e) => OnExtracted(e);

                _archive.Extract(null, uint.MaxValue, 0, cb);
                ThrowIfError(cb.Result, cb.Exception);
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
        ~ArchiveReader() { _dispose.Invoke(false); }

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
            _dispose.Invoke(true);
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
            if (disposing) _callback.Dispose();
            _archive.Close();
            _7z.Dispose();
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

            var stream = new ArchiveStreamReader(_io.OpenRead(Source));
            Debug.Assert(stream != null);

            _7z = new SevenZipLibrary();
            Debug.Assert(_7z != null);

            _archive = _7z.GetInArchive(Format);
            Debug.Assert(_archive != null);

            _callback = new ArchiveOpenCallback(Source, stream, _io) { Password = _password };
            _archive.Open(stream, IntPtr.Zero, _callback);
            _items = new ReadOnlyArchiveList(_archive, Format, Source, _password, _io);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ThrowIfError
        ///
        /// <summary>
        /// エラーが発生していた場合に例外を送出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ThrowIfError(OperationResult result, Exception err)
        {
            switch (result)
            {
                case OperationResult.OK:
                    return;
                case OperationResult.DataError:
                    if (Items.Any(x => x.Encrypted))
                    {
                        _password.Reset();
                        throw new EncryptionException();
                    }
                    break;
                case OperationResult.WrongPassword:
                    _password.Reset();
                    throw new EncryptionException();
                case OperationResult.UserCancel:
                    throw new OperationCanceledException();
                default:
                    break;
            }

            if (err != null) throw err;
            else throw new System.IO.IOException($"{result}");
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly IO _io;
        private readonly PasswordQuery _password;
        private SevenZipLibrary _7z;
        private IInArchive _archive;
        private ArchiveOpenCallback _callback;
        private ReadOnlyArchiveList _items;
        #endregion
    }
}
