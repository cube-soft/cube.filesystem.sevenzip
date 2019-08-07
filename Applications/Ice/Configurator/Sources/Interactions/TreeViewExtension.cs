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
using Cube.Mixin.Generics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Configurator
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
        /// <remarks>
        /// 子要素を持つ Node オブジェクトの Arguments は削除します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Register(this TreeNodeCollection src, IEnumerable<ContextMenu> menu)
        {
            foreach (var item in menu)
            {
                var tag = new ContextMenu
                {
                    Name      = item.Name,
                    Arguments = string.Empty,
                    IconIndex = item.IconIndex,
                };

                var node = new TreeNode
                {
                    Text               = item.Name,
                    ToolTipText        = item.Name,
                    ImageIndex         = item.IconIndex,
                    SelectedImageIndex = item.IconIndex,
                    Tag                = tag,
                };

                node.Nodes.Register(item.Children);
                if (node.Nodes.Count == 0) tag.Arguments = item.Arguments;
                src.Add(node);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsLeaf
        ///
        /// <summary>
        /// 子要素を保持できないオブジェクトかどうかを判別します。
        /// </summary>
        ///
        /// <param name="src">TreeNode オブジェクト</param>
        ///
        /// <returns>子要素を保持できるかどうか</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsLeaf(this TreeNode src) =>
            !string.IsNullOrEmpty(src.Tag.TryCast<ContextMenu>()?.Arguments);

        #endregion
    }
}
