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
using System.IO;
using NUnit.Framework;

namespace Cube.FileSystem.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileHandlerTest
    /// 
    /// <summary>
    /// FileHandler のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class FileHandlerTest : FileResource
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Move_Overwrite
        ///
        /// <summary>
        /// 上書き移動のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("Sample.txt")]
        public void Move_Overwrite(string filename)
        {
            var op = new Cube.FileSystem.FileHandler();
            op.Failed += (s, e) => Assert.Fail($"{e.Key}: {e.Value}");

            var name = Path.GetFileNameWithoutExtension(filename);
            var ext  = Path.GetExtension(filename);
            var src  = Result(filename);
            var dest = Result($"{name}-Move{ext}");

            op.Copy(Example(filename), src, false);
            op.Copy(src, dest, false);
            op.Move(src, dest, true);
            Assert.That(File.Exists(dest), Is.True);

            op.Delete(dest);
            Assert.That(File.Exists(dest), Is.False);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move_Failed
        ///
        /// <summary>
        /// 移動操作に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Move_Failed()
        {
            var failed = false;
            var op = new Cube.FileSystem.FileHandler();
            op.Failed += (s, e) =>
            {
                failed   = true;
                e.Cancel = true;
            };

            var src  = Result("FileNotFound.txt");
            var dest = Result("Moved.txt");
            op.Move(src, dest, true);

            Assert.That(failed, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move_Throws
        ///
        /// <summary>
        /// 移動操作に失敗するテストを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// Failed イベントにハンドラを登録していない場合、File.Move を
        /// 実行した時と同様の例外が発生します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Move_Throws()
            => Assert.That(() =>
            {
                var op   = new Cube.FileSystem.FileHandler();
                var src  = Result("FileNotFound.txt");
                var dest = Result("Moved.txt");
                op.Move(src, dest, true);
            },
            Throws.TypeOf<FileNotFoundException>());
    }
}
