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
using Cube.FileSystem.SevenZip;
using Cube.FileSystem.SevenZip.Archives;
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
    [TestFixture]
    class ArchiveReaderTest : FileHelper
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
            using (var archive = new ArchiveReader(src, password))
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
                    Assert.That(item.Encrypted,   Is.EqualTo(expected[i].Encrypted));
                    Assert.That(item.IsDirectory, Is.EqualTo(expected[i].IsDirectory));
                    if (expected[i].Length >= 0) Assert.That(item.Length, Is.EqualTo(expected[i].Length));
                }
            }
        }

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
            var dest = Result("CreateDirectory");
            using (var archive = new ArchiveReader(Example("Sample.zip")))
            {
                foreach (var item in archive.Items)
                {
                    item.CreateDirectory(dest);
                    item.SetAttributes(dest);
                }
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"Sample")),         Is.True );
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Foo.txt")), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Bar.txt")), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Bas.txt")), Is.False);
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
        [TestCase("Sample.arj",     ExpectedResult =  3)]
        [TestCase("Sample.cab",     ExpectedResult =  3)]
        [TestCase("Sample.chm",     ExpectedResult = 80)]
        [TestCase("Sample.cpio",    ExpectedResult =  3)]
        [TestCase("Sample.docx",    ExpectedResult = 13)]
        [TestCase("Sample.flv",     ExpectedResult =  2)]
        [TestCase("Sample.jar",     ExpectedResult =  3)]
        [TestCase("Sample.lha",     ExpectedResult =  3)]
        [TestCase("Sample.lzh",     ExpectedResult =  3)]
        [TestCase("Sample.nupkg",   ExpectedResult =  5)]
        [TestCase("Sample.pptx",    ExpectedResult = 40)]
        [TestCase("Sample.rar",     ExpectedResult =  3)]
        [TestCase("Sample.rar5",    ExpectedResult =  3)]
        [TestCase("Sample.rar.001", ExpectedResult =  3)]
        [TestCase("Sample.tar",     ExpectedResult =  3)]
        [TestCase("Sample.xlsx",    ExpectedResult = 14)]
        public int Extract(string filename)
        {
            var src = Example(filename);
            var count = 0;

            using (var archive = new ArchiveReader(src))
            {
                var dest = Result($@"Extract\{filename}");
                archive.Extracted += (s, e) => ++count;
                archive.Extract(dest);
            }

            return count;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Detail
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
        public void Extract_Detail(string filename, string password, IList<ExpectedItem> expected)
        {
            var src      = Example(filename);
            var bytes    = 0L;
            var progress = new Progress<ArchiveReport>(x => bytes = x.Bytes);

            using (var archive = new ArchiveReader(src, password))
            {
                var dest = Result($@"Extract_Detail\{filename}");
                archive.Extract(dest, progress);

                foreach (var item in expected)
                {
                    var info = IO.Get(IO.Combine(dest, item.FullName));
                    Assert.That(info.Exists,         Is.True);
                    Assert.That(info.CreationTime,   Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue));
                    if (item.Length >= 0) Assert.That(info.Length, Is.EqualTo(item.Length));
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
            using (var archive = new ArchiveReader(src, password))
            {
                var dest = Result($@"Extract_Each\{filename}");
                var actual = archive.Items.ToList();
                for (var i = 0; i < expected.Count; ++i)
                {
                    actual[i].Extract(dest);

                    var info = IO.Get(IO.Combine(dest, actual[i].FullName));
                    Assert.That(info.Exists,         Is.True);
                    Assert.That(info.CreationTime,   Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastWriteTime,  Is.Not.EqualTo(DateTime.MinValue));
                    Assert.That(info.LastAccessTime, Is.Not.EqualTo(DateTime.MinValue));
                    if (expected[i].Length >= 0) Assert.That(info.Length, Is.EqualTo(expected[i].Length));
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
                () => new ArchiveReader(Example("Sample.txt")),
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

            using (var archive = new ArchiveReader(src))
            {
                archive.Extract(dest);
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"NUL")),                Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL")),               Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_CON\_CON.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_CON\_AUX.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_CON\_PRN.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_CON\abf.txt")),  Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_AUX\_CON.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_AUX\_AUX.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_AUX\_PRN.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\_AUX\abf.txt")),  Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\abf\_CON.txt")),  Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\abf\_AUX.txt")),  Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\abf\_PRN.txt")),  Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_NUL\abf\abf.txt")),   Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_CON")),               Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"_CON.txt")),           Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"CON.txt")),            Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"abf")),                Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"abf.txt")),            Is.True);
        }

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
        public void Extract_PermissionError()
            => Assert.That(() =>
            {
                var dir  = Result("PermissionError");
                var dest = IO.Combine(dir, @"Sample\Foo.txt");

                IO.Copy(Example("Sample.txt"), dest);

                var io = new Operator();
                io.Failed += (s, e) => throw new OperationCanceledException();

                using (var _ = io.OpenRead(dest))
                using (var archive = new ArchiveReader(Example("Sample.zip"), "", io))
                {
                    archive.Extract(dir);
                }
            },
            Throws.TypeOf<OperationCanceledException>());

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
        public void Extract_MergeError()
            => Assert.That(() =>
            {
                var dir = Result("MergeError");
                for (var i = 1; i < 4; ++i)
                {
                    var name = $"Sample.rar.{i:000}";
                    IO.Copy(Example(name), IO.Combine(dir, name));
                }

                using (var archive = new ArchiveReader(IO.Combine(dir, "Sample.rar.001")))
                {
                    archive.Extract(dir);
                }
            },
            Throws.TypeOf<System.IO.IOException>());

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
        public void Extract_WrongPassword(string password)
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                using (var archive = new ArchiveReader(src, password))
                {
                    archive.Extract(Results);
                }
            },
            Throws.TypeOf<EncryptionException>());

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
        public void Extract_Each_WrongPassword(string password)
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                using (var archive = new ArchiveReader(src, password))
                {
                    foreach (var item in archive.Items) item.Extract(Results);
                }
            },
            Throws.TypeOf<EncryptionException>());

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
            var count = 0;

            Assert.That(() =>
            {
                var src   = Example("Password.7z");
                var query = new Query<string, string>(e => e.Cancel = true);
                using (var archive = new ArchiveReader(src, query))
                {
                    archive.Extracted += (s, e) => ++count;
                    archive.Extract(Results);
                }
            }, Throws.TypeOf<OperationCanceledException>());

            Assert.That(count, Is.EqualTo(1));
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
        public void Extract_Each_PasswordCancel()
            => Assert.That(() =>
            {
                var src = Example("Password.7z");
                var query = new Query<string, string>(e => e.Cancel = true);
                using (var archive = new ArchiveReader(src, query))
                {
                    foreach (var item in archive.Items) item.Extract(Results);
                }
            },
            Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extracting_Throws
        ///
        /// <summary>
        /// Extracting イベントで例外を送出した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extracting_Throws()
            => Assert.That(() =>
            {
                var src = Example("Sample.zip");
                using (var archive = new ArchiveReader(src))
                {
                    archive.Extracting += (s, e) => throw new ArgumentException();
                    archive.Extract(Results);
                }
            },
            Throws.TypeOf<System.IO.IOException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extracted_Throws
        ///
        /// <summary>
        /// Extracted イベントで例外を送出した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extraced_Throws()
            => Assert.That(() =>
            {
                var src = Example("Sample.zip");
                using (var archive = new ArchiveReader(src))
                {
                    archive.Extracted += (s, e) => throw new OperationCanceledException();
                    archive.Extract(Results);
                }
            },
            Throws.TypeOf<OperationCanceledException>());

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

                yield return new TestCaseData("MultiVolume.zip", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Split",
                        Extension     = string.Empty,
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = true,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Split\Manual.pdf",
                        Extension     = ".pdf",
                        Length        = 990040,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Split\Sample1.txt",
                        Extension     = ".txt",
                        Length        = 5,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Split\Sample2.txt",
                        Extension     = ".txt",
                        Length        = 6,
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

                yield return new TestCaseData("Sample.tar.bz2", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.tar.gz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.tar.lzma", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.tar.z", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.taz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.tb2", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.tbz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.tgz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.txz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.tar",
                        Extension     = ".tar",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.txt.bz2", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.txt",
                        Extension     = ".txt",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.txt.gz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Filter.txt",
                        Extension     = ".txt",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });

                yield return new TestCaseData("Sample.txt.xz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Sample.txt",
                        Extension     = ".txt",
                        Length        = -1,
                        Encrypted     = false,
                        IsDirectory   = false,
                    },
                });
            }
        }

        #endregion

        #region Helper

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
