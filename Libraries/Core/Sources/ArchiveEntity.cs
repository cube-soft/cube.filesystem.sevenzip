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
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEntity
    ///
    /// <summary>
    /// Represents an item in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ArchiveEntity : Entity
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveEntity
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveEntity class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /* ----------------------------------------------------------------- */
        internal ArchiveEntity(ArchiveEntitySource src) : base(src) { }

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
        public int Index => GetSource().Index;

        /* ----------------------------------------------------------------- */
        ///
        /// Crc
        ///
        /// <summary>
        /// Gets the CRC value of the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Crc => GetSource().Crc;

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// Gets the value indicating whether the archive is encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted => GetSource().Encrypted;

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
        public bool Match(IEnumerable<string> names) => names != null && GetSource().Filter.MatchAny(names);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetSource
        ///
        /// <summary>
        /// Gets the source object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveEntitySource GetSource()
        {
            var dest = Source as ArchiveEntitySource;
            Debug.Assert(dest is not null);
            return dest;
        }

        #endregion
    }
}
