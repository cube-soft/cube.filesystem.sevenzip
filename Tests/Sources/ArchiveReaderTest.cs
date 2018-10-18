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
using Cube.FileSystem.SevenZip.Archives;
using Cube.FileSystem.TestService;
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
    class ArchiveReaderTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 圧縮ファイルのリストを取得するテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// 圧縮形式によっては各項目のファイルサイズが取得できない場合が
        /// あるため、各項目のファイルサイズの確認は省略しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Items(string filename, string password)
        {
            var src = GetExamplesWith(filename);
            using (var archive = new ArchiveReader(src, password))
            {
                var actual   = archive.Items.ToList();
                var expected = Expect(filename).Keys.ToList();

                Assert.That(actual.Count, Is.EqualTo(expected.Count));

                for (var i = 0; i < expected.Count; ++i)
                {
                    var item = actual[i];
                    item.Refresh(); // NOP

                    Assert.That(item.Index,    Is.EqualTo(i));
                    Assert.That(item.FullName, Is.EqualTo(expected[i]));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 圧縮ファイルを展開するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract(string filename, string password) => IgnoreCultureError(() =>
        {
            var src  = GetExamplesWith(filename);
            var dest = GetResultsWith(nameof(Extract), filename);

            using (var archive = new ArchiveReader(src, password))
            {
                var bytes  = 0L;
                var report = new Progress<Report>(x => bytes = x.Bytes);
                archive.Extract(dest, report);
            }

            foreach (var kv in Expect(filename))
            {
                var fi = IO.Get(IO.Combine(dest, kv.Key));
                Assert.That(fi.Exists,         Is.True, kv.Key);
                Assert.That(fi.Length,         Is.EqualTo(kv.Value), kv.Key);
                Assert.That(fi.CreationTime,   Is.Not.EqualTo(DateTime.MinValue), kv.Key);
                Assert.That(fi.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue), kv.Key);
                Assert.That(fi.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue), kv.Key);
            }
        }, $"{filename}, {password}");

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each
        ///
        /// <summary>
        /// 圧縮ファイルの項目毎に展開するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract_Each(string filename, string password) => IgnoreCultureError(() =>
        {
            var src  = GetExamplesWith(filename);
            var dest = GetResultsWith(nameof(Extract_Each), filename);

            using (var archive = new ArchiveReader(src, password))
            {
                var actual   = archive.Items.ToList();
                var expected = Expect(filename);
                var keys     = expected.Keys.ToList();

                Assert.That(actual.Count, Is.EqualTo(expected.Count));

                for (var i = 0; i < keys.Count; ++i)
                {
                    actual[i].Extract(dest);

                    var key  = keys[i];
                    var info = IO.Get(IO.Combine(dest, key));

                    Assert.That(info.Exists,         Is.True, key);
                    Assert.That(info.Length,         Is.EqualTo(expected[key]), key);
                    Assert.That(info.CreationTime,   Is.Not.EqualTo(DateTime.MinValue), key);
                    Assert.That(info.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue), key);
                    Assert.That(info.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue), key);
                }
            }
        }, $"{filename}, {password}");

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Count
        ///
        /// <summary>
        /// 圧縮ファイルを展開するテストを実行します。
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
        [TestCase("Sample.rar",     ExpectedResult =  4)]
        [TestCase("Sample.rar5",    ExpectedResult =  4)]
        [TestCase("Sample.xlsx",    ExpectedResult = 14)]
        [TestCase("SampleEmpty.7z", ExpectedResult =  7)]
        [TestCase("SampleSfx.exe",  ExpectedResult =  4)]
        public int Extract_Count(string filename)
        {
            var src   = GetExamplesWith(filename);
            var count = Create();

            using (var archive = new ArchiveReader(src))
            {
                var path = GetResultsWith(nameof(Extract_Count), filename);
                archive.Extract(path, Create(count));
            }

            return count[ReportStatus.End];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Filter
        ///
        /// <summary>
        /// フィルタリング設定を行った時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Filter()
        {
            var src  = GetExamplesWith("SampleFilter.zip");
            var dest = GetResultsWith(nameof(Extract_Filter));

            using (var archive = new ArchiveReader(src))
            {
                archive.Filters = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
                archive.Extract(dest);
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用")),              Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\.DS_Store")),    Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\desktop.ini")),  Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\DS_Store.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\Thumbs.db")),    Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\__MACOSX")),     Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\フィルタリングされないファイル.txt")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_NotSupported
        ///
        /// <summary>
        /// 未対応のファイルが指定された時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_NotSupported() => Assert.That(
            () => new ArchiveReader(GetExamplesWith("Sample.txt")),
            Throws.TypeOf<NotSupportedException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_PermissionError
        ///
        /// <summary>
        /// 書き込みできないファイルを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_PermissionError() => Assert.That(() =>
        {
            var dir  = GetResultsWith(nameof(Extract_PermissionError));
            var dest = IO.Combine(dir, @"Sample\Foo.txt");

            IO.Copy(GetExamplesWith("Sample.txt"), dest);

            var io = new IO();
            io.Failed += (s, e) => throw new OperationCanceledException();

            using (var _ = io.OpenRead(dest))
            using (var archive = new ArchiveReader(GetExamplesWith("Sample.zip"), "", io))
            {
                archive.Extract(dir);
            }
        }, Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_MergeError
        ///
        /// <summary>
        /// 分割された圧縮ファイルの展開に失敗する時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_MergeError() => Assert.That(() =>
        {
            var dir = GetResultsWith(nameof(Extract_MergeError));
            for (var i = 1; i < 4; ++i)
            {
                var name = $"SampleVolume.rar.{i:000}";
                IO.Copy(GetExamplesWith(name), IO.Combine(dir, name));
            }

            using (var archive = new ArchiveReader(IO.Combine(dir, "SampleVolume.rar.001")))
            {
                archive.Extract(dir);
            }
        }, Throws.TypeOf<System.IO.IOException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_WrongPassword
        ///
        /// <summary>
        /// 暗号化されたファイルの展開に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        [TestCase("wrong")]
        public void Extract_WrongPassword(string password) => Assert.That(() =>
        {
            var src = GetExamplesWith("Password.7z");
            using (var archive = new ArchiveReader(src, password))
            {
                archive.Extract(Results);
            }
        }, Throws.TypeOf<EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each_WrongPassword
        ///
        /// <summary>
        /// 暗号化されたファイルの展開に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        public void Extract_Each_WrongPassword(string password) => Assert.That(() =>
        {
            var src = GetExamplesWith("Password.7z");
            using (var archive = new ArchiveReader(src, password))
            {
                foreach (var item in archive.Items) item.Extract(Results);
            }
        }, Throws.TypeOf<EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_UserCancel
        ///
        /// <summary>
        /// パスワード要求時にキャンセルするテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// 0 バイトのファイルはパスワード無しで展開が完了するため、
        /// Extracted イベントが 1 回発生します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_PasswordCancel()
        {
            var count = Create();

            Assert.That(() =>
            {
                var src   = GetExamplesWith("Password.7z");
                var query = new Query<string>(e => e.Cancel = true);

                using (var archive = new ArchiveReader(src, query))
                {
                    archive.Extract(Results, Create(count));
                }
            }, Throws.TypeOf<OperationCanceledException>());

            Assert.That(count[ReportStatus.End], Is.AtLeast(1));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each_PasswordCancel
        ///
        /// <summary>
        /// パスワード要求時にキャンセルするテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Each_PasswordCancel() => Assert.That(() =>
        {
            var src = GetExamplesWith("Password.7z");
            var query = new Query<string>(e => e.Cancel = true);
            using (var archive = new ArchiveReader(src, query))
            {
                foreach (var item in archive.Items) item.Extract(Results);
            }
        }, Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ArchiveItem の拡張メソッドのテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CreateDirectory()
        {
            var dest = GetResultsWith(nameof(CreateDirectory));
            using (var archive = new ArchiveReader(GetExamplesWith("Sample.zip")))
            {
                foreach (var item in archive.Items)
                {
                    item.CreateDirectory(dest);
                    item.SetAttributes(dest);
                }
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"Sample")),         Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Foo.txt")), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Bar.txt")), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Bas.txt")), Is.False);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Items および Extract_* のテスト用データを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("Sample.zip", "");
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
        /// Create
        ///
        /// <summary>
        /// Creates a new collection for ReportStatus.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDictionary<ReportStatus, int> Create() => new Dictionary<ReportStatus, int>
        {
            { ReportStatus.Begin,    0 },
            { ReportStatus.End,      0 },
            { ReportStatus.Progress, 0 },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the Progress(Report) class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IProgress<Report> Create(IDictionary<ReportStatus, int> src) =>
            new Progress<Report>(e => src[e.Status]++);

        /* ----------------------------------------------------------------- */
        ///
        /// Expect
        ///
        /// <summary>
        /// Creates the expected result corresponding to the specified
        /// filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IDictionary<string, long> Expect(string filename)
        {
            var dest = new Dictionary<string, long>();
            var path = GetExamplesWith("Expected", $"{filename}.txt");
            var csv  = new TextFieldParser(path, System.Text.Encoding.UTF8)
            {
                Delimiters                = new[] { "," },
                HasFieldsEnclosedInQuotes = true,
                TextFieldType             = FieldType.Delimited,
                TrimWhiteSpace            = true,
            };

            while (!csv.EndOfData)
            {
                var row = csv.ReadFields();
                dest.Add(row[0], long.Parse(row[1]));
            }

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IgnoreCultureError
        ///
        /// <summary>
        /// ロケールが日本語以外の環境で失敗するテストに関しては、現時点
        /// では無視しています。将来的には CodePage を指定可能な形に修正
        /// する事で対応する予定です。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void IgnoreCultureError(Action action, string message)
        {
            try { action(); }
            catch (EncryptionException)
            {
                var code = CultureInfo.CurrentCulture.Name;
                var option = StringComparison.InvariantCultureIgnoreCase;
                if (!string.Equals(code, "ja-JP", option)) Assert.Ignore(message);
                else throw;
            }
        }

        #endregion
    }
}
