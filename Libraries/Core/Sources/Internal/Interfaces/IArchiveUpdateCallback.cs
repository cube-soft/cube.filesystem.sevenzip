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
    /// IArchiveUpdateCallback
    ///
    /// <summary>
    /// Represents an interface for updating the file contents in an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600800000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveUpdateCallback
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        ///
        /// <summary>
        /// Notifies the total bytes of target files.
        /// </summary>
        ///
        /// <param name="bytes">Total bytes of target files.</param>
        ///
        /* ----------------------------------------------------------------- */
        void SetTotal(ulong bytes);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// Notifies the bytes to be archived.
        /// </summary>
        ///
        /// <param name="bytes">Bytes to be archived.</param>
        ///
        /* ----------------------------------------------------------------- */
        void SetCompleted(ref ulong bytes);

        /* ----------------------------------------------------------------- */
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
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetUpdateItemInfo(
            uint index,
            ref int newdata,
            ref int newprop,
            ref uint indexInArchive
        );

        /* ----------------------------------------------------------------- */
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
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetProperty(uint index, ItemPropId pid, ref PropVariant value);

        /* ----------------------------------------------------------------- */
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
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetStream(
            uint index,
            [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialInStream stream
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// Sets the specified operation result.
        /// </summary>
        ///
        /// <param name="result">Operation result.</param>
        ///
        /* ----------------------------------------------------------------- */
        void SetOperationResult(OperationResult result);

        /* ----------------------------------------------------------------- */
        ///
        /// EnumProperties
        ///
        /// <summary>
        /// EnumProperties 7-zip internal function.
        /// </summary>
        ///
        /// <param name="enumerator">The enumerator pointer.</param>
        ///
        /* ----------------------------------------------------------------- */
        long EnumProperties(IntPtr enumerator);
    }
}
