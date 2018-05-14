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
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App
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
    public partial class ProgressForm : Cube.Forms.StandardForm, IProgressView
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

            ExitButton.Click += (s, e) => EventHub.GetEvents()?.Cancel.Publish();

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
            get => MainProgressBar.Value;
            set
            {
                if (MainProgressBar.Value == value) return;

                MainProgressBar.Style = value > 0 ?
                                        ProgressBarStyle.Continuous :
                                        ProgressBarStyle.Marquee;

                var min = MainProgressBar.Minimum;
                var max = MainProgressBar.Maximum;
                MainProgressBar.Value = Math.Min(Math.Max(value, min), max);

                ExitButton.Enabled    = value > 0;
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
        /// <remarks>
        /// Unit が決定されるまでは Progress イベントが発生しないため、
        /// キャンセル不可能に設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int Unit
        {
            get => MainProgressBar.Maximum;
            set
            {
                if (MainProgressBar.Maximum == value) return;
                MainProgressBar.Maximum = value;
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
            get => _fileName;
            set
            {
                if (_fileName == value) return;
                _fileName = value;

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
            get => StatusLabel.Text;
            set
            {
                if (StatusLabel.Text == value) return;
                StatusLabel.Text = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// 処理を終了したファイル数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Count
        {
            get => _count;
            set
            {
                if (_count == value) return;
                _count = value;

                UpdateCountLabel();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalCount
        ///
        /// <summary>
        /// 処理対象ファイル数の合計を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long TotalCount
        {
            get => _totalCount;
            set
            {
                if (_totalCount == value) return;
                _totalCount = value;

                Value = 0;
                UpdateCountLabel();
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
            get => _remain;
            private set
            {
                if (Math.Abs((_remain - value).TotalSeconds) <= 1.0) return;
                _remain = value;

                RemainLabel.Visible = value > TimeSpan.Zero;
                RemainLabel.Text = string.Format("{0} : {1}",
                    Properties.Resources.MessageRemainTime,
                    ViewResource.GetTimeString(value)
                );
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
            get => HeaderPictureBox.Image;
            set => HeaderPictureBox.Image = value;
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

            UpdateCountLabel();
            UpdateElapseLabel();
            Start();

            EventHub.GetEvents()?.Show.Publish();
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
        /// UpdateCountLabel
        ///
        /// <summary>
        /// FileCountLabel の表示内容を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateCountLabel()
        {
            CountLabel.Visible = TotalCount > 0;
            CountLabel.Text = $"{Properties.Resources.MessageCount} : {Count:#,0} / {TotalCount:#,0}";
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
        private void UpdateElapseLabel() => ElapseLabel.Text = string.Format(
            "{0} : {1}",
            Properties.Resources.MessageElapsedTime,
            ViewResource.GetTimeString(Elapsed)
        );

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
        /// WhenSuspendOrResume
        ///
        /// <summary>
        /// 一時停止ボタンがクリックされた時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSuspendOrResume(object s, EventArgs e)
        {
            var suspend = !(bool)SuspendButton.Tag;
            SuspendButton.Tag  = suspend;
            SuspendButton.Text = suspend ?
                                 Properties.Resources.MenuResume :
                                 Properties.Resources.MenuSuspend;

            if (suspend) Stop();
            else Start();

            EventHub.GetEvents()?.Suspend.Publish(suspend);
        }

        #endregion

        #region Fields
        private readonly Stopwatch _watch = new Stopwatch();
        private readonly Timer _timer = new Timer();
        private string _fileName = string.Empty;
        private long _count = 0;
        private long _totalCount = 0;
        private TimeSpan _remain = TimeSpan.Zero;
        #endregion
    }
}
