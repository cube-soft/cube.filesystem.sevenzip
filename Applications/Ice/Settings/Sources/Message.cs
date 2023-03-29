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

using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Message
///
/// <summary>
/// Provides functionality to create a message object.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class Message
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// ForSaveDirectory
    ///
    /// <summary>
    /// Creates a new instance of the OpenDirectoryMessage class with
    /// the specified source.
    /// </summary>
    ///
    /// <param name="src">Query source object.</param>
    ///
    /// <returns>OpenDirectoryMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static OpenDirectoryMessage ForSaveDirectory(string src)
    {
        var dest = new OpenDirectoryMessage(Properties.Resources.MessageSave) { NewButton = true };
        if (src.HasValue()) dest.Value = src;
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ForAssociateHelp
    ///
    /// <summary>
    /// Creates a message to display a help page on file associations.
    /// </summary>
    ///
    /// <returns>ProcessMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static ProcessMessage ForAssociateHelp() =>
        new("https://clown.cube-soft.jp/entry/cubeice/issues/file-association");

    #endregion
}
