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
using System.IO;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileHandler
    /// 
    /// <summary>
    /// ファイル操作を実行するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileHandler
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// ファイルを移動します。
        /// </summary>
        /// 
        /// <param name="src">移動前のパス</param>
        /// <param name="dest">移動後のパス</param>
        /// <param name="overwrite">上書きするかどうかを表す値</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Move(string src, string dest, bool overwrite = false)
            => Execute(nameof(Move), () =>
        {
            if (!overwrite || !File.Exists(dest)) File.Move(src, dest);
            else
            {
                File.Copy(src, dest, true);
                File.Delete(src);
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// ファイルをコピーします。
        /// </summary>
        /// 
        /// <param name="src">コピー元のパス</param>
        /// <param name="dest">コピー先のパス</param>
        /// <param name="overwrite">上書きするかどうかを示す値</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Copy(string src, string dest, bool overwrite = false)
            => Execute(nameof(Copy), () => File.Copy(src, dest, overwrite));

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        /// 
        /// <param name="src">削除するファイルのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Delete(string src)
            => Execute(nameof(Delete), () => File.Delete(src));

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Failed
        ///
        /// <summary>
        /// 操作に失敗した時に発生するイベントです。
        /// </summary>
        /// 
        /// <remarks>
        /// Key には失敗したメソッド名、Value には失敗した時に送出された例外
        /// オブジェクトが設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public event KeyValueCanelEventHandler<string, Exception> Failed;

        /* ----------------------------------------------------------------- */
        ///
        /// OnFailed
        ///
        /// <summary>
        /// Failed イベントを発生させます。
        /// </summary>
        /// 
        /// <remarks>
        /// Failed イベントにハンドラが設定されていない場合、
        /// 例外をそのまま送出します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnFailed(KeyValueCancelEventArgs<string, Exception> e)
        {
            if (Failed != null) Failed(this, e);
            else throw e.Value;
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// 各種操作を実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// 操作に失敗した場合、イベントハンドラで Cancel が設定されるまで実行
        /// し続けます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Execute(string name, Action action)
        {
            var complete = false;
            while (!complete)
            {
                try
                {
                    action();
                    complete = true;
                }
                catch (Exception err)
                {
                    var args = KeyValueEventArgs.Create(name, err, false);
                    OnFailed(args);
                    complete = args.Cancel;
                }
            }
        }

        #endregion
    }
}
