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
namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileItemExtension
    ///
    /// <summary>
    /// Provides extended methods of the FileItem class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FileItemExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToFileItem
        ///
        /// <summary>
        /// Creates a new instance of the FileItem class with the specified
        /// file or directory information.
        /// </summary>
        ///
        /// <param name="src">File or directory information.</param>
        ///
        /// <returns>Converted result.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static FileItem ToFileItem(this Information src) =>
            new FileItem(src);

        /* ----------------------------------------------------------------- */
        ///
        /// ToFileItem
        ///
        /// <summary>
        /// Creates a new instance of the FileItem class with the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="src">File or directory information.</param>
        /// <param name="pathInArchive">Relative path in the archive.</param>
        ///
        /// <returns>Converted result.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static FileItem ToFileItem(this Information src, string pathInArchive) =>
            new FileItem(src, pathInArchive);

        #endregion
    }
}
