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
        #region Save

        /* ----------------------------------------------------------------- */
        ///
        /// Save
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
        public static void Save(this ArchiveReader src, string dest, ArchiveEntity item) =>
            src.Save(dest, item, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
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
        public static void Save(this ArchiveReader src,
            string dest, ArchiveEntity item, IProgress<Report> progress) =>
            src.Save(dest, new[] { (uint)item.Index }, progress);

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
        /// <param name="progress">Progress object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test(this ArchiveReader src, IProgress<Report> progress) =>
            src.Save(null, null, progress);

        #endregion
    }
}
