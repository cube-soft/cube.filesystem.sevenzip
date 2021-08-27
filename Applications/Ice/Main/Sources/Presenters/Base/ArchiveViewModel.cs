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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveViewModel
    ///
    /// <summary>
    /// Represents the ViewModel to create or extract archives with the
    /// ProgressWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ArchiveViewModel<TModel> : ProgressViewModel<TModel>
        where TModel : ArchiveFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CompressViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Model object.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveViewModel(TModel src, Aggregator aggregator, SynchronizationContext context) :
            base(src, aggregator, context)
        {
            Facade.Select   = new(Send, GetDispatcher(true));
            Facade.Password = new Query<string>(e => {
                e.Cancel = true;
                Send(e);
            }, GetDispatcher(true));
        }

        #endregion
    }
}
