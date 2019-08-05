/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Tests;
using NUnit.Framework;
using System;

namespace Cube.FileSystem.SevenZip.Tests
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
    [TestFixture]
    class FormatTest : FileFixture
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
        [TestCase("Sample.txt",      ExpectedResult = Format.Unknown)]
        [TestCase("Empty.txt",       ExpectedResult = Format.Unknown)]
        [TestCase("Password.7z",     ExpectedResult = Format.SevenZip)]
        [TestCase("Sample.cab",      ExpectedResult = Format.Cab)]
        [TestCase("Sample.chm",      ExpectedResult = Format.Chm)]
        [TestCase("Sample.docx",     ExpectedResult = Format.Zip)]
        [TestCase("Sample.exe",      ExpectedResult = Format.PE)]
        [TestCase("Sample.flv",      ExpectedResult = Format.Flv)]
        [TestCase("Sample.jar",      ExpectedResult = Format.Zip)]
        [TestCase("Sample.lha",      ExpectedResult = Format.Lzh)]
        [TestCase("Sample.lzh",      ExpectedResult = Format.Lzh)]
        [TestCase("Sample.nupkg",    ExpectedResult = Format.Zip)]
        [TestCase("Sample.pptx",     ExpectedResult = Format.Zip)]
        [TestCase("Sample.rar",      ExpectedResult = Format.Rar)]
        [TestCase("Sample.rar5",     ExpectedResult = Format.Rar5)]
        [TestCase("Sample.tar",      ExpectedResult = Format.Tar)]
        [TestCase("Sample.tar.z",    ExpectedResult = Format.Lzw)]
        [TestCase("Sample.tbz",      ExpectedResult = Format.BZip2)]
        [TestCase("Sample.tgz",      ExpectedResult = Format.GZip)]
        [TestCase("Sample.txz",      ExpectedResult = Format.XZ)]
        [TestCase("Sample.xlsx",     ExpectedResult = Format.Zip)]
        [TestCase("Sample.zip",      ExpectedResult = Format.Zip)]
        [TestCase("SampleSfx.exe",   ExpectedResult = Format.Sfx)]
        public Format Detect(string filename)
        {
            var src  = GetSource(filename);
            var dest = Get(Guid.NewGuid().ToString("D"));
            IO.Copy(src, dest);
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
        public void FromFile_NotFound()=> Assert.That(
            Formats.FromFile(GetSource("NotFound.rar")),
            Is.EqualTo(Format.Rar)
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FromStream_CannotRead
        ///
        /// <summary>
        /// 書き込み専用のストリームを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void FromStream_CannotRead() => Assert.That(
            () => Formats.FromStream(IO.OpenWrite(GetSource("Sample.zip"))),
            Throws.TypeOf<NotSupportedException>()
        );

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
        public string ToExtension(Format format) => format.ToExtension();

        /* ----------------------------------------------------------------- */
        ///
        /// ToFormat
        ///
        /// <summary>
        /// CompressionMethod から Format に変換するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(CompressionMethod.GZip, ExpectedResult = Format.GZip)]
        [TestCase(CompressionMethod.Lzma, ExpectedResult = Format.Unknown)]
        public Format ToFormat(CompressionMethod method) => method.ToFormat();

        /* ----------------------------------------------------------------- */
        ///
        /// ToMethod
        ///
        /// <summary>
        /// Format から CompressionMethod に変換するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(Format.BZip2, ExpectedResult = CompressionMethod.BZip2)]
        [TestCase(Format.Zip,   ExpectedResult = CompressionMethod.Default)]
        public CompressionMethod ToMethod(Format format) => format.ToMethod();

        #endregion
    }
}
