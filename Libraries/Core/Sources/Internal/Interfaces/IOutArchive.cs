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
    /* --------------------------------------------------------------------- */
    ///
    /// IOutArchive
    ///
    /// <summary>
    /// Represents an interface for creating an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600A00000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOutArchive
    {
        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int UpdateItems(
            [MarshalAs(UnmanagedType.Interface)] ISequentialOutStream stream,
            uint count,
            [MarshalAs(UnmanagedType.Interface)] IArchiveUpdateCallback callback
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileTimeType
        ///
        /// <summary>
        /// Gets file time type(?)
        /// </summary>
        ///
        /// <param name="type">Type pointer</param>
        ///
        /* ----------------------------------------------------------------- */
        void GetFileTimeType(IntPtr type);
    }
}
