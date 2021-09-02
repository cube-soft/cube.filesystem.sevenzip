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
using System;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    #region IArchiveOpenCallback

    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveOpenCallback
    ///
    /// <summary>
    /// Represents the interface for decompressing an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600100000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveOpenCallback
    {
        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        void SetTotal(IntPtr /* ref ulong */ count, IntPtr /* ref ulong */ bytes);

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        void SetCompleted(IntPtr /* ref ulong */ count, IntPtr /* ref ulong */ bytes);
    }

    #endregion

    #region IArchiveOpenVolumeCallback

    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveOpenVolumeCallback
    ///
    /// <summary>
    /// Represents an interface for decompressing a split archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600300000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveOpenVolumeCallback
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// Gets the property of the compressed file for the specified ID.
        /// </summary>
        ///
        /// <param name="pid">Property ID</param>
        /// <param name="value">
        /// Value corresponding to the property ID.
        /// </param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetProperty(ItemPropId pid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// Get the stream corresponding to the volume to be read.
        /// </summary>
        ///
        /// <param name="name">Volume name.</param>
        /// <param name="stream">Target input stream.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetStream(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            [Out, MarshalAs(UnmanagedType.Interface)] out IInStream stream
        );
    }

    #endregion
}
