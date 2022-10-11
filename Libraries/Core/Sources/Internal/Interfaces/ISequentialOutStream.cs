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
/// ISequentialOutStream
///
/// <summary>
/// Represents an interface for processing the output stream of an
/// archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000300020000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISequentialOutStream
{
    /* --------------------------------------------------------------------- */
    ///
    /// Write
    ///
    /// <summary>
    /// Writes data to unpacked file stream
    /// </summary>
    ///
    /// <param name="data">Array of bytes available for reading</param>
    /// <param name="size">Array size</param>
    /// <param name="processedSize">Processed data size</param>
    ///
    /// <returns>S_OK if success</returns>
    ///
    /// <remarks>
    /// If size != 0, return value is S_OK and (*processedSize == 0),
    /// then there are no more bytes in stream.
    /// If (size > 0) and there are bytes in stream,
    /// this function must read at least 1 byte.
    /// This function is allowed to rwrite less than "size" bytes.
    /// You must call Write function in loop, if you need exact
    /// amount of data.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    int Write(
        [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
        uint size,
        IntPtr processedSize
    );
}
