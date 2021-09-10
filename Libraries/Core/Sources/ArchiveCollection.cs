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
using Cube.Collections;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveCollection
    ///
    /// <summary>
    /// Represents the collection of items in the provided archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ArchiveCollection : EnumerableBase<ArchiveEntity>, IReadOnlyList<ArchiveEntity>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveCollection
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveCollection class with
        /// the specified controller.
        /// </summary>
        ///
        /// <param name="core">7-zip core object.</param>
        /// <param name="count">Number of items in the archive.</param>
        /// <param name="path">Path of the archive file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveCollection(IInArchive core, int count, string path)
        {
            _core = core;
            _path = path;
            Count = count;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Item
        ///
        /// <summary>
        /// Gets the element at the specified index
        /// </summary>
        ///
        /// <param name="index">Index of the element to get.</param>
        ///
        /// <returns>ArchiveItem object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveEntity this[int index]
        {
            get
            {
                using var src = new ArchiveEntitySource(_core, index, _path);
                return new(src);
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        ///
        /// <returns>
        /// Enumerator that can be used to iterate through the collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<ArchiveEntity> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) => _core = null;

        #endregion

        #region Fields
        private IInArchive _core;
        private readonly string _path;
        #endregion
    }
}
