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
using System.Threading;
using Cube.Mixin.Collections;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingViewModel
    ///
    /// <summary>
    /// Provides functionality to bind values to the MainWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingViewModel : PresentableBase<SettingFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">User settings.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingViewModel(SettingFolder src, SynchronizationContext context) :
            base(src, new(), context)
        {
            Compress  = new(src.Value.Compress,  Aggregator, Context);
            Extract   = new(src.Value.Extract,   Aggregator, Context);
            Associate = new(src.Value.Associate, Aggregator, Context);
            Menu      = new(src.Value.Context,   Aggregator, Context);
            Shortcut  = new(src.Value.Shortcut,  Aggregator, Context);

            Assets.Add(new ObservableProxy(Facade, this));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Compress
        ///
        /// <summary>
        /// Gets the ViewModel object for compression settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressViewModel Compress { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Gets the ViewModel object for extracting settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel Extract { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Associate
        ///
        /// <summary>
        /// Gets the ViewModel object for settings of the file association.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateViewModel Associate { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ContextMenu
        ///
        /// <summary>
        /// Gets the ViewModel object for context menu settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ContextViewModel Menu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcut
        ///
        /// <summary>
        /// Gets the ViewModel object for shortcut settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutViewModel Shortcut { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Product
        ///
        /// <summary>
        /// Gets the product name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Product => "CubeICE";

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// Gets the version text.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => $"Version {Facade.Version.ToString(3, true)}";

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to check for
        /// updates at startup.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Facade.Startup.Enabled;
            set => Facade.Startup.Enabled = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// Gets or sets the value to filter files and directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Filters
        {
            get => Transform(Facade.Value.Filters, "|", Environment.NewLine);
            set => Facade.Value.Filters = Transform(value, Environment.NewLine, "|");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AlphaFS
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to use the AlphaFS
        /// module for I/O operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool AlphaFS
        {
            get => Facade.Value.AlphaFS;
            set => Facade.Value.AlphaFS = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path used for the temp directory. If the value
        /// is empty, the same directory as the source file will be used.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp
        {
            get => Facade.Value.Temp;
            set => Facade.Value.Temp = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTip
        ///
        /// <summary>
        /// Gets or sets a value to show the tooltip.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ToolTip
        {
            get => Facade.Value.ToolTip;
            set => Facade.Value.ToolTip = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTipCount
        ///
        /// <summary>
        /// Gets or sets the number of items to show in the tooltip.
        /// The value is only applicable when ToolTip is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ToolTipCount
        {
            get => Facade.Value.ToolTipCount;
            set => Facade.Value.ToolTipCount = value;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(bool close) => Run(
            () => Facade.SaveEx(),
            () => { if (close) Send(new CloseMessage()); },
            true
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Browse
        ///
        /// <summary>
        /// Shows the dialog to select the temp directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Browse() => Send(Message.ForSaveDirectory(Temp), e => Temp = e, true);

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
        /// Transform
        ///
        /// <summary>
        /// Converts the format of a string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Transform(string src, string sch, string rep) =>
            src.Split(new[] { sch }, StringSplitOptions.RemoveEmptyEntries).Join(rep);

        #endregion
    }
}
