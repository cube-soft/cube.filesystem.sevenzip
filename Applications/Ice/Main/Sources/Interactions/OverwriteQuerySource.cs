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
namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteQuerySource
    ///
    /// <summary>
    /// Represents the request information to query an overwrite method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class OverwriteQuerySource
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteQuerySource
        ///
        /// <summary>
        /// Initializes a new instance of the OverwriteQuerySource class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">File information to overwrite.</param>
        /// <param name="dest">File information to be overwritten.</param>
        ///
        /* ----------------------------------------------------------------- */
        public OverwriteQuerySource(Entity src, Entity dest)
        {
            Source      = src;
            Destination = dest;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the file information to overwrite.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets the file information to be overwritten.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity Destination { get; }

        #endregion
    }
}
