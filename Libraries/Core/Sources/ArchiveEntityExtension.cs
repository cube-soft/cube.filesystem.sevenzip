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
    /// ArchiveEntityExtension
    ///
    /// <summary>
    /// Provides extended methods of the ArchiveEntity class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ArchiveEntityExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// Creates the directory.
        /// </summary>
        ///
        /// <param name="src">Information of the archived item.</param>
        /// <param name="root">Path of the root directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void CreateDirectory(this ArchiveEntity src, string root)
        {
            if (!src.IsDirectory) return;
            var path = Io.Combine(root, src.FullName);
            Io.CreateDirectory(path);
            SetAttributes(src, root);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// Sets attributes, creation time, last written time, and last
        /// accessed time to the extracted file or directory.
        /// </summary>
        ///
        /// <param name="src">Information of the archived item.</param>
        /// <param name="root">Path of the root directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetAttributes(this ArchiveEntity src, string root)
        {
            var path = Io.Combine(root, src.FullName);
            if (!Io.Exists(path)) return;

            SetCreationTime(src, path);
            SetLastWriteTime(src, path);
            SetLastAccessTime(src, path);
            Io.SetAttributes(path, src.Attributes);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetCreationTime
        ///
        /// <summary>
        /// Sets the creation time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetCreationTime(ArchiveEntity src, string path)
        {
            var time = src.CreationTime  != DateTime.MinValue ? src.CreationTime :
                       src.LastWriteTime != DateTime.MinValue ? src.LastWriteTime :
                       src.LastAccessTime;
            if (time != DateTime.MinValue) Io.SetCreationTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastWriteTime
        ///
        /// <summary>
        /// Sets the last written time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetLastWriteTime(ArchiveEntity src, string path)
        {
            var time = src.LastWriteTime  != DateTime.MinValue ? src.LastWriteTime :
                       src.LastAccessTime != DateTime.MinValue ? src.LastAccessTime :
                       src.CreationTime;
            if (time != DateTime.MinValue) Io.SetLastWriteTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastAccessTime
        ///
        /// <summary>
        /// Sets the last accessed time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetLastAccessTime(ArchiveEntity src, string path)
        {
            var time = src.LastAccessTime != DateTime.MinValue ? src.LastAccessTime :
                       src.LastWriteTime  != DateTime.MinValue ? src.LastWriteTime :
                       src.CreationTime;
            if (time != DateTime.MinValue) Io.SetLastAccessTime(path, time);
        }

        #endregion
    }
}
