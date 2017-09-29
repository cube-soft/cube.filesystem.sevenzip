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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Cube.FileSystem.SevenZip;
using Cube.FileSystem.Ice;
using Cube.Log;

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
    public abstract class ProgressFacade : IDisposable
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
        public ProgressFacade(Request request, SettingsFolder settings)
        {
            Request  = request;
            Settings = settings;
            IO       = new Operator(new Alpha());

            IO.Failed += WhenFailed;
            _timer.Elapsed += (s, e) => OnProgress(ValueEventArgs.Create(Report));
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
        /// Settings
        /// 
        /// <summary>
        /// ユーザ設定を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// 圧縮または展開したファイルの保存先パスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get { return _dest; }
            protected set
            {
                if (_dest == value) return;
                _dest = value;
                this.LogDebug($"Destination:{value}");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tmp
        /// 
        /// <summary>
        /// 一時領域用のパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Tmp
        {
            get { return _tmp; }
            private set
            {
                if (_tmp == value) return;
                _tmp = value;
                this.LogDebug($"Tmp:{value}");
            }
        }

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
        /// Report
        /// 
        /// <summary>
        /// 進捗状況の内容を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReport Report { get; protected set; } = new ArchiveReport();

        /* ----------------------------------------------------------------- */
        ///
        /// Interval
        /// 
        /// <summary>
        /// 進捗状況の報告間隔を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteMode
        /// 
        /// <summary>
        /// 上書き方法を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public OverwriteMode OverwriteMode { get; protected set; } = OverwriteMode.Query;

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        /// 
        /// <summary>
        /// ファイル操作オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Operator IO { get; }

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
            => DestinationRequired?.Invoke(this, e);

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
            => PasswordRequired?.Invoke(this, e);

        #endregion

        #region OpenDirectoryRequired

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectoryRequired
        /// 
        /// <summary>
        /// ディレクトリを開く時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event KeyValueEventHandler<string, string> OpenDirectoryRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnOpenDirectoryRequired
        /// 
        /// <summary>
        /// OpenDirectoryRequired を発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnOpenDirectoryRequired(KeyValueEventArgs<string, string> e)
            => OpenDirectoryRequired?.Invoke(this, e);

        #endregion

        #region OverwriteRequired

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteRequired
        /// 
        /// <summary>
        /// ファイルの上書き時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event OverwriteEventHandler OverwriteRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnOverwriteRequired
        /// 
        /// <summary>
        /// OverwriteRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnOverwriteRequired(OverwriteEventArgs e)
            => OverwriteRequired?.Invoke(this, e);

        #endregion

        #region MessageReceived

        /* ----------------------------------------------------------------- */
        ///
        /// MessageReceived
        /// 
        /// <summary>
        /// メッセージ受信時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event MessageEventHandler MessageReceived;

        /* ----------------------------------------------------------------- */
        ///
        /// OnMessageReceived
        /// 
        /// <summary>
        /// MessageReceived を発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMessageReceived(MessageEventArgs e)
            => MessageReceived?.Invoke(this, e);

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        /// 
        /// <summary>
        /// 処理をを開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public abstract void Start();

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        /// 
        /// <summary>
        /// 処理をキャンセルします。
        /// </summary>
        /// 
        /// <remarks>
        /// 一時停止状態になっている場合は、その状態を解除します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Cancel()
        {
            _cancel.Cancel();
            Resume();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Suspend
        /// 
        /// <summary>
        /// 処理を一時停止します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Suspend() => _wait.Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Resume
        /// 
        /// <summary>
        /// 処理を再開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Resume() => _wait.Set();

        #region Protected

        /* ----------------------------------------------------------------- */
        ///
        /// CreateInnerProgress
        /// 
        /// <summary>
        /// 内部処理用の IProgress(T) オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IProgress<ArchiveReport> CreateInnerProgress(Action<ArchiveReport> action)
            => new SuspendableProgress<ArchiveReport>(_cancel.Token, _wait, action);

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
        /// ProgressResult
        /// 
        /// <summary>
        /// 結果を設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// タイミングの関係で全ての結果が取り切れていない事があるので、
        /// 完了した結果を手動で設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected void ProgressResult()
        {
            this.LogDebug(string.Format(
                "Count:{0:#,0}\tBytes:{1:#,0}",
                Report.TotalCount,
                Report.TotalBytes
            ));

            // hack (see remarks)
            Report.Count = Report.TotalCount;
            Report.Bytes = Report.TotalBytes;

            OnProgress(ValueEventArgs.Create(Report));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        /// 
        /// <summary>
        /// ディレクトリを開きます。
        /// </summary>
        /// 
        /// <param name="path">圧縮・展開先のパス</param>
        /// <param name="mode">ポストプロセスの種類</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Open(string path, OpenDirectoryMethod mode)
        {
            if (!mode.HasFlag(OpenDirectoryMethod.Open)) return;

            var info = IO.Get(path);
            var src  = info.IsDirectory ? info.FullName : info.DirectoryName;
            var cmp  = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToLower();
            if (mode == OpenDirectoryMethod.OpenNotDesktop && src.ToLower().CompareTo(cmp) == 0) return;

            var exec = !string.IsNullOrEmpty(Settings.Value.Explorer) ?
                       Settings.Value.Explorer :
                       "explorer.exe";

            this.LogDebug($"Open:{src}\tExplorer:{exec}");

            OnOpenDirectoryRequired(KeyValueEventArgs.Create(exec, src));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error
        /// 
        /// <summary>
        /// エラー発生時の処理を実行します。
        /// </summary>
        /// 
        /// <param name="err">例外オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Error(Exception err)
        {
            this.LogError(err.ToString());
            if (!Settings.Value.ErrorReport) return;
            OnMessageReceived(new MessageEventArgs { Message = err.Message });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSaveLocation
        /// 
        /// <summary>
        /// SaveLocation および保存場所を示すパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected KeyValuePair<SaveLocation, string> GetSaveLocation(GeneralSettings settings, string query)
        {
            var key = Request.Location != SaveLocation.Unknown ?
                      Request.Location :
                      settings.SaveLocation;

            this.LogDebug(string.Format("SaveLocation:({0},{1})->{2}",
                Request.Location, settings.SaveLocation, key));

            return new KeyValuePair<SaveLocation, string>(key, GetSavePath(key, settings, query));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetTmp
        /// 
        /// <summary>
        /// Tmp にパスを設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetTmp(string directory)
            => Tmp = IO.Combine(directory, Guid.NewGuid().ToString("D"));

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ProgressFacade
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        //~ProgressFacade()
        //{
        //    Dispose(false);
        //}

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// リソースを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// リソースを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            try
            {
                IO.Failed -= WhenFailed;
                if (!string.IsNullOrEmpty(Tmp)) IO.Delete(Tmp);
            }
            catch (Exception err) { this.LogWarn(err.ToString(), err); }
            finally { _disposed = true; }
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetSavePath
        /// 
        /// <summary>
        /// 保存場所を示すパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetSavePath(SaveLocation key, GeneralSettings settings, string query)
        {
            switch (key)
            {
                case SaveLocation.Desktop:
                    return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                case SaveLocation.MyDocuments:
                    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                case SaveLocation.Runtime:
                    var e = new QueryEventArgs<string, string>(query, true);
                    OnDestinationRequired(e);
                    if (e.Cancel) throw new OperationCanceledException();
                    return e.Result;
                case SaveLocation.Source:
                    return IO.Get(Request.Sources.First()).DirectoryName;
                case SaveLocation.Drop:
                    return Request.DropDirectory;
                case SaveLocation.Others:
                default:
                    break;
            }

            return !string.IsNullOrEmpty(settings.SaveDirectoryName) ?
                   settings.SaveDirectoryName :
                   Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenFailed
        /// 
        /// <summary>
        /// Failed イベント発生時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenFailed(object sender, FailedEventArgs e)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var path in e.Paths) sb.AppendLine(path);
            sb.Append($"{e.Name} {e.Exception.Message}");

            var ev = new MessageEventArgs
            {
                Message = sb.ToString(),
                Buttons = MessageBoxButtons.RetryCancel,
                Icon    = MessageBoxIcon.Warning,
                Result  = DialogResult.Cancel,
            };

            OnMessageReceived(ev);
            this.LogWarn(sb.ToString());

            e.Cancel = ev.Result != DialogResult.Retry;
            if (e.Cancel) throw new OperationCanceledException();
        }

        #region Fields
        private bool _disposed = false;
        private string _dest;
        private string _tmp;
        private System.Timers.Timer _timer = new System.Timers.Timer(100.0);
        private CancellationTokenSource _cancel = new CancellationTokenSource();
        private ManualResetEvent _wait = new ManualResetEvent(true);
        #endregion

        #endregion
    }
}
