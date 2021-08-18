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
using System.Reflection;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.Assembly;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingFolder
    ///
    /// <summary>
    /// Represents the application and/or user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingFolder : SettingFolder<SettingValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFolder class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder() : this(
            typeof(SettingFolder).Assembly,
            DataContract.Format.Registry,
            @"CubeSoft\CubeICE\v3"
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFolder
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFolder with the
        /// specified parameters.
        /// </summary>
        ///
        /// <param name="assembly">Assembly information.</param>
        /// <param name="format">Serialized format.</param>
        /// <param name="location">Location for the settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder(Assembly assembly, DataContract.Format format, string location) :
            base(format, location, assembly.GetSoftwareVersion())
        {
            AutoSave       = false;
            Version.Suffix = Properties.Resources.VersionSuffix;
            Startup        = new("cubeice-checker")
            {
                Source = Io.Combine(assembly.GetDirectoryName(), "CubeChecker.exe"),
            };

            Startup.Arguments.Add("cubeice");
            Startup.Arguments.Add("/subkey");
            Startup.Arguments.Add("CubeICE");
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Startup
        ///
        /// <summary>
        /// Get the startup registration object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Startup Startup { get; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnSave
        ///
        /// <summary>
        /// Saves the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSave()
        {
            base.OnSave();
            Startup.Save(true);
        }

        #endregion
    }
}
