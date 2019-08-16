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
using Cube.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
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
    class CompressTest : FileFixture
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
        public void Compress(IEnumerable<string> files,
            IEnumerable<string> args,
            CompressValue settings,
            string dest,
            long count
        ) => Create(files, args, settings, vm => {

            var filename = GetFileName(GetSource(files.First()), dest);

            using (vm.SetPassword("password"))
            using (vm.SetDestination(Get("Runtime", filename)))
            {
                Assert.That(vm.Busy,  Is.False);
                Assert.That(vm.Logo,  Is.Not.Null);
                Assert.That(vm.Title, Does.StartWith("0%").And.EndsWith("CubeICE"));
                Assert.That(vm.Text,  Does.StartWith("ファイルを圧縮する準備をしています"));
                Assert.That(vm.Style, Is.EqualTo(ProgressBarStyle.Marquee));
                Assert.That(vm.Count, Is.Not.Null.And.Not.Empty);
                Assert.That(vm.CountVisible, Is.False);

                var token = vm.GetToken();
                vm.Start();
                Assert.That(Wait.For(token), "Timeout");
                Assert.That(vm.Count, Does.EndWith(count.ToString("#,0")));
            }

            Assert.That(IO.Exists(dest), Is.True, dest);
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
        /// - 圧縮するファイル名一覧
        /// - コマンドライン引数一覧
        /// - ユーザ設定用オブジェクト
        /// - 圧縮ファイルのパス
        /// - 圧縮するファイル + ディレクトリ数
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    new[] { "Sample.txt" },
                    PresetMenu.Compress.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Others\Sample.zip"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample 00..01" },
                    PresetMenu.Compress.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenMethod    = OpenMethod.None,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample 00..01.zip"),
                    4L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressSevenZip.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = false,
                    },
                    FullName(@"Others\Sample.7z"),
                    9L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressBZip2.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Others\Sample.tar.bz2"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressSfx.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Others\Sample.exe"),
                    5L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressOthers.ToArguments(),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Runtime\Sample.zip"),
                    5L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressSevenZip.ToArguments().Concat(new[] { "/p" }),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Query,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Runtime\Sample.7z"),
                    5L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.CompressBZip2.ToArguments().Concat(new[] { "/o:runtime" }),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Runtime\Sample.tar.bz2"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    DropRequest(PresetMenu.CompressGZip, "Drop"),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Drop\Sample.tar.gz"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    DropRequest(PresetMenu.CompressXz, "Drop"),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Drop\Sample.tar.xz"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    DropRequest(PresetMenu.MailZip, "Mail"),
                    new CompressValue
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenMethod   = OpenMethod.None,
                        Filtering    = true,
                    },
                    FullName(@"Mail\Sample.zip"),
                    5L
                );
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the CompressViewModel class with
        /// the specified arguments and invokes the specified callback.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Create(IEnumerable<string> files,
            IEnumerable<string> args,
            CompressValue settings,
            Action<CompressViewModel> callback
        ) {
            var context = new SynchronizationContext();
            var request = new Request(args.Concat(files.Select(e => GetSource(e))));
            var folder  = new SettingFolder(GetType().Assembly, IO);

            folder.Value.Compress = settings;
            folder.Value.Filters = "Filter.txt|FilterDirectory";

            using (var vm = new CompressViewModel(request, folder, context)) callback(vm);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        ///
        /// <summary>
        /// 結果を保存するディレクトリへの絶対パスに変換します。
        /// </summary>
        ///
        /// <remarks>
        /// MockViewHelper.Result と同じ内容を返す静的メソッドです。
        /// TestCase は静的に定義する必要があるためこちらを使用しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static string FullName(string path)
        {
            var io   = new IO();
            var asm  = Assembly.GetExecutingAssembly().Location;
            var root = io.Get(asm).DirectoryName;
            var dir  = typeof(CompressTest).FullName;
            return io.Combine(root, "Results", dir, path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DropRequest
        ///
        /// <summary>
        /// ドロップ先のパスを指定した Request オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<string> DropRequest(PresetMenu menu, string path) =>
            menu.ToArguments().Concat(new[] { $"/drop:{FullName(path)}" });

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets the filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFileName(string src, string dest)
        {
            var info = IO.Get(src);
            var name = info.IsDirectory ? info.Name : info.BaseName;
            var ext  = IO.Get(dest).Extension;
            return ext == ".bz2" || ext == ".gz" || ext == ".xz" ?
                   $"{name}.tar{ext}" :
                   $"{name}{ext}";
        }

        #endregion
    }
}
