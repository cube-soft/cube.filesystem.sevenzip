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
    /// <param name="reason">Error reason.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipException(ArchiveErrorReason reason) :
        base(reason.ToString()) => Reason = reason;

    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipException
    ///
    /// <summary>
    /// Initializes a new instance of the SevenZipException class with the
    /// specified arguments.
    /// </summary>
    /// 
    /// <param name="reason">Error reason.</param>
    /// <param name="inner">Inner exception.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipException(ArchiveErrorReason reason, Exception inner) :
        base(reason.ToString(), inner) => Reason = reason;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Reason
    ///
    /// <summary>
    /// Gets the error reason.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveErrorReason Reason { get; }

    #endregion
}
