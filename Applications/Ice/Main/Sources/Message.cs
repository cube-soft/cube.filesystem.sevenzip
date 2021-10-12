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
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.Collections;
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
            var items = Resource.GetDialogFilters(format);
            var dest  = new SaveFileMessage { Filter = items.GetFilter() };

            if (src.HasValue())
            {
                var fi = Io.Get(src);
                dest.Value            = fi.Name;
                dest.InitialDirectory = fi.DirectoryName;
                dest.FilterIndex      = GetIndex(items, fi);
            }

            return dest;
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
        public static OpenDirectoryMessage ForExtractLocation(SaveQuerySource src)
        {
            var dest = new OpenDirectoryMessage
            {
                Text      = Properties.Resources.MessageExtractDestination,
                NewButton = true,
            };

            if (src.Source.HasValue()) dest.Value = Io.Get(src.Source).DirectoryName;
            return dest;
        }

        #endregion

        #region Implementaions

        /* ----------------------------------------------------------------- */
        ///
        /// GetIndex
        ///
        /// <summary>
        /// Gets the index of the first occurrence of the specified path
        /// in the current DisplayFilter collection.
        /// </summary>
        ///
        /// <param name="src">DisplayFilter collection.</param>
        /// <param name="entity">Target file or directory.</param>
        ///
        /// <returns>Filter index.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static int GetIndex(IEnumerable<FileDialogFilter> src, Entity entity)
        {
            var opt = StringComparison.InvariantCultureIgnoreCase;
            var ext = entity.BaseName.EndsWith(".tar", opt) ?
                      $"{System.IO.Path.GetExtension(entity.BaseName)}{entity.Extension}" :
                      entity.Extension;

            return src.Select((e, i) => new KeyValuePair<int, FileDialogFilter>(i + 1, e))
                      .FirstOrDefault(e => e.Value.Targets.Any(e2 => e2.Equals(ext, opt)))
                      .Key;
        }

        #endregion
    }
}
