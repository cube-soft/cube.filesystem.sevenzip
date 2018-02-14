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
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathConverterTest
    ///
    /// <summary>
    /// PathConverter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PathConverterTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod_Default
        ///
        /// <summary>
        /// CompressionMethod の初期値を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CompressionMethod_Default()
        {
            var cvt = new PathConverter(@"c:\foo\bar\src.txt", Format.Zip);
            Assert.That(cvt.CompressionMethod, Is.EqualTo(CompressionMethod.Default));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// 変換処理のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public string Convert(string src, Format format, CompressionMethod method) =>
            new PathConverter(src, format, method).Result.FullName;

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// テスト用データを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    @"c:\foo\bar\test.txt",
                    Format.Zip,
                    CompressionMethod.Deflate
                ).Returns(@"c:\foo\bar\test.zip");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.exe.config",
                    Format.SevenZip,
                    CompressionMethod.Lzma
                ).Returns(@"c:\foo\bar\test.exe.7z");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.tar.gz",
                    Format.Sfx,
                    CompressionMethod.Lzma2
                ).Returns(@"c:\foo\bar\test.exe");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.txt",
                    Format.GZip,
                    CompressionMethod.Default
                ).Returns(@"c:\foo\bar\test.tar.gz");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.txt",
                    Format.Tar,
                    CompressionMethod.Copy
                ).Returns(@"c:\foo\bar\test.tar");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.txt",
                    Format.Tar,
                    CompressionMethod.GZip
                ).Returns(@"c:\foo\bar\test.tar.gz");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.txt",
                    Format.Tar,
                    CompressionMethod.BZip2
                ).Returns(@"c:\foo\bar\test.tar.bz2");

                yield return new TestCaseData(
                    @"c:\foo\bar\test.txt",
                    Format.Tar,
                    CompressionMethod.XZ
                ).Returns(@"c:\foo\bar\test.tar.xz");
            }
        }

        #endregion
    }
}
