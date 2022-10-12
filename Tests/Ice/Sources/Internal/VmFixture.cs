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
namespace Cube.FileSystem.SevenZip.Ice.Tests;

using System.Collections.Generic;
using Cube.Tests;

/* ------------------------------------------------------------------------- */
///
/// VmFixture
///
/// <summary>
/// Provides functionality to help the tests for ArchiveViewModel
/// and its inherited classes.
/// </summary>
///
/* ------------------------------------------------------------------------- */
abstract class VmFixture : FileFixture
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// NewVM
    ///
    /// <summary>
    /// Creates a new instance of the CompressViewModel class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="args">Program arguments.</param>
    /// <param name="settings">User settings for compression.</param>
    ///
    /// <returns>CompressViewModel object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected CompressViewModel NewVM(IEnumerable<string> args, CompressionSettingValue settings)
    {
        var ss = NewSettings();

        ss.Value.Compression = settings;
        ss.Value.Compression.OpenMethod = OpenMethod.None;
        ss.Value.Compression.SaveDirectory = Get("Preset");

        return new CompressViewModel(new(args), ss, new());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// NewVM
    ///
    /// <summary>
    /// Creates a new instance of the ExtractViewModel class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="args">Program arguments.</param>
    /// <param name="settings">User settings for compression.</param>
    ///
    /// <returns>ExtractViewModel object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected ExtractViewModel NewVM(IEnumerable<string> args, ExtractionSettingValue settings)
    {
        var ss = NewSettings();

        ss.Value.Extraction = settings;
        ss.Value.Extraction.OpenMethod = OpenMethod.None;

        return new ExtractViewModel(new(args), ss, new());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// NewVM
    ///
    /// <summary>
    /// Creates a new instance of the SettingViewModel class.
    /// </summary>
    ///
    /// <returns>SettingViewModel object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected Settings.SettingViewModel NewVM() => NewVM(NewSettings());

    /* --------------------------------------------------------------------- */
    ///
    /// NewVM
    ///
    /// <summary>
    /// Creates a new instance of the SettingViewModel class with the
    /// specified settings.
    /// </summary>
    ///
    /// <param name="src">User settings.</param>
    ///
    /// <returns>SettingViewModel object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected Settings.SettingViewModel NewVM(SettingFolder src) => new(src, new());

    /* --------------------------------------------------------------------- */
    ///
    /// NewSettings
    ///
    /// <summary>
    /// Creates a new instance of the SettingFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected SettingFolder NewSettings()
    {
        var dest = new SettingFolder(DataContract.Format.Json, Get("Settings.json"));

        dest.Value.Filters  = "Filter.txt|FilterDirectory|__MACOSX";
        dest.Value.Explorer = "dummy.exe";

        return dest;
    }

    #endregion
}
