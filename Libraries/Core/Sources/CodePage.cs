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
/// CodePage
///
/// <summary>
/// Specifies code pages in Windows.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum CodePage
{
    /// <summary>ASCII</summary>
    Ascii = 0,
    /// <summary>Code page corresponding to the locale settings</summary>
    Oem = 1,
    /// <summary>Symbol</summary>
    Symbol = 42,
    /// <summary>Japanese (Shift_JIS compatible)</summary>
    Japanese = 932,
    /// <summary>UTF-8</summary>
    Utf8 = 65001,
}
