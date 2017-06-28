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

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressMockView
    /// 
    /// <summary>
    /// 進捗表示画面のダミークラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class ProgressMockView : MockViewBase, IProgressView
    {
        #region Properties
        public int Value { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public long DoneCount { get; set; }
        public long FileCount { get; set; }
        public TimeSpan Elapsed { get; }
        public TimeSpan Remain { get; set; }
        #endregion

        #region Methods
        public void Start() { }
        public void Stop() { }
        #endregion
    }
}
