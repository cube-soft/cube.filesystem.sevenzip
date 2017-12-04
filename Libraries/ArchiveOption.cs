/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOption
    /// 
    /// <summary>
    /// 圧縮時に設定可能なオプションを保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// 圧縮レベルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Normal;

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// 圧縮処理時の最大スレッド数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount { get; set; } = 1;

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// TarOption
    /// 
    /// <summary>
    /// Tar 圧縮時に設定可能なオプションを保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class TarOption : ArchiveOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// 圧縮方法取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.Copy;

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ZipOption
    /// 
    /// <summary>
    /// Zip 圧縮時に設定可能なオプションを保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ZipOption : ArchiveOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// 圧縮方法取得または設定します。
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
        /// <remarks>
        /// 設定可能な値は ZipCrypto, Aes128, Aes192, Aes256 です。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod { get; set; } = EncryptionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// UseUtf8
        ///
        /// <summary>
        /// ファイル名を UTF-8 に変換するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool UseUtf8 { get; set; } = false;

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipOption
    /// 
    /// <summary>
    /// 7z 圧縮時に設定可能なオプションを保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SevenZipOption : ArchiveOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// 圧縮方法取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.Default;

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SfxOption
    /// 
    /// <summary>
    /// 自己解凍形式の圧縮ファイル作成時に設定可能なオプションを保持する
    /// ためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SfxOption : SevenZipOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Module
        ///
        /// <summary>
        /// 自己解凍形式用モジュールのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Module { get; set; }

        #endregion
    }
}
