/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
