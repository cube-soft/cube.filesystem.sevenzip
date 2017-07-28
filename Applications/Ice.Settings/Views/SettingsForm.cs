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
using System.ComponentModel;
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
    public partial class SettingsForm : Cube.Forms.FormBase, ISettingsView
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

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// Product
        /// 
        /// <summary>
        /// アプリケーション名を取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [Browsable(true)]
        public string Product
        {
            get { return VersionPanel.Product; }
            set { VersionPanel.Product = value; }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Version
        /// 
        /// <summary>
        /// バージョン情報を取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [Browsable(true)]
        public string Version
        {
            get { return VersionPanel.Version; }
            set { VersionPanel.Version = value; }
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

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// オブジェクトを関連付けます。
        /// </summary>
        /// 
        /// <param name="settings">関連付けるオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Bind(Cube.FileSystem.Ice.Settings settigns)
        {
            if (settigns == null) return;

            SettingsBindingSource.DataSource          = settigns;
            AssociateSettingsBindingSource.DataSource = settigns.Associate;
            ContextSetttingsBindingSource.DataSource  = settigns.Context;
            ShortcutSettingsBindingSource.DataSource  = settigns.Shortcut;
            ArchiveSettingsBindingSource.DataSource   = settigns.Archive;
            ExtractSettingsBindingSource.DataSource   = settigns.Extract;
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
            ShortcutArchiveCheckBox.CheckedChanged += WhenShortcutChanged;
            ShortcutExtractCheckBox.CheckedChanged += WhenShortcutChanged;
            ShortcutSettingsCheckBox.CheckedChanged += WhenShortcutChanged;

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
                Create(nameof(AssociateSettings.Zip),      "*.zip",  index++),
                Create(nameof(AssociateSettings.Lzh),      "*.lzh",  index++),
                Create(nameof(AssociateSettings.Rar),      "*.rar",  index++),
                Create(nameof(AssociateSettings.SevenZip), "*.7z",   index++),
                Create(nameof(AssociateSettings.Iso),      "*.iso",  index++),
                Create(nameof(AssociateSettings.Tar),      "*.tar",  index++),
                Create(nameof(AssociateSettings.GZ),       "*.gz",   index++),
                Create(nameof(AssociateSettings.Tgz),      "*.tgz",  index++),
                Create(nameof(AssociateSettings.BZ2),      "*.bz2",  index++),
                Create(nameof(AssociateSettings.Tbz),      "*.tbz",  index++),
                Create(nameof(AssociateSettings.XZ),       "*.xz",   index++),
                Create(nameof(AssociateSettings.Txz),      "*.txz",  index++),

                // others
                Create(nameof(AssociateSettings.Arj),      "*.arj",  index++),
                Create(nameof(AssociateSettings.Cab),      "*.cab",  index++),
                Create(nameof(AssociateSettings.Chm),      "*.chm",  index++),
                Create(nameof(AssociateSettings.Cpio),     "*.cpio", index++),
                Create(nameof(AssociateSettings.Deb),      "*.deb",  index++),
                Create(nameof(AssociateSettings.Dmg),      "*.dmg",  index++),
                Create(nameof(AssociateSettings.Flv),      "*.flv",  index++),
                Create(nameof(AssociateSettings.Jar),      "*.jar",  index++),
                Create(nameof(AssociateSettings.Rpm),      "*.rpm",  index++),
                Create(nameof(AssociateSettings.Swf),      "*.swf",  index++),
                Create(nameof(AssociateSettings.Vhd),      "*.vhd",  index++),
                Create(nameof(AssociateSettings.Vmdk),     "*.vmdk", index++),
                Create(nameof(AssociateSettings.Wim),      "*.wim",  index++),
                Create(nameof(AssociateSettings.Xar),      "*.xar",  index++),
                Create(nameof(AssociateSettings.Z),        "*.z",    index++),
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
        /// Update
        /// 
        /// <summary>
        /// GroupBox のデータを更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update(CheckBox src, GroupBox dest)
        {
            if (src == null || dest == null) return;
            if (src.Tag is PresetMenu st)
            {
                var dt = dest.Tag is PresetMenu ? (PresetMenu)dest.Tag : PresetMenu.None;
                if (src.Checked) dt |= st;
                else dt &= ~st;
                dest.Tag = dt;
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
        private CheckBox Create(string name, string text, int index)
        {
            var dest = new CheckBox
            {
                AutoSize = false,
                Size     = new Size(70, 19),
                Text     = text,
                TabIndex = index,
            };

            dest.DataBindings.Add(new Binding(nameof(dest.Checked), AssociateSettingsBindingSource, name, true));
            return dest;
        }

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
        {
            var dest = new CheckBox
            {
                AutoSize  = true,
                Text      = text,
                TabIndex  = index,
                Tag       = menu,
                TextAlign = ContentAlignment.MiddleLeft,
            };

            dest.CheckedChanged += (s, e) => Update(s as CheckBox, ContextGroupBox);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenShortcutChanged
        /// 
        /// <summary>
        /// ショートカットメニュー変更時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenShortcutChanged(object sender, EventArgs e)
        {
            Update(sender as CheckBox, ShortcutGroupBox);
        }

        #region Fields
        private Cube.Forms.VersionControl VersionPanel = new Cube.Forms.VersionControl();
        #endregion

        #endregion
    }
}
