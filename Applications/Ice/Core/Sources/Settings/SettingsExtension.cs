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
namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsExtension
    ///
    /// <summary>
    /// ユーザ設定に関する拡張メソッド用のクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class SettingsExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToOption
        ///
        /// <summary>
        /// ArchiveOption オブジェクトに変換します。
        /// </summary>
        ///
        /// <param name="src">実行時圧縮設定</param>
        /// <param name="settings">ユーザ設定</param>
        ///
        /* ----------------------------------------------------------------- */
        public static ArchiveOption ToOption(this CompressRtsValue src, SettingFolder settings) =>
            ToOption(src, settings.Value.Archive);

        /* ----------------------------------------------------------------- */
        ///
        /// ToOption
        ///
        /// <summary>
        /// ArchiveOption オブジェクトに変換します。
        /// </summary>
        ///
        /// <param name="src">実行時圧縮設定</param>
        /// <param name="common">圧縮に関するユーザ設定</param>
        ///
        /* ----------------------------------------------------------------- */
        public static ArchiveOption ToOption(this CompressRtsValue src, CompressSettingValue common)
        {
            switch (src.Format)
            {
                case Format.Zip:      return CreateZipOption(src, common);
                case Format.SevenZip: return CreateSevenZipOption(src, common);
                case Format.Sfx:      return CreateSfxOption(src, common);
                case Format.Tar:      return CreateTarOption(src, common);
                default:              return CreateArchiveOption(src, common);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateZipOption
        ///
        /// <summary>
        /// ZipOption オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ZipOption CreateZipOption(CompressRtsValue src, CompressSettingValue common) =>
            new ZipOption
            {
                CompressionLevel  = src.CompressionLevel,
                CompressionMethod = src.CompressionMethod,
                EncryptionMethod  = src.EncryptionMethod,
                ThreadCount       = src.ThreadCount,
                CodePage          = common.UseUtf8 ? CodePage.Utf8 : CodePage.Oem,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSevenZipOption
        ///
        /// <summary>
        /// SevenZipOption オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SevenZipOption CreateSevenZipOption(CompressRtsValue src, CompressSettingValue common) =>
            new SevenZipOption
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
        /// SfxOption オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static SfxOption CreateSfxOption(CompressRtsValue src, CompressSettingValue common) =>
            new SfxOption
            {
                CompressionLevel  = src.CompressionLevel,
                CompressionMethod = src.CompressionMethod,
                ThreadCount       = src.ThreadCount,
                Module            = src.SfxModule,
            };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateTarOption
        ///
        /// <summary>
        /// TarOption オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static TarOption CreateTarOption(CompressRtsValue src, CompressSettingValue common) =>
            new TarOption
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
        /// ArchiveOption オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static ArchiveOption CreateArchiveOption(CompressRtsValue src, CompressSettingValue common) =>
            new ArchiveOption
            {
                CompressionLevel = src.CompressionLevel,
                ThreadCount      = src.ThreadCount,
            };

        #endregion
    }
}
