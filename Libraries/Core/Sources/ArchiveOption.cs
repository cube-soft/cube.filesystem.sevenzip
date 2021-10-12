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
    /// ArchiveOption
    ///
    /// <summary>
    /// Represents options when creating a new archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveOption
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// Gets or sets the number of threads that the archiver is
        /// available.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount { get; init; } = 1;

        /* ----------------------------------------------------------------- */
        ///
        /// CodePage
        ///
        /// <summary>
        /// Gets or sets the value of code page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CodePage CodePage { get; init; } = CodePage.Oem;

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Gets or sets the predicate function to filter some of files or
        /// directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Predicate<Entity> Filter { get; init; }
    }
}
