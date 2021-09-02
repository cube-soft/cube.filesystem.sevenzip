﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
