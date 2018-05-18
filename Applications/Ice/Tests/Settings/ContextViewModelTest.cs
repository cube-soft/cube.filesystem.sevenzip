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
    class ContextViewModelTest : SettingsMockViewHelper
    {
        #region Tests

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
            var src  = vm.Context;
            var dest = m.Value.Context;

            Set(src, true);
            Assert.That((uint)dest.Preset, Is.EqualTo(0x000fff3));

            Set(src, false);
            Assert.That(dest.Preset, Is.EqualTo(PresetMenu.None));

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
    }
}
