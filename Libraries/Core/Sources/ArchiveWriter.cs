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
using System.Linq;
using Cube.Collections;

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
    public sealed class ArchiveWriter : DisposableBase
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
        public ArchiveWriter(Format format) : this(format, new()) { }

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
        /// <param name="options">Archive options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveWriter(Format format, CompressionOption options)
        {
            Format  = format;
            Options = options;
        }

        #endregion

        #region Properties

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
        /// Options
        ///
        /// <summary>
        /// Gets or sets the options when creating a new archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionOption Options { get; }

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
        /// <param name="name">Relative path in the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(string src, string name)
        {
            var e = new RawEntity(IoEx.GetEntitySource(src), name);
            if (e.Exists) AddItem(e);
            else throw new System.IO.FileNotFoundException(e.FullName);
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
        public void Clear() => _items.Clear();

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
        public void Save(string dest) => Save(dest, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string dest, IProgress<Report> progress)
        {
            if (Format == Format.Sfx) SaveAsSfx(dest, _items, progress);
            else if (Format == Format.Tar) SaveAsTar(dest, _items, progress);
            else SaveAs(dest, _items, Format, progress);
        }

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

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAs
        ///
        /// <summary>
        /// Creates a new archive and saves to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveAs(string dest, IList<RawEntity> src, Format fmt, IProgress<Report> progress)
        {
            var dir = Io.Get(dest).DirectoryName;
            Io.CreateDirectory(dir);

            Invoke(cb =>
            {
                using var ss = new ArchiveStreamWriter(Io.Create(dest));
                var archive = _lib.GetOutArchive(fmt);
                var setter  = CompressionOptionSetter.From(Format, Options);

                setter?.Invoke(archive as ISetProperties);
                _ = archive.UpdateItems(ss, (uint)src.Count, cb);
            }, src, dest, progress);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAsTar
        ///
        /// <summary>
        /// Creates a new TAR archive and saves to the specified path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveAsTar(string dest, IList<RawEntity> src, IProgress<Report> progress)
        {
            var fi  = Io.Get(dest);
            var dir = Io.Combine(fi.DirectoryName, Guid.NewGuid().ToString("N"));
            var tmp = Io.Combine(dir, GetTarName(fi));

            try
            {
                SaveAs(tmp, src, Format.Tar, progress);

                var m = Options.CompressionMethod;
                if (m == CompressionMethod.BZip2 || m == CompressionMethod.GZip || m == CompressionMethod.XZ)
                {
                    var f = new List<RawEntity> { new(IoEx.GetEntitySource(tmp)) };
                    SaveAs(dest, f, m.ToFormat(), progress);
                }
                else Io.Move(tmp, dest, true);
            }
            finally { GetType().LogWarn(() => Io.Delete(dir)); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAsSfx
        ///
        /// <summary>
        /// Creates the self-executable archive and saves to the specified
        /// path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveAsSfx(string dest, IList<RawEntity> src, IProgress<Report> progress)
        {
            var sfx = (Options as SfxOption)?.Module;
            if (!Io.Exists(sfx)) throw new System.IO.FileNotFoundException("SFX");
            var tmp = Io.Combine(Io.Get(dest).DirectoryName, Guid.NewGuid().ToString("N"));

            try
            {
                SaveAs(tmp, src, Format.SevenZip, progress);

                using var ds = Io.Create(dest);
                using (var ss = Io.Open(sfx)) ss.CopyTo(ds);
                using (var ss = Io.Open(tmp)) ss.CopyTo(ds);
            }
            finally { GetType().LogWarn(() => Io.Delete(tmp)); }
        }

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
        /// CanRead
        ///
        /// <summary>
        /// Gets the value indicating whether the specified file or
        /// directory is readable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool CanRead(RawEntity src)
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
        /// AddItem
        ///
        /// <summary>
        /// Add the specified file or directory to the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddItem(RawEntity src)
        {
            if (Options.Filter?.Invoke(src) ?? false) return;
            if (CanRead(src)) _items.Add(src);
            if (!src.IsDirectory) return;

            var files = Io.GetFiles(src.FullName).Select(e => IoEx.GetEntitySource(e));
            foreach (var f in files)
            {
                var e = new RawEntity(f, Io.Combine(src.RelativeName, f.Name));
                if (Options.Filter?.Invoke(e) ?? false) continue;
                _items.Add(new(f, Io.Combine(src.RelativeName, f.Name)));
            }

            var dirs = Io.GetDirectories(src.FullName).Select(e => IoEx.GetEntitySource(e));
            foreach (var e in dirs) AddItem(new(e, Io.Combine(src.RelativeName, e.Name)));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveUpdateCallback class
        /// and executes the specified callback.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(Action<UpdateCallback> callback,
            IList<RawEntity> src, string dest, IProgress<Report> progress)
        {
            var error = default(Exception);
            var cb    = new UpdateCallback(src)
            {
                Destination = dest,
                Password    = Options.Password,
                Progress    = progress,
            };

            try { callback(cb); }
            catch (Exception e) { error = e; }
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
            throw new System.IO.IOException($"{src}", error);
        }

        #endregion

        #region Fields
        private readonly SevenZipLibrary _lib = new();
        private readonly List<RawEntity> _items = new();
        #endregion
    }
}
