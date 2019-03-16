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
        public FileItem(Information src, string pathInArchive) : base(src.Source)
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
