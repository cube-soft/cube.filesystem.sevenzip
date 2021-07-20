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
        public Format Invoke(Format format, string filename, string password,
            string[] items, ArchiveOption option)
        {
            var dest = Get(filename);

            using (var archive = new ArchiveWriter(format))
            {
                archive.Option = option;
                foreach (var e in items) archive.Add(GetSource(e));
                archive.Save(dest, password);
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
        /// - Archive format
        /// - Path to save
        /// - Password
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
                    Format.Zip,
                    "ZipSingle.zip",
                    "",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipDirectory.zip",
                    "",
                    new[] { "Sample 00..01" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipFast.zip",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new ZipOption { CompressionLevel = CompressionLevel.Fast }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipUltra.zip",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new ZipOption
                    {
                        CompressionLevel = CompressionLevel.Ultra,
                        ThreadCount      = Environment.ProcessorCount,
                    }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipLzma.zip",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new ZipOption { CompressionMethod = CompressionMethod.Lzma }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipPassword.zip",
                    "password",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipPasswordSymbol.zip",
                    "()[]{}<>\\#$%@?!&|+-*/=\"'^~`,._",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipPasswordJapanese01.zip",
                    "日本語パスワード",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipPasswordJapanese02.zip",
                    "ｶﾞｷﾞｸﾞｹﾞｺﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.Zip,
                    "ZipPasswordAes256.zip",
                    "password",
                    new[] { "Sample.txt" },
                    new ZipOption { EncryptionMethod = EncryptionMethod.Aes256 }
                ).Returns(Format.Zip);

                yield return new TestCaseData(
                    Format.SevenZip,
                    "7zLzma2.7z",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new SevenZipOption
                    {
                        CompressionLevel  = CompressionLevel.High,
                        CompressionMethod = CompressionMethod.Lzma2,
                    }
                ).Returns(Format.SevenZip);

                yield return new TestCaseData(
                    Format.BZip2,
                    "BZip2Test.bz",
                    "",
                    new[] { "Sample.txt" },
                    new ArchiveOption()
                ).Returns(Format.BZip2);

                yield return new TestCaseData(
                    Format.GZip,
                    "GZipTest.gz",
                    "",
                    new[] { "Sample.txt" },
                    new ArchiveOption()
                ).Returns(Format.GZip);

                yield return new TestCaseData(
                    Format.XZ,
                    "XzTest.xz",
                    "",
                    new[] { "Sample.txt" },
                    new ArchiveOption()
                ).Returns(Format.XZ);

                yield return new TestCaseData(
                    Format.Tar,
                    "TarTest.tar",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    null
                ).Returns(Format.Tar);

                yield return new TestCaseData(
                    Format.Tar,
                    "TarTest.tar.gz",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new TarOption
                    {
                        CompressionMethod = CompressionMethod.GZip,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.GZip);

                yield return new TestCaseData(
                    Format.Tar,
                    "TarTest.tar.bz",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new TarOption
                    {
                        CompressionMethod = CompressionMethod.BZip2,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.BZip2);

                yield return new TestCaseData(
                    Format.Tar,
                    "TarTest.tar.xz",
                    "",
                    new[] { "Sample.txt", "Sample 00..01" },
                    new TarOption
                    {
                        CompressionMethod = CompressionMethod.XZ,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.XZ);

                yield return new TestCaseData(
                    Format.Sfx,
                    "ExecutableTest.exe",
                    "",
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
