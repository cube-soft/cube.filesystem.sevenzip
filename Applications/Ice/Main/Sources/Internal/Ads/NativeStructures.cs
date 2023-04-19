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
/// StreamInfoLevels
///
/// <summary>
/// Specifies the stream information level.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum StreamInfoLevels : int
{
    /// <summary>Standard</summary>
    FindStreamInfoStandard = 0,
    /// <summary>Max</summary>
    FindStreamInfoMaxInfoLevel
}

/* ------------------------------------------------------------------------- */
///
/// FileCreationDisposition
///
/// <summary>
/// Specifies the file creation disposition.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum FileCreationDisposition : int
{
    /// <summary>None</summary>
    None = 0,
    /// <summary>Create always</summary>
    CREATE_ALWAYS = 2,
    /// <summary>Create</summary>
    CREATE_NEW = 1,
    /// <summary>Open always</summary>
    OPEN_ALWAYS = 4,
    /// <summary>Open only if exists</summary>
    OPEN_EXISTING = 3,
    /// <summary>Truncate only if exists</summary>
    TRUNCATE_EXISTING = 5
}

/* ------------------------------------------------------------------------- */
///
/// FileAccessMode
///
/// <summary>
/// Specifies the file access mode.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum FileAccessMode : uint
{
    /// <summary>None</summary>
    None = 0,
    /// <summary>Read</summary>
    GENERIC_READ = 0x80000000,
    /// <summary>Write</summary>
    GENERIC_WRITE = 0x40000000,
    /// <summary>Execute</summary>
    GENERIC_EXECUTE = 0x20000000,
    /// <summary>All</summary>
    GENERIC_ALL = 0x10000000
}

/* ------------------------------------------------------------------------- */
///
/// FileShareMode
///
/// <summary>
/// Specifies the file share mode.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum FileShareMode : int
{
    /// <summary>None</summary>
    None = 0,
    /// <summary>Delete</summary>
    FILE_SHARE_DELETE = 0x00000004,
    /// <summary>Read</summary>
    FILE_SHARE_READ = 0x00000001,
    /// <summary>Write</summary>
    FILE_SHARE_WRITE = 0x00000002
}

/* ------------------------------------------------------------------------- */
///
/// FileFlagsAndAttributes
///
/// <summary>
/// Specifies the file flags and attributes.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum FileFlagsAndAttributes : uint
{
    /// <summary>None</summary>
    None = 0,

    /// <summary>Archive attribute</summary>
    FILE_ATTRIBUTE_ARCHIVE = 0x20,
    /// <summary>Encrypted attribute</summary>
    FILE_ATTRIBUTE_ENCRYPTED = 0x4000,
    /// <summary>Hidden attribute</summary>
    FILE_ATTRIBUTE_HIDDEN = 0x2,
    /// <summary>Normal</summary>
    FILE_ATTRIBUTE_NORMAL = 0x80,
    /// <summary>Offline attribute</summary>
    FILE_ATTRIBUTE_OFFLINE = 0x1000,
    /// <summary>Read-only attribute</summary>
    FILE_ATTRIBUTE_READONLY = 0x1,
    /// <summary>System attribute</summary>
    FILE_ATTRIBUTE_SYSTEM = 0x4,
    /// <summary>Temporary attribute</summary>
    FILE_ATTRIBUTE_TEMPORARY = 0x100,

    /// <summary>Backup flag</summary>
    FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,
    /// <summary>Delete-on-close flag</summary>
    FILE_FLAG_DELETE_ON_CLOSE = 0x04000000,
    /// <summary>No-buffering flag</summary>
    FILE_FLAG_NO_BUFFERING = 0x20000000,
    /// <summary>Open-no-recall flag</summary>
    FILE_FLAG_OPEN_NO_RECALL = 0x00100000,
    /// <summary>Open-reparse-point flag</summary>
    FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000,
    /// <summary>Overlapped flag</summary>
    FILE_FLAG_OVERLAPPED = 0x40000000,
    /// <summary>Posix flag</summary>
    FILE_FLAG_POSIX_SEMANTICS = 0x01000000,
    /// <summary>Random-access flag</summary>
    FILE_FLAG_RANDOM_ACCESS = 0x10000000,
    /// <summary>Session-aware flag</summary>
    FILE_FLAG_SESSION_AWARE = 0x00800000,
    /// <summary>Sequential-scan flag</summary>
    FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,
    /// <summary>Write-through flag</summary>
    FILE_FLAG_WRITE_THROUGH = 0x80000000
}

/* ------------------------------------------------------------------------- */
///
/// FormatMessageFlag
///
/// <summary>
/// Specifies the flags for the message.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum FormatMessageFlags : int
{
    /// <summary>Allocate buffer flag</summary>
    FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100,
    /// <summary>Argument array flag</summary>
    FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x2000,
    /// <summary>From HModule flag</summary>
    FORMAT_MESSAGE_FROM_HMODULE = 0x800,
    /// <summary>From string flag</summary>
    FORMAT_MESSAGE_FROM_STRING = 0x400,
    /// <summary>From system flag</summary>
    FORMAT_MESSAGE_FROM_SYSTEM = 0x1000,
    /// <summary>Ignore inserts flag</summary>
    FORMAT_MESSAGE_IGNORE_INSERTS = 0x200,
    /// <summary>Max width mask</summary>
    FORMAT_MESSAGE_MAX_WIDTH_MASK = 0xFF,
    /// <summary>Default flags</summary>
    Defaults = FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ARGUMENT_ARRAY
}

/* ------------------------------------------------------------------------- */
///
/// FindStreamData
///
/// <summary>
/// Represents the find stream data.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[StructLayout(LayoutKind.Sequential)]
public unsafe struct FindStreamData
{
    /* --------------------------------------------------------------------- */
    ///
    /// StreamSize
    ///
    /// <summary>
    /// Gets or sets the stream name length.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long StreamSize;

    /* --------------------------------------------------------------------- */
    ///
    /// cStreamName
    ///
    /// <summary>
    /// Gets or sets the buffer for the stream name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public fixed char cStreamName[296];
}
