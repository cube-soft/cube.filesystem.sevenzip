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
using Cube.FileSystem.SevenZip;
using NUnit.Framework;

namespace Cube.FileSystem.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FormatTest
    /// 
    /// <summary>
    /// Format に関わる機能のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class FormatTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Detect
        ///
        /// <summary>
        /// 圧縮ファイル形式を判別するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.txt",    ExpectedResult = Format.Unknown)]
        [TestCase("Sample.zip",    ExpectedResult = Format.Zip)]
        [TestCase("Sample.tar",    ExpectedResult = Format.Tar)]
        [TestCase("Sample.tar.gz", ExpectedResult = Format.GZip)]
        [TestCase("Sample.tar.bz", ExpectedResult = Format.BZip2)]
        [TestCase("Password.7z",   ExpectedResult = Format.SevenZip)]
        public Format Detect(string filename)
        {
            var src  = Example(filename);
            var dest = Result(Guid.NewGuid().ToString("D"));
            System.IO.File.Copy(src, dest);
            return FormatConversions.FromFile(dest);
        }

        #endregion
    }
}
