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
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextViewModelTest
    ///
    /// <summary>
    /// コンテキストメニューの ViewModel をテストするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ContextViewModelTest : SettingsMockViewFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Customize
        ///
        /// <summary>
        /// コンテキストメニューのカスタマイズのテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize()
        {
            Mock.CustomizeContext = _ => true;

            var dest = ExecuteCustomize();
            Assert.That(dest.IsCustomized, Is.True);

            var root = dest.Custom;
            Assert.That(root.Count, Is.EqualTo(2));
            Assert.That(root[0].Name,             Is.EqualTo("圧縮"));
            Assert.That(root[0].IconIndex,        Is.EqualTo(1));
            Assert.That(root[0].Children.Count,   Is.EqualTo(7));
            Assert.That(root[0].Children[0].Name, Is.EqualTo("Zip"));
            Assert.That(root[0].Children[1].Name, Is.EqualTo("Zip (パスワード)"));
            Assert.That(root[0].Children[2].Name, Is.EqualTo("7-Zip"));
            Assert.That(root[0].Children[3].Name, Is.EqualTo("BZip2"));
            Assert.That(root[0].Children[4].Name, Is.EqualTo("GZip"));
            Assert.That(root[0].Children[5].Name, Is.EqualTo("自己解凍形式"));
            Assert.That(root[0].Children[6].Name, Is.EqualTo("詳細設定"));

            foreach (var item in root[0].Children)
            {
                Assert.That(item.Arguments, Does.StartWith("/c"), item.Name);
                Assert.That(item.IconIndex, Is.EqualTo(1), item.Name);
            }

            Assert.That(root[1].Name,             Is.EqualTo("解凍"));
            Assert.That(root[1].IconIndex,        Is.EqualTo(2));
            Assert.That(root[1].Children.Count,   Is.EqualTo(4));
            Assert.That(root[1].Children[0].Name, Is.EqualTo("ここに解凍"));
            Assert.That(root[1].Children[1].Name, Is.EqualTo("デスクトップに解凍"));
            Assert.That(root[1].Children[2].Name, Is.EqualTo("マイドキュメントに解凍"));
            Assert.That(root[1].Children[3].Name, Is.EqualTo("場所を指定して解凍"));

            foreach (var item in root[1].Children)
            {
                Assert.That(item.Arguments, Does.StartWith("/x"), item.Name);
                Assert.That(item.IconIndex, Is.EqualTo(2), item.Name);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_Cancel
        ///
        /// <summary>
        /// コンテキストメニューのカスタマイズ操作をキャンセルした時の
        /// 挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_Cancel()
        {
            Mock.CustomizeContext = _ => false;

            var dest = ExecuteCustomize();
            Assert.That(dest.IsCustomized, Is.False);
            Assert.That(dest.Custom.Count, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_Add
        ///
        /// <summary>
        /// 新しいメニューを追加するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_Add()
        {
            Mock.CustomizeContext = v =>
            {
                v.Source.SelectedNode = v.Source.Nodes[2];
                v.AddMenu.Execute();
                return true;
            };

            var dest = ExecuteCustomize().Custom;
            Assert.That(dest.Count,             Is.EqualTo(3));
            Assert.That(dest[2].Name,           Is.EqualTo("圧縮してメール送信"));
            Assert.That(dest[2].IconIndex,      Is.EqualTo(1));
            Assert.That(dest[2].Children.Count, Is.EqualTo(8));

            foreach (var item in dest[2].Children) Assert.That(
                item.Arguments,
                Does.StartWith("/c").And.Contain("/m"),
                item.Name
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_NewCategory
        ///
        /// <summary>
        /// 新しいカテゴリーを追加するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_NewCategory()
        {
            Mock.CustomizeContext = v =>
            {
                var node = v.Target.Nodes[0].Nodes[0].Nodes[0];
                Assert.That(node.Text, Is.EqualTo("Zip"));

                v.Target.SelectedNode = node;
                v.NewCategoryMenu.Execute();
                v.Target.SelectedNode.EndEdit(false);

                return true;
            };

            var menu = ExecuteCustomize().Custom;
            Assert.That(menu.Count,   Is.EqualTo(2));
            Assert.That(menu[0].Name, Is.EqualTo("圧縮"));

            var dest = menu[0].Children;
            Assert.That(dest.Count,             Is.EqualTo(8));
            Assert.That(dest[7].Name,           Is.EqualTo("新しいカテゴリー"));
            Assert.That(dest[7].Arguments,      Is.Empty);
            Assert.That(dest[7].IconIndex,      Is.EqualTo(0));
            Assert.That(dest[7].Children.Count, Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_Remove
        ///
        /// <summary>
        /// 選択項目を削除するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_Remove()
        {
            Mock.CustomizeContext = v =>
            {
                v.Target.SelectedNode = v.Target.Nodes[0].Nodes[0];
                v.RemoveMenu.Execute();
                return true;
            };

            var dest = ExecuteCustomize().Custom;
            Assert.That(dest.Count,   Is.EqualTo(1));
            Assert.That(dest[0].Name, Is.EqualTo("解凍"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_Up
        ///
        /// <summary>
        /// 選択項目を上に移動するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_Up()
        {
            Mock.CustomizeContext = v =>
            {
                v.Target.SelectedNode = v.Target.Nodes[0].Nodes[1];
                Assert.That(v.Target.SelectedNode.Text, Is.EqualTo("解凍"));

                v.UpMenu.Execute();
                v.UpMenu.Execute();

                return true;
            };

            var dest = ExecuteCustomize().Custom;
            Assert.That(dest.Count,   Is.EqualTo(2));
            Assert.That(dest[0].Name, Is.EqualTo("解凍"));
            Assert.That(dest[1].Name, Is.EqualTo("圧縮"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_Down
        ///
        /// <summary>
        /// 選択項目を下に移動するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_Down()
        {
            Mock.CustomizeContext = v =>
            {
                v.Target.SelectedNode = v.Target.Nodes[0].Nodes[0];
                Assert.That(v.Target.SelectedNode.Text, Is.EqualTo("圧縮"));

                v.DownMenu.Execute();
                v.DownMenu.Execute();

                return true;
            };

            var dest = ExecuteCustomize().Custom;
            Assert.That(dest.Count,   Is.EqualTo(2));
            Assert.That(dest[0].Name, Is.EqualTo("解凍"));
            Assert.That(dest[1].Name, Is.EqualTo("圧縮"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_Move_Root
        ///
        /// <summary>
        /// ルート項目を選択した状態で移動操作を実行した時の挙動を
        /// 確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_Move_Root()
        {
            Mock.CustomizeContext = v =>
            {
                v.Target.SelectedNode = v.Target.Nodes[0];
                Assert.That(v.Target.SelectedNode.Text, Is.EqualTo("メニュートップ"));

                v.UpMenu.Execute();
                v.DownMenu.Execute();

                return true;
            };

            var dest = ExecuteCustomize().Custom;
            Assert.That(dest.Count,   Is.EqualTo(2));
            Assert.That(dest[0].Name, Is.EqualTo("圧縮"));
            Assert.That(dest[1].Name, Is.EqualTo("解凍"));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize_NotSelected
        ///
        /// <summary>
        /// 非選択時に操作を実行した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Customize_NotSelected()
        {
            Mock.CustomizeContext = v =>
            {
                v.Source.SelectedNode = null;
                v.Target.SelectedNode = null;

                v.AddMenu.Execute();
                v.UpMenu.Execute();
                v.DownMenu.Execute();
                v.RenameMenu.Execute();
                v.RemoveMenu.Execute();

                return true;
            };

            var dest = ExecuteCustomize().Custom;
            Assert.That(dest.Count, Is.EqualTo(2));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PresetSettings
        ///
        /// <summary>
        /// Preset メニューに対応する ViewModel の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void PresetSettings()
        {
            void Set(ContextViewModel cs, bool enabled)
            {
                cs.Archive            = enabled;
                cs.ArchiveBZip2       = enabled;
                cs.ArchiveDetails     = enabled;
                cs.ArchiveGZip        = enabled;
                cs.ArchiveXZ          = enabled;
                cs.ArchiveSevenZip    = enabled;
                cs.ArchiveSfx         = enabled;
                cs.ArchiveZip         = enabled;
                cs.ArchiveZipPassword = enabled;
                cs.Extract            = enabled;
                cs.ExtractDesktop     = enabled;
                cs.ExtractMyDocuments = enabled;
                cs.ExtractRuntime     = enabled;
                cs.ExtractSource      = enabled;
            }

            var m    = CreateSettings();
            var vm   = new MainViewModel(m);
            var src  = vm.Menu;
            var dest = m.Value.Menu;

            Set(src, true);
            Assert.That(src.PresetEnabled, Is.True);
            Assert.That((uint)dest.Preset, Is.EqualTo(0x000fff3));

            Set(src, false);
            Assert.That(src.PresetEnabled, Is.True);
            Assert.That(dest.Preset,       Is.EqualTo(PresetMenu.None));

            src.Reset();
            Assert.That(src.Archive,            Is.True);
            Assert.That(src.ArchiveBZip2,       Is.True);
            Assert.That(src.ArchiveDetails,     Is.True);
            Assert.That(src.ArchiveGZip,        Is.True);
            Assert.That(src.ArchiveXZ,          Is.False);
            Assert.That(src.ArchiveSevenZip,    Is.True);
            Assert.That(src.ArchiveSfx,         Is.True);
            Assert.That(src.ArchiveZip,         Is.True);
            Assert.That(src.ArchiveZipPassword, Is.True);
            Assert.That(src.Extract,            Is.True);
            Assert.That(src.ExtractDesktop,     Is.True);
            Assert.That(src.ExtractMyDocuments, Is.True);
            Assert.That(src.ExtractRuntime,     Is.True);
            Assert.That(src.ExtractSource,      Is.True);
            Assert.That(dest.Preset,            Is.EqualTo(PresetMenu.DefaultContext));
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// ExecuteCustomize
        ///
        /// <summary>
        /// カスタマイズ操作を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ContextSettings ExecuteCustomize()
        {
            var m  = CreateSettings();
            var vm = new MainViewModel(m);
            vm.Menu.Customize();
            return m.Value.Menu;
        }

        #endregion
    }
}
