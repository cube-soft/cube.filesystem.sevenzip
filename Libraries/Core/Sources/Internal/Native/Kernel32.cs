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
using System.Runtime.InteropServices;
using System.Security;

namespace Cube.FileSystem.SevenZip.Kernel32
{
    /* --------------------------------------------------------------------- */
    ///
    /// Kernel32.NativeMethods
    ///
    /// <summary>
    /// Provides native methods defined in the kernel32.dll.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// LoadLibrary
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms684175.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeLibraryHandle LoadLibrary(string lpFileName);

        /* ----------------------------------------------------------------- */
        ///
        /// GetProcAddress
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms683212.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetProcAddress(
            SafeLibraryHandle hModule,
            [MarshalAs(UnmanagedType.LPStr)] string procName
        );

        /* ----------------------------------------------------------------- */
        ///
        /// FreeLibrary
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms683152.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        #endregion

        #region Fields
        private const string LibName = "kernel32.dll";
        #endregion
    }
}
