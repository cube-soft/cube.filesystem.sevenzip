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
        /// Creates a new instance of the CompressionOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Runtime settings.</param>
        /// <param name="settings">User settings.</param>
        ///
        /// <remarks>CompressionOption object.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionOption ToOption(this CompressRuntimeSetting src,
            SettingFolder settings) => src.ToOption(settings.Value.Compress);

        /* ----------------------------------------------------------------- */
        ///
        /// ToOption
        ///
        /// <summary>
        /// Creates a new instance of the CompressionOption class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Runtime settings.</param>
        /// <param name="settings">User settings for compression.</param>
        ///
        /// <remarks>CompressionOption object.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionOption ToOption(this CompressRuntimeSetting src,
            CompressSetting settings) => src.Format switch
        {
            Format.Zip      => MakeZip(src, settings),
            Format.SevenZip => MakeSevenZip(src),
            Format.Sfx      => MakeSfx(src),
            Format.Tar      => MakeTar(src),
            _               => MakeCommon(src),
        };

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// MakeZip
        ///
        /// <summary>
        /// Creates a new instance of the CompressionOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static CompressionOption MakeZip(CompressRuntimeSetting src, CompressSetting common) => new()
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
        private static CompressionOption MakeSevenZip(CompressRuntimeSetting src) => new()
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
        private static SfxOption MakeSfx(CompressRuntimeSetting src) => new()
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
        /// Creates a new instance of the CompressionOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static CompressionOption MakeTar(CompressRuntimeSetting src) => new()
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// MakeCommon
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveOption class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static CompressionOption MakeCommon(CompressRuntimeSetting src) => new()
        {
            CompressionLevel = src.CompressionLevel,
            ThreadCount      = src.ThreadCount,
        };

        #endregion
    }
}
