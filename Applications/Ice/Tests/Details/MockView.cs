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
    /// MockViewBase
    /// 
    /// <summary>
    /// ダミー View の基底クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockViewBase : Cube.Forms.IForm
    {
        #region Properties
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        #endregion

        #region Events
        public event EventHandler Load;
        public event EventHandler Activated;
        public event EventHandler Deactivate;
        public event EventHandler Click;
        public event EventHandler EnabledChanged;
        public event EventHandler VisibleChanged;
        public event EventHandler Move;
        public event EventHandler Resize;
        #endregion

        #region Methods
        public void Activate() { }
        public void Close() { }
        public void Show() { }
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MockViewFactory
    ///
    /// <summary>
    /// 各種ダミー View の生成用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockViewFactory : ViewFactory
    {
        public override IProgressView CreateProgressView()
            => new ProgressMockView();

        public override void ShowPasswordView(QueryEventArgs<string, string> e)
        {
            e.Cancel = false;
            e.Result = "password";
        }
    }
}
