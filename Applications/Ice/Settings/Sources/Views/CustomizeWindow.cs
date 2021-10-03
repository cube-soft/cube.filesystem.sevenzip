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
using System.Windows.Forms;
using Cube.Forms.Behaviors;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CustomizeWindow
    ///
    /// <summary>
    /// Represents the cutomize window for the context menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class CustomizeWindow : Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CustomizeWindow
        ///
        /// <summary>
        /// Initiaizes a new instance of the CustomizeWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeWindow()
        {
            InitializeComponent();

            _menu = new(SourceTreeView, DestinationTreeView);
            _menu.Updated += (s, e) => {
                RenameButton.Enabled = _menu.Editable;
                RemoveButton.Enabled = _menu.Editable;
                UpButton.Enabled     = _menu.Editable;
                DownButton.Enabled   = _menu.Editable;
            };

            ShortcutKeys.Add(Keys.F2, () => _menu.RenameMenu.Execute());
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
            if (src is not CustomizeViewModel vm) return;

            Behaviors.Add(new ClickBehavior(ApplyButton, vm.Save));
            Behaviors.Add(new ClickBehavior(ExitButton, Close));
            Behaviors.Add(new ClickBehavior(RenameButton, _menu.RenameMenu.Execute));
            Behaviors.Add(new ClickBehavior(AddButton, _menu.AddMenu.Execute));
            Behaviors.Add(new ClickBehavior(NewCategoryButton, _menu.NewCategoryMenu.Execute));
            Behaviors.Add(new ClickBehavior(RemoveButton, _menu.RemoveMenu.Execute));
            Behaviors.Add(new ClickBehavior(UpButton, _menu.UpMenu.Execute));
            Behaviors.Add(new ClickBehavior(DownButton, _menu.DownMenu.Execute));

            _menu.Register(vm.Source, vm.Current, vm.Images);
        }

        #endregion

        #region Fields
        private readonly CustomizeMenu _menu;
        #endregion
    }
}
