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
namespace Cube.FileSystem.SevenZip.Ice.Settings;

using System.Runtime.Serialization;

/* ------------------------------------------------------------------------- */
///
/// ExtractSettingValue
///
/// <summary>
/// Represents the settings when extracting archives.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
public sealed class ExtractSettingValue : ArchiveSettingValue
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// RootDirectory
    ///
    /// <summary>
    /// Gets or sets the method to determine the root directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "RootDirectory")]
    public SaveMethod SaveMethod
    {
        get => Get(() => SaveMethod.CreateSmart);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DeleteSource
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to delete the source
    /// archive after extracting.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool DeleteSource
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Bursty
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to extract archives
    /// burstly.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool Bursty
    {
        get => Get(() => true);
        set => Set(value);
    }

    #endregion
}
