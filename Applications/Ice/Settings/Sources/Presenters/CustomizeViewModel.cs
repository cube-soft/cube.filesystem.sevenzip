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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Cube.Images.Icons;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CustomizeViewModel
    ///
    /// <summary>
    /// Provides functionality to bind values to the CustomizeWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CustomizeViewModel : Presentable<IEnumerable<Context>>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CustomizeViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CustomizeViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Current context menu.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        /// <param name="callback">Callback action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeViewModel(IEnumerable<Context> src,
            Aggregator aggregator,
            SynchronizationContext context,
            Action<IEnumerable<Context>> callback
        ) : base(src, aggregator, context)
        {
            Save = () => Track(() => { Send<CloseMessage>(); callback(Facade); }, true);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Get the list of menus that can be added.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Context> Source { get; } = PresetExtension.ToContextCollection(
            Preset.Compress | Preset.CompressMask |
            Preset.Extract | Preset.ExtractMask
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Current
        ///
        /// <summary>
        /// Get the current menu list.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Context> Current => Facade;

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// Get the list of displayed images.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public List<Image> Images { get; } = new()
        {
            IconFactory.Create(StockIcons.Folder, IconSize.Small).ToBitmap(),
            Properties.Resources.Archive,
            Properties.Resources.Extract,
            IconFactory.Create(StockIcons.FolderOpen, IconSize.Small).ToBitmap(),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Gets the action to save the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Action Save { get; }

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
    }
}
