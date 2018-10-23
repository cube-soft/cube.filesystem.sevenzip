/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriterTest
    ///
    /// <summary>
    /// ArchiveWriter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveWriterTest : ArchiveFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// 圧縮ファイルを作成するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public Format Archive(Format format, string filename, string password,
            string[] items, ArchiveOption option)
        {
            var dest = GetResultsWith(filename);

            using (var writer = new ArchiveWriter(format))
            {
                writer.Option = option;
                foreach (var item in items) writer.Add(GetExamplesWith(item));
                writer.Save(dest, password);
                writer.Clear();
            }

            using (var ss = IO.OpenRead(dest)) return Formats.FromStream(ss);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Archive のテスト用データを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// テストケースには、以下の順で指定します。
        /// - 圧縮形式
        /// - 圧縮ファイル名
        /// - パスワード
        /// - 圧縮するファイル名一覧
        /// - 圧縮オプション
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
                        Module            = Current(Formats.SfxName),
                    }
                ).Returns(Format.PE);
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Current
        ///
        /// <summary>
        /// カレントディレクトリとパス結合を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string Current(string filename)
        {
            var asm = Assembly.GetExecutingAssembly().GetReader();
            var dir = System.IO.Path.GetDirectoryName(asm.Location);
            return System.IO.Path.Combine(dir, filename);
        }

        #endregion
    }
}
