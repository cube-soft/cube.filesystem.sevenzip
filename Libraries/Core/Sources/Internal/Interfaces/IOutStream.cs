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
using System.IO;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    #region ISequentialOutStream

    /* --------------------------------------------------------------------- */
    ///
    /// ISequentialOutStream
    ///
    /// <summary>
    /// Represents an interface for processing the output stream of an
    /// archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300020000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialOutStream
    {
        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize
        );
    }

    #endregion

    #region IOutStream

    /* --------------------------------------------------------------------- */
    ///
    /// IOutStream
    ///
    /// <summary>
    /// Represents an interface for processing the output stream of an
    /// archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300040000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOutStream
    {
        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size,
            IntPtr processedSize
        );

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        void Seek(long offset, SeekOrigin origin, IntPtr newpos);

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int SetSize(long size);
    }

    #endregion
}
