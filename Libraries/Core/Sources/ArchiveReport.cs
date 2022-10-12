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
namespace Cube.FileSystem.SevenZip;

using System;

/* ------------------------------------------------------------------------- */
///
/// ArchiveReport
///
/// <summary>
/// Represetns the progress of compression and extraction.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class ArchiveReport
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Status
    ///
    /// <summary>
    /// Gets or sets the current status.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveStatus Status { get; set; } = ArchiveStatus.Progress;

    /* --------------------------------------------------------------------- */
    ///
    /// Exception
    ///
    /// <summary>
    /// Gets or sets an exception object. The property is set when the value
    /// of Status is Failed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Exception Exception { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Handled
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the user has performed the
    /// processing according to the report.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Handled { get; set; } = false;

    /* --------------------------------------------------------------------- */
    ///
    /// Current
    ///
    /// <summary>
    /// Gets or sets the file information that is currently processing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Entity Current { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Count
    ///
    /// <summary>
    /// Gets or sets the number of processed files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long Count { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// TotalCount
    ///
    /// <summary>
    /// Gets or sets the number of processing target files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long TotalCount { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Bytes
    ///
    /// <summary>
    /// Gets or sets the number of processed bytes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long Bytes { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// TotalBytes
    ///
    /// <summary>
    /// Gets or sets the number of processing target bytes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long TotalBytes { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Ratio
    ///
    /// <summary>
    /// Gets the progress ratio within the range of [0, 1].
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public double Ratio => Math.Min(
        TotalBytes > 0 ? Bytes / (double)TotalBytes : 0.0,
        TotalCount > 0 ? Count / (double)TotalCount : 0.0
    );

    #endregion
}
