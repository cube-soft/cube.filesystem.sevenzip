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
using Cube.Mixin.Assembly;
using Cube.Mixin.String;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutValue
    ///
    /// <summary>
    /// Represents the settings for creating shortcut links on the desktop.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ShortcutValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutValue
        ///
        /// <summary>
        /// Initializes a new instance of the ShortcutValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutValue() { Reset(); }

        #endregion

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
        public PresetMenu Preset
        {
            get => _preset;
            set => SetProperty(ref _preset, value);
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
        public string Directory { get; set; } = string.Empty;

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
            if (b0) Preset |= PresetMenu.Compress;
            else Preset &= ~PresetMenu.Compress;

            var b1 = new Shortcut { FullName = GetFileName(Properties.Resources.ScExtract) }.Exists;
            if (b1) Preset |= PresetMenu.Extract;
            else Preset &= ~PresetMenu.Extract;

            var b2 = new Shortcut { FullName = GetFileName(Properties.Resources.ScSettings) }.Exists;
            if (b2) Preset |= PresetMenu.Settings;
            else Preset &= ~PresetMenu.Settings;
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
        /// OnDeserializing
        ///
        /// <summary>
        /// Occurs before deserializing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets the value.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _preset = PresetMenu.DefaultDesktop;
        }

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

            if ((Preset & PresetMenu.Compress) != 0) sc.Create();
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
                Arguments    = string.Join(" ", PresetMenu.Extract.ToArguments().Select(e => e.Quote())),
                IconLocation = $"{dest},2",
            };

            if ((Preset & PresetMenu.Extract) != 0) sc.Create();
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

            if ((Preset & PresetMenu.Settings) != 0) sc.Create();
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
            typeof(ShortcutValue).Assembly.GetDirectoryName(),
            filename
        );

        #endregion

        #region Fields
        private PresetMenu _preset;
        #endregion
    }
}
