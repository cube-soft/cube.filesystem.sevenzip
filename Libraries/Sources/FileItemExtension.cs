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
