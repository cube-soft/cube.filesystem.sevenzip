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
using System.Linq;
using NUnit.Framework;

namespace Cube.FileSystem.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// OperatorTest
    /// 
    /// <summary>
    /// Cube.FileSystem.Operator のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class OperatorTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetFiles
        ///
        /// <summary>
        /// ファイル一覧を取得するテストを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// Example フォルダにテスト用のサンプルファイルを追加した場合、
        /// それに合わせて期待値を更新して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void GetFiles(IOperatorCore core)
        {
            var io = new Operator(core);
            Assert.That(io.GetFiles(Examples).Count(), Is.EqualTo(20));
            Assert.That(io.GetFiles(Example("Sample.txt")), Is.Null);

            var empty = Result("Empty");
            io.CreateDirectory(empty);
            var result = io.GetFiles(empty);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectories
        ///
        /// <summary>
        /// ディレクトリ一覧を取得するテストを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// Example フォルダにテスト用のサンプルファイルを追加した場合、
        /// それに合わせて期待値を更新して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void GetDirectories(IOperatorCore core)
        {
            var io = new Operator(core);
            Assert.That(io.GetDirectories(Examples).Count(), Is.EqualTo(1));
            Assert.That(io.GetDirectories(Example("Sample.txt")), Is.Null);

            var empty = Result("Empty");
            io.CreateDirectory(empty);
            var result = io.GetDirectories(empty);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get_DateTime
        ///
        /// <summary>
        /// 各種日時情報の内容を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void Get_DateTime(IOperatorCore core)
        {
            var io = new Operator(core);
            var info = io.Get(Example("Sample.txt"));

            Assert.That(info.CreationTime,   Is.GreaterThan(new DateTime(2017, 6, 5)));
            Assert.That(info.LastWriteTime,  Is.GreaterThan(info.CreationTime));
            Assert.That(info.LastAccessTime, Is.GreaterThan(info.CreationTime));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get_Throws
        ///
        /// <summary>
        /// ファイル情報の取得に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void Get_Throws(IOperatorCore core)
        {
            var io = new Operator(core);
            Assert.That(() => io.Get(string.Empty), Throws.TypeOf<ArgumentException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWrite
        ///
        /// <summary>
        /// 書き込み用ストリームを生成するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void OpenWrite(IOperatorCore core)
        {
            var dest = Result("OpenWrite.txt");
            var io = new Operator(core);
            io.Copy(Example("Sample.txt"), dest, true);

            var count = io.Get(dest).Length;
            using (var stream = io.OpenWrite(dest)) stream.WriteByte((byte)'A');
            Assert.That(io.Get(dest).Length, Is.EqualTo(count));

            var newfile = Result("OpenWrite_Create.txt");
            using (var stream = io.OpenWrite(newfile)) stream.WriteByte((byte)'A');
            Assert.That(io.Get(newfile).Length, Is.EqualTo(1));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// 移動のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void Move(IOperatorCore core)
        {
            var io = new Operator(core);
            io.Failed += (s, e) => Assert.Fail($"{e.Name}: {e.Exception}");

            var info = io.Get("Sample.txt");
            var src  = io.Get(io.Combine(Results, info.Name));
            var dest = io.Get(io.Combine(Results, $"{info.NameWithoutExtension}-Move{info.Extension}"));

            io.Copy(io.Combine(Examples, info.Name), src.FullName, false);
            src.Refresh();
            Assert.That(src.Exists, Is.True);

            io.Copy(src.FullName, dest.FullName, false);
            io.Move(src.FullName, dest.FullName, true);
            src.Refresh();
            dest.Refresh();
            Assert.That(src.Exists,  Is.False);
            Assert.That(dest.Exists, Is.True);

            io.Move(dest.FullName, src.FullName);
            src.Refresh();
            dest.Refresh();
            Assert.That(src.Exists,  Is.True);
            Assert.That(dest.Exists, Is.False);

            io.Delete(src.FullName);
            src.Refresh();
            Assert.That(src.Exists,  Is.False);
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
        [TestCaseSource(nameof(Operator_TestCases))]
        public void Move_Failed(IOperatorCore core)
        {
            var failed = false;
            var io = new Operator(core);
            io.Failed += (s, e) =>
            {
                failed   = true;
                e.Cancel = true;

                Assert.That(e.Name,          Is.EqualTo("Move"));
                Assert.That(e.Paths.Count(), Is.EqualTo(2));
            };

            var src  = io.Combine(Results, "FileNotFound.txt");
            var dest = io.Combine(Results, "Moved.txt");
            io.Move(src, dest);

            Assert.That(failed, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open_Failed
        ///
        /// <summary>
        /// ファイルを開く操作に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Operator_TestCases))]
        public void Open_Failed(IOperatorCore core)
        {
            var failed = false;
            var io = new Operator(core);
            io.Failed += (s, e) =>
            {
                failed   = true;
                e.Cancel = true;

                Assert.That(e.Name, Is.EqualTo("OpenRead"));
                Assert.That(e.Paths.Count(), Is.EqualTo(1));
            };

            var src    = io.Combine(Results, "FileNotFound.txt");
            var stream = io.OpenRead(src);

            Assert.That(failed, Is.True);
            Assert.That(stream, Is.Null);
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
        [TestCaseSource(nameof(Operator_TestCases))]
        public void Move_Throws(IOperatorCore core)
            => Assert.That(() =>
            {
                var io   = new Operator(core);
                var src  = io.Combine(Results, "FileNotFound.txt");
                var dest = io.Combine(Results, "Moved.txt");
                io.Move(src, dest);
            },
            Throws.TypeOf<System.IO.FileNotFoundException>());

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// Operator_TestCases
        ///
        /// <summary>
        /// 各種 Operator のテスト用データを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> Operator_TestCases
        {
            get
            {
                yield return new TestCaseData(new OperatorCore());
                yield return new TestCaseData(new Alpha());
            }
        }

        #endregion
    }
}
