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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateIconWindow
    ///
    /// <summary>
    /// Represents the window to select the icon for file association.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class AssociateIconWindow : Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateIconWindow
        ///
        /// <summary>
        /// Initiaizes a new instance of the AssociateIconWindow class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateIconWindow()
        {
            InitializeComponent();

            ExecButton.Click += (s, e) => Close();
            ExitButton.Click += (s, e) => Close();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedIndex
        ///
        /// <summary>
        /// Gets the selected index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int SelectedIndex => IconListView.SelectedIndices.Cast<int>().FirstOrDefault();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// Sets the selection source and the selected index.
        /// </summary>
        ///
        /// <param name="src">Selection source.</param>
        /// <param name="index">Initial value of the selected index.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(IEnumerable<Image> src, int index)
        {
            if (!src.Any()) return;

            IconListView.LargeImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize  = new(src.First().Width, src.First().Height),
            };

            foreach (var e in src)
            {
                IconListView.LargeImageList.Images.Add(e);
                var i = IconListView.LargeImageList.Images.Count - 1;
                _ = IconListView.Items.Add(new ListViewItem
                {
                    Text       = string.Empty,
                    ImageIndex = i,
                    Selected   = i == index,
                });
            }
        }

        #endregion
    }
}
