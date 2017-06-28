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

            Remain = TimeSpan.MinValue;
            ExitButton.Click += (s, e) => Close();

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

                UpdateTitle();
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
        /// 圧縮・展開処理の残り時間を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan Remain
        {
            get { return _remain; }
            set
            {
                if (_remain == value) return;
                _remain = value;

                RemainLabel.Visible = value >= TimeSpan.Zero;
                RemainLabel.Text = $"{Properties.Resources.MessageRemainTime} : {GetTimeString(value)}";
            }
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
        /// OnLoad
        ///
        /// <summary>
        /// ロード時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            UpdateFileCountLabel();
            UpdateElapseLabel();

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
            var ss = new System.Text.StringBuilder();
            ss.Append($"{Value}%");
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
            => FileCountLabel.Text = $"{Properties.Resources.MessageFileCount} : {DoneCount:#,0} / {FileCount:#,0}";

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
        /// GetTimeString
        ///
        /// <summary>
        /// TimeSpan オブジェクトを文字列に変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTimeString(TimeSpan src)
            => string.Format("{0:00}:{1:00}:{2:00}", src.TotalHours, src.Minutes, src.Seconds);

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
