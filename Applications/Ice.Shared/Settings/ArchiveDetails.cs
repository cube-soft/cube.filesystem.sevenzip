/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Reflection;
using Cube.FileSystem.SevenZip;

namespace Cube.FileSystem.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveDetails
    ///
    /// <summary>
    /// 圧縮処理の実行時詳細設定を保持するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveDetails
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveDetails
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveDetails()
        {
            var asm = Assembly.GetExecutingAssembly();
            var dir = System.IO.Path.GetDirectoryName(asm.Location);
            SfxModule = System.IO.Path.Combine(dir, "7z.sfx");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveDetails
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="format">圧縮形式</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveDetails(Format format) : this()
        {
            Format = format;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        /// 
        /// <summary>
        /// ファイル形式を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        /// 
        /// <summary>
        /// 保存先パスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        /// 
        /// <summary>
        /// パスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// SfxModule
        /// 
        /// <summary>
        /// 自己解凍形式モジュールのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SfxModule { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        /// 
        /// <summary>
        /// 圧縮レベルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Ultra;

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        /// 
        /// <summary>
        /// 圧縮方法を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethod
        /// 
        /// <summary>
        /// 暗号化方法を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod { get; set; } = EncryptionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        /// 
        /// <summary>
        /// 圧縮処理時の最大スレッド数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount { get; set; } = Environment.ProcessorCount;

        #endregion
    }
}
