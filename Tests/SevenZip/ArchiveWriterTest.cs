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
        public void Archive(string filename, long size, string[] items)
        {
            var dest = Result(filename);

            using (var writer = new SevenZip.ArchiveWriter(SevenZip.Format.Zip))
            {
                foreach (var item in items) writer.Add(Example(item));
                writer.Save(dest, string.Empty);
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
                yield return new TestCaseData("SingleFile.zip", 167L, new[]
                {
                    "Sample.txt",
                });

                yield return new TestCaseData("SingleDirectory.zip", 488L, new[]
                {
                    "Archive",
                });

                yield return new TestCaseData("FileAndDirectory.zip", 633L, new[]
                {
                    "Sample.txt",
                    "Archive",
                });
            }
        }

        #endregion
    }
}
