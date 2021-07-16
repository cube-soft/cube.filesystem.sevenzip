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
using Cube.Collections;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveList
    ///
    /// <summary>
    /// Represents the collection of items in the provided archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveList : EnumerableBase<ArchiveItem>, IReadOnlyList<ArchiveItem>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveList
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveList class with the
        /// specified controller.
        /// </summary>
        ///
        /// <param name="controller">Controller object.</param>
        /// <param name="count">Number of items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveList(ArchiveReaderController controller, int count)
        {
            _controller = controller;
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
        public ArchiveItem this[int index] => new ArchiveItem(_controller, index);

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
        public override IEnumerator<ArchiveItem> GetEnumerator()
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
        private readonly ArchiveReaderController _controller;
        #endregion
    }
}
