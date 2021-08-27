﻿/* ------------------------------------------------------------------------- */
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
    public partial class ProgressWindow : Cube.Forms.Window
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
        }

        #endregion

        #region Properties

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
                _taskbar.Maximum = value;
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
            get => ExitButton.Enabled;
            set
            {
                ExitButton.Enabled    = value;
                SuspendButton.Enabled = value;
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

        #region Fields
        private readonly Cube.Forms.TaskbarProgress _taskbar;
        #endregion
    }
}
