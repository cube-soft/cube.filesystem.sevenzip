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
/// ReportExtension
///
/// <summary>
/// Provides extended methods of the Report class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class ReportExtension
{
    /* --------------------------------------------------------------------- */
    ///
    /// GetRatio
    ///
    /// <summary>
    /// Gets the progress ratio within the range of [0, 1].
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static double GetRatio(this Report src) => Math.Min(
        src.TotalBytes > 0 ? src.Bytes / (double)src.TotalBytes : 0.0,
        src.TotalCount > 0 ? src.Count / (double)src.TotalCount : 0.0
    );
}
