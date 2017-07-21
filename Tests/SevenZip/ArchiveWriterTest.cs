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
        public void Archive(Format format, string filename, string password,
            string[] items, ArchiveOption option, long size)
        {
            var dest = Result(filename);

            using (var writer = new ArchiveWriter(format))
            {
                writer.Option = option;
                foreach (var item in items) writer.Add(Example(item));
                writer.Save(dest, password);
            }

            var info = new System.IO.FileInfo(dest);
            Assert.That(info.Exists, Is.True);
            Assert.That(info.Length, Is.EqualTo(size));
        }

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
                    null,
                    167L
                );

                yield return new TestCaseData(Format.Zip,
                    "ZipDirectory.zip",
                    "",
                    new[] { "Archive" },
                    null,
                    11873L
                );

                yield return new TestCaseData(Format.Zip,
                    "ZipUltra.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ZipOption { CompressionLevel = CompressionLevel.Ultra },
                    11822L
                );

                yield return new TestCaseData(Format.Zip,
                    "ZipLzma.zip",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    new ZipOption { CompressionMethod = CompressionMethod.Lzma },
                    11645L
                );

                yield return new TestCaseData(Format.Zip,
                    "ZipPassword.zip",
                    "password",
                    new[] { "Sample.txt" },
                    null,
                    179L
                );

                yield return new TestCaseData(Format.Zip,
                    "ZipAes256.zip",
                    "password",
                    new[] { "Sample.txt" },
                    new ZipOption
                    {
                        EncryptionMethod = EncryptionMethod.Aes256,
                        IsEncrypted = true,
                    },
                    217L
                );

                yield return new TestCaseData(Format.Tar,
                    "TarTest.tar",
                    "",
                    new[] { "Sample.txt", "Archive" },
                    null,
                    38400L
                );

                yield return new TestCaseData(Format.BZip2,
                    "BZip2Test.bz",
                    "",
                    new[] { "Sample.txt" },
                    null,
                    51L
                );

                yield return new TestCaseData(Format.GZip,
                    "GZipTest.gz",
                    "",
                    new[] { "Sample.txt" },
                    null,
                    47L
                );
            }
        }

        #endregion
    }
}
