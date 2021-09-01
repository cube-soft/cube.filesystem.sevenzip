﻿/* ------------------------------------------------------------------------- */
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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SelectQuery
    ///
    /// <summary>
    /// Provides functionality to query a save path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SelectQuery : Query<SelectQuerySource, string>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SelectPathQuery
        ///
        /// <summary>
        /// Initializes a new instance of the SelectPathQuery class with
        /// the specified action.
        /// </summary>
        ///
        /// <param name="callback">User action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SelectQuery(Action<QueryMessage<SelectQuerySource, string>> callback) :
            base(callback, Dispatcher.Vanilla) { }

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
        /// <returns>Message object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static QueryMessage<SelectQuerySource, string> NewMessage(string src, Format format) => new()
        {
            Source = new SelectQuerySource(src, format),
            Value  = string.Empty,
            Cancel = true,
        };
    }
}
