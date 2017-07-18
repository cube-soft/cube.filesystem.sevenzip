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
using System.ComponentModel;
using System.Collections.Generic;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// FailedEventArgs
    ///
    /// <summary>
    /// Failed イベントの情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FailedEventArgs : CancelEventArgs
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FailedEventArgs
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="name">操作に失敗したメソッド名</param>
        /// <param name="paths">失敗時に指定したパス一覧</param>
        /// <param name="err">送出された例外オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public FailedEventArgs(string name, IEnumerable<string> paths, Exception err)
            : this(name, paths, err, false) { }

        /* ----------------------------------------------------------------- */
        ///
        /// FailedEventArgs
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="name">操作に失敗したメソッド名</param>
        /// <param name="paths">失敗時に指定したパス一覧</param>
        /// <param name="err">送出された例外オブジェクト</param>
        /// <param name="cancel">キャンセルするかどうかを示す値</param>
        ///
        /* ----------------------------------------------------------------- */
        public FailedEventArgs(string name, IEnumerable<string> paths,
            Exception err, bool cancel) : base(cancel)
        {
            Name      = name;
            Paths     = paths;
            Exception = err;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        /// 
        /// <summary>
        /// 操作に失敗したメソッド名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Paths
        /// 
        /// <summary>
        /// 失敗時に指定されたパス一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Paths { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Exception
        /// 
        /// <summary>
        /// 送出された例外オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Exception Exception { get; }

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FailedEventHandler
    /// 
    /// <summary>
    /// イベントを処理するメソッドを表します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public delegate void FailedEventHandler(object sender, FailedEventArgs e);
}
