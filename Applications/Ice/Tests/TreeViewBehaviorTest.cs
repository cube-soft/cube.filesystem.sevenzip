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
using Cube.FileSystem.SevenZip.Ice.App.Settings;
using NUnit.Framework;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// TreeViewBehaviorTest
    ///
    /// <summary>
    /// TreeViewBehavior のテスト用クラスです。
    /// </summary>
    ///
    /// <remarks>
    /// TreeViewBehavior に関する多くの通常テストは ContextViewModelTest
    /// クラスで各種 ViewModel を介して実行しています。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class TreeViewBehaviorTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Throws
        ///
        /// <summary>
        /// 無効な TreeView オブジェクトを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Create_Throws() => Assert.That(
            () => new TreeViewBehavior(default(TreeView)),
            Throws.TypeOf<ArgumentException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Add_Unregistered
        ///
        /// <summary>
        /// Register 前に各種操作を実行した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test, RequiresThread(ApartmentState.STA)]
        public void Add_Unregistered() => Assert.That(
            () => new TreeViewBehavior(new TreeView()).Add(),
            Throws.TypeOf<InvalidOperationException>()
        );

        #endregion
    }
}
