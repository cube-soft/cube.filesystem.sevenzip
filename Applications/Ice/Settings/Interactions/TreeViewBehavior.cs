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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        /// <param name="rootCreation">
        /// ルートとなる Node オブジェクトを挿入するかどうか
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public TreeViewBehavior(TreeView src, bool rootCreation)
        {
            Debug.Assert(src != null);

            Source  = src;
            HasRoot = rootCreation;

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
        /// HasRoot
        ///
        /// <summary>
        /// ルートとなる Node オブジェクトを保持するかどうかを示す
        /// 値を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// true の場合、Source.Nodes には 1 つの TreeNode オブジェクト
        /// のみが登録され、この Node オブジェクトは編集不可能な特別な
        /// オブジェクトと認識されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool HasRoot { get; }

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
            (!HasRoot || Source.SelectedNode != RootNode());

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
            _lastIndex = Math.Max(images.Count() - 1, 0);
            Source.ImageList = images.ToImageList();

            Source.Nodes.Clear();
            if (HasRoot) Source.Nodes.Add(CreateRootNode(src));
            else RegisterCore(src, Source.Nodes);
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
                SelectedImageIndex = _lastIndex,
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
            var root = RootNode();
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
                SelectedImageIndex = _lastIndex,
            };
            RegisterCore(src, dest.Nodes);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RootNode
        ///
        /// <summary>
        /// ルートとなる Node オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode RootNode() =>
            Source.Nodes.Count > 0 ? Source.Nodes[0] : null;

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
            var dest = Source.SelectedNode ?? RootNode();
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
        /// ContextMenu オブジェクトをコピーします。
        /// </summary>
        ///
        /// <remarks>
        /// 階層構造は TreeNode で管理するため、Children プロパティは
        /// 空の状態にします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private ContextMenu Copy(ContextMenu src) => new ContextMenu
        {
            Name      = src?.Name,
            Arguments = src?.Arguments,
            IconIndex = src?.IconIndex ?? -1,
        };

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
            var dest = new TreeNode
            {
                Text               = src.Text,
                ToolTipText        = src.ToolTipText,
                ImageIndex         = src.ImageIndex,
                SelectedImageIndex = src.SelectedImageIndex,
                Tag                = Copy(src.Tag as ContextMenu),
            };

            foreach (TreeNode node in src.Nodes) dest.Nodes.Add(Copy(node));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// 対象となる TreeView オブジェクトに、コンテキストメニューに対応
        /// する TreeNode 一覧を追加します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RegisterCore(IEnumerable<ContextMenu> src, TreeNodeCollection dest)
        {
            foreach (var item in src)
            {
                var node = new TreeNode
                {
                    Text               = item.Name,
                    ToolTipText        = item.Name,
                    ImageIndex         = item.IconIndex,
                    SelectedImageIndex = item.IconIndex != 0 ? item.IconIndex : _lastIndex,
                    Tag                = Copy(item),
                };

                RegisterCore(item.Children, node.Nodes);
                dest.Add(node);
            }
        }

        #endregion

        #region Fields
        private int _lastIndex;
        #endregion
    }
}
