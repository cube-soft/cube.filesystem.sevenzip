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
using System.Runtime.InteropServices;

/* ------------------------------------------------------------------------- */
///
/// IOutStream
///
/// <summary>
/// Represents an interface for processing the output stream of an
/// archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000300040000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IOutStream
{
    /* --------------------------------------------------------------------- */
    ///
    /// Write
    ///
    /// <summary>
    /// Write routine
    /// </summary>
    ///
    /// <param name="data">Array of bytes to get</param>
    /// <param name="size">Array size</param>
    /// <param name="processedSize">Processed size</param>
    ///
    /// <returns>Zero if Ok</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    int Write(
        [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
        uint size,
        IntPtr processedSize
    );

    /* --------------------------------------------------------------------- */
    ///
    /// Seek
    ///
    /// <summary>
    /// Seek routine
    /// </summary>
    ///
    /// <param name="offset">Offset value</param>
    /// <param name="origin">Seek origin value</param>
    /// <param name="newpos">New position pointer</param>
    ///
    /* --------------------------------------------------------------------- */
    void Seek(long offset, SeekOrigin origin, IntPtr newpos);

    /* --------------------------------------------------------------------- */
    ///
    /// SetSize
    ///
    /// <summary>
    /// Set size routine
    /// </summary>
    ///
    /// <param name="size">New size value</param>
    ///
    /// <returns>Zero if Ok</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    int SetSize(long size);
}
