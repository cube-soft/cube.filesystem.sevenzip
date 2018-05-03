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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveForm
    ///
    /// <summary>
    /// 圧縮処理の詳細画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ArchiveForm : Form
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveForm()
        {
            InitializeComponent();

            _password = new PasswordBehavior(PasswordTextBox, ConfirmTextBox, ShowPasswordCheckBox);
            _password.Updated += (s, e) => UpdateEncryptionCondition();

            UpdateThreadCount();
            UpdateFormat();
            UpdateCompressionLevel();
            UpdateCompressionMethod();
            UpdateEncryptionMethod();
            UpdateEncryptionCondition();

            ExecuteButton.Enabled = false;

            ExecuteButton.Click += (s, e) => Close();
            ExitButton.Click += (s, e) => Close();
            EncryptionCheckBox.CheckedChanged += (s, e) => UpdateEncryptionCondition();

            OutputButton.Click                             += WhenPathRequested;
            OutputTextBox.TextChanged                      += WhenPathChanged;
            FormatComboBox.SelectedValueChanged            += WhenFormatChanged;
            CompressionMethodComboBox.SelectedValueChanged += WhenCompressionMethodChanged;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 圧縮ファイル形式を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format => (Format)FormatComboBox.SelectedValue;

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// 圧縮レベルを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel =>
            (CompressionLevel)CompressionLevelComboBox.SelectedValue;

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// 圧縮方法を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod =>
            CompressionMethodComboBox.Enabled ?
            (CompressionMethod)CompressionMethodComboBox.SelectedValue :
            CompressionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethod
        ///
        /// <summary>
        /// 暗号化方法を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod =>
            EncryptionMethodComboBox.Enabled ?
            (EncryptionMethod)EncryptionMethodComboBox.SelectedValue :
            EncryptionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// 圧縮方法を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount => (int)ThreadNumericUpDown.Value;

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// 保存先パスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path
        {
            get => OutputTextBox.Text;
            set
            {
                if (OutputTextBox.Text == value) return;
                OutputTextBox.Text = value;
                OutputTextBox.SelectionStart = Math.Max(value.Length - 1, 0);
                OutputTextBox.SelectionLength = 0;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワードを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password =>
            EncryptionGroupBox.Enabled && EncryptionPanel.Enabled ?
            PasswordTextBox.Text :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// IsValidPath
        ///
        /// <summary>
        /// パス設定が正しいかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private bool IsValidPath
        {
            get => _path;
            set
            {
                if (_path == value) return;
                _path = value;
                ExecuteButton.Enabled = value & IsValidEncryption;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsValidEncryption
        ///
        /// <summary>
        /// 暗号化設定が正しいかどうかを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private bool IsValidEncryption
        {
            get => _encryption;
            set
            {
                if (_encryption == value) return;
                _encryption = value;
                ExecuteButton.Enabled = value & IsValidPath;
            }
        }

        #endregion

        #region Implementations

        #region Update

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateFormat
        ///
        /// <summary>
        /// Format を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateFormat() =>
            Update(FormatComboBox, ViewResource.Formats, 0);

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateCompressionLevel
        ///
        /// <summary>
        /// CompressionLevel を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateCompressionLevel() =>
            Update(CompressionLevelComboBox, ViewResource.CompressionLevels, 5);

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateCompressionMethod
        ///
        /// <summary>
        /// CompressionMethod を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateCompressionMethod() =>
            Update(CompressionMethodComboBox, ViewResource.GetCompressionMethod(Format), 0);

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateEncryptionMethod
        ///
        /// <summary>
        /// EncryptionMethod を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateEncryptionMethod() =>
            Update(EncryptionMethodComboBox, ViewResource.EncryptionMethods, 0);

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateEncryptionCondition
        ///
        /// <summary>
        /// 暗号化設定に関する状態を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateEncryptionCondition()
        {
            EncryptionGroupBox.Enabled       = ViewResource.IsEncryptionSupported(Format);
            EncryptionPanel.Enabled          = EncryptionCheckBox.Checked;
            EncryptionMethodComboBox.Enabled = (Format == Format.Zip);
            IsValidEncryption                = !EncryptionGroupBox.Enabled ||
                                               !EncryptionPanel.Enabled ||
                                               _password.IsValid;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateThreadCount
        ///
        /// <summary>
        /// スレッド数に関する設定を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateThreadCount()
        {
            var count = Environment.ProcessorCount;
            ThreadNumericUpDown.Maximum = count;
            ThreadNumericUpDown.Value   = count;
            ThreadNumericUpDown.Enabled = count > 1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// ComboBox.DataSource オブジェクトを更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update<T>(ComboBox src, IList<KeyValuePair<string, T>> data, int index)
        {
            src.Enabled       = (data != null);
            src.DataSource    = data;
            src.DisplayMember = "Key";
            src.ValueMember   = "Value";
            if (src.Enabled) src.SelectedIndex = index;
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// WhenFormatChanged
        ///
        /// <summary>
        /// Format 変更時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenFormatChanged(object sender, EventArgs e)
        {
            UpdateCompressionMethod();
            UpdateEncryptionCondition();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPathRequested
        ///
        /// <summary>
        /// 保存パスの要求時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPathRequested(object sender, EventArgs e)
        {
            var cvt  = new PathConverter(Path, Format, CompressionMethod);
            var args = new PathQueryEventArgs(cvt.Result.FullName, cvt.ResultFormat, true);

            Views.ShowSaveView(args);
            if (!args.Cancel) Path = args.Result;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPathChanged
        ///
        /// <summary>
        /// 保存パスの変更時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPathChanged(object sender, EventArgs e) =>
            IsValidPath = OutputTextBox.TextLength > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// WhenCompressionMethodChanged
        ///
        /// <summary>
        /// CompressionMethod 変更時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenCompressionMethodChanged(object sender, EventArgs e) =>
            Path = new PathConverter(Path, Format, CompressionMethod).Result.FullName;

        #endregion

        #region Fields
        private bool _path = false;
        private bool _encryption = true;
        private PasswordBehavior _password;
        #endregion
    }
}
