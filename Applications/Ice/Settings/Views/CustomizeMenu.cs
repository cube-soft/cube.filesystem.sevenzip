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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CustomizeMenuItem
    ///
    /// <summary>
    /// カスタマイズ画面のコンテキストメニュー項目を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CustomizeMenuItem : ToolStripMenuItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CustomizeMenuItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="text">表示テキスト</param>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem(string text) : base(text) { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Click イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Execute() => OnClick(EventArgs.Empty);

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CustomizeMenu
    ///
    /// <summary>
    /// コンテキストメニューのカスタマイズ画面を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CustomizeMenu : ContextMenuStrip
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CustomizeMenu
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">追加可能なメニュー一覧</param>
        /// <param name="dest">現在のメニュー一覧</param>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenu(TreeView src, TreeView dest)
        {
            _core  = new TreeViewBehavior(dest);
            Source = src;
            Target = dest;

            InitializeShortcutKeys();
            InitializeEvents();
            InitializeMenu();

            Target.ContextMenuStrip = this;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 追加可能なメニュー一覧を表す TreeView オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TreeView Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Target
        ///
        /// <summary>
        /// 現在のメニュー一覧を表す TreeView オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TreeView Target { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsEditable
        ///
        /// <summary>
        /// 選択中の Node オブジェクトが編集可能かどうかを示す値を取得
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsEditable => _core.IsEditable;

        /* ----------------------------------------------------------------- */
        ///
        /// IsRegistered
        ///
        /// <summary>
        /// データが登録されたかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsRegistered => _core.IsRegistered;

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 編集結果を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ContextMenu> Result => _core.Result;

        /* ----------------------------------------------------------------- */
        ///
        /// AddMenu
        ///
        /// <summary>
        /// 追加メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem AddMenu { get; } =
            new CustomizeMenuItem(Properties.Resources.MenuAdd);

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveMenu
        ///
        /// <summary>
        /// 削除メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem RemoveMenu { get; } =
            new CustomizeMenuItem(Properties.Resources.MenuRemove);

        /* ----------------------------------------------------------------- */
        ///
        /// RenameMenu
        ///
        /// <summary>
        /// 名前を変更メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem RenameMenu { get; } =
            new CustomizeMenuItem(Properties.Resources.MenuRename);

        /* ----------------------------------------------------------------- */
        ///
        /// UpMenu
        ///
        /// <summary>
        /// 上へ移動メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem UpMenu { get; } =
            new CustomizeMenuItem(Properties.Resources.MenuUp);

        /* ----------------------------------------------------------------- */
        ///
        /// DownMenu
        ///
        /// <summary>
        /// 下へ移動メニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem DownMenu { get; } =
            new CustomizeMenuItem(Properties.Resources.MenuDown);

        /* ----------------------------------------------------------------- */
        ///
        /// NewCategoryMenu
        ///
        /// <summary>
        /// 新しいカテゴリーメニューを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeMenuItem NewCategoryMenu { get; } =
            new CustomizeMenuItem(Properties.Resources.MenuNewCategory);

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Updated
        ///
        /// <summary>
        /// 状態が更新された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Updated;

        /* ----------------------------------------------------------------- */
        ///
        /// OnUpdated
        ///
        /// <summary>
        /// Updated イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnUpdated(EventArgs e) => Updated?.Invoke(this, e);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// データを登録します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Register(IEnumerable<ContextMenu> src,
            IEnumerable<ContextMenu> dest, IEnumerable<Image> images)
        {
            Source.ImageList = images.ToImageList();
            Source.Nodes.Register(src);
            _core.Register(dest, images);
            if (Source.Nodes.Count > 0) Source.SelectedNode = Source.Nodes[0];
            Target.SelectedNode = Target.Nodes[0];
            RaiseUpdated();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeEvents
        ///
        /// <summary>
        /// 各種イベントを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeEvents()
        {
            AddMenu.Click         += (s, e) => _core.Add(Source.SelectedNode);
            RemoveMenu.Click      += (s, e) => _core.Remove();
            RenameMenu.Click      += (s, e) => _core.Rename();
            UpMenu.Click          += (s, e) => _core.Move(-1);
            DownMenu.Click        += (s, e) => _core.Move(1);
            NewCategoryMenu.Click += (s, e) => _core.Add();
            Target.NodeMouseClick += (s, e) => Target.SelectedNode = e.Node;
            Target.AfterSelect    += (s, e) => RaiseUpdated();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeShortcutKeys
        ///
        /// <summary>
        /// ショートカットキーを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeShortcutKeys()
        {
            AddMenu.ShortcutKeys         = Keys.Control | Keys.V;
            RemoveMenu.ShortcutKeys      = Keys.Control | Keys.D;
            RenameMenu.ShortcutKeys      = Keys.Control | Keys.R;
            UpMenu.ShortcutKeys          = Keys.Control | Keys.Up;
            DownMenu.ShortcutKeys        = Keys.Control | Keys.Down;
            NewCategoryMenu.ShortcutKeys = Keys.Control | Keys.N;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeMenu
        ///
        /// <summary>
        /// メニューを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeMenu()
        {
            Items.AddRange(new ToolStripItem[]
            {
                AddMenu,
                NewCategoryMenu,
                new ToolStripSeparator(),
                UpMenu,
                DownMenu,
                new ToolStripSeparator(),
                RemoveMenu,
                RenameMenu
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseUpdated
        ///
        /// <summary>
        /// メニューの状態を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseUpdated()
        {
            RenameMenu.Enabled = IsEditable;
            RemoveMenu.Enabled = IsEditable;
            UpMenu.Enabled     = IsEditable;
            DownMenu.Enabled   = IsEditable;

            OnUpdated(EventArgs.Empty);
        }

        #endregion

        #region Fields
        private readonly TreeViewBehavior _core;
        #endregion
    }
}
