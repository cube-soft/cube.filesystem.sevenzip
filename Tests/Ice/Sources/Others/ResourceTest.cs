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
using System;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ResourceTest
    ///
    /// <summary>
    /// Tests the Resource class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ResourceTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// Tests the Formats property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Formats()
        {
            Assert.That(Resource.Formats.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevels
        ///
        /// <summary>
        /// Tests the CompressionLevels property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CompressionLevels()
        {
            Assert.That(Resource.CompressionLevels.Count, Is.EqualTo(6));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethods
        ///
        /// <summary>
        /// Tests the CompressionMethods property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CompressionMethods()
        {
            Assert.That(Resource.CompressionMethods.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethods
        ///
        /// <summary>
        /// Tests the EncryptionMethods property.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void EncryptionMethods()
        {
            Assert.That(Resource.EncryptionMethods.Count, Is.EqualTo(4));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTimeString
        ///
        /// <summary>
        /// Tests the GetTimeString method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(0,  0,  0,  0, ExpectedResult = "00:00:00")]
        [TestCase(0,  0, 29,  0, ExpectedResult = "00:29:00")]
        [TestCase(0,  0, 30,  0, ExpectedResult = "00:30:00")]
        [TestCase(0, 23, 59, 59, ExpectedResult = "23:59:59")]
        [TestCase(1, 23, 59, 59, ExpectedResult = "47:59:59")]
        public string GetTimeString(int day, int hour, int min, int sec) =>
            Resource.GetTimeString(new TimeSpan(day, hour, min, sec));

        /* ----------------------------------------------------------------- */
        ///
        /// GetCompressionMethods
        ///
        /// <summary>
        /// Tests the GetCompressionMethods method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      6)]
        [TestCase(Format.SevenZip, 6)]
        [TestCase(Format.Tar,      4)]
        [TestCase(Format.Sfx,      6)]
        [TestCase(Format.BZip2,    0)]
        public void GetCompressionMethod(Format format, int n)
        {
            Assert.That(Resource.GetCompressionMethods(format)?.Count ?? 0, Is.EqualTo(n));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtensionFilter
        ///
        /// <summary>
        /// Tests the GetExtensionFilter method.
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
        public void GetExtensionFilter(Format format, string piece)
        {
            var dest = Resource.GetExtensionFilter(format);
            Assert.That(dest.Contains("*.*"),                    Is.True);
            Assert.That(dest.Contains(piece),                    Is.True);
            Assert.That(dest.Contains(piece.ToUpperInvariant()), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter_7z
        ///
        /// <summary>
        /// Tests the GetExtensionFilter method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetExtensionFilter_7z()
        {
            var dest = Resource.GetExtensionFilter("7z");
            Assert.That(dest.Contains("*.*"),  Is.True);
            Assert.That(dest.Contains("*.7z"), Is.True);
            Assert.That(dest.Contains("*.7Z"), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtensionFilter_NotSupported()
        ///
        /// <summary>
        /// Tests the GetExtensionFilter method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetExtensionFilter_NotSupported()
        {
            var dest = Resource.GetExtensionFilter(Format.Rar);
            Assert.That(dest, Is.EqualTo("すべてのファイル (*.*)|*.*"));
        }

        #endregion
    }
}
