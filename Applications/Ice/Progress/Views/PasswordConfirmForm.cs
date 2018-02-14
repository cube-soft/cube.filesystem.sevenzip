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
using System.Drawing;
using System.Windows.Forms;
using Cube.Images.Icons;

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordConfirmForm
    ///
    /// <summary>
    /// 入力確認項目付のパスワード設定画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordConfirmForm : Form
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordConfirmForm()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcons.Warning.GetIcon(IconSize.Large).ToBitmap();

            ExecuteButton.Click += (s, e) => Close();
            ExitButton.Click += (s, e) => Close();

            PasswordTextBox.TextChanged         += WhenPasswordChanged;
            ConfirmTextBox.TextChanged          += WhenConfirmChanged;
            ConfirmTextBox.EnabledChanged       += WhenConfirmEnabledChanged;
            ShowPasswordCheckBox.CheckedChanged += WhenShowPasswordChanged;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワードを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password => PasswordTextBox.Text;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPasswordChanged
        ///
        /// <summary>
        /// パスワード入力が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPasswordChanged(object sender, EventArgs e)
        {
            if (ShowPasswordCheckBox.Checked) ExecuteButton.Enabled = PasswordTextBox.TextLength > 0;
            else ConfirmTextBox.Text = string.Empty;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenConfirmChanged
        ///
        /// <summary>
        /// 確認項目のテキストが変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenConfirmChanged(object sender, EventArgs e)
        {
            if (!ConfirmTextBox.Enabled) return;

            var eq = PasswordTextBox.Text.Equals(ConfirmTextBox.Text);
            ExecuteButton.Enabled    = eq && PasswordTextBox.TextLength > 0;
            ConfirmTextBox.BackColor = eq || ConfirmTextBox.TextLength <= 0 ?
                                       SystemColors.Window :
                                       Color.FromArgb(255, 102, 102);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenEnabledChanged
        ///
        /// <summary>
        /// 確認項目の Enabled が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenConfirmEnabledChanged(object sender, EventArgs e) =>
            ConfirmTextBox.BackColor = ConfirmTextBox.Enabled ?
                                       SystemColors.Window :
                                       SystemColors.Control;

        /* ----------------------------------------------------------------- */
        ///
        /// WhenShowPasswordChanged
        ///
        /// <summary>
        /// パスワードを表示の状態が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenShowPasswordChanged(object sender, EventArgs e)
        {
            var show = ShowPasswordCheckBox.Checked;

            PasswordTextBox.UseSystemPasswordChar = !show;
            ConfirmTextBox.Enabled = !show;
            ConfirmTextBox.Text = string.Empty;
            ExecuteButton.Enabled = show & (PasswordTextBox.TextLength > 0);
        }

        #endregion
    }
}
