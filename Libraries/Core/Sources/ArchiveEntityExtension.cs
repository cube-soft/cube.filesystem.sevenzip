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

            Io.SetAttributes(path, src.Attributes);
            SetCreationTime(src, path);
            SetLastWriteTime(src, path);
            SetLastAccessTime(src, path);
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
