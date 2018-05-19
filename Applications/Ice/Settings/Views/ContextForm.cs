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
using System.Collections.Generic;
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

            _tv = new TreeViewBehavior(DestinationTreeView);

            DestinationTreeView.AfterSelect += (s, e) => UpdateMenu();

            ApplyButton.Click       += (s, e) => Close();
            ExitButton.Click        += (s, e) => Close();
            RenameButton.Click      += (s, e) => _tv.Rename();
            AddButton.Click         += (s, e) => _tv.Add(SourceTreeView.SelectedNode);
            NewCategoryButton.Click += (s, e) => _tv.Add();
            RemoveButton.Click      += (s, e) => _tv.Remove();
            UpButton.Click += (s, e) => _tv.Move(-1);
            DownButton.Click += (s, e) => _tv.Move(1);

            ShortcutKeys.Add(Keys.F2, () => _tv.Rename());
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 操作結果を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ContextMenu> Result => _tv.Result;

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
        public void Bind(CustomContextViewModel vm)
        {
            SourceTreeView.ImageList = vm.Images.ToImageList();
            SourceTreeView.Nodes.Register(vm.Source);
            _tv.Register(vm.Current, vm.Images);
            UpdateMenu();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateMenu
        ///
        /// <summary>
        /// ボタンおよびメニュー項目の状態を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateMenu()
        {
            RenameButton.Enabled = _tv.IsEditable;
            RemoveButton.Enabled = _tv.IsEditable;
            UpButton.Enabled     = _tv.IsEditable;
            DownButton.Enabled   = _tv.IsEditable;
        }

        #endregion

        #region Fields
        private readonly TreeViewBehavior _tv;
        #endregion
    }
}
