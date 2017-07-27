/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Drawing;
using System.Windows.Forms;
using Cube.FileSystem.SevenZip;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsForm
    ///
    /// <summary>
    /// 設定画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class SettingsForm : Cube.Forms.FormBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// OK ボタンまたは適用ボタンのクリック時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Apply
        {
            add { SettingsPanel.Apply += value; }
            remove { SettingsPanel.Apply -= value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// キャンセルボタンのクリック時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Cancel
        {
            add { SettingsPanel.Cancel += value; }
            remove { SettingsPanel.Cancel -= value; }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoad
        ///
        /// <summary>
        /// ロード時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnLoad(EventArgs ev)
        {
            // Association
            InitializeAssociate();

            AssociateAllButton.Click   += (s, e) => ResetAssociate(true);
            AssociateClearButton.Click += (s, e) => ResetAssociate(false);

            // ContextMenu
            ContextArchiveCheckBox.CheckedChanged += (s, e)
                => ContextArchivePanel.Enabled = ContextArchiveCheckBox.Checked;
            ContextExtractCheckBox.CheckedChanged += (s, e)
                => ContextExtractPanel.Enabled = ContextExtractCheckBox.Checked;
            ContextMailCheckBox.CheckedChanged += (s, e)
                => ContextMailPanel.Enabled = ContextMailCheckBox.Checked;

            // Shortcut
            DesktopArchiveCheckBox.CheckedChanged += (s, e)
                => DesktopArchiveComboBox.Enabled = DesktopArchiveCheckBox.Checked;

            // Version
            VersionPanel.Description = string.Empty;
            VersionPanel.Image       = Properties.Resources.Logo;
            VersionPanel.Uri         = new Uri(Properties.Resources.WebPage);
            VersionPanel.Location    = new Point(40, 40);

            VersionTabPage.Controls.Add(VersionPanel);

            // Buttons
            SettingsPanel.OKButton     = ExecuteButton;
            SettingsPanel.CancelButton = ExitButton;
            SettingsPanel.ApplyButton  = ApplyButton;

            base.OnLoad(ev);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeAssociate
        /// 
        /// <summary>
        /// 関連付けの項目を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeAssociate()
        {
            var index = 0;

            AssociateMenuPanel.Controls.AddRange(new[]
            {
                // well-known
                Create(Format.Zip,      "*.zip",  index++),
                Create(Format.Lzh,      "*.lzh",  index++),
                Create(Format.Rar,      "*.rar",  index++),
                Create(Format.SevenZip, "*.7z",   index++),
                Create(Format.Iso,      "*.iso",  index++),
                Create(Format.Tar,      "*.tar",  index++),
                Create(Format.GZip,     "*.gz",   index++),
                Create(Format.GZip,     "*.tgz",  index++),
                Create(Format.BZip2,    "*.bz2",  index++),
                Create(Format.BZip2,    "*.tbz",  index++),
                Create(Format.XZ,       "*.xz",   index++),
                Create(Format.XZ,       "*.txz",  index++),

                // others
                Create(Format.Arj,      "*.arj",  index++),
                Create(Format.Cab,      "*.cab",  index++),
                Create(Format.Chm,      "*.chm",  index++),
                Create(Format.Cpio,     "*.cpio", index++),
                Create(Format.Deb,      "*.deb",  index++),
                Create(Format.Dmg,      "*.dmg",  index++),
                Create(Format.Flv,      "*.flv",  index++),
                Create(Format.Zip,      "*.jar",  index++),
                Create(Format.Rpm,      "*.rpm",  index++),
                Create(Format.Swf,      "*.swf",  index++),
                Create(Format.Vhd,      "*.vhd",  index++),
                Create(Format.Vmdk,     "*.vmdk", index++),
                Create(Format.Wim,      "*.wim",  index++),
                Create(Format.Xar,      "*.xar",  index++),
                Create(Format.Lzw,      "*.z",    index++),
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResetAssociate
        /// 
        /// <summary>
        /// 全ての関連付け用項目を有効または無効状態に再設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetAssociate(bool check)
        {
            foreach (var obj in AssociateMenuPanel.Controls)
            {
                if (obj is CheckBox control) control.Checked = check;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// 関連付け用のチェックボックスを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CheckBox Create(Format format, string text, int index)
            => new CheckBox
            {
                AutoSize = false,
                Size     = new Size(68, 19),
                Text     = text,
                TabIndex = index,
                Tag      = format,
            };

        #region Fields
        private Cube.Forms.VersionControl VersionPanel = new Cube.Forms.VersionControl();
        #endregion

        #endregion
    }
}
