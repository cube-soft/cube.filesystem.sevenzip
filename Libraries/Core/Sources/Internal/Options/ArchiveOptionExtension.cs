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
    /// ArchiveOptionSetterExtension
    ///
    /// <summary>
    /// Provides extended methods for the ArchiveOptionSetter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ArchiveOptionExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts from the specified object to the new instance of
        /// the ArchiveOptionSetter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static ArchiveOptionSetter Convert(this ArchiveOption src, Format format) =>
            src != null ? format switch
        {
            Format.Zip      => new ZipOptionSetter(src),
            Format.SevenZip => new SevenZipOptionSetter(src),
            Format.Sfx      => new SevenZipOptionSetter(src),
            Format.Tar      => null,
            _               => new ArchiveOptionSetter(src),
        } : default;

        #endregion
    }
}
