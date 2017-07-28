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
using Cube.FileSystem.Ice;

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
            InitializeAssociate();
            InitializeContext();

            VersionTabPage.Controls.Add(VersionPanel);
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
            AssociateAllButton.Click   += (s, e) => Reset(AssociateMenuPanel, true);
            AssociateClearButton.Click += (s, e) => Reset(AssociateMenuPanel, false);

            // ContextMenu
            ContextArchiveCheckBox.CheckedChanged += (s, e)
                => ContextArchivePanel.Enabled = ContextArchiveCheckBox.Checked;
            ContextExtractCheckBox.CheckedChanged += (s, e)
                => ContextExtractPanel.Enabled = ContextExtractCheckBox.Checked;
            ContextMailCheckBox.CheckedChanged += (s, e)
                => ContextMailPanel.Enabled = ContextMailCheckBox.Checked;
            ContextResetButton.Click += (s, e) => ResetContext();

            // Shortcut
            ShortcutArchiveCheckBox.CheckedChanged += (s, e)
                => ShortcutArchiveComboBox.Enabled = ShortcutArchiveCheckBox.Checked;

            // Archive
            ArchiveOpenDirectoryCheckBox.CheckedChanged += (s, e)
                => ArchiveOpenSmartCheckBox.Enabled = ArchiveOpenDirectoryCheckBox.Checked;
            ArchiveSaveOthersRadioButton.CheckedChanged += (s, e)
                => ArchiveSaveTextBox.Enabled =
                   ArchiveSaveButton.Enabled = ArchiveSaveOthersRadioButton.Checked;

            // Extract
            ExtractCreateDirectoryCheckBox.CheckedChanged += (s, e)
                => ExtractCreateSmartCheckBox.Enabled = ExtractCreateDirectoryCheckBox.Checked;
            ExtractOpenDirectoryCheckBox.CheckedChanged += (s, e)
                => ExtractOpenSmartCheckBox.Enabled = ExtractOpenDirectoryCheckBox.Checked;
            ExtractSaveOthersRadioButton.CheckedChanged += (s, e)
                => ExtractSaveTextBox.Enabled =
                   ExtractSaveButton.Enabled = ExtractSaveOthersRadioButton.Checked;

            // Details
            ToolTipCheckBox.CheckedChanged += (s, e)
                => ToolTipNumericUpDown.Enabled = ToolTipCheckBox.Checked;

            // Version
            VersionPanel.Description = string.Empty;
            VersionPanel.Image       = Properties.Resources.Logo;
            VersionPanel.Uri         = new Uri(Properties.Resources.WebPage);
            VersionPanel.Location    = new Point(40, 40);
            VersionPanel.Size        = new Size(400, 300);

            // Buttons
            SettingsPanel.OKButton     = ExecuteButton;
            SettingsPanel.CancelButton = ExitButton;
            SettingsPanel.ApplyButton  = ApplyButton;

            base.OnLoad(ev);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// 表示時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            var area = Screen.FromControl(this).WorkingArea;
            if (Height > area.Height) Size = new Size(MaximumSize.Width, area.Height);
            base.OnShown(e);
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
        /// InitializeContext
        /// 
        /// <summary>
        /// コンテキストメニューの項目を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeContext()
        {
            var index = 0;

            ContextArchiveCheckBox.Tag = PresetMenu.Archive;
            ContextArchivePanel.Controls.AddRange(new[]
            {
                Create(PresetMenu.ArchiveZip,         Properties.Resources.MenuZip,         index++),
                Create(PresetMenu.ArchiveZipPassword, Properties.Resources.MenuZipPassword, index++),
                Create(PresetMenu.ArchiveSevenZip,    Properties.Resources.MenuSevenZip,    index++),
                Create(PresetMenu.ArchiveBZip2,       Properties.Resources.MenuBZip2,       index++),
                Create(PresetMenu.ArchiveGZip,        Properties.Resources.MenuGZip,        index++),
                Create(PresetMenu.ArchiveSfx,         Properties.Resources.MenuSfx,         index++),
                Create(PresetMenu.ArchiveDetail,      Properties.Resources.MenuDetail,      index++),
            });

            ContextExtractCheckBox.Tag = PresetMenu.Extract;
            ContextExtractPanel.Controls.AddRange(new[]
            {
                Create(PresetMenu.ExtractSource,      Properties.Resources.MenuHere,        index++),
                Create(PresetMenu.ExtractDesktop,     Properties.Resources.MenuDesktop,     index++),
                Create(PresetMenu.ExtractMyDocuments, Properties.Resources.MenuMyDocuments, index++),
                Create(PresetMenu.ExtractRuntime,     Properties.Resources.MenuRuntime,     index++),
            });

            ContextMailCheckBox.Tag = PresetMenu.Mail;
            ContextMailPanel.Controls.AddRange(new[]
            {
                Create(PresetMenu.MailZip,            Properties.Resources.MenuZip,         index++),
                Create(PresetMenu.MailZipPassword,    Properties.Resources.MenuZipPassword, index++),
                Create(PresetMenu.MailSevenZip,       Properties.Resources.MenuSevenZip,    index++),
                Create(PresetMenu.MailBZip2,          Properties.Resources.MenuBZip2,       index++),
                Create(PresetMenu.MailGZip,           Properties.Resources.MenuGZip,        index++),
                Create(PresetMenu.MailSfx,            Properties.Resources.MenuSfx,         index++),
                Create(PresetMenu.MailDetail,         Properties.Resources.MenuDetail,      index++),
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        /// 
        /// <summary>
        /// コントロールが保持している CheckBox オブジェクトの
        /// チェック状態を再設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset(Control container, bool check)
        {
            foreach (var control in container.Controls)
            {
                if (control is CheckBox cb) cb.Checked = check;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResetContext
        /// 
        /// <summary>
        /// コンテキストメニューの設定状態をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetContext()
        {
            ContextArchiveCheckBox.Checked = true;
            ContextExtractCheckBox.Checked = true;
            ContextMailCheckBox.Checked    = false;

            Reset(ContextArchivePanel,  true);
            Reset(ContextExtractPanel,  true);
            Reset(ContextMailPanel,    false);
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
                Size     = new Size(70, 19),
                Text     = text,
                TabIndex = index,
                Tag      = format,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// コンテキストメニュー用のチェックボックスを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CheckBox Create(PresetMenu menu, string text, int index)
            => new CheckBox
            {
                AutoSize  = true,
                Text      = text,
                TabIndex  = index,
                Tag       = menu,
                TextAlign = ContentAlignment.MiddleLeft,
            };

        #region Fields
        private Cube.Forms.VersionControl VersionPanel = new Cube.Forms.VersionControl();
        #endregion

        #endregion
    }
}
