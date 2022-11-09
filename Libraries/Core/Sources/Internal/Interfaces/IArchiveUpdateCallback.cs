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
/// IArchiveUpdateCallback
///
/// <summary>
/// Represents an interface for updating the file contents in an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[ComImport]
[Guid("23170F69-40C1-278A-0000-000600800000")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IArchiveUpdateCallback
{
    /* --------------------------------------------------------------------- */
    ///
    /// SetTotal
    ///
    /// <summary>
    /// Notifies the total bytes of target files.
    /// </summary>
    ///
    /// <param name="bytes">Total bytes of target files.</param>
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
    /// Notifies the bytes to be archived.
    /// </summary>
    ///
    /// <param name="bytes">Bytes to be archived.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode SetCompleted(IntPtr bytes);

    /* --------------------------------------------------------------------- */
    ///
    /// GetUpdateItemInfo
    ///
    /// <summary>
    /// Gets information of updating item.
    /// </summary>
    ///
    /// <param name="index">Index of the item.</param>
    /// <param name="newdata">1 if new, 0 if not</param>
    /// <param name="newprop">1 if new, 0 if not</param>
    /// <param name="indexInArchive">-1 if doesn't matter</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode GetUpdateItemInfo(uint index, ref int newdata, ref int newprop, ref uint indexInArchive);

    /* --------------------------------------------------------------------- */
    ///
    /// GetProperty
    ///
    /// <summary>
    /// Gets the property information according to the specified
    /// arguments.
    /// </summary>
    ///
    /// <param name="index">Index of the target file.</param>
    /// <param name="pid">Property ID to get information.</param>
    /// <param name="value">Value of the specified property.</param>
    ///
    /// <returns>Operation result</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode GetProperty(uint index, ItemPropId pid, ref PropVariant value);

    /* --------------------------------------------------------------------- */
    ///
    /// GetStream
    ///
    /// <summary>
    /// Gets the stream according to the specified arguments.
    /// </summary>
    ///
    /// <param name="index">Index of the target file.</param>
    /// <param name="stream">Stream to read data.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode GetStream(uint index, [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialInStream stream);

    /* --------------------------------------------------------------------- */
    ///
    /// SetOperationResult
    ///
    /// <summary>
    /// Sets the specified operation result.
    /// </summary>
    ///
    /// <param name="result">Operation result.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    [PreserveSig]
    SevenZipCode SetOperationResult(SevenZipCode result);

    /* --------------------------------------------------------------------- */
    ///
    /// EnumProperties
    ///
    /// <summary>
    /// EnumProperties 7-zip internal function.
    /// </summary>
    ///
    /// <param name="enumerator">The enumerator pointer.</param>
    ///
    /* --------------------------------------------------------------------- */
    long EnumProperties(IntPtr enumerator);
}
