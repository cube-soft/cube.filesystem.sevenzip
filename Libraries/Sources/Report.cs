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
namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Represents information of the archived or extracted report.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Report
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Status
        ///
        /// <summary>
        /// Gets or sets the reporting status.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ReportStatus Status { get; set; } = ReportStatus.Progress;

        /* ----------------------------------------------------------------- */
        ///
        /// Current
        ///
        /// <summary>
        /// Gets or sets the file information that is currently processing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Information Current { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets or sets the number of processed files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Count { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalCount
        ///
        /// <summary>
        /// Gets or sets the number of processing target files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long TotalCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Bytes
        ///
        /// <summary>
        /// Gets or sets the number of processed bytes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Bytes { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalBytes
        ///
        /// <summary>
        /// Gets or sets the number of processing target bytes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long TotalBytes { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Ratio
        ///
        /// <summary>
        /// Gets the progress ratio within the range of [0, 1].
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public double Ratio => TotalBytes > 0 ? Bytes / (double)TotalBytes : 0.0;

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ReportStatus
    ///
    /// <summary>
    /// Specifies status of the provided report.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum ReportStatus
    {
        /// <summary>Current file begins to be archived or extracted</summary>
        Begin,
        /// <summary>Current file ends to be archived or extracted</summary>
        End,
        /// <summary>Archiving or Extracting operation is in progress.</summary>
        Progress,
    }
}
