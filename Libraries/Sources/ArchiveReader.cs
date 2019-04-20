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
        private ArchiveReader(Format format, string src, PasswordQuery password, IO io)
        {
            _controller = new ArchiveReaderController(format, src, password, io);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source => _controller.Source;

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the format of archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format => _controller.Format;

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the collection of archived items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IReadOnlyList<ArchiveItem> Items => _controller.Items;

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
        public IEnumerable<string> Filters
        {
            get => _controller.Filters;
            set => _controller.Filters = value;
        }

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
        public void Extract(string directory, IProgress<Report> progress) =>
            _controller.Extract(directory, false, progress);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extract operations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract() => Extract(default(IProgress<Report>));

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extract operations.
        /// </summary>
        ///
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(IProgress<Report> progress) =>
            _controller.Extract(string.Empty, true, progress);

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
        protected override void Dispose(bool disposing) => _controller?.Dispose();

        #endregion

        #region Fields
        private readonly ArchiveReaderController _controller;
        #endregion
    }
}
