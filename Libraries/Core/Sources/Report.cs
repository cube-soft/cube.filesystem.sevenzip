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
        public Entity Current { get; set; }

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
