/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.Collections;
using Cube.Logging;
using Cube.Mixin.String;

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
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format) { Format = format; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the collection of files or directories to be archived.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IList<RawEntity> Items { get; } = new List<RawEntity>();

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
        public void Add(string src) => Add(src, Io.Get(src).Name);

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
            var fi = IoEx.GetEntitySource(src);
            if (fi.Exists) AddItem(fi, pathInArchive);
            else throw new System.IO.FileNotFoundException(fi.FullName);
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
        private void Invoke(IList<RawEntity> src, Format fmt, string dest,
            IQuery<string> query, IProgress<Report> progress)
        {
            var dir = Io.Get(dest).DirectoryName;
            Io.CreateDirectory(dir);

            Create(src, dest, query, progress, cb =>
            {
                using var ss = new ArchiveStreamWriter(Io.Create(dest));
                var archive = _lib.GetOutArchive(fmt);
                Option.Convert(Format)?.Execute(archive as ISetProperties);
                _ = archive.UpdateItems(ss, (uint)src.Count, cb);
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
        private void InvokeTar(IList<RawEntity> src, string dest,
            IQuery<string> query, IProgress<Report> progress)
        {
            var fi  = Io.Get(dest);
            var dir = Io.Combine(fi.DirectoryName, Guid.NewGuid().ToString("N"));
            var tmp = Io.Combine(dir, GetTarName(fi));

            try
            {
                Invoke(src, Format.Tar, tmp, query, progress);

                var m = (Option as TarOption)?.CompressionMethod ?? CompressionMethod.Copy;
                if (m == CompressionMethod.BZip2 || m == CompressionMethod.GZip || m == CompressionMethod.XZ)
                {
                    var f = new List<RawEntity> { new(IoEx.GetEntitySource(tmp)) };
                    Invoke(f, m.ToFormat(), dest, query, progress);
                }
                else Io.Move(tmp, dest, true);
            }
            finally { GetType().LogWarn(() => Io.Delete(dir)); }
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
        private void InvokeSfx(IList<RawEntity> src, string dest,
            IQuery<string> password, IProgress<Report> progress)
        {
            var sfx = (Option as SfxOption)?.Module;
            if (!Io.Exists(sfx)) throw new System.IO.FileNotFoundException("SFX");
            var tmp = Io.Combine(Io.Get(dest).DirectoryName, Guid.NewGuid().ToString("N"));

            try
            {
                Invoke(src, Format.SevenZip, tmp, password, progress);

                using (var ds = Io.Create(dest))
                {
                    using (var ss = Io.Open(sfx)) ss.CopyTo(ds);
                    using (var ss = Io.Open(tmp)) ss.CopyTo(ds);
                }
            }
            finally { GetType().LogWarn(() => Io.Delete(tmp)); }
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
        protected override void Dispose(bool disposing) => _lib.Dispose();

        /* ----------------------------------------------------------------- */
        ///
        /// GetTarName
        ///
        /// <summary>
        /// Gets the filename of TAR archive from the specified information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTarName(Entity src)
        {
            var name = src.BaseName;
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
        private IList<RawEntity> GetItems() => (Filters == null) ?
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
        private void AddItem(EntitySource src, string name)
        {
            if (CanRead(src)) Items.Add(new(src, name));
            if (!src.IsDirectory) return;

            var files = Io.GetFiles(src.FullName).Select(e => IoEx.GetEntitySource(e));
            foreach (var f in files) Items.Add(new(f, Io.Combine(name, f.Name)));

            var dirs = Io.GetDirectories(src.FullName).Select(e => IoEx.GetEntitySource(e));
            foreach (var d in dirs) AddItem(d, Io.Combine(name, d.Name));
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
        private bool CanRead(EntitySource src)
        {
            if (src.IsDirectory) return true;
            try
            {
                using var stream = Io.Open(src.FullName);
                return stream != null;
            }
            catch { return false; }
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
        private void Create(IList<RawEntity> src, string dest, IQuery<string> query,
            IProgress<Report> progress, Action<UpdateCallback> callback)
        {
            var error = default(Exception);
            var cb    = new UpdateCallback(src, dest)
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
        private readonly SevenZipLibrary _lib = new();
        #endregion
    }
}
