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
using System.Collections.Generic;
using Cube.FileSystem.SevenZip.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests.Settings
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
    class AssociateRegistrarTest
    {
        /* --------------------------------------------------------------------- */
        ///
        /// Command
        /// 
        /// <summary>
        /// レジストリに登録されるコマンドライン用文字列を確認します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public string Command(string path, IList<string> args)
            => new AssociateRegistrar(path)
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
        /// Update 実行時の挙動を確認します。
        /// </summary>
        /// 
        /// <remarks>
        /// Update では HKEY_CLASSES_ROOT 下のサブキーを修正をしようとする
        /// ため、通常のアクセス権限では操作に失敗します。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        [Test]
        public void Update_Throws() => Assert.That(() =>
        {
            var path      = @"C:\Program Files\CubeICE\cubeice.exe";
            var settings  = new SettingsFolder();
            var registrar = new AssociateRegistrar(path)
            {
                Arguments    = new List<string> { "/x" },
                IconLocation = "",
                ToolTip      = false,
            };
            registrar.Update(settings.Value.Associate.Value);
        }, Throws.TypeOf<UnauthorizedAccessException>());

        /* --------------------------------------------------------------------- */
        ///
        /// TestCases
        /// 
        /// <summary>
        /// Command のテスト用データを取得します。
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
    }
}
