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
    /// ExtractSettingsViewModel
    /// 
    /// <summary>
    /// ExtractSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ExtractSettingsViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractSettingsViewModel
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="model">Model オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ExtractSettingsViewModel(ExtractSettings model)
        {
            _model = model;
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
            get { return _model.SaveLocation == SaveLocation.Others; }
            set { SetSaveLocation(SaveLocation.Others, value); }
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
            get { return _model.SaveLocation == SaveLocation.Source; }
            set { SetSaveLocation(SaveLocation.Source, value); }
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
            get { return _model.SaveLocation == SaveLocation.Runtime; }
            set { SetSaveLocation(SaveLocation.Runtime, value); }
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
        public string SaveDirectory
        {
            get { return _model.SaveDirectory; }
            set { _model.SaveDirectory = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        /// 
        /// <summary>
        /// フォルダを作成するかどうかを示す値を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool CreateDirectory
        {
            get { return _model.RootDirectory.HasFlag(DirectoryCondition.Create); }
            set { SetRootDirectory(DirectoryCondition.Create, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SkipSingleDirectory
        /// 
        /// <summary>
        /// 単一フォルダの場合にはフォルダを作成しないかどうかを示す値を取
        /// 得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool SkipSingleDirectory
        {
            get { return _model.RootDirectory.HasFlag(DirectoryCondition.SkipSingleDirectory); }
            set { SetRootDirectory(DirectoryCondition.SkipSingleDirectory, value); }
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
            get { return _model.Filtering; }
            set { _model.Filtering = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        /// 
        /// <summary>
        /// 解凍処理後に元のファイルを削除するかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool DeleteSource
        {
            get { return _model.DeleteSource; }
            set { _model.DeleteSource = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// 圧縮処理終了後にフォルダを開くかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Open
        {
            get { return _model.PostProcess.HasFlag(PostProcess.Open); }
            set
            {
                if (value) _model.PostProcess |= PostProcess.Open;
                else _model.PostProcess &= ~PostProcess.Open;
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
            get { return _model.PostProcess.HasFlag(PostProcess.SkipDesktop); }
            set
            {
                if (value) _model.PostProcess |= PostProcess.SkipDesktop;
                else _model.PostProcess &= ~PostProcess.SkipDesktop;
            }
        }

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
            if (!check || _model.SaveLocation == value) return;
            _model.SaveLocation = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetRootDirectory
        /// 
        /// <summary>
        /// RootDirectory の値を設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetRootDirectory(DirectoryCondition value, bool check)
        {
            if (check) _model.RootDirectory |= value;
            else _model.RootDirectory &= ~value;
        }

        #region Fields
        private ExtractSettings _model;
        #endregion

        #endregion
    }
}
