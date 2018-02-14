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
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// SafeLibraryHandle
    ///
    /// <summary>
    /// DLL のハンドルを保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SafeLibraryHandle
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SafeLibraryHandle() : base(true) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ReleaseHandle
        ///
        /// <summary>
        /// ハンドルを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle() => Kernel32.NativeMethods.FreeLibrary(handle);
    }
}
