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
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// TreeViewExtension
    ///
    /// <summary>
    /// TreeView に関連する拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class TreeViewExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// TreeNodeCollection にメニューを登録します。
        /// </summary>
        ///
        /// <param name="src">登録先オブジェクト</param>
        /// <param name="menu">登録メニュー</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Register(this TreeNodeCollection src, IEnumerable<ContextMenu> menu)
        {
            foreach (var item in menu)
            {
                var node = new TreeNode
                {
                    Text               = item.Name,
                    ToolTipText        = item.Name,
                    ImageIndex         = item.IconIndex,
                    SelectedImageIndex = item.IconIndex,
                    Tag                = new ContextMenu
                    {
                        Name      = item.Name,
                        Arguments = item.Arguments,
                        IconIndex = item.IconIndex,
                    },
                };

                node.Nodes.Register(item.Children);
                src.Add(node);
            }
        }

        #endregion
    }
}
