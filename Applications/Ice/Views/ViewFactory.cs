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
using System.Windows.Forms;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewFactory
    ///
    /// <summary>
    /// 各種 View の生成用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewFactory
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CreateProgressView
        /// 
        /// <summary>
        /// 進捗表示用画面を生成します。
        /// </summary>
        /// 
        /// <returns>進捗表示用画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public virtual IProgressView CreateProgressView() => new ProgressForm();

        /* ----------------------------------------------------------------- */
        ///
        /// ShowPasswordView
        /// 
        /// <summary>
        /// パスワード入力画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">パスワード情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void ShowPasswordView(QueryEventArgs<string, string> e)
        {
            using (var view = new PasswordForm())
            {
                view.StartPosition = FormStartPosition.CenterParent;
                e.Cancel = view.ShowDialog() == DialogResult.Cancel;
                if (!e.Cancel) e.Result = view.Password;
            }
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Views
    ///
    /// <summary>
    /// 各種 View の生成用クラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// Views は ViewFactory のプロキシとして実装されています。
    /// 実際の View 生成コードは ViewFactory および継承クラスで実装して
    /// 下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static class Views
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        /// 
        /// <summary>
        /// Facotry オブジェクトを設定します。
        /// </summary>
        /// 
        /// <param name="factory">Factory オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure(ViewFactory factory) => _factory = factory;

        #region Factory methods

        public static IProgressView CreateProgressView()
            => _factory?.CreateProgressView();

        public static void ShowPasswordView(QueryEventArgs<string, string> e)
            => _factory?.ShowPasswordView(e);

        #endregion

        #region Fields
        private static ViewFactory _factory = new ViewFactory();
        #endregion
    }
}
