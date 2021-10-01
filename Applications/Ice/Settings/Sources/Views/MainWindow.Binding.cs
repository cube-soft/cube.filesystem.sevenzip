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
using System.Windows.Forms;
using Cube.Mixin.Forms.Controls;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    partial class MainWindow
    {
        /* ----------------------------------------------------------------- */
        ///
        /// BindCore
        ///
        /// <summary>
        /// Invokes the binding settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void BindCore(SettingViewModel vm)
        {
            // General
            var s0 = vm;
            var b0 = Behaviors.Hook(new BindingSource(s0, ""));
            b0.Bind(nameof(s0.Filters),      FilterTextBox,        nameof(TextBox.Text));
            b0.Bind(nameof(s0.ToolTip),      ToolTipCheckBox,      nameof(CheckBox.Checked));
            b0.Bind(nameof(s0.ToolTip),      ToolTipNumericUpDown, nameof(Enabled));
            b0.Bind(nameof(s0.ToolTipCount), ToolTipNumericUpDown, nameof(NumericUpDown.Value));
            b0.Bind(nameof(s0.CheckUpdate),  UpdateCheckBox,       nameof(CheckBox.Checked));

            // File association
            var b1 = Behaviors.Hook(new BindingSource(vm.Associate, ""));
            BindAssociate(b1, 0);

            // Context menu
            var s2 = vm.Menu;
            var b2 = Behaviors.Hook(new BindingSource(s2, ""));
            b2.Bind(nameof(s2.PresetEnabled), ContextPresetPanel,     nameof(Enabled));
            b2.Bind(nameof(s2.Compress),      ContextArchiveCheckBox, nameof(CheckBox.Checked));
            b2.Bind(nameof(s2.Compress),      ContextArchivePanel,    nameof(Enabled));
            b2.Bind(nameof(s2.Extract),       ContextExtractCheckBox, nameof(CheckBox.Checked));
            b2.Bind(nameof(s2.Extract),       ContextExtractPanel,    nameof(Enabled));
            BindContext(b2, 0);

            // Desktop shortcut
            var s3 = vm.Shortcut;
            var b3 = Behaviors.Hook(new BindingSource(s3, ""));
            b3.Bind(nameof(s3.Compress),       ShortcutArchiveCheckBox,  nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.Compress),       ShortcutArchiveComboBox,  nameof(Enabled));
            b3.Bind(nameof(s3.CompressOption), ShortcutArchiveComboBox,  nameof(ComboBox.SelectedValue));
            b3.Bind(nameof(s3.Extract),        ShortcutExtractCheckBox,  nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.Settings),       ShortcutSettingsCheckBox, nameof(CheckBox.Checked));
            ShortcutArchiveComboBox.Bind(Resource.Shortcuts);

            // Compression
            var s4 = vm.Compress;
            var b4 = Behaviors.Hook(new BindingSource(s4, ""));
            b4.Bind(nameof(s4.SaveSource),      ArchiveSaveSourceRadioButton , nameof(RadioButton.Checked));
            b4.Bind(nameof(s4.SaveQuery),       ArchiveSaveRuntimeRadioButton, nameof(RadioButton.Checked));
            b4.Bind(nameof(s4.SaveOthers),      ArchiveSaveOthersRadioButton,  nameof(RadioButton.Checked));
            b4.Bind(nameof(s4.SaveDirectory),   ArchiveSaveTextBox,            nameof(TextBox.Text));
            b4.Bind(nameof(s4.Filtering),       ArchiveFilterCheckBox,         nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.UseUtf8),         UseUtf8CheckBox,               nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.OverwritePrompt), OverwritePromptCheckBox,       nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.OpenDirectory),   ArchiveOpenDirectoryCheckBox,  nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.OpenDirectory),   ArchiveOpenSmartCheckBox,      nameof(Enabled));
            b4.Bind(nameof(s4.SkipDesktop),     ArchiveOpenSmartCheckBox,      nameof(CheckBox.Checked));

            // Extracting
            var s5 = vm.Extract;
            var b5 = Behaviors.Hook(new BindingSource(s5, ""));
            b5.Bind(nameof(s5.SaveSource),          ExtractSaveSourceRadioButton,   nameof(RadioButton.Checked));
            b5.Bind(nameof(s5.SaveQuery),           ExtractSaveRuntimeRadioButton,  nameof(RadioButton.Checked));
            b5.Bind(nameof(s5.SaveOthers),          ExtractSaveOthersRadioButton,   nameof(RadioButton.Checked));
            b5.Bind(nameof(s5.SaveDirectory),       ExtractSaveTextBox,             nameof(TextBox.Text));
            b5.Bind(nameof(s5.Filtering),           ExtractFilterCheckBox,          nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.DeleteSource),        ExtractDeleteSourceCheckBox,    nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.Bursty),              BurstyCheckBox,                 nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.CreateDirectory),     ExtractCreateDirectoryCheckBox, nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.CreateDirectory),     ExtractCreateSmartCheckBox,     nameof(Enabled));
            b5.Bind(nameof(s5.SkipSingleDirectory), ExtractCreateSmartCheckBox,     nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.OpenDirectory),       ExtractOpenDirectoryCheckBox,   nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.OpenDirectory),       ExtractOpenSmartCheckBox,       nameof(Enabled));
            b5.Bind(nameof(s5.SkipDesktop),         ExtractOpenSmartCheckBox,       nameof(CheckBox.Checked));
        }

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
        private void BindAssociate(BindingSource src, int index) => AssociateMenuPanel.Controls.AddRange(new[]
        {
            // well-known
            src.Bind(nameof(AssociateSetting.Zip),      "*.zip",   index++),
            src.Bind(nameof(AssociateSetting.Lzh),      "*.lzh",   index++),
            src.Bind(nameof(AssociateSetting.Rar),      "*.rar",   index++),
            src.Bind(nameof(AssociateSetting.SevenZip), "*.7z",    index++),
            src.Bind(nameof(AssociateSetting.Iso),      "*.iso",   index++),
            src.Bind(nameof(AssociateSetting.Tar),      "*.tar",   index++),
            src.Bind(nameof(AssociateSetting.Gz),       "*.gz",    index++),
            src.Bind(nameof(AssociateSetting.Tgz),      "*.tgz",   index++),
            src.Bind(nameof(AssociateSetting.Bz2),      "*.bz2",   index++),
            src.Bind(nameof(AssociateSetting.Tbz),      "*.tbz",   index++),
            src.Bind(nameof(AssociateSetting.Xz),       "*.xz",    index++),
            src.Bind(nameof(AssociateSetting.Txz),      "*.txz",   index++),

            // others
            src.Bind(nameof(AssociateSetting.Arj),      "*.arj",   index++),
            src.Bind(nameof(AssociateSetting.Cab),      "*.cab",   index++),
            src.Bind(nameof(AssociateSetting.Chm),      "*.chm",   index++),
            src.Bind(nameof(AssociateSetting.Cpio),     "*.cpio",  index++),
            src.Bind(nameof(AssociateSetting.Deb),      "*.deb",   index++),
            src.Bind(nameof(AssociateSetting.Dmg),      "*.dmg",   index++),
            src.Bind(nameof(AssociateSetting.Hfs),      "*.hfs",   index++),
            src.Bind(nameof(AssociateSetting.Jar),      "*.jar",   index++),
            src.Bind(nameof(AssociateSetting.Nupkg),    "*.nupkg", index++),
            src.Bind(nameof(AssociateSetting.Rpm),      "*.rpm",   index++),
            src.Bind(nameof(AssociateSetting.Vhd),      "*.vhd",   index++),
            src.Bind(nameof(AssociateSetting.Vmdk),     "*.vmdk",  index++),
            src.Bind(nameof(AssociateSetting.Wim),      "*.wim",   index++),
            src.Bind(nameof(AssociateSetting.Xar),      "*.xar",   index++),
            src.Bind(nameof(AssociateSetting.Z),        "*.z",     index++),
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
        private void BindContext(BindingSource src, int start)
        {
            ContextArchivePanel.Controls.AddRange(new[]
            {
                src.Bind(Preset.CompressZip,         Properties.Resources.MenuZip,         start++),
                src.Bind(Preset.CompressZipPassword, Properties.Resources.MenuZipPassword, start++),
                src.Bind(Preset.Compress7z,          Properties.Resources.MenuSevenZip,    start++),
                src.Bind(Preset.CompressBz2,         Properties.Resources.MenuBZip2,       start++),
                src.Bind(Preset.CompressGz,          Properties.Resources.MenuGZip,        start++),
                src.Bind(Preset.CompressXz,          Properties.Resources.MenuXZ,          start++),
                src.Bind(Preset.CompressSfx,         Properties.Resources.MenuSfx,         start++),
                src.Bind(Preset.CompressDetails,     Properties.Resources.MenuDetails,     start++),
            });

            ContextExtractPanel.Controls.AddRange(new[]
            {
                src.Bind(Preset.ExtractSource,      Properties.Resources.MenuHere,         start++),
                src.Bind(Preset.ExtractDesktop,     Properties.Resources.MenuDesktop,      start++),
                src.Bind(Preset.ExtractMyDocuments, Properties.Resources.MenuMyDocuments,  start++),
                src.Bind(Preset.ExtractQuery,       Properties.Resources.MenuRuntime,      start++),
            });
        }
    }
}
