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
using System;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathQueryEventArgs
    ///
    /// <summary>
    /// パス情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PathQueryEventArgs : QueryMessage<string, string>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// PathQueryEventArgs
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="query">ソースファイルのパス</param>
        /// <param name="format">圧縮フォーマット</param>
        /// <param name="cancel">キャンセル状態</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathQueryEventArgs(string query, Format format, bool cancel)
        {
            Query  = query;
            Format = format;
            Cancel = cancel;
        }

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 圧縮ファイルのフォーマットを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PathQueryEventHandler
    ///
    /// <summary>
    /// 保存パスを指定するダイアログを表示するための delegate です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public delegate void PathQueryEventHandler(object sender, PathQueryEventArgs e);
}
