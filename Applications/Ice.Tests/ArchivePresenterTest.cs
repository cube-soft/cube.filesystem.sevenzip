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
using Cube.FileSystem.SevenZip;
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
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchivePresenterTest : FileResource
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

            MockViewFactory.Destination = Result($@"Runtime\{filename}");
            MockViewFactory.Password = "password"; // used by "/p" option

            using (var ap = Create(request))
            {
                ap.Settings.Value.Archive = archive;
                ap.Settings.Value.Archive.SaveDirectoryName = Result("Settings");
                ap.View.Show();

                Assert.That(ap.View.Visible, Is.True);
                for (var i = 0; ap.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ap.View.Visible, Is.False, "Timeout");

                Assert.That(ap.View.Count,      Is.EqualTo(count));
                Assert.That(ap.View.TotalCount, Is.EqualTo(count));
                Assert.That(ap.View.Value,      Is.EqualTo(100));
                Assert.That(ap.View.FileName,   Is.EqualTo(filename));
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
                ap.Settings.Value.Archive.DeleteOnMail = false;
                ap.View.Show();

                for (var i = 0; ap.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ap.View.Visible, Is.False, "Timeout");
            }

            Assert.That(IO.Get(Result(dest)).Exists, Is.True);
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
                        DeleteOnMail  = false,
                    },
                    @"Settings\Sample.zip",
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
                        DeleteOnMail  = false,
                    },
                    @"Settings\Sample.7z",
                    9L
                );

                yield return new TestCaseData(
                    new[] { "Sample.txt", "Archive" },
                    PresetMenu.ArchiveBZip2.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                        DeleteOnMail  = false,
                    },
                    @"Settings\Sample.tar.bz2",
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
                        DeleteOnMail  = false,
                    },
                    @"Settings\Sample.exe",
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
                        DeleteOnMail  = false,
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
                        DeleteOnMail  = false,
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
                        DeleteOnMail  = false,
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
            => new ArchivePresenter(
                Views.CreateProgressView(),
                request,
                new SettingsFolder(),
                new EventAggregator()
            );

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
        /// OneTimeSetUp
        /// 
        /// <summary>
        /// 一度だけ実行される SetUp 処理です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [OneTimeSetUp]
        public void OneTimeSetUp() => MockViewFactory.Configure();

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
        public void TearDown() => MockViewFactory.Reset();

        #endregion
    }
}
