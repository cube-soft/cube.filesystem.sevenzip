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
using System.Threading;
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextMenuViewModelTest
    ///
    /// <summary>
    /// Tests the ContextMenuViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ContextMenuViewModelTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Preset
        ///
        /// <summary>
        /// Tests the Preset menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Preset()
        {
            static void Set(ContextViewModel cs, bool enabled)
            {
                cs.Compress            = enabled;
                cs.CompressBZip2       = enabled;
                cs.CompressOthers      = enabled;
                cs.CompressGZip        = enabled;
                cs.CompressXZ          = enabled;
                cs.CompressSevenZip    = enabled;
                cs.CompressSfx         = enabled;
                cs.CompressZip         = enabled;
                cs.CompressZipPassword = enabled;
                cs.Extract             = enabled;
                cs.ExtractDesktop      = enabled;
                cs.ExtractMyDocuments  = enabled;
                cs.ExtractRuntime      = enabled;
                cs.ExtractSource       = enabled;
            }

            using var m  = NewSettings();
            using var vm = NewVM(m);

            var src  = vm.Menu;
            var dest = m.Value.Context;

            Set(src, true);
            Assert.That(src.PresetEnabled, Is.True);
            Assert.That((uint)dest.Preset, Is.EqualTo(0x000fff3));

            Set(src, false);
            Assert.That(src.PresetEnabled, Is.True);
            Assert.That(dest.Preset, Is.EqualTo(Ice.Preset.None));

            src.Reset();
            Assert.That(src.Compress,            Is.True);
            Assert.That(src.CompressBZip2,       Is.True);
            Assert.That(src.CompressOthers,      Is.True);
            Assert.That(src.CompressGZip,        Is.True);
            Assert.That(src.CompressXZ,          Is.False);
            Assert.That(src.CompressSevenZip,    Is.True);
            Assert.That(src.CompressSfx,         Is.True);
            Assert.That(src.CompressZip,         Is.True);
            Assert.That(src.CompressZipPassword, Is.True);
            Assert.That(src.Extract,             Is.True);
            Assert.That(src.ExtractDesktop,      Is.True);
            Assert.That(src.ExtractMyDocuments,  Is.True);
            Assert.That(src.ExtractRuntime,      Is.True);
            Assert.That(src.ExtractSource,       Is.True);
            Assert.That(dest.Preset, Is.EqualTo(Ice.Preset.DefaultContext));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize
        ///
        /// <summary>
        /// Tests the Customize method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        [Ignore("TODO:Under migration")]
        public void Customize()
        {
            var dest = InvokeCustomize();
            Assert.That(dest.UseCustom, Is.True);

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

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// InvokeCustomize
        ///
        /// <summary>
        /// Invokes the customize operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ContextSetting InvokeCustomize()
        {
            var dest = NewSettings();
            using (var vm = NewVM(dest)) vm.Customize();
            return dest.Value.Context;
        }

        #endregion
    }
}
