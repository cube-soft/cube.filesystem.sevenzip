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
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainForm
    ///
    /// <summary>
    /// 設定画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainForm : Cube.Forms.StandardForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm()
        {
            InitializeComponent();
            InitializeAssociate();
            InitializeContext();
            InitializeShortcut();

            VersionTabPage.Controls.Add(VersionPanel);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MainForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="install">インストールモードかどうか</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainForm(bool install) : this()
        {
            if (!install) return;

            ExitButton.Enabled  = false;
            ApplyButton.Enabled = true;
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
        public void Bind(MainViewModel vm)
        {
            if (vm == null) return;

            SettingsBindingSource.DataSource          = vm;
            AssociateSettingsBindingSource.DataSource = vm.Associate;
            ContextSettingsBindingSource.DataSource   = vm.Menu;
            ShortcutSettingsBindingSource.DataSource  = vm.Shortcut;
            ArchiveSettingsBindingSource.DataSource   = vm.Archive;
            ExtractSettingsBindingSource.DataSource   = vm.Extract;
            VersionPanel.Version                      = vm.Version;

            Enable(ArchiveSaveOthersRadioButton, ArchiveSaveTextBox, ArchiveSaveButton);
            Enable(ExtractSaveOthersRadioButton, ExtractSaveTextBox, ExtractSaveButton);

            SettingsPanel.Apply          += (s, e) => vm.Update();
            ContextResetButton.Click     += (s, e) => vm.Menu.Reset();
            AssociateAllButton.Click     += (s, e) => vm.Associate.SelectAll();
            AssociateClearButton.Click   += (s, e) => vm.Associate.Clear();
            ContextCustomizeButton.Click += (s, e) => vm.Menu.Customize();
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
            // Archive
            ArchiveSaveButton.Click += (s, e) => Browse(ArchiveSaveTextBox);
            ArchiveSaveOthersRadioButton.CheckedChanged += (s, e)
                => Enable(s, ArchiveSaveTextBox, ArchiveSaveButton);

            // Extract
            ExtractSaveButton.Click += (s, e) => Browse(ExtractSaveTextBox);
            ExtractSaveOthersRadioButton.CheckedChanged += (s, e)
                => Enable(s, ExtractSaveTextBox, ExtractSaveButton);

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
        /// Enable
        ///
        /// <summary>
        /// 指定されたコントロールの Enabled を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Enable(object s, params Control[] controls)
        {
            if (!(s is RadioButton src)) return;
            foreach (var c in controls) c.Enabled = src.Checked;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Browse
        ///
        /// <summary>
        /// 保存ディレクトリを選択するためのダイアログを表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Browse(TextBox dest)
        {
            var dialog = new FolderBrowserDialog
            {
                Description         = Properties.Resources.MessageSave,
                SelectedPath        = dest.Text,
                ShowNewFolderButton = true,
            };

            if (dialog.ShowDialog() == DialogResult.Cancel) return;
            dest.Text = dialog.SelectedPath;
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
                Create(nameof(AssociateSettings.Zip),      "*.zip",   index++),
                Create(nameof(AssociateSettings.Lzh),      "*.lzh",   index++),
                Create(nameof(AssociateSettings.Rar),      "*.rar",   index++),
                Create(nameof(AssociateSettings.SevenZip), "*.7z",    index++),
                Create(nameof(AssociateSettings.Iso),      "*.iso",   index++),
                Create(nameof(AssociateSettings.Tar),      "*.tar",   index++),
                Create(nameof(AssociateSettings.GZ),       "*.gz",    index++),
                Create(nameof(AssociateSettings.Tgz),      "*.tgz",   index++),
                Create(nameof(AssociateSettings.BZ2),      "*.bz2",   index++),
                Create(nameof(AssociateSettings.Tbz),      "*.tbz",   index++),
                Create(nameof(AssociateSettings.XZ),       "*.xz",    index++),
                Create(nameof(AssociateSettings.Txz),      "*.txz",   index++),

                // others
                Create(nameof(AssociateSettings.Arj),      "*.arj",   index++),
                Create(nameof(AssociateSettings.Cab),      "*.cab",   index++),
                Create(nameof(AssociateSettings.Chm),      "*.chm",   index++),
                Create(nameof(AssociateSettings.Cpio),     "*.cpio",  index++),
                Create(nameof(AssociateSettings.Deb),      "*.deb",   index++),
                Create(nameof(AssociateSettings.Dmg),      "*.dmg",   index++),
                Create(nameof(AssociateSettings.Hfs),      "*.hfs",   index++),
                Create(nameof(AssociateSettings.Jar),      "*.jar",   index++),
                Create(nameof(AssociateSettings.Nupkg),    "*.nupkg", index++),
                Create(nameof(AssociateSettings.Rpm),      "*.rpm",   index++),
                Create(nameof(AssociateSettings.Vhd),      "*.vhd",   index++),
                Create(nameof(AssociateSettings.Vmdk),     "*.vmdk",  index++),
                Create(nameof(AssociateSettings.Wim),      "*.wim",   index++),
                Create(nameof(AssociateSettings.Xar),      "*.xar",   index++),
                Create(nameof(AssociateSettings.Z),        "*.z",     index++),
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
                Create(PresetMenu.ArchiveXz,          Properties.Resources.MenuXZ,          index++),
                Create(PresetMenu.ArchiveSfx,         Properties.Resources.MenuSfx,         index++),
                Create(PresetMenu.ArchiveDetails,     Properties.Resources.MenuDetails,      index++),
            });

            ContextExtractPanel.Controls.AddRange(new[]
            {
                Create(PresetMenu.ExtractSource,      Properties.Resources.MenuHere,        index++),
                Create(PresetMenu.ExtractDesktop,     Properties.Resources.MenuDesktop,     index++),
                Create(PresetMenu.ExtractMyDocuments, Properties.Resources.MenuMyDocuments, index++),
                Create(PresetMenu.ExtractRuntime,     Properties.Resources.MenuRuntime,     index++),
            });

            ContextCustomizeButton.Click += (s, e) => ApplyButton.Enabled = true;
            ContextResetButton.Click     += (s, e) => ApplyButton.Enabled = true;
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
                Create(Properties.Resources.MenuDetails,     PresetMenu.ArchiveDetails),
            };
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
                Size     = new Size(75, 19),
                Margin   = new Padding(0, 3, 0, 3),
                Padding  = new Padding(0),
                Text     = text,
                TabIndex = index,
            };

            dest.DataBindings.Add(new Binding(nameof(dest.Checked),
                AssociateSettingsBindingSource, name, true,
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
                ContextSettingsBindingSource, menu.ToString(), true,
                DataSourceUpdateMode.OnPropertyChanged
            ));

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// KeyValuePaier オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private KeyValuePair<string, PresetMenu> Create(string key, PresetMenu value) =>
            new KeyValuePair<string, PresetMenu>(key, value);

        #endregion

        #endregion

        #region Fields
        private readonly Cube.Forms.VersionControl VersionPanel = new Cube.Forms.VersionControl(Assembly.GetExecutingAssembly());
        #endregion
    }
}
