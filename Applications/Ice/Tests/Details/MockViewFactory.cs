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
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// ShowSaveFileView または ShowSaveDirectoryView で設定するパスを
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string Destination { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        /// 
        /// <summary>
        /// ShowPasswordView で設定するパスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string Password { get; set; }

        #endregion

        #region Methods

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
        /// Reset
        /// 
        /// <summary>
        /// 各種プロパティの値をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Reset()
        {
            Destination = string.Empty;
            Password = string.Empty;
        }

        #endregion

        #region ViewFactory

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
        /// ShowSaveFileView
        /// 
        /// <summary>
        /// 保存ファイル名を選択する画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">パスを保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowSaveFileView(QueryEventArgs<string, string> e)
        {
            e.Cancel = string.IsNullOrEmpty(Destination);
            e.Result = Destination;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowSaveDirectoryView
        /// 
        /// <summary>
        /// 保存ディレクトリ名を選択する画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">パスを保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowSaveDirectoryView(QueryEventArgs<string, string> e)
        {
            e.Cancel = string.IsNullOrEmpty(Destination);
            e.Result = Destination;
        }

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
            e.Cancel = string.IsNullOrEmpty(Password);
            e.Result = Password;
        }

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
        public override void ShowPasswordConfirmView(QueryEventArgs<string, string> e)
        {
            e.Cancel = string.IsNullOrEmpty(Password);
            e.Result = Password;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowOverwriteView
        /// 
        /// <summary>
        /// 上書き確認用画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowOverwriteView(QueryEventArgs<OverwriteInfo, OverwriteMode> e)
        {
            e.Cancel = false;
            e.Result = OverwriteMode.AlwaysYes;
        }

        #endregion
    }
}
