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
/// IArchiveOpenCallback
///
/// <summary>
/// Represents the interface for decompressing an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000600100000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IArchiveOpenCallback
{
    /* --------------------------------------------------------------------- */
    ///
    /// SetTotal
    ///
    /// <summary>
    /// Gets the total size of the compressed file upon decompression.
    /// </summary>
    ///
    /// <param name="count">Total number of files.</param>
    /// <param name="bytes">Total compressed bytes.</param>
    ///
    /// <remarks>
    /// IntPtr is used instead of ref ulong because 7z.dll is often set
    /// to null. To get the value when non-null, use Marshal.ReadInt64.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    void SetTotal(IntPtr /* ref ulong */ count, IntPtr /* ref ulong */ bytes);

    /* --------------------------------------------------------------------- */
    ///
    /// SetCompleted
    ///
    /// <summary>
    /// Get the size of the stream ready to be read.
    /// </summary>
    ///
    /// <param name="count">Number of files.</param>
    /// <param name="bytes">Completed bytes.</param>
    ///
    /* --------------------------------------------------------------------- */
    void SetCompleted(IntPtr /* ref ulong */ count, IntPtr /* ref ulong */ bytes);
}
