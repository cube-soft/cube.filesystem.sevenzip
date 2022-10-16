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
using System.IO;

/* ------------------------------------------------------------------------- */
///
/// SevenZipException
///
/// <summary>
/// Represents the exception that an error occurs in the 7-Zip library.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class SevenZipException : IOException
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipException
    ///
    /// <summary>
    /// Initializes a new instance of the SevenZipException class with the
    /// specified error reason.
    /// </summary>
    ///
    /// <param name="code">Error code.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipException(SevenZipErrorCode code) :
        base(code.ToString()) => Code = code;

    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipException
    ///
    /// <summary>
    /// Initializes a new instance of the SevenZipException class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="code">Error code.</param>
    /// <param name="inner">Inner exception.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipException(SevenZipErrorCode code, Exception inner) :
        base(code.ToString(), inner) => Code = code;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Code
    ///
    /// <summary>
    /// Gets the error code.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipErrorCode Code { get; }

    #endregion
}
