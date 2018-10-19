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
using Cube.FileSystem.TestService;
using System;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveFixture
    ///
    /// <summary>
    /// Provides helper methods to test ArchiveReader and ArchiveWriter
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    abstract class ArchiveFixture : FileFixture
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveFixture
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveFixture class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveFixture() { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateReport
        ///
        /// <summary>
        /// Creates a new collection for ReportStatus.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IDictionary<ReportStatus, int> CreateReport() =>
            new Dictionary<ReportStatus, int>
            {
                { ReportStatus.Begin,    0 },
                { ReportStatus.End,      0 },
                { ReportStatus.Progress, 0 },
            };

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the Progress(Report) class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IProgress<Report> Create(IDictionary<ReportStatus, int> src) =>
            new SyncProgress<Report>(e => src[e.Status]++);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SyncProgress
        ///
        /// <summary>
        /// Provides functioanlity to execute the specified action
        /// as a synchronous operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class SyncProgress<T> : IProgress<T>
        {
            public SyncProgress(Action<T> e) { _do = e; }
            public void Report(T e) => _do(e);
            private readonly Action<T> _do;
        }

        #endregion
    }
}
