/* ------------------------------------------------------------------------- */
//
// This file is part of Managed NTFS Data Streams project
//
// Copyright 2020 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
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
/// FileDataStreamException
///
/// <summary>
/// Represents an exception that occurred during NTFS data stream APIs
/// execution.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public class FileDataStreamException : Exception
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileDataStreamException
    ///
    /// <summary>
    /// Initializes a new instance of the FileDataStreamException class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="code">Error code.</param>
    /// <param name="name">File path or NTFS data stream name.</param>
    ///
    /* --------------------------------------------------------------------- */
    public FileDataStreamException(int code, string name) : base(GetMessage(code))
    {
        Code = code;
        Name = name ?? "<null>";
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FileDataStreamException
    ///
    /// <summary>
    /// Initializes a new instance of the FileDataStreamException class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="message">Error message.</param>
    /// <param name="name">File path or NTFS data stream name.</param>
    ///
    /* --------------------------------------------------------------------- */
    public FileDataStreamException(string message, string name) : base(message)
    {
        Code = 0;
        Name = name;
    }

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Code
    ///
    /// <summary>
    /// Gets the provided error code.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Code { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Name
    ///
    /// <summary>
    /// Gets the provided file path or stream name.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Name { get; }

    #endregion

    #region Others

    /* --------------------------------------------------------------------- */
    ///
    /// GetMessage
    ///
    /// <summary>
    /// Creates a new message with the specified error code.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetMessage(int code) => code switch
    {
          2 => "Specified NTFS data stream was not found.",
          3 => "Could not find a part of the path.",
          5 => "Access to the NTFS data stream was denied.",
         15 => "Could not find the drive of the path.",
         32 => "The process cannot access the NTFS data stream because it is being used by another process.",
         80 => "The NTFS data stream already exists.",
        183 => "File or directory with the same name already exists.",
        206 => "The specified path is too long.",
        995 => "Operation was canceled by user.",
          _ => $"Unknown error ({code})",
    };

    #endregion
}
