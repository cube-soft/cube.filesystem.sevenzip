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
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Message
    ///
    /// <summary>
    /// Provides functionality to create a message object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Message
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ForCompressLocation
        ///
        /// <summary>
        /// Creates a new instance of the SaveFileDialog class with
        /// the specified source.
        /// </summary>
        ///
        /// <param name="src">Query source object.</param>
        ///
        /// <returns>SaveFileDialog object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage ForCompressLocation(SaveQuerySource src) =>
            ForCompressLocation(src.Source, src.Format);

        /* ----------------------------------------------------------------- */
        ///
        /// ForCompressLocation
        ///
        /// <summary>
        /// Creates a new instance of the SaveFileDialog class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path to save.</param>
        /// <param name="format">Format to save.</param>
        ///
        /// <returns>SaveFileDialog object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static SaveFileMessage ForCompressLocation(string src, Format format)
        {
            if (!src.HasValue()) return new();

            var fi = Io.Get(src);
            return new()
            {
                InitialDirectory = fi.DirectoryName,
                Value            = fi.Name,
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ForExtractLocation
        ///
        /// <summary>
        /// Creates a new instance of the OpenDirectoryMessage class with
        /// the specified source.
        /// </summary>
        ///
        /// <param name="src">Query source object.</param>
        ///
        /// <returns>OpenDirectoryMessage object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static OpenDirectoryMessage ForExtractLocation(SaveQuerySource src) => new()
        {
            NewButton = true,
            Text      = Properties.Resources.MessageExtractDestination,
            Value     = Io.Get(src.Source).DirectoryName,
        };

        #endregion
    }
}
