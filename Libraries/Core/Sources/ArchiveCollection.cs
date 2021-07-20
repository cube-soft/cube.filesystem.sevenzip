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
    internal class ArchiveCollection : EnumerableBase<ArchiveEntity>, IReadOnlyList<ArchiveEntity>
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
        public ArchiveEntity this[int index] => new(new(_core, index, _path));

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
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Fields
        private readonly IInArchive _core;
        private readonly string _path;
        #endregion
    }
}
