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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ------------------------------------------------------------------------- */
///
/// CallbackExtension
///
/// <summary>
/// Provides extended methods of the CallbackBase class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class CallbackExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetException
    ///
    /// <summary>
    /// Creates a new instance of the SevenZipException class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Callback object.</param>
    /// <param name="code">Operation result.</param>
    ///
    /// <returns>Exception object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static SevenZipException GetException(this CallbackBase src, int code) =>
        src.Exceptions.Count > 0 ?
        new SevenZipException((SevenZipCode)code, src.Exceptions.Pop()) :
        new SevenZipException((SevenZipCode)code);

    /* --------------------------------------------------------------------- */
    ///
    /// CreateCancelException
    ///
    /// <summary>
    /// Creates a new instance of the OperationCanceledException class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Callback object.</param>
    ///
    /// <returns>Exception object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static OperationCanceledException GetCancelException(this CallbackBase src) =>
        src.Exceptions.Count > 0 ?
        new OperationCanceledException("", src.Exceptions.Pop()) :
        new OperationCanceledException();

    #endregion
}
