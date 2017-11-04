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
        /// Format
        /// 
        /// <summary>
        /// 表示文字列と Format オブジェクトの対応関係を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, Format>> Format
            => _format = _format ?? new List<KeyValuePair<string, Format>>
        {
            Pair("Zip",                          SevenZip.Format.Zip),
            Pair("7z",                           SevenZip.Format.SevenZip),
            Pair("Tar",                          SevenZip.Format.Tar),
            Pair(Properties.Resources.FormatSfx, SevenZip.Format.Sfx),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        /// 
        /// <summary>
        /// 表示文字列と CompressionLevel オブジェクトの対応関係を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, CompressionLevel>> CompressionLevel
            => _compressionLevel = _compressionLevel ??
            new List<KeyValuePair<string, CompressionLevel>>
        {
            Pair(Properties.Resources.LevelNone,   SevenZip.CompressionLevel.None),
            Pair(Properties.Resources.LevelFast,   SevenZip.CompressionLevel.Fast),
            Pair(Properties.Resources.LevelLow,    SevenZip.CompressionLevel.Low),
            Pair(Properties.Resources.LevelNormal, SevenZip.CompressionLevel.Normal),
            Pair(Properties.Resources.LevelHigh,   SevenZip.CompressionLevel.High),
            Pair(Properties.Resources.LevelUltra,  SevenZip.CompressionLevel.Ultra)
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
                SevenZip.Format.Zip, new List<KeyValuePair<string, CompressionMethod>>
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
                SevenZip.Format.SevenZip, new List<KeyValuePair<string, CompressionMethod>>
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
                SevenZip.Format.Tar, new List<KeyValuePair<string, CompressionMethod>>
                {
                    Pair("GZip",      CompressionMethod.GZip),
                    Pair("BZip2",     CompressionMethod.BZip2),
                    Pair("XZ",        CompressionMethod.XZ),
                    Pair("Copy",      CompressionMethod.Copy),
                }
            },
            {
                SevenZip.Format.Sfx, new List<KeyValuePair<string, CompressionMethod>>
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
        /// EncryptionMethod
        /// 
        /// <summary>
        /// 表示文字列と EncryptionMethod オブジェクトの対応関係を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IList<KeyValuePair<string, EncryptionMethod>> EncryptionMethod
            => _encryptionMethod = _encryptionMethod ??
            new List<KeyValuePair<string, EncryptionMethod>>
        {
            Pair("ZipCrypto", SevenZip.EncryptionMethod.ZipCrypto),
            Pair("AES128",    SevenZip.EncryptionMethod.Aes128),
            Pair("AES192",    SevenZip.EncryptionMethod.Aes192),
            Pair("AES256",    SevenZip.EncryptionMethod.Aes256),
        };

        #endregion

        #region Methods

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
        private static IList<KeyValuePair<string, Format>> _format;
        private static IList<KeyValuePair<string, EncryptionMethod>> _encryptionMethod;
        private static IList<KeyValuePair<string, CompressionLevel>> _compressionLevel;
        private static IDictionary<Format, IList<KeyValuePair<string, CompressionMethod>>> _compressionMethods;
        #endregion

        #endregion
    }
}
