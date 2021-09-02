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
using Cube.Forms.Behaviors;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressWindow
    ///
    /// <summary>
    /// Provides functionality to show the progress of compression or
    /// extraction process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ProgressWindow : Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressWindow
        ///
        /// <summary>
        /// Initializes a new instance of the ProgressWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressWindow()
        {
            InitializeComponent();
            _taskbar = new(this);

            Behaviors.Add(new ClickBehavior(ExitButton, Close));

            // Manual bindings.
            Bind(nameof(Value));
            Bind(nameof(Unit));
            Bind(nameof(Cancelable));
            Bind(nameof(Suspended));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Value
        {
            get => MainProgressBar.Value;
            set
            {
                MainProgressBar.Value = value;
                _taskbar.Value        = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unit
        ///
        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Unit
        {
            get => MainProgressBar.Maximum;
            set
            {
                MainProgressBar.Maximum = value;
                _taskbar.Maximum        = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancelable
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the current process can
        /// be canceled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Cancelable
        {
            get => SuspendButton.Enabled;
            set
            {
                SuspendButton.Enabled = value;
                CountLabel.Visible    = value;
                RemainLabel.Visible   = value;
                MainProgressBar.Style = value ?
                                        ProgressBarStyle.Continuous :
                                        ProgressBarStyle.Marquee;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Suspended
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the current process is
        /// suspended.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Suspended
        {
            get => _taskbar.State == Forms.TaskbarProgressState.Paused;
            set => _taskbar.State  = value ?
                                     Forms.TaskbarProgressState.Paused :
                                     Forms.TaskbarProgressState.Normal;
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
            if (src is not ProgressViewModel vm) return;

            MainBindingSource.DataSource = vm;

            Behaviors.Add(new DialogBehavior(vm));
            Behaviors.Add(new OpenDirectoryBehavior(vm));
            Behaviors.Add(new SaveFileBehavior(vm));
            Behaviors.Add(new OverwriteBehavior(vm));
            Behaviors.Add(new PasswordBehavior(vm));
            Behaviors.Add(new CloseBehavior(this, vm));
            Behaviors.Add(new ShownBehavior(this, vm.Start));
            Behaviors.Add(new ClickBehavior(SuspendButton, vm.SuspendOrResume));
            Behaviors.Add(new ShowDialogBehavior<CompressSettingWindow, CompressSettingViewModel>(vm));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Adds the binding settings with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Bind(string name) =>
            DataBindings.Add(name, MainBindingSource, name, false, DataSourceUpdateMode.OnPropertyChanged);

        #endregion

        #region Fields
        private readonly Forms.TaskbarProgress _taskbar;
        #endregion
    }
}
