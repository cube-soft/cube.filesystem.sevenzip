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
using Cube.Forms;
using Cube.Generics;
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice
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
    public abstract class ProgressFacade : DisposableBase
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
        /// <param name="settings">設定情報</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ProgressFacade(Request request, SettingsFolder settings)
        {
            Request  = request;
            Settings = settings;

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
            get => _dest;
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
            get => _tmp;
            private set
            {
                if (_tmp == value) return;
                _tmp = value;
                this.LogDebug($"Tmp:{value}");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Report
        ///
        /// <summary>
        /// 進捗状況の内容を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Report Report { get; protected set; } = new Report();

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
            get => TimeSpan.FromMilliseconds(_timer.Interval);
            set => _timer.Interval = value.TotalMilliseconds;
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
        /// I/O オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => Settings.IO;

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
        public event ValueEventHandler<Report> Progress;

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgress
        ///
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnProgress(ValueEventArgs<Report> e) =>
            Progress?.Invoke(this, e);

        #endregion

        #region ProgressReset

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressReset
        ///
        /// <summary>
        /// 進捗状況のリセット時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler ProgressReset;

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgressReset
        ///
        /// <summary>
        /// ProgressReset イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnProgressReset(EventArgs e) =>
            ProgressReset?.Invoke(this, e);

        #endregion

        #region DestinationRequested

        /* ----------------------------------------------------------------- */
        ///
        /// DestinationRequested
        ///
        /// <summary>
        /// 保存パス要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event PathQueryEventHandler DestinationRequested;

        /* ----------------------------------------------------------------- */
        ///
        /// OnDestinationRequested
        ///
        /// <summary>
        /// DestinationRequested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void OnDestinationRequested(PathQueryEventArgs e) =>
            DestinationRequested?.Invoke(this, e);

        #endregion

        #region PasswordRequested

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordRequested
        ///
        /// <summary>
        /// パスワード要求時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event QueryEventHandler<string, string> PasswordRequested;

        /* ----------------------------------------------------------------- */
        ///
        /// OnPasswordRequested
        ///
        /// <summary>
        /// PasswordRequested イベントを発生させます。
        /// </summary>
        ///
        /// <remarks>
        /// PasswordRequested イベントにハンドラが設定されていない場合、
        /// SevenZip.EncryptionException 例外が送出されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnPasswordRequested(QueryEventArgs<string, string> e) =>
            PasswordRequested?.Invoke(this, e);

        #endregion

        #region OpenDirectoryRequested

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectoryRequested
        ///
        /// <summary>
        /// ディレクトリを開く時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event KeyValueEventHandler<string, string> OpenDirectoryRequested;

        /* ----------------------------------------------------------------- */
        ///
        /// OnOpenDirectoryRequested
        ///
        /// <summary>
        /// OpenDirectoryRequested を発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnOpenDirectoryRequested(KeyValueEventArgs<string, string> e) =>
            OpenDirectoryRequested?.Invoke(this, e);

        #endregion

        #region OverwriteRequested

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteRequested
        ///
        /// <summary>
        /// ファイルの上書き時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event OverwriteEventHandler OverwriteRequested;

        /* ----------------------------------------------------------------- */
        ///
        /// OnOverwriteRequested
        ///
        /// <summary>
        /// OverwriteRequested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnOverwriteRequested(OverwriteEventArgs e) =>
            OverwriteRequested?.Invoke(this, e);

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
        protected virtual void OnMessageReceived(MessageEventArgs e) =>
            MessageReceived?.Invoke(this, e);

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
        protected IProgress<Report> CreateInnerProgress(Action<Report> action) =>
            new SuspendableProgress<Report>(_cancel.Token, _wait, action);

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
            // hack (see remarks)
            Report.Count = Report.TotalCount;
            Report.Bytes = Report.TotalBytes;

            this.LogDebug($"Count:{Report.Count:#,0}\tBytes:{Report.Bytes:#,0}");
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
            var cmp  = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var skip = mode == OpenDirectoryMethod.OpenNotDesktop && src.FuzzyEquals(cmp);

            if (skip) return;

            var exec = Settings.Value.Explorer.HasValue() ?
                       Settings.Value.Explorer :
                       "explorer.exe";

            this.LogDebug($"Open:{src.Quote()}", $"Explorer:{exec.Quote()}");
            OnOpenDirectoryRequested(KeyValueEventArgs.Create(exec, src));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error
        ///
        /// <summary>
        /// エラー発生時の処理を実行します。
        /// </summary>
        ///
        /// <param name="error">例外オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Error(Exception error)
        {
            this.LogError(error);
            if (!Settings.Value.ErrorReport) return;
            OnMessageReceived(new MessageEventArgs(GetMessage(error), Properties.Resources.TitleError));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetMessage
        ///
        /// <summary>
        /// Gets the message from the specified exception.
        /// </summary>
        ///
        /// <param name="src">Exception object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual string GetMessage(Exception src) => src.Message;

        /* ----------------------------------------------------------------- */
        ///
        /// GetSaveLocation
        ///
        /// <summary>
        /// SaveLocation および保存場所を示すパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected KeyValuePair<SaveLocation, string> GetSaveLocation(ArchiveSettingsBase settings, Format format, string query)
        {
            var key = Request.Location != SaveLocation.Unknown ?
                      Request.Location :
                      settings.SaveLocation;

            this.LogDebug(string.Format("SaveLocation:({0},{1})->{2}",
                Request.Location, settings.SaveLocation, key));

            return new KeyValuePair<SaveLocation, string>(key, GetSavePath(key, settings, format, query));
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
        {
            if (!Tmp.HasValue()) Tmp = IO.Combine(directory, Guid.NewGuid().ToString("D"));
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを解放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
                _wait.Dispose();
            }

            IO.Failed -= WhenFailed;
            IO.TryDelete(Tmp);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSavePath
        ///
        /// <summary>
        /// 保存場所を示すパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetSavePath(SaveLocation key, ArchiveSettingsBase settings, Format format, string query)
        {
            switch (key)
            {
                case SaveLocation.Desktop:
                    return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                case SaveLocation.MyDocuments:
                    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                case SaveLocation.Runtime:
                    var e = new PathQueryEventArgs(query, format, true);
                    OnDestinationRequested(e);
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

            return settings.SaveDirectoryName.HasValue() ?
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
        private void WhenFailed(object s, FailedEventArgs e)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var path in e.Paths) sb.AppendLine(path);
            sb.Append($"{e.Name} {e.Exception.Message}");

            var ev = new MessageEventArgs(
                sb.ToString(),
                Properties.Resources.TitleError,
                MessageBoxButtons.RetryCancel,
                MessageBoxIcon.Warning
            );

            OnMessageReceived(ev);
            this.LogWarn(sb.ToString());

            e.Cancel = ev.Result != DialogResult.Retry;
            if (e.Cancel) throw new OperationCanceledException();
        }

        #endregion

        #region Fields
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(100.0);
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly ManualResetEvent _wait = new ManualResetEvent(true);
        private string _dest;
        private string _tmp;
        #endregion
    }
}
