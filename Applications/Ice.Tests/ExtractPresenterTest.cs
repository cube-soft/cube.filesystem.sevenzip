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
        public async Task<long> Extract(string filename, string password,
            IEnumerable<string> args, ExtractSettings extract)
        {
            var src = Example(filename);
            var request = new Request(args.Concat(new[] { src }));
            request.DropDirectory = Result(request.DropDirectory);

            // Preset
            MockViewFactory.Destination = Result("Runtime");
            MockViewFactory.Password = password;

            using (var ep = Create(request))
            {
                ep.Settings.Value.Extract = extract;
                ep.Settings.Value.Extract.SaveDirectoryName = Result("Others");
                ep.View.Show();

                Assert.That(ep.View.Visible, Is.True);
                for (var i = 0; ep.View.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(ep.View.Visible, Is.False, "Timeout");

                Assert.That(ep.View.FileName, Is.EqualTo(filename));
                Assert.That(ep.View.Count,    Is.EqualTo(ep.View.TotalCount));
                Assert.That(ep.View.Value,    Is.EqualTo(100));

                var facade = ep.Model as ExtractFacade;
                Assert.That(IO.Get(facade.Destination).Exists, Is.True);
                var dir = IO.Combine(facade.Destination, facade.OpenDirectoryName);
                Assert.That(IO.Get(dir).Exists, Is.True);

                return ep.View.Count;
            }
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
            try
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
            catch (SevenZip.UserCancelException /* err */) { Assert.Pass(); }
            catch (Exception err) { Assert.Fail(err.ToString()); }
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
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.ExtractRuntime.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);

                yield return new TestCaseData("Password.7z", "password",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(3L);

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.None,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(1L);

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(1L);

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(1L);

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(1L);

                yield return new TestCaseData("Single.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile |
                                        RootDirectoryCondition.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource = false,
                    }
                ).Returns(1L);

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.None,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData("SingleDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile |
                                        RootDirectoryCondition.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.None,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(7L);

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(7L);

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(7L);

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(7L);

                yield return new TestCaseData("MultiDirectory.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile |
                                        RootDirectoryCondition.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(7L);

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.None,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.CreateSmart,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);

                yield return new TestCaseData("Complex.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = RootDirectoryCondition.Create |
                                        RootDirectoryCondition.SkipSingleFile |
                                        RootDirectoryCondition.SkipSingleDirectory,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteSource  = false,
                    }
                ).Returns(5L);
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
            var r = new Request(new[] { "/x", src });
            var s = new SettingsFolder();

            s.Value.Extract.SaveLocation      = SaveLocation.Others;
            s.Value.Extract.SaveDirectoryName = dest;
            s.Value.Extract.OpenDirectory     = OpenDirectoryCondition.None;

            return new ExtractPresenter(Views.CreateProgressView(), r, s, new EventAggregator());
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
            => new ExtractPresenter(
                Views.CreateProgressView(),
                request,
                new SettingsFolder(),
                new EventAggregator()
            );

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
