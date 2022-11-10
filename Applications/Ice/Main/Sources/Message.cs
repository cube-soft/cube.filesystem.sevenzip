﻿/* ------------------------------------------------------------------------- */
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
using System.Text;
using System.Windows.Forms;
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
    /// Error
    ///
    /// <summary>
    /// Create a message to show a DialogBox with an error icon and YES/NO
    /// buttons.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static DialogMessage Error(Report src) => new(GetErrorText(src))
    {
        Title   = "CubeICE",
        Icon    = DialogIcon.Error,
        Buttons = DialogButtons.YesNo,
        CancelCandidates = new[] { DialogStatus.Cancel, DialogStatus.No },
    };

    /* --------------------------------------------------------------------- */
    ///
    /// ForCompressLocation
    ///
    /// <summary>
    /// Creates a new instance of the SaveFileDialog class with
    /// the specified source.
    /// </summary>
    ///
    /// <param name="src">Query source object.</param>
    ///
    /// <returns>SaveFileDialog object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static SaveFileMessage ForCompressLocation(SaveQuerySource src) =>
        ForCompressLocation(src.Source, src.Format);

    /* --------------------------------------------------------------------- */
    ///
    /// ForCompressLocation
    ///
    /// <summary>
    /// Creates a new instance of the SaveFileDialog class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path to save.</param>
    /// <param name="format">Format to save.</param>
    ///
    /// <returns>SaveFileDialog object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static SaveFileMessage ForCompressLocation(string src, Format format) =>
        new(src) { Filters = Resource.GetDialogFilters(format) };

    /* --------------------------------------------------------------------- */
    ///
    /// ForExtractLocation
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
    public static OpenDirectoryMessage ForExtractLocation(SaveQuerySource src)
    {
        var dest = new OpenDirectoryMessage(Properties.Resources.MessageExtractDestination) { NewButton = true };
        if (src.Source.HasValue()) dest.Value = Io.GetDirectoryName(src.Source);
        return dest;
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Error
    ///
    /// <summary>
    /// Create a message to show a DialogBox with an error icon and YES/NO
    /// buttons.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetErrorText(Report src)
    {
        var dest = new StringBuilder();
        var e    = src.Exception is null ? "UnexpectedError" :
                   src.Exception is SevenZipException se ? se.Code.ToString() :
                   src.Exception.GetType().Name;

        if (src.Target is not null) _ = dest.AppendLine(src.Target.RawName);

        return dest.AppendFormat(Properties.Resources.ErrorGeneric, e)
                   .AppendLine()
                   .AppendLine()
                   .Append(Properties.Resources.ErrorContinue)
                   .ToString();
    }

    #endregion
}
