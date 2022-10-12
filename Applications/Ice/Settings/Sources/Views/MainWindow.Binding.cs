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
using Cube.Forms.Binding;

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
        /// <param name="vm">VM object to bind.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void BindCore(SettingViewModel vm)
        {
            // General
            var s0 = vm;
            var b0 = Behaviors.Hook(new BindingSource(s0, ""));
            b0.Bind(nameof(s0.Filters),      FilterTextBox,     nameof(TextBox.Text));
            b0.Bind(nameof(s0.ToolTip),      ToolTipCheckBox,   nameof(CheckBox.Checked));
            b0.Bind(nameof(s0.ToolTip),      ToolTipNumeric,    nameof(Enabled));
            b0.Bind(nameof(s0.ToolTipCount), ToolTipNumeric,    nameof(NumericUpDown.Value));
            b0.Bind(nameof(s0.AlphaFS),      IoHandlerComboBox, nameof(ComboBox.SelectedValue));
            b0.Bind(nameof(s0.Temp),         TempTextBox,       nameof(TextBox.Text));
            b0.Bind(nameof(s0.CheckUpdate),  UpdateCheckBox,    nameof(CheckBox.Checked));
            IoHandlerComboBox.Bind(Resource.IoHandlers);

            // File association
            var b1 = Behaviors.Hook(new BindingSource(vm.Associate, ""));
            BindAssociate(b1, 0);

            // Context menu
            var s2 = vm.Menu;
            var b2 = Behaviors.Hook(new BindingSource(s2, ""));
            b2.Bind(nameof(s2.UsePreset), ContextPresetPanel,      nameof(Enabled));
            b2.Bind(nameof(s2.Compress),  ContextCompressCheckBox, nameof(CheckBox.Checked));
            b2.Bind(nameof(s2.Compress),  ContextCompressPanel,    nameof(Enabled));
            b2.Bind(nameof(s2.Extract),   ContextExtractCheckBox,  nameof(CheckBox.Checked));
            b2.Bind(nameof(s2.Extract),   ContextExtractPanel,     nameof(Enabled));
            BindContext(b2, 0);

            // Desktop shortcut
            var s3 = vm.Shortcut;
            var b3 = Behaviors.Hook(new BindingSource(s3, ""));
            b3.Bind(nameof(s3.Compress),        ShortcutCompressCheckBox,  nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.Compress),        ShortcutCompressComboBox,  nameof(Enabled));
            b3.Bind(nameof(s3.CompressOptions), ShortcutCompressComboBox,  nameof(ComboBox.SelectedValue));
            b3.Bind(nameof(s3.Extract),         ShortcutExtractCheckBox,   nameof(CheckBox.Checked));
            b3.Bind(nameof(s3.Settings),        ShortcutSettingsCheckBox,  nameof(CheckBox.Checked));
            ShortcutCompressComboBox.Bind(Resource.Shortcuts);

            // Compression
            var s4 = vm.Compress;
            var b4 = Behaviors.Hook(new BindingSource(s4, ""));
            b4.Bind(nameof(s4.SaveSource),       CompressSaveSourceRadioButton, nameof(RadioButton.Checked));
            b4.Bind(nameof(s4.SaveQuery),        CompressSaveQueryRadioButton,  nameof(RadioButton.Checked));
            b4.Bind(nameof(s4.SaveOthers),       CompressSaveRadioButton,       nameof(RadioButton.Checked));
            b4.Bind(nameof(s4.SaveOthers),       CompressSaveButton,            nameof(Enabled));
            b4.Bind(nameof(s4.SaveOthers),       CompressSaveTextBox,           nameof(Enabled));
            b4.Bind(nameof(s4.SaveDirectory),    CompressSaveTextBox,           nameof(TextBox.Text));
            b4.Bind(nameof(s4.CompressionLevel), CompressLevelComboBox,         nameof(ComboBox.SelectedValue));
            b4.Bind(nameof(s4.Filtering),        CompressFilterCheckBox,        nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.UseUtf8),          CompressUtf8CheckBox,          nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.OverwritePrompt),  CompressOverwriteCheckBox,     nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.OpenDirectory),    CompressOpenCheckBox,          nameof(CheckBox.Checked));
            b4.Bind(nameof(s4.OpenDirectory),    CompressOpenSmartCheckBox,     nameof(Enabled));
            b4.Bind(nameof(s4.SkipDesktop),      CompressOpenSmartCheckBox,     nameof(CheckBox.Checked));
            CompressLevelComboBox.Bind(Resource.CompressionLevels);

            // Extracting
            var s5 = vm.Extract;
            var b5 = Behaviors.Hook(new BindingSource(s5, ""));
            b5.Bind(nameof(s5.SaveSource),          ExtractSaveSourceRadioButton, nameof(RadioButton.Checked));
            b5.Bind(nameof(s5.SaveQuery),           ExtractSaveQueryRadioButton,  nameof(RadioButton.Checked));
            b5.Bind(nameof(s5.SaveOthers),          ExtractSaveRadioButton,       nameof(RadioButton.Checked));
            b5.Bind(nameof(s5.SaveOthers),          ExtractSaveButton,            nameof(Enabled));
            b5.Bind(nameof(s5.SaveOthers),          ExtractSaveTextBox,           nameof(Enabled));
            b5.Bind(nameof(s5.SaveDirectory),       ExtractSaveTextBox,           nameof(TextBox.Text));
            b5.Bind(nameof(s5.Filtering),           ExtractFilterCheckBox,        nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.DeleteSource),        ExtractDeleteCheckBox,        nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.Bursty),              ExtractBurstCheckBox,         nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.CreateDirectory),     ExtractCreateCheckBox,        nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.CreateDirectory),     ExtractCreateSmartCheckBox,   nameof(Enabled));
            b5.Bind(nameof(s5.SkipSingleDirectory), ExtractCreateSmartCheckBox,   nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.OpenDirectory),       ExtractOpenCheckBox,          nameof(CheckBox.Checked));
            b5.Bind(nameof(s5.OpenDirectory),       ExtractOpenSmartCheckBox,     nameof(Enabled));
            b5.Bind(nameof(s5.SkipDesktop),         ExtractOpenSmartCheckBox,     nameof(CheckBox.Checked));
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
        /// <param name="src">Binding source.</param>
        /// <param name="start">Start tab index.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void BindAssociate(BindingSource src, int start) => AssociateMenuPanel.Controls.AddRange(new[]
        {
            // well-known
            src.Bind(nameof(AssociationSettingValue.Zip),      "*.zip",   start++),
            src.Bind(nameof(AssociationSettingValue.Lzh),      "*.lzh",   start++),
            src.Bind(nameof(AssociationSettingValue.Rar),      "*.rar",   start++),
            src.Bind(nameof(AssociationSettingValue.SevenZip), "*.7z",    start++),
            src.Bind(nameof(AssociationSettingValue.Iso),      "*.iso",   start++),
            src.Bind(nameof(AssociationSettingValue.Tar),      "*.tar",   start++),
            src.Bind(nameof(AssociationSettingValue.Gz),       "*.gz",    start++),
            src.Bind(nameof(AssociationSettingValue.Tgz),      "*.tgz",   start++),
            src.Bind(nameof(AssociationSettingValue.Bz2),      "*.bz2",   start++),
            src.Bind(nameof(AssociationSettingValue.Tbz),      "*.tbz",   start++),
            src.Bind(nameof(AssociationSettingValue.Xz),       "*.xz",    start++),
            src.Bind(nameof(AssociationSettingValue.Txz),      "*.txz",   start++),

            // others
            src.Bind(nameof(AssociationSettingValue.Arj),      "*.arj",   start++),
            src.Bind(nameof(AssociationSettingValue.Cab),      "*.cab",   start++),
            src.Bind(nameof(AssociationSettingValue.Chm),      "*.chm",   start++),
            src.Bind(nameof(AssociationSettingValue.Cpio),     "*.cpio",  start++),
            src.Bind(nameof(AssociationSettingValue.Deb),      "*.deb",   start++),
            src.Bind(nameof(AssociationSettingValue.Dmg),      "*.dmg",   start++),
            src.Bind(nameof(AssociationSettingValue.Hfs),      "*.hfs",   start++),
            src.Bind(nameof(AssociationSettingValue.Jar),      "*.jar",   start++),
            src.Bind(nameof(AssociationSettingValue.Nupkg),    "*.nupkg", start++),
            src.Bind(nameof(AssociationSettingValue.Rpm),      "*.rpm",   start++),
            src.Bind(nameof(AssociationSettingValue.Vhd),      "*.vhd",   start++),
            src.Bind(nameof(AssociationSettingValue.Vmdk),     "*.vmdk",  start++),
            src.Bind(nameof(AssociationSettingValue.Wim),      "*.wim",   start++),
            src.Bind(nameof(AssociationSettingValue.Xar),      "*.xar",   start++),
            src.Bind(nameof(AssociationSettingValue.Z),        "*.z",     start++),
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
        /// <param name="src">Binding source.</param>
        /// <param name="start">Start tab index.</param>
        ///
        /* ----------------------------------------------------------------- */
        private void BindContext(BindingSource src, int start)
        {
            ContextCompressPanel.Controls.AddRange(new[]
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
