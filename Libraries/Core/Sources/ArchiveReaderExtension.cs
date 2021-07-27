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
using System.Collections.Generic;
using System.Linq;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReaderExtension
    ///
    /// <summary>
    /// Provides extended methods of the ArchiveReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ArchiveReaderExtension
    {
        #region Extract

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts all files and saves them in the specified directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest) =>
            src.Extract(dest, null, null, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts all files and saves them in the specified directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        ///
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest, IProgress<Report> progress) =>
            src.Extract(dest, null, null, progress);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts all files except those matching the specified filters
        /// and saves them in the specified directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        ///
        /// <param name="filters">
        /// List of paths to skip decompressing files or folders that match
        /// the contained values.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest, IEnumerable<string> filters) =>
            src.Extract(dest, null, filters, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts all files except those matching the specified filters
        /// and saves them in the specified directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        ///
        /// <param name="filters">
        /// List of paths to skip decompressing files or folders that match
        /// the contained values.
        /// </param>
        ///
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest,
            IEnumerable<string> filters, IProgress<Report> progress) =>
            src.Extract(dest, null, filters, progress);

        #endregion

        #region Extract (ArchiveEntity)

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified item and saves them in the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        /// <param name="item">Archived item to extract.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest, ArchiveEntity item) =>
            src.Extract(dest, item, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified item and saves them in the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        /// <param name="item">Archived item to extract.</param>
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest,
            ArchiveEntity item, IProgress<Report> progress) =>
            src.Extract(dest, new[] { item }, progress);

        #endregion

        #region Extract (IEnumerable<ArchiveEntity>)

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified items and saves them in the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        /// <param name="items">List of archived items to extract.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest, IEnumerable<ArchiveEntity> items) =>
            src.Extract(dest, items, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified items and saves them in the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        /// <param name="dest">
        /// Path of the directory to save. If the parameter is set to null
        /// or empty, the method invokes as a test mode.
        /// </param>
        /// <param name="items">List of archived items to extract.</param>
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Extract(this ArchiveReader src, string dest,
            IEnumerable<ArchiveEntity> items, IProgress<Report> progress)
        {
            var indices = items?.Select(e => (uint)e.Index)?.ToArray();
            if (indices is null) return;
            src.Extract(dest, indices, null, progress);
        }

        #endregion

        #region Test

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests to extract all files. The extracted data will be discarded.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test(this ArchiveReader src) => src.Test(null);

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests to extract all files except those matching the specified
        /// filters. The extracted data will be discarded.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /// <param name="filters">
        /// List of paths to skip decompressing files or folders that match
        /// the contained values.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test(this ArchiveReader src, IEnumerable<string> filters) =>
            src.Test(filters, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests to extract all files except those matching the specified
        /// filters. The extracted data will be discarded.
        /// </summary>
        ///
        /// <param name="src">Source reader object.</param>
        ///
        /// <param name="filters">
        /// List of paths to skip decompressing files or folders that match
        /// the contained values.
        /// </param>
        ///
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test(this ArchiveReader src,
            IEnumerable<string> filters, IProgress<Report> progress) =>
            src.Extract(null, null, filters, progress);

        #endregion
    }
}
