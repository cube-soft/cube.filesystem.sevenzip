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
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetTypeName
        ///
        /// <summary>
        /// ファイルの種類を表す文字列を取得します。
        /// </summary>
        ///
        /// <param name="info">IInformation オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public static string GetTypeName(this IInformation info)
        {
            if (info == null) return null;

            var attr   = Shell32.NativeMethods.FILE_ATTRIBUTE_NORMAL;
            var flags  = Shell32.NativeMethods.SHGFI_TYPENAME |
                         Shell32.NativeMethods.SHGFI_USEFILEATTRIBUTES;
            var shfi   = new SHFILEINFO();
            var result = Shell32.NativeMethods.SHGetFileInfo(info.FullName,
                attr, ref shfi, (uint)Marshal.SizeOf(shfi), flags);

            return (result != IntPtr.Zero) ? shfi.szTypeName : null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUniqueName
        ///
        /// <summary>
        /// IInformation オブジェクトを基にした一意なパスを取得します。
        /// </summary>
        /// 
        /// <param name="info">IInformation オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetUniqueName(this IInformation info)
            => GetUniqueName(info, new Operator());

        /* ----------------------------------------------------------------- */
        ///
        /// GetUniqueName
        ///
        /// <summary>
        /// IInformation オブジェクトを基にした一意なパスを取得します。
        /// </summary>
        /// 
        /// <param name="info">IInformation オブジェクト</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetUniqueName(this IInformation info, Operator io)
        {
            if (!info.Exists) return info.FullName;

            var path = info.FullName;
            var dir  = info.DirectoryName;
            var name = System.IO.Path.GetFileNameWithoutExtension(info.Name);
            var ext  = info.Extension;

            for (var i = 2; ; ++i)
            {
                var dest = io.Combine(dir, $"{name}({i}){ext}");
                if (!System.IO.File.Exists(dest) && !System.IO.Directory.Exists(dest)) return dest;
            }
        }

        #endregion
    }
}
