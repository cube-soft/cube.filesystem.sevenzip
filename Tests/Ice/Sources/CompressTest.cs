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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressTest
    ///
    /// <summary>
    /// Tests to create archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class CompressTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Compress
        ///
        /// <summary>
        /// Tests the compression.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Compress(string dest, IEnumerable<string> files, IEnumerable<string> args, CompressSetting settings)
        {
            using var vm = NewVM(args.Concat(files.Select(e => GetSource(e))), settings);
            var filename = GetFileName(GetSource(files.First()), dest);
            var cvt      = Get("Runtime", filename);

            using (vm.SetPassword("password"))
            using (vm.SetDestination(cvt))
            using (vm.SetRuntime(cvt))
            {
                Assert.That(vm.State, Is.EqualTo(TimerState.Stop));
                Assert.That(vm.Logo,  Is.Not.Null);
                Assert.That(vm.Title, Does.StartWith("0%").And.EndsWith("CubeICE"));
                Assert.That(vm.Text,  Does.StartWith("ファイルを圧縮する準備をしています"));
                Assert.That(vm.Style, Is.EqualTo(ProgressBarStyle.Marquee));
                Assert.That(vm.Count, Is.Not.Null.And.Not.Empty);
                Assert.That(vm.CountVisible, Is.False);

                vm.Test();
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
        private static IEnumerable<TestCaseData> TestCases { get
        {
            yield return new(
                @"Preset\Sample.zip",
                new[] { "Sample.txt" },
                Preset.Compress.ToArguments(),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Preset\Sample 00..01.zip",
                new[] { "Sample 00..01" },
                Preset.Compress.ToArguments(),
                new CompressSetting
                {
                    SaveLocation  = SaveLocation.Preset,
                    OpenMethod    = OpenMethod.None,
                    Filtering     = true,
                }
            );

            yield return new(
                @"Preset\Sample.7z",
                new[] { "Sample.txt", "Sample 00..01" },
                Preset.Compress7z.ToArguments(),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = false,
                }
            );

            yield return new(
                @"Preset\Sample.tar.bz2",
                new[] { "Sample.txt", "Sample 00..01" },
                Preset.CompressBz2.ToArguments(),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Preset\Sample.exe",
                new[] { "Sample.txt", "Sample 00..01" },
                Preset.CompressSfx.ToArguments(),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Runtime\Sample.zip",
                new[] { "Sample.txt", "Sample 00..01" },
                Preset.CompressDetails.ToArguments(),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Runtime\Sample.7z",
                new[] { "Sample.txt", "Sample 00..01" },
                Preset.Compress7z.ToArguments().Concat(new[] { "/p" }),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Query,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Runtime\Sample.tar.bz2",
                new[] { "Sample.txt", "Sample 00..01" },
                Preset.CompressBz2.ToArguments().Concat(new[] { "/o:runtime" }),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Drop\Sample.tar.gz",
                new[] { "Sample.txt", "Sample 00..01" },
                GetPathArgs(Preset.CompressGz, "Drop"),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
            );

            yield return new(
                @"Drop\Sample.tar.xz",
                new[] { "Sample.txt", "Sample 00..01" },
                GetPathArgs(Preset.CompressXz, "Drop"),
                new CompressSetting
                {
                    SaveLocation = SaveLocation.Preset,
                    OpenMethod   = OpenMethod.None,
                    Filtering    = true,
                }
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
        private static IEnumerable<string> GetPathArgs(Preset menu, string filename) =>
            menu.ToArguments()
                .Concat(new[] { $"/drop:{typeof(CompressTest).GetPath(filename)}" });

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets the filename of save path from the specified paths.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFileName(string src, string dest)
        {
            var fi   = Io.Get(src);
            var name = fi.IsDirectory ? fi.Name : fi.BaseName;
            var ext  = Io.Get(dest).Extension;
            return ext == ".bz2" || ext == ".gz" || ext == ".xz" ?
                   $"{name}.tar{ext}" :
                   $"{name}{ext}";
        }

        #endregion
    }
}
