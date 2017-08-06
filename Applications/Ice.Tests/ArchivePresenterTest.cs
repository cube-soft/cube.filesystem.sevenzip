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
using Cube.FileSystem.Ice;
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
        [TestCaseSource(nameof(TestCases))]
        public async Task<long> Archive(IEnumerable<string> args, ArchiveSettings archive)
        {
            var settings = new SettingsFolder();
            var events   = new EventAggregator();
            var view     = Views.CreateProgressView();
            var request  = new Request(args.Concat(new[]
            {
                Example("Sample.txt"),
                Example("Archive"),
            }));

            // Preset
            settings.Value.Archive = archive;
            settings.Value.Archive.SaveDirectoryName = Results;
            request.DropDirectory = Result(request.DropDirectory);
            MockViewFactory.Destination = Result("Runtime");
            MockViewFactory.Password = "password"; // used by "/p" option

            // Main
            using (var ap = new ArchivePresenter(view, request, settings, events))
            {
                view.Show();

                Assert.That(view.Visible, Is.True);
                for (var i = 0; view.Visible && i < 50; ++i) await Task.Delay(100);
                Assert.That(view.Visible, Is.False, "Timeout");

                Assert.That(view.Count, Is.EqualTo(view.TotalCount));
                Assert.That(view.Value, Is.EqualTo(100));

                var name = IO.Get(view.FileName).NameWithoutExtension;
                Assert.That(name, Is.EqualTo("Sample"));

                var facade = ap.Model as ArchiveFacade;
                Assert.That(IO.Get(facade.Destination).Exists, Is.True);

                return view.Count;
            }
        }

        #endregion

        #region TestCases

        /* ----------------------------------------------------------------- */
        ///
        /// TestCases
        /// 
        /// <summary>
        /// 圧縮処理のテスト用データを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(
                    PresetMenu.Archive.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveDetail.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveSevenZip.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveBZip2.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail = false,
                    }
                ).Returns(1L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveSfx.ToArguments(),
                    new ArchiveSettings
                    {
                        SaveLocation = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveZipPassword.ToArguments().Concat(new[] { "/o:runtime" }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveSevenZip.ToArguments().Concat(new[] { "/p" }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Runtime,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail  = false,
                    }
                ).Returns(4L);

                yield return new TestCaseData(
                    PresetMenu.ArchiveGZip.ToArguments().Concat(new[]
                    {
                        "/o:source",
                        "/drop:Drop",
                    }),
                    new ArchiveSettings
                    {
                        SaveLocation  = SaveLocation.Others,
                        OpenDirectory = OpenDirectoryCondition.None,
                        DeleteOnMail  = false,
                    }
                ).Returns(1L);
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
