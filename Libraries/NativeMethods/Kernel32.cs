/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
    /// kernel32.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
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

        #region Fields
        private const string LibName = "kernel32.dll";
        #endregion
    }
}
