﻿/* ------------------------------------------------------------------------- */
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
using Cube.Forms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractTest
    ///
    /// <summary>
    /// 展開処理のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractTest : ProgressViewModelFixture
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
        public void Extract(string filename, string password,
            IEnumerable<string> args, ExtractSettings settings, string exists, long count)
        {
            var request = new Request(args.Concat(new[] { GetSource(filename) }));
            var tmp     = string.Empty;

            Mock.Destination = Get("Runtime");
            Mock.Password    = password;

            using (var p = Create(request))
            {
                p.Settings.Value.Explorer = "dummy.exe";
                p.Settings.Value.Extract = settings;
                p.Settings.Value.Extract.SaveDirectoryName = Get("Others");
                p.View.Show();

                Assert.That(p.View.Visible,       Is.True, "Visible");
                Assert.That(Wait(p.View).Result,  Is.True, "Timeout");
                Assert.That(p.View.Elapsed,       Is.GreaterThan(TimeSpan.Zero), "Elapsed");
                Assert.That(p.View.FileName,      Is.EqualTo(filename), "FileName");
                Assert.That(p.View.Count,         Is.EqualTo(count), "Count");
                Assert.That(p.View.TotalCount,    Is.EqualTo(count), "TotalCount");
                Assert.That(p.View.Value,         Is.EqualTo(100), "Value");
                Assert.That(p.Model.Report.Ratio, Is.EqualTo(1.0).Within(0.01), "Ratio");

                tmp = p.Model.Tmp;
            }

            Assert.That(tmp, Is.Not.Null.And.Not.Empty);
            Assert.That(IO.Exists(tmp), Is.False, tmp);
            Assert.That(IO.Exists(exists), Is.True, exists);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Multiple
        ///
        /// <summary>
        /// OverwriteMode (Rename) の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Multiple()
        {
            var dest = Get("Multiple");
            var src  = new[]
            {
                GetSource("Complex.1.0.0.zip"),
                GetSource("Single.1.0.0.zip"),
            };

            using (var p = Create(dest, src))
            {
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }

            var i0 = IO.Get(IO.Combine(dest, @"Complex.1.0.0\Foo.txt"));
            var i1 = IO.Get(IO.Combine(dest, @"Single.1.0.0\Sample 00..01.txt"));

            Assert.That(i0.Length, Is.AtLeast(1));
            Assert.That(i1.Length, Is.AtLeast(1));
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
        public void Extract_Rename()
        {
            Mock.OverwriteMode = OverwriteMode.Rename;

            var dummy = GetSource("Sample.txt");
            var src   = GetSource("Complex.1.0.0.zip");
            var dest  = Get("Rename");

            IO.Copy(dummy, IO.Combine(dest, @"Foo.txt"));
            IO.Copy(dummy, IO.Combine(dest, @"Directory\Empty.txt"));

            using (var p = Create(dest, src))
            {
                p.Settings.Value.Extract.RootDirectory = CreateDirectoryMethod.None;
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"Foo (1).txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"Directory\Empty (1).txt")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Overwrite_Cancel
        ///
        /// <summary>
        /// 上書き確認をキャンセルした時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Overwrite_Cancel()
        {
            Mock.OverwriteMode = OverwriteMode.Cancel;

            var dummy = GetSource("Sample.txt");
            var size  = IO.Get(dummy).Length;
            var src   = GetSource("Complex.1.0.0.zip");
            var dest  = Get("OverwriteCancel");
            var tmp   = string.Empty;

            IO.Copy(dummy, IO.Combine(dest, "Foo.txt"));

            using (var p = Create(dest, src))
            {
                p.Settings.Value.Extract.RootDirectory = CreateDirectoryMethod.None;
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");

                tmp = p.Model.Tmp;
                Assert.That(tmp, Is.Not.Null.And.Not.Empty);
                Assert.That(IO.Exists(tmp), Is.True);
                Assert.That(IO.Exists(IO.Combine(tmp, "Foo.txt")), Is.True);
            }

            Assert.That(IO.Get(IO.Combine(dest, "Foo.txt")).Length, Is.EqualTo(size));
            Assert.That(IO.Exists(tmp), Is.False);
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
        public void Extract_DeleteSource()
        {
            var src    = Get("Complex.zip");
            var dest   = Get("DeleteSource");
            var exists = Get("DeleteSource", "Complex");

            IO.Copy(GetSource("Complex.1.0.0.zip"), src);

            using (var p = Create(dest, src))
            {
                p.Settings.Value.Extract.DeleteSource = true;
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }

            Assert.That(IO.Exists(src), Is.False, src);
            Assert.That(IO.Exists(exists), Is.True, exists);
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
        public void Extract_Suspend()
        {
            var src    = GetSource("Complex.1.0.0.zip");
            var dest   = Get("Suspend");
            var exists = Get("Suspend", "Complex.1.0.0");

            using (var p = Create(dest, src))
            {
                p.Model.Interval = TimeSpan.FromMilliseconds(50);
                p.View.Show();
                p.Aggregator.GetEvents().Suspend.Publish(true);
                var count = p.View.Value;
                Task.Delay(150).Wait();
                Assert.That(p.View.Value, Is.EqualTo(count).Within(10)); // see remarks
                p.Aggregator.GetEvents().Suspend.Publish(false);
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }

            Assert.That(IO.Exists(exists), Is.True, exists);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Cancel
        ///
        /// <summary>
        /// 展開処理をキャンセルするテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Cancel()
        {
            using (var p = Create("", GetSource("Complex.zip")))
            {
                p.View.Show();
                p.Aggregator.GetEvents().Cancel.Publish();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");
            }
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
        public void Extract_PasswordCancel()
        {
            var tmp = string.Empty;

            using (var p = Create("", GetSource("Password.7z")))
            {
                p.View.Show();
                Assert.That(Wait(p.View).Result, Is.True, "Timeout");

                tmp = p.Model.Tmp;
                Assert.That(tmp, Is.Not.Null.And.Not.Empty);
                Assert.That(IO.Exists(tmp), Is.True);
            }

            Assert.That(IO.Exists(tmp), Is.False);
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
        public void Extract_ErrorReport()
        {
            using (var p = Create("", GetSource("Sample.txt")))
            {
                p.Settings.Value.ErrorReport = true;
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
        /// 展開処理のテスト用データを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// テストケースには、以下の順で指定します。
        /// - 展開する圧縮ファイル名
        /// - パスワード、
        /// - コマンドライン引数を表す IEnumerable(string) オブジェクト
        /// - ユーザ設定用オブジェクト
        /// - 展開成功確認用のパス（存在チェック）
        /// - 展開後に生成されるファイル + ディレクトリ数
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.None,
                        Filtering     = false,
                    },
                    FullName(@"Others\Complex.1.0.0"),
                    5L
                );

                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    PresetMenu.ExtractRuntime.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.Open,
                        Filtering     = false,
                    },
                    FullName(@"Runtime\Complex.1.0.0"),
                    5L
                );

                yield return new TestCaseData(
                    "SampleEmpty.zip",
                    "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.OpenNotDesktop,
                        Filtering     = false,
                    },
                    FullName(@"Others\Sample"),
                    7L
                );

                yield return new TestCaseData(
                    "SampleFilter.zip",
                    "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        OpenDirectory = OpenDirectoryMethod.OpenNotDesktop,
                        Filtering     = true,
                    },
                    FullName(@"Others\フィルタリング テスト用"),
                    9L
                );

                yield return new TestCaseData(
                    "SampleMac.zip",
                    "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        Filtering     = true,
                    },
                    FullName(@"Others\名称未設定フォルダ"),
                    19L
                );

                yield return new TestCaseData(
                    "Sample 2018.02.13.zip",
                    "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample 2018.02.13"),
                    8L
                );

                yield return new TestCaseData(
                    "Sample..DoubleDot.zip",
                    "",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                        Filtering     = true,
                    },
                    FullName(@"Others\Sample..DoubleDot"),
                    8L
                );

                yield return new TestCaseData(
                    "Password.7z",
                    "password",
                    PresetMenu.Extract.ToArguments(),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Others\Password"),
                    3L
                );

                yield return new TestCaseData(
                    "Single.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Single-0x00"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    FullName(@"RootDirectory\Single-0x00\Sample 00..01.txt"),
                    1L
                );

                yield return new TestCaseData(
                    "Single.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Single-0x01"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    FullName(@"RootDirectory\Single-0x01\Single.1.0.0\Sample 00..01.txt"),
                    1L
                );

                yield return new TestCaseData(
                    "Single.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Single-0x03"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"RootDirectory\Single-0x03\Single.1.0.0\Sample 00..01.txt"),
                    1L
                );

                yield return new TestCaseData(
                    "Single.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Single-0x05"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    FullName(@"RootDirectory\Single-0x05\Sample 00..01.txt"),
                    1L
                );

                yield return new TestCaseData(
                    "Single.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Single-0x07"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                    },
                    FullName(@"RootDirectory\Single-0x07\Sample 00..01.txt"),
                    1L
                );

                yield return new TestCaseData(
                    "SingleDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\SingleDirectory-0x00"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    FullName(@"RootDirectory\SingleDirectory-0x00\Sample"),
                    4L
                );

                yield return new TestCaseData(
                    "SingleDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\SingleDirectory-0x01"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    FullName(@"RootDirectory\SingleDirectory-0x01\SingleDirectory.1.0.0"),
                    4L
                );

                yield return new TestCaseData(
                    "SingleDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\SingleDirectory-0x03"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"RootDirectory\SingleDirectory-0x03\Sample"),
                    4L
                );

                yield return new TestCaseData(
                    "SingleDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\SingleDirectory-0x05"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    FullName(@"RootDirectory\SingleDirectory-0x05\SingleDirectory.1.0.0"),
                    4L
                );

                yield return new TestCaseData(
                    "SingleDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\SingleDirectory-0x07"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                    },
                    FullName(@"RootDirectory\SingleDirectory-0x07\Sample"),
                    4L
                );

                yield return new TestCaseData(
                    "MultiDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\MultiDirectory-0x00"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    FullName(@"RootDirectory\MultiDirectory-0x00\Directory"),
                    7L
                );

                yield return new TestCaseData(
                    "MultiDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\MultiDirectory-0x01"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    FullName(@"RootDirectory\MultiDirectory-0x01\MultiDirectory.1.0.0"),
                    7L
                );

                yield return new TestCaseData(
                    "MultiDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\MultiDirectory-0x03"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"RootDirectory\MultiDirectory-0x03\MultiDirectory.1.0.0"),
                    7L
                );

                yield return new TestCaseData(
                    "MultiDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\MultiDirectory-0x05"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    FullName(@"RootDirectory\MultiDirectory-0x05\MultiDirectory.1.0.0"),
                    7L
                );

                yield return new TestCaseData(
                    "MultiDirectory.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\MultiDirectory-0x07"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                    },
                    FullName(@"RootDirectory\MultiDirectory-0x07\MultiDirectory.1.0.0"),
                    7L
                );

                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Complex-0x00"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.None,
                    },
                    FullName(@"RootDirectory\Complex-0x00\Foo.txt"),
                    5L
                );

                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Complex-0x01"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create,
                    },
                    FullName(@"RootDirectory\Complex-0x01\Complex.1.0.0"),
                    5L
                );

                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Complex-0x03"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"RootDirectory\Complex-0x03\Complex.1.0.0"),
                    5L
                );

                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Complex-0x05"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile,
                    },
                    FullName(@"RootDirectory\Complex-0x05\Complex.1.0.0"),
                    5L
                );

                yield return new TestCaseData(
                    "Complex.1.0.0.zip",
                    "",
                    DropRequest(@"RootDirectory\Complex-0x07"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.Create |
                                        CreateDirectoryMethod.SkipSingleFile |
                                        CreateDirectoryMethod.SkipSingleDirectory,
                    },
                    FullName(@"RootDirectory\Complex-0x07\Complex.1.0.0"),
                    5L
                );

                yield return new TestCaseData(
                    "Sample.tar",
                    "",
                    DropRequest("Tar"),
                    new ExtractSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Tar\TarSample"),
                    4L
                );

                yield return new TestCaseData(
                    "Sample.tbz",
                    "",
                    DropRequest(@"Tar\BZipSample"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Tar\BZipSample\TarSample"),
                    4L
                );

                yield return new TestCaseData(
                    "Sample.tgz",
                    "",
                    DropRequest(@"Tar\GZipSample"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Tar\GZipSample\TarSample"),
                    4L
                );

                yield return new TestCaseData(
                    "Sample.tar.lzma",
                    "",
                    DropRequest(@"Tar\LzmaSample"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Tar\LzmaSample\Sample"),
                    5L
                );

                yield return new TestCaseData(
                    "Sample.tar.z",
                    "",
                    DropRequest(@"Tar\LzwSample"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Tar\LzwSample\Sample"),
                    5L
                );

                yield return new TestCaseData(
                    "Sample.txt.bz2",
                    "",
                    DropRequest("Bz2Sample"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"Bz2Sample\Sample\Sample.txt"),
                    1L
                );

                yield return new TestCaseData(
                    "SampleSfx.exe",
                    "",
                    DropRequest("SfxSample"),
                    new ExtractSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        RootDirectory = CreateDirectoryMethod.CreateSmart,
                    },
                    FullName(@"SfxSample\Sample\Foo.txt"),
                    4L
                );
            }
        }

        #endregion

        #region Others

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
            var dir  = typeof(ExtractTest).FullName;
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
        private static IEnumerable<string> DropRequest(string path) =>
            PresetMenu.Extract
                      .ToArguments()
                      .Concat(new[] { "/o:source", $"/drop:{FullName(path)}" });

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Presenter オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ExtractPresenter Create(string dest, params string[] src)
        {
            var p = Create(new Request(new[] { "/x" }.Concat(src)));

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
            var a = Assembly.GetExecutingAssembly();
            var v = Views.CreateProgressView();
            var e = new Aggregator();
            var s = new SettingsFolder(a, IO);

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
