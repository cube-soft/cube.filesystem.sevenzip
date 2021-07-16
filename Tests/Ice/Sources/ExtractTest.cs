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
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractTest
    ///
    /// <summary>
    /// Tests to extract archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractTest : ArchiveFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extraction.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract(string dest,
            IEnumerable<string> files,
            IEnumerable<string> args,
            ExtractValue settings
        ) => Create(files, args, Make(settings), vm =>
        {
            using (vm.SetPassword("password")) // if needed
            using (vm.SetDestination(Get("Runtime")))
            {
                Assert.That(vm.State, Is.EqualTo(TimerState.Stop));
                Assert.That(vm.Logo,  Is.Not.Null);
                Assert.That(vm.Title, Does.StartWith("0%").And.EndsWith("CubeICE"));
                Assert.That(vm.Text,  Does.StartWith("ファイルを解凍する準備をしています"));
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
        /// - 展開成功確認用のパス（存在チェック）
        /// - 展開する圧縮ファイル名
        /// - コマンドライン引数を表す IEnumerable(string) オブジェクト
        /// - ユーザ設定用オブジェクト
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    @"Others\Complex.1.0.0",
                    new[] { "Complex.1.0.0.zip" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = false,
                    }
                );

                yield return new TestCaseData(
                    @"Runtime\Complex.1.0.0",
                    new[] { "Complex.1.0.0.zip" },
                    PresetMenu.ExtractRuntime.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = false,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample",
                    new[] { "SampleEmpty.zip" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = false,
                    }
                );

                yield return new TestCaseData(
                    @"Others\フィルタリング テスト用",
                    new[] { "SampleFilter.zip" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\名称未設定フォルダ",
                    new[] { "SampleMac.zip" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample 2018.02.13",
                    new[] { "Sample 2018.02.13.zip" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Sample..DoubleDot",
                    new[] { "Sample..DoubleDot.zip" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                        Filtering    = true,
                    }
                );

                yield return new TestCaseData(
                    @"Others\Password",
                    new[] { "Password.7z" },
                    PresetMenu.Extract.ToArguments(),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Single-0x00\Sample 00..01.txt",
                    new[] { "Single.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Single-0x00"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.None,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Single-0x01\Single.1.0.0\Sample 00..01.txt",
                    new[] { "Single.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Single-0x01"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Single-0x03\Single.1.0.0\Sample 00..01.txt",
                    new[] { "Single.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Single-0x03"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Single-0x05\Sample 00..01.txt",
                    new[] { "Single.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Single-0x05"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create | SaveMethod.SkipSingleFile,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Single-0x07\Sample 00..01.txt",
                    new[] { "Single.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Single-0x07"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create |
                                       SaveMethod.SkipSingleFile |
                                       SaveMethod.SkipSingleDirectory,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\SingleDirectory-0x00\Sample",
                    new[] { "SingleDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\SingleDirectory-0x00"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.None,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\SingleDirectory-0x01\SingleDirectory.1.0.0",
                    new[] { "SingleDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\SingleDirectory-0x01"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\SingleDirectory-0x03\Sample",
                    new[] { "SingleDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\SingleDirectory-0x03"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\SingleDirectory-0x05\SingleDirectory.1.0.0",
                    new[] { "SingleDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\SingleDirectory-0x05"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create | SaveMethod.SkipSingleFile,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\SingleDirectory-0x07\Sample",
                    new[] { "SingleDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\SingleDirectory-0x07"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create |
                                       SaveMethod.SkipSingleFile |
                                       SaveMethod.SkipSingleDirectory,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\MultiDirectory-0x00\Directory",
                    new[] { "MultiDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\MultiDirectory-0x00"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.None,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\MultiDirectory-0x01\MultiDirectory.1.0.0",
                    new[] { "MultiDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\MultiDirectory-0x01"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\MultiDirectory-0x03\MultiDirectory.1.0.0",
                    new[] { "MultiDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\MultiDirectory-0x03"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\MultiDirectory-0x05\MultiDirectory.1.0.0",
                    new[] { "MultiDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\MultiDirectory-0x05"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create | SaveMethod.SkipSingleFile,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\MultiDirectory-0x07\MultiDirectory.1.0.0",
                    new[] { "MultiDirectory.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\MultiDirectory-0x07"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create |
                                       SaveMethod.SkipSingleFile |
                                       SaveMethod.SkipSingleDirectory,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Complex-0x00\Foo.txt",
                    new[] { "Complex.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Complex-0x00"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.None,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Complex-0x01\Complex.1.0.0",
                    new[] { "Complex.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Complex-0x01"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Complex-0x03\Complex.1.0.0",
                    new[] { "Complex.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Complex-0x03"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Complex-0x05\Complex.1.0.0",
                    "Complex.1.0.0.zip",
                    "",
                    GetPathArgs(@"RootDirectory\Complex-0x05"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create | SaveMethod.SkipSingleFile,
                    }
                );

                yield return new TestCaseData(
                    @"RootDirectory\Complex-0x07\Complex.1.0.0",
                    new[] { "Complex.1.0.0.zip" },
                    GetPathArgs(@"RootDirectory\Complex-0x07"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.Create |
                                       SaveMethod.SkipSingleFile |
                                       SaveMethod.SkipSingleDirectory,
                    }
                );

                yield return new TestCaseData(
                    @"Tar\TarSample",
                    new[] { "Sample.tar" },
                    GetPathArgs("Tar"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"Tar\BZipSample\TarSample",
                    new[] { "Sample.tbz" },
                    GetPathArgs(@"Tar\BZipSample"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"Tar\GZipSample\TarSample",
                    new[] { "Sample.tgz" },
                    GetPathArgs(@"Tar\GZipSample"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"Tar\LzmaSample\Sample",
                    new[] { "Sample.tar.lzma" },
                    GetPathArgs(@"Tar\LzmaSample"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"Tar\LzwSample\Sample",
                    new[] { "Sample.tar.z" },
                    GetPathArgs(@"Tar\LzwSample"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod  = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"Bz2Sample\Sample\Sample.txt",
                    new[] { "Sample.txt.bz2" },
                    GetPathArgs("Bz2Sample"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"SfxSample\Sample\Foo.txt",
                    new[] { "SampleSfx.exe" },
                    GetPathArgs("SfxSample"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod   = SaveMethod.CreateSmart,
                    }
                );

                yield return new TestCaseData(
                    @"Multiple\Complex.1.0.0",
                    new[] { "Complex.1.0.0.zip", "Single.1.0.0.zip" },
                    GetPathArgs("Multiple"),
                    new ExtractValue
                    {
                        SaveLocation = SaveLocation.Preset,
                        SaveMethod = SaveMethod.CreateSmart,
                    }
                );
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Make
        ///
        /// <summary>
        /// Adds some settings to the specified value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ExtractValue Make(ExtractValue src)
        {
            src.SaveDirectory = Get("Others");
            return src;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPathArgs
        ///
        /// <summary>
        /// Gets the arguments that contain the path argument.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<string> GetPathArgs(string filename) =>
            PresetMenu.Extract
                      .ToArguments()
                      .Concat(new[] { "/o:source", $"/drop:{typeof(ExtractTest).GetPath(filename)}" });

        #endregion
    }
}
