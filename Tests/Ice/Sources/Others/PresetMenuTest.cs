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
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PresetMenuTest
    ///
    /// <summary>
    /// Tests the PresetMenu enum.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class PresetMenuTest
    {
        #region Tests

        /* --------------------------------------------------------------------- */
        ///
        /// ToContextMenuGroup
        ///
        /// <summary>
        /// Tests the ToContextMenuGroup method.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [Test]
        public void ToContextMenuGroup()
        {
            var dest = PresetMenu.DefaultContext.ToContextMenuGroup().ToList();
            Assert.That(dest.Count,   Is.EqualTo(2));
            Assert.That(dest[0].Name, Is.EqualTo("圧縮"));
            Assert.That(dest[1].Name, Is.EqualTo("解凍"));

            var d0 = dest[0].Children;
            Assert.That(d0.Count, Is.EqualTo(7));
            Assert.That(d0[0].Name, Is.EqualTo("Zip"));
            Assert.That(d0[1].Name, Is.EqualTo("Zip (パスワード)"));
            Assert.That(d0[2].Name, Is.EqualTo("7-Zip"));
            Assert.That(d0[3].Name, Is.EqualTo("BZip2"));
            Assert.That(d0[4].Name, Is.EqualTo("GZip"));
            Assert.That(d0[5].Name, Is.EqualTo("自己解凍形式"));
            Assert.That(d0[6].Name, Is.EqualTo("詳細設定"));

            var d1 = dest[1].Children;
            Assert.That(d1.Count, Is.EqualTo(4));
            Assert.That(d1[0].Name, Is.EqualTo("ここに解凍"));
            Assert.That(d1[1].Name, Is.EqualTo("デスクトップに解凍"));
            Assert.That(d1[2].Name, Is.EqualTo("マイドキュメントに解凍"));
            Assert.That(d1[3].Name, Is.EqualTo("場所を指定して解凍"));

            foreach (var m in d0) Assert.That(m.IconIndex, Is.EqualTo(1), m.Name);
            foreach (var m in d1) Assert.That(m.IconIndex, Is.EqualTo(2), m.Name);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToArguments
        ///
        /// <summary>
        /// Tests the ToArguments method.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [TestCase(PresetMenu.Compress,            ExpectedResult = 1)]
        [TestCase(PresetMenu.CompressZipPassword, ExpectedResult = 2)]
        [TestCase(PresetMenu.Extract,             ExpectedResult = 1)]
        [TestCase(PresetMenu.ExtractDesktop,      ExpectedResult = 2)]
        [TestCase(PresetMenu.ExtractMyDocuments,  ExpectedResult = 2)]
        [TestCase(PresetMenu.ExtractRuntime,      ExpectedResult = 2)]
        [TestCase(PresetMenu.ExtractSource,       ExpectedResult = 2)]
        [TestCase(PresetMenu.Settings,            ExpectedResult = 0)]
        [TestCase(PresetMenu.Mail,                ExpectedResult = 2)]
        [TestCase(PresetMenu.MailZip,             ExpectedResult = 2)]
        [TestCase(PresetMenu.MailZipPassword,     ExpectedResult = 3)]
        [TestCase(PresetMenu.MailSevenZip,        ExpectedResult = 2)]
        [TestCase(PresetMenu.MailBZip2,           ExpectedResult = 2)]
        [TestCase(PresetMenu.MailGZip,            ExpectedResult = 2)]
        [TestCase(PresetMenu.MailXz,              ExpectedResult = 2)]
        [TestCase(PresetMenu.MailSfx,             ExpectedResult = 2)]
        [TestCase(PresetMenu.MailOthers,          ExpectedResult = 2)]
        public int ToArguments(PresetMenu menu) => menu.ToArguments().Count();

        #endregion
    }
}
