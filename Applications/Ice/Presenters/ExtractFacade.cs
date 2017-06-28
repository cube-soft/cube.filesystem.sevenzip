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
using Cube.Log;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractFacade
    ///
    /// <summary>
    /// 展開処理を実行するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractFacade : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFacade
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="src">圧縮ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(string src)
        {
            Source = src;
            Destination = System.IO.Path.GetDirectoryName(Source);
            _timer.Elapsed += (s, e) => Progress?.Invoke(this, e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        /// 
        /// <summary>
        /// 圧縮ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// 展開したファイルの保存先パスを取得または設定します。
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
        public string Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneSize
        /// 
        /// <summary>
        /// 展開の終了したファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long DoneSize
        {
            get { return _doneSize; }
            private set { SetProperty(ref _doneSize, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileSize
        /// 
        /// <summary>
        /// 展開後のファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long FileSize
        {
            get { return _fileSize; }
            private set { SetProperty(ref _fileSize, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        /// 
        /// <summary>
        /// 展開の終了したファイル数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long DoneCount
        {
            get { return _doneCount; }
            set { SetProperty(ref _doneCount, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        /// 
        /// <summary>
        /// 展開後のファイル数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long FileCount
        {
            get { return _fileCount; }
            set { SetProperty(ref _fileCount, value); }
        }

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

        #endregion

        #region Events

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
        /// PasswordRequired
        /// 
        /// <summary>
        /// パスワード要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public QueryEventHandler<string, string> PasswordRequired;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        /// 
        /// <summary>
        /// 展開を開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Start()
        {
            using (var reader = new SevenZip.ArchiveReader())
            {
                try
                {
                    reader.Open(Source);
                    Collect(reader, out string password);
                    Extract(reader, password);
                }
                catch (SevenZip.EncryptionException /* err */) { /* user cancel */ }
                catch (Exception err) { this.LogWarn(err.ToString(), err); }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Collect
        /// 
        /// <summary>
        /// 展開処理に関連する各種情報を収集します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Collect(SevenZip.ArchiveReader reader, out string password)
        {
            FileCount = reader.Items.Count;

            var query = new QueryEventArgs<string, string>(Source);
            var done  = false;
            var size  = 0L;

            foreach (var item in reader.Items)
            {
                size += item.Size;
                if (item.Encrypted && !done)
                {
                    RaisePasswordRequired(this, query);
                    done = true;
                }
            }

            FileSize = size;
            password = query.Result ?? string.Empty;

            this.LogDebug($"Count:{FileCount:#,0}\tSize:{FileSize:#,0}\tPath:{Source}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// ファイルを展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(SevenZip.ArchiveReader reader, string password)
        {
            try
            {
                _timer.Start(); // Progress timer
                foreach (var item in reader.Items) Extract(item, password);
                Progress?.Invoke(this, EventArgs.Empty);
            }
            finally { _timer.Stop(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// ファイルを展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(SevenZip.ArchiveItem src, string password)
        {
            var done = DoneSize;
            ValueEventHandler<long> h = (s, e) => DoneSize = done + e.Value;

            try
            {
                Current = src.Path;
                src.PasswordRequired += RaisePasswordRequired;
                src.Progress += h;
                src.Extract(Destination, password);
                DoneCount++;
            }
            finally
            {
                src.PasswordRequired -= RaisePasswordRequired;
                src.Progress -= h;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaisePasswordRequired
        /// 
        /// <summary>
        /// PasswordRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaisePasswordRequired(object sender, QueryEventArgs<string, string> e)
        {
            if (PasswordRequired != null) PasswordRequired(this, e);
            else e.Cancel = true;
            if (e.Cancel) throw new SevenZip.EncryptionException("user cancel");
        }

        #region Fields
        private Timer _timer = new Timer(100.0);
        private string _current = string.Empty;
        private long _doneSize = 0;
        private long _fileSize = 0;
        private long _doneCount = 0;
        private long _fileCount = 0;
        #endregion

        #endregion
    }
}
