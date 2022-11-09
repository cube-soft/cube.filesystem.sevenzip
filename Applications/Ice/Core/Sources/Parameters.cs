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
namespace Cube.FileSystem.SevenZip.Ice;

using System;

/* ------------------------------------------------------------------------- */
///
/// Mode
///
/// <summary>
/// Specifies the operation mode.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum Mode
{
    /// <summary>None</summary>
    None,
    /// <summary>Compress</summary>
    Compress,
    /// <summary>Extract</summary>
    Extract,
}

/* ------------------------------------------------------------------------- */
///
/// SaveLocation
///
/// <summary>
/// Specifies the kind of save path.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum SaveLocation
{
    /// <summary>Use the preset settings.</summary>
    Preset = 0,
    /// <summary>Same as the source file.</summary>
    Source = 1,
    /// <summary>Ask the user to select.</summary>
    Query = 2,
    /// <summary>Desktop folder.</summary>
    Desktop = 3,
    /// <summary>My documents folder.</summary>
    MyDocuments = 4,
    /// <summary>Explicitly specified in the command line.</summary>
    Explicit = 10,
    /// <summary>Unknown</summary>
    Unknown = -1,
}

/* ------------------------------------------------------------------------- */
///
/// SaveMethod
///
/// <summary>
/// Specifies the method to create the save directory.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum SaveMethod
{
    /// <summary>Not create.</summary>
    None = 0x00,
    /// <summary>Skip if all items are contained in a single directory.</summary>
    SkipSingleDirectory = 0x02,
    /// <summary>Skip if the archive has a file.</summary>
    SkipSingleFile = 0x04,
    /// <summary>Skip options.</summary>
    SkipOptions = SkipSingleDirectory | SkipSingleFile,
    /// <summary>Create directory.</summary>
    Create = 0x01,
    /// <summary>Create directory if needed.</summary>
    CreateSmart = Create | SkipSingleDirectory,
}

/* ------------------------------------------------------------------------- */
///
/// OverwriteMethod
///
/// <summary>
/// Specifies the method to overwrite a file or directory.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum OverwriteMethod
{
    /// <summary>Ask the user.</summary>
    Query = 0x000,
    /// <summary>Cancel.</summary>
    Cancel = 0x002,
    /// <summary>Yes.</summary>
    Yes = 0x006,
    /// <summary>No.</summary>
    No = 0x007,
    /// <summary>Rename instead of overwriting.</summary>
    Rename = 0x010,
    /// <summary>Mask for operations.</summary>
    Operations = 0x01f,

    /// <summary>Same as the previous operation.</summary>
    Always = 0x100,
    /// <summary>Always yes.</summary>
    AlwaysYes = Always | Yes,
    /// <summary>Always no.</summary>
    AlwaysNo = Always | No,
    /// <summary>Always rename.</summary>
    AlwaysRename = Always | Rename,
}

/* ------------------------------------------------------------------------- */
///
/// OpenMethod
///
/// <summary>
/// Specifies the method to open the directory.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Flags]
public enum OpenMethod
{
    /// <summary>Not open.</summary>
    None = 0x0000,
    /// <summary>Skip if the specified path represents the Desktop.</summary>
    SkipDesktop = 0x0002,
    /// <summary>Open directory.</summary>
    Open = 0x0001,
    /// <summary>Open directory if needed.</summary>
    OpenNotDesktop = Open | SkipDesktop,
}
