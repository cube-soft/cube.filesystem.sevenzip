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
using Cube.Generics;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReaderTest
    ///
    /// <summary>
    /// ArchiveReader のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveReaderTest : ArchiveFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Executes a test to extract the specified archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract(string filename, string password) => IgnoreCultureError(() =>
        {
            var src    = GetExamplesWith(filename);
            var dest   = GetResultsWith(nameof(Extract), filename);
            var report = CreateReport();

            using (var obj = new ArchiveReader(src, password)) obj.Extract(dest, Create(report));

            foreach (var cmp in GetExpectedValues(filename))
            {
                var fi = IO.Get(IO.Combine(dest, cmp.Key));

                Assert.That(fi.Exists,         Is.True, cmp.Key);
                Assert.That(fi.Length,         Is.EqualTo(cmp.Value.Length), cmp.Key);
                Assert.That(fi.CreationTime,   Is.Not.EqualTo(DateTime.MinValue), cmp.Key);
                Assert.That(fi.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue), cmp.Key);
                Assert.That(fi.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue), cmp.Key);
            }
        }, $"{filename}, {password}");

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_EachItem
        ///
        /// <summary>
        /// Executes a test to extract the specified archive for each item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract_EachItem(string filename, string password) => IgnoreCultureError(() =>
        {
            var src  = GetExamplesWith(filename);
            var dest = GetResultsWith(nameof(Extract_EachItem), filename);

            using (var obj = new ArchiveReader(src, password))
            {
                var items = obj.Items.ToList();
                var cmp   = GetExpectedValues(filename);
                var keys  = cmp.Keys.ToList();

                Assert.That(items.Count, Is.EqualTo(cmp.Count));

                for (var i = 0; i < keys.Count; ++i)
                {
                    var name = keys[i];
                    Assert.That(items[i].Index,    Is.EqualTo(i), name);
                    Assert.That(items[i].FullName, Is.EqualTo(name), name);
                    Assert.That(items[i].Crc,      Is.EqualTo(cmp[name].Crc), name);

                    items[i].Extract(dest);
                    var fi = IO.Get(IO.Combine(dest, name));
                    Assert.That(fi.Exists,         Is.True, name);
                    Assert.That(fi.Length,         Is.EqualTo(cmp[name].Length), name);
                    Assert.That(fi.CreationTime,   Is.Not.EqualTo(DateTime.MinValue), name);
                    Assert.That(fi.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue), name);
                    Assert.That(fi.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue), name);
                }
            }
        }, $"{filename}, {password}");

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Lite
        ///
        /// <summary>
        /// Executes a test to extract the specified archive.
        /// </summary>
        ///
        /// <remarks>
        /// 解凍処理が正常に終了したかどうかを解凍された個数で確認する
        /// 簡易テストです。
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
            var src    = GetExamplesWith(filename);
            var dest   = GetResultsWith(nameof(Extract_Lite), filename);
            var report = CreateReport();

            using (var obj = new ArchiveReader(src)) obj.Extract(dest, Create(report));
            return report[ReportStatus.End];
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets test cases.
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
        /// GetExpectedValues
        ///
        /// <summary>
        /// Creates the expected result from the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDictionary<string, Expected> GetExpectedValues(string filename)
        {
            var src = GetExamplesWith("Expected", $"{filename}.txt");
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
