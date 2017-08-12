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
        [TestCase("Empty.txt",     ExpectedResult = Format.Unknown)]
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
            return Formats.FromFile(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromFile_NotFound
        ///
        /// <summary>
        /// 存在しないファイルを指定した時の挙動を確認します。
        /// </summary>
        /// 
        /// <remarks>
        /// ファイルが存在しなかった場合、拡張子から判断します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void FromFile_NotFound()
            => Assert.That(
                Formats.FromFile(Example("NotFound.rar")),
                Is.EqualTo(Format.Rar)
            );

        /* ----------------------------------------------------------------- */
        ///
        /// ToClassId
        ///
        /// <summary>
        /// Class ID に変換するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,     ExpectedResult = "23170f69-40c1-278a-1000-000110010000")]
        [TestCase(Format.Unknown, ExpectedResult = "00000000-0000-0000-0000-000000000000")]
        public string ToClassId(Format format)
            => format.ToClassId().ToString("D").ToLower();

        /* ----------------------------------------------------------------- */
        ///
        /// ToExtension
        ///
        /// <summary>
        /// 拡張子に変換するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      ExpectedResult = ".zip")]
        [TestCase(Format.SevenZip, ExpectedResult = ".7z")]
        [TestCase(Format.BZip2,    ExpectedResult = ".bz2")]
        [TestCase(Format.GZip,     ExpectedResult = ".gz")]
        [TestCase(Format.Lzw,      ExpectedResult = ".z")]
        [TestCase(Format.Sfx,      ExpectedResult = ".exe")]
        [TestCase(Format.Unknown,  ExpectedResult = "")]
        public string ToExtension(Format format)
            => format.ToExtension();

        #endregion
    }
}
