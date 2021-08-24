﻿/* ------------------------------------------------------------------------- */
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
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// SafeLibraryHandle
    ///
    /// <summary>
    /// Provides the functionality to hold a DLL handle.
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
        /// Initializes a new instance of the SafeLibraryHandle class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SafeLibraryHandle() : base(true) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ReleaseHandle
        ///
        /// <summary>
        /// Releases the handle.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle() => Kernel32.NativeMethods.FreeLibrary(handle);
    }
}