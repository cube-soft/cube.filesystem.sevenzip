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
    /// PasswordWindow
    ///
    /// <summary>
    /// Represents a dialog to input a password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordWindow
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordWindow()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcon.Warning.GetImage(IconSize.Large);

            Behaviors.Add(new ClickEventBehavior(ExecButton, Close));
            Behaviors.Add(new ClickEventBehavior(ExitButton, Close));
            Behaviors.Add(new EventBehavior(PasswordTextBox, "TextChanged",
                () => ExecButton.Enabled = PasswordTextBox.TextLength > 0));
            Behaviors.Add(new EventBehavior(VisibleCheckBox, "CheckedChanged",
                () => PasswordTextBox.UseSystemPasswordChar = !VisibleCheckBox.Checked));
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
