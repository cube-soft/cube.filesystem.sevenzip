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
        [TestCaseSource(nameof(Extract_TestCases))]
        public void Items(string filename, string password, IList<ExpectedItem> expected)
        {
            var src = Example(filename);
            using (var archive = new SevenZip.ArchiveReader(src, password))
            {
                var actual = archive.Items.ToList();
                Assert.That(actual.Count, Is.EqualTo(expected.Count));

                for (var i = 0; i < expected.Count; ++i)
                {
                    Assert.That(actual[i].Index,          Is.EqualTo(i));
                    Assert.That(actual[i].FullName,       Is.EqualTo(expected[i].FullName));
                    Assert.That(actual[i].Extension,      Is.EqualTo(expected[i].Extension));
                    Assert.That(actual[i].Length,         Is.EqualTo(expected[i].Length));
                    Assert.That(actual[i].Encrypted,      Is.EqualTo(expected[i].Encrypted));
                    Assert.That(actual[i].IsDirectory,    Is.EqualTo(expected[i].IsDirectory));
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
        [TestCaseSource(nameof(Extract_TestCases))]
        public void Extract(string filename, string password, IList<ExpectedItem> expected)
        {
            var src = Example(filename);
            using (var archive = new SevenZip.ArchiveReader(src, password))
            {
                var actual = archive.Items.ToList();
                for (var i = 0; i < expected.Count; ++i)
                {
                    actual[i].Extract(Results);
                    var dest = Result(actual[i].FullName);
                    var dir  = expected[i].IsDirectory;
                    Assert.That(Exists(dest, dir), Is.True);
                    Assert.That(Length(dest, dir), Is.EqualTo(expected[i].Length));
                }
            }
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
                    foreach (var item in archive.Items) item.Extract(Results);
                }
            },
            Throws.TypeOf<SevenZip.UserCancelException>());

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_TestCases
        ///
        /// <summary>
        /// Items および Extract のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> Extract_TestCases
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

                yield return new TestCaseData("Sample.tar.gz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Sample.tar",
                        Extension     = ".tar",
                        Length        = 20480,
                        Encrypted     = false,
                        IsDirectory   = false,
                    }
                });

                yield return new TestCaseData("Sample.tar.xz", "", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = "Sample.tar",
                        Extension     = ".tar",
                        Length        = 20480,
                        Encrypted     = false,
                        IsDirectory   = false,
                    }
                });
            }
        }

        #endregion

        #region Helper classes and methods

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// ファイルまたはディレクトリが存在するかどうかを判別します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private bool Exists(string path, bool directory)
            => directory ?
               System.IO.Directory.Exists(path) :
               System.IO.File.Exists(path);

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// ファイルサイズを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private long Length(string path, bool directory)
            => directory ?
               0 :
               new System.IO.FileInfo(path).Length;

        /* ----------------------------------------------------------------- */
        ///
        /// ExpectedItem
        ///
        /// <summary>
        /// 展開後ファイルの期待値を格納するためのクラスです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public class ExpectedItem : IInformation
        {
            public bool Exists { get; set; }
            public bool IsDirectory { get; set; }
            public bool Encrypted { get; set; }
            public string Name { get; set; }
            public string NameWithoutExtension { get; set; }
            public string Extension { get; set; }
            public string DirectoryName { get; set; }
            public string FullName { get; set; }
            public long Length { get; set; }
            public System.IO.FileAttributes Attributes { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime LastWriteTime { get; set; }
            public DateTime LastAccessTime { get; set; }
            public void Refresh() { }
        }

        #endregion
    }
}
