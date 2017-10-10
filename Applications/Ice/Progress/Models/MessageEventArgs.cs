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
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// MessageEventArgs
    /// 
    /// <summary>
    /// メッセージボックスに表示する情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MessageEventArgs : EventArgs
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Title
        /// 
        /// <summary>
        /// メッセージボックスのタイトルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Message
        /// 
        /// <summary>
        /// メッセージを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Message { get; set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Icon
        /// 
        /// <summary>
        /// メッセージボックスに表示するアイコンを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MessageBoxIcon Icon { get; set; } = MessageBoxIcon.Error;

        /* ----------------------------------------------------------------- */
        ///
        /// Buttons
        /// 
        /// <summary>
        /// メッセージボックスに表示するボタンを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.OK;

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        /// 
        /// <summary>
        /// ユーザがどのボタンをクリックしたかを示す値を取得または設定
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DialogResult Result { get; set; } = DialogResult.OK;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MessageEventHandler
    /// 
    /// <summary>
    /// メッセージボックスを表示するための delegate です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);
}
