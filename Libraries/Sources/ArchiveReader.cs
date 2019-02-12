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
    public class ArchiveReader : DisposableBase
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
            this(src, password, new IO()) { }

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
            this(src, password, new IO()) { }

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string src, string password, IO io) :
            this(Formats.FromFile(src), src, new PasswordQuery(password), io) { }

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReader(string src, IQuery<string> password, IO io) :
            this(Formats.FromFile(src), src, new PasswordQuery(password), io) { }

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
        private ArchiveReader(Format format, string src, PasswordQuery query, IO io)
        {
            if (format == Format.Unknown) throw new NotSupportedException();

            var asr   = new ArchiveStreamReader(io.OpenRead(src));
            _core     = new SevenZipLibrary();
            _query    = query;
            _callback = new ArchiveOpenCallback(src, asr, io) { Password = _query };
            _module   = _core.GetInArchive(format);
            _module.Open(asr, IntPtr.Zero, _callback);

            IO     = io;
            Format = format;
            Source = src;
            Items  = new ReadOnlyArchiveList(_module, format, src, _query, io);
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
        /// Source
        ///
        /// <summary>
        /// Gets the path of archive.
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
        /// Extracts files and saves to the specified directory.
        /// </summary>
        ///
        /// <param name="directory">Save directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts files and saves to the specified directory.
        /// </summary>
        ///
        /// <param name="directory">Save directory.</param>
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, IProgress<Report> progress)
        {
            using (var cb = Create(directory, progress))
            {
                _module.Extract(null, uint.MaxValue, 0, cb);
                Terminate(cb.Result, cb.Exception);
            }
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
            _callback.Dispose();
            _module.Close();
            _core.Dispose();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveExtractCallback class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveExtractCallback Create(string directory, IProgress<Report> progress) =>
            new ArchiveExtractCallback(Source, directory, Items, IO)
        {
            TotalCount = Items.Count,
            Password   = _query,
            Progress   = progress,
            Filters    = Filters,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateEncryptionException
        ///
        /// <summary>
        /// Resets the password and returns a new instance of the
        /// EncryptionException class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private EncryptionException CreateEncryptionException()
        {
            _query.Reset();
            return new EncryptionException();
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
            if (src == OperationResult.WrongPassword) throw CreateEncryptionException();
            if (src == OperationResult.DataError && Items.Any(x => x.Encrypted)) throw CreateEncryptionException();
            if (error != null) throw error;
            else throw new System.IO.IOException($"{src}");
        }

        #endregion

        #region Fields
        private readonly PasswordQuery _query;
        private readonly ArchiveOpenCallback _callback;
        private readonly SevenZipLibrary _core;
        private readonly IInArchive _module;
        #endregion
    }
}
