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
using System.Threading;
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

            IO.Failed += (s, e) => RaiseFailed(e);
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
            protected set
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
        /// ProgressReport
        /// 
        /// <summary>
        /// 進捗状況の内容を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveReport ProgressReport { get; protected set; } = new ArchiveReport();

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
            => new SuspendableProgress<ArchiveReport>(_wait, action);

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
        protected void Open(string path, OpenDirectoryCondition mode)
        {
            if (mode == OpenDirectoryCondition.None) return;

            var info = IO.Get(path);
            var src  = info.IsDirectory ? info.FullName : info.DirectoryName;
            var cmp  = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).ToLower();
            if (mode == OpenDirectoryCondition.OpenNotDesktop && src.ToLower().CompareTo(cmp) == 0) return;

            var exec = !string.IsNullOrEmpty(Settings.Value.Explorer) ?
                       Settings.Value.Explorer :
                       "explorer.exe";

            this.LogDebug($"Open:{src}\tExplorer:{exec}");
            System.Diagnostics.Process.Start(exec, $"\"{src}\"");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        /// 
        /// <summary>
        /// Destination プロパティを設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetDestination(GeneralSettings settings, string query)
        {
            var value = Request.Location != SaveLocation.Unknown ?
                        Request.Location :
                        settings.SaveLocation;

            this.LogDebug(string.Format("SaveLocation:({0},{1})->{2}",
                Request.Location, settings.SaveLocation, value));

            switch (value)
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
                    Destination = IO.Get(Request.Sources.First()).DirectoryName;
                    break;
                case SaveLocation.Drop:
                    Destination = Request.DropDirectory;
                    break;
                case SaveLocation.Others:
                    Destination = !string.IsNullOrEmpty(settings.SaveDirectoryName) ?
                                  settings.SaveDirectoryName :
                                  Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    break;
            }
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
        ~ProgressFacade()
        {
            Dispose(false);
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                if (string.IsNullOrEmpty(Tmp)) return;
                else if (IO.Get(Tmp).Exists) IO.Delete(Tmp);
            }
            catch (Exception err) { this.LogWarn(err.ToString(), err); }
            finally { _disposed = true; }
        }

        #endregion

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

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseFailed
        /// 
        /// <summary>
        /// Failed イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseFailed(FailedEventArgs e)
        {
            this.LogWarn(e.Name);
            foreach (var path in e.Paths) this.LogWarn(path);
            this.LogWarn(e.Exception.ToString(), e.Exception);

            e.Cancel = true;
        }

        #region Fields
        private bool _disposed = false;
        private string _dest;
        private string _tmp;
        private System.Timers.Timer _timer = new System.Timers.Timer(100.0);
        private ManualResetEvent _wait = new ManualResetEvent(true);
        #endregion

        #endregion
    }
}
