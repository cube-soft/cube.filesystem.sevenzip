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
    /// FileItem
    ///
    /// <summary>
    /// Represents an item to be archived.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileItem : Information
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// Creates a new instance of the FileItem class with the specified
        /// information.
        /// </summary>
        ///
        /// <param name="src">File or directory information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(Information src) : this(src, src.Name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// FileItem
        ///
        /// <summary>
        /// Creates a new instance of the FileItem class with the specified
        /// information.
        /// </summary>
        ///
        /// <param name="src">File or directory information.</param>
        /// <param name="pathInArchive">Relative path in the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileItem(Information src, string pathInArchive) : base(src)
        {
            PathInArchive = pathInArchive;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PathInArchive
        ///
        /// <summary>
        /// Gets the relative path in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string PathInArchive { get; }

        #endregion
    }
}
