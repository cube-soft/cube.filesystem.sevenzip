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
    /// CompressViewModel
    ///
    /// <summary>
    /// Represents the ViewModel to create an archive with the
    /// ProgressWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressViewModel : ArchiveViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CompressViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Request of the transaction.</param>
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressViewModel(Request src, SettingFolder settings) :
            this (src, settings, SynchronizationContext.Current) { }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CompressViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Request of the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressViewModel(Request src, SettingFolder settings, SynchronizationContext context) :
            this(new CompressFacade(src, settings, new ContextDispatcher(context, false)), context) { }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CompressViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Model object.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        private CompressViewModel(CompressFacade src, SynchronizationContext context) :
            base(src, new(), context)
        {
            src.Configure = new(Send, GetDispatcher(true));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the source object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CompressFacade Source => (CompressFacade)Facade;

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
        protected override string GetTitle() => Source.GetTitle();

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetText() => Source.GetText();

        #endregion
    }
}
