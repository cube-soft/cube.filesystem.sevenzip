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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressTest
    ///
    /// <summary>
    /// Tests to create archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class CompressTest : ArchiveFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Compress
        ///
        /// <summary>
        /// Tests the compression.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Compress(string dest,
            IEnumerable<string> files,
            IEnumerable<string> args,
            CompressValue settings
        ) => Create(files, args, settings, vm => {
            var filename = GetFileName(GetSource(files.First()), dest);

            using (vm.SetPassword("password"))
            using (vm.SetDestination(Get("Runtime", filename)))
            {
                Assert.That(vm.State, Is.EqualTo(TimerState.Stop));
                Assert.That(vm.Logo,  Is.Not.Null);
                Assert.That(vm.Title, Does.StartWith("0%").And.EndsWith("CubeICE"));
                Assert.That(vm.Text,  Does.StartWith("ファイルを圧縮する準備をしています"));
                Assert.That(vm.Style, Is.EqualTo(ProgressBarStyle.Marquee));
                Assert.That(vm.Count, Is.Not.Null.And.Not.Empty);
                Assert.That(vm.CountVisible, Is.False);

                vm.Test();
            }

            Assert.That(IO.Exists(Get(dest)), Is.True, dest);
        });

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets the test cases.
        /// </summary>
        ///
        /// <remarks>
        /// テストケースには、以下の順で指定します。
        /// - 保存パス
        /// - 圧縮ファイル名一覧
        /// - コマンドライン引数一覧
        /// - ユーザ設定用オブジェクト
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    @"Others\Sample.zip",
                    new[] { "Sample.txt" },
                    PresetMenu.Compress.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample 00..01.zip",
                    new[] { "Sample 00..01" },
                    PresetMenu.Compress.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenMethod    = OpenMethod.None,
                        Filtering     = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample.7z",
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressSevenZip.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = false,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample.tar.bz2",
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressBZip2.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample.exe",
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressSfx.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Runtime\Sample.zip",
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressOthers.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Runtime\Sample.7z",
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressSevenZip.ToArguments().Concat(new[] { "/p" }),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Query,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Runtime\Sample.tar.bz2",
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressBZip2.ToArguments().Concat(new[] { "/o:runtime" }),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Drop\Sample.tar.gz",
                    new[] { "Sample.txt", "Sample 00..01" },
                    GetPathArgs(PresetMenu.CompressGZip, "Drop"),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Drop\Sample.tar.xz",
                    new[] { "Sample.txt", "Sample 00..01" },
                    GetPathArgs(PresetMenu.CompressXz, "Drop"),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Mail\Sample.zip",
                    new[] { "Sample.txt", "Sample 00..01" },
                    GetPathArgs(PresetMenu.MailZip, "Mail"),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    }
                );
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// GetPathArgs
        ///
        /// <summary>
        /// Gets the arguments that contain the path argument.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<string> GetPathArgs(PresetMenu menu, string filename) =>
            menu.ToArguments()
                .Concat(new[] { $"/drop:{typeof(CompressTest).GetPath(filename)}" });

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets the filename of save path from the specified paths.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFileName(string src, string dest)
        {
            var fi   = IO.Get(src);
            var name = fi.IsDirectory ? fi.Name : fi.BaseName;
            var ext  = IO.Get(dest).Extension;
            return ext == ".bz2" || ext == ".gz" || ext == ".xz" ?
                   $"{name}.tar{ext}" :
                   $"{name}{ext}";
        }

        #endregion
    }
}
