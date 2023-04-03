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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

internal static class Interop
{
    #region Stream Enumerators
    public static IEnumerable<FileDataStream> EnumerateDataStreams(string src)
    {
        if (string.IsNullOrEmpty(src)) throw new ArgumentNullException(nameof(src));

        // init locals
        var hFindStream = NativeMethods.INVALID_HANDLE_VALUE;

        try
        {
            // check if we can get any stream
            var first = FindFirstStream(src, out hFindStream);
            if (!first.HasValue) yield break;

            // Extract stream info
            ExtractStreamInfo(first.Value.PtrToString(), out var streamName, out var streamType);
            yield return new(src, streamName, first.Value.StreamSize, streamType);

            // Extract more streams until we run out
            while (true)
            {
                var fsd = FindNextStream(hFindStream);
                if (!fsd.HasValue) break;
                ExtractStreamInfo(fsd.Value.PtrToString(), out streamName, out streamType);
                yield return new(src, streamName, fsd.Value.StreamSize, streamType);
            }
        }
        finally
        {
            if (hFindStream != NativeMethods.INVALID_HANDLE_VALUE) FindClose(hFindStream);
        }
    }

    // enumerator workarounds
    private static unsafe NativeMethods.FindStreamData? FindFirstStream(string lpFileName, out IntPtr hFindStream)
    {
        NativeMethods.FindStreamData data = new();
        IntPtr lpFindStreamData = new IntPtr(&data);
        hFindStream = NativeMethods.FindFirstStreamW(lpFileName, NativeMethods.StreamInfoLevels.FindStreamInfoStandard, lpFindStreamData, 0);
        if (hFindStream == NativeMethods.INVALID_HANDLE_VALUE)
        {
            var err = Marshal.GetLastWin32Error();
            if (err == NativeMethods.ERROR_HANDLE_EOF)
                return default;

            ThrowErrorAsException(err, lpFileName);
        }

        Logger.Debug($"{lpFileName} -> {new string(data.cStreamName)}");
        return data;
    }

    private static unsafe NativeMethods.FindStreamData? FindNextStream(IntPtr hFindStream)
    {
        NativeMethods.FindStreamData data = new();
        IntPtr lpFindStreamData = new IntPtr(&data);
        if (!NativeMethods.FindNextStreamW(hFindStream, lpFindStreamData))
        {
            var err = Marshal.GetLastWin32Error();
            if (err == NativeMethods.ERROR_HANDLE_EOF)
                return default;

            ThrowErrorAsException(err, null);
        }

        return data;
    }

    private static unsafe void FindClose(IntPtr hFindStream)
    {
        if (!NativeMethods.FindClose(hFindStream))
            ThrowErrorAsException(Marshal.GetLastWin32Error(), null);
    }
    #endregion

    #region IO Helpers
    public static FileStream Open(FileDataStream ads, FileMode mode, FileAccess access, FileShare share)
    {
        if (ads.Type != FileDataStreamType.Data)
            throw new InvalidOperationException("Only $DATA streams can be opened for reading or writing.");

        if (ads.Name.Length == 0) // default stream
            return Io.Open(ads.Source);

        var nmode   = ManagedToNative(mode);
        var nflags  = NativeMethods.FileFlagsAndAttributes.FILE_FLAG_OVERLAPPED;
        var naccess = ManagedToNative(access);
        var nshare  = ManagedToNative(share);

        var lpFileName = $"{ads.Source}:{ads.Name}";

        var hFile = NativeMethods.CreateFileW(lpFileName, naccess, nshare, IntPtr.Zero, nmode, nflags, IntPtr.Zero);
        if (hFile == NativeMethods.INVALID_HANDLE_VALUE)
        {
            ThrowErrorAsException(Marshal.GetLastWin32Error(), lpFileName);
            return null;
        }

        return new FileStream(new SafeFileHandle(hFile, true), access, 4096, true);
    }

    public static void Delete(FileDataStream ads)
    {
        if (ads.Type != FileDataStreamType.Data)
            throw new InvalidOperationException("Only $DATA streams can be deleted.");

        if (ads.Name.Length == 0) // default stream
        {
            Io.Delete(ads.Source);
            return;
        }

        var lpFileName = $"{ads.Source}:{ads.Name}";
        if (!NativeMethods.DeleteFileW(lpFileName))
            ThrowErrorAsException(Marshal.GetLastWin32Error(), lpFileName);
    }

