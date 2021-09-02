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
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Provides functionality to extract an archived file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ArchiveReader : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveReader class with
        /// the specified path.
        /// </summary>
        ///
        /// <param name="src">Path of the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string src) : this(src, string.Empty) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveReader class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the archive.</param>
        /// <param name="password">Password of the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string src, string password) :
            this(Formatter.FromFile(src), src, new PasswordQuery(password)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveReader class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the archive.</param>
        /// <param name="password">Query object to get password.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string src, IQuery<string> password) :
            this(Formatter.FromFile(src), src, new PasswordQuery(password)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveReader
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveReader class with
        /// the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveReader(Format format, string src, PasswordQuery password)
        {
            if (format == Format.Unknown) throw new UnknownFormatException();

            Source = src;
            Format = format;
            _password = password;

            var ss = new ArchiveStreamReader(Io.Open(src));
            _callback = new OpenCallback(src, ss) { Password = password };
            _core = _7zip.GetInArchive(format);
            _ = _core.Open(ss, IntPtr.Zero, _callback);

            var n = (int)Math.Max(_core.GetNumberOfItems(), 1);
            Items = new ArchiveCollection(_core, n, src);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the archive path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

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
        /// Items
        ///
        /// <summary>
        /// Gets the collection of archived items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IReadOnlyList<ArchiveEntity> Items { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the files corresponding to the specified indices except
        /// those matching the specified filters, and saves them in the
        /// specified directory.
        /// </summary>
        ///
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        ///
        /// <param name="src">Source indices to extract.</param>
        ///
        /// <param name="filters">
        /// List of paths to skip decompressing files or folders that match
        /// the contained values.
        /// </param>
        ///
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string dest, uint[] src, IEnumerable<string> filters, IProgress<Report> progress)
        {
            using var cb = src != null ?
                      new ExtractCallback(this, src.Select(i => (int)i), src.Length, dest) :
                      new ExtractCallback(this, dest);

            cb.Password = _password;
            cb.Progress = progress;
            cb.Filters  = filters ?? Enumerable.Empty<string>();

            var count = (uint?)src?.Length ?? uint.MaxValue;
            var test  = dest.HasValue() ? 0 : 1;
            _ = _core.Extract(src, count , test, cb);

            if (cb.Result == OperationResult.OK) return;
            if (cb.Result == OperationResult.UserCancel) throw new OperationCanceledException();
            if (cb.Result == OperationResult.WrongPassword ||
                cb.Result == OperationResult.DataError && IsEncrypted(src))
            {
                _password.Reset();
                throw new EncryptionException();
            }
            throw cb.Exception ?? new System.IO.IOException($"{cb.Result}");
        }

        #endregion

        #region Implementations

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
        protected override void Dispose(bool disposing)
        {
            _core?.Close();
            _callback?.Dispose();
            _7zip?.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsEncrypted
        ///
        /// <summary>
        /// Determines if any of the specified items are encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsEncrypted(IEnumerable<uint> indices) =>
            (indices?.Select(i => Items[(int)i]) ?? Items).Any(e => e.Encrypted);

        #endregion

        #region Fields
        private readonly SevenZipLibrary _7zip = new();
        private readonly IInArchive _core;
        private readonly OpenCallback _callback;
        private readonly PasswordQuery _password;
        #endregion
    }
}
