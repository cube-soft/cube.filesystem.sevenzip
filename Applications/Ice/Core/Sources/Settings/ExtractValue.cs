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
    /// ExtractValue
    ///
    /// <summary>
    /// Represents the settings when extracting archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ExtractValue : ArchiveValue
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractValue
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractValue() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RootDirectory
        ///
        /// <summary>
        /// Gets or sets the method to determine the root directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public CreateDirectoryMethod RootDirectory
        {
            get => _rootDirectory;
            set => SetProperty(ref _rootDirectory, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to delete the source
        /// archive after extracting.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool DeleteSource
        {
            get => _deleteSource;
            set => SetProperty(ref _deleteSource, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bursty
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to extract archives
        /// burstly.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool Bursty
        {
            get => _bursty;
            set => SetProperty(ref _bursty, value);
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
        protected override void Reset()
        {
            _deleteSource  = false;
            _bursty        = true;
            _rootDirectory = CreateDirectoryMethod.CreateSmart;

            base.Reset();
        }

        #endregion

        #region Fields
        private bool _deleteSource;
        private bool _bursty;
        private CreateDirectoryMethod _rootDirectory;
        #endregion
    }
}
