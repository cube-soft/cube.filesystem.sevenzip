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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cube.FileSystem.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePresenterTest
    /// 
    /// <summary>
    /// ArchivePresenterTest のテスト用クラスです。
    /// </summary>
    ///
    /// <remarks>
    /// Presenter クラスは静的クラス (Views) に対して変更を加えるため、
    /// Parallelizable 属性を指定すると予期せぬエラーが発生する事が
    /// あります。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchivePresenterTest : MockViewHandler
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
        public async Task Archive(string[] files, IEnumerable<string> args,
            ArchiveSettings archive, string dest, long count)
        {
            var filename = GetFileName(files.First(), dest);
            var request = new Request(args.Concat(files.Select(s => Example(s))));
            request.DropDirectory = Result(request.DropDirectory);

            Mock.Destination = Result($@"Runtime\{filename}");
            Mock.Password = "password"; // used by "/p" option

            using (var ap = Create(request))
            {
                ap.Settings.Value.Archive = archive;
                ap.Settings.Value.Archive.SaveDirectoryName = Result("Others");

                Assert.That(ap.Model.ProgressReport.Ratio, Is.EqualTo(0.0));
                ap.View.Show();
                Assert.That(ap.View.Visible, Is.True);
                for (var i = 0; ap.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ap.View.Visible, Is.False, "Timeout");
                Assert.That(ap.Model.ProgressReport.Ratio, Is.EqualTo(1.0).Within(0.01));

                Assert.That(ap.View.FileName,   Is.EqualTo(filename));
                Assert.That(ap.View.Count,      Is.EqualTo(count));
                Assert.That(ap.View.TotalCount, Is.EqualTo(count));
                Assert.That(ap.View.Value,      Is.EqualTo(100));
            }

            Assert.That(IO.Get(Result(dest)).Exists, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Detail
        /// 
        /// <summary>
        /// 実行時設定を反映した圧縮処理のテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Archive_Detail()
        {
            var src  = Result(@"Detail\Sample.txt");
            var dest = Result(@"Detail\Sample.zip");
            var args = PresetMenu.ArchiveDetail.ToArguments().Concat(new[] { src });

            IO.CreateDirectory(Result("Detail"));
            IO.Copy(Example("Sample.txt"), src);

            using (var ap = Create(new Request(args)))
            {
                ap.Settings.Value.Archive.OpenDirectory = OpenDirectoryMethod.None;
                ap.View.Show();

                for (var i = 0; ap.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ap.View.Visible, Is.False, "Timeout");
            }

            Assert.That(IO.Get(dest).Exists, Is.True);
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
        public async Task Archive_Exists()
        {
            var src    = Example("Sample.txt");
            var exists = Result(@"Exists\Sample.zip");
            var dest   = Result(@"Exists\SampleRuntime.zip");
            var args   = PresetMenu.Archive.ToArguments().Concat(new[] { src });

            IO.CreateDirectory(Result("Exists"));
            IO.Copy(Example("Single.zip"), exists);
            Mock.Destination = dest;

            using (var ap = Create(new Request(args)))
            {
                ap.Settings.Value.Archive.OpenDirectory = OpenDirectoryMethod.None;
                ap.Settings.Value.Archive.SaveLocation = SaveLocation.Others;
                ap.Settings.Value.Archive.SaveDirectoryName = Result("Exists");
                ap.View.Show();

                for (var i = 0; ap.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ap.View.Visible, Is.False, "Timeout");
            }

            Assert.That(IO.Get(dest).Exists, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_OpenDirectory
        /// 
        /// <summary>
        /// 圧縮後にディレクトリを開くテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Archive_OpenDirectory()
        {
            var args = PresetMenu.Archive.ToArguments().Concat(new[] { Example("Sample.txt") });

            using (var ap = Create(new Request(args)))
            {
                ap.Settings.Value.Explorer = "dummy.exe";
                ap.Settings.Value.Archive.OpenDirectory = OpenDirectoryMethod.OpenNotDesktop;
                ap.Settings.Value.Archive.SaveLocation = SaveLocation.Others;
                ap.Settings.Value.Archive.SaveDirectoryName = Result("OpenDirectory");
                ap.View.Show();

                for (var i = 0; ap.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ap.View.Visible, Is.False, "Timeout");
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
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    @"Others\Sample.zip",
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveSevenZip.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = false,
                    },
                    @"Others\Sample.7z",
                    8L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveBZip2.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    @"Others\Sample.tar.bz2",
                    1L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveSfx.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    @"Others\Sample.exe",
                    4L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveZipPassword.ToArguments().Concat(new[] { "/o:runtime" }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    @"Runtime\Sample.zip",
                    4L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveSevenZip.ToArguments().Concat(new[] { "/p" }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Runtime,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    @"Runtime\Sample.7z",
                    4L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveGZip.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        "/drop:Drop",
                    }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                    },
                    @"Drop\Sample.tar.gz",
                    1L
                );
            }
        }

        #endregion

        #region Helper

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
            var e = new EventAggregator();
            var s = new SettingsFolder();

            s.Value.Filters = "Filter.txt|FilterDirectory";

            return new ArchivePresenter(v, request, s, e);
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
            var name = IO.Get(src).NameWithoutExtension;
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
