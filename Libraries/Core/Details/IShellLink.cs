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
using System.Text;
using System.Runtime.InteropServices;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// IShellLink
    /// 
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb774950.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [ComImport]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellLink
    {
        void GetPath([Out] StringBuilder pszFile, int cch, [In, Out] IntPtr pfd, int fFlags);
        void GetIDList([Out] IntPtr ppidl);
        void SetIDList([In] IntPtr pidl);
        void GetDescription([Out] StringBuilder pszName, int cch);
        void SetDescription([In] string pszName);
        void GetWorkingDirectory([Out] StringBuilder pszDir, int cch);
        void SetWorkingDirectory([In] string pszDir);
        void GetArguments([Out] StringBuilder pszArgs, int cch);
        void SetArguments([In] string pszArgs);
        void GetHotkey([Out] out ushort pwHotkey);
        void SetHotkey([In] ushort wHotkey);
        void GetShowCmd([Out] out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out] StringBuilder pszIconPath, int cch, [Out] out int piIcon);
        void SetIconLocation([In] string pszIconPath, int iIcon);
        void SetRelativePath([In] string pszPathRel, int dwReserved);
        void Resolve([In] IntPtr hwnd, int fFlags);
        void SetPath([In] string pszFile);
    }
}
