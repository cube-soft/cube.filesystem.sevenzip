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
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingViewModelTest
    ///
    /// <summary>
    /// Tests the SettingViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class SettingViewModelTest : SettingFixture
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

            var vm = new MainViewModel(m, new SynchronizationContext())
            {
                CheckUpdate  = true,
                ErrorReport  = true,
                Filtering    = string.Join(Environment.NewLine, new[] { "Foo", "Bar" }),
                ToolTip      = true,
                ToolTipCount = 15
            };

            Assert.That(vm.Version, Does.StartWith("Version"));

            Assert.That(m.Value.ErrorReport,          Is.True);
            Assert.That(m.Value.GetFilters().Count(), Is.EqualTo(2));
            Assert.That(m.Value.ToolTip,              Is.True);
            Assert.That(m.Value.ToolTipCount,         Is.EqualTo(15));

            vm.CheckUpdate  = false;
            vm.ErrorReport  = false;
            vm.Filtering    = string.Empty;
            vm.ToolTip      = false;
            vm.ToolTipCount = 0;

            Assert.That(m.Value.ErrorReport,          Is.False);
            Assert.That(m.Value.GetFilters().Count(), Is.EqualTo(0));
            Assert.That(m.Value.ToolTip,              Is.False);
            Assert.That(m.Value.ToolTipCount,         Is.EqualTo(0));

            m.Value.ErrorReport  = true;
            m.Value.Filters      = "Add|Filter|By|Model";
            m.Value.ToolTip      = true;
            m.Value.ToolTipCount = 9;

            var f = vm.Filtering.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries);

            Assert.That(f.Count(),            Is.EqualTo(4));
            Assert.That(vm.CheckUpdate,       Is.True);
            Assert.That(vm.ErrorReport,       Is.True);
            Assert.That(vm.ToolTip,           Is.True);
            Assert.That(vm.ToolTipCount,      Is.EqualTo(9));
            Assert.That(vm.Associate.Changed, Is.True);
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
            var vm   = new MainViewModel(m, new SynchronizationContext());
            var src  = vm.Compress;
            var dest = m.Value.Compress;

            src.SaveSource = true;
            Assert.That(src.SaveSource,    Is.True);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.False);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Source));

            src.SaveRuntime = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.True);
            Assert.That(src.SaveOthers,    Is.False);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Query));

            src.SaveOthers = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            // update only when set to true
            src.SaveOthers = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            src.SaveSource = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            src.SaveRuntime = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            src.UseUtf8 = true;
            Assert.That(dest.UseUtf8, Is.EqualTo(src.UseUtf8).And.True);

            src.OverwritePrompt = false;
            Assert.That(dest.OverwritePrompt, Is.EqualTo(src.OverwritePrompt).And.False);

            src.OpenDirectory = true;
            src.SkipDesktop = true;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.OpenNotDesktop));

            src.SkipDesktop = false;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.Open));

            src.OpenDirectory = false;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.None));

            src.SkipDesktop = true;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.SkipDesktop));

            dest.OpenMethod = OpenMethod.OpenNotDesktop;
            Assert.That(src.OpenDirectory, Is.True);
            Assert.That(src.SkipDesktop,   Is.True);

            src.SaveDirectory = @"path\to\save";
            Assert.That(dest.SaveDirectory, Is.EqualTo(@"path\to\save"));
            src.SaveDirectory = string.Empty;
            Assert.That(dest.SaveDirectory, Is.Empty);
            dest.SaveDirectory = @"path\to\new";
            Assert.That(src.SaveDirectory, Is.EqualTo(@"path\to\new"));

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
            var vm   = new MainViewModel(m, new SynchronizationContext());
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
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Query));

            src.SaveOthers = true;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            // update only when set to true
            src.SaveOthers = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            src.SaveSource = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            src.SaveRuntime = false;
            Assert.That(src.SaveSource,    Is.False);
            Assert.That(src.SaveRuntime,   Is.False);
            Assert.That(src.SaveOthers,    Is.True);
            Assert.That(dest.SaveLocation, Is.EqualTo(SaveLocation.Preset));

            src.OpenDirectory = true;
            src.SkipDesktop = true;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.OpenNotDesktop));

            src.SkipDesktop = false;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.Open));

            src.OpenDirectory = false;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.None));

            src.SkipDesktop = true;
            Assert.That(dest.OpenMethod, Is.EqualTo(OpenMethod.SkipDesktop));

            dest.OpenMethod = OpenMethod.OpenNotDesktop;
            Assert.That(src.OpenDirectory, Is.True);
            Assert.That(src.SkipDesktop,   Is.True);

            src.CreateDirectory = true;
            src.SkipSingleDirectory = true;
            Assert.That(dest.SaveMethod, Is.EqualTo(SaveMethod.CreateSmart));

            src.SkipSingleDirectory = false;
            Assert.That(dest.SaveMethod, Is.EqualTo(SaveMethod.Create));

            src.CreateDirectory = false;
            Assert.That(dest.SaveMethod, Is.EqualTo(SaveMethod.None));

            src.SkipSingleDirectory = true;
            Assert.That(dest.SaveMethod, Is.EqualTo(SaveMethod.SkipSingleDirectory));

            dest.SaveMethod = SaveMethod.Create |
                              SaveMethod.SkipSingleDirectory |
                              SaveMethod.SkipSingleFile;
            Assert.That(src.CreateDirectory,     Is.True);
            Assert.That(src.SkipSingleDirectory, Is.True);

            src.SaveDirectory = @"path\to\extract";
            Assert.That(dest.SaveDirectory, Is.EqualTo(@"path\to\extract"));
            src.SaveDirectory = string.Empty;
            Assert.That(dest.SaveDirectory, Is.Empty);
            dest.SaveDirectory = @"path\to\ex2";
            Assert.That(src.SaveDirectory, Is.EqualTo(@"path\to\ex2"));

            src.DeleteSource = true;
            Assert.That(dest.DeleteSource, Is.True);
            src.DeleteSource = false;
            Assert.That(dest.DeleteSource, Is.False);
            dest.DeleteSource = true;
            Assert.That(src.DeleteSource, Is.True);

            src.Bursty = true;
            Assert.That(dest.Bursty, Is.True);
            src.Bursty = false;
            Assert.That(dest.Bursty, Is.False);
            dest.Bursty = true;
            Assert.That(src.Bursty, Is.True);

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
            var vm   = new MainViewModel(m, new SynchronizationContext());
            var src  = vm.Associate;
            var dest = m.Value.Associate;

            Assert.That(dest.Value.Count, Is.EqualTo(29));
            Assert.That(dest.IconIndex,   Is.EqualTo(3));
            Assert.That(dest.Changed,     Is.False);

            src.IconIndex = 0;
            Assert.That(dest.IconIndex, Is.EqualTo(src.IconIndex).And.EqualTo(0));
            Assert.That(dest.Changed,   Is.True);

            src.SelectAll();
            Assert.That(dest.Arj,      Is.EqualTo(src.Arj).And.True);
            Assert.That(dest.BZ2,      Is.EqualTo(src.BZ2).And.True);
            Assert.That(dest.Cab,      Is.EqualTo(src.Cab).And.True);
            Assert.That(dest.Chm,      Is.EqualTo(src.Chm).And.True);
            Assert.That(dest.Cpio,     Is.EqualTo(src.Cpio).And.True);
            Assert.That(dest.Deb,      Is.EqualTo(src.Deb).And.True);
            Assert.That(dest.Dmg,      Is.EqualTo(src.Dmg).And.True);
            Assert.That(dest.Flv,      Is.EqualTo(src.Flv).And.True);
            Assert.That(dest.GZ,       Is.EqualTo(src.GZ).And.True);
            Assert.That(dest.Hfs,      Is.EqualTo(src.Hfs).And.True);
            Assert.That(dest.Iso,      Is.EqualTo(src.Iso).And.True);
            Assert.That(dest.Jar,      Is.EqualTo(src.Jar).And.True);
            Assert.That(dest.Lzh,      Is.EqualTo(src.Lzh).And.True);
            Assert.That(dest.Nupkg,    Is.EqualTo(src.Nupkg).And.True);
            Assert.That(dest.Rar,      Is.EqualTo(src.Rar).And.True);
            Assert.That(dest.Rpm,      Is.EqualTo(src.Rpm).And.True);
            Assert.That(dest.SevenZip, Is.EqualTo(src.SevenZip).And.True);
            Assert.That(dest.Swf,      Is.EqualTo(src.Swf).And.True);
            Assert.That(dest.Tar,      Is.EqualTo(src.Tar).And.True);
            Assert.That(dest.Tbz,      Is.EqualTo(src.Tbz).And.True);
            Assert.That(dest.Tgz,      Is.EqualTo(src.Tgz).And.True);
            Assert.That(dest.Txz,      Is.EqualTo(src.Txz).And.True);
            Assert.That(dest.Vhd,      Is.EqualTo(src.Vhd).And.True);
            Assert.That(dest.Vmdk,     Is.EqualTo(src.Vmdk).And.True);
            Assert.That(dest.Wim,      Is.EqualTo(src.Wim).And.True);
            Assert.That(dest.Xar,      Is.EqualTo(src.Xar).And.True);
            Assert.That(dest.XZ,       Is.EqualTo(src.XZ).And.True);
            Assert.That(dest.Z,        Is.EqualTo(src.Z).And.True);
            Assert.That(dest.Zip,      Is.EqualTo(src.Zip).And.True);

            src.Clear();
            Assert.That(dest.Arj,      Is.EqualTo(src.Arj).And.False);
            Assert.That(dest.BZ2,      Is.EqualTo(src.BZ2).And.False);
            Assert.That(dest.Cab,      Is.EqualTo(src.Cab).And.False);
            Assert.That(dest.Chm,      Is.EqualTo(src.Chm).And.False);
            Assert.That(dest.Cpio,     Is.EqualTo(src.Cpio).And.False);
            Assert.That(dest.Deb,      Is.EqualTo(src.Deb).And.False);
            Assert.That(dest.Dmg,      Is.EqualTo(src.Dmg).And.False);
            Assert.That(dest.Flv,      Is.EqualTo(src.Flv).And.False);
            Assert.That(dest.GZ,       Is.EqualTo(src.GZ).And.False);
            Assert.That(dest.Hfs,      Is.EqualTo(src.Hfs).And.False);
            Assert.That(dest.Iso,      Is.EqualTo(src.Iso).And.False);
            Assert.That(dest.Jar,      Is.EqualTo(src.Jar).And.False);
            Assert.That(dest.Lzh,      Is.EqualTo(src.Lzh).And.False);
            Assert.That(dest.Nupkg,    Is.EqualTo(src.Nupkg).And.False);
            Assert.That(dest.Rar,      Is.EqualTo(src.Rar).And.False);
            Assert.That(dest.Rpm,      Is.EqualTo(src.Rpm).And.False);
            Assert.That(dest.SevenZip, Is.EqualTo(src.SevenZip).And.False);
            Assert.That(dest.Swf,      Is.EqualTo(src.Swf).And.False);
            Assert.That(dest.Tar,      Is.EqualTo(src.Tar).And.False);
            Assert.That(dest.Tbz,      Is.EqualTo(src.Tbz).And.False);
            Assert.That(dest.Tgz,      Is.EqualTo(src.Tgz).And.False);
            Assert.That(dest.Txz,      Is.EqualTo(src.Txz).And.False);
            Assert.That(dest.Vhd,      Is.EqualTo(src.Vhd).And.False);
            Assert.That(dest.Vmdk,     Is.EqualTo(src.Vmdk).And.False);
            Assert.That(dest.Wim,      Is.EqualTo(src.Wim).And.False);
            Assert.That(dest.Xar,      Is.EqualTo(src.Xar).And.False);
            Assert.That(dest.XZ,       Is.EqualTo(src.XZ).And.False);
            Assert.That(dest.Z,        Is.EqualTo(src.Z).And.False);
            Assert.That(dest.Zip,      Is.EqualTo(src.Zip).And.False);

            src.Arj      = true; Assert.That(dest.Arj,      Is.True);
            src.BZ2      = true; Assert.That(dest.BZ2,      Is.True);
            src.Cab      = true; Assert.That(dest.Cab,      Is.True);
            src.Chm      = true; Assert.That(dest.Chm,      Is.True);
            src.Cpio     = true; Assert.That(dest.Cpio,     Is.True);
            src.Deb      = true; Assert.That(dest.Deb,      Is.True);
            src.Dmg      = true; Assert.That(dest.Dmg,      Is.True);
            src.Flv      = true; Assert.That(dest.Flv,      Is.True);
            src.GZ       = true; Assert.That(dest.GZ,       Is.True);
            src.Hfs      = true; Assert.That(dest.Hfs,      Is.True);
            src.Iso      = true; Assert.That(dest.Iso,      Is.True);
            src.Jar      = true; Assert.That(dest.Jar,      Is.True);
            src.Lzh      = true; Assert.That(dest.Lzh,      Is.True);
            src.Nupkg    = true; Assert.That(dest.Nupkg,    Is.True);
            src.Rar      = true; Assert.That(dest.Rar,      Is.True);
            src.Rpm      = true; Assert.That(dest.Rpm,      Is.True);
            src.SevenZip = true; Assert.That(dest.SevenZip, Is.True);
            src.Swf      = true; Assert.That(dest.Swf,      Is.True);
            src.Tar      = true; Assert.That(dest.Tar,      Is.True);
            src.Tbz      = true; Assert.That(dest.Tbz,      Is.True);
            src.Tgz      = true; Assert.That(dest.Tgz,      Is.True);
            src.Txz      = true; Assert.That(dest.Txz,      Is.True);
            src.Vhd      = true; Assert.That(dest.Vhd,      Is.True);
            src.Vmdk     = true; Assert.That(dest.Vmdk,     Is.True);
            src.Wim      = true; Assert.That(dest.Wim,      Is.True);
            src.Xar      = true; Assert.That(dest.Xar,      Is.True);
            src.XZ       = true; Assert.That(dest.XZ,       Is.True);
            src.Z        = true; Assert.That(dest.Z,        Is.True);
            src.Zip      = true; Assert.That(dest.Zip,      Is.True);
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
            var vm   = new MainViewModel(m, new SynchronizationContext());
            var src  = vm.Shortcut;
            var dest = m.Value.Shortcut;

            dest.Directory = Results;

            src.Compress       = true;
            src.Extract        = true;
            src.Settings       = true;
            src.CompressOption = PresetMenu.CompressZip;
            Assert.That((uint)dest.Preset, Is.EqualTo(0x107));

            src.Sync();
            Assert.That(src.Compress,       Is.False);
            Assert.That(src.Extract,        Is.False);
            Assert.That(src.Settings,       Is.False);
            Assert.That(src.CompressOption, Is.EqualTo(PresetMenu.CompressZip));
            Assert.That(dest.Preset,        Is.EqualTo(PresetMenu.CompressZip));

            src.Compress  = false;
            src.Extract   = false;
            src.Settings  = false;
            Assert.That(dest.Preset, Is.EqualTo(PresetMenu.CompressZip));
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
        public void SyncUpdate(bool install) => Assert.DoesNotThrow(() =>
        {
            var m0 = Create();
            m0.Value.Shortcut.Directory = Results;

            var vm0 = new MainViewModel(m0, new SynchronizationContext()) { CheckUpdate = true };
            vm0.Associate.Changed = install;
            vm0.Sync();
            vm0.Associate.Clear();
            vm0.Update();

            var m1 = Create();
            m1.Load();
            m1.Value.Shortcut.Directory = Results;

            var vm1 = new MainViewModel(m1, new SynchronizationContext()) { CheckUpdate = false };
            vm1.Update();
        });

        #endregion
    }
}
