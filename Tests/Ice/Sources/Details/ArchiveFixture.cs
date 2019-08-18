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
using Cube.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveFixture
    ///
    /// <summary>
    /// Provides functionality to
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    abstract class ArchiveFixture : FileFixture
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the CompressViewModel class with
        /// the specified arguments and invokes the specified callback.
        /// </summary>
        ///
        /// <param name="files">Source files.</param>
        /// <param name="args">Program arguments.</param>
        /// <param name="settings">User settings for compression.</param>
        /// <param name="callback">Callback action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Create(IEnumerable<string> files,
            IEnumerable<string> args,
            CompressValue settings,
            Action<CompressViewModel> callback
        ) {
            var context = new SynchronizationContext();
            var request = new Request(args.Concat(files.Select(e => GetSource(e))));
            var folder  = new SettingFolder(GetType().Assembly, IO);

            folder.Value.Compress = settings;
            folder.Value.Filters = "Filter.txt|FilterDirectory";

            using (var vm = new CompressViewModel(request, folder, context)) callback(vm);
        }

        #endregion
    }
}
