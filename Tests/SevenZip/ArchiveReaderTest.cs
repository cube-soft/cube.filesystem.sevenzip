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
                    Assert.That(actual[i].Index,         Is.EqualTo(i));
                    Assert.That(actual[i].FullName,      Is.EqualTo(expected[i].FullName));
                    Assert.That(actual[i].Extension,     Is.EqualTo(expected[i].Extension));
                    Assert.That(actual[i].Length,        Is.EqualTo(expected[i].Length));
                    Assert.That(actual[i].Encrypted,     Is.EqualTo(expected[i].Encrypted));
                    Assert.That(actual[i].IsDirectory,   Is.EqualTo(expected[i].IsDirectory));
                    Assert.That(actual[i].CreationTime,  Is.EqualTo(expected[i].CreationTime.ToLocalTime()));
                    Assert.That(actual[i].LastWriteTime, Is.EqualTo(expected[i].LastWriteTime.ToLocalTime()));
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
                        CreationTime  = new DateTime(636335461672026312L, DateTimeKind.Utc),
                        LastWriteTime = new DateTime(636335461673382389L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Sample\Bar.txt",
                        Extension     = ".txt",
                        Length        = 7816,
                        Encrypted     = false,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(636329192793637655L, DateTimeKind.Utc),
                        LastWriteTime = new DateTime(636329193340933256L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Sample\Bas.txt",
                        Extension     = ".txt",
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(636329193392193901L, DateTimeKind.Utc),
                        LastWriteTime = new DateTime(636329193392193901L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Sample\Foo.txt",
                        Extension     = ".txt",
                        Length        = 3,
                        Encrypted     = false,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(636329192793637655L, DateTimeKind.Utc),
                        LastWriteTime = new DateTime(636329192889544731L, DateTimeKind.Utc),
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
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636335462377258383L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Password\Second.txt",
                        Extension     = ".txt",
                        Length        = 0,
                        Encrypted     = false,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636335460490216213L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Password\First.txt",
                        Extension     = ".txt",
                        Length        = 26,
                        Encrypted     = true,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636335458772834750L, DateTimeKind.Utc),
                    },
                });

                yield return new TestCaseData("PasswordSymbol.zip", "!\"#$%&'()-=^~\\|@`[]{}+*<>,./?_", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Symbol1.txt",
                        Extension     = ".txt",
                        Length        = 7816,
                        Encrypted     = true,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636353593020000000L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Symbol2.txt",
                        Extension     = ".txt",
                        Length        = 6,
                        Encrypted     = true,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636353593220000000L, DateTimeKind.Utc),
                    },
                });

                yield return new TestCaseData("PasswordJapanese.zip", "日本語パスワード", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        FullName      = @"Japanese1.txt",
                        Extension     = ".txt",
                        Length        = 22187,
                        Encrypted     = true,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636353615100000000L, DateTimeKind.Utc),
                    },

                    new ExpectedItem
                    {
                        FullName      = @"Japanese2.txt",
                        Extension     = ".txt",
                        Length        = 21,
                        Encrypted     = true,
                        IsDirectory   = false,
                        CreationTime  = new DateTime(0L, DateTimeKind.Local),
                        LastWriteTime = new DateTime(636353615360000000L, DateTimeKind.Utc),
                    },
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
