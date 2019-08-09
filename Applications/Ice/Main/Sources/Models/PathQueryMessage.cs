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
    /// PathQueryMessage
    ///
    /// <summary>
    /// Represents the query information of the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PathQueryMessage : QueryMessage<string, string>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// PathQueryMessage
        ///
        /// <summary>
        /// Initializes a new instance of the PathQueryMessage class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="query">Path of the source file.</param>
        /// <param name="format">Compression format.</param>
        /// <param name="cancel">Cancel or not.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathQueryMessage(string query, Format format, bool cancel)
        {
            Query  = query;
            Format = format;
            Cancel = cancel;
        }

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the format of the target archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        #endregion
    }
}
