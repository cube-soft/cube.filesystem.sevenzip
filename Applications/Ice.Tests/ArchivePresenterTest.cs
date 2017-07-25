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
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePresenterTest
    /// 
    /// <summary>
    /// ArchivePresenterTest のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchivePresenterTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        /// 
        /// <summary>
        /// 圧縮処理のテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(Archive_TestCases))]
        public async Task<long> Archive(string filename, string[] args)
        {
            var settings = new SettingsFolder();
            var events   = new EventAggregator();
            var view     = Views.CreateProgressView();
            var model    = new Request(args.Concat(new[]
            {
                Example("Sample.txt"),
                Example("Archive"),
            }).ToArray());

            MockViewFactory.Destination = Result(filename);

            using (var ap = new ArchivePresenter(view, model, settings, events))
            {
                view.Show();

                Assert.That(view.Visible, Is.True);
                for (var i = 0; view.Visible && i < 20; ++i) await Task.Delay(100);
                Assert.That(view.Visible, Is.False, "Timeout");

                Assert.That(view.FileName,  Is.EqualTo(filename));
                Assert.That(view.DoneCount, Is.EqualTo(view.FileCount));
                Assert.That(view.Value,     Is.EqualTo(100));

                return view.FileCount;
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_TestCases
        /// 
        /// <summary>
        /// 圧縮処理のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> Archive_TestCases
        {
            get
            {
                yield return new TestCaseData("7zTest.7z", new[] { "/c:7z", "/o:runtime" }).Returns(4L);
                yield return new TestCaseData("TarTest.tar.bz", new[] { "/c:bzip2", "/o:runtime" }).Returns(1L);
                yield return new TestCaseData("ZipDetail.zip", new[] { "/c:detail" }).Returns(4L);
            }
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// OneTimeSetUp
        /// 
        /// <summary>
        /// 一度だけ実行される SetUp 処理です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [OneTimeSetUp]
        public void OneTimeSetUp() => MockViewFactory.Configure();

        /* ----------------------------------------------------------------- */
        ///
        /// OneTimeTearDown
        /// 
        /// <summary>
        /// 一度だけ実行される TearDown 処理です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [OneTimeTearDown]
        public void OneTimeTearDown() => MockViewFactory.Reset();

        #endregion
    }
}
