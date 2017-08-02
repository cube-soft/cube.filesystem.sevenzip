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
using System.Collections.Generic;
using Cube.FileSystem.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests.Settings
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
    [Parallelizable]
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
        {
            var reg = new AssociateRegistrar(path) { IconLocation = "" };
            foreach (var s in args) reg.Arguments.Add(s);
            return reg.Command;
        }

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
