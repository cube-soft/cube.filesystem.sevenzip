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
using System.Linq;
using NUnit.Framework;

namespace Cube.FileSystem.Tests
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
    [Parallelizable]
    [TestFixture]
    class ArchiveReaderTest : FileResource
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
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Items(string filename, string password, IList<ExpectedItem> expected)
        {
            var src = Example(filename);
            using (var archive = new SevenZip.ArchiveReader(src, password))
            {
                var actual = archive.Items.ToList();
                Assert.That(actual.Count, Is.EqualTo(expected.Count));

                for (var i = 0; i < expected.Count; ++i)
                {
                    var item = actual[i];
                    item.Refresh(); // NOP

                    Assert.That(item.Index,       Is.EqualTo(i));
                    Assert.That(item.FullName,    Is.EqualTo(expected[i].FullName));
                    Assert.That(item.Extension,   Is.EqualTo(expected[i].Extension));
                    Assert.That(item.Length,      Is.EqualTo(expected[i].Length));
                    Assert.That(item.Encrypted,   Is.EqualTo(expected[i].Encrypted));
                    Assert.That(item.IsDirectory, Is.EqualTo(expected[i].IsDirectory));
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
        [TestCase("Sample.arj")]
        [TestCase("Sample.cab")]
        [TestCase("Sample.chm")]
        [TestCase("Sample.cpio")]
        [TestCase("Sample.docx")]
        [TestCase("Sample.flv")]
        [TestCase("Sample.jar")]
        [TestCase("Sample.lha")]
        [TestCase("Sample.lzh")]
        [TestCase("Sample.nupkg")]
        [TestCase("Sample.pptx")]
        [TestCase("Sample.rar")]
        [TestCase("Sample.rar5")]
        [TestCase("Sample.tar")]
        [TestCase("Sample.tar.bz2")]
        [TestCase("Sample.tar.gz")]
        [TestCase("Sample.tar.lzma")]
        [TestCase("Sample.tar.xz")]
        [TestCase("Sample.tar.z")]
        [TestCase("Sample.xlsx")]
        public void Extract(string filename)
        {
            var src = Example(filename);
            using (var archive = new SevenZip.ArchiveReader(src))
            {
                var dest = Result($@"Extract\{filename}");
                archive.Extract(dest);

                foreach (var item in archive.Items)
                {
                    var info = IO.Get(IO.Combine(dest, item.FullName));
                    Assert.That(info.Exists, Is.True, info.FullName);
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
        /// <remarks>
        /// 展開された項目に対しても確認します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public void Extract(string filename, string password, IList<ExpectedItem> expected)
        {
            var src = Example(filename);
            using (var archive = new SevenZip.ArchiveReader(src, password))
            {
                var dest = Result($@"Extract_Detail\{filename}");
                archive.Extract(dest);

                foreach (var item in expected)
                {
                    var info = IO.Get(IO.Combine(dest, item.FullName));
                    Assert.That(info.Exists,         Is.True);
                    Assert.That(info.Length,         Is.EqualTo(item.Length));
                    Assert.That(info.CreationTime,   Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue));
                }
            }
        }

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
        public void Extract_Each(string filename, string password, IList<ExpectedItem> expected)
        {
            var src = Example(filename);
            using (var archive = new SevenZip.ArchiveReader(src, password))
            {
                var dest = Result($@"Extract_Each\{filename}");
                var actual = archive.Items.ToList();
                for (var i = 0; i < expected.Count; ++i)
                {
                    actual[i].Extract(dest);

                    var info = IO.Get(IO.Combine(dest, actual[i].FullName));
                    Assert.That(info.Exists,         Is.True);
                    Assert.That(info.Length,         Is.EqualTo(expected[i].Length));
                    Assert.That(info.CreationTime,   Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue));
                }
            }
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
        public void Extract_NotSupported()
            => Assert.That(
                () => new SevenZip.ArchiveReader(Example("Sample.txt")),
                Throws.TypeOf<NotSupportedException>()
            );

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
            var src  = Example("SampleFilter.zip");
            var dest = Result("Extract_Filter");

            using (var archive = new SevenZip.ArchiveReader(src))
            {
                archive.Filters = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
                archive.Extract(dest);
            }

            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用")).Exists,              Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用\.DS_Store")).Exists,    Is.False);
            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用\desktop.ini")).Exists,  Is.False);
            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用\DS_Store.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用\Thumbs.db")).Exists,    Is.False);
            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用\__MACOSX")).Exists,     Is.False);
            Assert.That(IO.Get(IO.Combine(dest, @"フィルタリング テスト用\フィルタリングされないファイル.txt")).Exists, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Reserved
        ///
        /// <summary>
        /// 圧縮ファイルに予約文字のファイル名およびディレクトリ名が
        /// 含まれる場合の展開テストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Reserved()
        {
            var src  = Example("InvalidReserved.zip");
            var dest = Result("Extract_Reserved");

            using (var archive = new SevenZip.ArchiveReader(src))
            {
                archive.Extract(dest);
            }

            Assert.That(IO.Get(IO.Combine(dest, @"NUL")).Exists,                Is.False);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL")).Exists,               Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_CON\_CON.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_CON\_AUX.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_CON\_PRN.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_CON\abf.txt")).Exists,  Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_AUX\_CON.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_AUX\_AUX.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_AUX\_PRN.txt")).Exists, Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\_AUX\abf.txt")).Exists,  Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\abf\_CON.txt")).Exists,  Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\abf\_AUX.txt")).Exists,  Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\abf\_PRN.txt")).Exists,  Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_NUL\abf\abf.txt")).Exists,   Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_CON")).Exists,               Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"_CON.txt")).Exists,           Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"CON.txt")).Exists,            Is.False);
            Assert.That(IO.Get(IO.Combine(dest, @"abf")).Exists,                Is.True);
            Assert.That(IO.Get(IO.Combine(dest, @"abf.txt")).Exists,            Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Throws
        ///
        /// <summary>
        /// 暗号化されたファイルの展開に失敗するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        [TestCase("wrong")]
        public void Extract_Throws(string password)
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                using (var archive = new SevenZip.ArchiveReader(src, password))
                {
                    archive.Extract(Results);
                }
            },
            Throws.TypeOf<SevenZip.EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each_Throws
        ///
        /// <summary>
        /// 暗号化されたファイルの展開に失敗するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        public void Extract_Each_Throws(string password)
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                using (var archive = new SevenZip.ArchiveReader(src, password))
                {
                    foreach (var item in archive.Items) item.Extract(Results);
                }
            },
            Throws.TypeOf<SevenZip.EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_UserCancel
        ///
        /// <summary>
        /// パスワード要求時にキャンセルするテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_UserCancel()
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                var query = new Query<string, string>(e => e.Cancel = true);
                using (var archive = new SevenZip.ArchiveReader(src, query))
                {
                    archive.Extract(Results);
                }
            },
            Throws.TypeOf<SevenZip.UserCancelException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each_UserCancel
        ///
        /// <summary>
        /// パスワード要求時にキャンセルするテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Each_UserCancel()
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                var query = new Query<string, string>(e => e.Cancel = true);
                using (var archive = new SevenZip.ArchiveReader(src, query))
                {
                    foreach (var item in archive.Items) item.Extract(Results);
                }
            },
            Throws.TypeOf<SevenZip.UserCancelException>());

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Items および Extract のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("Sample.zip", string.Empty, new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Sample",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Sample\Bar.txt",
                        Extension     = ".txt",
                        Length        = 7816,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Sample\Bas.txt",
                        Extension     = ".txt",
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Sample\Foo.txt",
                        Extension     = ".txt",
                        Length        = 3,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Password.7z", "password", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Password",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Password\Second.txt",
                        Extension     = ".txt",
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Password\First.txt",
                        Extension     = ".txt",
                        Length        = 26,
                        Encrypted     = true,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("PasswordHeader.7z", "password", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Password",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Password\Second.txt",
                        Extension     = ".txt",
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Password\First.txt",
                        Extension     = ".txt",
                        Length        = 26,
                        Encrypted     = true,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("PasswordSymbol01.zip", "()[]{}<>", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Symbol.txt",
                        Extension     = ".txt",
                        Length        = 22,
                        Encrypted     = true,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("PasswordSymbol02.zip", "\\#$%@?", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Symbol.txt",
                        Extension     = ".txt",
                        Length        = 22,
                        Encrypted     = true,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("PasswordSymbol03.zip", "!&|+-*/=", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Symbol.txt",
                        Extension     = ".txt",
                        Length        = 22,
                        Encrypted     = true,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("PasswordSymbol04.zip", "\"'^~`,._", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Symbol.txt",
                        Extension     = ".txt",
                        Length        = 22,
                        Encrypted     = true,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("PasswordJapanese01.zip", "日本語パスワード", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Japanese.txt",
                        Extension     = ".txt",
                        Length        = 39,
                        Encrypted     = true,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("PasswordJapanese02.zip", "ｶﾞｷﾞｸﾞｹﾞｺﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Japanese.txt",
                        Extension     = ".txt",
                        Length        = 39,
                        Encrypted     = true,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("SampleComma.zip", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"カンマ",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"カンマ\hello, world.txt",
                        Extension     = ".txt",
                        Length        = 4,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"カンマ\test,テスト,ﾃｽﾄ.txt",
                        Extension     = ".txt",
                        Length        = 4,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("SampleMac.zip", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\がぎぐげご.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._がぎぐげご.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\ぱぴぷぺぽ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._ぱぴぷぺぽ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\ガギグゲゴ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._ガギグゲゴ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\カタログ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._カタログ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\パピプペポ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._パピプペポ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\ｶﾀﾛｸﾞ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._ｶﾀﾛｸﾞ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\ｶﾞｷﾞｸﾞｹﾞｺﾞ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._ｶﾞｷﾞｸﾞｹﾞｺﾞ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"名称未設定フォルダ\ﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"__MACOSX\名称未設定フォルダ\._ﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ.txt",
                        Extension     = ".txt",
                        Length        = 171,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
               });

                yield return new TestCaseData("InvalidSymbol.zip", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"test\_foo_bar_buzz_.txt",
                        Extension     = ".txt",
                        Length        = 5,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"test\test(2012_05_07).txt",
                        Extension     = ".txt",
                        Length        = 5,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });
            }
        }

        #endregion

        #region Helper class

        /* ----------------------------------------------------------------- */
        ///
        /// ExpectedItem
        ///
        /// <summary>
        /// 展開後ファイルの期待値を格納するためのクラスです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public class ExpectedItem // : IInformation
        {
            public bool IsDirectory { get; set; }
            public bool Encrypted { get; set; }
            public string Extension { get; set; }
            public string FullName { get; set; }
            public long Length { get; set; }
            // public bool Exists { get; set; }
            // public string Name { get; set; }
            // public string NameWithoutExtension { get; set; }
            // public string DirectoryName { get; set; }
            // public System.IO.FileAttributes Attributes { get; set; }
            // public DateTime CreationTime { get; set; }
            // public DateTime LastWriteTime { get; set; }
            // public DateTime LastAccessTime { get; set; }
            // public void Refresh() { }
        }

        #endregion
    }
}
