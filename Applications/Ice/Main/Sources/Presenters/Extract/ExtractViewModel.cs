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
using System.Drawing;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractViewModel
    ///
    /// <summary>
    /// Represents the ViewModel to extract archives with the
    /// ProgressWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractViewModel : ArchiveViewModel<ExtractFacade>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Request of the process.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel(Request src, SettingFolder settings, SynchronizationContext context) :
            base(new(src, settings, new ContextDispatcher(context, false)), new(), context)
        {
            Facade.Overwrite = new(Send, GetDispatcher(true));
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetTitle() => Facade.GetTitle();

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetText() => Facade.GetText();

        #endregion
    }
}
