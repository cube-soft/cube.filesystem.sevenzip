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
/// IOutArchive
///
/// <summary>
/// Represents an interface for creating an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000600A00000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IOutArchive
{
    /* --------------------------------------------------------------------- */
    ///
    /// UpdateItems
    ///
    /// <summary>
    /// Updates archive items
    /// </summary>
    ///
    /// <param name="stream">
    /// Stream pointer for writing the archive data
    /// </param>
    /// <param name="count">Number of archive items</param>
    /// <param name="callback">The Callback pointer</param>
    ///
    /// <returns>Zero if Ok</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    int UpdateItems(
        [MarshalAs(UnmanagedType.Interface)] ISequentialOutStream stream,
        uint count,
        [MarshalAs(UnmanagedType.Interface)] IArchiveUpdateCallback callback
    );

    /* --------------------------------------------------------------------- */
    ///
    /// GetFileTimeType
    ///
    /// <summary>
    /// Gets file time type(?)
    /// </summary>
    ///
    /// <param name="type">Type pointer</param>
    ///
    /* --------------------------------------------------------------------- */
    void GetFileTimeType(IntPtr type);
}
