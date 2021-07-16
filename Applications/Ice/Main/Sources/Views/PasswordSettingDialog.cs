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
using Cube.Forms.Behaviors;
using Cube.Images.Icons;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordSettingDialog
    ///
    /// <summary>
    /// Represents a dialog to set a new password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordSettingDialog : Cube.Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressPasswordDialog
        ///
        /// <summary>
        /// Initializes a new instance of the CompressPasswordDialog class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordSettingDialog()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcons.Warning.GetIcon(IconSize.Large).ToBitmap();

            ExecuteButton.Click += (s, e) => Close();
            ExitButton.Click    += (s, e) => Close();

            var b = new PasswordBehavior(PasswordTextBox, ConfirmTextBox, ShowPasswordCheckBox);
            b.Updated += (s, e) => ExecuteButton.Enabled = b.IsValid;
            Behaviors.Add(b);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password => PasswordTextBox.Text;

        #endregion
    }
}
