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
    /// <remarks>
    /// Presenter クラスは静的クラス (Views) に対して変更を加えるため、
    /// Parallelizable 属性を指定すると予期せぬエラーが発生する事が
    /// あります。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractPresenterTest : MockViewHelper
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

            Mock.Destination = Result("Runtime");
            Mock.Password = password;

            var tmp = string.Empty;

            using (var ep = Create(request))
            {
                ep.Settings.Value.Explorer = "dummy.exe";
                ep.Settings.Value.Extract = extract;
                ep.Settings.Value.Extract.SaveDirectoryName = Result("Others");

                Assert.That(ep.Model.Report.Ratio, Is.EqualTo(0.0));
                ep.View.Show();
                Assert.That(ep.View.Visible, Is.True);
                await Wait(ep.View);
                Assert.That(ep.View.Visible, Is.False, "Timeout");

                Assert.That(ep.View.Elapsed,       Is.GreaterThan(TimeSpan.Zero));
                Assert.That(ep.View.FileName,      Is.EqualTo(filename));
                Assert.That(ep.View.Count,         Is.EqualTo(count));
                Assert.That(ep.View.TotalCount,    Is.EqualTo(count));
                Assert.That(ep.View.Value,         Is.EqualTo(100));
                Assert.That(ep.Model.Report.Ratio, Is.EqualTo(1.0).Within(0.01));

                tmp = ep.Model.Tmp;
                Assert.That(tmp, Is.Not.Null.And.Not.Empty);
            }

            Assert.That(IO.Exists(Result(check)), Is.True);
            Assert.That(IO.Exists(tmp), Is.False);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Rename
        /// 
        /// <summary>
        /// OverwriteMode (Rename) の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_Rename()
        {
            var dummy = Example("Sample.txt");
            var src   = Example("Complex.1.0.0.zip");
            var dest  = Result("Overwrite");

            IO.Copy(dummy, Result(@"Overwrite\Foo.txt"));
            IO.Copy(dummy, Result(@"Overwrite\Directory\Empty.txt"));

            using (var ep = Create(src, dest))
            {
                ep.Settings.Value.Extract.RootDirectory = CreateDirectoryMethod.None;
                ep.View.Show();
                await Wait(ep.View);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }

            Assert.That(IO.Exists(Result(@"Overwrite\Foo(2).txt")), Is.True);
            Assert.That(IO.Exists(Result(@"Overwrite\Directory\Empty(2).txt")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Cancel
        /// 
        /// <summary>
        /// 処理をキャンセルするテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_Cancel()
        {
            var src = Example("Complex.zip");
            var dest = Result("UserCancel");

            using (var ep = Create(src, dest))
            {
                ep.View.Show();
                ep.EventHub.GetEvents().Cancel.Publish();
                await Wait(ep.View);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }

            Assert.Pass();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Suspend
        /// 
        /// <summary>
        /// 処理を一時停止するテストを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// Suspend イベント発行後、実際に展開が一時停止されるまでに
        /// タイムラグが発生する事があるため、チェックする値に多少の
        /// 幅を持たせています。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_Suspend()
        {
            var src  = Example("Complex.1.0.0.zip");
            var dest = Result("Suspend");

            using (var ep = Create(src, dest))
            {
                ep.Model.Interval = TimeSpan.FromMilliseconds(50);
                ep.View.Show();

                ep.EventHub.GetEvents().Suspend.Publish(true);
                var count = ep.View.Value;
                await Task.Delay(150);
                Assert.That(ep.View.Value, Is.EqualTo(count).Within(10)); // see remarks
                ep.EventHub.GetEvents().Suspend.Publish(false);

                await Wait(ep.View);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }

            Assert.That(IO.Exists(Result(@"Suspend\Complex.1.0.0")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_DeleteSource
        /// 
        /// <summary>
        /// 展開後に元の圧縮ファイルを削除するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_DeleteSource()
        {
            var src  = Result("Complex.zip");
            var dest = Result("DeleteSource");

            IO.Copy(Example("Complex.1.0.0.zip"), src);

            using (var ep = Create(src, dest))
            {
                ep.Settings.Value.Extract.DeleteSource = true;
                ep.View.Show();
                await Wait(ep.View);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }

            Assert.That(IO.Exists(Result(@"DeleteSource\Complex")), Is.True);
            Assert.That(IO.Exists(src), Is.False);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_PasswordCancel
        /// 
        /// <summary>
        /// パスワード入力をキャンセルした時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_PasswordCancel()
        {
            var src  = Example("Password.7z");
            var dest = Result("PasswordCancel");

            using (var ep = Create(src, dest))
            {
                ep.View.Show();
                await Wait(ep.View);
                Assert.That(ep.View.Visible, Is.False, "Timeout");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_ErrorReport
        /// 
        /// <summary>
        /// エラーレポートの表示テストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public async Task Extract_ErrorReport()
        {
            var src  = Example("Sample.txt");
            var dest = Result("ErrorReport");

            using (var ep = Create(src, dest))
            {
                ep.Settings.Value.ErrorReport = true;
                ep.View.Show();
                await Wait(ep.View);
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
                yield return new TestCaseData("Complex.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = false,
                    },
                    @"Others\Complex.1.0.0",
                    5L
                );

                yield return new TestCaseData("Complex.1.0.0.zip", "",
                    PresetMenu.ExtractRuntime.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.Open,
                        Filtering     = false,
                    },
                    @"Runtime\Complex.1.0.0",
                    5L
                );

                yield return new TestCaseData("SampleFilter.zip", "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.OpenNotDesktop,
                        Filtering     = true,
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
                        Filtering     = true,
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
                    },
                    @"Others\Password",
                    3L
                );

                yield return new TestCaseData("Single.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    @"RootDirectory\Single-0x00\Sample.txt",
                    1L
                );

                yield return new TestCaseData("Single.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    @"RootDirectory\Single-0x01\Single.1.0",
                    1L
                );

                yield return new TestCaseData("Single.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"RootDirectory\Single-0x03\Single.1.0",
                    1L
                );

                yield return new TestCaseData("Single.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    @"RootDirectory\Single-0x05\Sample.txt",
                    1L
                );

                yield return new TestCaseData("Single.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\Single-0x07",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                    },
                    @"RootDirectory\Single-0x07\Sample.txt",
                    1L
                );

                yield return new TestCaseData("SingleDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    @"RootDirectory\SingleDirectory-0x00\Sample",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    @"RootDirectory\SingleDirectory-0x01\SingleDirectory.1.0.0",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"RootDirectory\SingleDirectory-0x03\Sample",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\SingleDirectory-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    @"RootDirectory\SingleDirectory-0x05\SingleDirectory.1.0.0",
                    4L
                );

                yield return new TestCaseData("SingleDirectory.1.0.0.zip", "",
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
                    },
                    @"RootDirectory\SingleDirectory-0x07\Sample",
                    4L
                );

                yield return new TestCaseData("MultiDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    @"RootDirectory\MultiDirectory-0x00\Directory",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    @"RootDirectory\MultiDirectory-0x01\MultiDirectory.1.0.0",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"RootDirectory\MultiDirectory-0x03\MultiDirectory.1.0.0",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[] {
                        "/o:source",
                        @"/drop:RootDirectory\MultiDirectory-0x05",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    @"RootDirectory\MultiDirectory-0x05\MultiDirectory.1.0.0",
                    7L
                );

                yield return new TestCaseData("MultiDirectory.1.0.0.zip", "",
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
                    },
                    @"RootDirectory\MultiDirectory-0x07\MultiDirectory.1.0.0",
                    7L
                );

                yield return new TestCaseData("Complex.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x00",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    @"RootDirectory\Complex-0x00\Foo.txt",
                    5L
                );

                yield return new TestCaseData("Complex.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x01",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    @"RootDirectory\Complex-0x01\Complex.1.0.0",
                    5L
                );

                yield return new TestCaseData("Complex.1.0.0.zip", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:RootDirectory\Complex-0x03",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"RootDirectory\Complex-0x03\Complex.1.0.0",
                    5L
                );

                yield return new TestCaseData("Complex.1.0.0.zip", "",
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
                    },
                    @"RootDirectory\Complex-0x05\Complex.1.0.0",
                    5L
                );

                yield return new TestCaseData("Complex.1.0.0.zip", "",
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
                    },
                    @"RootDirectory\Complex-0x07\Complex.1.0.0",
                    5L
                );

                yield return new TestCaseData("Sample.tar", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:Tar",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"Tar\TarSample",
                    4L
                );

                yield return new TestCaseData("Sample.tbz", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:Tar\BZipSample",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"Tar\BZipSample\TarSample",
                    4L
                );

                yield return new TestCaseData("Sample.tgz", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:Tar\GZipSample",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"Tar\GZipSample\TarSample",
                    4L
                );

                yield return new TestCaseData("Sample.tar.lzma", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:Tar\LzmaSample",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"Tar\LzmaSample\Sample",
                    5L
                );

                yield return new TestCaseData("Sample.tar.z", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:Tar\LzwSample",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"Tar\LzwSample\Sample",
                    5L
                );

                yield return new TestCaseData("Sample.txt.bz2", "",
                    PresetMenu.Extract.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        @"/drop:Bz2Sample",
                    }),
                    new ExtractSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    @"Bz2Sample\Sample\Sample.txt",
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
            var e = new EventHub();
            var s = new SettingsFolder();

            var dest = new ExtractPresenter(v, request, s, e);
            Assert.That(dest.Model.Interval.TotalMilliseconds, Is.EqualTo(100).Within(1));
            dest.Model.Interval = TimeSpan.FromMilliseconds(20);
            return dest;
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
