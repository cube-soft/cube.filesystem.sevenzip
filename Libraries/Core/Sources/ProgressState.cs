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

/* ------------------------------------------------------------------------- */
///
/// ProgressState
///
/// <summary>
/// Specifies the progress state for compression or extraction.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum ProgressState
{
    /// <summary>Prepares operation for compression or extraction.</summary>
    Prepare,
    /// <summary>Starts compressing or extracting for the current file.</summary>
    Start,
    /// <summary>Compression or Extraction is in progress.</summary>
    Progress,
    /// <summary>Successfully compressed or decompressed.</summary>
    Success,
    /// <summary>Any erorrs occur when compressing or extracting.</summary>
    Failed,
}
