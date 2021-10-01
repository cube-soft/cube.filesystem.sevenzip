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
using Cube.Forms.Behaviors;
using Cube.Mixin.Forms.Controls;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressSettingWindow
    ///
    /// <summary>
    /// Represents the window for the runtime compression settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class CompressSettingWindow : Forms.WindowBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressSettingWindow
        ///
        /// <summary>
        /// Initializes a new instance of the CompressSettingWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressSettingWindow()
        {
            InitializeComponent();

            CompressionLevelComboBox.Bind(Resource.CompressionLevels);
            EncryptionMethodComboBox.Bind(Resource.EncryptionMethods);
            FormatComboBox.Bind(Resource.Formats);
            FormatComboBox.SelectedValueChanged += (s, e) => Update((Format)FormatComboBox.SelectedValue);

            Behaviors.Add(new PasswordLintBehavior(PasswordTextBox, ConfirmTextBox, ShowPasswordCheckBox));
            Behaviors.Add(new PathLintBehavior(DestinationTextBox, PathToolTip));
            Behaviors.Add(new ClickBehavior(ExitButton, Close));
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnBind
        ///
        /// <summary>
        /// Binds the specified object.
        /// </summary>
        ///
        /// <param name="src">Bindable object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IBindable src)
        {
            if (src is not CompressSettingViewModel vm) return;

            Update(vm.Format);

            MainBindingSource.DataSource = vm;

            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new ClickBehavior(DestinationButton, vm.Select));
            Behaviors.Add(new ClickBehavior(ExecuteButton, vm.Execute));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates the ComboBox bindings with the specified format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update(Format src)
        {
            CompressionMethodComboBox.Bind(Resource.GetCompressionMethods(src));
            CompressionMethodComboBox.SelectedIndex = 0;
        }

        #endregion
    }
}
