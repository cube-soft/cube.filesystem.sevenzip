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
    /// RawEntity
    ///
    /// <summary>
    /// Represents the information of the file or directory to be archived.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class RawEntity : Entity
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// RawEntity
        ///
        /// <summary>
        /// Initializes a new instance of the RawEntity class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RawEntity(EntitySource src) : this(src, src.Name) { }

        /* ----------------------------------------------------------------- */
        ///
        /// RawEntity
        ///
        /// <summary>
        /// Initializes a new instance of the RawEntity class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="name">Relative path in the archive.</param>
        ///
        /* ----------------------------------------------------------------- */
        public RawEntity(EntitySource src, string name) : base(src)
        {
            RelativeName = name;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RelativeName
        ///
        /// <summary>
        /// Gets the relative path in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string RelativeName { get; }

        #endregion
    }
}
