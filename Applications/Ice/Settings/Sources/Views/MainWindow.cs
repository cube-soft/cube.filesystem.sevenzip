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
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// 設定画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : Cube.Forms.Window
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
        public MainWindow()
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
        public MainWindow(bool install) : this()
        {
            if (!install) return;

            ExitButton.Enabled  = false;
            ApplyButton.Enabled = true;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnBind
        ///
        /// <summary>
        /// Binds the windows to the specified object.
        /// </summary>
        ///
        /// <param name="src">Binding object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            base.OnBind(src);
            if (src is not SettingViewModel vm) return;

            SettingsBindingSource.DataSource          = vm;
            AssociateSettingsBindingSource.DataSource = vm.Associate;
            ContextSettingsBindingSource.DataSource   = vm.Menu;
            ShortcutSettingsBindingSource.DataSource  = vm.Shortcut;
            ArchiveSettingsBindingSource.DataSource   = vm.Compress;
            ExtractSettingsBindingSource.DataSource   = vm.Extract;
            VersionPanel.Version                      = vm.Version;

            Enable(ArchiveSaveOthersRadioButton, ArchiveSaveTextBox, ArchiveSaveButton);
            Enable(ExtractSaveOthersRadioButton, ExtractSaveTextBox, ExtractSaveButton);

            SettingsPanel.Apply          += (s, e) => vm.Update();
            ContextResetButton.Click     += (s, e) => vm.Menu.Reset();
            ContextCustomizeButton.Click += (s, e) => vm.Menu.Customize();
            AssociateAllButton.Click     += (s, e) => vm.Associate.SelectAll();
            AssociateClearButton.Click   += (s, e) => vm.Associate.Clear();
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
                Create(nameof(AssociateSetting.Zip),      "*.zip",   index++),
                Create(nameof(AssociateSetting.Lzh),      "*.lzh",   index++),
                Create(nameof(AssociateSetting.Rar),      "*.rar",   index++),
                Create(nameof(AssociateSetting.SevenZip), "*.7z",    index++),
                Create(nameof(AssociateSetting.Iso),      "*.iso",   index++),
                Create(nameof(AssociateSetting.Tar),      "*.tar",   index++),
                Create(nameof(AssociateSetting.GZ),       "*.gz",    index++),
                Create(nameof(AssociateSetting.Tgz),      "*.tgz",   index++),
                Create(nameof(AssociateSetting.BZ2),      "*.bz2",   index++),
                Create(nameof(AssociateSetting.Tbz),      "*.tbz",   index++),
                Create(nameof(AssociateSetting.XZ),       "*.xz",    index++),
                Create(nameof(AssociateSetting.Txz),      "*.txz",   index++),

                // others
                Create(nameof(AssociateSetting.Arj),      "*.arj",   index++),
                Create(nameof(AssociateSetting.Cab),      "*.cab",   index++),
                Create(nameof(AssociateSetting.Chm),      "*.chm",   index++),
                Create(nameof(AssociateSetting.Cpio),     "*.cpio",  index++),
                Create(nameof(AssociateSetting.Deb),      "*.deb",   index++),
                Create(nameof(AssociateSetting.Dmg),      "*.dmg",   index++),
                Create(nameof(AssociateSetting.Hfs),      "*.hfs",   index++),
                Create(nameof(AssociateSetting.Jar),      "*.jar",   index++),
                Create(nameof(AssociateSetting.Nupkg),    "*.nupkg", index++),
                Create(nameof(AssociateSetting.Rpm),      "*.rpm",   index++),
                Create(nameof(AssociateSetting.Vhd),      "*.vhd",   index++),
                Create(nameof(AssociateSetting.Vmdk),     "*.vmdk",  index++),
                Create(nameof(AssociateSetting.Wim),      "*.wim",   index++),
                Create(nameof(AssociateSetting.Xar),      "*.xar",   index++),
                Create(nameof(AssociateSetting.Z),        "*.z",     index++),
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
                Create(Preset.CompressZip,         Properties.Resources.MenuZip,         index++),
                Create(Preset.CompressZipPassword, Properties.Resources.MenuZipPassword, index++),
                Create(Preset.CompressSevenZip,    Properties.Resources.MenuSevenZip,    index++),
                Create(Preset.CompressBZip2,       Properties.Resources.MenuBZip2,       index++),
                Create(Preset.CompressGZip,        Properties.Resources.MenuGZip,        index++),
                Create(Preset.CompressXz,          Properties.Resources.MenuXZ,          index++),
                Create(Preset.CompressSfx,         Properties.Resources.MenuSfx,         index++),
                Create(Preset.CompressOthers,     Properties.Resources.MenuDetails,      index++),
            });

            ContextExtractPanel.Controls.AddRange(new[]
            {
                Create(Preset.ExtractSource,      Properties.Resources.MenuHere,        index++),
                Create(Preset.ExtractDesktop,     Properties.Resources.MenuDesktop,     index++),
                Create(Preset.ExtractMyDocuments, Properties.Resources.MenuMyDocuments, index++),
                Create(Preset.ExtractRuntime,     Properties.Resources.MenuRuntime,     index++),
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
            ShortcutArchiveComboBox.DataSource    = new List<KeyValuePair<string, Preset>>
            {
                Create(Properties.Resources.MenuZip,         Preset.CompressZip),
                Create(Properties.Resources.MenuZipPassword, Preset.CompressZipPassword),
                Create(Properties.Resources.MenuSevenZip,    Preset.CompressSevenZip),
                Create(Properties.Resources.MenuBZip2,       Preset.CompressBZip2),
                Create(Properties.Resources.MenuGZip,        Preset.CompressGZip),
                Create(Properties.Resources.MenuSfx,         Preset.CompressSfx),
                Create(Properties.Resources.MenuDetails,     Preset.CompressOthers),
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
        private CheckBox Create(Preset menu, string text, int index)
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
        private KeyValuePair<string, Preset> Create(string key, Preset value) =>
            new KeyValuePair<string, Preset>(key, value);

        #endregion

        #endregion

        #region Fields
        private readonly Cube.Forms.Controls.VersionControl VersionPanel = new(typeof(MainWindow).Assembly);
        #endregion
    }
}
