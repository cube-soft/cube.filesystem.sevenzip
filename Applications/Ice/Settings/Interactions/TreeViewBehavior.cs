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
using Cube.Forms.Images;
using Cube.Generics;
using System;
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
            Source = src ?? throw new ArgumentException();
            Source.AllowDrop        = true;
            Source.ItemDrag        += WhenItemDrag;
            Source.DragOver        += WhenDragOver;
            Source.DragDrop        += WhenDragDrop;
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
        /// IsRegistered
        ///
        /// <summary>
        /// Register メソッドが実行されたかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsRegistered { get; private set; } = false;

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
        private TreeNode RootNode
        {
            get
            {
                if (!IsRegistered) throw new InvalidOperationException("unregistered");
                return Source.Nodes[0];
            }
        }

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
            IsRegistered = true;
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

            var dest = GetTargetNode();
            Debug.Assert(dest != null);
            dest.Nodes.Add(Copy(src));
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
            var dest = GetTargetNode();
            var src  = new TreeNode
            {
                Text               = Properties.Resources.MenuNewCategory,
                ToolTipText        = Properties.Resources.MenuNewCategory,
                ImageIndex         = 0,
                SelectedImageIndex = 0,
                Tag                = new ContextMenu(),
            };

            Debug.Assert(dest != null);
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
            if (!IsEditable) return;

            var src = Source.SelectedNode;
            Debug.Assert(src != null);
            var dest = src.Parent;
            Debug.Assert(dest != null);

            var index = dest.Nodes.IndexOf(Source.SelectedNode);
            if (index + delta < 0 || index + delta > dest.Nodes.Count - 1) return;

            src.Remove();
            dest.Nodes.Insert(index + delta, src);
            Source.SelectedNode = src;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// src が dest の子要素になるように移動します。
        /// </summary>
        ///
        /// <param name="src">移動元オブジェクト</param>
        /// <param name="dest">移動先オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(TreeNode src, TreeNode dest)
        {
            if (!IsMovable(src, dest)) return;

            src.Remove();
            dest.Nodes.Add(src);
            Source.SelectedNode = src;
            dest.Expand();
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
            Debug.Assert(root != null);
            foreach (TreeNode n in root.Nodes) dest.Add(CreateMenu(n));
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
            var dest = GetContextMenu(src);
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
        /// GetTargetNode
        ///
        /// <summary>
        /// 追加等の操作の対象となる Node オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode GetTargetNode()
        {
            var dest = Source.SelectedNode ?? RootNode;
            Debug.Assert(dest != null);

            return string.IsNullOrEmpty(dest.Tag.TryCast<ContextMenu>()?.Arguments) ?
                   dest :
                   dest.Parent;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTargetNode
        ///
        /// <summary>
        /// 指定された座標に対応する Node オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode GetTargetNode(int x, int y) =>
            Source.GetNodeAt(Source.PointToClient(new Point(x, y)));

        /* ----------------------------------------------------------------- */
        ///
        /// GetContextMenu
        ///
        /// <summary>
        /// ContextMenu オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ContextMenu GetContextMenu(TreeNode src)
        {
            var dest = src.Tag as ContextMenu;
            Debug.Assert(dest != null);
            return dest;
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
            var menu = GetContextMenu(src);
            var dest = new TreeNode
            {
                Text               = src.Text,
                ToolTipText        = src.ToolTipText,
                ImageIndex         = src.ImageIndex,
                SelectedImageIndex = src.SelectedImageIndex,
                Tag                = new ContextMenu
                {
                    Name      = menu.Name,
                    Arguments = menu.Arguments,
                    IconIndex = menu.IconIndex,
                },
            };

            foreach (TreeNode node in src.Nodes) dest.Nodes.Add(Copy(node));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsMovable
        ///
        /// <summary>
        /// src ノードから dest ノードへ移動可能かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsMovable(TreeNode src, TreeNode dest)
        {
            Debug.Assert(src != null);

            if (dest == null            ||
                dest.Equals(src)        ||
                dest.Equals(src.Parent) ||
                dest.IsLeaf()
            ) return false;

            // Check if ancestor
            var node = dest;
            while (node != null && node != src) node = node.Parent;
            return node == null;
        }

        #region Drag&Drop

        /* ----------------------------------------------------------------- */
        ///
        /// WhenItemDrag
        ///
        /// <summary>
        /// Node オブジェクトのドラッグ開始時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenItemDrag(object s, ItemDragEventArgs e)
        {
            var src = e.Item.TryCast<TreeNode>();
            if (src.Text == Properties.Resources.MenuTop) return;
            Source.SelectedNode = src;
            Source.DoDragDrop(src, DragDropEffects.Move);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragOver
        ///
        /// <summary>
        /// ドラッグ状態でのマウスオーバ時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragOver(object s, DragEventArgs e)
        {
            var src  = e.Data.GetData(typeof(TreeNode)).TryCast<TreeNode>();
            var dest = GetTargetNode(e.X, e.Y);
            e.Effect = IsMovable(src, dest) ? DragDropEffects.Move : DragDropEffects.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragDrop
        ///
        /// <summary>
        /// Node オブジェクトがドロップされた時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragDrop(object s, DragEventArgs e) => Move(
            e.Data.GetData(typeof(TreeNode)).TryCast<TreeNode>(),
            GetTargetNode(e.X, e.Y)
        );

        #endregion

        #endregion
    }
}
