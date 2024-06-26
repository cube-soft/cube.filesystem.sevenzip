﻿/* ------------------------------------------------------------------------- */
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
using System.Windows.Forms;
using Cube.Forms.Behaviors;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// Represents the settings window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainWindow
        ///
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainWindow()
        {
            InitializeComponent();

            _version.Description = string.Empty;
            _version.Image       = Properties.Resources.Logo;
            _version.Uri         = new("https://www.cube-soft.jp/");
            _version.Location    = new(40, 40);
            _version.Size        = new(400, 300);
            VersionTabPage.Controls.Add(_version);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnBind
        ///
        /// <summary>
        /// Binds the windows to the specified object.
        /// </summary>
        ///
        /// <param name="src">Binding object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            if (src is not SettingViewModel vm) return;

            BindCore(vm);

            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenDirectoryBehavior(vm));
            Behaviors.Add(new ProcessBehavior(vm));
            Behaviors.Add(new ClickEventBehavior(ExecuteButton, () => vm.Save(true)));
            Behaviors.Add(new ClickEventBehavior(ApplyButton, () => vm.Save(false)));
            Behaviors.Add(new ClickEventBehavior(ExitButton, Close));
            Behaviors.Add(new ClickEventBehavior(ContextResetButton, vm.Menu.Reset));
            Behaviors.Add(new ClickEventBehavior(ContextCustomizeButton, vm.Menu.Customize));
            Behaviors.Add(new ClickEventBehavior(AssociateIconButton, vm.Associate.SelectIcon));
            Behaviors.Add(new ClickEventBehavior(AssociateAllButton, vm.Associate.SelectAll));
            Behaviors.Add(new ClickEventBehavior(AssociateClearButton, vm.Associate.Clear));
            Behaviors.Add(new ClickEventBehavior(AssociateHelpButton, vm.Associate.Help));
            Behaviors.Add(new ClickEventBehavior(CompressSaveButton, vm.Compress.Browse));
            Behaviors.Add(new ClickEventBehavior(ExtractSaveButton, vm.Extract.Browse));
            Behaviors.Add(new ClickEventBehavior(TempButton, vm.Browse));
            Behaviors.Add(new PathLintBehavior(CompressSaveTextBox, _tooltip));
            Behaviors.Add(new PathLintBehavior(ExtractSaveTextBox, _tooltip));
            Behaviors.Add(new AssociateIconBehavior(vm));
            Behaviors.Add(new ShowDialogBehavior<CustomizeWindow, CustomizeViewModel>(vm));

            _version.Product = vm.Product;
            _version.Version = vm.Version;
            _tooltip.ToolTipTitle = Properties.Resources.MessageInvalidChars;
        }

        #endregion

        #region Fields
        private readonly Forms.Controls.VersionControl _version = new();
        private readonly ToolTip _tooltip = new();
        #endregion
    }
}
