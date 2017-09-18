/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using Cube.FileSystem.Ice;
using NUnit.Framework;
using Cube.Enumerations;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutSettingsTest
    /// 
    /// <summary>
    /// ShortcutSettings のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ShortcutSettingsTest : MockViewHandler
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SyncUpdate
        /// 
        /// <summary>
        /// ShortcutSettings の更新テストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void SyncUpdate()
        {
            var archive  = new Shortcut(Result("CubeICE 圧縮"));
            var extract  = new Shortcut(Result("CubeICE 解凍"));
            var settings = new Shortcut(Result("CubeICE 設定"));

            var src  = new ShortcutSettings() { Directory = Results };
            var menu = PresetMenu.Archive |
                       PresetMenu.ArchiveDetail |
                       PresetMenu.Extract |
                       PresetMenu.Settings;

            src.Preset = menu;
            Assert.That(src.Preset, Is.EqualTo(menu));

            src.Sync();
            Assert.That(src.Preset.HasFlag(PresetMenu.Archive),       Is.False);
            Assert.That(src.Preset.HasFlag(PresetMenu.ArchiveDetail), Is.True);
            Assert.That(src.Preset.HasFlag(PresetMenu.Extract),       Is.False);
            Assert.That(src.Preset.HasFlag(PresetMenu.Settings),      Is.False);

            src.Preset = menu;
            src.Update();
            Assert.That(archive.Exists,  Is.True);
            Assert.That(extract.Exists,  Is.True);
            Assert.That(settings.Exists, Is.True);

            src.Preset = PresetMenu.None;
            src.Sync();
            Assert.That(src.Preset.HasFlag(PresetMenu.Archive),  Is.True);
            Assert.That(src.Preset.HasFlag(PresetMenu.Extract),  Is.True);
            Assert.That(src.Preset.HasFlag(PresetMenu.Settings), Is.True);

            src.Preset = PresetMenu.None;
            src.Update();
            Assert.That(archive.Exists,  Is.False);
            Assert.That(extract.Exists,  Is.False);
            Assert.That(settings.Exists, Is.False);
        }
    }
}
