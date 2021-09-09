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
        /// <param name="password">Password to be set.</param>
        ///
        /// <remarks>CompressionOption object.</remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionOption ToOption(this CompressRuntimeSetting src,
            SettingFolder settings, string password)
        {
            var filter = new Filter(settings.Value.GetFilters(settings.Value.Compress.Filtering));

            return src.Format switch
            {
                Format.Zip      => MakeZip(src, password, filter, settings.Value.Compress),
                Format.SevenZip => MakeSevenZip(src, password, filter),
                Format.Sfx      => MakeSfx(src, password, filter),
                Format.Tar      => MakeTar(src, password, filter),
                _               => MakeCommon(src, password, filter),
            };
        }

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
        private static CompressionOption MakeZip(CompressRuntimeSetting src,
            string password, Filter filter, CompressSetting others) => new()
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            EncryptionMethod  = src.EncryptionMethod,
            Password          = password,
            ThreadCount       = src.ThreadCount,
            CodePage          = others.UseUtf8 ? CodePage.Utf8 : CodePage.Oem,
            Filter            = filter.Match,
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
        private static CompressionOption MakeSevenZip(CompressRuntimeSetting src,
            string password, Filter filter) => new()
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
            Password          = password,
            Filter            = filter.Match,
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
        private static SfxOption MakeSfx(CompressRuntimeSetting src, string password, Filter filter) => new()
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
            Module            = src.Sfx,
            Password          = password,
            Filter            = filter.Match,
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
        private static CompressionOption MakeTar(CompressRuntimeSetting src,
            string password, Filter filter) => new()
        {
            CompressionLevel  = src.CompressionLevel,
            CompressionMethod = src.CompressionMethod,
            ThreadCount       = src.ThreadCount,
            Password          = password,
            Filter            = filter.Match,
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
        private static CompressionOption MakeCommon(CompressRuntimeSetting src,
            string password, Filter filter) => new()
        {
            CompressionLevel  = src.CompressionLevel,
            ThreadCount       = src.ThreadCount,
            Password          = password,
            Filter            = filter.Match,
        };

        #endregion
    }
}
