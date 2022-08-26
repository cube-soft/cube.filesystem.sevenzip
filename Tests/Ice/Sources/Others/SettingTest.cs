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
using System;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingTest
    ///
    /// <summary>
    /// Tests SettingFolder and related classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Tests the constructor and confirms properties.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create()
        {
            var dest = new SettingFolder();
            Assert.That(dest.AutoSave,           Is.False);
            Assert.That(dest.AutoSaveDelay,      Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(dest.Version.ToString(), Is.EqualTo("2.0.2"));
            Assert.That(dest.Format,             Is.EqualTo(DataContract.Format.Registry));
            Assert.That(dest.Location,           Is.EqualTo(@"CubeSoft\CubeICE\v3"));

            var v = dest.Value;
            Assert.That(v, Is.Not.Null);
            Assert.That(v.Associate.Changed, Is.False);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Sync_Shortcut
        ///
        /// <summary>
        /// Tests the Sync method that loads the shortcut settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Sync_Shortcut()
        {
            var archive  = new Shortcut { FullName = Get("CubeICE 圧縮") };
            var extract  = new Shortcut { FullName = Get("CubeICE 解凍") };
            var settings = new Shortcut { FullName = Get("CubeICE 設定") };

            var src  = new ShortcutSettingValue { Directory = Results };
            var menu = Preset.Compress |
                       Preset.CompressDetails |
                       Preset.Extract |
                       Preset.Settings;

            src.Preset = menu;
            Assert.That(src.Preset, Is.EqualTo(menu));

            src.Load();
            Assert.That(src.Preset.HasFlag(Preset.Compress),       Is.False);
            Assert.That(src.Preset.HasFlag(Preset.CompressDetails), Is.True);
            Assert.That(src.Preset.HasFlag(Preset.Extract),        Is.False);
            Assert.That(src.Preset.HasFlag(Preset.Settings),       Is.False);

            src.Preset = menu;
            src.Save();
            Assert.That(archive.Exists,  Is.True);
            Assert.That(extract.Exists,  Is.True);
            Assert.That(settings.Exists, Is.True);

            src.Preset = Preset.None;
            src.Load();
            Assert.That(src.Preset.HasFlag(Preset.Compress), Is.True);
            Assert.That(src.Preset.HasFlag(Preset.Extract),  Is.True);
            Assert.That(src.Preset.HasFlag(Preset.Settings), Is.True);

            src.Preset = Preset.None;
            src.Save();
            Assert.That(archive.Exists,  Is.False);
            Assert.That(extract.Exists,  Is.False);
            Assert.That(settings.Exists, Is.False);
        }

        #endregion
    }
}
