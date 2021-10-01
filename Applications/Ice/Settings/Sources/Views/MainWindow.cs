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
using System.Drawing;
using System.Windows.Forms;
using Cube.Forms.Behaviors;
using Cube.Mixin.Forms.Controls;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainWindow
    ///
    /// <summary>
    /// Represetns the settings window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class MainWindow : Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainWindow
        ///
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MainWindow()
        {
            InitializeComponent();

            _version.Description = string.Empty;
            _version.Image       = Properties.Resources.Logo;
            _version.Uri         = new(Properties.Resources.WebPage);
            _version.Location    = new(40, 40);
            _version.Size        = new(400, 300);
            VersionTabPage.Controls.Add(_version);

            SettingsPanel.OKButton     = ExecuteButton;
            SettingsPanel.CancelButton = ExitButton;
            SettingsPanel.ApplyButton  = ApplyButton;

            BindAssociate(0);
            BindContext(0);
            ShortcutArchiveComboBox.Bind(Resource.Shortcuts);
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
            if (src is not SettingViewModel vm) return;

            MainBindingSource.DataSource      = vm;
            AssociateBindingSource.DataSource = vm.Associate;
            ContextBindingSource.DataSource   = vm.Menu;
            ShortcutBindingSource.DataSource  = vm.Shortcut;
            CompressBindingSource.DataSource  = vm.Compress;
            ExtractBindingSource.DataSource   = vm.Extract;
            _version.Version                  = vm.Version;

            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenDirectoryBehavior(vm));
            Behaviors.Add(new EventBehavior(SettingsPanel, "Apply", vm.Save));
            Behaviors.Add(new ClickBehavior(ContextResetButton, vm.Menu.Reset));
            Behaviors.Add(new ClickBehavior(ContextCustomizeButton, vm.Menu.Customize));
            Behaviors.Add(new ClickBehavior(AssociateAllButton, vm.Associate.SelectAll));
            Behaviors.Add(new ClickBehavior(AssociateClearButton, vm.Associate.Clear));
            Behaviors.Add(new ClickBehavior(ArchiveSaveButton, vm.Compress.Browse));
            Behaviors.Add(new ClickBehavior(ExtractSaveButton, vm.Extract.Browse));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// BindAssociate
        ///
        /// <summary>
        /// Creates view components for file associations and invokes the
        /// binding settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void BindAssociate(int index) => AssociateMenuPanel.Controls.AddRange(new[]
        {
            // well-known
            Bind(nameof(AssociateSetting.Zip),      "*.zip",   index++),
            Bind(nameof(AssociateSetting.Lzh),      "*.lzh",   index++),
            Bind(nameof(AssociateSetting.Rar),      "*.rar",   index++),
            Bind(nameof(AssociateSetting.SevenZip), "*.7z",    index++),
            Bind(nameof(AssociateSetting.Iso),      "*.iso",   index++),
            Bind(nameof(AssociateSetting.Tar),      "*.tar",   index++),
            Bind(nameof(AssociateSetting.Gz),       "*.gz",    index++),
            Bind(nameof(AssociateSetting.Tgz),      "*.tgz",   index++),
            Bind(nameof(AssociateSetting.Bz2),      "*.bz2",   index++),
            Bind(nameof(AssociateSetting.Tbz),      "*.tbz",   index++),
            Bind(nameof(AssociateSetting.Xz),       "*.xz",    index++),
            Bind(nameof(AssociateSetting.Txz),      "*.txz",   index++),

            // others
            Bind(nameof(AssociateSetting.Arj),      "*.arj",   index++),
            Bind(nameof(AssociateSetting.Cab),      "*.cab",   index++),
            Bind(nameof(AssociateSetting.Chm),      "*.chm",   index++),
            Bind(nameof(AssociateSetting.Cpio),     "*.cpio",  index++),
            Bind(nameof(AssociateSetting.Deb),      "*.deb",   index++),
            Bind(nameof(AssociateSetting.Dmg),      "*.dmg",   index++),
            Bind(nameof(AssociateSetting.Hfs),      "*.hfs",   index++),
            Bind(nameof(AssociateSetting.Jar),      "*.jar",   index++),
            Bind(nameof(AssociateSetting.Nupkg),    "*.nupkg", index++),
            Bind(nameof(AssociateSetting.Rpm),      "*.rpm",   index++),
            Bind(nameof(AssociateSetting.Vhd),      "*.vhd",   index++),
            Bind(nameof(AssociateSetting.Vmdk),     "*.vmdk",  index++),
            Bind(nameof(AssociateSetting.Wim),      "*.wim",   index++),
            Bind(nameof(AssociateSetting.Xar),      "*.xar",   index++),
            Bind(nameof(AssociateSetting.Z),        "*.z",     index++),
        });

        /* ----------------------------------------------------------------- */
        ///
        /// BindContext
        ///
        /// <summary>
        /// Creates view components for file associations and invokes the
        /// binding settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void BindContext(int index)
        {
            ContextArchivePanel.Controls.AddRange(new[]
            {
                Bind(Preset.CompressZip,         Properties.Resources.MenuZip,         index++),
                Bind(Preset.CompressZipPassword, Properties.Resources.MenuZipPassword, index++),
                Bind(Preset.Compress7z,          Properties.Resources.MenuSevenZip,    index++),
                Bind(Preset.CompressBz2,         Properties.Resources.MenuBZip2,       index++),
                Bind(Preset.CompressGz,          Properties.Resources.MenuGZip,        index++),
                Bind(Preset.CompressXz,          Properties.Resources.MenuXZ,          index++),
                Bind(Preset.CompressSfx,         Properties.Resources.MenuSfx,         index++),
                Bind(Preset.CompressDetails,     Properties.Resources.MenuDetails,     index++),
            });

            ContextExtractPanel.Controls.AddRange(new[]
            {
                Bind(Preset.ExtractSource,      Properties.Resources.MenuHere,         index++),
                Bind(Preset.ExtractDesktop,     Properties.Resources.MenuDesktop,      index++),
                Bind(Preset.ExtractMyDocuments, Properties.Resources.MenuMyDocuments,  index++),
                Bind(Preset.ExtractQuery,       Properties.Resources.MenuRuntime,      index++),
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Creates a CheckBox object for the file association and invokes
        /// the binding settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CheckBox Bind(string name, string text, int index)
        {
            var src = AssociateBindingSource;
            var dest = new CheckBox
            {
                AutoSize = false,
                Size     = new Size(75, 19),
                Margin   = new Padding(0, 3, 0, 3),
                Padding  = new Padding(0),
                Text     = text,
                TabIndex = index,
            };

            dest.DataBindings.Add(new(nameof(dest.Checked),
                src,
                name,
                true,
                DataSourceUpdateMode.OnPropertyChanged
            ));

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Creates a CheckBox object for the context menu and invokes the
        /// binding settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CheckBox Bind(Preset menu, string text, int index)
        {
            var src = ContextBindingSource;
            var dest = new CheckBox
            {
                AutoSize  = true,
                Text      = text,
                TabIndex  = index,
                Tag       = menu,
                TextAlign = ContentAlignment.MiddleLeft,
            };

            dest.DataBindings.Add(new(nameof(dest.Checked),
                src,
                menu.ToString(),
                true,
                DataSourceUpdateMode.OnPropertyChanged
            ));

            return dest;
        }

        #endregion

        #region Fields
        private readonly Forms.Controls.VersionControl _version = new(typeof(MainWindow).Assembly);
        #endregion
    }
}
