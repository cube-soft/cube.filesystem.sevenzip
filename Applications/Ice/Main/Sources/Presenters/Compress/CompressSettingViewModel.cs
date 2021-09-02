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
using System.Threading;
using Cube.FileSystem.SevenZip.Ice.Settings;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressSettingViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the CompressSettingWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressSettingViewModel : Presentable<QueryMessage<string, CompressRuntimeSetting>>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressSettingViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CompressSettingViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Query message.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressSettingViewModel(
            QueryMessage<string, CompressRuntimeSetting> src,
            SynchronizationContext context
        ) : base(src, new(), context) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the source object. The project will be removed after
        /// the implementation is finished.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public QueryMessage<string, CompressRuntimeSetting> Source => Facade;

        #endregion
    }
}
