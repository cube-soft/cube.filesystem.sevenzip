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
    /// FileSystem.Operator に対する拡張メソッドを定義したクラスです。
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
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// <param name="info">ファイル情報</param>
        /// 
        /* ----------------------------------------------------------------- */
        public static string GetTypeName(this Operator io, IInformation info)
            => GetTypeName(io, info?.FullName);

        /* ----------------------------------------------------------------- */
        ///
        /// GetTypeName
        ///
        /// <summary>
        /// ファイルの種類を表す文字列を取得します。
        /// </summary>
        ///
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// <param name="path">対象となるパス</param>
        /// 
        /// <remarks>
        /// 現在は Operator オブジェクトは使用していません。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public static string GetTypeName(this Operator io, string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var dest   = new SHFILEINFO();
            var result = Shell32.NativeMethods.SHGetFileInfo(path,
                0x0080, // FILE_ATTRIBUTE_NORMAL
                ref dest,
                (uint)Marshal.SizeOf(dest),
                0x0410 // SHGFI_TYPENAME | SHGFI_USEFILEATTRIBUTES
            );

            return result != IntPtr.Zero ? dest.szTypeName : null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUniqueName
        ///
        /// <summary>
        /// 指定されたパスを基にした一意なパスを取得します。
        /// </summary>
        /// 
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// <param name="path">対象となるパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetUniqueName(this Operator io, string path)
            => GetUniqueName(io, io?.Get(path));

        /* ----------------------------------------------------------------- */
        ///
        /// GetUniqueName
        ///
        /// <summary>
        /// IInformation オブジェクトを基にした一意なパスを取得します。
        /// </summary>
        /// 
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// <param name="info">ファイル情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetUniqueName(this Operator io, IInformation info)
        {
            if (info == null) return null;
            if (!info.Exists) return info.FullName;
            if (io == null) return null;

            for (var i = 2; ; ++i)
            {
                var name = $"{info.NameWithoutExtension}({i}){info.Extension}";
                var dest = io.Combine(info.DirectoryName, name);
                if (!io.Get(dest).Exists) return dest;
            }
        }
    }
}
