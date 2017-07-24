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
using System.Drawing;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MockView
    /// 
    /// <summary>
    /// Cube.Forms.IForm を最小構成で実装した Mock クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockView : Cube.Forms.IForm
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// EventAggregator
        /// 
        /// <summary>
        /// イベントを集約するためのオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEventAggregator EventAggregator { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Enabled
        /// 
        /// <summary>
        /// コントロールが有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                OnEnabledChanged(EventArgs.Empty);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Visible
        /// 
        /// <summary>
        /// コントロールが表示されているかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible == value) return;
                _visible = value;
                OnVisibleChanged(EventArgs.Empty);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Location
        /// 
        /// <summary>
        /// 表示位置を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Point Location
        {
            get { return _location; }
            set
            {
                if (_location == value) return;
                _location = value;
                OnMove(EventArgs.Empty);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        /// 
        /// <summary>
        /// 表示サイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Size Size
        {
            get { return _size; }
            set
            {
                if (_size == value) return;
                _size = value;
                OnResize(EventArgs.Empty);
            }
        }

        #endregion

        #region Events

        #region Load

        /* ----------------------------------------------------------------- */
        ///
        /// Load
        /// 
        /// <summary>
        /// オブジェクトがロードされた時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Load;

        /* ----------------------------------------------------------------- */
        ///
        /// OnLoad
        /// 
        /// <summary>
        /// Load イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnLoad(EventArgs e)
            => Load?.Invoke(this, e);

        #endregion

        #region Activated

        /* ----------------------------------------------------------------- */
        ///
        /// Activated
        /// 
        /// <summary>
        /// View がアクティブ化した時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Activated;

        /* ----------------------------------------------------------------- */
        ///
        /// OnActivated
        /// 
        /// <summary>
        /// Activated イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnActivated(EventArgs e)
            => Activated?.Invoke(this, e);

        #endregion

        #region Deactivate

        /* ----------------------------------------------------------------- */
        ///
        /// Deactivate
        /// 
        /// <summary>
        /// View が非アクティブ化した時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Deactivate;

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeactivate
        /// 
        /// <summary>
        /// Deactivate イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnDeactivate(EventArgs e)
            => Deactivate?.Invoke(this, e);

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseDeactivate
        /// 
        /// <summary>
        /// Deactivate イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void RaiseDeactivate()
            => OnDeactivate(EventArgs.Empty);

        #endregion

        #region Click

        /* ----------------------------------------------------------------- */
        ///
        /// Click
        /// 
        /// <summary>
        /// クリック時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Click;

        /* ----------------------------------------------------------------- */
        ///
        /// OnClick
        /// 
        /// <summary>
        /// Click イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnClick(EventArgs e)
            => Click?.Invoke(this, e);

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseClick
        /// 
        /// <summary>
        /// Click イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void RaiseClick()
            => OnClick(EventArgs.Empty);

        #endregion

        #region EnabledChanged

        /* ----------------------------------------------------------------- */
        ///
        /// EnabledChanged
        /// 
        /// <summary>
        /// Enabled が変更された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler EnabledChanged;

        /* ----------------------------------------------------------------- */
        ///
        /// OnEnabledChanged
        /// 
        /// <summary>
        /// EnabledChanged イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnEnabledChanged(EventArgs e)
            => EnabledChanged?.Invoke(this, e);

        #endregion

        #region VisibleChanged

        /* ----------------------------------------------------------------- */
        ///
        /// VisibleChanged
        /// 
        /// <summary>
        /// Visible が変更された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler VisibleChanged;

        /* ----------------------------------------------------------------- */
        ///
        /// OnVisibleChanged
        /// 
        /// <summary>
        /// VisibleChanged イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnVisibleChanged(EventArgs e)
            => VisibleChanged?.Invoke(this, e);

        #endregion

        #region Move

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        /// 
        /// <summary>
        /// 移動時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Move;

        /* ----------------------------------------------------------------- */
        ///
        /// OnMove
        /// 
        /// <summary>
        /// Move イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnMove(EventArgs e)
            => Move?.Invoke(this, e);

        #endregion

        #region Resize

        /* ----------------------------------------------------------------- */
        ///
        /// Resize
        /// 
        /// <summary>
        /// リサイズ時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler Resize;

        /* ----------------------------------------------------------------- */
        ///
        /// OnResize
        /// 
        /// <summary>
        /// Move イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnResize(EventArgs e)
            => Resize?.Invoke(this, e);

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Activate
        /// 
        /// <summary>
        /// View をアクティブ化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Activate() => OnActivated(EventArgs.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Close
        /// 
        /// <summary>
        /// View を閉じます。
        /// </summary>
        /// 
        /// <remarks>
        /// MockView では、Visible プロパティを false に設定します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Close() => Visible = false;

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        /// 
        /// <summary>
        /// View を表示します。
        /// </summary>
        /// 
        /// <remarks>
        /// MockView では Load イベントを発生させた後、Visible プロパティを
        /// true に設定します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Show()
        {
            OnLoad(EventArgs.Empty);
            Visible = true;
        }

        #endregion

        #region Fields
        private bool _enabled = true;
        private bool _visible = false;
        private Point _location;
        private Size _size;
        #endregion
    }
}
