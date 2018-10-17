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
    /// <remarks>
    /// TreeViewBehavior に関する多くの通常テストは ContextViewModelTest
    /// クラスで各種 ViewModel を介して実行しています。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class TreeViewBehaviorTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Throws
        ///
        /// <summary>
        /// 無効な TreeView オブジェクトを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Create_Throws() => Assert.That(
            () => new TreeViewBehavior(default(TreeView)),
            Throws.TypeOf<ArgumentException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Add_Unregistered
        ///
        /// <summary>
        /// Register 前に各種操作を実行した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Add_Unregistered() => Assert.That(
            () => new TreeViewBehavior(new TreeView()).Add(),
            Throws.TypeOf<InvalidOperationException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// DragDrop
        ///
        /// <summary>
        /// ドラッグドロップによる移動操作のテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// 実際には、DragDrop イベントで最終的に実行される Move メソッドの
        /// 動作を確認しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDrop()
        {
            var tv   = Create();
            var root = tv.Source.Nodes[0];
            var src  = root.Nodes[0].Nodes[0];
            var dest = root.Nodes[1];

            Assert.That(src.Text,  Is.EqualTo("Zip"));
            Assert.That(dest.Text, Is.EqualTo("解凍"));

            tv.Move(src, dest);
            Assert.That(root.Nodes[0].Nodes.Count,   Is.EqualTo(6));
            Assert.That(root.Nodes[0].Nodes[0].Text, Is.EqualTo("Zip (パスワード)"));
            Assert.That(root.Nodes[1].Nodes.Count,   Is.EqualTo(5));
            Assert.That(root.Nodes[1].Nodes[4].Text, Is.EqualTo("Zip"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DragDrop_Null
        ///
        /// <summary>
        /// ドロップ地点に TreeNode オブジェクトがない時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDrop_Null()
        {
            var tv   = Create();
            var root = tv.Source.Nodes[0];
            tv.Move(root.Nodes[0].Nodes[0], null);

            Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
            Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DragDrop_Same
        ///
        /// <summary>
        /// ドラッグ項目とドロップ項目が同じ時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDrop_Same()
        {
            var tv = Create();
            var root = tv.Source.Nodes[0];
            var src  = root.Nodes[0];
            tv.Move(src, src);

            Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
            Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DragDrop_Root
        ///
        /// <summary>
        /// ドラッグ項目がトップメニューである時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDrop_Root()
        {
            var tv   = Create();
            var root = tv.Source.Nodes[0];
            tv.Move(root, root.Nodes[0].Nodes[0]);

            Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
            Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DragDrop_Parent
        ///
        /// <summary>
        /// ドロップ地点が自分の親要素である時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDrop_Parent()
        {
            var tv   = Create();
            var root = tv.Source.Nodes[0];
            var src  = root.Nodes[0].Nodes[6];
            tv.Move(src, src.Parent);

            Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
            Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DragDrop_Parent
        ///
        /// <summary>
        /// ドロップ地点が自分の祖父母要素である時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DragDrop_Grandparent()
        {
            var tv   = Create();
            var root = tv.Source.Nodes[0];
            var src  = root.Nodes[0].Nodes[3];
            tv.Move(src, src.Parent.Parent);

            Assert.That(root.Nodes.Count,          Is.EqualTo(3));
            Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(6));
            Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
            Assert.That(root.Nodes[2].Text,        Is.EqualTo("BZip2"));
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// TreeViewBehavior オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeViewBehavior Create()
        {
            var m    = PresetMenu.DefaultContext.ToContextMenuGroup();
            var vm   = new CustomContextViewModel(m);
            var dest = new TreeViewBehavior(new TreeView());

            dest.Register(vm.Current, vm.Images);
            return dest;
        }

        #endregion
    }
}
