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
namespace Cube.FileSystem.SevenZip.Ice;

using System.Windows.Forms;
using Cube.Forms.Behaviors;
using Cube.Forms.Binding;

/* ------------------------------------------------------------------------- */
///
/// CompressSettingWindow
///
/// <summary>
/// Represents the window for the runtime compression settings.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public partial class CompressSettingWindow : Forms.Window
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// CompressSettingWindow
    ///
    /// <summary>
    /// Initializes a new instance of the CompressSettingWindow class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CompressSettingWindow() => InitializeComponent();

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// OnBind
    ///
    /// <summary>
    /// Binds the specified object.
    /// </summary>
    ///
    /// <param name="src">Bindable object.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnBind(IBindable src)
    {
        if (src is not CompressSettingViewModel vm) return;

        BindCore(vm);

        Behaviors.Add(new DialogBehavior(vm));
        Behaviors.Add(new SaveFileBehavior(vm));
        Behaviors.Add(new CloseBehavior(this, vm));
        Behaviors.Add(new ClickEventBehavior(DestinationButton, vm.Browse));
        Behaviors.Add(new ClickEventBehavior(ExecuteButton, vm.Execute));
        Behaviors.Add(new ClickEventBehavior(ExitButton, Close));
        Behaviors.Add(new PathLintBehavior(DestinationTextBox, PathToolTip));
        Behaviors.Add(new PasswordLintBehavior(PasswordTextBox, ConfirmTextBox, ShowPasswordCheckBox));
        Behaviors.Add(new CompressMethodBehavior(CompressionMethodComboBox, FormatComboBox));
    }

    #endregion

    #region  Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// BindCore
    ///
    /// <summary>
    /// Invokes the binding settings.
    /// </summary>
    ///
    /// <param name="vm">VM object to bind.</param>
    ///
    /* --------------------------------------------------------------------- */
    private void BindCore(CompressSettingViewModel vm)
    {
        var src = Behaviors.Hook(new BindingSource(vm, ""));

        src.Bind(nameof(vm.Executable),              ExecuteButton,             nameof(ExecuteButton.Enabled));
        src.Bind(nameof(vm.Destination),             DestinationTextBox,        nameof(DestinationTextBox.Text));
        src.Bind(nameof(vm.Format),                  FormatComboBox,            nameof(FormatComboBox.SelectedValue));
        src.Bind(nameof(vm.CompressionLevel),        CompressionLevelComboBox,  nameof(CompressionLevelComboBox.SelectedValue));
        src.Bind(nameof(vm.CompressionMethod),       CompressionMethodComboBox, nameof(CompressionMethodComboBox.SelectedValue));
        src.Bind(nameof(vm.ThreadCount),             ThreadNumeric,             nameof(ThreadNumeric.Value));
        src.Bind(nameof(vm.MaximumThreadCount),      ThreadNumeric,             nameof(ThreadNumeric.Maximum));
        src.Bind(nameof(vm.EncryptionSupported),     EncryptionGroupBox,        nameof(EncryptionGroupBox.Enabled));
        src.Bind(nameof(vm.EncryptionEnabled),       EncryptionCheckBox,        nameof(EncryptionCheckBox.Checked));
        src.Bind(nameof(vm.EncryptionEnabled),       EncryptionPanel,           nameof(EncryptionPanel.Enabled));
        src.Bind(nameof(vm.EncryptionMethod),        EncryptionMethodComboBox,  nameof(EncryptionMethodComboBox.SelectedValue));
        src.Bind(nameof(vm.EncryptionMethodEnabled), EncryptionMethodComboBox,  nameof(EncryptionMethodComboBox.Enabled));
        src.Bind(nameof(vm.Password),                PasswordTextBox,           nameof(PasswordTextBox.Text));
        src.Bind(nameof(vm.PasswordConfirmation),    ConfirmTextBox,            nameof(ConfirmTextBox.Text));
        src.Bind(nameof(vm.PasswordVisible),         ShowPasswordCheckBox,      nameof(ShowPasswordCheckBox.Checked));

        FormatComboBox.Bind(Resource.Formats);
        EncryptionMethodComboBox.Bind(Resource.EncryptionMethods);
        CompressionLevelComboBox.Bind(Resource.CompressionLevels);
        CompressionMethodComboBox.Bind(Resource.GetCompressionMethods(vm.Format));
        CompressionMethodComboBox.SelectedIndex = 0;
        PathToolTip.ToolTipTitle = Properties.Resources.MessageInvalidChars;
    }

    #endregion
}
