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
using Cube.FileSystem.SevenZip.Ice.Settings;
using System;

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
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntimeQuery(
            Action<QueryMessage<string, CompressRuntime>> callback,
            Invoker invoker
        ) : base(callback, invoker) { }

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntime GetValue(string src, Format format, IO io) =>
            _value ?? (_value = Get(src, format, io));

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
        private CompressRuntime Get(string src, Format format, IO io)
        {
            switch (format)
            {
                case Format.Tar:
                case Format.Zip:
                case Format.SevenZip:
                case Format.Sfx:
                    return new CompressRuntime(format, io);
                case Format.BZip2:
                case Format.GZip:
                case Format.XZ:
                    return new CompressRuntime(Format.Tar, io) { CompressionMethod = format.ToMethod() };
                default:
                    return Request(src, io);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        ///
        /// <summary>
        /// Asks the user to input the runtime settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CompressRuntime Request(string src, IO io)
        {
            var fi   = io.Get(src);
            var path = io.Combine(fi.DirectoryName, $"{fi.BaseName}.zip");
            var msg  = Query.NewMessage(path, new CompressRuntime(io));

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
