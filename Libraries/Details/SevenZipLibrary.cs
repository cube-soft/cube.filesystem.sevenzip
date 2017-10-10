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
using System.IO;
using System.Reflection;
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
    internal sealed class SevenZipLibrary : IDisposable
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
            var asm = Assembly.GetExecutingAssembly();
            var dir = Path.GetDirectoryName(asm.Location);
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

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~LibLoader
        ///
        /// <summary>
        /// オブジェクトを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        //~SevenZipLibrary()
        //{
        //    Dispose(false);
        //}

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
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
        void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (_handle != null && !_handle.IsClosed) _handle.Close();
            _disposed = true;
        }

        #endregion

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

        #region Fields
        private bool _disposed = false;
        private SafeLibraryHandle _handle;
        #endregion

        #endregion
    }
}
