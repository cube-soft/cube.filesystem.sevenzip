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
using Cube.FileSystem.SevenZip.Ice;

namespace Cube.FileSystem.SevenZip.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// GeneralSettingsViewModel
    ///
    /// <summary>
    /// GeneralSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class GeneralSettingsViewModel<TModel> : ObservableProperty
        where TModel : GeneralSettings
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// GeneralSettingsViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">Model オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public GeneralSettingsViewModel(TModel model)
        {
            Model = model;
            Model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOthers
        ///
        /// <summary>
        /// SaveLocation.Others かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveOthers
        {
            get => Model.SaveLocation == SaveLocation.Others;
            set => SetSaveLocation(SaveLocation.Others, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveSource
        ///
        /// <summary>
        /// SaveLocation.Source かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveSource
        {
            get => Model.SaveLocation == SaveLocation.Source;
            set => SetSaveLocation(SaveLocation.Source, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveRuntime
        ///
        /// <summary>
        /// SaveLocation.Runtime かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveRuntime
        {
            get => Model.SaveLocation == SaveLocation.Runtime;
            set => SetSaveLocation(SaveLocation.Runtime, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectory
        ///
        /// <summary>
        /// 保存ディレクトリのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SaveDirectoryName
        {
            get => Model.SaveDirectoryName;
            set => Model.SaveDirectoryName = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        ///
        /// <summary>
        /// 特定のファイルまたはディレクトリをフィルタリングするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Filtering
        {
            get => Model.Filtering;
            set => Model.Filtering = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// 圧縮処理終了後にフォルダを開くかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool OpenDirectory
        {
            get => Model.OpenDirectory.HasFlag(OpenDirectoryMethod.Open);
            set
            {
                if (value) Model.OpenDirectory |= OpenDirectoryMethod.Open;
                else Model.OpenDirectory &= ~OpenDirectoryMethod.Open;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SkipDesktop
        ///
        /// <summary>
        /// 後処理時に対象がデスクトップの場合にスキップするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SkipDesktop
        {
            get => Model.OpenDirectory.HasFlag(OpenDirectoryMethod.SkipDesktop);
            set
            {
                if (value) Model.OpenDirectory |= OpenDirectoryMethod.SkipDesktop;
                else Model.OpenDirectory &= ~OpenDirectoryMethod.SkipDesktop;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Model
        ///
        /// <summary>
        /// Model オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected TModel Model { get; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetSaveLocation
        ///
        /// <summary>
        /// SaveLocation の値を設定します。
        /// </summary>
        ///
        /// <remarks>
        /// SaveLocation は GUI 上は RadioButton で表現されています。
        /// そこで、SetSaveLocation() では Checked = true のタイミングで
        /// 値の内容を更新する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetSaveLocation(SaveLocation value, bool check)
        {
            if (!check || Model.SaveLocation == value) return;
            Model.SaveLocation = value;
        }

        #endregion
    }
}
