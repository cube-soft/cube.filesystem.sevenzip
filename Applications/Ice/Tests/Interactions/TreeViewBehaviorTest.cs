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
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// TreeViewBehaviorTest
    ///
    /// <summary>
    /// TreeViewBehavior のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [RequiresThread(ApartmentState.STA)]
    class TreeViewBehaviorTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// TreeView オブジェクトにコンテキストメニュー一覧を登録する
        /// テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Register()
        {
            var view = new TreeView();
            var src  = new TreeViewBehavior(view, true);
            Assert.That(src.HasRoot,    Is.True);
            Assert.That(src.IsEditable, Is.False);
            Assert.That(src.Source,     Is.EqualTo(view));

            src.Register(PresetMenu.DefaultContext.ToContextMenuGroup(), Images);
            Assert.That(view.Nodes.Count, Is.EqualTo(1));

            var dest = view.Nodes[0].Nodes;
            Assert.That(dest.Count,            Is.EqualTo(2));
            Assert.That(dest[0].Nodes.Count,   Is.EqualTo(7));
            Assert.That(dest[0].Nodes[0].Text, Is.EqualTo("Zip"));
            Assert.That(dest[0].Nodes[1].Text, Is.EqualTo("Zip (パスワード)"));
            Assert.That(dest[0].Nodes[2].Text, Is.EqualTo("7-Zip"));
            Assert.That(dest[0].Nodes[3].Text, Is.EqualTo("BZip2"));
            Assert.That(dest[0].Nodes[4].Text, Is.EqualTo("GZip"));
            Assert.That(dest[0].Nodes[5].Text, Is.EqualTo("自己解凍形式"));
            Assert.That(dest[0].Nodes[6].Text, Is.EqualTo("詳細設定"));
            Assert.That(dest[1].Nodes.Count,   Is.EqualTo(4));
            Assert.That(dest[1].Nodes[0].Text, Is.EqualTo("ここに解凍"));
            Assert.That(dest[1].Nodes[1].Text, Is.EqualTo("デスクトップに解凍"));
            Assert.That(dest[1].Nodes[2].Text, Is.EqualTo("マイドキュメントに解凍"));
            Assert.That(dest[1].Nodes[3].Text, Is.EqualTo("場所を指定して解凍"));
        }

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Images
        ///
        /// <summary>
        /// ダミー用のイメージ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Image> Images => new List<Image>
        {
            StockIcons.Folder.GetIcon(IconSize.Small).ToBitmap(),
            StockIcons.Application.GetIcon(IconSize.Small).ToBitmap(),
            StockIcons.Application.GetIcon(IconSize.Small).ToBitmap(),
            StockIcons.FolderOpen.GetIcon(IconSize.Small).ToBitmap(),
        };

        #endregion
    }
}
