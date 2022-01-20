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
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Icons;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordSettingWindow
    ///
    /// <summary>
    /// Represents a dialog to set a new password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordSettingWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordSettingWindow
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordSettingWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordSettingWindow()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcon.Warning.GetImage(IconSize.Large);

            Behaviors.Add(new ClickEventBehavior(ExecuteButton, Close));
            Behaviors.Add(new ClickEventBehavior(ExitButton, Close));

            var obj = Behaviors.Hook(new PasswordLintBehavior(PasswordTextBox, ConfirmTextBox, ShowPasswordCheckBox));
            obj.Updated += (s, e) => ExecuteButton.Enabled = obj.Valid;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Value => PasswordTextBox.Text;

        #endregion
    }
}
