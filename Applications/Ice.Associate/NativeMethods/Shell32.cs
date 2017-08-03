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
using System.Runtime.InteropServices;

namespace Cube.FileSystem.App.Ice.Associate.Shell32
{
    /* --------------------------------------------------------------------- */
    ///
    /// Shell32.NativeMethods
    /// 
    /// <summary>
    /// shell32.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SHChangeNotify
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/bb762118.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        static public extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        #region Fields
        private const string LibName = "Shell32.dll";
        #endregion
    }
}
