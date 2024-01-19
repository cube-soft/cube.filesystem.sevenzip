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

using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ZoneId
///
/// <summary>
/// Provides functionality to get or set the ZoneID.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class ZoneId
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// FileName
    ///
    /// <summary>
    /// Gets the ADS file name for the ZoneID.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static string FileName => "Zone.Identifier";

    /* --------------------------------------------------------------------- */
    ///
    /// SectionName
    ///
    /// <summary>
    /// Gets the section name for the ZoneID.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static string SectionName => "ZoneTransfer";

    /* --------------------------------------------------------------------- */
    ///
    /// KeyName
    ///
    /// <summary>
    /// Gets the key name of the ZoneID.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static string KeyName => "ZoneId";

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Sets the Zone ID of the specified file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static SecurityZone Get(string src)
    {
        var ads = GetStream(src);
        if (ads is null) return SecurityZone.NoZone;

        using var reader = new StreamReader(ads.OpenRead(), Encoding.Default);
        if (!MoveSection(reader)) return SecurityZone.NoZone;

        var key = $"{KeyName}=";

        while (reader.ReadLine() is { } str)
        {
            if (str.FuzzyStartsWith(key) &&
                int.TryParse(str.Trim().Substring(key.Length), out var n)
            ) return n switch
            {
                 0 => SecurityZone.MyComputer,
                 1 => SecurityZone.Intranet,
                 2 => SecurityZone.Trusted,
                 3 => SecurityZone.Internet,
                 4 => SecurityZone.Untrusted,
                 _ => SecurityZone.NoZone,
            };
        }

        return SecurityZone.NoZone;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Set
    ///
    /// <summary>
    /// Sets the Zone ID to the specified file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static void Set(string src, SecurityZone id)
    {
        var attr = new Entity(src).Attributes;

        try
        {
            Io.SetAttributes(src, FileAttributes.Normal);
            var ads = new FileDataStream(src, FileName, 0, FileDataStreamType.Data);
            using var writer = new StreamWriter(ads.Create(), Encoding.Default);
            writer.WriteLine($"[{SectionName}]");
            writer.WriteLine($"{KeyName}={id:D}");
        }
        finally { Io.SetAttributes(src, attr); }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetStream
    ///
    /// <summary>
    /// Gets the Zone ADS stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static FileDataStream GetStream(string src) => FileDataStreamHelper
        .GetStreams(src)
        .FirstOrDefault(e => e.Type == FileDataStreamType.Data && e.Name == FileName);

    /* --------------------------------------------------------------------- */
    ///
    /// MoveSection
    ///
    /// <summary>
    /// Moves the ZoneTransfer section.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static bool MoveSection(StreamReader src)
    {
        while (src.ReadLine() is { } str)
        {
            if (str.Trim().FuzzyEquals($"[{SectionName}]")) return true;
        }
        return false;
    }

    #endregion
}
