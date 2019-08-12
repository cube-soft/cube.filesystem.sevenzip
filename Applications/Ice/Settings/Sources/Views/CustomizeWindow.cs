﻿/* ------------------------------------------------------------------------- */
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

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CustomizeWindow
    ///
    /// <summary>
    /// コンテキストメニューのカスタマイズ画面を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class CustomizeWindow : Cube.Forms.Window
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CustomizeForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CustomizeWindow()
        {
            InitializeComponent();

            _menu = new CustomizeMenu(SourceTreeView, DestinationTreeView);
            _menu.Updated += (s, e) => UpdateMenu();

            ApplyButton.Click       += (s, e) => Close();
            ExitButton.Click        += (s, e) => Close();
            RenameButton.Click      += (s, e) => _menu.RenameMenu.Execute();
            AddButton.Click         += (s, e) => _menu.AddMenu.Execute();
            NewCategoryButton.Click += (s, e) => _menu.NewCategoryMenu.Execute();
            RemoveButton.Click      += (s, e) => _menu.RemoveMenu.Execute();
            UpButton.Click          += (s, e) => _menu.UpMenu.Execute();
            DownButton.Click        += (s, e) => _menu.DownMenu.Execute();

            ShortcutKeys.Add(Keys.F2, () => _menu.RenameMenu.Execute());
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
        public IEnumerable<ContextMenu> Result => _menu.Result;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnBind
        ///
        /// <summary>
        /// Binds the windows to the specified object.
        /// </summary>
        ///
        /// <param name="src">Binding object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnBind(IPresentable src)
        {
            base.OnBind(src);
            if (!(src is CustomMenuViewModel vm)) return;
            _menu.Register(vm.Source, vm.Current, vm.Images);
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
            RenameButton.Enabled = _menu.Editable;
            RemoveButton.Enabled = _menu.Editable;
            UpButton.Enabled     = _menu.Editable;
            DownButton.Enabled   = _menu.Editable;
        }

        #endregion

        #region Fields
        private readonly CustomizeMenu _menu;
        #endregion
    }
}
