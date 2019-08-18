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
using System.Collections.Generic;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateRegistrarTest
    ///
    /// <summary>
    /// AssociateSettings のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class AssociateRegistrarTest : FileFixture
    {
        #region Tests

        /* --------------------------------------------------------------------- */
        ///
        /// Command
        ///
        /// <summary>
        /// Tests the constructor and confirms the Command property.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public string Command(string path, IList<string> args) => new AssociateRegistrar(path)
        {
            Arguments    = args,
            IconLocation = "",
            ToolTip      = false,
        }.Command;

        /* --------------------------------------------------------------------- */
        ///
        /// Update_Throws
        ///
        /// <summary>
        /// Tests the Update method.
        /// </summary>
        ///
        /// <remarks>
        /// Update では HKEY_CLASSES_ROOT 下のサブキーを修正をしようとする
        /// ため、通常のアクセス権限では操作に失敗します。管理者権限で
        /// 実行された場合は、CubeICE が対応する全ての拡張子に対して、
        /// 関連付けを解除します。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        [Test]
        public void Update_Throws()
        {
            try
            {
                var settings  = new SettingFolder(GetType().Assembly, IO);
                var path      = @"C:\Program Files\CubeICE\cubeice.exe";
                var registrar = new AssociateRegistrar(path)
                {
                    Arguments = new List<string> { "/x" },
                    IconLocation = $"{path},{settings.Value.Associate.IconIndex}",
                    ToolTip = false,
                };

                registrar.Update(settings.Value.Associate.Value);

                foreach (var key in settings.Value.Associate.Value.Keys.ToArray())
                {
                    settings.Value.Associate.Value[key] = false;
                }
                registrar.Update(settings.Value.Associate.Value);

                Assert.Pass("Administrator");
            }
            catch (UnauthorizedAccessException err) { Assert.Ignore(err.Message); }
        }

        #endregion

        #region TestCases

        /* --------------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// Gets the test cases.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "C:\\Program Files\\CubeICE\\cubeice.exe",
                    new List<string> { "/x" }
                ).Returns("\"C:\\Program Files\\CubeICE\\cubeice.exe\" \"/x\" \"%1\"");

                yield return new TestCaseData(
                    "C:\\Program Files (x86)\\CubeICE\\cubeice.exe",
                    new List<string>()
                ).Returns("\"C:\\Program Files (x86)\\CubeICE\\cubeice.exe\" \"%1\"");
            }
        }

        #endregion
    }
}
