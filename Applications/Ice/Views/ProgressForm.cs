/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
    public partial class ProgressForm : Form
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
                MainProgressBar.Value = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileIndex
        ///
        /// <summary>
        /// 現在処理中のファイルのインデックスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int FileIndex
        {
            get { return _fileIndex; }
            set
            {
                if (_fileIndex == value) return;
                _fileIndex = value;

                UpdateFileCountLabel();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        ///
        /// <summary>
        /// 処理対象ファイルの合計値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int FileCount
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
                RemainLabel.Text = $"{Properties.Resources.MessageRemainTime} {GetTimeString(value)}";
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnShown
        ///
        /// <summary>
        /// 初回表示時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            UpdateFileCountLabel();
            UpdateElapseLabel();

            _timer.Start();
            _watch.Start();
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
            => FileCountLabel.Text = $"{Properties.Resources.MessageFileCount} {FileIndex} / {FileCount}";

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
            => ElapseLabel.Text = $"{Properties.Resources.MessageElapsedTime} {GetTimeString(Elapsed)}";

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
        private int _fileIndex = 0;
        private int _fileCount = 0;
        private Stopwatch _watch = new Stopwatch();
        private Timer _timer = new Timer();
        private TimeSpan _remain = TimeSpan.Zero;
        #endregion

        #endregion
    }
}
