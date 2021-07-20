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
    /// CompressionMethod
    ///
    /// <summary>
    /// Specifies compression methods.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum CompressionMethod
    {
        /// <summary>Copy</summary>
        Copy,
        /// <summary>Deflate</summary>
        Deflate,
        /// <summary>Deflate (64bit)</summary>
        Deflate64,
        /// <summary>GZip</summary>
        GZip,
        /// <summary>BZip2</summary>
        BZip2,
        /// <summary>XZ</summary>
        XZ,
        /// <summary>LZMA</summary>
        Lzma,
        /// <summary>LZMA2</summary>
        Lzma2,
        /// <summary>PPMD</summary>
        Ppmd,
        /// <summary>Default settings</summary>
        Default,
    }
}
