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
using System.Runtime.Serialization;
using Cube.DataContract;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveSetting
    ///
    /// <summary>
    /// Represents the common settings of compressing or extracting
    /// archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public abstract class ArchiveSetting : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SaveLocation
        ///
        /// <summary>
        /// Gets or sets the value that represents the save location.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public SaveLocation SaveLocation
        {
            get => Get(() => SaveLocation.Preset);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectory
        ///
        /// <summary>
        /// Gets or sets the directory name to save.
        /// </summary>
        ///
        /// <remarks>
        /// The property is used when the SaveLocation property is set
        /// to Others.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "SaveDirectoryName")]
        public string SaveDirectory
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to filter some files
        /// and directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool Filtering
        {
            get => Get(() => true);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// Gets or sets the value that represents the method to open
        /// directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "OpenDirectory")]
        public OpenMethod OpenMethod
        {
            get => Get(() => OpenMethod.OpenNotDesktop);
            set => Set(value);
        }

        #endregion
    }
}
