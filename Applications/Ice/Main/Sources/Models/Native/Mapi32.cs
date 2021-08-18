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

namespace Cube.FileSystem.SevenZip.Ice.Mapi32
{
    /* --------------------------------------------------------------------- */
    ///
    /// MapiMessage
    ///
    /// <summary>
    /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/dd296732.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class MapiMessage
    {
        public int reserved;
        public string subject;
        public string noteText;
        public string messageType;
        public string dateReceived;
        public string conversationID;
        public int flags;
        public IntPtr originator;
        public int recipCount;
        public IntPtr recips;
        public int fileCount;
        public IntPtr files;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MapiFileDesc
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd296737.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class MapiFileDesc
    {
        public int reserved;
        public int flags;
        public int position;
        public string path;
        public string name;
        public IntPtr type;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MapiRecipDesc
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd296720.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class MapiRecipDesc
    {
        public int reserved;
        public int recipClass;
        public string name;
        public string address;
        public int eIDSize;
        public IntPtr entryID;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Mapi32.NativeMethods
    ///
    /// <summary>
    /// mapi32.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        /* ----------------------------------------------------------------- */
        ///
        /// MAPISendMail
        ///
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/windows/desktop/dd296721.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport("mapi32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int MAPISendMail(
            IntPtr lhSession,
            IntPtr ulUIParam,
            MapiMessage lpMessage,
            int flFlags,
            int ulReserved
        );
    }
}
