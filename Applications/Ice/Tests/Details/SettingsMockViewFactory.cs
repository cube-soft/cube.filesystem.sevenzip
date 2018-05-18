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
using Cube.Images.Icons;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// Icons
        ///
        /// <summary>
        /// ダミー用のアイコン一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Image> Icons { get; } = new List<Image>
        {
            StockIcons.Folder.GetIcon(IconSize.Small).ToBitmap(),
            StockIcons.Application.GetIcon(IconSize.Small).ToBitmap(),
            StockIcons.Application.GetIcon(IconSize.Small).ToBitmap(),
            StockIcons.FolderOpen.GetIcon(IconSize.Small).ToBitmap(),
        };

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
        public Func<TreeViewBehavior, bool> CustomizeContext { get; set; } = (_) => false;

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
            var src  = new TreeViewBehavior(view, true);
            src.Register(e.Query, Icons);

            Assert.That(src.HasRoot,    Is.True);
            Assert.That(src.IsEditable, Is.False);
            Assert.That(src.Source,     Is.EqualTo(view));

            e.Cancel = !CustomizeContext(src);
            if (!e.Cancel) e.Result = src.Result;
        }

        #endregion
    }
}
