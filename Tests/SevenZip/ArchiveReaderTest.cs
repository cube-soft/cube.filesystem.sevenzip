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
        [TestCaseSource(nameof(Items_TestCases))]
        public void Items(string filename, string password, IList<ExpectedItem> expected)
        {
            using (var archive = new SevenZip.ArchiveReader())
            {
                archive.Open(Example(filename), password);

                var actual = archive.Items.ToList();
                Assert.That(actual.Count, Is.EqualTo(expected.Count));

                for (var i = 0; i < expected.Count; ++i)
                {
                    Assert.That(actual[i].Index,       Is.EqualTo(i));
                    Assert.That(actual[i].Path,        Is.EqualTo(expected[i].Path));
                    Assert.That(actual[i].Password,    Is.EqualTo(password));
                    Assert.That(actual[i].Size,        Is.EqualTo(expected[i].Size));
                    Assert.That(actual[i].IsDirectory, Is.EqualTo(expected[i].IsDirectory));

                    actual[i].Save(Results);
                    var dest = Result(actual[i].Path);
                    var dir  = expected[i].IsDirectory;
                    Assert.That(Exists(dest, dir), Is.True);
                    Assert.That(Length(dest, dir), Is.EqualTo(expected[i].Size));
                }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Items_TestCases
        ///
        /// <summary>
        /// Items のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static IEnumerable<TestCaseData> Items_TestCases
        {
            get
            {
                yield return new TestCaseData("Sample.zip", string.Empty, new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        Path        = "Sample",
                        Size        = 0,
                        IsDirectory = true,
                    },

                    new ExpectedItem
                    {
                        Path        = @"Sample\Bar.txt",
                        Size        = 7816,
                        IsDirectory = false,
                    },

                    new ExpectedItem
                    {
                        Path        = @"Sample\Bas.txt",
                        Size        = 0,
                        IsDirectory = false,
                    },

                    new ExpectedItem
                    {
                        Path        = @"Sample\Foo.txt",
                        Size        = 3,
                        IsDirectory = false,
                    },
                });

                yield return new TestCaseData("Password.7z", "password", new List<ExpectedItem>
                {
                    new ExpectedItem
                    {
                        Path        = "Password",
                        Size        = 0,
                        IsDirectory = true,
                    },

                    new ExpectedItem
                    {
                        Path        = @"Password\Second.txt",
                        Size        = 0,
                        IsDirectory = false,
                    },

                    new ExpectedItem
                    {
                        Path        = @"Password\First.txt",
                        Size        = 26,
                        IsDirectory = false,
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
        public class ExpectedItem
        {
            public string Path { get; set; }
            public ulong Size { get; set; }
            public bool IsDirectory { get; set; }
        }

        #endregion
    }
}
