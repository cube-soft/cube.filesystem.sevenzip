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
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// Resource
    ///
    /// <summary>
    /// Provides resources for display.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Resource
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcuts
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a display
        /// string and a Preset pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, Preset>> Shortcuts { get; } = new()
        {
            new(Properties.Resources.MenuZip,         Preset.CompressZip),
            new(Properties.Resources.MenuZipPassword, Preset.CompressZipPassword),
            new(Properties.Resources.MenuSevenZip,    Preset.Compress7z),
            new(Properties.Resources.MenuBZip2,       Preset.CompressBz2),
            new(Properties.Resources.MenuGZip,        Preset.CompressGz),
            new(Properties.Resources.MenuSfx,         Preset.CompressSfx),
            new(Properties.Resources.MenuDetails,     Preset.CompressDetails),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// IoHandlers
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a display
        /// string and a bool pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, bool>> IoHandlers { get; } = new()
        {
            new(Properties.Resources.MenuNormal,   false),
            new(Properties.Resources.MenuExtended,  true),
        };

        #endregion
    }
}
