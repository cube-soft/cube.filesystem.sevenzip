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
using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressQuery
    ///
    /// <summary>
    /// Provides functionality to get the runtime settings for compression.
    /// The class may query the user as needed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressQuery : Query<string, CompressRuntimeSetting>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressQuery
        ///
        /// <summary>
        /// Initializes a new instance of the CompressQuery class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="callback">Callback action for the request.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressQuery(Action<QueryMessage<string, CompressRuntimeSetting>> callback) :
            base(callback, Dispatcher.Vanilla) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the runtime settings for compression.
        /// The method may invoke the Query.Request method as needed.
        /// </summary>
        ///
        /// <param name="request">User request.</param>
        /// <param name="settings">Default compression settings.</param>
        ///
        /// <returns>Compression query value.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntimeSetting Get(Request request, CompressSetting settings) =>
            _cache ??= GetValue(request, settings);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetValue
        ///
        /// <summary>
        /// Creates a new instance of the CompressRuntimeSetting class
        /// with the specified arguments. The method may invoke the
        /// Query.Request method as needed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CompressRuntimeSetting GetValue(Request request, CompressSetting settings) => request.Format switch
        {
            Format.Tar      or
            Format.Zip      or
            Format.SevenZip or
            Format.Sfx         => new(request.Format, settings),
            Format.BZip2    or
            Format.GZip     or
            Format.XZ          => new(Format.Tar, settings) { CompressionMethod = request.Format.ToMethod() },
            _                  => Invoke(request, settings),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Asks the user to input the runtime settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CompressRuntimeSetting Invoke(Request request, CompressSetting settings)
        {
            var m = new QueryMessage<string, CompressRuntimeSetting>
            {
                Value  = new(settings),
                Cancel = true,
            };

            var src = request.Sources.First();
            if (src.HasValue())
            {
                var f = Io.Get(src);
                m.Source = Io.Combine(f.DirectoryName, $"{f.BaseName}.zip");
                m.Value.Destination = m.Source;
            }

            Request(m);
            if (m.Cancel) throw new OperationCanceledException();
            return m.Value;
        }

        #endregion

        #region Fields
        private CompressRuntimeSetting _cache;
        #endregion
    }
}
