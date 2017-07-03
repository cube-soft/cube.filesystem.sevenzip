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
using System.Timers;

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
        /* ----------------------------------------------------------------- */
        public ProgressFacade()
        {
            _timer.Elapsed += (s, e) => OnProgress(EventArgs.Empty);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// 圧縮または展開したファイルの保存先パスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Current
        /// 
        /// <summary>
        /// 現在処理中のファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Current { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneSize
        /// 
        /// <summary>
        /// 展開の終了したファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long DoneSize { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileSize
        /// 
        /// <summary>
        /// 展開後のファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long FileSize { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        /// 
        /// <summary>
        /// 展開の終了したファイル数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long DoneCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        /// 
        /// <summary>
        /// 展開後のファイル数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long FileCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Percentage
        /// 
        /// <summary>
        /// 進捗率を示す値をパーセント単位で取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Percentage =>
            FileSize > 0 ?
            (int)(DoneSize / (double)FileSize * 100.0) :
            0;

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
        public EventHandler Progress;

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgress
        /// 
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnProgress(EventArgs e)
            => Progress?.Invoke(this, e);

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
            if (e.Cancel) throw new SevenZip.EncryptionException("user cancel");
        }

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgressStart
        /// 
        /// <summary>
        /// 進捗状況の報告を開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void OnProgressStart() => _timer.Start();

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgressEnd
        /// 
        /// <summary>
        /// 進捗状況の報告を停止します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void OnProgressStop() => _timer.Stop();

        #endregion

        #region Fields
        private Timer _timer = new Timer(100.0);
        #endregion
    }
}
