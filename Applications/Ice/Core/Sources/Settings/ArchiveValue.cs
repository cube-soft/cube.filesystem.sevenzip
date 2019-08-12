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

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveValue
    ///
    /// <summary>
    /// Represents the common setting of compressing or extracting
    /// archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public abstract class ArchiveValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveValue
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveValue() { }

        #endregion

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
            get => _saveLocation;
            set => SetProperty(ref _saveLocation, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectoryName
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
        [DataMember]
        public string SaveDirectoryName
        {
            get => _saveDirectoryName;
            set => SetProperty(ref _saveDirectoryName, value);
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
            get => _filtering;
            set => SetProperty(ref _filtering, value);
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
        [DataMember]
        public OpenDirectoryMethod OpenDirectory
        {
            get => _openDirectory;
            set => SetProperty(ref _openDirectory, value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets the settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Reset()
        {
            _saveLocation      = SaveLocation.Others;
            _saveDirectoryName = string.Empty;
            _filtering         = true;
            _openDirectory     = OpenDirectoryMethod.OpenNotDesktop;
        }

        #endregion

        #region Fields
        private SaveLocation _saveLocation;
        private string _saveDirectoryName;
        private bool _filtering;
        private OpenDirectoryMethod _openDirectory;
        #endregion
    }
}
