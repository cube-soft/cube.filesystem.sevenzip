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
using Cube.FileSystem.SevenZip.Ice;
using Cube.Tests;
using NUnit.Framework;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressTest
    ///
    /// <summary>
    /// Tests the CompressViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class CompressViewModelTest : FileFixture
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Exists
        ///
        /// <summary>
        /// 保存パスに指定されたファイルが既に存在する場合の挙動を確認
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_Exists()
        {
            var src = GetSource("Sample.txt");
            var exists = Get("Exists", "Sample.zip");
            var dest = Get("Exists", "SampleRuntime.zip");
            var args = PresetMenu.Archive.ToArguments().Concat(new[] { src });

            Mock.Destination = dest;
            IO.Copy(GetSource("Single.1.0.0.zip"), exists);

            using (var p = Create(new Request(args)))
            {
                p.Settings.Value.ErrorReport = false;
                p.Settings.Value.Archive.SaveLocation = SaveLocation.Others;
                p.Settings.Value.Archive.SaveDirectoryName = Get("Exists");
                p.View.Show();

                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }

            Assert.That(IO.Exists(dest), Is.True, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Overwrite
        ///
        /// <summary>
        /// 保存パスに指定されたファイルが既に存在する場合の挙動を確認
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_Overwrite()
        {
            var src = GetSource("Sample.txt");
            var dest = Get("Overwrite", "Sample.zip");
            var args = PresetMenu.Archive.ToArguments().Concat(new[] { src });
            var tmp = string.Empty;

            Mock.Destination = dest;
            IO.Copy(GetSource("Single.1.0.0.zip"), dest);

            using (var p = Create(new Request(args)))
            {
                p.Settings.Value.Archive.SaveLocation = SaveLocation.Runtime;
                p.View.Show();

                Assert.That(Wait(p.View).Result, Is.True, "Timeout");

                tmp = p.Model.Tmp;
            }

            Assert.That(tmp, Is.Not.Null.And.Not.Empty);
            Assert.That(IO.Exists(tmp), Is.False, tmp);
            Assert.That(IO.Exists(dest), Is.True, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_PasswordCancel
        ///
        /// <summary>
        /// パスワードの設定をキャンセルするテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_PasswordCancel()
        {
            var dir = Get("PasswordCancel");
            var src = GetSource("Sample.txt");
            var dest = IO.Combine(dir, "Sample.zip");
            var args = PresetMenu.ArchiveZipPassword.ToArguments().Concat(new[] { src });

            using (var p = Create(new Request(args)))
            {
                p.Settings.Value.Archive.SaveLocation = SaveLocation.Others;
                p.Settings.Value.Archive.SaveDirectoryName = dir;
                p.View.Show();

                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }

            Assert.That(IO.Exists(dest), Is.False, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_MoveFailed
        ///
        /// <summary>
        /// ファイルの移動に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_MoveFailed()
        {
            var dir = Get("MoveFailed");
            var src = GetSource("Sample.txt");
            var dest = IO.Combine(dir, "Sample.zip");

            Mock.Destination = dir;
            IO.Copy(GetSource("Single.1.0.0.zip"), dest, true);

            var args = PresetMenu.ArchiveZip.ToArguments().Concat(new[] { "/o:runtime", src });

            using (var _ = IO.OpenRead(dest))
            using (var p = Create(new Request(args)))
            {
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }
        }
    }
}
