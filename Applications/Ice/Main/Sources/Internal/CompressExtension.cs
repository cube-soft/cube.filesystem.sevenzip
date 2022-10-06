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

using System.Linq;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// CompressExtension
///
/// <summary>
/// Provides extended methods of the CompressFacade class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class CompressExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Select
    ///
    /// <summary>
    /// Gets the directory to save the compressed archive file.
    /// The method may query the user as needed.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    /// <param name="settings">Runtime compress settings.</param>
    ///
    /// <returns>Path to save.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string Select(this CompressFacade src, CompressQueryValue settings)
    {
        if (settings.Destination.HasValue()) return settings.Destination;

        var ss  = src.Settings.Value.Compress;
        var cvt = new ArchiveName(src.Request.Sources.First(), src.Request.Format);
        var obj = new SaveQueryProxy(src.Select, cvt.Value.FullName, src.Request, ss)
        {
            Format = cvt.Format,
        };

        if (obj.Location == SaveLocation.Query) return obj.Value;

        var dest = Io.Combine(obj.Value, cvt.Value.Name);
        return Io.Exists(dest) && ss.OverwritePrompt ? obj.Invoke() : dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetTitle
    ///
    /// <summary>
    /// Gets the title displayed in the main window.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /// <returns>Title displayed in the main window.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetTitle(this CompressFacade src) => src.GetTitle(src.Destination);

    /* --------------------------------------------------------------------- */
    ///
    /// GetText
    ///
    /// <summary>
    /// Gets the text displayed in the main window.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /// <returns>Text that represents the current status.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetText(this CompressFacade src) =>
        src.Destination.HasValue() ?
        string.Format(Properties.Resources.MessageArchive, src.Destination) :
        Properties.Resources.MessagePreArchive;

    #endregion
}
