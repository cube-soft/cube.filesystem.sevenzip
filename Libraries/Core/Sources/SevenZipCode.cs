﻿/* ------------------------------------------------------------------------- */
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
/// SevenZipCode
///
/// <summary>
/// Specifies the operation code for compressing or extracting archives
/// via the 7-Zip library.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum SevenZipCode
{
    /// <summary>Operation success.</summary>
    Success = 0,
    /// <summary>Specified method does not support.</summary>
    UnsupportedMethod,
    /// <summary>Some errors exist in compressing or extracting data.</summary>
    DataError,
    /// <summary>CRC does not match.</summary>
    CrcError,
    /// <summary>Specified archive item is not available.</summary>
    Unavailable,
    /// <summary>Unexpected end position was found.</summary>
    UnexpectedEnd,
    /// <summary>Data was found after the end position.</summary>
    DataAfterEnd,
    /// <summary>Specified file is not an archive.</summary>
    IsNotArc,
    /// <summary>Some errors are detected in the archive header.</summary>
    HeadersError,
    /// <summary>Input password is not correct.</summary>
    WrongPassword,
    /// <summary>Unknown error.</summary>
    UnknownError = -1,
    /// <summary>Operation is canceled by user.</summary>
    Cancel = -2,
}
