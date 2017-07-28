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
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsPresenter
    /// 
    /// <summary>
    /// 設定画面の Presenter クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class SettingsPresenter
        : Cube.Forms.PresenterBase<ISettingsView, SettingsFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="view">View オブジェクト</param>
        /// <param name="model">設定用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsPresenter(ISettingsView view, SettingsFolder model)
            : base(view, model)
        {
            View.Product = "CubeICE";
            View.Version = $"Version {Model.Version.ToString(true)}";
            View.Load   += (s, e) => View.Bind(Model.Value);
            View.Apply  += (s, e) => Model.Save();
        }

        #endregion
    }
}
