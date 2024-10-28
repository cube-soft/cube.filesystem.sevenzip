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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// AccessException
///
/// <summary>
/// Represents the exception where access to the file failed.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class AccessException : IOException
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// AccessException
    ///
    /// <summary>
    /// Initializes a new instance with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path that failed to be accessed.</param>
    ///
    /* --------------------------------------------------------------------- */
    public AccessException(string src) :
        base($"{src.Quote()} access failed") => FileName = src;

    /* --------------------------------------------------------------------- */
    ///
    /// AccessException
    ///
    /// <summary>
    /// Initializes a new instance with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path that failed to be accessed.</param>
    /// <param name="inner">Inner exception.</param>
    ///
    /* --------------------------------------------------------------------- */
    public AccessException(string src, Exception inner) :
        base($"{src.Quote()} access failed", inner) => FileName = src;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// FileName
    ///
    /// <summary>
    /// Gets the path that failed to be accessed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string FileName { get; }

    #endregion
}
