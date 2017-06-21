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
        /// Save
        ///
        /// <summary>
        /// 圧縮ファイルを作成するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Save()
        {
            var dest = Result("ArchiveWriter.zip");

            using (var writer = new SevenZip.ArchiveWriter(SevenZip.Format.Zip))
            {
                writer.Items.Add(GetFileInfo("Sample.txt"));
                writer.Items.Add(GetFileInfo("Empty.txt"));
                writer.Save(dest, string.Empty);
            }

            Assert.That(System.IO.File.Exists(dest), Is.True);
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileInfo
        ///
        /// <summary>
        /// 指定されたファイル名に対応する FileInfo オブジェクトを
        /// 取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private System.IO.FileInfo GetFileInfo(string filename)
            => new System.IO.FileInfo(Example(filename));

        #endregion
    }
}
