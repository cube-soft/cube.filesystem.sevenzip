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
        /// GetDialogFilters
        ///
        /// <summary>
        /// Tests the GetExtensionFilter method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(Format.Zip,      "*.zip"    )]
        [TestCase(Format.SevenZip, "*.7z"     )]
        [TestCase(Format.Tar,      "*.tar"    )]
        [TestCase(Format.GZip,     "*.tar.gz" )]
        [TestCase(Format.GZip,     "*.tgz"    )]
        [TestCase(Format.BZip2,    "*.tar.bz2")]
        [TestCase(Format.BZip2,    "*.tb2"    )]
        [TestCase(Format.BZip2,    "*.tar.bz" )]
        [TestCase(Format.BZip2,    "*.tbz"    )]
        [TestCase(Format.XZ,       "*.tar.xz" )]
        [TestCase(Format.XZ,       "*.txz"    )]
        [TestCase(Format.Sfx,      "*.exe"    )]
        [TestCase(Format.Unknown,  "*.zip"    )] /* Contains all formats */
        public void GetDialogFilters(Format format, string piece)
        {
            var dest  = Resource.GetDialogFilters(format).GetFilter();
            var upper = piece.ToUpperInvariant();
            Assert.That(dest.Contains("*.*"));
            Assert.That(dest.Contains(piece), piece);
            Assert.That(dest.Contains(upper), upper);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDialogFilter_NotSupported
        ///
        /// <summary>
        /// Tests the GetDialogFilters method with an unsupported format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetDialogFilters_NotSupported() => Assert.That(
            Resource.GetDialogFilters(Format.Rar).GetFilter(),
            Is.EqualTo("すべてのファイル (*.*)|*.*")
        );

        #endregion
    }
}
