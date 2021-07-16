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
using Cube.Mixin.Environment;
using Cube.Mixin.String;
using System;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// OpenAction
    ///
    /// <summary>
    /// Provides functionality to open the directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class OpenAction
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the action.
        /// </summary>
        ///
        /// <param name="src">Path to open.</param>
        /// <param name="method">Method to open.</param>
        /// <param name="exec">Path of the application.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(Entity src, OpenDirectoryMethod method, string exec)
        {
            if (!method.HasFlag(OpenDirectoryMethod.Open)) return;
            var dest = src.IsDirectory ? src.FullName : src.DirectoryName;
            if (IsSkip(dest, method)) return;

            var cvt = exec.HasValue() ? exec : "explorer.exe";
            Logger.Debug(typeof(OpenAction), $"Path:{src.FullName.Quote()}", $"Explorer:{cvt.Quote()}");
            Start(cvt, src.FullName.Quote());
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// IsSkip
        ///
        /// <summary>
        /// Determines whether to skip the action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool IsSkip(string src, OpenDirectoryMethod method) =>
            method.HasFlag(OpenDirectoryMethod.SkipDesktop) ?
            src.FuzzyEquals(Environment.SpecialFolder.Desktop.GetName()) :
            false;

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ProcessStartInfo class with the
        /// specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Start(string exec, string args) => new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName        = exec,
                Arguments       = args,
                CreateNoWindow  = false,
                UseShellExecute = true,
                LoadUserProfile = false,
                WindowStyle     = ProcessWindowStyle.Normal,
            }
        }.Start();

        #endregion
    }
}
