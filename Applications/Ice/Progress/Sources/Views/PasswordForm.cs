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
using Cube.Images.Icons;
using System;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordForm
    ///
    /// <summary>
    /// パスワード入力用画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PasswordForm : Form
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
        public PasswordForm()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcons.Warning.GetIcon(IconSize.Large).ToBitmap();

            ExecButton.Click += (s, e) => Close();
            ExitButton.Click += (s, e) => Close();
            PasswordTextBox.TextChanged    += WhenTextChanged;
            VisibleCheckBox.CheckedChanged += WhenCheckedChanged;
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
        /// WhenTextChanged
        ///
        /// <summary>
        /// テキストボックスの入力が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenTextChanged(object s, EventArgs e) =>
            ExecButton.Enabled = PasswordTextBox.TextLength > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// WhenCheckedChanged
        ///
        /// <summary>
        /// チェックボックスの状態が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenCheckedChanged(object s, EventArgs e) =>
            PasswordTextBox.UseSystemPasswordChar = !VisibleCheckBox.Checked;

        #endregion
    }
}
