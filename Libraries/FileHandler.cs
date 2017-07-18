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
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileHandler
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public FileHandler() : this(new FileOperator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// FileHandler
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="op">各種操作を実行するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public FileHandler(IFileOperator op)
        {
            _op = op;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// ファイルまたはディレクトリが存在するかどうかを判別します。
        /// </summary>
        /// 
        /// <param name="src">判別対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Exists(string src) => _op.Exists(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// ファイルまたはディレクトリを削除します。
        /// </summary>
        /// 
        /// <param name="src">削除するファイルのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Delete(string src)
            => Execute(nameof(Delete), () => _op.Delete(src));

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを作成します。
        /// </summary>
        /// 
        /// <param name="src">ディレクトリのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void CreateDirectory(string src)
            => Execute(nameof(CreateDirectory), () => _op.CreateDirectory(src));

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
        /// 
        /* ----------------------------------------------------------------- */
        public void Move(string src, string dest) => Move(src, dest, false);

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
        public void Move(string src, string dest, bool overwrite)
        {
            if (!overwrite || !_op.Exists(dest)) MoveCore(src, dest);
            else
            {
                var dir = _op.GetDirectoryName(src);
                var tmp = _op.Combine(dir, Guid.NewGuid().ToString("D"));

                if (!MoveCore(dest, tmp)) return;
                if (MoveCore(src, dest)) _op.Delete(tmp);
                else MoveCore(tmp, dest); // recover
            }
        }

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
        /// 
        /* ----------------------------------------------------------------- */
        public void Copy(string src, string dest) => Copy(src, dest, false);

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
        public void Copy(string src, string dest, bool overwrite)
            => Execute(nameof(Copy), () => _op.Copy(src, dest, overwrite));

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string GetFileName(string src) => _op.GetFileName(src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string GetDirectoryName(string src) => _op.GetDirectoryName(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Combine
        ///
        /// <summary>
        /// パスを結合します。
        /// </summary>
        /// 
        /// <param name="paths">結合するパス一覧</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string Combine(params string[] paths) => _op.Combine(paths);

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

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// MoveCore
        ///
        /// <summary>
        /// 移動操作を実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private bool MoveCore(string src, string dest) => Execute(nameof(Move), () =>
        {
            var dir = _op.GetDirectoryName(dest);
            if (!_op.Exists(dir)) _op.CreateDirectory(dir);
            _op.Move(src, dest);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// 操作を実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// 操作に失敗した場合、イベントハンドラで Cancel が設定されるまで
        /// 実行し続けます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private bool Execute(string name, Action action)
        {
            while (true)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception err)
                {
                    var args = KeyValueEventArgs.Create(name, err, false);
                    OnFailed(args);
                    if (args.Cancel) return false;
                }
            }
        }

        #region Fields
        private IFileOperator _op;
        #endregion

        #endregion
    }
}
