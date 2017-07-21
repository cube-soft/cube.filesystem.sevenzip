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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressForm
    /// 
    /// <summary>
    /// 圧縮・展開の進捗状況を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class ProgressForm : Cube.Forms.FormBase, IProgressView
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressForm()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) => Close();

            SuspendButton.Tag = false;
            SuspendButton.Click += WhenSuspendOrResume;

            _timer.Tick += (s, e) => UpdateElapseLabel();
            _timer.Interval = 500;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 進捗状況を示す値をパーセント単位で取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Value
        {
            get { return MainProgressBar.Value; }
            set
            {
                if (MainProgressBar.Value == value) return;

                MainProgressBar.Style = value > 0 ?
                                        ProgressBarStyle.Continuous :
                                        ProgressBarStyle.Marquee;

                var min = MainProgressBar.Minimum;
                var max = MainProgressBar.Maximum;
                MainProgressBar.Value = Math.Min(Math.Max(value, min), max);

                SuspendButton.Enabled = value > 0;

                UpdateTitle();
                UpdateRemainTime();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Unit
        ///
        /// <summary>
        /// 進捗状況を示す値の単位を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public int Unit
        {
            get { return MainProgressBar.Maximum; }
            set
            {
                if (MainProgressBar.Maximum == value) return;
                MainProgressBar.Maximum = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Status
        ///
        /// <summary>
        /// 現在の状況を表す文字列を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Status
        {
            get { return StatusLabel.Text; }
            set
            {
                if (StatusLabel.Text == value) return;
                StatusLabel.Text = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        ///
        /// <summary>
        /// 処理を終了したファイル数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long DoneCount
        {
            get { return _doneCount; }
            set
            {
                if (_doneCount == value) return;
                _doneCount = value;

                UpdateFileCountLabel();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// 対象とするファイル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;

                UpdateTitle();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        ///
        /// <summary>
        /// 処理対象ファイル数の合計を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long FileCount
        {
            get { return _fileCount; }
            set
            {
                if (_fileCount == value) return;
                _fileCount = value;

                UpdateFileCountLabel();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Elapsed
        ///
        /// <summary>
        /// 圧縮・展開処理開始からの経過時間を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan Elapsed => _watch.Elapsed;

        /* ----------------------------------------------------------------- */
        ///
        /// Remain
        ///
        /// <summary>
        /// 残り時間の目安を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan Remain
        {
            get { return _remain; }
            private set
            {
                if (Math.Abs((_remain - value).TotalSeconds) <= 1.0) return;
                _remain = value;

                RemainLabel.Visible = value > TimeSpan.Zero;
                RemainLabel.Text = $"{Properties.Resources.MessageRemainTime} : {GetTimeString(value)}";
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Logo
        ///
        /// <summary>
        /// ロゴ画像を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Logo
        {
            get { return HeaderPictureBox.Image; }
            set { HeaderPictureBox.Image = value; }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// タイマーを開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Start()
        {
            _watch.Start();
            _timer.Start();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Stop
        ///
        /// <summary>
        /// タイマーを停止します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Stop()
        {
            _timer.Stop();
            _watch.Stop();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// メイン画面表示時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            UpdateFileCountLabel();
            UpdateElapseLabel();
            Start();

            EventAggregator.GetEvents()?.Show.Publish();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateTitle
        ///
        /// <summary>
        /// タイトルバーの表記を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateTitle()
        {
            var ratio = (int)(Value / (double)Unit * 100.0);
            var ss = new System.Text.StringBuilder();
            ss.Append($"{ratio}%");
            if (!string.IsNullOrEmpty(FileName)) ss.Append($" - {FileName}");
            ss.Append($" - {ProductName}");
            Text = ss.ToString();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateFileCountLabel
        ///
        /// <summary>
        /// FileCountLabel の表示内容を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateFileCountLabel()
        {
            FileCountLabel.Visible = FileCount > 0;
            FileCountLabel.Text = $"{Properties.Resources.MessageFileCount} : {DoneCount:#,0} / {FileCount:#,0}";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateElapseLabel
        ///
        /// <summary>
        /// ElapseLabel の表示内容を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateElapseLabel()
            => ElapseLabel.Text = $"{Properties.Resources.MessageElapsedTime} : {GetTimeString(Elapsed)}";

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateRemainTime
        ///
        /// <summary>
        /// Remain の容を更新します。
        /// </summary>
        /// 
        /// <remarks>
        /// 表示上のバタつきを抑えるために、残り時間の 10 秒以内の増加に
        /// ついては反映しないようにしています。また、残り時間は 5 秒
        /// 単位で更新します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        private void UpdateRemainTime()
        {
            if (Value <= 1 || Elapsed <= TimeSpan.Zero) return;

            var unit  = 5L;
            var ratio = Math.Max(Unit / (double)Value - 1.0, 0.0);
            var value = Elapsed.TotalSeconds * ratio;
            var delta = value - Remain.TotalSeconds;

            if (delta >= 0.0 && delta < unit * 2) return; // hack (see remarks)
            Remain = TimeSpan.FromSeconds(((long)value / unit + 1) * unit);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTimeString
        ///
        /// <summary>
        /// TimeSpan オブジェクトを文字列に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTimeString(TimeSpan src)
            => $"{src.TotalHours:00}:{src.Minutes:00}:{src.Seconds:00}";

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSuspendOrResume
        ///
        /// <summary>
        /// 一時停止ボタンがクリックされた時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSuspendOrResume(object sender, EventArgs e)
        {
            var suspend = !(bool)SuspendButton.Tag;
            SuspendButton.Tag  = suspend;
            SuspendButton.Text = suspend ?
                                 Properties.Resources.MenuResume :
                                 Properties.Resources.MenuSuspend;

            if (suspend) Stop();
            else Start();

            EventAggregator.GetEvents()?.Suspend.Publish(suspend);
        }

        #region Fields
        private string _fileName = string.Empty;
        private long _doneCount = 0;
        private long _fileCount = 0;
        private Stopwatch _watch = new Stopwatch();
        private Timer _timer = new Timer();
        private TimeSpan _remain = TimeSpan.Zero;
        #endregion

        #endregion
    }
}
