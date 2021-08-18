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
    /// IInArchive
    ///
    /// <summary>
    /// Represents an interface for processing an existing archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600600000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInArchive
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Opens archive for reading.
        /// </summary>
        ///
        /// <param name="stream">Source stream.</param>
        /// <param name="max">Maximum header length.</param>
        /// <param name="callback">Callback object.</param>
        ///
        /// <remarks>
        /// If the value of the argument max is set to null, 7-Zip will
        /// attempt to parse the header length and use this function.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Open(
            IInStream stream,
            IntPtr max, // [In] ref ulong max
            [MarshalAs(UnmanagedType.Interface)] IArchiveOpenCallback callback
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Closes the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Close();

        /* ----------------------------------------------------------------- */
        ///
        /// GetNumberOfItems
        ///
        /// <summary>
        /// Gets the number of files in the archive file table  .
        /// </summary>
        ///
        /// <returns>The number of files in the archive</returns>
        ///
        /* ----------------------------------------------------------------- */
        uint GetNumberOfItems();

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// Retrieves specific property data.
        /// </summary>
        ///
        /// <param name="index">File index in the archive file table</param>
        /// <param name="pid">Property ID</param>
        /// <param name="value">Property variant value</param>
        ///
        /* ----------------------------------------------------------------- */
        void GetProperty(uint index, ItemPropId pid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts files from the opened archive.
        /// </summary>
        ///
        /// <param name="indexes">
        /// Indexes of files to be extracted (must be sorted).
        /// </param>
        /// <param name="count">0xFFFFFFFF means all files.</param>
        /// <param name="test">test != 0 means test mode.</param>
        /// <param name="callback">Callback for operations handling.</param>
        ///
        /// <returns>0 if success</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Extract(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] indexes,
            uint count,
            int test,
            [MarshalAs(UnmanagedType.Interface)] IArchiveExtractCallback callback
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetArchiveProperty
        ///
        /// <summary>
        /// Gets archive property data
        /// </summary>
        ///
        /// <param name="aid">Archive property ID</param>
        /// <param name="value">Property value</param>
        ///
        /* ----------------------------------------------------------------- */
        void GetArchiveProperty(ArchivePropId aid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
        ///
        /// GetNumberOfProperties
        ///
        /// <summary>
        /// Gets the number of properties
        /// </summary>
        ///
        /// <returns>The number of properties</returns>
        ///
        /* ----------------------------------------------------------------- */
        uint GetNumberOfProperties();

        /* ----------------------------------------------------------------- */
        ///
        /// GetPropertyInfo
        ///
        /// <summary>
        /// Gets property information
        /// </summary>
        ///
        /// <param name="index">Item index</param>
        /// <param name="name">Name</param>
        /// <param name="pid">Property identificator</param>
        /// <param name="type">Variant type</param>
        ///
        /* ----------------------------------------------------------------- */
        void GetPropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropId pid,
            out ushort type
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetNumberOfArchiveProperties
        ///
        /// <summary>
        /// Gets the number of archive properties
        /// </summary>
        ///
        /// <returns>The number of archive properties</returns>
        ///
        /* ----------------------------------------------------------------- */
        uint GetNumberOfArchiveProperties();

        /* ----------------------------------------------------------------- */
        ///
        /// GetArchivePropertyInfo
        ///
        /// <summary>
        /// Gets the archive property information
        /// </summary>
        ///
        /// <param name="index">Item index</param>
        /// <param name="name">Name</param>
        /// <param name="pid">Property identificator</param>
        /// <param name="type">Variant type</param>
        ///
        /* ----------------------------------------------------------------- */
        void GetArchivePropertyInfo(
            uint index,
            [MarshalAs(UnmanagedType.BStr)] out string name,
            out ItemPropId pid,
            out ushort type
        );
    }
}
