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
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewResourceTest
    /// 
    /// <summary>
    /// ViewResource のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ViewResourceTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        /// 
        /// <summary>
        /// Format で指定可能な種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(4)]
        public void Formats(int expected)
        {
            Assert.That(ViewResource.Formats.Count, Is.EqualTo(expected));
            Assert.That(ViewResource.Formats.Count, Is.EqualTo(expected));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        /// 
        /// <summary>
        /// CompressionLevel で指定可能な種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(6)]
        public void CompressionLevel(int expected)
        {
            Assert.That(ViewResource.CompressionLevels.Count, Is.EqualTo(expected));
            Assert.That(ViewResource.CompressionLevels.Count, Is.EqualTo(expected));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethods
        /// 
        /// <summary>
        /// CompressionMethod で指定可能な種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(4)]
        public void CompressionMethods(int expected)
        {
            Assert.That(ViewResource.CompressionMethods.Count, Is.EqualTo(expected));
            Assert.That(ViewResource.CompressionMethods.Count, Is.EqualTo(expected));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethod
        /// 
        /// <summary>
        /// EncryptionMethod で指定可能な種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(4)]
        public void EncryptionMethod(int expected)
        {
            Assert.That(ViewResource.EncryptionMethods.Count, Is.EqualTo(expected));
            Assert.That(ViewResource.EncryptionMethods.Count, Is.EqualTo(expected));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsEncryptionSupported
        /// 
        /// <summary>
        /// 暗号化に対応しているかどうかを判別するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      ExpectedResult =  true)]
        [TestCase(Format.SevenZip, ExpectedResult =  true)]
        [TestCase(Format.Sfx,      ExpectedResult =  true)]
        [TestCase(Format.Tar,      ExpectedResult = false)]
        public bool IsEncryptionSupported(Format format)
            => ViewResource.IsEncryptionSupported(format);

        /* ----------------------------------------------------------------- */
        ///
        /// GetCompressionMethod
        /// 
        /// <summary>
        /// 各 Format に対応する CompressionMethod の種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      6)]
        [TestCase(Format.SevenZip, 6)]
        [TestCase(Format.Tar,      4)]
        [TestCase(Format.Sfx,      6)]
        [TestCase(Format.BZip2,    0)]
        public void GetCompressionMethod(Format format, int expected)
        {
            Assert.That(ViewResource.GetCompressionMethod(format)?.Count ?? 0, Is.EqualTo(expected));
            Assert.That(ViewResource.GetCompressionMethod(format)?.Count ?? 0, Is.EqualTo(expected));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormat
        /// 
        /// <summary>
        /// 拡張子フィルタを取得するために必要なファイルの種類を
        /// 取得するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      CompressionMethod.Default, ExpectedResult = Format.Zip)]
        [TestCase(Format.SevenZip, CompressionMethod.Default, ExpectedResult = Format.SevenZip)]
        [TestCase(Format.Sfx,      CompressionMethod.Default, ExpectedResult = Format.Sfx)]
        [TestCase(Format.Tar,      CompressionMethod.Copy,    ExpectedResult = Format.Tar)]
        [TestCase(Format.Tar,      CompressionMethod.BZip2,   ExpectedResult = Format.BZip2)]
        [TestCase(Format.Tar,      CompressionMethod.GZip,    ExpectedResult = Format.GZip)]
        [TestCase(Format.Tar,      CompressionMethod.XZ,      ExpectedResult = Format.XZ)]
        public Format GetFormat(Format format, CompressionMethod method)
            => ViewResource.GetFormat(format, method);

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        /// 
        /// <summary>
        /// ファイルの種類に対応する拡張子を取得するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      ExpectedResult = ".zip")]
        [TestCase(Format.SevenZip, ExpectedResult = ".7z")]
        [TestCase(Format.Sfx,      ExpectedResult = ".exe")]
        [TestCase(Format.Tar,      ExpectedResult = ".tar")]
        [TestCase(Format.BZip2,    ExpectedResult = ".tar.bz2")]
        [TestCase(Format.GZip,     ExpectedResult = ".tar.gz")]
        [TestCase(Format.XZ,       ExpectedResult = ".tar.xz")]
        public string GetExtension(Format format)
            => ViewResource.GetExtension(format);

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter
        /// 
        /// <summary>
        /// Format に対応する 拡張子フィルタを取得するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      "*.zip")]
        [TestCase(Format.SevenZip, "*.7z")]
        [TestCase(Format.Tar,      "*.tar")]
        [TestCase(Format.GZip,     "*.tar.gz;*.tgz")]
        [TestCase(Format.BZip2,    "*.tar.bz2;*.tb2;*.tar.bz;*.tbz")]
        [TestCase(Format.XZ,       "*.tar.xz;*.txz")]
        [TestCase(Format.Sfx,      "*.exe")]
        public void GetFilter(Format format, string piece)
        {
            var result = ViewResource.GetFilter(format);
            Assert.That(result.Contains("*.*"),           Is.True);
            Assert.That(result.Contains(piece),           Is.True);
            Assert.That(result.Contains(piece.ToUpper()), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter_7z
        /// 
        /// <summary>
        /// 7z に対応する 拡張子フィルタを取得するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetFilter_7z()
        {
            var result = ViewResource.GetFilter("7z");
            Assert.That(result.Contains("*.*"),  Is.True);
            Assert.That(result.Contains("*.7z"), Is.True);
            Assert.That(result.Contains("*.7Z"), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter_NotSupported
        /// 
        /// <summary>
        /// 圧縮処理に非対応の形式が指定された時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetFilter_NotSupported() => Assert.That(
            ViewResource.GetFilter(Format.Rar),
            Is.EqualTo("すべてのファイル (*.*)|*.*")
        );

        #endregion
    }
}
