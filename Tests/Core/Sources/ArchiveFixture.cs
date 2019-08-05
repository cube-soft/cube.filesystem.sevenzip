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
using Cube.Tests;
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
