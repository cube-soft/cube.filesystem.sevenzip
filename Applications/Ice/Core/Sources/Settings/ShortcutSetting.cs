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
using System.Linq;
using System.Runtime.Serialization;
using Cube.Mixin.Assembly;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutSetting
    ///
    /// <summary>
    /// Represents the settings for creating shortcut links on the desktop.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ShortcutSetting : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Preset
        ///
        /// <summary>
        /// Gets or sets the preset menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Preset Preset
        {
            get => Get(() => Preset.DefaultDesktop);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Directory
        ///
        /// <summary>
        /// Gets or sets the path to create shortcut links.
        /// </summary>
        ///
        /// <remarks>
        /// 未設定の場合はデスクトップに作成されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Directory
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Sync
        ///
        /// <summary>
        /// Applies the current shortcut link existence to properties of
        /// the object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Sync()
        {
            var b0 = new Shortcut { FullName = GetFileName(Properties.Resources.ScArcive) }.Exists;
            if (b0) Preset |= Preset.Compress;
            else Preset &= ~Preset.Compress;

            var b1 = new Shortcut { FullName = GetFileName(Properties.Resources.ScExtract) }.Exists;
            if (b1) Preset |= Preset.Extract;
            else Preset &= ~Preset.Extract;

            var b2 = new Shortcut { FullName = GetFileName(Properties.Resources.ScSettings) }.Exists;
            if (b2) Preset |= Preset.Settings;
            else Preset &= ~Preset.Settings;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// Creates or removes shortcut links.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Update()
        {
            UpdateArchive();
            UpdateExtract();
            UpdateSettings();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateArchive
        ///
        /// <summary>
        /// Creates or removes the shortcut link for compressing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateArchive()
        {
            var src  = GetFileName(Properties.Resources.ScArcive);
            var dest = GetLink("cubeice.exe");
            var sc   = new Shortcut
            {
                FullName     = src,
                Target       = dest,
                Arguments    = string.Join(" ", Preset.ToArguments().Select(e => e.Quote())),
                IconLocation = $"{dest},1",
            };

            if ((Preset & Preset.Compress) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateExtract
        ///
        /// <summary>
        /// Creates or removes the shortcut link for extracting.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateExtract()
        {
            var src  = GetFileName(Properties.Resources.ScExtract);
            var dest = GetLink("cubeice.exe");
            var sc   = new Shortcut
            {
                FullName     = src,
                Target       = dest,
                Arguments    = string.Join(" ", Preset.Extract.ToArguments().Select(e => e.Quote())),
                IconLocation = $"{dest},2",
            };

            if ((Preset & Preset.Extract) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateSettings
        ///
        /// <summary>
        /// Creates or removes the shortcut link for settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void UpdateSettings()
        {
            var src  = GetFileName(Properties.Resources.ScSettings);
            var dest = GetLink("cubeice-setting.exe");
            var sc   = new Shortcut
            {
                FullName     = src,
                Target       = dest,
                IconLocation = dest,
            };

            if ((Preset & Preset.Settings) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// Gets the path of the shortcut link.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFileName(string name)
        {
            var dir = Directory.HasValue() ?
                      Directory :
                      Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return System.IO.Path.Combine(dir, name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetLink
        ///
        /// <summary>
        /// Gets the target path of the specified shortcut link.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetLink(string filename) => System.IO.Path.Combine(
            typeof(ShortcutSetting).Assembly.GetDirectoryName(),
            filename
        );

        #endregion
    }
}
