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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
            InitializeShortcut();

            VersionTabPage.Controls.Add(VersionPanel);
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
        /// <param name="vm">ViewModel オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Bind(SettingsViewModel vm)
        {
            if (vm == null) return;

            SettingsBindingSource.DataSource          = vm;
            AssociateSettingsBindingSource.DataSource = vm.Associate;
            ContextSettingsBindingSource.DataSource   = vm.Context;
            ShortcutSettingsBindingSource.DataSource  = vm.Shortcut;
            ArchiveSettingsBindingSource.DataSource   = vm.Archive;
            ExtractSettingsBindingSource.DataSource   = vm.Extract;

            SettingsPanel.Apply += (s, e) => vm.Save();
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

        #region Initialize

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

            ContextExtractPanel.Controls.AddRange(new[]
            {
                Create(PresetMenu.ExtractSource,      Properties.Resources.MenuHere,        index++),
                Create(PresetMenu.ExtractDesktop,     Properties.Resources.MenuDesktop,     index++),
                Create(PresetMenu.ExtractMyDocuments, Properties.Resources.MenuMyDocuments, index++),
                Create(PresetMenu.ExtractRuntime,     Properties.Resources.MenuRuntime,     index++),
            });

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
        /// InitializeShortcut
        /// 
        /// <summary>
        /// ショートカットメニューを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeShortcut()
        {
            ShortcutArchiveComboBox.DisplayMember = "Key";
            ShortcutArchiveComboBox.ValueMember   = "Value";
            ShortcutArchiveComboBox.DataSource    = new List<KeyValuePair<string, PresetMenu>>
            {
                Create(Properties.Resources.MenuZip,         PresetMenu.ArchiveZip),
                Create(Properties.Resources.MenuZipPassword, PresetMenu.ArchiveZipPassword),
                Create(Properties.Resources.MenuSevenZip,    PresetMenu.ArchiveSevenZip),
                Create(Properties.Resources.MenuBZip2,       PresetMenu.ArchiveBZip2),
                Create(Properties.Resources.MenuGZip,        PresetMenu.ArchiveGZip),
                Create(Properties.Resources.MenuSfx,         PresetMenu.ArchiveSfx),
                Create(Properties.Resources.MenuDetail,      PresetMenu.ArchiveDetail),
            };
        }

        #endregion

        #region Reset

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

        #endregion

        #region Create

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// KeyValuePaier オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private KeyValuePair<string, PresetMenu> Create(string key, PresetMenu value)
            => new KeyValuePair<string, PresetMenu>(key, value);

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

            dest.DataBindings.Add(new Binding(nameof(dest.Checked),
                AssociateSettingsBindingSource, name, false,
                DataSourceUpdateMode.OnPropertyChanged
            ));

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

            dest.DataBindings.Add(new Binding(nameof(dest.Checked),
                ContextSettingsBindingSource, menu.ToString(), false,
                DataSourceUpdateMode.OnPropertyChanged
            ));

            return dest;
        }

        #endregion

        #region Fields
        private Cube.Forms.VersionControl VersionPanel = new Cube.Forms.VersionControl();
        #endregion

        #endregion
    }
}
