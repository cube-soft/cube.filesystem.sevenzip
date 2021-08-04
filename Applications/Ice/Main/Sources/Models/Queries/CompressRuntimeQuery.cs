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
using Cube.FileSystem.SevenZip.Ice.Settings;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressRuntimeQuery
    ///
    /// <summary>
    /// Provides functionality to get the runtime settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressRuntimeQuery : Query<string, CompressRuntime>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressRuntimeQuery
        ///
        /// <summary>
        /// Initializes a new instance of the CompressRuntimeQuery class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="callback">Callback action for the request.</param>
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntimeQuery(
            Action<QueryMessage<string, CompressRuntime>> callback,
            Dispatcher dispatcher
        ) : base(callback, dispatcher) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetValue
        ///
        /// <summary>
        /// Gets the runtime settings.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntime GetValue(string src, Format format) => _value ??= Get(src, format);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the runtime settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CompressRuntime Get(string src, Format format)=> format switch
        {
            Format.Tar      or
            Format.Zip      or
            Format.SevenZip or
            Format.Sfx         => new(format),
            Format.BZip2    or
            Format.GZip     or
            Format.XZ          => new(Format.Tar) { CompressionMethod = format.ToMethod() },
            _                  => Request(src),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        ///
        /// <summary>
        /// Asks the user to input the runtime settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CompressRuntime Request(string src)
        {
            var fi  = Io.Get(src);
            var msg = new QueryMessage<string, CompressRuntime>
            {
                Source = Io.Combine(fi.DirectoryName, $"{fi.BaseName}.zip"),
                Value  = new(),
                Cancel = true,
            };

            Request(msg);
            if (msg.Cancel) throw new OperationCanceledException();
            return msg.Value;
        }

        #endregion

        #region Fields
        private CompressRuntime _value;
        #endregion
    }
}
