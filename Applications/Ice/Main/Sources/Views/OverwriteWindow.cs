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
using System.ComponentModel;
using System.Text;
using Cube.Forms;
using Cube.Forms.Behaviors;
using Cube.Icons;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteWindow
    ///
    /// <summary>
    /// Represents a dialog to confirm the overwrite method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class OverwriteWindow : Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteWindow
        ///
        /// <summary>
        /// Initializes a new instance of the OverwriteWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public OverwriteWindow()
        {
            InitializeComponent();

            IconPictureBox.Image = StockIcon.Warning.GetImage(IconSize.Large);

            Behaviors.Add(new ClickEventBehavior(YesButton, () => Close(OverwriteMethod.Yes)));
            Behaviors.Add(new ClickEventBehavior(NoButton, () => Close(OverwriteMethod.No)));
            Behaviors.Add(new ClickEventBehavior(ExitButton, () => Close(OverwriteMethod.Cancel)));
            Behaviors.Add(new ClickEventBehavior(AlwaysYesButton, () => Close(OverwriteMethod.AlwaysYes)));
            Behaviors.Add(new ClickEventBehavior(AlwaysNoButton, () => Close(OverwriteMethod.AlwaysNo)));
            Behaviors.Add(new ClickEventBehavior(AlwaysRenameButton, () => Close(OverwriteMethod.AlwaysRename)));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the file information to overwrite.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Entity Source { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets the file information to be overwritten.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Entity Destination { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the overwrite method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OverwriteMethod Value { get; private set; } = OverwriteMethod.Cancel;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// Occurs when the dialog is displayed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            DescriptionLabel.Text = new StringBuilder()
                .AppendLine(Properties.Resources.MessageOverwrite)
                .AppendLine()
                .AppendLine(Properties.Resources.MessageCurrent)
                .AppendLine(Destination)
                .AppendLine()
                .AppendLine(Properties.Resources.MessageNewFile)
                .AppendLine(Source)
                .ToString();

            base.OnShown(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        ///
        /// <summary>
        /// Sets the overwrite method and close the dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Close(OverwriteMethod value)
        {
            Value = value;
            Close();
        }

        #endregion
    }
}
