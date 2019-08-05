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
using Cube.FileSystem.SevenZip.Mixin;
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReaderController
    ///
    /// <summary>
    /// Provides functionality to get properties of an archived item and
    /// execute the extract operations.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ArchiveReaderController : Controller, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReaderController
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveReaderController
        /// class with the specified arguments.
        /// </summary>
        ///
        /// <param name="format">Format of the archive..</param>
        /// <param name="src">Path of the archive.</param>
        /// <param name="password">Query to get password.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /// <remarks>
        /// BZip2, GZip など一部の圧縮形式で項目数を取得出来ていないため、
        /// 暫定的に初期値を 1 に設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReaderController(Format format, string src, PasswordQuery password, IO io)
        {
            if (format == Format.Unknown) throw new UnknownFormatException();

            var s = new ArchiveStreamReader(io.OpenRead(src));
            _szip = new SevenZipLibrary();
            _open = new ArchiveOpenCallback(src, s, io) { Password = password };
            _core = _szip.GetInArchive(format);
            _core.Open(s, IntPtr.Zero, _open);

            var n = (int)Math.Max(_core.GetNumberOfItems(), 1); // see remarks
            Source   = src;
            Format   = format;
            Password = password;
            IO       = io;
            Items    = new ArchiveList(this, n);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the format of archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets the query to get password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordQuery Password { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the collection of archived items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IReadOnlyList<ArchiveItem> Items { get; }

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
        /// Extract
        ///
        /// <summary>
        /// Extracts all items and save to the specified directory.
        /// </summary>
        ///
        /// <param name="dest">Directory path to save.</param>
        /// <param name="test">Test mode or not.</param>
        /// <param name="pg">Object to notify progress.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string dest, bool test, IProgress<Report> pg) => Invoke(
            null, Items.Count, dest, pg,
            e => _core.Extract(null, uint.MaxValue, test ? 1 : 0, e)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the item of specified index and save to the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="index">Index to extract.</param>
        /// <param name="dest">Directory path to save.</param>
        /// <param name="test">Test mode or not.</param>
        /// <param name="pg">Object to notify progress.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(int index, string dest, bool test, IProgress<Report> pg) => Invoke(
            new[] { index }, 1, dest, pg,
            e => _core.Extract(new[] { (uint)index }, 1, test ? 1 : 0, e)
        );

        #region Controller

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the specified path.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="options">Optional parameters.</param>
        ///
        /// <returns>Controllable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public override Controllable Create(string src, params object[] options)
        {
            Debug.Assert(options.Length > 0 && options[0] is int);
            var dest = new ArchiveItemControllable(src, (int)options[0]);
            Refresh(dest);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Refreshes information of the item.
        /// </summary>
        ///
        /// <param name="src">Object to be refreshed.</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void Refresh(Controllable src)
        {
            var ai = src.ToAi();
            ai.RawName        = _core.GetPath(ai.Index, ai.Source, IO);
            ai.Crc            = _core.Get<uint>(ai.Index, ItemPropId.Crc);
            ai.Encrypted      = _core.Get<bool>(ai.Index, ItemPropId.Encrypted);
            ai.Exists         = true;
            ai.IsDirectory    = _core.Get<bool>(ai.Index, ItemPropId.IsDirectory);
            ai.Attributes     = (System.IO.FileAttributes)_core.Get<uint>(ai.Index, ItemPropId.Attributes);
            ai.Length         = (long)_core.Get<ulong>(ai.Index, ItemPropId.Size);
            ai.CreationTime   = _core.Get<DateTime>(ai.Index, ItemPropId.CreationTime);
            ai.LastWriteTime  = _core.Get<DateTime>(ai.Index, ItemPropId.LastWriteTime);
            ai.LastAccessTime = _core.Get<DateTime>(ai.Index, ItemPropId.LastAccessTime);
            ai.Filter         = new PathFilter(ai.RawName)
            {
                AllowParentDirectory  = false,
                AllowDriveLetter      = false,
                AllowCurrentDirectory = false,
                AllowInactivation     = false,
                AllowUnc              = false,
            };

            var path = ai.Filter.Result;
            var fi   = path.HasValue() ? IO.Get(path) : default;
            ai.FullName      = ai.Filter.Result;
            ai.Name          = fi?.Name;
            ai.BaseName      = fi?.BaseName;
            ai.Extension     = fi?.Extension;
            ai.DirectoryName = fi?.DirectoryName;

            if (ai.FullName == ai.RawName) return;
            this.LogDebug($"Escape:{ai.FullName}", $"Raw:{ai.RawName}");
        }

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveReaderController
        ///
        /// <summary>
        /// Finalizes the ArchiveReaderController.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ArchiveReaderController() { Dispose(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the ArchiveReaderController.
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
        /// Releases the unmanaged resources used by the ArchiveReader
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            _core?.Close();
            _open?.Dispose();
            _szip?.Dispose();
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Executes the extract operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(IEnumerable<int> src, int count, string direcotry,
            IProgress<Report> progress, Action<ArchiveExtractCallback> action)
        {
            using(var cb = src != null ?
                      new ArchiveExtractCallback(this, src, count, direcotry) :
                      new ArchiveExtractCallback(this, direcotry))
            {
                cb.Password   = Password;
                cb.Progress   = progress;
                if (src == null) cb.Filters = Filters;

                action(cb);

                var v = src?.Select(i => new ArchiveItem(this, i)) ?? Items;
                if (cb.Result == OperationResult.OK) return;
                if (cb.Result == OperationResult.UserCancel) throw new OperationCanceledException();
                if (cb.Result == OperationResult.WrongPassword ||
                    cb.Result == OperationResult.DataError && v.Any(e => e.Encrypted))
                {
                    Password.Reset();
                    throw new EncryptionException();
                }
                throw cb.Exception ?? new System.IO.IOException($"{cb.Result}");
            }
        }

        #endregion

        #region Fields
        private readonly SevenZipLibrary _szip;
        private readonly ArchiveOpenCallback _open;
        private readonly IInArchive _core;
        private bool _disposed = false;
        #endregion
    }
}
