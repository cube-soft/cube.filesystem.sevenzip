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
using System.Linq;
using System.Timers;
using Cube.FileSystem.SevenZip;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressFacade
    ///
    /// <summary>
    /// 圧縮および解凍処理の進捗状況を報告するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ProgressFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressFacade
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="request">リクエストオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressFacade(Request request)
        {
            Request = request;
            _timer.Elapsed += (s, e) => OnProgress(ValueEventArgs.Create(ProgressReport));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        /// 
        /// <summary>
        /// リクエストオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Request Request { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// 圧縮または展開したファイルの保存先パスを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// このプロパティは Request.Location および関連するオブジェクト
        /// から決定されます。Destination の値を設定する場合は、継承クラス
        /// にて SetDestination メソッドを実行してください。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Current
        /// 
        /// <summary>
        /// 現在処理中のファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Current { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressReport
        /// 
        /// <summary>
        /// 進捗状況の内容を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReport ProgressReport { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressInterval
        /// 
        /// <summary>
        /// 進捗状況の報告間隔を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimeSpan ProgressInterval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        #endregion

        #region Events

        #region Progress

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        /// 
        /// <summary>
        /// 進捗状況の更新時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ValueEventHandler<ArchiveReport> Progress;

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgress
        /// 
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnProgress(ValueEventArgs<ArchiveReport> e)
            => Progress?.Invoke(this, e);

        #endregion

        #region DestinationRequired

        /* ----------------------------------------------------------------- */
        ///
        /// DestinationRequired
        /// 
        /// <summary>
        /// 保存パス要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public QueryEventHandler<string, string> DestinationRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnDestinationRequired
        /// 
        /// <summary>
        /// DestinationRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void OnDestinationRequired(QueryEventArgs<string, string> e)
        {
            if (DestinationRequired != null) DestinationRequired(this, e);
            else e.Cancel = true;
            if (e.Cancel) RaiseUserCancel();
        }

        #endregion

        #region PasswordRequired

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordRequired
        /// 
        /// <summary>
        /// パスワード要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public QueryEventHandler<string, string> PasswordRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnPasswordRequired
        /// 
        /// <summary>
        /// PasswordRequired イベントを発生させます。
        /// </summary>
        /// 
        /// <remarks>
        /// PasswordRequire イベントにハンドラが設定されていない場合、
        /// SevenZip.EncryptionException 例外が送出されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnPasswordRequired(QueryEventArgs<string, string> e)
        {
            if (PasswordRequired != null) PasswordRequired(this, e);
            else e.Cancel = true;
            if (e.Cancel) RaiseUserCancel();
        }

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressStart
        /// 
        /// <summary>
        /// 進捗状況の報告を開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void ProgressStart() => _timer.Start();

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressEnd
        /// 
        /// <summary>
        /// 進捗状況の報告を停止します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void ProgressStop() => _timer.Stop();

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        /// 
        /// <summary>
        /// Destination プロパティを設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetDestination(string query = null)
        {
            switch (Request.Location)
            {
                case SaveLocation.Desktop:
                    Destination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    break;
                case SaveLocation.MyDocuments:
                    Destination = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    break;
                case SaveLocation.Runtime:
                    var e = new QueryEventArgs<string, string>(query);
                    OnDestinationRequired(e);
                    Destination = e.Result;
                    break;
                case SaveLocation.Source:
                    Destination = System.IO.Path.GetDirectoryName(Request.Sources.First());
                    break;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseUserCancel
        /// 
        /// <summary>
        /// UserCancelException を送出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseUserCancel() => throw new UserCancelException();

        #region Fields
        private Timer _timer = new Timer(100.0);
        #endregion

        #endregion
    }
}
