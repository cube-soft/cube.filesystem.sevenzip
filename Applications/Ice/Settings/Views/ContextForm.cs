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
using Cube.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextForm
    ///
    /// <summary>
    /// コンテキストメニューのカスタマイズ画面を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ContextForm : Cube.Forms.StandardForm
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ContextForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ContextForm()
        {
            InitializeComponent();

            ShortcutKeys.Add(Keys.F2, () => DestinationTreeView.SelectedNode?.BeginEdit());

            DestinationTreeView.BeforeLabelEdit += (s, e) => e.CancelEdit = (e.Node.Parent == null);

            ApplyButton.Click  += (s, e) => Close();
            ExitButton.Click   += (s, e) => Close();
            RenameButton.Click += (s, e) => DestinationTreeView.SelectedNode?.BeginEdit();
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// ViewModel と関連付けます。
        /// </summary>
        ///
        /// <param name="vm">ViewModel オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Bind(ContextCustomizationViewModel vm)
        {
            SourceTreeView.ImageList = CreateImageList(vm.Images);
            SourceTreeView.Nodes.Clear();
            Add(vm.Source, SourceTreeView.Nodes, vm.Images);

            DestinationTreeView.ImageList = CreateImageList(vm.Images);
            DestinationTreeView.Nodes.Clear();
            DestinationTreeView.Nodes.Add(CreateTopMenu(vm.Current, vm.Images));
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateTopMenu
        ///
        /// <summary>
        /// 現在のメニューのルート項目を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode CreateTopMenu(IEnumerable<ContextMenu> src, IList<Image> images)
        {
            var dest = new TreeNode
            {
                Text               = Properties.Resources.MenuTop,
                ToolTipText        = Properties.Resources.MenuTop,
                ImageIndex         = 0,
                SelectedImageIndex = images.LastIndex(),
            };

            Add(src, dest.Nodes, images);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateImageList
        ///
        /// <summary>
        /// ImageList オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageList CreateImageList(IList<Image> src)
        {
            var dest = new ImageList
            {
                ImageSize  = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit,
            };
            foreach (var image in src) dest.Images.Add(image);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ContextMenu オブジェクトの内容に対応した項目を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Add(IEnumerable<ContextMenu> src, TreeNodeCollection dest, IList<Image> images)
        {
            foreach (var item in src)
            {
                var node = new TreeNode
                {
                    Text               = item.Name,
                    ToolTipText        = item.Name,
                    Tag                = Copy(item),
                    ImageIndex         = item.IconIndex,
                    SelectedImageIndex = (item.IconIndex != 0) ? item.IconIndex : images.LastIndex(),
                };

                Add(item.Children, node.Nodes, images);
                dest.Add(node);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// ContextMenu オブジェクトをコピーします。
        /// </summary>
        ///
        /// <remarks>
        /// 階層構造は TreeNode で管理するため、Child プロパティは空の
        /// 状態にします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private ContextMenu Copy(ContextMenu src) => new ContextMenu
        {
            Name      = src.Name,
            Arguments = src.Arguments,
            IconIndex = src.IconIndex,
        };

        #endregion
    }
}
