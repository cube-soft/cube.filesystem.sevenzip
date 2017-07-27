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
using Cube.FileSystem.Files;

namespace Cube.FileSystem.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FilesTest
    /// 
    /// <summary>
    /// FileSystem.Operator の拡張メソッドのテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class FilesTest : FileResource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetTypeName
        ///
        /// <summary>
        /// ファイルの種類を表す文字列を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.txt",     ExpectedResult = true)]
        [TestCase("NotExist.dummy", ExpectedResult = true)]
        public bool GetTypeName(string filename)
            => !string.IsNullOrEmpty(IO.GetTypeName(IO.Get(Example(filename))));

        /* ----------------------------------------------------------------- */
        ///
        /// GetTypeName_Null
        ///
        /// <summary>
        /// 引数に null を指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetTypeName_Null()
        {
            Assert.That(IO.GetTypeName(string.Empty), Is.Null);
            Assert.That(IO.GetTypeName(default(string)), Is.Null);
            Assert.That(IO.GetTypeName(default(IInformation)), Is.Null);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUniqueName
        ///
        /// <summary>
        /// 一意なパスを取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetUniqueName()
        {
            var src = Result($"UniqueTest.txt");
            var u1 = IO.GetUniqueName(src);
            Assert.That(u1, Is.EqualTo(src));

            IO.Copy(Example("Sample.txt"), u1);
            var u2 = IO.GetUniqueName(src);
            Assert.That(u2, Is.EqualTo(Result($"UniqueTest(2).txt")));

            IO.Copy(Example("Sample.txt"), u2);
            var u3 = IO.GetUniqueName(IO.Get(src));
            Assert.That(u3, Is.EqualTo(Result($"UniqueTest(3).txt")));

            IO.Copy(Example("Sample.txt"), u3);
            var u4 = IO.GetUniqueName(u3); // Not src
            Assert.That(u4, Is.EqualTo(Result($"UniqueTest(3)(2).txt")));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUniqueName_Null
        ///
        /// <summary>
        /// 引数に null を指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetUniqueName_Null()
        {
            var dummy = default(Operator);
            var src   = Example("Sample.txt");

            Assert.That(dummy.GetUniqueName(src), Is.Null);
            Assert.That(dummy.GetUniqueName(IO.Get(src)), Is.Null);
        }
    }
}
