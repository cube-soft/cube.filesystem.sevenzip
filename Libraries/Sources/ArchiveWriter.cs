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
using Cube.Collections;
using Cube.Generics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriter
    ///
    /// <summary>
    /// Provides functionality to create a new archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveWriter : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveWriter
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveWriter class with the
        /// specified format.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format) : this(format, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveWriter
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveWriter class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format, IO io)
        {
            _core  = new SevenZipLibrary();
            Format = format;
            IO     = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the collection of files or directories to be archived.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IList<FileItem> Items { get; } = new List<FileItem>();

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Option
        ///
        /// <summary>
        /// Gets or sets creating options of the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveOption Option { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// Gets or sets the collection of file or directory names to
        /// filter.
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
        /// Adds the specified file or directory to the archive.
        /// </summary>
        ///
        /// <param name="src">Path of file or directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(string src) => Add(src, IO.Get(src).Name);

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the specified file or directory to the archive.
        /// </summary>
        ///
        /// <param name="src">Path of file or directory.</param>
        /// <param name="pathInArchive">Relative path in the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(string src, string pathInArchive)
        {
            var info = IO.Get(src);
            if (info.Exists) AddItem(info, pathInArchive);
            else throw new System.IO.FileNotFoundException(info.FullName);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Clears all of added files and directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Items.Clear();

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /// <param name="dest">Path to save the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest) => Save(dest, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /// <param name="dest">Path to save the archive.</param>
        /// <param name="password">Password.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest, string password) => Save(dest, password, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /// <param name="dest">Path to save the archive.</param>
        /// <param name="password">Password query.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest, IQuery<string> password) => Save(dest, password, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /// <param name="dest">Path to save the archive.</param>
        /// <param name="password">Password.</param>
        /// <param name="progress">Progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest, string password, IProgress<Report> progress) =>
            Invoke(dest, password.HasValue() ? new PasswordQuery(password) : null, progress);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /// <param name="dest">Path to save the archive.</param>
        /// <param name="password">Password query.</param>
        /// <param name="progress">Progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest, IQuery<string> password, IProgress<Report> progress) =>
            Invoke(dest, password != null ? new PasswordQuery(password) : null, progress);

        #endregion

        #region Implementations

        #region Invoke

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(string dest, PasswordQuery query, IProgress<Report> progress)
        {
            if (Format == Format.Sfx) InvokeSfx(GetItems(), dest, query, progress);
            else if (Format == Format.Tar) InvokeTar(GetItems(), dest, query, progress);
            else Invoke(GetItems(), Format, dest, query, progress);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(IList<FileItem> src, Format fmt, string dest,
            IQuery<string> query, IProgress<Report> progress)
        {
            var dir = IO.Get(dest).DirectoryName;
            if (!IO.Exists(dir)) IO.CreateDirectory(dir);

            Create(src, dest, query, progress, cb =>
            {
                using (var ss = new ArchiveStreamWriter(IO.Create(dest)))
                {
                    var archive = _core.GetOutArchive(fmt);
                    Option.Convert(Format)?.Execute(archive as ISetProperties);
                    archive.UpdateItems(ss, (uint)src.Count, cb);
                }
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeTar
        ///
        /// <summary>
        /// Creates a new TAR archive and saves to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeTar(IList<FileItem> src, string dest,
            IQuery<string> query, IProgress<Report> progress)
        {
            var fi  = IO.Get(dest);
            var dir = IO.Combine(fi.DirectoryName, Guid.NewGuid().ToString("D"));
            var tmp = IO.Combine(dir, GetTarName(fi));

            try
            {
                Invoke(src, Format.Tar, tmp, query, progress);

                var m = (Option as TarOption)?.CompressionMethod ?? CompressionMethod.Copy;
                if (m == CompressionMethod.BZip2 || m == CompressionMethod.GZip || m == CompressionMethod.XZ)
                {
                    var f = new List<FileItem> { IO.Get(tmp).ToFileItem() };
                    Invoke(f, m.ToFormat(), dest, query, progress);
                }
                else IO.Move(tmp, dest, true);
            }
            finally { IO.TryDelete(dir); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeSfx
        ///
        /// <summary>
        /// Creates the self-executable archive and saves to the specified
        /// path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokeSfx(IList<FileItem> src, string dest,
            IQuery<string> password, IProgress<Report> progress)
        {
            var sfx = (Option as SfxOption)?.Module;
            if (!IO.Exists(sfx)) throw new System.IO.FileNotFoundException("SFX");
            var tmp = IO.Combine(IO.Get(dest).DirectoryName, Guid.NewGuid().ToString("D"));

            try
            {
                Invoke(src, Format.SevenZip, tmp, password, progress);

                using (var ds = IO.Create(dest))
                {
                    using (var ss = IO.OpenRead(sfx)) ss.CopyTo(ds);
                    using (var ss = IO.OpenRead(tmp)) ss.CopyTo(ds);
                }
            }
            finally { IO.TryDelete(tmp); }
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the ArchiveWriter
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) => _core.Dispose();

        /* ----------------------------------------------------------------- */
        ///
        /// GetTarName
        ///
        /// <summary>
        /// Gets the filename of TAR archive from the specified information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTarName(Information src)
        {
            var name = src.NameWithoutExtension;
            var cmp  = StringComparison.InvariantCultureIgnoreCase;
            return name.EndsWith(".tar", cmp) ? name : $"{name}.tar";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetItems
        ///
        /// <summary>
        /// Gets the collection of files or directories to be archived.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IList<FileItem> GetItems() => (Filters == null) ?
            Items :
            Items.Where(x => !new PathFilter(x.FullName).MatchAny(Filters)).ToList();

        /* ----------------------------------------------------------------- */
        ///
        /// AddItem
        ///
        /// <summary>
        /// Add the specified file or directory to the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddItem(Information src, string name)
        {
            if (CanRead(src)) Items.Add(src.ToFileItem(name));
            if (!src.IsDirectory) return;

            var files = IO.GetFiles(src.FullName).Select(e => IO.Get(e));
            foreach (var f in files) Items.Add(f.ToFileItem(IO.Combine(name, f.Name)));

            var dirs = IO.GetDirectories(src.FullName).Select(e => IO.Get(e));
            foreach (var d in dirs) AddItem(d, IO.Combine(name, d.Name));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CanRead
        ///
        /// <summary>
        /// Gets the value indicating whether the specified file or
        /// directory is readable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool CanRead(Information src)
        {
            if (src.IsDirectory) return true;
            using (var stream = IO.OpenRead(src.FullName)) return stream != null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveUpdateCallback class
        /// and executes the specified callback.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Create(IList<FileItem> src, string dest, IQuery<string> query,
            IProgress<Report> progress, Action<ArchiveUpdateCallback> callback)
        {
            var error = default(Exception);
            var cb    = new ArchiveUpdateCallback(src, dest, IO)
            {
                Password = query,
                Progress = progress,
            };

            try { callback(cb); }
            catch (Exception err) { error = err; }
            finally
            {
                var kv = KeyValuePair.Create(cb.Result, error ?? cb.Exception);
                cb.Dispose();
                Terminate(kv.Key, kv.Value);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Terminate
        ///
        /// <summary>
        /// Invokes post processing and throws an exception if needed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Terminate(OperationResult src, Exception error)
        {
            if (src == OperationResult.OK) return;
            if (src == OperationResult.UserCancel) throw new OperationCanceledException();
            if (error != null) throw error;
            else throw new System.IO.IOException($"{src}");
        }

        #endregion

        #region Fields
        private readonly SevenZipLibrary _core;
        #endregion
    }
}
