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
    /// CompressValue
    ///
    /// <summary>
    /// Represents the settings when compressing archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class CompressValue : ArchiveValue
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressValue
        ///
        /// <summary>
        /// Initializes a new instance of the CompressValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressValue() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// UseUtf8
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to convert to the UTF-8
        /// encoding when compressing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "UseUTF8")]
        public bool UseUtf8
        {
            get => _useUtf8;
            set => SetProperty(ref _useUtf8, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OverwritePrompt
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to show the overwrite
        /// prompt.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool OverwritePrompt
        {
            get => _overwritePrompt;
            set => SetProperty(ref _overwritePrompt, value);
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
            _useUtf8         = false;
            _overwritePrompt = true;

            base.Reset();
        }

        #endregion

        #region Fields
        private bool _useUtf8;
        private bool _overwritePrompt;
        #endregion
    }
}
