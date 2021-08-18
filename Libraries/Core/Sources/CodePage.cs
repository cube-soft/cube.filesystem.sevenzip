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
namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// CodePage
    ///
    /// <summary>
    /// Specifies code pages in Windows.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
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
}
