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
    #region ISequentialInStream

    /* --------------------------------------------------------------------- */
    ///
    /// ISequentialInStream
    ///
    /// <summary>
    /// Represents an interface for processing the input stream of an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300010000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialInStream
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// Writes data to 7-zip packer
        /// </summary>
        ///
        /// <param name="data">Array of bytes available for writing</param>
        /// <param name="size">Array size</param>
        ///
        /// <returns>S_OK if success</returns>
        ///
        /// <remarks>
        /// If (size > 0) and there are bytes in stream,
        /// this function must read at least 1 byte.
        /// This function is allowed to read less than "size" bytes.
        /// You must call Read function in loop, if you need exact
        /// amount of data.
        /// </remarks>
        /* ----------------------------------------------------------------- */
        int Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
        );
    }

    #endregion

    #region IInStream

    /* --------------------------------------------------------------------- */
    ///
    /// IInStream
    ///
    /// <summary>
    /// Represents an interface for processing the input stream of an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000300030000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInStream
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Read
        ///
        /// <summary>
        /// Read routine
        /// </summary>
        ///
        /// <param name="data">Array of bytes to set</param>
        /// <param name="size">Array size</param>
        ///
        /// <returns>Zero if Ok</returns>
        ///
        /* ----------------------------------------------------------------- */
        int Read(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data,
            uint size
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
    }

    #endregion
}
