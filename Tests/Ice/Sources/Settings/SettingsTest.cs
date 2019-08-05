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
using Cube.FileSystem.TestService;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsTest
    ///
    /// <summary>
    /// 各種 Settings のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingsTest : FileFixture
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Settings クラスの初期値を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var asm  = Assembly.GetExecutingAssembly();
            var dest = new SettingsFolder(asm, IO);
            Assert.That(dest.AutoSave,           Is.False);
            Assert.That(dest.AutoSaveDelay,      Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(dest.Version.ToString(), Is.EqualTo("0.10.0β"));
            Assert.That(dest.Format,             Is.EqualTo(Cube.DataContract.Format.Registry));
            Assert.That(dest.Location,           Is.EqualTo(@"CubeSoft\CubeICE\v3"));
            Assert.That(dest.Value,              Is.Not.Null);
        }

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
            var archive  = new Shortcut { FullName = Get("CubeICE 圧縮") };
            var extract  = new Shortcut { FullName = Get("CubeICE 解凍") };
            var settings = new Shortcut { FullName = Get("CubeICE 設定") };

            var src  = new ShortcutSettings { Directory = Results };
            var menu = PresetMenu.Archive |
                       PresetMenu.ArchiveDetails |
                       PresetMenu.Extract |
                       PresetMenu.Settings;

            src.Preset = menu;
            Assert.That(src.Preset, Is.EqualTo(menu));

            src.Sync();
            Assert.That(src.Preset.HasFlag(PresetMenu.Archive),        Is.False);
            Assert.That(src.Preset.HasFlag(PresetMenu.ArchiveDetails), Is.True);
            Assert.That(src.Preset.HasFlag(PresetMenu.Extract),        Is.False);
            Assert.That(src.Preset.HasFlag(PresetMenu.Settings),       Is.False);

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

        /* ----------------------------------------------------------------- */
        ///
        /// Convert_ToOption
        ///
        /// <summary>
        /// ArchiveRtSettings を ArchiveOption オブジェクトに変換する
        /// テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Convert_ToOption()
        {
            var asm  = Assembly.GetExecutingAssembly();
            var dest = new ArchiveRtSettings(IO)
            {
                CompressionLevel  = CompressionLevel.High,
                CompressionMethod = CompressionMethod.Ppmd,
                EncryptionMethod  = EncryptionMethod.Aes192,
                Format            = Format.GZip,
                Password          = "password",
                Path              = "dummy",
                SfxModule         = string.Empty,
                ThreadCount       = 3,
            }.ToOption(new SettingsFolder(asm, IO));

            Assert.That(dest.CompressionLevel, Is.EqualTo(CompressionLevel.High));
            Assert.That(dest.ThreadCount,      Is.EqualTo(3));
        }
    }
}
