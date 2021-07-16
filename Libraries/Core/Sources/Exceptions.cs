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
using System;
using System.IO;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// UnknownFormatException
    ///
    /// <summary>
    /// Represents that the specified file is not supported by the SevenZip
    /// module.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class UnknownFormatException : NotSupportedException { }

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionException
    ///
    /// <summary>
    /// Represents the encryption related exception like the password
    /// unmatched error.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Serializable]
    public class EncryptionException : IOException { }
}
