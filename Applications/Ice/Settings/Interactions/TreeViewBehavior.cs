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
using Cube.Forms;
using Cube.Generics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// TreeViewBehavior
    ///
    /// <summary>
    /// コンテキストメニューを編集するための TreeView オブジェクトの
    /// 動作を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class TreeViewBehavior
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// TreeViewAction
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">対象となる TreeView オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public TreeViewBehavior(TreeView src)
        {
            Debug.Assert(src != null);
            Source = src;
            Source.BeforeLabelEdit += (s, e) => e.CancelEdit = !IsEditable;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 動作の適用される TreeView オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TreeView Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 操作結果を表す ContextMenu 一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ContextMenu> Result => CreateResult();

        /* ----------------------------------------------------------------- */
        ///
        /// IsEditable
        ///
        /// <summary>
        /// 現在選択中の Node オブジェクトが編集可能かどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsEditable =>
            Source.SelectedNode != null &&
            Source.SelectedNode != RootNode;

        /* ----------------------------------------------------------------- */
        ///
        /// RootNode
        ///
        /// <summary>
        /// ルートとなる Node オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode RootNode => Source.Nodes.Count > 0 ? Source.Nodes[0] : null;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// 対象となる TreeView オブジェクトに、コンテキストメニューに対応
        /// する TreeNode 一覧を追加します。
        /// </summary>
        ///
        /// <param name="src">コンテキストメニュー一覧</param>
        /// <param name="images">表示アイコン一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Register(IEnumerable<ContextMenu> src, IEnumerable<Image> images)
        {
            Source.ImageList = images.ToImageList();
            Source.Nodes.Clear();
            Source.Nodes.Add(CreateRootNode(src));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// TreeNode オブジェクトの内容をコピーして追加します。
        /// </summary>
        ///
        /// <param name="src">追加される Node オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(TreeNode src)
        {
            if (src == null) return;
            var dest = TargetNode();
            if (dest == null) return;

            var cp = Copy(src);
            dest.Nodes.Add(cp);
            dest.Expand();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// 新しい TreeNode オブジェクトを追加します。追加された TreeNode
        /// オブジェクトは名前を変更する状態となります。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Add()
        {
            var dest = TargetNode();
            if (dest == null) return;

            var src = new TreeNode
            {
                Text               = Properties.Resources.MenuNewCategory,
                ToolTipText        = Properties.Resources.MenuNewCategory,
                ImageIndex         = 0,
                SelectedImageIndex = 0,
                Tag                = new ContextMenu(),
            };

            dest.Nodes.Add(src);
            dest.Expand();
            Source.SelectedNode = src;
            Rename();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// 現在選択中の TreeNode オブジェクトを削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove()
        {
            if (IsEditable) Source.SelectedNode.Remove();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rename
        ///
        /// <summary>
        /// 名前を変更処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Rename() => Source.SelectedNode?.BeginEdit();

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// 現在選択中の TreeNode オブジェクトを指定した量だけ移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta)
        {
            var src = Source.SelectedNode;
            if (src == null) return;

            var nodes = src.Parent?.Nodes;
            if (nodes == null) return;

            var index = nodes.IndexOf(Source.SelectedNode);
            if (index + delta < 0 || index + delta > nodes.Count - 1) return;

            nodes.Remove(src);
            nodes.Insert(index + delta, src);
            Source.SelectedNode = src;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateResult
        ///
        /// <summary>
        /// 操作結果を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<ContextMenu> CreateResult()
        {
            var dest = new List<ContextMenu>();
            var root = RootNode;
            if (root != null)
            {
                foreach (TreeNode n in root.Nodes) dest.Add(CreateMenu(n));
            }
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMenu
        ///
        /// <summary>
        /// TreeMode の内容を基にして ContextMenu オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ContextMenu CreateMenu(TreeNode src)
        {
            var dest = src.Tag as ContextMenu ?? new ContextMenu();
            dest.Name      = src.Text;
            dest.IconIndex = src.ImageIndex;

            foreach (TreeNode n in src.Nodes) dest.Children.Add(CreateMenu(n));

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateRootNode
        ///
        /// <summary>
        /// ルート項目を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode CreateRootNode(IEnumerable<ContextMenu> src)
        {
            var dest = new TreeNode
            {
                Text               = Properties.Resources.MenuTop,
                ToolTipText        = Properties.Resources.MenuTop,
                ImageIndex         = 0,
                SelectedImageIndex = 0,
            };
            dest.Nodes.Register(src);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TargetNode
        ///
        /// <summary>
        /// 追加等の操作の対象となる Node オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode TargetNode()
        {
            var dest = Source.SelectedNode ?? RootNode;
            if (dest == null) return null;

            var command = !string.IsNullOrEmpty(dest.Tag.TryCast<ContextMenu>()?.Arguments);
            return dest.Nodes.Count <= 0 && command ?
                   dest.Parent :
                   dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// TreeNode オブジェクトをコピーします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode Copy(TreeNode src)
        {
            var menu = src.Tag as ContextMenu;
            var dest = new TreeNode
            {
                Text               = src.Text,
                ToolTipText        = src.ToolTipText,
                ImageIndex         = src.ImageIndex,
                SelectedImageIndex = src.SelectedImageIndex,
                Tag                = new ContextMenu
                {
                    Name      = menu?.Name,
                    Arguments = menu?.Arguments,
                    IconIndex = menu?.IconIndex ?? 0,
                },
            };

            foreach (TreeNode node in src.Nodes) dest.Nodes.Add(Copy(node));
            return dest;
        }

        #endregion
    }
}
