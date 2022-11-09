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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ArchiveFacade
///
/// <summary>
/// Represents the base class for compressing or extracting archives.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public abstract class ArchiveFacade : ProgressFacade
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveFacade
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveFacade class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Request of the process.</param>
    /// <param name="settings">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected ArchiveFacade(Request src, SettingFolder settings)
    {
        Request  = src;
        Settings = settings;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Request
    ///
    /// <summary>
    /// Gets the request for the process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Request Request { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Settings
    ///
    /// <summary>
    /// Gets the user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SettingFolder Settings { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Destination
    ///
    /// <summary>
    /// Gets or sets the path of file or directory to save.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Destination
    {
        get => Get(() => string.Empty);
        protected set
        {
            if (value.FuzzyEquals(Destination)) return;
            Logger.Debug($"{nameof(Destination)}:{value}");
            _ = Set(value);
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Temp
    ///
    /// <summary>
    /// Gets or sets the path of the working directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Temp
    {
        get => Get(() => string.Empty);
        private set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Select
    ///
    /// <summary>
    /// Gets or sets the query object to select the destination.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Query<SaveQuerySource, string> Select { get; set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Gets or sets the query object to get the password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Query<string> Password { get; set; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// SetTemp
    ///
    /// <summary>
    /// Sets the value to the Temp property with the specified
    /// directory.
    /// </summary>
    ///
    /// <param name="alternate">
    /// Path of the root directory to be used if the Temp setting does
    /// not exist.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected void SetTemp(string alternate)
    {
        if (!Temp.HasValue())
        {
            var src  = Settings.Value.Temp;
            var root = src.HasValue() && Io.Exists(src) ? src : alternate;
            Temp = Io.Combine(root, Guid.NewGuid().ToString("N"));
        }

        Logger.Debug($"{nameof(Temp)}:{Temp}");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the object and
    /// optionally releases the managed resources.
    /// </summary>
    ///
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources;
    /// false to release only unmanaged resources.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void Dispose(bool disposing)
    {
        Logger.Warn(() => Io.Delete(Temp));
        base.Dispose(disposing);
    }

    #endregion
}
