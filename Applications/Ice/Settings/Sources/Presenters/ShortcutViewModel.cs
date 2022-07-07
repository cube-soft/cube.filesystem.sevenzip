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
using Cube.Backports;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutViewModel
    ///
    /// <summary>
    /// Provides functionality to bind values to view components for the
    /// shortcut settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShortcutViewModel : PresentableBase<ShortcutSettingValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ShortcutViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Shortcut settings.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutViewModel(ShortcutSettingValue src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(src, aggregator, context) => Assets.Add(new ObservableProxy(Facade, this));

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Compress
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the compression menu is
        /// enabled or not.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Compress
        {
            get => Facade.Preset.HasFlag(Preset.Compress);
            set => Set(Preset.Compress, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the extract menu is
        /// enabled or not.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Extract
        {
            get => Facade.Preset.HasFlag(Preset.Extract);
            set => Set(Preset.Extract, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the settings menu is
        /// enabled or not.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Settings
        {
            get => Facade.Preset.HasFlag(Preset.Settings);
            set => Set(Preset.Settings, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressOptions
        ///
        /// <summary>
        /// Gets or sets options for the compression.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Preset CompressOptions
        {
            get => Facade.Preset & Preset.CompressMask;
            set
            {
                var strip = Facade.Preset & ~Preset.CompressMask;
                Facade.Preset = strip | value;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Updates the Preset property with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(Preset value, bool check)
        {
            if (check) Facade.Preset |= value;
            else Facade.Preset &= ~value;
        }

        #endregion
    }
}
