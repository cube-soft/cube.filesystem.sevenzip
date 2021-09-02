﻿/* ------------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    ///
    /// IArchiveExtractCallback
    ///
    /// <summary>
    /// Represents the interface for decompressing each file in an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("23170F69-40C1-278A-0000-000600200000")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IArchiveExtractCallback
    {
        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        void SetTotal(ulong bytes);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// Sets the extracted bytes.
        /// </summary>
        ///
        /// <param name="bytes">Number of bytes.</param>
        ///
        /// <remarks>
        /// When the IInArchive.Extract method is invoked multiple times,
        /// the values obtained by SetTotal and SetCompleted differ
        /// depending on the Format. ArchiveExtractCallback normalizes the
        /// values to eliminate the differences between formats.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        void SetCompleted(ref ulong bytes);

        /* ----------------------------------------------------------------- */
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
        /// <returns>Operation result.</returns>
        ///
        /// <remarks>
        /// If you want to skip the decompression, return a value other than
        /// OperationResult.OK.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [PreserveSig]
        int GetStream(
            uint index,
            [Out, MarshalAs(UnmanagedType.Interface)] out ISequentialOutStream stream,
            AskMode mode
        );

        /* ----------------------------------------------------------------- */
        ///
        /// PrepareOperation
        ///
        /// <summary>
        /// Invokes just before extracting a file.
        /// </summary>
        ///
        /// <param name="mode">Operation mode.</param>
        ///
        /* ----------------------------------------------------------------- */
        void PrepareOperation(AskMode mode);

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// Sets the extracted result.
        /// </summary>
        ///
        /// <param name="result">Operation result.</param>
        ///
        /* ----------------------------------------------------------------- */
        void SetOperationResult(OperationResult result);
    }
}
