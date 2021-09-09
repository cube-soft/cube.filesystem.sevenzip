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
using System;
using System.Collections.Generic;
using Cube.Mixin.Assembly;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriterTest
    ///
    /// <summary>
    /// Tests the ArchiveWriter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveWriterTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Tests the methods to create an archive file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public Format Invoke(string filename, Format format, string[] src, CompressionOption options)
        {
            var dest = Get(filename);

            using (var archive = new ArchiveWriter(format, options ?? new()))
            {
                foreach (var e in src) archive.Add(GetSource(e));
                archive.Save(dest);
                archive.Clear();
            }

            using (var ss = Io.Open(dest)) return Formatter.FromStream(ss);
        }

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
        /// The test cases should be specified in the following order:
        /// - Path to save
        /// - Archive format
        /// - Source files
        /// - Archive options
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "ZipSingle.zip",
                    Format.Zip,
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipDirectory.zip",
                    Format.Zip,
                    new[] { "Sample 00..01" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipFast.zip",
                    Format.Zip,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption { CompressionLevel = CompressionLevel.Fast }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipUltra.zip",
                    Format.Zip,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption
                    {
                        CompressionLevel = CompressionLevel.Ultra,
                        ThreadCount      = Environment.ProcessorCount,
                    }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipLzma.zip",
                    Format.Zip,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption { CompressionMethod = CompressionMethod.Lzma }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipPassword.zip",
                    Format.Zip,
                    new[] { "Sample.txt" },
                    new CompressionOption { Password = "password" }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipPasswordJapanese01.zip",
                    Format.Zip,
                    new[] { "Sample.txt" },
                    new CompressionOption { Password = "日本語パスワード" }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipPasswordJapanese02.zip",
                    Format.Zip,
                    new[] { "Sample.txt" },
                    new CompressionOption { Password = "ｶﾞｷﾞｸﾞｹﾞｺﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ" }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "ZipPasswordAes256.zip",
                    Format.Zip,
                    new[] { "Sample.txt" },
                    new CompressionOption { Password = "password" }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    "7zLzma2.7z",
                    Format.SevenZip,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption
                    {
                        CompressionLevel  = CompressionLevel.High,
                        CompressionMethod = CompressionMethod.Lzma2,
                    }
                ).Returns(Format.SevenZip);

                yield return new TestCaseData(
                    "BZip2Test.bz",
                    Format.BZip2,
                    new[] { "Sample.txt" },
                    new CompressionOption()
                ).Returns(Format.BZip2);

                yield return new TestCaseData(
                    "GZipTest.gz",
                    Format.GZip,
                    new[] { "Sample.txt" },
                    new CompressionOption()
                ).Returns(Format.GZip);

                yield return new TestCaseData(
                    "XzTest.xz",
                    Format.XZ,
                    new[] { "Sample.txt" },
                    new CompressionOption()
                ).Returns(Format.XZ);

                yield return new TestCaseData(
                    "TarTest.tar",
                    Format.Tar,
                    new[] { "Sample.txt", "Sample 00..01" },
                    null
                ).Returns(Format.Tar);

                yield return new TestCaseData(
                    "TarTest.tar.gz",
                    Format.Tar,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption
                    {
                        CompressionMethod = CompressionMethod.GZip,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.GZip);

                yield return new TestCaseData(
                    "TarTest.tar.bz",
                    Format.Tar,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption
                    {
                        CompressionMethod = CompressionMethod.BZip2,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.BZip2);

                yield return new TestCaseData(
                    "TarTest.tar.xz",
                    Format.Tar,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new CompressionOption
                    {
                        CompressionMethod = CompressionMethod.XZ,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.XZ);

                yield return new TestCaseData(
                    "ExecutableTest.exe",
                    Format.Sfx,
                    new[] { "Sample.txt", "Sample 00..01" },
                    new SfxOption
                    {
                        CompressionMethod = CompressionMethod.Lzma,
                        CompressionLevel  = CompressionLevel.Ultra,
                        Module            = Pwd(Formatter.SfxName),
                    }
                ).Returns(Format.PE);
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Pwd
        ///
        /// <summary>
        /// Get the path combining the current directory and the specified
        /// filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string Pwd(string filename) =>
            Io.Combine(typeof(ArchiveWriterTest).Assembly.GetDirectoryName(), filename);

        #endregion
    }
}
