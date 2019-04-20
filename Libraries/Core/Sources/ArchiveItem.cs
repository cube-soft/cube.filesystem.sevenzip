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
using Cube.FileSystem.SevenZip.Mixin;
using System;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItem
    ///
    /// <summary>
    /// Represents an item in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveItem : Information
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItem
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveItem class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the archive.</param>
        /// <param name="index">Index of the archive.</param>
        /// <param name="controller">Controller object.</param>
        ///
        /* ----------------------------------------------------------------- */
        internal ArchiveItem(string src, int index, ArchiveItemController controller) :
            base(src, controller, index) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index => Controllable.ToAi().Index;

        /* ----------------------------------------------------------------- */
        ///
        /// RawName
        ///
        /// <summary>
        /// Gets the original name that represents the relative path
        /// in the archive.
        /// </summary>
        ///
        /// <remarks>
        /// FullName property represents the normalized result against
        /// the RawName property.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string RawName => Controllable.ToAi().RawName;

        /* ----------------------------------------------------------------- */
        ///
        /// Crc
        ///
        /// <summary>
        /// Gets the CRC value of the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Crc => Controllable.ToAi().Crc;

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// Gets the value indicating whether the archive is encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted => Controllable.ToAi().Encrypted;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// Gets the value indicating whether any of the specified
        /// collection matches the all or part of the path.
        /// </summary>
        ///
        /// <param name="names">Collection of names.</param>
        ///
        /// <returns>true for match.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Match(IEnumerable<string> names) => Controllable.ToAi().Filter.MatchAny(names);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the archived item and saves to the specified path.
        /// </summary>
        ///
        /// <param name="directory">Directory to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the archived item and saves to the specified path.
        /// </summary>
        ///
        /// <param name="directory">Directory to save.</param>
        /// <param name="progress">Progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, IProgress<Report> progress) =>
            ((ArchiveItemController)Controller).Extract(this, directory, false, progress);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extract operation on the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract() => Extract(default(IProgress<Report>));

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extract operation on the item.
        /// </summary>
        ///
        /// <param name="progress">Progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(IProgress<Report> progress) =>
            ((ArchiveItemController)Controller).Extract(this, null, true, progress);

        #endregion
    }
}
