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
/// OperationResult
///
/// <summary>
/// Specifies the operation result.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum ArchiveErrorReason
{
    /// <summary>No error is detected.</summary>
    OK = 0,
    /// <summary>Specified method does not support.</summary>
    UnsupportedMethod,
    /// <summary>Some errors exist in compressing or extracting data.</summary>
    DataError,
    /// <summary>CRC does not match.</summary>
    CrcError,
    /// <summary></summary>
    Unavailable,
    /// <summary></summary>
    UnexpectedEnd,
    /// <summary></summary>
    DataAfterEnd,
    /// <summary></summary>
    IsNotArc,
    /// <summary></summary>
    HeadersError,
    /// <summary>Input password is not correct.</summary>
    WrongPassword,
    /// <summary>Operation is canceled by user.</summary>
    UserCancel = -2,
}
