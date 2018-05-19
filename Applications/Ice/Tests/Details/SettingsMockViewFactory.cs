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
using Cube.FileSystem.SevenZip.Ice.App.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressMockViewFactory
    ///
    /// <summary>
    /// 各種ダミー View の生成および設定用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class SettingsMockViewFactory : ViewFactory
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsMockViewFactory
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsMockViewFactory()
        {
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(
                    new SynchronizationContext()
                );
            }
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CustomizeContext
        ///
        /// <summary>
        /// ShowContextView で実行する内容を示す関数オブジェクトを
        /// 取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// false を返した場合、Cancel プロパティに true が設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public Func<CustomContextViewModel, TreeViewBehavior, bool> CustomizeContext { get; set; } =
            (_, __) => false;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ShowContextView
        ///
        /// <summary>
        /// コンテキストメニューのカスタマイズ画面を表示します。
        /// </summary>
        ///
        /// <param name="e">コンテキストメニュー</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowContextView(QueryEventArgs<IEnumerable<ContextMenu>> e)
        {
            var view = new TreeView();
            var vm   = new CustomContextViewModel(e.Query);
            var b    = new TreeViewBehavior(view, true);
            b.Register(vm.Current, vm.Images);

            Assert.That(b.HasRoot,    Is.True);
            Assert.That(b.IsEditable, Is.False);
            Assert.That(b.Source,     Is.EqualTo(view));

            e.Cancel = !CustomizeContext(vm, b);
            if (!e.Cancel) e.Result = b.Result;
        }

        #endregion
    }
}
