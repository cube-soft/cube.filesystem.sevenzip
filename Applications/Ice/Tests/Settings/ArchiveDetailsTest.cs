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

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveRuntimeSettingsTest
    ///
    /// <summary>
    /// ArchiveRuntimeSettings のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveRuntimeSettingsTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ToOption
        ///
        /// <summary>
        /// ArchiveRuntimeSettings を ArchiveOption オブジェクトに変換する
        /// テストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void ToOption()
        {
            var actual = new ArchiveRuntimeSettings
            {
                CompressionLevel  = CompressionLevel.High,
                CompressionMethod = CompressionMethod.Ppmd,
                EncryptionMethod  = EncryptionMethod.Aes192,
                Format            = Format.GZip,
                Password          = "password",
                Path              = "dummy",
                SfxModule         = string.Empty,
                ThreadCount       = 3,
            }.ToOption(new SettingsFolder());

            Assert.That(actual.CompressionLevel, Is.EqualTo(CompressionLevel.High));
            Assert.That(actual.ThreadCount,      Is.EqualTo(3));
        }
    }
}
