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
using System.Runtime.Serialization;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Settings
    /// 
    /// <summary>
    /// ユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class Settings : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        /// 
        /// <summary>
        /// 起動時にアップデートの確認を実行するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool CheckUpdate
        {
            get { return _checkUpdate; }
            set { SetProperty(ref _checkUpdate, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ErrorReport
        /// 
        /// <summary>
        /// エラーレポートを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ErrorReport
        {
            get { return _errorReport; }
            set { SetProperty(ref _errorReport, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Explorer
        /// 
        /// <summary>
        /// ファイル一覧を表示するプログラムのパスを取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// 設定値が空文字列の場合 explorer.exe が使用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Explorer
        {
            get { return _explorer; }
            set { SetProperty(ref _explorer, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        /// 
        /// <summary>
        /// 圧縮・展開時に除外するファイルまたはディレクトリ名の一覧を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Filtering
        {
            get { return _filtering; }
            set { SetProperty(ref _filtering, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTip
        /// 
        /// <summary>
        /// マウスポインタを圧縮ファイルに指定した時にファイル一覧を表示
        /// するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ToolTip
        {
            get { return _toolTip; }
            set { SetProperty(ref _toolTip, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTipCount
        /// 
        /// <summary>
        /// マウスポインタを圧縮ファイルに指定した時に一覧を表示する
        /// ファイル数を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// この値は ToolTip が有効な場合にのみ適用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int ToolTipCount
        {
            get { return _toolTipCount; }
            set { SetProperty(ref _toolTipCount, value); }
        }

        #endregion

        #region Fields
        private bool _checkUpdate = true;
        private bool _errorReport = true;
        private string _explorer = string.Empty;
        private string _filtering = ".DS_Store|Thumbs.db|__MACOSX|desktop.ini";
        private bool _toolTip = true;
        private int _toolTipCount = 5;
        #endregion
    }
}
