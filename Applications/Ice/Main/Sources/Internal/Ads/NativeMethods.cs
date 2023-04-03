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

    #region Structures and values

    public enum StreamInfoLevels : int
    {
        FindStreamInfoStandard = 0,
        FindStreamInfoMaxInfoLevel
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FindStreamData
    {
        public long StreamSize;
        // TODO: name
        public fixed char cStreamName[296];
    }

    public const int ERROR_HANDLE_EOF = 38;
    public static IntPtr INVALID_HANDLE_VALUE { get; } = new IntPtr(-1);

    #endregion

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
    /// <returns>Handle or <see cref="INVALID_HANDLE_VALUE"/>.</returns>
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

    #region Enum values

    [Flags]
    public enum FileAccessMode : uint
    {
        None            = 0,
        GENERIC_READ    = 0x80000000,
        GENERIC_WRITE   = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL     = 0x10000000
    }

    [Flags]
    public enum FileShareMode : int
    {
        None              = 0,
        FILE_SHARE_DELETE = 0x00000004,
        FILE_SHARE_READ   = 0x00000001,
        FILE_SHARE_WRITE  = 0x00000002
    }

    public enum FileCreationDisposition : int
    {
        None              = 0,
        CREATE_ALWAYS     = 2,
        CREATE_NEW        = 1,
        OPEN_ALWAYS       = 4,
        OPEN_EXISTING     = 3,
        TRUNCATE_EXISTING = 5
    }

    [Flags]
    public enum FileFlagsAndAttributes : uint
    {
        None                         = 0,

        FILE_ATTRIBUTE_ARCHIVE       = 0x20,
        FILE_ATTRIBUTE_ENCRYPTED     = 0x4000,
        FILE_ATTRIBUTE_HIDDEN        = 0x2,
        FILE_ATTRIBUTE_NORMAL        = 0x80,
        FILE_ATTRIBUTE_OFFLINE       = 0x1000,
        FILE_ATTRIBUTE_READONLY      = 0x1,
        FILE_ATTRIBUTE_SYSTEM        = 0x4,
        FILE_ATTRIBUTE_TEMPORARY     = 0x100,

        FILE_FLAG_BACKUP_SEMANTICS   = 0x02000000,
        FILE_FLAG_DELETE_ON_CLOSE    = 0x04000000,
        FILE_FLAG_NO_BUFFERING       = 0x20000000,
        FILE_FLAG_OPEN_NO_RECALL     = 0x00100000,
        FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000,
        FILE_FLAG_OVERLAPPED         = 0x40000000,
        FILE_FLAG_POSIX_SEMANTICS    = 0x01000000,
        FILE_FLAG_RANDOM_ACCESS      = 0x10000000,
        FILE_FLAG_SESSION_AWARE      = 0x00800000,
        FILE_FLAG_SEQUENTIAL_SCAN    = 0x08000000,
        FILE_FLAG_WRITE_THROUGH      = 0x80000000
    }

    #endregion

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

    [Flags]
    public enum FormatMessageFlags : int
    {
        FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100,
        FORMAT_MESSAGE_ARGUMENT_ARRAY  = 0x2000,
        FORMAT_MESSAGE_FROM_HMODULE    = 0x800,
        FORMAT_MESSAGE_FROM_STRING     = 0x400,
        FORMAT_MESSAGE_FROM_SYSTEM     = 0x1000,
        FORMAT_MESSAGE_IGNORE_INSERTS  = 0x200,
        FORMAT_MESSAGE_MAX_WIDTH_MASK  = 0xFF,
        Defaults = FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ARGUMENT_ARRAY
    }

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
