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
using Cube.Streams;

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
            _dispose = new OnceAction<bool>(Dispose);
            Format = format;
            _io = io;
            _7z = new SevenZipLibrary();
            Debug.Assert(_7z != null);
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
        /// Option
        ///
        /// <summary>
        /// 圧縮ファイルのフォーマットを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveOption Option { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// 圧縮ファイルに含めないファイル名またはディレクトリ名一覧を
        /// 取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Filters { get; set; }

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
            => Add(path, _io.Get(path).Name);

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
            var info = _io.Get(path);
            if (info.Exists) AddItem(info, pathInArchive);
            else throw new System.IO.FileNotFoundException(info.FullName);
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
        {
            var query = !string.IsNullOrEmpty(password) ?
                        new PasswordQuery(password) :
                        null;

            if (Format == Format.Sfx) SaveCoreSfx(path, query, null, GetItems());
            else if (Format == Format.Tar) SaveCoreTar(path, query, null, GetItems());
            else SaveCore(Format, path, query, null, GetItems());
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
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="progress">進捗状況報告用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Save(string path, IQuery<string, string> password, IProgress<ArchiveReport> progress)
        {
            var query = password != null ?
                        new PasswordQuery(password) :
                        null;

            if (Format == Format.Sfx) SaveCoreSfx(path, query, progress, GetItems());
            else if (Format == Format.Tar) SaveCoreTar(path, query, progress, GetItems());
            else SaveCore(Format, path, query, progress, GetItems());
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
        ~ArchiveWriter() { _dispose.Invoke(false); }

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
            _7z.Dispose();
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SaveCoreSfx
        ///
        /// <summary>
        /// 自己解凍形式ファイルを作成し保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void SaveCoreSfx(string path, IQuery<string, string> password,
            IProgress<ArchiveReport> progress, IList<FileItem> items)
        {
            var sfx = (Option as SfxOption)?.Module;
            if (string.IsNullOrEmpty(sfx) || !_io.Exists(sfx))
            {
                throw new System.IO.FileNotFoundException("SFX");
            }

            var tmp = _io.Combine(_io.Get(path).DirectoryName, Guid.NewGuid().ToString("D"));

            try
            {

                SaveCore(Format.SevenZip, tmp, password, progress, items);

                using (var dest = _io.Create(path))
                {
                    using (var src = _io.OpenRead(sfx)) src.CopyTo(dest);
                    using (var src = _io.OpenRead(tmp)) src.CopyTo(dest);
                }
            }
            finally { _io.Delete(tmp); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveCoreTar
        ///
        /// <summary>
        /// Tar ファイルを作成し保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void SaveCoreTar(string path, IQuery<string, string> password,
            IProgress<ArchiveReport> progress, IList<FileItem> items)
        {
            var info = _io.Get(path);
            var nwe  = _io.Get(info.NameWithoutExtension);
            var name = (nwe.Extension == ".tar") ? nwe.Name : $"{nwe.Name}.tar";
            var dir  = _io.Combine(info.DirectoryName, Guid.NewGuid().ToString("D"));
            var tmp  = _io.Combine(dir, name);

            try
            {
                SaveCore(Format.Tar, tmp, password, progress, items);

                var f = new List<FileItem> { new FileItem(tmp) };
                var m = (Option as TarOption)?.CompressionMethod ?? CompressionMethod.Copy;

                switch (m)
                {
                    case CompressionMethod.BZip2:
                    case CompressionMethod.GZip:
                    case CompressionMethod.XZ:
                        SaveCore(Formats.FromMethod(m), path, password, progress, f);
                        break;
                    default: // Copy
                        _io.Move(tmp, path, true);
                        break;
                }
            }
            finally { _io.Delete(dir); }
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
        private void SaveCore(Format format, string path, IQuery<string, string> password,
            IProgress<ArchiveReport> progress, IList<FileItem> items)
        {
            var dir = _io.Get(_io.Get(path).DirectoryName);
            if (!dir.Exists) _io.CreateDirectory(dir.FullName);

            var archive = _7z.GetOutArchive(format);
            var stream  = new ArchiveStreamWriter(_io.Create(path));
            var cb      = new ArchiveUpdateCallback(items, path, _io)
            {
                Password = password,
                Progress = progress,
            };

            try
            {
                GetSetter()?.Execute(archive as ISetProperties);
                archive.UpdateItems(stream, (uint)items.Count, cb);
            }
            finally
            {
                var result = cb.Result;
                var err    = cb.Exception;
                stream.Dispose();
                cb.Dispose();
                ThrowIfError(result, err);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddItem
        ///
        /// <summary>
        /// ファイルまたはディレクトリを圧縮ファイルに追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void AddItem(IInformation info, string name)
        {
            var path = info.FullName;
            if (CanRead(info)) _items.Add(new FileItem(path, name));
            if (!info.IsDirectory) return;

            foreach (var file in _io.GetFiles(path))
            {
                var child = _io.Get(file);
                _items.Add(new FileItem(child.FullName, _io.Combine(name, child.Name)));
            }

            foreach (var dir in _io.GetDirectories(path))
            {
                var child = _io.Get(dir);
                AddItem(child, _io.Combine(name, child.Name));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetItems
        ///
        /// <summary>
        /// 圧縮項目一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private IList<FileItem> GetItems()
            => Filters == null ?
               _items :
               _items.Where(x => !new PathFilter(x.FullName).MatchAny(Filters)).ToList();

        /* ----------------------------------------------------------------- */
        ///
        /// CanRead
        ///
        /// <summary>
        /// 読み込み可能なファイルかどうかを判別します。
        /// </summary>
        /// 
        /// <remarks>
        /// ディレクトリの場合は true が返ります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        private bool CanRead(IInformation info)
        {
            if (info.IsDirectory) return true;
            using (var stream = _io.OpenRead(info.FullName)) return stream != null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSetter
        ///
        /// <summary>
        /// ArchiveOptionSetter オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private ArchiveOptionSetter GetSetter()
        {
            if (Option == null) return null;

            switch (Format)
            {
                case Format.Zip:
                    return new ZipOptionSetter(Option);
                case Format.SevenZip:
                case Format.Sfx:
                    return new SevenZipOptionSetter(Option);
                case Format.Tar:
                    return null;
                default:
                    return new ArchiveOptionSetter(Option);
            }
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
                case OperationResult.UserCancel:
                    throw new OperationCanceledException();
            }

            if (err != null) throw err;
            else throw new System.IO.IOException($"{result}");
        }

        #region Fields
        private OnceAction<bool> _dispose;
        private SevenZipLibrary _7z;
        private Operator _io;
        private IList<FileItem> _items = new List<FileItem>();
        #endregion

        #endregion
    }
}
