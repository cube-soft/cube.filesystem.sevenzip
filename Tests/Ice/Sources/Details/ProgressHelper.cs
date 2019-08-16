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
using Cube.Mixin.Observing;
using System;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressHelper
    ///
    /// <summary>
    /// Provides extended methods of ProgressViewModel and inherited
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class ProgressHelper
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SetPassword
        ///
        /// <summary>
        /// Subscribes the message to set password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable SetPassword<T>(this ArchiveViewModel<T> src, string value)
            where T : ArchiveFacade => src.Subscribe<QueryMessage<string, string>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Subscribes the message to select the save path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable SetDestination<T>(this ArchiveViewModel<T> src, string value)
            where T : ArchiveFacade => src.Subscribe<QueryMessage<SelectQuerySource, string>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetToken
        ///
        /// <summary>
        /// Gets token to wait for the main operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static CancellationToken GetToken<T>(this ProgressViewModel<T> src)
            where T : ProgressFacade
        {
            var dest = new CancellationTokenSource();
            _ = src.Subscribe(e => {
                if (e == nameof(src.State) && src.State == TimerState.Stop) dest.Cancel();
            });
            return dest.Token;
        }

        #endregion
    }
}
