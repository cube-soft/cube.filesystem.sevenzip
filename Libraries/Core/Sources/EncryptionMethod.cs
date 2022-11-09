/* ------------------------------------------------------------------------- */
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
namespace Cube.FileSystem.SevenZip;

/* ------------------------------------------------------------------------- */
///
/// EncryptionMethod
///
/// <summary>
/// Specifies encryption methods.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum EncryptionMethod
{
    /// <summary>AES 128bit</summary>
    Aes128,
    /// <summary>AES 192bit</summary>
    Aes192,
    /// <summary>AES 256bit</summary>
    Aes256,
    /// <summary>Zip crypto algorithm</summary>
    ZipCrypto,
    /// <summary>Default settings</summary>
    Default,
}
