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
using Cube.Mixin.Generics;
using System;

namespace Cube.FileSystem.SevenZip.Ice
{
    #region PathQuery

    /* --------------------------------------------------------------------- */
    ///
    /// PathQuery
    ///
    /// <summary>
    /// Provides functionality to query a save path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PathQuery : Query<PathQuerySource, string>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// PathQuery
        ///
        /// <summary>
        /// Initializes a new instance of the PathQuery class with
        /// the specified action.
        /// </summary>
        ///
        /// <param name="callback">User action.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathQuery(Action<PathQueryMessage> callback, Invoker invoker) :
            base(e => callback(e.TryCast<PathQueryMessage>()), invoker) { }

        /* ----------------------------------------------------------------- */
        ///
        /// NewMessage
        ///
        /// <summary>
        /// Creates a new instance of the QueryMessage class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="format">Compression format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static PathQueryMessage NewMessage(string src, Format format) => new PathQueryMessage
        {
            Source = new PathQuerySource(src, format),
            Value  = string.Empty,
            Cancel = true,
        };
    }

    #endregion

    #region PathQuerySource

    /* --------------------------------------------------------------------- */
    ///
    /// PathQuerySource
    ///
    /// <summary>
    /// Represents the request information to query a save path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PathQuerySource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// PathQueryRequest
        ///
        /// <summary>
        /// Initializes a new instance of the PathRequest class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="format">Compression format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathQuerySource(string src, Format format)
        {
            Source = src;
            Format = format;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

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
    }

    #endregion

    #region PathQueryMessage

    /* --------------------------------------------------------------------- */
    ///
    /// PathQueryMessage
    ///
    /// <summary>
    /// Represents the message for the PathQuery class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PathQueryMessage : QueryMessage<PathQuerySource, string> { }

    #endregion
}
