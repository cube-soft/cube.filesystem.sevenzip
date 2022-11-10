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
/// Report
///
/// <summary>
/// Represetns the progress information for compression or extraction.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class Report
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Cancel
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the process was canceled
    /// by the user.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Cancel { get; set; } = false;

    /* --------------------------------------------------------------------- */
    ///
    /// State
    ///
    /// <summary>
    /// Gets or sets the current progress state.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ProgressState State { get; set; } = ProgressState.Progress;

    /* --------------------------------------------------------------------- */
    ///
    /// Target
    ///
    /// <summary>
    /// Gets or sets the file information that is currently processing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Entity Target { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Exception
    ///
    /// <summary>
    /// Gets or sets an exception object. The property is set when the value
    /// of State is Failed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Exception Exception { get; set; }

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

    #endregion
}
