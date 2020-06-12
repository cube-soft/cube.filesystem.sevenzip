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
using Cube.FileSystem.SevenZip.Ice;
using NUnit.Framework;
using System;

namespace Cube.FileSystem.SevenZip.Ice.Tests
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
        public bool IsEncryptionSupported(Format format) =>
            ViewResource.IsEncryptionSupported(format);

        /* ----------------------------------------------------------------- */
        ///
        /// GetTimeString
        ///
        /// <summary>
        /// TimeSpan オブジェクトの書式化のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(0,  0,  0,  0, ExpectedResult = "00:00:00")]
        [TestCase(0,  0, 29,  0, ExpectedResult = "00:29:00")]
        [TestCase(0,  0, 30,  0, ExpectedResult = "00:30:00")]
        [TestCase(0, 23, 59, 59, ExpectedResult = "23:59:59")]
        [TestCase(1, 23, 59, 59, ExpectedResult = "47:59:59")]
        public string GetTimeString(int day, int hour, int min, int sec) =>
            ViewResource.GetTimeString(new TimeSpan(day, hour, min, sec));

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
            var dest = ViewResource.GetFilter(format);
            Assert.That(dest.Contains("*.*"),                    Is.True);
            Assert.That(dest.Contains(piece),                    Is.True);
            Assert.That(dest.Contains(piece.ToUpperInvariant()), Is.True);
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
            var dest = ViewResource.GetFilter("7z");
            Assert.That(dest.Contains("*.*"),  Is.True);
            Assert.That(dest.Contains("*.7z"), Is.True);
            Assert.That(dest.Contains("*.7Z"), Is.True);
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
