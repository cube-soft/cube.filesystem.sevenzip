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
using System.Runtime.InteropServices;

/* ------------------------------------------------------------------------- */
///
/// ICryptoGetTextPassword2
///
/// <summary>
/// Represents an interface for entering a password when compressing
/// an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000500110000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ICryptoGetTextPassword2
{
    /* --------------------------------------------------------------------- */
    ///
    /// CryptoGetTextPassword2
    ///
    /// <summary>
    /// Gets the password to be set for the compressed file.
    /// </summary>
    ///
    /// <param name="enabled">Password is enabled or not.</param>
    /// <param name="password">Password string.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode CryptoGetTextPassword2(ref int enabled, [MarshalAs(UnmanagedType.BStr)] out string password);
}
