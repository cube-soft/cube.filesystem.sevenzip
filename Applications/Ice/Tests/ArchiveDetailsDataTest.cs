/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveDetailsDataTest
    /// 
    /// <summary>
    /// ArchiveDetailsData のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveDetailsDataTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Format
        /// 
        /// <summary>
        /// Format で指定可能な種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(4)]
        public void Format(int expected)
        {
            Assert.That(ArchiveDetailsData.Format.Count, Is.EqualTo(expected));
            Assert.That(ArchiveDetailsData.Format.Count, Is.EqualTo(expected));
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
            Assert.That(ArchiveDetailsData.CompressionLevel.Count, Is.EqualTo(expected));
            Assert.That(ArchiveDetailsData.CompressionLevel.Count, Is.EqualTo(expected));
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
            Assert.That(ArchiveDetailsData.CompressionMethods.Count, Is.EqualTo(expected));
            Assert.That(ArchiveDetailsData.CompressionMethods.Count, Is.EqualTo(expected));
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
            Assert.That(ArchiveDetailsData.EncryptionMethod.Count, Is.EqualTo(expected));
            Assert.That(ArchiveDetailsData.EncryptionMethod.Count, Is.EqualTo(expected));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetCompressionMethod
        /// 
        /// <summary>
        /// 各 Format に対応する CompressionMethod の種類を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(SevenZip.Format.Zip,      6)]
        [TestCase(SevenZip.Format.SevenZip, 6)]
        [TestCase(SevenZip.Format.Tar,      4)]
        [TestCase(SevenZip.Format.Sfx,      6)]
        [TestCase(SevenZip.Format.BZip2,    0)]
        public void GetCompressionMethod(SevenZip.Format format, int expected)
        {
            Assert.That(ArchiveDetailsData.GetCompressionMethod(format)?.Count ?? 0, Is.EqualTo(expected));
            Assert.That(ArchiveDetailsData.GetCompressionMethod(format)?.Count ?? 0, Is.EqualTo(expected));
        }
    }
}
