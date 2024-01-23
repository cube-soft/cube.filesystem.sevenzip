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
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using System.Diagnostics;
using Cube.Backports;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// EntityExtension
///
/// <summary>
/// Provides extended methods of the Entity class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class EntityExtension
{
    #region Open

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Opens the specified source directory with the specified
    /// application.
    /// </summary>
    ///
    /// <param name="src">Path to open.</param>
    /// <param name="method">Method to open.</param>
    /// <param name="app">Path of the application.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Open(this Entity src, OpenMethod method, string app)
    {
        if (!method.HasFlag(OpenMethod.Open)) return;

        var dest = src.IsDirectory ? src.FullName : src.DirectoryName;
        var skip = method.HasFlag(OpenMethod.SkipDesktop) &&
                   dest.FuzzyEquals(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        if (skip) return;

        var cvt = app.HasValue() ? app : "explorer.exe";
        Logger.Debug($"Method:{nameof(Open)}, Source:{src.FullName.Quote()}, App:{cvt.Quote()}");
        Logger.Try(() => new Process
        {
            StartInfo = new()
            {
                FileName        = cvt,
                Arguments       = dest.Quote(),
                CreateNoWindow  = false,
                UseShellExecute = true,
                LoadUserProfile = false,
                WindowStyle     = ProcessWindowStyle.Normal,
            }
        }.Start());
    }

    #endregion

    #region Move

    /* --------------------------------------------------------------------- */
    ///
    /// Move
    ///
    /// <summary>
    /// Moves the specified file or directory.
    /// </summary>
    ///
    /// <param name="src">Source file or directory information.</param>
    /// <param name="dest">Destination path to move.</param>
    /// <param name="query">Query to confirm to overwrite.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void Move(this Entity src, string dest, OverwriteQuery query)
    {
        if (src.IsDirectory)
        {
            if (!Io.Exists(dest))
            {
                Io.CreateDirectory(dest);
                Logger.Try(() => Io.SetCreationTime(dest, src.CreationTime));
                Logger.Try(() => Io.SetLastWriteTime(dest, src.LastWriteTime));
                Logger.Try(() => Io.SetLastAccessTime(dest, src.LastAccessTime));
                Logger.Try(() => Io.SetAttributes(dest, src.Attributes));
            }
        }
        else if (Io.Exists(dest)) Move(src, dest, query.Get(src, new(dest)));
        else Io.Move(src.FullName, dest, true);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Move
    ///
    /// <summary>
    /// Moves the specified file or directory according to the
    /// specified method.
    /// </summary>
    ///
    /// <param name="src">Source file or directory information.</param>
    /// <param name="dest">Destination path to move.</param>
    /// <param name="method">Overwrite method.</param>
    ///
    /* --------------------------------------------------------------------- */
    private static void Move(this Entity src, string dest, OverwriteMethod method)
    {
        switch (method & OverwriteMethod.Operations)
        {
            case OverwriteMethod.Yes:
                // Because Move is a little tricky...
                Io.Copy(src.FullName, dest, true);
                break;
            case OverwriteMethod.Rename:
                Io.Move(src.FullName, IoEx.GetUniqueName(dest), false);
                break;
            case OverwriteMethod.No:
            case OverwriteMethod.Cancel:
                break;
        }
    }

    #endregion
}
