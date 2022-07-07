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
    /// CompressSettingValue
    ///
    /// <summary>
    /// Represents the settings when compressing archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class CompressSettingValue : ArchiveSettingValue
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public CompressionLevel CompressionLevel
        {
            get => Get(() => CompressionLevel.Normal);
            set => Set(value);
        }

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
            get => Get(() => false);
            set => Set(value);
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
            get => Get(() => true);
            set => Set(value);
        }

        #endregion
    }
}
