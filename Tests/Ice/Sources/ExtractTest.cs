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
using System.Collections.Generic;
using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;

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
    [NonParallelizable]
    class ExtractTest : VmFixture
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
        public void Extract(string dest, IEnumerable<string> files, IEnumerable<string> args, ExtractSettingValue settings)
        {
            settings.SaveDirectory = Get("Preset");

            using var vm = NewVM(args.Concat(files.Select(e => GetSource(e))), settings);
            using (vm.SetPassword("password")) // if needed
            using (vm.SetDestination(Get("Runtime")))
            {
                Assert.That(vm.State,      Is.EqualTo(TimerState.Stop));
                Assert.That(vm.Cancelable, Is.False);
                Assert.That(vm.Suspended,  Is.False);
                Assert.That(vm.Count,      Is.Not.Null.And.Not.Empty);
                Assert.That(vm.Title,      Does.StartWith("0%").And.EndsWith("CubeICE"));
                Assert.That(vm.Text,       Does.StartWith("ファイルを解凍する準備をしています"));
                vm.Test();
                GetType().LogDebug(vm.Elapsed, vm.Remaining, vm.Title);
            }

            Assert.That(Io.Exists(Get(dest)), Is.True, dest);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets the test cases. Format is: destination, source files,
        /// other arguments, and user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases { get
        {
            yield return new(
                @"Preset\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip" },
                Enumerable.Empty<string>(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = false,
                }
            );

            yield return new(
                @"Runtime\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip" },
                Preset.ExtractQuery.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = false,
                }
            );

            yield return new(
                @"Preset\Sample\Empty",
                new[] { "SampleEmpty.zip" },
                Preset.Extract.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = false,
                }
            );

            yield return new(
                @"Preset\フィルタリング テスト用",
                new[] { "SampleFilter.zip" },
                Preset.Extract.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Preset\名称未設定フォルダ",
                new[] { "SampleMac.zip" },
                Preset.Extract.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Preset\Sample 2018.02.13",
                new[] { "Sample 2018.02.13.zip" },
                Preset.Extract.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Preset\Sample..DoubleDot",
                new[] { "Sample..DoubleDot.zip" },
                Preset.Extract.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Preset\Password",
                new[] { "Password.7z" },
                Preset.Extract.ToArguments(),
                new ExtractSettingValue
                {
                    SaveLocation = SaveLocation.Preset,
                    SaveMethod   = SaveMethod.CreateSmart,
                }
            );

            yield return new(
                @"RootDirectory\Single-0x00\Sample 00..01.txt",
                new[] { "Single.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Single-0x00"),
                new ExtractSettingValue { SaveMethod = SaveMethod.None }
            );

            yield return new(
                @"RootDirectory\Single-0x01\Single.1.0.0\Sample 00..01.txt",
                new[] { "Single.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Single-0x01"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create }
            );

            yield return new(
                @"RootDirectory\Single-0x03\Single.1.0.0\Sample 00..01.txt",
                new[] { "Single.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Single-0x03"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"RootDirectory\Single-0x05\Sample 00..01.txt",
                new[] { "Single.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Single-0x05"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create | SaveMethod.SkipSingleFile }
            );

            yield return new(
                @"RootDirectory\Single-0x07\Sample 00..01.txt",
                new[] { "Single.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Single-0x07"),
                new ExtractSettingValue
                {
                    SaveMethod = SaveMethod.Create |
                                 SaveMethod.SkipSingleFile |
                                 SaveMethod.SkipSingleDirectory,
                }
            );

            yield return new(
                @"RootDirectory\SingleDirectory-0x00\Sample",
                new[] { "SingleDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\SingleDirectory-0x00"),
                new ExtractSettingValue { SaveMethod = SaveMethod.None }
            );

            yield return new(
                @"RootDirectory\SingleDirectory-0x01\SingleDirectory.1.0.0",
                new[] { "SingleDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\SingleDirectory-0x01"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create }
            );

            yield return new(
                @"RootDirectory\SingleDirectory-0x03\Sample",
                new[] { "SingleDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\SingleDirectory-0x03"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"RootDirectory\SingleDirectory-0x05\SingleDirectory.1.0.0",
                new[] { "SingleDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\SingleDirectory-0x05"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create | SaveMethod.SkipSingleFile }
            );

            yield return new(
                @"RootDirectory\SingleDirectory-0x07\Sample",
                new[] { "SingleDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\SingleDirectory-0x07"),
                new ExtractSettingValue
                {
                    SaveMethod = SaveMethod.Create |
                                 SaveMethod.SkipSingleFile |
                                 SaveMethod.SkipSingleDirectory,
                }
            );

            yield return new(
                @"RootDirectory\MultiDirectory-0x00\Directory",
                new[] { "MultiDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\MultiDirectory-0x00"),
                new ExtractSettingValue { SaveMethod = SaveMethod.None }
            );

            yield return new(
                @"RootDirectory\MultiDirectory-0x01\MultiDirectory.1.0.0",
                new[] { "MultiDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\MultiDirectory-0x01"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create }
            );

            yield return new(
                @"RootDirectory\MultiDirectory-0x03\MultiDirectory.1.0.0",
                new[] { "MultiDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\MultiDirectory-0x03"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"RootDirectory\MultiDirectory-0x05\MultiDirectory.1.0.0",
                new[] { "MultiDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\MultiDirectory-0x05"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create | SaveMethod.SkipSingleFile }
            );

            yield return new(
                @"RootDirectory\MultiDirectory-0x07\MultiDirectory.1.0.0",
                new[] { "MultiDirectory.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\MultiDirectory-0x07"),
                new ExtractSettingValue
                {
                    SaveMethod = SaveMethod.Create |
                                 SaveMethod.SkipSingleFile |
                                 SaveMethod.SkipSingleDirectory,
                }
            );

            yield return new(
                @"RootDirectory\Complex-0x00\Foo.txt",
                new[] { "Complex.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Complex-0x00"),
                new ExtractSettingValue { SaveMethod = SaveMethod.None }
            );

            yield return new(
                @"RootDirectory\Complex-0x01\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Complex-0x01"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create }
            );

            yield return new(
                @"RootDirectory\Complex-0x03\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Complex-0x03"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"RootDirectory\Complex-0x05\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Complex-0x05"),
                new ExtractSettingValue { SaveMethod = SaveMethod.Create | SaveMethod.SkipSingleFile }
            );

            yield return new(
                @"RootDirectory\Complex-0x07\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip" },
                GetPathArgs(@"RootDirectory\Complex-0x07"),
                new ExtractSettingValue
                {
                    SaveMethod = SaveMethod.Create |
                                 SaveMethod.SkipSingleFile |
                                 SaveMethod.SkipSingleDirectory,
                }
            );

            yield return new(
                @"Tar\TarSample",
                new[] { "Sample.tar" },
                GetPathArgs("Tar"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"Tar\BZipSample\TarSample",
                new[] { "Sample.tbz" },
                GetPathArgs(@"Tar\BZipSample"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"Tar\GZipSample\TarSample",
                new[] { "Sample.tgz" },
                GetPathArgs(@"Tar\GZipSample"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"Tar\LzmaSample\Sample",
                new[] { "Sample.tar.lzma" },
                GetPathArgs(@"Tar\LzmaSample"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"Tar\LzwSample\Sample",
                new[] { "Sample.tar.z" },
                GetPathArgs(@"Tar\LzwSample"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"Bz2Sample\Sample\Sample.txt",
                new[] { "Sample.txt.bz2" },
                GetPathArgs("Bz2Sample"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"SfxSample\Sample\Foo.txt",
                new[] { "SampleSfx.exe" },
                GetPathArgs("SfxSample"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );

            yield return new(
                @"Multiple\Complex.1.0.0",
                new[] { "Complex.1.0.0.zip", "Single.1.0.0.zip" },
                GetPathArgs("Multiple"),
                new ExtractSettingValue { SaveMethod = SaveMethod.CreateSmart }
            );
        }}

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// GetPathArgs
        ///
        /// <summary>
        /// Gets the arguments that contain the path argument.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<string> GetPathArgs(string filename) => new[]
        {
            $"/o:to:{typeof(ExtractTest).GetPath(filename)}",
        };

        #endregion
    }
}
