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
using System.Threading.Tasks;
using Cube.FileSystem.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractPresenterTest
    /// 
    /// <summary>
    /// ExtractPresenter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractPresenterTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 展開処理のテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase("Single.zip",           "",         ExpectedResult = 1)]
        [TestCase("SingleDirectory.zip",  "",         ExpectedResult = 4)]
        [TestCase("MultiDirectory.zip",   "",         ExpectedResult = 7)]
        [TestCase("Complex.zip",          "",         ExpectedResult = 5)]
        [TestCase("Password.7z",          "password", ExpectedResult = 3)]
        public async Task<long> Extract(string filename, string password)
        {
            var source   = Example(filename);
            var args     = PresetMenu.ExtractRuntime.ToArguments();
            var model    = new Request(args.Concat(new[] { source }));
            var settings = new SettingsFolder();
            var events   = new EventAggregator();
            var view     = Views.CreateProgressView();

            // Preset
            MockViewFactory.Destination = Results;
            MockViewFactory.Password    = password;

            settings.Value.Extract.SaveLocation  = SaveLocation.Runtime;
            settings.Value.Extract.RootDirectory = RootDirectoryCondition.CreateSmart;
            settings.Value.Extract.OpenDirectory = OpenDirectoryCondition.None;
            settings.Value.Extract.DeleteSource  = false;

            // Main
            using (var ep = new ExtractPresenter(view, model, settings, events))
            {
                view.Show();

                Assert.That(view.Visible, Is.True);
                for (var i = 0; view.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(view.Visible, Is.False, "Timeout");

                Assert.That(view.FileName, Is.EqualTo(filename));
                Assert.That(view.Count,    Is.EqualTo(view.TotalCount));
                Assert.That(view.Value,    Is.EqualTo(100));

                return view.Count;
            }
        }

        #endregion

        #region Helper

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
