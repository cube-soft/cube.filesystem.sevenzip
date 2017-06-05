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

namespace Cube.FileSystem.Files
{
    /* --------------------------------------------------------------------- */
    ///
    /// Files.Operations
    /// 
    /// <summary>
    /// FileInfo に対する拡張メソッドを定義するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Operations
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetTypeName
        ///
        /// <summary>
        /// ファイルの種類を表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetTypeName(this System.IO.FileInfo fi)
        {
            if (fi == null) return null;

            var attr   = Shell32.NativeMethods.FILE_ATTRIBUTE_NORMAL;
            var flags  = Shell32.NativeMethods.SHGFI_TYPENAME |
                         Shell32.NativeMethods.SHGFI_USEFILEATTRIBUTES;
            var shfi   = new SHFILEINFO();
            var result = Shell32.NativeMethods.SHGetFileInfo(fi.FullName,
                attr, ref shfi, (uint)Marshal.SizeOf(shfi), flags);

            return (result != IntPtr.Zero) ? shfi.szTypeName : null;
        }
    }
}
