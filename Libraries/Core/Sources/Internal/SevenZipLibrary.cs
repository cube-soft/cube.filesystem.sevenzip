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
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Cube.Mixin.Assembly;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipLibrary
    ///
    /// <summary>
    /// Provides functionality of the 7z.dll.
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
        /// Initializes a new instance of the SevenZipLibrary class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SevenZipLibrary()
        {
            var dll = Io.Combine(GetType().Assembly.GetDirectoryName(), "7z.dll");
            _handle = Kernel32.NativeMethods.LoadLibrary(dll);
            if (_handle.IsInvalid) throw new Win32Exception("LoadLibrary");
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetInArchive
        ///
        /// <summary>
        /// Gets the InArchive object with the specified format.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        ///
        /// <returns>InArchive object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive GetInArchive(Format format) => GetInArchive(format.ToClassId());

        /* ----------------------------------------------------------------- */
        ///
        /// GetInArchive
        ///
        /// <summary>
        /// Gets the InArchive object with the specified class ID.
        /// </summary>
        ///
        /// <param name="clsid">Class ID.</param>
        ///
        /// <returns>InArchive object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive GetInArchive(Guid clsid)
        {
            var iid  = typeof(IInArchive).GUID;
            var func = GetDelegate();
            _ = func(ref clsid, ref iid, out object result);
            return result as IInArchive;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetOutArchive
        ///
        /// <summary>
        /// Gets the OutArchive object with the specified archive format.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        ///
        /// <returns>OutArchive object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IOutArchive GetOutArchive(Format format) => GetOutArchive(format.ToClassId());

        /* ----------------------------------------------------------------- */
        ///
        /// GetOutArchive
        ///
        /// <summary>
        /// Gets the OutArchive object with the specified class ID.
        /// </summary>
        ///
        /// <param name="clsid">Class ID.</param>
        ///
        /// <returns>OutArchive object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IOutArchive GetOutArchive(Guid clsid)
        {
            var iid  = typeof(IOutArchive).GUID;
            var func = GetDelegate();
            _ = func(ref clsid, ref iid, out object result);
            return result as IOutArchive;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
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
        /// GetDelegate
        ///
        /// <summary>
        /// Gets the delegate object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private CreateObjectDelegate GetDelegate() =>
            Marshal.GetDelegateForFunctionPointer(
                Kernel32.NativeMethods.GetProcAddress(_handle, "CreateObject"),
                typeof(CreateObjectDelegate)
            ) as CreateObjectDelegate;

        /* ----------------------------------------------------------------- */
        ///
        /// CreateObjectDelegate
        ///
        /// <summary>
        /// Represents the delegate of the CreateObject function.
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
