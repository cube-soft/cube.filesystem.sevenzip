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
namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressExtension
    ///
    /// <summary>
    /// Provides extended methods of the CompressRuntime class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class CompressExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToOption
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Runtime settings.</param>
        /// <param name="common">User settings.</param>
        ///
        /// <remarks>ArchiveOption object.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static ArchiveOption ToOption(this CompressRuntimeSetting src, SettingFolder common) =>
            ToOption(src, common.Value.Compress);

        /* ----------------------------------------------------------------- */
        ///
        /// ToOption
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Runtime settings.</param>
        /// <param name="common">User settings for compression.</param>
        ///
        /// <remarks>ArchiveOption object.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static ArchiveOption ToOption(this CompressRuntimeSetting src, CompressSetting common)
        {
            switch (src.Format)
            {
                case Format.Zip:      return MakeZip(src, common);
                case Format.SevenZip: return MakeSevenZip(src);
                case Format.Sfx:      return MakeSfx(src);
                case Format.Tar:      return MakeTar(src);
                default:              return MakeCommon(src);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// MakeZip
        ///
        /// <summary>
        /// Creates a new instance of the ZipOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ZipOption MakeZip(CompressRuntimeSetting src, CompressSetting common) => new ZipOption
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            EncryptionMethod  = src.EncryptionMethod,
            ThreadCount       = src.ThreadCount,
            CodePage          = common.UseUtf8 ? CodePage.Utf8 : CodePage.Oem,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// MakeSevenZip
        ///
        /// <summary>
        /// Creates a new instance of the SevenZipOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SevenZipOption MakeSevenZip(CompressRuntimeSetting src) => new SevenZipOption
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSfxOption
        ///
        /// <summary>
        /// Creates a new instance of the SfxOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SfxOption MakeSfx(CompressRuntimeSetting src) => new SfxOption
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
            Module            = src.Sfx,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateTarOption
        ///
        /// <summary>
        /// Creates a new instance of the TarOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static TarOption MakeTar(CompressRuntimeSetting src) => new TarOption
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateArchiveOption
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ArchiveOption MakeCommon(CompressRuntimeSetting src) => new ArchiveOption
        {
            CompressionLevel = src.CompressionLevel,
            ThreadCount      = src.ThreadCount,
        };

        #endregion
    }
}
