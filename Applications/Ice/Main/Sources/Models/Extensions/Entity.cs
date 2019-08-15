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
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// EntityExtension
    ///
    /// <summary>
    /// Provides extended methods of the Entity class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class EntityExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetBaseName
        ///
        /// <summary>
        /// Gets the base-name from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">File information.</param>
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetBaseName(this Entity src, Format format) =>
            new[] { Format.BZip2, Format.GZip, Format.XZ }.Contains(format) ?
            TrimExtension(src.BaseName) :
            src.BaseName;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// TrimExtension
        ///
        /// <summary>
        /// Trims the extension of the specified filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string TrimExtension(string src)
        {
            var index = src.LastIndexOf('.');
            return index < 0 ? src : src.Substring(0, index);
        }

        #endregion
    }
}
