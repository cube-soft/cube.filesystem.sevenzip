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
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewResource
    ///
    /// <summary>
    /// 表示時に使用するリソースを定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ViewResource
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        /// 
        /// <summary>
        /// 表示文字列と Format オブジェクトの対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Format>> Formats
            => _formats = _formats ?? new List<KeyValuePair<string, Format>>
        {
            Pair("Zip",                          Format.Zip),
            Pair("7z",                           Format.SevenZip),
            Pair("Tar",                          Format.Tar),
            Pair(Properties.Resources.FormatSfx, Format.Sfx),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevels
        /// 
        /// <summary>
        /// 表示文字列と CompressionLevel オブジェクトの対応関係を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, CompressionLevel>> CompressionLevels
            => _compressionLevels = _compressionLevels ??
            new List<KeyValuePair<string, CompressionLevel>>
        {
            Pair(Properties.Resources.LevelNone,   CompressionLevel.None),
            Pair(Properties.Resources.LevelFast,   CompressionLevel.Fast),
            Pair(Properties.Resources.LevelLow,    CompressionLevel.Low),
            Pair(Properties.Resources.LevelNormal, CompressionLevel.Normal),
            Pair(Properties.Resources.LevelHigh,   CompressionLevel.High),
            Pair(Properties.Resources.LevelUltra,  CompressionLevel.Ultra)
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethods
        /// 
        /// <summary>
        /// 表示文字列と CompressionMethod オブジェクトの対応関係を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IDictionary<Format, IList<KeyValuePair<string, CompressionMethod>>> CompressionMethods
            => _compressionMethods = _compressionMethods ?? 
            new Dictionary<Format, IList<KeyValuePair<string, CompressionMethod>>>
        {
            {
                Format.Zip, new List<KeyValuePair<string, CompressionMethod>>
                {
                    Pair("Deflate",   CompressionMethod.Deflate),
                    Pair("Deflate64", CompressionMethod.Deflate64),
                    Pair("BZip2",     CompressionMethod.BZip2),
                    Pair("LZMA",      CompressionMethod.Lzma),
                    Pair("PPMd",      CompressionMethod.Ppmd),
                    Pair("Copy",      CompressionMethod.Copy),
                }
            },
            {
                Format.SevenZip, new List<KeyValuePair<string, CompressionMethod>>
                {
                    Pair("LZMA",      CompressionMethod.Lzma),
                    Pair("LZMA2",     CompressionMethod.Lzma2),
                    Pair("PPMd",      CompressionMethod.Ppmd),
                    Pair("BZip2",     CompressionMethod.BZip2),
                    Pair("Deflate",   CompressionMethod.Deflate),
                    Pair("Copy",      CompressionMethod.Copy),
                }
            },
            {
                Format.Tar, new List<KeyValuePair<string, CompressionMethod>>
                {
                    Pair("GZip",      CompressionMethod.GZip),
                    Pair("BZip2",     CompressionMethod.BZip2),
                    Pair("XZ",        CompressionMethod.XZ),
                    Pair("Copy",      CompressionMethod.Copy),
                }
            },
            {
                Format.Sfx, new List<KeyValuePair<string, CompressionMethod>>
                {
                    Pair("LZMA",      CompressionMethod.Lzma),
                    Pair("LZMA2",     CompressionMethod.Lzma2),
                    Pair("PPMd",      CompressionMethod.Ppmd),
                    Pair("BZip2",     CompressionMethod.BZip2),
                    Pair("Deflate",   CompressionMethod.Deflate),
                    Pair("Copy",      CompressionMethod.Copy),
                }
            }
        };

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethods
        /// 
        /// <summary>
        /// 表示文字列と EncryptionMethod オブジェクトの対応関係を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, EncryptionMethod>> EncryptionMethods
            => _encryptionMethods = _encryptionMethods ??
            new List<KeyValuePair<string, EncryptionMethod>>
        {
            Pair("ZipCrypto", EncryptionMethod.ZipCrypto),
            Pair("AES128",    EncryptionMethod.Aes128),
            Pair("AES192",    EncryptionMethod.Aes192),
            Pair("AES256",    EncryptionMethod.Aes256),
        };

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsEncryptionSupported
        /// 
        /// <summary>
        /// 暗号化に対応しているかどうかを判別します。
        /// </summary>
        /// 
        /// <param name="format">圧縮ファイル形式</param>
        /// 
        /// <returns>暗号化に対応しているかどうか</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsEncryptionSupported(Format format)
            => format == Format.Zip ||
               format == Format.SevenZip ||
               format == Format.Sfx;

        /* ----------------------------------------------------------------- */
        ///
        /// GetCompressionMethod
        /// 
        /// <summary>
        /// 圧縮ファイル形式が対応している CompressionMethod とその表示
        /// 文字列一覧を取得します。
        /// </summary>
        /// 
        /// <param name="format">圧縮ファイル形式</param>
        /// 
        /// <returns>
        /// 表示文字列と CompressionMethod オブジェクトの対応関係
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, CompressionMethod>> GetCompressionMethod(Format format)
            => CompressionMethods.ContainsKey(format) ? CompressionMethods[format] : null;

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter
        /// 
        /// <summary>
        /// 圧縮ファイル形式に対応する拡張子フィルタを取得します。
        /// </summary>
        /// 
        /// <param name="format">圧縮ファイル形式</param>
        /// 
        /// <returns>拡張子フィルタ</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetFilter(Format format) => GetFilter(format.ToString());

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter
        /// 
        /// <summary>
        /// 圧縮ファイル形式に対応する拡張子フィルタを取得します。
        /// </summary>
        /// 
        /// <param name="format">圧縮ファイル形式</param>
        /// 
        /// <returns>拡張子フィルタ</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetFilter(string format)
        {
            var cvt  = format.ToLower();
            var dest = cvt == "zip"      ? Properties.Resources.FilterZip :
                       cvt == "7z"       ? Properties.Resources.FilterSevenZip :
                       cvt == "sevenzip" ? Properties.Resources.FilterSevenZip :
                       cvt == "tar"      ? Properties.Resources.FilterTar :
                       cvt == "gzip"     ? Properties.Resources.FilterGzip :
                       cvt == "bzip2"    ? Properties.Resources.FilterBzip2 :
                       cvt == "xz"       ? Properties.Resources.FilterXZ :
                       cvt == "sfx"      ? Properties.Resources.FilterSfx :
                       string.Empty;

            return !string.IsNullOrEmpty(dest) ?
                   $"{dest}|{Properties.Resources.FilterAll}" :
                   Properties.Resources.FilterAll;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Pair(T, U)
        /// 
        /// <summary>
        /// KeyValuePair(T, U) を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static KeyValuePair<K, V> Pair<K, V>(K key, V value)
            => new KeyValuePair<K, V>(key, value);

        #region Fields
        private static IList<KeyValuePair<string, Format>> _formats;
        private static IList<KeyValuePair<string, EncryptionMethod>> _encryptionMethods;
        private static IList<KeyValuePair<string, CompressionLevel>> _compressionLevels;
        private static IDictionary<Format, IList<KeyValuePair<string, CompressionMethod>>> _compressionMethods;
        #endregion

        #endregion
    }
}
