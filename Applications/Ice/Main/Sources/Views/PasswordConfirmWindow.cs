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
using Cube.Forms.Behaviors;
using Cube.Images.Icons;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice
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
    public partial class PasswordConfirmWindow : Form
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
        public PasswordConfirmWindow()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcons.Warning.GetIcon(IconSize.Large).ToBitmap();

            ExecuteButton.Click += (s, e) => Close();
            ExitButton.Click += (s, e) => Close();

            _behavior = new PasswordBehavior(PasswordTextBox, ConfirmTextBox, ShowPasswordCheckBox);
            _behavior.Updated += (s, e) => ExecuteButton.Enabled = _behavior.IsValid;
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

        #region Fields
        private readonly PasswordBehavior _behavior;
        #endregion
    }
}
