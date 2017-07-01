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
using System.Threading;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MockViewFactory
    ///
    /// <summary>
    /// 各種ダミー View の生成および設定用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockViewFactory : ViewFactory
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        /// 
        /// <summary>
        /// テストに必要な設定を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure()
        {
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }
            Views.Configure(new MockViewFactory());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProgressView
        /// 
        /// <summary>
        /// IProgressView オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override IProgressView CreateProgressView()
            => new ProgressMockView();

        /* ----------------------------------------------------------------- */
        ///
        /// ShowPasswordView
        /// 
        /// <summary>
        /// パスワードを設定します。
        /// </summary>
        /// 
        /// <param name="e">パスワード設定用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowPasswordView(QueryEventArgs<string, string> e)
        {
            e.Cancel = false;
            e.Result = "password";
        }
    }
}
