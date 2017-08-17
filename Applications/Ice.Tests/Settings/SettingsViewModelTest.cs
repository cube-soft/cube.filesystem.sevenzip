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
using System;
using System.Linq;
using Microsoft.Win32;
using Cube.FileSystem.Ice;
using Cube.FileSystem.App.Ice.Settings;
using NUnit.Framework;
using Cube.Registries;

namespace Cube.FileSystem.App.Ice.Tests.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsViewModelTest
    /// 
    /// <summary>
    /// SettingsViewModel のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Parallelizable]
    class SettingsViewModelTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GeneralSettings
        ///
        /// <summary>
        /// Settings オブジェクトに対応する ViewModel の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void GeneralSettings()
        {
            var m = Create();

            var vm = new SettingsViewModel(m)
            {
                CheckUpdate  = true,
                ErrorReport  = true,
                Filtering    = string.Join(Environment.NewLine, new[] { "Foo", "Bar" }),
                ToolTip      = true,
                ToolTipCount = 15
            };

            Assert.That(vm.Version,     Does.StartWith("Version 1.0.0"));
            Assert.That(vm.InstallMode, Is.False);

            Assert.That(m.Value.CheckUpdate,          Is.True);
            Assert.That(m.Value.ErrorReport,          Is.True);
            Assert.That(m.Value.GetFilters().Count(), Is.EqualTo(2));
            Assert.That(m.Value.ToolTip,              Is.True);
            Assert.That(m.Value.ToolTipCount,         Is.EqualTo(15));

            vm.CheckUpdate  = false;
            vm.ErrorReport  = false;
            vm.Filtering    = string.Empty;
            vm.ToolTip      = false;
            vm.ToolTipCount = 0;

            Assert.That(m.Value.CheckUpdate,          Is.False);
            Assert.That(m.Value.ErrorReport,          Is.False);
            Assert.That(m.Value.GetFilters().Count(), Is.EqualTo(0));
            Assert.That(m.Value.ToolTip,              Is.False);
            Assert.That(m.Value.ToolTipCount,         Is.EqualTo(0));

            m.Value.CheckUpdate  = true;
            m.Value.ErrorReport  = true;
            m.Value.Filters      = "Add|Filter|By|Model";
            m.Value.ToolTip      = true;
            m.Value.ToolTipCount = 9;

            var f = vm.Filtering.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries);

            Assert.That(f.Count(),       Is.EqualTo(4));
            Assert.That(vm.CheckUpdate,  Is.True);
            Assert.That(vm.ErrorReport,  Is.True);
            Assert.That(vm.ToolTip,      Is.True);
            Assert.That(vm.ToolTipCount, Is.EqualTo(9));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveSettings
        ///
        /// <summary>
        /// ArchiveSettings オブジェクトに対応する ViewModel の挙動を
        /// 確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void ArchiveSettings()
        {
            var m    = Create();
            var vm   = new SettingsViewModel(m);
            var src  = vm.Archive;
            var dest = m.Value.Archive;

            src.SaveSource = true;
            Assert.That(src.SaveSource,    Is.True);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.False);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Source));

            src.SaveRuntime = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.True);
            Assert.That(src.SaveOthers,    Is.False);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Runtime));

            src.SaveOthers = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            // update only when set to true
            src.SaveOthers = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            src.SaveSource = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            src.SaveRuntime = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            src.OpenDirectory = true;
            src.SkipDesktop = true;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.OpenNotDesktop));

            src.SkipDesktop = false;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.Open));

            src.OpenDirectory = false;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.None));

            src.SkipDesktop = true;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.SkipDesktop));

            dest.OpenDirectory = OpenDirectoryMethod.OpenNotDesktop;
            Assert.That(src.OpenDirectory, Is.True);
            Assert.That(src.SkipDesktop,   Is.True);

            src.SaveDirectoryName = @"path\to\save";
            Assert.That(dest.SaveDirectoryName, Is.EqualTo(@"path\to\save"));
            src.SaveDirectoryName = string.Empty;
            Assert.That(dest.SaveDirectoryName, Is.Empty);
            dest.SaveDirectoryName = @"path\to\new";
            Assert.That(src.SaveDirectoryName, Is.EqualTo(@"path\to\new"));

            src.Filtering = true;
            Assert.That(dest.Filtering, Is.True);
            src.Filtering = false;
            Assert.That(dest.Filtering, Is.False);
            dest.Filtering = true;
            Assert.That(src.Filtering, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractSettings
        ///
        /// <summary>
        /// ExtractSettings オブジェクトに対応する ViewModel の挙動を
        /// 確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void ExtractSettings()
        {
            var m    = Create();
            var vm   = new SettingsViewModel(m);
            var src  = vm.Extract;
            var dest = m.Value.Extract;

            src.SaveSource = true;
            Assert.That(src.SaveSource,    Is.True);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.False);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Source));

            src.SaveRuntime = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.True);
            Assert.That(src.SaveOthers,    Is.False);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Runtime));

            src.SaveOthers = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            // update only when set to true
            src.SaveOthers = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            src.SaveSource = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            src.SaveRuntime = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Others));

            src.OpenDirectory = true;
            src.SkipDesktop = true;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.OpenNotDesktop));

            src.SkipDesktop = false;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.Open));

            src.OpenDirectory = false;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.None));

            src.SkipDesktop = true;
            Assert.That(dest.OpenDirectory, Is.EqualTo(OpenDirectoryMethod.SkipDesktop));

            dest.OpenDirectory = OpenDirectoryMethod.OpenNotDesktop;
            Assert.That(src.OpenDirectory, Is.True);
            Assert.That(src.SkipDesktop,   Is.True);

            src.CreateDirectory = true;
            src.SkipSingleDirectory = true;
            Assert.That(dest.RootDirectory, Is.EqualTo(CreateDirectoryMethod.CreateSmart));

            src.SkipSingleDirectory = false;
            Assert.That(dest.RootDirectory, Is.EqualTo(CreateDirectoryMethod.Create));

            src.CreateDirectory = false;
            Assert.That(dest.RootDirectory, Is.EqualTo(CreateDirectoryMethod.None));

            src.SkipSingleDirectory = true;
            Assert.That(dest.RootDirectory, Is.EqualTo(CreateDirectoryMethod.SkipSingleDirectory));

            dest.RootDirectory = CreateDirectoryMethod.Create |
                                 CreateDirectoryMethod.SkipSingleDirectory |
                                 CreateDirectoryMethod.SkipSingleFile;
            Assert.That(src.CreateDirectory,     Is.True);
            Assert.That(src.SkipSingleDirectory, Is.True);

            src.SaveDirectoryName = @"path\to\extract";
            Assert.That(dest.SaveDirectoryName, Is.EqualTo(@"path\to\extract"));
            src.SaveDirectoryName = string.Empty;
            Assert.That(dest.SaveDirectoryName, Is.Empty);
            dest.SaveDirectoryName = @"path\to\ex2";
            Assert.That(src.SaveDirectoryName, Is.EqualTo(@"path\to\ex2"));

            src.DeleteSource = true;
            Assert.That(dest.DeleteSource, Is.True);
            src.DeleteSource = false;
            Assert.That(dest.DeleteSource, Is.False);
            dest.DeleteSource = true;
            Assert.That(src.DeleteSource, Is.True);

            src.Filtering = true;
            Assert.That(dest.Filtering, Is.True);
            src.Filtering = false;
            Assert.That(dest.Filtering, Is.False);
            dest.Filtering = true;
            Assert.That(src.Filtering, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateSettings
        ///
        /// <summary>
        /// AssociateSettings オブジェクトに対応する ViewModel の
        /// 挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void AssociateSettings()
        {
            var m    = Create();
            var vm   = new SettingsViewModel(m);
            var src  = vm.Associate;
            var dest = m.Value.Associate;

            Assert.That(dest.Value.Count, Is.EqualTo(29));

            src.SelectAll();
            Assert.That(src.Arj,      Is.True); Assert.That(dest.Arj,      Is.True);
            Assert.That(src.BZ2,      Is.True); Assert.That(dest.BZ2,      Is.True);
            Assert.That(src.Cab,      Is.True); Assert.That(dest.Cab,      Is.True);
            Assert.That(src.Chm,      Is.True); Assert.That(dest.Chm,      Is.True);
            Assert.That(src.Cpio,     Is.True); Assert.That(dest.Cpio,     Is.True);
            Assert.That(src.Deb,      Is.True); Assert.That(dest.Deb,      Is.True);
            Assert.That(src.Dmg,      Is.True); Assert.That(dest.Dmg,      Is.True);
            Assert.That(src.Flv,      Is.True); Assert.That(dest.Flv,      Is.True);
            Assert.That(src.GZ,       Is.True); Assert.That(dest.GZ,       Is.True);
            Assert.That(src.Hfs,      Is.True); Assert.That(dest.Hfs,      Is.True);
            Assert.That(src.Iso,      Is.True); Assert.That(dest.Iso,      Is.True);
            Assert.That(src.Jar,      Is.True); Assert.That(dest.Jar,      Is.True);
            Assert.That(src.Lzh,      Is.True); Assert.That(dest.Lzh,      Is.True);
            Assert.That(src.Nupkg,    Is.True); Assert.That(dest.Nupkg,    Is.True);
            Assert.That(src.Rar,      Is.True); Assert.That(dest.Rar,      Is.True);
            Assert.That(src.Rpm,      Is.True); Assert.That(dest.Rpm,      Is.True);
            Assert.That(src.SevenZip, Is.True); Assert.That(dest.SevenZip, Is.True);
            Assert.That(src.Swf,      Is.True); Assert.That(dest.Swf,      Is.True);
            Assert.That(src.Tar,      Is.True); Assert.That(dest.Tar,      Is.True);
            Assert.That(src.Tbz,      Is.True); Assert.That(dest.Tbz,      Is.True);
            Assert.That(src.Tgz,      Is.True); Assert.That(dest.Tgz,      Is.True);
            Assert.That(src.Txz,      Is.True); Assert.That(dest.Txz,      Is.True);
            Assert.That(src.Vhd,      Is.True); Assert.That(dest.Vhd,      Is.True);
            Assert.That(src.Vmdk,     Is.True); Assert.That(dest.Vmdk,     Is.True);
            Assert.That(src.Wim,      Is.True); Assert.That(dest.Wim,      Is.True);
            Assert.That(src.Xar,      Is.True); Assert.That(dest.Xar,      Is.True);
            Assert.That(src.XZ,       Is.True); Assert.That(dest.XZ,       Is.True);
            Assert.That(src.Z,        Is.True); Assert.That(dest.Z,        Is.True);
            Assert.That(src.Zip,      Is.True); Assert.That(dest.Zip,      Is.True);

            src.Clear();
            Assert.That(src.Arj,      Is.False); Assert.That(dest.Arj,      Is.False);
            Assert.That(src.BZ2,      Is.False); Assert.That(dest.BZ2,      Is.False);
            Assert.That(src.Cab,      Is.False); Assert.That(dest.Cab,      Is.False);
            Assert.That(src.Chm,      Is.False); Assert.That(dest.Chm,      Is.False);
            Assert.That(src.Cpio,     Is.False); Assert.That(dest.Cpio,     Is.False);
            Assert.That(src.Deb,      Is.False); Assert.That(dest.Deb,      Is.False);
            Assert.That(src.Dmg,      Is.False); Assert.That(dest.Dmg,      Is.False);
            Assert.That(src.Flv,      Is.False); Assert.That(dest.Flv,      Is.False);
            Assert.That(src.GZ,       Is.False); Assert.That(dest.GZ,       Is.False);
            Assert.That(src.Hfs,      Is.False); Assert.That(dest.Hfs,      Is.False);
            Assert.That(src.Iso,      Is.False); Assert.That(dest.Iso,      Is.False);
            Assert.That(src.Jar,      Is.False); Assert.That(dest.Jar,      Is.False);
            Assert.That(src.Lzh,      Is.False); Assert.That(dest.Lzh,      Is.False);
            Assert.That(src.Nupkg,    Is.False); Assert.That(dest.Nupkg,    Is.False);
            Assert.That(src.Rar,      Is.False); Assert.That(dest.Rar,      Is.False);
            Assert.That(src.Rpm,      Is.False); Assert.That(dest.Rpm,      Is.False);
            Assert.That(src.SevenZip, Is.False); Assert.That(dest.SevenZip, Is.False);
            Assert.That(src.Swf,      Is.False); Assert.That(dest.Swf,      Is.False);
            Assert.That(src.Tar,      Is.False); Assert.That(dest.Tar,      Is.False);
            Assert.That(src.Tbz,      Is.False); Assert.That(dest.Tbz,      Is.False);
            Assert.That(src.Tgz,      Is.False); Assert.That(dest.Tgz,      Is.False);
            Assert.That(src.Txz,      Is.False); Assert.That(dest.Txz,      Is.False);
            Assert.That(src.Vhd,      Is.False); Assert.That(dest.Vhd,      Is.False);
            Assert.That(src.Vmdk,     Is.False); Assert.That(dest.Vmdk,     Is.False);
            Assert.That(src.Wim,      Is.False); Assert.That(dest.Wim,      Is.False);
            Assert.That(src.Xar,      Is.False); Assert.That(dest.Xar,      Is.False);
            Assert.That(src.XZ,       Is.False); Assert.That(dest.XZ,       Is.False);
            Assert.That(src.Z,        Is.False); Assert.That(dest.Z,        Is.False);
            Assert.That(src.Zip,      Is.False); Assert.That(dest.Zip,      Is.False);

            src.Arj = true;
            Assert.That(dest.Arj, Is.True);
            src.Zip = true;
            Assert.That(dest.Zip, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ContextMenuSettings
        ///
        /// <summary>
        /// ContextMenuSettings オブジェクトに対応する ViewModel の
        /// 挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void ContextMenuSettings()
        {
            var m    = Create();
            var vm   = new SettingsViewModel(m);
            var src  = vm.Context;
            var dest = m.Value.Context;

            src.Archive            = true;
            src.ArchiveBZip2       = true;
            src.ArchiveDetail      = true;
            src.ArchiveGZip        = true;
            src.ArchiveSevenZip    = true;
            src.ArchiveSfx         = true;
            src.ArchiveZip         = true;
            src.ArchiveZipPassword = true;
            src.Extract            = true;
            src.ExtractDesktop     = true;
            src.ExtractMyDocuments = true;
            src.ExtractRuntime     = true;
            src.ExtractSource      = true;
            src.Mail               = true;
            src.MailBZip2          = true;
            src.MailDetail         = true;
            src.MailGZip           = true;
            src.MailSevenZip       = true;
            src.MailSfx            = true;
            src.MailZip            = true;
            src.MailZipPassword    = true;
            Assert.That((uint)dest.Preset, Is.EqualTo(0x07f7ffb));

            src.Archive            = false;
            src.ArchiveBZip2       = false;
            src.ArchiveDetail      = false;
            src.ArchiveGZip        = false;
            src.ArchiveSevenZip    = false;
            src.ArchiveSfx         = false;
            src.ArchiveZip         = false;
            src.ArchiveZipPassword = false;
            src.Extract            = false;
            src.ExtractDesktop     = false;
            src.ExtractMyDocuments = false;
            src.ExtractRuntime     = false;
            src.ExtractSource      = false;
            src.Mail               = false;
            src.MailBZip2          = false;
            src.MailDetail         = false;
            src.MailGZip           = false;
            src.MailSevenZip       = false;
            src.MailSfx            = false;
            src.MailZip            = false;
            src.MailZipPassword    = false;
            Assert.That(dest.Preset, Is.EqualTo(PresetMenu.None));

            src.Reset();
            Assert.That(src.Archive,            Is.True);
            Assert.That(src.ArchiveBZip2,       Is.True);
            Assert.That(src.ArchiveDetail,      Is.True);
            Assert.That(src.ArchiveGZip,        Is.True);
            Assert.That(src.ArchiveSevenZip,    Is.True);
            Assert.That(src.ArchiveSfx,         Is.True);
            Assert.That(src.ArchiveZip,         Is.True);
            Assert.That(src.ArchiveZipPassword, Is.True);
            Assert.That(src.Extract,            Is.True);
            Assert.That(src.ExtractDesktop,     Is.True);
            Assert.That(src.ExtractMyDocuments, Is.True);
            Assert.That(src.ExtractRuntime,     Is.True);
            Assert.That(src.ExtractSource,      Is.True);
            Assert.That(src.Mail,               Is.False);
            Assert.That(src.MailBZip2,          Is.False);
            Assert.That(src.MailDetail,         Is.False);
            Assert.That(src.MailGZip,           Is.False);
            Assert.That(src.MailSevenZip,       Is.False);
            Assert.That(src.MailSfx,            Is.False);
            Assert.That(src.MailZip,            Is.False);
            Assert.That(src.MailZipPassword,    Is.False);
            Assert.That(dest.Preset,            Is.EqualTo(PresetMenu.DefaultContext));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutSettings
        ///
        /// <summary>
        /// ShortcutSettings オブジェクトに対応する ViewModel の
        /// 挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void ShortcutSettings()
        {
            var m    = Create();
            var vm   = new SettingsViewModel(m);
            var src  = vm.Shortcut;
            var dest = m.Value.Shortcut;

            var io  = new Operator();
            var asm = System.Reflection.Assembly.GetExecutingAssembly().Location;
            dest.Directory = io.Combine(io.Get(asm).DirectoryName, "Results");

            src.Archive       = true;
            src.Extract       = true;
            src.Settings      = true;
            src.ArchiveOption = PresetMenu.ArchiveZip;
            Assert.That((uint)dest.Preset, Is.EqualTo(0x107));

            src.Sync();
            Assert.That(src.Archive,       Is.False);
            Assert.That(src.Extract,       Is.False);
            Assert.That(src.Settings,      Is.False);
            Assert.That(src.ArchiveOption, Is.EqualTo(PresetMenu.ArchiveZip));
            Assert.That(dest.Preset,       Is.EqualTo(PresetMenu.ArchiveZip));

            src.Archive  = false;
            src.Extract  = false;
            src.Settings = false;
            Assert.That(dest.Preset, Is.EqualTo(PresetMenu.ArchiveZip));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SyncUpdate
        ///
        /// <summary>
        /// Sync および Update コマンドのテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(true)]
        [TestCase(false)]
        public void SyncUpdate(bool install)
        {
            var vm = new SettingsViewModel(Create()) { InstallMode = install };
            vm.CheckUpdate = false;
            vm.Sync();
            vm.Associate.Clear();
            vm.Update();

            var m = Create();
            m.Load();
            new SettingsViewModel(m).Update();

            Assert.Pass();
        }

        #endregion

        #region Helper

        /* ----------------------------------------------------------------- */
        ///
        /// KeyName
        ///
        /// <summary>
        /// テスト用のレジストリキー名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private static string KeyName = "CubeIceTest";

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Model オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private SettingsFolder Create() => new SettingsFolder(KeyName);

        /* ----------------------------------------------------------------- */
        ///
        /// TearDown
        ///
        /// <summary>
        /// テスト毎に実行される TearDown 処理です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TearDown]
        public void TearDown()
        {
            try
            {
                using (var root = Registry.CurrentUser.OpenSubKey("CubeSoft", true))
                {
                    root.DeleteSubKeyTree(KeyName, false);
                }
            }
            catch (Exception /* err */) { /* ignore */ }
        }

        #endregion
    }
}
