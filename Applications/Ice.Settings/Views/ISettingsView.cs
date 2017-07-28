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
using System;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ISettingsView
    /// 
    /// <summary>
    /// 設定画面を表すインターフェースです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public interface ISettingsView : Cube.Forms.IForm
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Product
        /// 
        /// <summary>
        /// アプリケーション名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string Product { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        /// 
        /// <summary>
        /// バージョン情報を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string Version { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// オブジェクトを関連付けます。
        /// </summary>
        /// 
        /// <param name="settings">関連付けるオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        void Bind(Cube.FileSystem.Ice.Settings settings);

        /* ----------------------------------------------------------------- */
        ///
        /// Apply
        ///
        /// <summary>
        /// OK ボタンまたは適用ボタンのクリック時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        event EventHandler Apply;

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// キャンセルボタンのクリック時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        event EventHandler Cancel;
    }
}
