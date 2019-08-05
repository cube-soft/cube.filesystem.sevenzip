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
using Cube.Mixin.Assembly;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipLibrary
    ///
    /// <summary>
    /// 7z.dll を扱うためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class SevenZipLibrary : DisposableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SevenZipLibrary
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SevenZipLibrary()
        {
            var asm = GetType().Assembly;
            var dir = asm.GetDirectoryName();
            _handle = Kernel32.NativeMethods.LoadLibrary(Path.Combine(dir, "7z.dll"));
            if (_handle.IsInvalid) throw new Win32Exception("LoadLibrary");
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetInArchive
        ///
        /// <summary>
        /// InArchive オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="format">フォーマット</param>
        ///
        /// <returns>InArchive オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive GetInArchive(Format format) => GetInArchive(format.ToClassId());

        /* ----------------------------------------------------------------- */
        ///
        /// GetInArchive
        ///
        /// <summary>
        /// InArchive オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="clsid">Class ID</param>
        ///
        /// <returns>InArchive オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive GetInArchive(Guid clsid)
        {
            var func = Marshal.GetDelegateForFunctionPointer(
                Kernel32.NativeMethods.GetProcAddress(_handle, "CreateObject"),
                typeof(CreateObjectDelegate)
            ) as CreateObjectDelegate;

            Debug.Assert(func != null);

            var iid = typeof(IInArchive).GUID;
            func(ref clsid, ref iid, out object result);
            return result as IInArchive;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetOutArchive
        ///
        /// <summary>
        /// OutArchive オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="format">フォーマット</param>
        ///
        /// <returns>OutArchive オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IOutArchive GetOutArchive(Format format) => GetOutArchive(format.ToClassId());

        /* ----------------------------------------------------------------- */
        ///
        /// GetOutArchive
        ///
        /// <summary>
        /// OutArchive オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="clsid">Class ID</param>
        ///
        /// <returns>OutArchive オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IOutArchive GetOutArchive(Guid clsid)
        {
            var func = Marshal.GetDelegateForFunctionPointer(
                Kernel32.NativeMethods.GetProcAddress(_handle, "CreateObject"),
                typeof(CreateObjectDelegate)
            ) as CreateObjectDelegate;

            Debug.Assert(func != null);

            var iid = typeof(IOutArchive).GUID;
            func(ref clsid, ref iid, out object result);
            return result as IOutArchive;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (_handle != null && !_handle.IsClosed) _handle.Close();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateObjectDelegate
        ///
        /// <summary>
        /// CreateObject のデリゲートです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int CreateObjectDelegate(
            [In] ref Guid classID,
            [In] ref Guid interfaceID,
            [MarshalAs(UnmanagedType.Interface)] out object outObject
        );

        #endregion

        #region Fields
        private readonly SafeLibraryHandle _handle;
        #endregion
    }
}
