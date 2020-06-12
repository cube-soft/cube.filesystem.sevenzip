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
using Cube.FileSystem.SevenZip.Ice.Configurator;
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
    class SettingsMockViewFactory : Configurator.ViewFactory
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
        public Func<CustomizeMenu, bool> CustomizeContext { get; set; } = (_) => false;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ShowCustomizeView
        ///
        /// <summary>
        /// コンテキストメニューのカスタマイズ画面を表示します。
        /// </summary>
        ///
        /// <param name="e">コンテキストメニュー</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowCustomizeView(QueryEventArgs<IEnumerable<ContextMenu>> e)
        {
            var n    = 0;
            var vm   = new CustomContextViewModel(e.Query);
            var view = new CustomizeMenu(new TreeView(), new TreeView());

            view.Updated += (s, ev) => ++n;
            view.Register(vm.Source, vm.Current, vm.Images);

            Assert.That(view.IsRegistered,        Is.True);
            Assert.That(view.IsEditable,          Is.False);
            Assert.That(view.Source.SelectedNode, Is.Not.Null);
            Assert.That(view.Target.SelectedNode, Is.Not.Null);
            Assert.That(n,                        Is.EqualTo(1));

            e.Cancel = !CustomizeContext(view);
            if (!e.Cancel) e.Result = view.Result;
        }

        #endregion
    }
}
