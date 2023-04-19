/* ------------------------------------------------------------------------- */
//
// This file is part of Managed NTFS Data Streams project
//
// Copyright 2020 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using System.Runtime.InteropServices;

/* ------------------------------------------------------------------------- */
///
/// NativeMethods
///
/// <summary>
/// Provides functionality to invoke the Win32 APIs.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class NativeMethods
{
    #region Data Streams

    /* ------------------------------------------------------------------------- */
    ///
    /// FindFirstStreamW
    ///
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-findfirststreamw
    /// </summary>
    ///
    /// <param name="lpFileName">Path to file.</param>
    /// <param name="infoLevel">Must be <see cref="StreamInfoLevels.FindStreamInfoStandard"/>.</param>
    /// <param name="lpFindStreamData">Pointer to <see cref="FindStreamData"/>.</param>
    /// <param name="flags">Must be 0.</param>
    ///
    /* ------------------------------------------------------------------------- */
    [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr FindFirstStreamW(string lpFileName, StreamInfoLevels infoLevel, IntPtr lpFindStreamData, int flags);

    /* ------------------------------------------------------------------------- */
    /// FindNextStreamW
    /* ------------------------------------------------------------------------- */
    [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool FindNextStreamW(IntPtr hFindStream, IntPtr lpFindStreamData);

    /* ------------------------------------------------------------------------- */
    /// FindClose
    /* ------------------------------------------------------------------------- */
    [DllImport(LibName, SetLastError = true)]
    public static extern bool FindClose(IntPtr hFindFile);

    #endregion

    #region File IO

    /* ------------------------------------------------------------------------- */
    /// CreateFileW
    /* ------------------------------------------------------------------------- */
    [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr CreateFileW(
        string lpFileName,
        FileAccessMode dwDesiredAccess,
        FileShareMode dwShareMode,
        IntPtr lpSecurityAttributes, // always 0
        FileCreationDisposition dwCreationDisposition,
        FileFlagsAndAttributes dwFlagsAndAttributes, // typically just FILE_FLAG_OVERLAPPED
        IntPtr hTemplateFile);

    /* ------------------------------------------------------------------------- */
    /// DeleteFileW
    /* ------------------------------------------------------------------------- */
    [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool DeleteFileW(string lpFileName);

    #endregion

    #region Error Messages

    /* ------------------------------------------------------------------------- */
    /// FormatMessage
    /* ------------------------------------------------------------------------- */
    [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int FormatMessage(
        FormatMessageFlags dwFlags,
        IntPtr lpSource,
        int dwMessageId,
        int dwLanguageId,
        IntPtr buffer,
        int nSize,
        IntPtr arguments);

    #endregion

    #region Fields
    private const string LibName = "kernel32";
    #endregion
}
