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
using System.Globalization;
using Cube.Mixin.String;
using Cube.Tests;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReaderTest
    ///
    /// <summary>
    /// Tests the ArchiveReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveReaderTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the Save method with the specified archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract(string filename, string password) => IgnoreCultureError(() =>
        {
            var src  = GetSource(filename);
            var dest = Get(nameof(Extract), filename);

            using var archive = new ArchiveReader(src, password);
            archive.Save(dest);

            foreach (var cmp in GetAnswer(filename))
            {
                var fi = Io.Get(Io.Combine(dest, cmp.Key));

                Assert.That(fi.Exists,         Is.True, cmp.Key);
                Assert.That(fi.Length,         Is.EqualTo(cmp.Value.Length), cmp.Key);
                Assert.That(fi.CreationTime,   Is.Not.EqualTo(DateTime.MinValue), cmp.Key);
                Assert.That(fi.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue), cmp.Key);
                Assert.That(fi.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue), cmp.Key);
            }
        }, $"{filename}, {password}");

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Lite
        ///
        /// <summary>
        /// Tests the Save method with the specified archive.
        /// </summary>
        ///
        /// <remarks>
        /// This is a simple test to check if the decompression process has
        /// been completed successfully by the number of decompressed files.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.cab",     ExpectedResult =  3)]
        [TestCase("Sample.chm",     ExpectedResult = 89)]
        [TestCase("Sample.cpio",    ExpectedResult =  5)]
        [TestCase("Sample.docx",    ExpectedResult = 13)]
        [TestCase("Sample.exe",     ExpectedResult =  4)]
        [TestCase("Sample.nupkg",   ExpectedResult =  5)]
        [TestCase("Sample.pptx",    ExpectedResult = 40)]
        [TestCase("Sample.xlsx",    ExpectedResult = 14)]
        [TestCase("SampleSfx.exe",  ExpectedResult =  4)]
        public int Extract_Lite(string filename)
        {
            var src  = GetSource(filename);
            var dest = Get(nameof(Extract_Lite), filename);
            var cnt  = new Counter();

            using (var obj = new ArchiveReader(src)) obj.Test(); // Test
            using (var obj = new ArchiveReader(src)) obj.Save(dest, cnt);

            return cnt.Results[ReportStatus.End];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the Test method with the specified archive in test mode.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Test(string filename, string password) => IgnoreCultureError(() => {
            var src = GetSource(filename);
            using var archive = new ArchiveReader(src, password);
            archive.Test();
        }, $"{filename}, {password}");

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
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("Sample.zip", "");
                yield return new TestCaseData("SampleEmpty.zip", "");
                yield return new TestCaseData("SampleVolume.zip", "");
                yield return new TestCaseData("SampleVolume.rar.001", "");
                yield return new TestCaseData("SampleComma.zip", "");
                yield return new TestCaseData("SampleMac.zip", "");
                yield return new TestCaseData("SampleUtf8.zip", "");
                yield return new TestCaseData("SampleKanji.zip", "");
                yield return new TestCaseData("SampleUnixSjis.zip", "");
                yield return new TestCaseData("Sample 2018.02.13.zip", "");
                yield return new TestCaseData("Sample..DoubleDot.zip", "");
                yield return new TestCaseData("Sample.tar", "");
                yield return new TestCaseData("Sample.tar.bz2", "");
                yield return new TestCaseData("Sample.tar.gz", "");
                yield return new TestCaseData("Sample.tar.lzma", "");
                yield return new TestCaseData("Sample.tar.z", "");
                yield return new TestCaseData("Sample.taz", "");
                yield return new TestCaseData("Sample.tb2", "");
                yield return new TestCaseData("Sample.tbz", "");
                yield return new TestCaseData("Sample.tgz", "");
                yield return new TestCaseData("Sample.txz", "");
                yield return new TestCaseData("Sample.txt.bz2", "");
                yield return new TestCaseData("Sample.txt.gz", "");
                yield return new TestCaseData("Sample.txt.xz", "");
                yield return new TestCaseData("Sample.arj", "");
                yield return new TestCaseData("Sample.flv", "");
                yield return new TestCaseData("Sample.jar", "");
                yield return new TestCaseData("Sample.lha", "");
                yield return new TestCaseData("Sample.lzh", "");
                yield return new TestCaseData("Sample.rar", "");
                yield return new TestCaseData("Sample.rar5", "");
                yield return new TestCaseData("SampleEmpty.rar", "");
                yield return new TestCaseData("SampleEmpty.7z", "");
                yield return new TestCaseData("Password.7z", "password");
                yield return new TestCaseData("PasswordHeader.7z", "password");
                yield return new TestCaseData("PasswordSymbol01.zip", "()[]{}<>");
                yield return new TestCaseData("PasswordSymbol02.zip", "\\#$%@?");
                yield return new TestCaseData("PasswordSymbol03.zip", "!&|+-*/=");
                yield return new TestCaseData("PasswordSymbol04.zip", "\"'^~`,._");
                yield return new TestCaseData("PasswordJapanese01.zip", "日本語パスワード");
                yield return new TestCaseData("PasswordJapanese02.zip", "ｶﾞｷﾞｸﾞｹﾞｺﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ");
                yield return new TestCaseData("InvalidSymbol.zip", "");
                yield return new TestCaseData("InvalidReserved.zip", "");
                yield return new TestCaseData("ZipSlip.zip", "");
                yield return new TestCaseData("ZipSlip.tar", "");
                yield return new TestCaseData("ZipSlipWin.zip", "");
                yield return new TestCaseData("ZipSlipWin.tar", "");
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Expected
        ///
        /// <summary>
        /// Represents the expected values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class Expected
        {
            public long Length { get; set; }
            public uint Crc { get; set; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetAnswer
        ///
        /// <summary>
        /// Gets the expected results of the specified archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDictionary<string, Expected> GetAnswer(string filename)
        {
            var src = GetSource("Expected", $"{filename}.txt");
            var csv = new TextFieldParser(src, System.Text.Encoding.UTF8)
            {
                Delimiters                = new[] { "," },
                HasFieldsEnclosedInQuotes = true,
                TextFieldType             = FieldType.Delimited,
                TrimWhiteSpace            = true,
            };

            var dest = new Dictionary<string, Expected>();
            while (!csv.EndOfData)
            {
                var row = csv.ReadFields();
                dest.Add(row[0], new Expected
                {
                    Length = long.Parse(row[1]),
                    Crc    = uint.Parse(row[2])
                });
            }
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IgnoreCultureError
        ///
        /// <summary>
        /// Checks if the thrown exception is the EncryptionException class.
        /// </summary>
        ///
        /// <remarks>
        /// ロケールが日本語以外の環境で失敗するテストに関しては、現時点
        /// では無視しています。将来的には CodePage を指定可能な形に修正
        /// する事で対応する予定です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void IgnoreCultureError(Action action, string message)
        {
            try { action(); }
            catch (EncryptionException)
            {
                var code = CultureInfo.CurrentCulture.Name;
                if (!code.FuzzyEquals("ja-JP")) Assert.Ignore(message);
                else throw;
            }
        }

        #endregion
    }
}
