/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Collections.Generic;
using System.Reflection;
using Cube.FileSystem.SevenZip;
using NUnit.Framework;

namespace Cube.FileSystem.Tests
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
    [Parallelizable]
    [TestFixture]
    class ArchiveWriterTest : FileResource
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
        [TestCaseSource(nameof(Archive_TestCases))]
        public Format Archive(Format format, string filename, string password,
            string[] items, ArchiveOption option)
        {
            var dest = Result(filename);

            using (var writer = new ArchiveWriter(format))
            {
                writer.Option = option;
                foreach (var item in items) writer.Add(Example(item));
                writer.Save(dest, password);
            }

            using (var stream = System.IO.File.OpenRead(dest))
            {
                return Formats.FromStream(stream);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Filter
        ///
        /// <summary>
        /// フィルタ設定の結果を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(true,  ExpectedResult = 4)]
        [TestCase(false, ExpectedResult = 8)]
        public int Archive_Filter(bool filter)
        {
            var names = new[] { "Filter.txt", "FilterDirectory" };
            var s     = filter ? "True" : "False";
            var dest  = Result($"Filter{s}.zip");

            using (var writer = new ArchiveWriter(Format.Zip))
            {
                if (filter) writer.Filters = names;
                writer.Add(Example("Sample.txt"));
                writer.Add(Example("Archive"));
                writer.Save(dest);
            }

            using (var reader = new ArchiveReader(dest)) return reader.Items.Count;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Japanese
        ///
        /// <summary>
        /// 日本語のファイル名を含むファイルを圧縮するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_Japanese()
        {
            var fmt  = Format.Zip;
            var src  = Result("日本語のファイル名.txt");
            var dest = Result("ZipJapanese.zip");

            System.IO.File.Copy(Example("Sample.txt"), src);
            Assert.That(System.IO.File.Exists(src), Is.True);

            using (var writer = new ArchiveWriter(fmt))
            {
                writer.Add(src);
                writer.Save(dest);
            }

            using (var stream = System.IO.File.OpenRead(dest))
            {
                Assert.That(Formats.FromStream(stream), Is.EqualTo(fmt));
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_TestCases
        ///
        /// <summary>
        /// Archive のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> Archive_TestCases
        {
            get
            {
                yield return new TestCaseData(Format.Zip,
                    "ZipSingle.zip",
                    "",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipDirectory.zip",
                    "",
                    new[] { "Archive" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipUltra.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ZipOption { CompressionLevel = CompressionLevel.Ultra }
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipLzma.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ZipOption { CompressionMethod = CompressionMethod.Lzma }
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipPassword.zip",
                    "password",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipPasswordJapanese01.zip",
                    "日本語パスワード",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipPasswordJapanese02.zip",
                    "ｶﾞｷﾞｸﾞｹﾞｺﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipPasswordAes256.zip",
                    "password",
                    new[] { "Sample.txt" },
                    new ZipOption { EncryptionMethod = EncryptionMethod.Aes256 }
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.SevenZip,
                    "7zLzma2.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new SevenZipOption
                    {
                        CompressionLevel  = CompressionLevel.High,
                        CompressionMethod = CompressionMethod.Lzma2,
                    }
                ).Returns(Format.SevenZip);

                yield return new TestCaseData(Format.BZip2,
                    "BZip2Test.bz",
                    "",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.BZip2);

                yield return new TestCaseData(Format.GZip,
                    "GZipTest.gz",
                    "",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.GZip);

                yield return new TestCaseData(Format.Tar,
                    "TarTest.tar",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    null
                ).Returns(Format.Tar);

                yield return new TestCaseData(Format.Tar,
                    "TarTest.tar.gz",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new TarOption
                    {
                        CompressionMethod = CompressionMethod.GZip,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.GZip);

                yield return new TestCaseData(Format.Tar,
                    "TarTest.tar.bz",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new TarOption
                    {
                        CompressionMethod = CompressionMethod.BZip2,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.BZip2);

                yield return new TestCaseData(Format.Sfx,
                    "ExecutableTest.exe",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ExecutableOption
                    {
                        CompressionMethod = CompressionMethod.Lzma,
                        CompressionLevel  = CompressionLevel.Ultra,
                        Module            = Current("7z.sfx"),
                    }
                ).Returns(Format.PE);
            }
        }

        #endregion

        #region Helper

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
            var asm = Assembly.GetExecutingAssembly().Location;
            var dir = System.IO.Path.GetDirectoryName(asm);
            return System.IO.Path.Combine(dir, filename);
        }

        #endregion
    }
}
