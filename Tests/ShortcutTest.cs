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
using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace Cube.FileSystem.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutTest
    /// 
    /// <summary>
    /// Shortcut のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Parallelizable]
    class ShortcutTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// ショートカットを作成するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(TestCases))]
        public bool Create(string name, string link, int index, IList<string> args)
        {
            var src  = Result(name);
            var dest = GetLinkPath(link);
            var sc   = new Shortcut(src)
            {
                Link         = dest,
                Arguments    = args,
                IconLocation = $"{dest},{index}",
            };

            sc.Create();
            return sc.Exists;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Empty
        ///
        /// <summary>
        /// リンク先パスに空文字を指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Empty()
        {
            var sc = new Shortcut(Result("ScEmpty"))
            {
                Link         = string.Empty,
                Arguments    = null,
                IconLocation = string.Empty,
            };

            sc.Create();
            Assert.That(sc.Exists, Is.False);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Throw
        ///
        /// <summary>
        /// 空文字を指定した時に例外が送出される事を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Throw()
            => Assert.That(() => new Shortcut(""), Throws.TypeOf<ArgumentException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// ショートカットを削除するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Delete()
        {
            var src  = Result("DeleteTest");
            var dest = GetLinkPath("Cube.FileSystem.dll");
            var sc   = new Shortcut(src)
            {
                Link         = dest,
                Arguments    = null,
                IconLocation = dest,
            };

            sc.Create();
            Assert.That(sc.Exists, Is.True);

            sc.Delete();
            sc.Delete(); // ignore
            Assert.That(sc.Exists, Is.False);
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        ///
        /// <summary>
        /// ショートカット操作のテスト用データを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("ScNormal", "Cube.FileSystem.dll", 0,
                    new List<string> { "/foo", "bar", "/bas" }
                ).Returns(true);

                yield return new TestCaseData("ScNullArgs", "Cube.FileSystem.dll", 0,
                    null
                ).Returns(true);

                yield return new TestCaseData("ScEmptyArgs", "Cube.FileSystem.dll", 0,
                    new List<string>()
                ).Returns(true);

                yield return new TestCaseData("ScWrongIconIndex", "Cube.FileSystem.dll", 3,
                    new List<string> { "/foo" }
                ).Returns(true);

                yield return new TestCaseData("ScWrongLink", "dummy.exe", 0,
                    new List<string> { "/foo" }
                ).Returns(false);

                yield return new TestCaseData("ScWithLnk.lnk", "Cube.FileSystem.dll", 0,
                    new List<string> { "args" }
                ).Returns(true);

                yield return new TestCaseData("日本語ショートカット", "Cube.FileSystem.dll", 0,
                    new List<string> { "args" }
                ).Returns(true);
            }
        }

        #endregion

        #region Helper

        /* ----------------------------------------------------------------- */
        ///
        /// GetLinkPath
        /// 
        /// <summary>
        /// リンク先のパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetLinkPath(string filename)
        {
            var asm = Assembly.GetExecutingAssembly().Location;
            var dir = IO.Get(asm).DirectoryName;
            return IO.Combine(dir, filename);
        }

        #endregion
    }
}
