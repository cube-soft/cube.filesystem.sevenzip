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
using System;
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
    [TestFixture]
    class ArchiveWriterTest : FileHandler
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
        [TestCase(true)]
        [TestCase(false)]
        public void Archive_Japanese(bool utf8)
        {
            var fmt  = Format.Zip;
            var src  = Result("日本語のファイル名.txt");
            var code = utf8 ? "UTF8" : "SJis";
            var dest = Result($"ZipJapanese{code}.zip");

            IO.Copy(Example("Sample.txt"), src, true);
            Assert.That(IO.Get(src).Exists, Is.True);

            using (var writer = new ArchiveWriter(fmt))
            {
                writer.Option = new ZipOption { UseUtf8 = utf8 };
                writer.Add(src);
                writer.Save(dest);
            }

            using (var stream = System.IO.File.OpenRead(dest))
            {
                Assert.That(Formats.FromStream(stream), Is.EqualTo(fmt));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_PasswordCancel
        /// 
        /// <summary>
        /// パスワードの設定をキャンセルした時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_PasswordCancel()
            => Assert.That(() =>
            {
                using (var writer = new ArchiveWriter(Format.Zip))
                {
                    var dest  = Result("PasswordCancel.zip");
                    var query = new Query<string, string>(x => x.Cancel = true);
                    writer.Add(Example("Sample.txt"));
                    writer.Save(dest, query, null);
                }
            },
            Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_SfxNotFound
        /// 
        /// <summary>
        /// 存在しない SFX モジュールを設定した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_SfxNotFound()
            => Assert.That(() =>
            {
                using (var writer = new ArchiveWriter(Format.Sfx))
                {
                    var dest = Result("SfxNotFound.exe");
                    writer.Option = new ExecutableOption { Module = "dummy.sfx" };
                    writer.Add(Example("Sample.txt"));
                    writer.Save(dest);
                }
            },
            Throws.TypeOf<System.IO.FileNotFoundException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_PermissionError
        ///
        /// <summary>
        /// 読み込みできないファイルを指定した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_PermissionError()
            => Assert.That(() =>
            {
                var dir = Result("PermissionError");
                var src = IO.Combine(dir, "Sample.txt");

                IO.Copy(Example("Sample.txt"), src);

                using (var _ = System.IO.File.Open(
                    src,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.ReadWrite,
                    System.IO.FileShare.None
                )) {
                    using (var writer = new ArchiveWriter(Format.Zip))
                    {
                        writer.Add(src);
                        writer.Save(IO.Combine(dir, "Sample.zip"));
                    }
                }
            },
            Throws.TypeOf<System.IO.IOException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Add_NotFound
        ///
        /// <summary>
        /// 存在しないファイルを指定した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Add_NotFound()
            => Assert.That(() =>
            {
                using (var writer = new ArchiveWriter(Format.Zip))
                {
                    writer.Add(Example("NotFound.txt"));
                }
            },
            Throws.TypeOf<System.IO.FileNotFoundException>());

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
                    "ZipFast.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ZipOption { CompressionLevel = CompressionLevel.Fast }
                ).Returns(Format.Zip);

                yield return new TestCaseData(Format.Zip,
                    "ZipUltra.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ZipOption
                    {
                        CompressionLevel = CompressionLevel.Ultra,
                        ThreadCount      = Environment.ProcessorCount,
                    }
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
                    "7zLzma2.7z",
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

                yield return new TestCaseData(Format.XZ,
                    "XzTest.xz",
                    "",
                    new[] { "Sample.txt" },
                    null
                ).Returns(Format.XZ);

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

                yield return new TestCaseData(Format.Tar,
                    "TarTest.tar.xz",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new TarOption
                    {
                        CompressionMethod = CompressionMethod.XZ,
                        CompressionLevel  = CompressionLevel.Ultra,
                    }
                ).Returns(Format.XZ);

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
