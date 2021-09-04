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
using System.Collections.Generic;
using System.Linq;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// Filter
    ///
    /// <summary>
    /// Provides functionality to determine if the provided file or
    /// directory is filtered.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Filter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Initializes a new instance of the Filter class with the
        /// specified file or directory names.
        /// </summary>
        ///
        /// <param name="src">
        /// Collection of file or directory  names to be filtered.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public Filter(IEnumerable<string> src) => Names = src;

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Names
        ///
        /// <summary>
        /// Gets the collection of file or directory names to be filtered.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Names { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// Determines if the specified file or directory is filtered.
        /// </summary>
        ///
        /// <param name="src">File or directory information.</param>
        ///
        /// <returns>true for filtered.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Match(Entity src)
        {
            if (!Names.Any()) return false;
            var parts = Split(src.FullName);
            if (!parts.Any()) return false;

            foreach (var name in Names)
            {
                if (parts.Any(e => string.Compare(e, name, true) == 0)) return true;
            }
            return false;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// Splits the specified path with the path separator.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> Split(string src) =>
            src.Split(SafePath.SeparatorChars.ToArray())
               .SkipWhile(s => !s.HasValue());

        #endregion
    }
}
