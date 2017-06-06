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
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// Loader
    /// 
    /// <summary>
    /// 7z.dll をロードするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class Loader : IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Loader
        ///
        /// <summary>
        /// オブジェクトを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Loader(string path)
        {
            _handle = Kernel32.NativeMethods.LoadLibrary(path);
            if (_handle.IsInvalid) throw new Win32Exception("LoadLibrary");

            var ptr = Kernel32.NativeMethods.GetProcAddress(_handle, "GetHandlerProperty");
            if (ptr == IntPtr.Zero)
            {
                _handle.Close();
                throw new ArgumentException("GetProcAddress");
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// InArchive オブジェクトを生成します。
        /// </summary>
        /// 
        /// <param name="fmt">フォーマット</param>
        /// 
        /// <returns>InArchive オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive Create(Format fmt) => Create(fmt.ToClassId());

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// InArchive オブジェクトを生成します。
        /// </summary>
        /// 
        /// <param name="clsid">Class ID</param>
        /// 
        /// <returns>InArchive オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive Create(Guid clsid)
        {
            var func = Marshal.GetDelegateForFunctionPointer(
                Kernel32.NativeMethods.GetProcAddress(_handle, "CreateObject"),
                typeof(CreateObjectDelegate)
            ) as CreateObjectDelegate;

            if (func == null) return null;

            var iid = typeof(IInArchive).GUID;
            func(ref clsid, ref iid, out object result);
            return result as IInArchive;
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
        ~Loader()
        {
            Dispose(false);
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
            if (disposing)
            {
                if (_handle != null && !_handle.IsClosed) _handle.Close();
            }
            _disposed = true;
        }

        #endregion

        #endregion

        #region Fields
        private bool _disposed = false;
        private SafeLibraryHandle _handle;
        #endregion
    }
}
