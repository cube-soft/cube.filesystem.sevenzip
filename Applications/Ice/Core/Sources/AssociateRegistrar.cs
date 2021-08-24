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
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.Logging;
using Cube.Mixin.Collections;
using Cube.Mixin.String;
using Microsoft.Win32;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateRegistrar
    ///
    /// <summary>
    /// Provides functionality to update the registry for file associations.
    /// </summary>
    ///
    /// <remarks>
    /// The class edits KEY_CLASSES_ROOT in the registry.
    /// Therefore, it requires the administrator privilege to invoke.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class AssociateRegistrar
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateRegistrar
        ///
        /// <summary>
        /// Initializes a new instance of the AssociateRegistrar class
        /// with the specified path.
        /// </summary>
        ///
        /// <param name="file">Path to invoke when double clicked.</param>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateRegistrar(string file) => FileName = file;

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        ///
        /// <summary>
        /// Gets or sets the path that will be executed when double clicked.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FileName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// Gets or sets the list of file arguments to be executed when
        /// double clicked.
        /// </summary>
        ///
        /// <remarks>
        /// The actual arguments are the ones set in the property plus "%1".
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Arguments { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// IconLocation
        ///
        /// <summary>
        /// Gets or sets the path of the icon to be displayed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string IconLocation { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTip
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to customize the
        /// tooltip display on mouse over.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ToolTip { get; set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// Command
        ///
        /// <summary>
        /// Get the command line to be registered in the registry.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Command => $"{FileName.Quote()} {Arguments.Concat("%1").Select(e => e.Quote()).Join(" ")}";

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates file associations.
        /// </summary>
        ///
        /// <param name="extensions">
        /// Object that defines file associations.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(IDictionary<string, bool> extensions)
        {
            foreach (var kv in extensions) Update(kv.Key, kv.Value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Updates file associations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update(string extension, bool enabled)
        {
            if (!extension.HasValue()) return;
            if (enabled) Register(extension);
            else Delete(extension);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateToolTip
        ///
        /// <summary>
        /// Updates the settings related to the tooltip that is displayed
        /// on mouse over.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateToolTip(RegistryKey src, bool enabled)
        {
            var guid = TooTipKey.ToString("B").ToUpperInvariant();
            var name = $@"shellex\{guid}";

            if (enabled)
            {
                var s = ToolTipHandler.ToString("B");
                using var sk = src.CreateSubKey(name);
                sk.SetValue("", s);
            }
            else src.DeleteSubKeyTree(name, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Creates and registers a registry entry for file associations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Register(string extension)
        {
            if (!FileName.HasValue()) return;

            var id   = extension.TrimStart('.');
            var root = Registry.ClassesRoot;
            var name = GetSubKeyName(id);
            using (var key = root.CreateSubKey(name)) Register(key, id);

            Register(extension, name);
            DeleteUserChoise(extension);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Creates and registers a registry entry for file associations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Register(RegistryKey src, string id)
        {
            src.SetValue("", $"{id} {Properties.Resources.FileSuffix}".ToUpperInvariant());

            using (var sk = src.CreateSubKey("shell"))
            {
                sk.SetValue("", "open");
                using var cmd = sk.CreateSubKey(@"open\command");
                cmd.SetValue("", Command);
            }

            using (var sk = src.CreateSubKey("DefaultIcon")) sk.SetValue("", IconLocation);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates and registers settings to associate CubeICE with the
        /// registry entry representing the extension.
        /// </summary>
        ///
        /// <remarks>
        /// ToolTip の処理が未実装なため、現在は ToolTip に関連する項目は
        /// 強制的に削除しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Register(string extension, string name)
        {
            using var sk = Registry.ClassesRoot.CreateSubKey(GetExtension(extension));

            var prev = sk.GetValue("") as string;
            if (prev.HasValue() && prev != name) sk.SetValue(PreArchiver, prev);

            sk.SetValue("", name);
            UpdateToolTip(sk, ToolTip);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// Removes the registry entry for file associations.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Delete(string extension)
        {
            using (var sk = Registry.ClassesRoot.CreateSubKey(GetExtension(extension)))
            {
                var prev = sk.GetValue(PreArchiver, "") as string;
                if (prev.HasValue())
                {
                    sk.SetValue("", prev);
                    sk.DeleteValue(PreArchiver, false);
                }
                else sk.DeleteValue("", false);

                UpdateToolTip(sk, false);
            }
            Registry.ClassesRoot.DeleteSubKeyTree(GetSubKeyName(extension), false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteUserChoise
        ///
        /// <summary>
        /// Deletes the UserChoise subkey of the specified extension.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DeleteUserChoise(string extension) => GetType().LogWarn(() =>
        {
            var src  = "UserChoice";
            var root = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts";
            var name = $@"{root}\{GetExtension(extension)}";

            using var sk = Registry.CurrentUser.OpenSubKey(name, true);
            if (sk != null && sk.GetSubKeyNames().Any(e => e.FuzzyEquals(src)))
            {
                sk.DeleteSubKey(src, false);
                GetType().LogDebug($"Reset:{name.Quote()}");
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetSubKeyName
        ///
        /// <summary>
        /// Gets the subkey name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetSubKeyName(string id) =>
            $"{System.IO.Path.GetFileNameWithoutExtension(FileName)}_{id}".ToLowerInvariant();

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        ///
        /// <summary>
        /// Gets the normalized extension.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetExtension(string src) =>
            ((src[0] == '.') ? src : $".{src}").ToLowerInvariant();

        #endregion

        #region Fields
        private static readonly string PreArchiver = "PreArchiver";
        private static readonly Guid TooTipKey = new("{00021500-0000-0000-c000-000000000046}");
        private static readonly Guid ToolTipHandler = new("{cb8641a3-ebc7-4758-a302-aa6667b817c8}");
        #endregion
    }
}
