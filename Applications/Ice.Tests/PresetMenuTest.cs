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
using System.Linq;
using Cube.FileSystem.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PresetMenuTest
    /// 
    /// <summary>
    /// PresetMenu のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class PresetMenuTest
    {
        /* --------------------------------------------------------------------- */
        ///
        /// ToArguments
        ///
        /// <summary>
        /// PresetMenu.ToArguments の拡張メソッドのテストを実行します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [TestCase(PresetMenu.Archive,            ExpectedResult = 1)]
        [TestCase(PresetMenu.ArchiveZipPassword, ExpectedResult = 2)]
        [TestCase(PresetMenu.Extract,            ExpectedResult = 1)]
        [TestCase(PresetMenu.ExtractDesktop,     ExpectedResult = 2)]
        [TestCase(PresetMenu.ExtractMyDocuments, ExpectedResult = 2)]
        [TestCase(PresetMenu.ExtractRuntime,     ExpectedResult = 2)]
        [TestCase(PresetMenu.ExtractSource,      ExpectedResult = 2)]
        [TestCase(PresetMenu.Settings,           ExpectedResult = 0)]
        [TestCase(PresetMenu.Mail,               ExpectedResult = 2)]
        [TestCase(PresetMenu.MailZip,            ExpectedResult = 2)]
        [TestCase(PresetMenu.MailZipPassword,    ExpectedResult = 3)]
        [TestCase(PresetMenu.MailSevenZip,       ExpectedResult = 2)]
        [TestCase(PresetMenu.MailBZip2,          ExpectedResult = 2)]
        [TestCase(PresetMenu.MailGZip,           ExpectedResult = 2)]
        [TestCase(PresetMenu.MailXZ,             ExpectedResult = 2)]
        [TestCase(PresetMenu.MailSfx,            ExpectedResult = 2)]
        [TestCase(PresetMenu.MailDetail,         ExpectedResult = 2)]
        public int ToArguments(PresetMenu menu)
            => menu.ToArguments().Count();
    }
}
