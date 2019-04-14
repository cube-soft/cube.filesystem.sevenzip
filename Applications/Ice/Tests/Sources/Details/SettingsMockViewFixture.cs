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
using Cube.FileSystem.TestService;
using Microsoft.Win32;
using NUnit.Framework;
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsMockViewFixture
    ///
    /// <summary>
    /// テストで MockView を使用するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class SettingsMockViewFixture : FileFixture
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsMockViewFixture
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsMockViewFixture() : this(new AfsIO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsMockViewFixture
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsMockViewFixture(IO io) : base(io) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Mock
        ///
        /// <summary>
        /// ダミー用の ViewFactory を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingsMockViewFactory Mock { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// SubKeyName
        ///
        /// <summary>
        /// テスト用のレジストリ・サブキー名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string SubKeyName => @"CubeSoft\CubeIceTest";

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSettings
        ///
        /// <summary>
        /// SettingsFolder オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingsFolder CreateSettings()
        {
            var asm  = Assembly.GetExecutingAssembly();
            var dc   = Cube.DataContract.Format.Registry;
            var dest = new SettingsFolder(asm, dc, SubKeyName, IO) { AutoSave = false };
            Assert.That(dest.Location, Does.Not.StartsWith("Software"));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Setup
        ///
        /// <summary>
        /// テスト毎に実行される SetUp 処理です。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [SetUp]
        public virtual void Setup()
        {
            Mock = new SettingsMockViewFactory();
            Configurator.Views.Configure(Mock);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Teardown
        ///
        /// <summary>
        /// テスト毎に実行される TearDown 処理です。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TearDown]
        public virtual void Teardown()
        {
            using (var root = Registry.CurrentUser.OpenSubKey("Software", true))
            {
                root.DeleteSubKeyTree(SubKeyName, false);
            }
        }

        #endregion
    }
}