    private static NativeMethods.FileShareMode ManagedToNative(FileShare share)
    {
        var mode = NativeMethods.FileShareMode.None;

        if ((share & FileShare.Delete) == FileShare.Delete) mode |= NativeMethods.FileShareMode.FILE_SHARE_DELETE;
        if ((share & FileShare.Read) == FileShare.Read) mode |= NativeMethods.FileShareMode.FILE_SHARE_READ;
        if ((share & FileShare.Write) == FileShare.Write) mode |= NativeMethods.FileShareMode.FILE_SHARE_WRITE;

        return mode;
    }

    private static NativeMethods.FileAccessMode ManagedToNative(FileAccess access)
    {
        var mode = NativeMethods.FileAccessMode.None;

        if ((access & FileAccess.Read) == FileAccess.Read) mode |= NativeMethods.FileAccessMode.GENERIC_READ;
        if ((access & FileAccess.Write) == FileAccess.Write) mode |= NativeMethods.FileAccessMode.GENERIC_WRITE;

        return mode;
    }

    private static NativeMethods.FileCreationDisposition ManagedToNative(FileMode mode) =>  mode switch
    {
        FileMode.CreateNew    => NativeMethods.FileCreationDisposition.CREATE_NEW,
        FileMode.Create       => NativeMethods.FileCreationDisposition.CREATE_ALWAYS,
        FileMode.Open         => NativeMethods.FileCreationDisposition.OPEN_EXISTING,
        FileMode.OpenOrCreate => NativeMethods.FileCreationDisposition.OPEN_ALWAYS,
        FileMode.Truncate     => NativeMethods.FileCreationDisposition.TRUNCATE_EXISTING,
        FileMode.Append       => NativeMethods.FileCreationDisposition.OPEN_ALWAYS,
        _                     => NativeMethods.FileCreationDisposition.None,
    };

    #endregion

    #region Helpers
    private static unsafe string PtrToString(this NativeMethods.FindStreamData fsd) => new(fsd.cStreamName);

    private static void ExtractStreamInfo(string s, out string name, out FileDataStreamType type)
    {
        name = string.Empty;
        type = FileDataStreamType.Unknown;

        if (string.IsNullOrEmpty(s) || s[0] != ':') return;

        var pos = s.IndexOf(':', 1);
        if (pos < 0) return;
        if (pos - 1 > 0) name = s.Substring(1, pos - 1);
        type = FileDataStreamHelper.GetStreamType(s.Substring(pos + 1));
    }

    private static void ThrowErrorAsException(int code, string fpath)
    {
        fpath ??= "<null>";
        throw code switch
        {
            2   => new FileNotFoundException("Specified file was not found.", fpath),
            3   => new DirectoryNotFoundException($"Could not find a part of the path \"{fpath}\"."),
            5   => new UnauthorizedAccessException($"Access to the path \"{fpath}\" was denied."),
            15  => new DriveNotFoundException($"Access to the path \"{fpath}\" was denied."),
            32  => new IOException($"The process cannot access the file \"{fpath}\" because it is being used by another process.", HResultFromError(code)),
            80  => new IOException($"The file \"{fpath}\" already exists.", HResultFromError(code)),
            87  => new IOException(MessageFromError(code), HResultFromError(code)),
            183 => new IOException($"Cannot create \"{fpath}\" because a file or directory with the same name already exists."),
            206 => new PathTooLongException(),
            995 => new OperationCanceledException(),
            _   => Marshal.GetExceptionForHR(HResultFromError(code))
        };
    }

    private static int HResultFromError(int code)
        => unchecked((int)0x80070000 | code);

    private unsafe static string MessageFromError(int code)
    {
        var lpBufferSize = 512;
        var lpBuffer = stackalloc char[lpBufferSize];
        lpBufferSize = NativeMethods.FormatMessage(NativeMethods.FormatMessageFlags.Defaults, IntPtr.Zero, code, 0, new IntPtr(lpBuffer), lpBufferSize, IntPtr.Zero);
        if (lpBufferSize != 0)
            return new string(lpBuffer, 0, lpBufferSize);

        return $"Unknown IO error.";
    }

    internal static FileStream SeekToEnd(this FileStream fs)
    {
        fs.Seek(0, SeekOrigin.End);
        return fs;
    }
    #endregion
}
