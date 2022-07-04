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
    /// ExtractViewModel
    ///
    /// <summary>
    /// Provides functionality to bind values to the ExtractWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ExtractViewModel : ArchiveViewModel<ExtractSettingValue>
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
        /// <param name="src">Settings for extracting.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel(ExtractSettingValue src, Aggregator aggregator, SynchronizationContext context) :
            base(src, aggregator, context) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to create the
        /// folder.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CreateDirectory
        {
            get => Facade.SaveMethod.HasFlag(SaveMethod.Create);
            set => SetSaveMethod(SaveMethod.Create, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SkipSingleDirectory
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to skip creating
        /// a folder if the extracted result is a single folder.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SkipSingleDirectory
        {
            get => Facade.SaveMethod.HasFlag(SaveMethod.SkipSingleDirectory);
            set => SetSaveMethod(SaveMethod.SkipSingleDirectory, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to delete the
        /// original file after extracting.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool DeleteSource
        {
            get => Facade.DeleteSource;
            set => Facade.DeleteSource = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bursty
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not multiple
        /// compressed files should be extracted at the same time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Bursty
        {
            get => Facade.Bursty;
            set => Facade.Bursty = value;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetSaveMethod
        ///
        /// <summary>
        /// Updates the SaveMethod property with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SetSaveMethod(SaveMethod value, bool check)
        {
            if (check) Facade.SaveMethod |= value;
            else Facade.SaveMethod &= ~value;
        }

        #endregion
    }
}
