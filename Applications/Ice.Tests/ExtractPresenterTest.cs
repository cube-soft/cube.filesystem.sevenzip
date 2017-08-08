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
            var source   = Example(filename);
            var request  = new Request(args.Concat(new[] { source }));
            var settings = new SettingsFolder();
            var events   = new EventAggregator();
            var view     = Views.CreateProgressView();

            // Preset
            settings.Value.Extract = extract;
            settings.Value.Extract.SaveDirectoryName = Result("Others");
            request.DropDirectory = Result(request.DropDirectory);
            MockViewFactory.Destination = Result("Runtime");
            MockViewFactory.Password = password;

            // Main
            using (var ep = new ExtractPresenter(view, request, settings, events))
            {
                view.Show();

                Assert.That(view.Visible, Is.True);
                for (var i = 0; view.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(view.Visible, Is.False, "Timeout");

                Assert.That(view.FileName, Is.EqualTo(filename));
                Assert.That(view.Count,    Is.EqualTo(view.TotalCount));
                Assert.That(view.Value,    Is.EqualTo(100));

                var facade = ep.Model as ExtractFacade;
                Assert.That(IO.Get(facade.Destination).Exists, Is.True);
                var dir = IO.Combine(facade.Destination, facade.OpenDirectoryName);
                Assert.That(IO.Get(dir).Exists, Is.True);

                return view.Count;
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
        /// OneTimeTearDown
        /// 
        /// <summary>
        /// 一度だけ実行される TearDown 処理です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [OneTimeTearDown]
        public void OneTimeTearDown() => MockViewFactory.Reset();

        #endregion
    }
}
