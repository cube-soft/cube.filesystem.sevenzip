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

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressViewModel
    ///
    /// <summary>
    /// Provides functionality to bind values to the CompressWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CompressViewModel : ArchiveViewModel<CompressSetting>
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
        /// <param name="src">Settings for compression.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressViewModel(CompressSetting src, Aggregator aggregator, SynchronizationContext context) :
            base(src, aggregator, context) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel
        {
            get => Facade.CompressionLevel;
            set => Facade.CompressionLevel = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UseUtf8
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to convert the file name
        /// to UTF-8 or not.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseUtf8
        {
            get => Facade.UseUtf8;
            set => Facade.UseUtf8 = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OverwritePrompt
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to show the
        /// Save As dialog when a file with the same name exists in the
        /// path specified as the destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool OverwritePrompt
        {
            get => Facade.OverwritePrompt;
            set => Facade.OverwritePrompt = value;
        }

        #endregion
    }
}
