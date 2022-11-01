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
/// IArchiveExtractCallback
///
/// <summary>
/// Represents the interface for decompressing each file in an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000600200000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IArchiveExtractCallback
{
    /* --------------------------------------------------------------------- */
    ///
    /// SetTotal
    ///
    /// <summary>
    /// Sets the number of bytes when all of the specified items have
    /// been extracted.
    /// </summary>
    ///
    /// <param name="bytes">Number of bytes.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode SetTotal(ulong bytes);

    /* --------------------------------------------------------------------- */
    ///
    /// SetCompleted
    ///
    /// <summary>
    /// Sets the extracted bytes.
    /// </summary>
    ///
    /// <param name="bytes">Number of bytes.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode SetCompleted(IntPtr bytes);

    /* --------------------------------------------------------------------- */
    ///
    /// GetStream
    ///
    /// <summary>
    /// Gets the stream to save the extracted data.
    /// </summary>
    ///
    /// <param name="index">Index of the archive.</param>
    /// <param name="stream">Output stream.</param>
    /// <param name="mode">Operation mode.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode GetStream(
        uint index,
        [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialOutStream stream,
        AskMode mode
    );

    /* --------------------------------------------------------------------- */
    ///
    /// PrepareOperation
    ///
    /// <summary>
    /// Invokes just before extracting a file.
    /// </summary>
    ///
    /// <param name="mode">Operation mode.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode PrepareOperation(AskMode mode);

    /* --------------------------------------------------------------------- */
    ///
    /// SetOperationResult
    ///
    /// <summary>
    /// Sets the extracted result.
    /// </summary>
    ///
    /// <param name="code">Operation result.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode SetOperationResult(SevenZipCode code);
}
