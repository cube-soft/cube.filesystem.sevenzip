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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cube.FileSystem.SevenZip.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveTest
    ///
    /// <summary>
    /// 圧縮処理 のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveTest : MockViewHelper
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// 圧縮処理のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Archive(string[] files, IEnumerable<string> args,
            ArchiveSettings archive, string dest, long count)
        {
            var filename = GetFileName(Example(files.First()), dest);
            var request  = new Request(args.Concat(files.Select(s => Example(s))));

            Mock.Destination = Result($@"Runtime\{filename}");
            Mock.Password    = "password"; // used by "/p" option

            using (var p = Create(request))
            {
                p.Settings.Value.Archive = archive;
                p.Settings.Value.Archive.SaveDirectoryName = Result("Others");
                p.View.Show();

                Assert.That(p.View.Visible,       Is.True, "Visible");
                Assert.That(Wait(p.View).Result,  Is.True, "Timeout");
                Assert.That(p.Model.Report.Ratio, Is.EqualTo(1.0).Within(0.01), "Ratio");
                Assert.That(p.View.Elapsed,       Is.GreaterThan(TimeSpan.Zero), "Elapsed");
                Assert.That(p.View.FileName,      Is.EqualTo(filename), "FileName");
                Assert.That(p.View.Count,         Is.EqualTo(count), "Count");
                Assert.That(p.View.TotalCount,    Is.EqualTo(count), "TotalCount");
                Assert.That(p.View.Value,         Is.EqualTo(100), "Value");
            }

            Assert.That(IO.Exists(dest), Is.True, dest);
        }

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
            var src    = Example("Sample.txt");
            var exists = Result(@"Exists\Sample.zip");
            var dest   = Result(@"Exists\SampleRuntime.zip");
            var args   = PresetMenu.Archive.ToArguments().Concat(new[] { src });

            Mock.Destination = dest;
            IO.Copy(Example("Single.1.0.0.zip"), exists);

            using (var p = Create(new Request(args)))
            {
                p.Settings.Value.ErrorReport = false;
                p.Settings.Value.Archive.SaveLocation = SaveLocation.Others;
                p.Settings.Value.Archive.SaveDirectoryName = Result("Exists");
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
            var dir  = Result("Overwrite");
            var src  = Example("Sample.txt");
            var args = PresetMenu.Archive.ToArguments().Concat(new[] { src });
            var dest = IO.Combine(dir, "Sample.zip");
            var tmp  = string.Empty;

            Mock.Destination = dest;
            IO.Copy(Example("Single.1.0.0.zip"), dest);

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
            var dir  = Result("PasswordCancel");
            var src  = Example("Sample.txt");
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
            var dir  = Result("MoveFailed");
            var src  = Example("Sample.txt");
            var dest = IO.Combine(dir, "Sample.zip");

            Mock.Destination = dir;
            IO.Copy(Example("Single.1.0.0.zip"), dest, true);

            var args = PresetMenu.ArchiveZip.ToArguments().Concat(new[] { "/o:runtime", src });

            using (var _ = IO.OpenRead(dest))
            using (var p = Create(new Request(args)))
            {
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// 圧縮処理のテスト用データを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// テストケースには、以下の順で指定します。
        /// - 圧縮するファイル名一覧
        /// - コマンドライン引数を表す IEnumerable(string) オブジェクト
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
                    PresetMenu.Archive.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.Open,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample.zip"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample 00..01" },
                    PresetMenu.Archive.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample 00..01.zip"),
                    4L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.ArchiveSevenZip.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.OpenNotDesktop,
                        Filtering     = false,
                    },
                    FullName(@"Others\Sample.7z"),
                    9L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.ArchiveBZip2.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample.tar.bz2"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.ArchiveSfx.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample.exe"),
                    5L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.ArchiveDetail.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        Filtering     = true,
                    },
                    FullName(@"Runtime\Sample.zip"),
                    5L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.ArchiveSevenZip.ToArguments().Concat(new[] { "/p" }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Runtime,
                        Filtering     = true,
                    },
                    FullName(@"Runtime\Sample.7z"),
                    5L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    PresetMenu.ArchiveBZip2.ToArguments().Concat(new[] { "/o:runtime" }),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        Filtering    = true,
                    },
                    FullName(@"Runtime\Sample.tar.bz2"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    DropRequest(PresetMenu.ArchiveGZip, "Drop"),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        Filtering     = true,
                    },
                    FullName(@"Drop\Sample.tar.gz"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    DropRequest(PresetMenu.ArchiveXZ, "Drop"),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        Filtering    = true,
                    },
                    FullName(@"Drop\Sample.tar.xz"),
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Sample 00..01" },
                    DropRequest(PresetMenu.MailZip, "Mail"),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        Filtering    = true,
                    },
                    FullName(@"Mail\Sample.zip"),
                    5L
                );
            }
        }

        #endregion

        #region Helper methods

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
            var io   = new Operator();
            var asm  = Assembly.GetExecutingAssembly().Location;
            var root = io.Get(asm).DirectoryName;
            var dir  = typeof(ArchiveTest).FullName;
            return io.Combine(root, ResultsName, dir, path);
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
        private static IEnumerable<string> DropRequest(PresetMenu menu, string path)
            => menu.ToArguments().Concat(new[] { $"/drop:{FullName(path)}" });

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Presenter オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchivePresenter Create(Request request)
        {
            var v = Views.CreateProgressView();
            var e = new EventHub();
            var s = new SettingsFolder();

            s.Value.Filters = "Filter.txt|FilterDirectory";

            var dest = new ArchivePresenter(v, request, s, e);
            Assert.That(dest.Model.Interval.TotalMilliseconds, Is.EqualTo(100).Within(1));
            dest.Model.Interval = TimeSpan.FromMilliseconds(20);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFileName(string src, string dest)
        {
            var info = IO.Get(src);
            var name = info.IsDirectory ? info.Name : info.NameWithoutExtension;
            var ext  = IO.Get(dest).Extension;
            return ext == ".bz2" || ext == ".gz" || ext == ".xz" ?
                   $"{name}.tar{ext}" :
                   $"{name}{ext}";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetUp
        ///
        /// <summary>
        /// テスト毎に実行される SetUp 処理です。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [SetUp]
        public void SetUp() => Reset();

        #endregion
    }
}
