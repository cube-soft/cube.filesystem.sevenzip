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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cube.FileSystem.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractPresenterTest
    /// 
    /// <summary>
    /// ExtractPresenter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractPresenterTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 展開処理のテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public async Task Extract(string filename, string password,
            IEnumerable<string> args, ExtractSettings extract, string check, long count)
        {
            var src = Example(filename);
            var request = new Request(args.Concat(new[] { src }));
            request.DropDirectory = Result(request.DropDirectory);

            MockViewFactory.Destination = Result("Runtime");
            MockViewFactory.Password = password;

            using (var ep = Create(request))
            {
                ep.Settings.Value.Extract = extract;
                ep.Settings.Value.Extract.SaveDirectoryName = Result("Others");

                Assert.That(ep.Model.ProgressReport.Ratio, Is.EqualTo(0.0));
                ep.View.Show();
                Assert.That(ep.View.Visible, Is.True);
                for (var i = 0; ep.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
                Assert.That(ep.Model.ProgressReport.Ratio, Is.EqualTo(1.0).Within(0.01));

                Assert.That(ep.View.FileName,   Is.EqualTo(filename));
                Assert.That(ep.View.Count,      Is.EqualTo(count));
                Assert.That(ep.View.TotalCount, Is.EqualTo(count));
                Assert.That(ep.View.Value,      Is.EqualTo(100));
            }

            Assert.That(IO.Get(Result(check)).Exists, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Overwrite
        /// 
        /// <summary>
        /// 展開したファイルの上書きテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_Overwrite()
        {
            var src  = Example("Complex.zip");
            var dest = Result("Overwrite");

            using (var reader = new SevenZip.ArchiveReader(src)) reader.Extract(dest);
            using (var ep = Create(src, dest))
            {
                ep.Settings.Value.Extract.RootDirectory = CreateDirectoryMethod.None;
                ep.View.Show();
                for (var i = 0; ep.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_UserCancel
        /// 
        /// <summary>
        /// パスワード入力をキャンセルした時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_UserCancel()
        {
            var src  = Example("Password.7z");
            var dest = Result("UserCancel");

            using (var ep = Create(src, dest))
            {
                ep.View.Show();
                for (var i = 0; ep.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_OpenDirectory
        /// 
        /// <summary>
        /// 展開後にディレクトリを開くテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_OpenDirectory()
        {
            var src  = Example("Sample.zip");
            var dest = Result("OpenDirectory");

            using (var ep = Create(src, dest))
            {
                ep.Settings.Value.Explorer = "dummy.exe";
                ep.Settings.Value.Extract.OpenDirectory = OpenDirectoryMethod.OpenNotDesktop;
                ep.View.Show();

                for (var i = 0; ep.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        /// 
        /// <summary>
        /// 展開処理のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"Others\Complex",
                    5L
                );

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.ExtractRuntime.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"Runtime\Complex",
                    5L
                );

                yield return new TestCaseData("SampleFilter.zip", "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                        DeleteSource  = false,
                    },
                    @"Others\フィルタリング テスト用",
                    9L
                );

                yield return new TestCaseData("SampleMac.zip", "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = true,
                        DeleteSource  = false,
                    },
                    @"Others\名称未設定フォルダ",
                    19L
                );

                yield return new TestCaseData("Password.7z", "password",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"Others\Password",
                    3L
                );

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Single-0x00\Sample.txt",
                    1L
                );

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Single-0x01\Single",
                    1L
                );

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Single-0x03\Single",
                    1L
                );

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Single-0x05\Sample.txt",
                    1L
                );

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource = false,
                    },
                    @"RootDirectory\Single-0x07\Sample.txt",
                    1L
                );

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\SingleDirectory-0x00\Sample",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\SingleDirectory-0x01\SingleDirectory",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\SingleDirectory-0x03\Sample",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\SingleDirectory-0x05\SingleDirectory",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\SingleDirectory-0x07\Sample",
                    4L
                );

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\MultiDirectory-0x00\Directory",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\MultiDirectory-0x01\MultiDirectory",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\MultiDirectory-0x03\MultiDirectory",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\MultiDirectory-0x05\MultiDirectory",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\MultiDirectory-0x07\MultiDirectory",
                    7L
                );

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Complex-0x00\Foo.txt",
                    5L
                );

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Complex-0x01\Complex",
                    5L
                );

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Complex-0x03\Complex",
                    5L
                );

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Complex-0x05\Complex",
                    5L
                );

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryMethod.None,
                        DeleteSource  = false,
                    },
                    @"RootDirectory\Complex-0x07\Complex",
                    5L
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
        private ExtractPresenter Create(string src, string dest)
        {
            var p = Create(new Request(new[] { "/x", src }));

            p.Settings.Value.Extract.SaveLocation      = SaveLocation.Others;
            p.Settings.Value.Extract.SaveDirectoryName = dest;
            p.Settings.Value.Extract.OpenDirectory     = OpenDirectoryMethod.None;

            return p;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// Presenter オブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private ExtractPresenter Create(Request request)
        {
            var v = Views.CreateProgressView();
            var e = new EventAggregator();
            var s = new SettingsFolder();

            return new ExtractPresenter(v, request, s, e);
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
        /// テスト後に毎回実行される TearDown 処理です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TearDown]
        public void TearDown() => MockViewFactory.Reset();

        #endregion
    }
}
