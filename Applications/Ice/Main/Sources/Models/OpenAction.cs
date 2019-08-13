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
using Cube.Mixin.Environment;
using Cube.Mixin.Logging;
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
    internal sealed class OpenAction
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// OpenAction
        ///
        /// <summary>
        /// Initializes a new instance of the OpenAction class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public OpenAction(SettingFolder settings) { Settings = settings; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        #endregion

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
        /// <param name="method">
        /// Method to open the specified path.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invok(string src, OpenDirectoryMethod method)
        {
            if (!method.HasFlag(OpenDirectoryMethod.Open)) return;
            var fi = Settings.IO.Get(src);
            var dest = fi.IsDirectory ? fi.FullName : fi.DirectoryName;
            if (IsSkip(dest, method)) return;

            var exec = Settings.Value.Explorer.HasValue() ?
                       Settings.Value.Explorer :
                       "explorer.exe";

            this.LogDebug($"Open:{src.Quote()}", $"Explorer:{exec.Quote()}");
            Start(exec, src.Quote());
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
        private bool IsSkip(string src, OpenDirectoryMethod method) =>
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
        private void Start(string exec, string args) => new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exec,
                Arguments = args,
                CreateNoWindow = false,
                UseShellExecute = true,
                LoadUserProfile = false,
                WindowStyle = ProcessWindowStyle.Normal,
            }
        }.Start();

        #endregion
    }
}
