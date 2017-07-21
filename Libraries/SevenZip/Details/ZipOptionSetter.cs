/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System.Linq;

namespace Cube.FileSystem.SevenZip.Details
{
    /* --------------------------------------------------------------------- */
    ///
    /// ZipOptionSetter
    /// 
    /// <summary>
    /// 圧縮ファイルのオプション項目を設定するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ZipOptionSetter : ArchiveOptionSetter
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// AllowedCompressionMethods
        ///
        /// <summary>
        /// 設定可能な圧縮方法一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static CompressionMethod[] AllowedCompressionMethods
            => new[]
            {
                CompressionMethod.Copy,
                CompressionMethod.Deflate,
                CompressionMethod.Deflate64,
                CompressionMethod.BZip2,
                CompressionMethod.Lzma,
                CompressionMethod.Ppmd,
            };

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
        public EncryptionMethod EncryptionMethod { get; set; } = EncryptionMethod.ZipCrypto;

        /* ----------------------------------------------------------------- */
        ///
        /// IsEncrypted
        ///
        /// <summary>
        /// 暗号化が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsEncrypted { get; set; } = false;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// オプションをアーカイブ・オブジェクトに設定します。
        /// </summary>
        /// 
        /// <param name="dest">アーカイブ・オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void Execute(ISetProperties dest)
        {
            var method = AllowedCompressionMethods.Contains(CompressionMethod) ?
                         CompressionMethod :
                         CompressionMethod.Default;
            if (method != CompressionMethod.Default) Add("m", method.ToString().ToLower());
            if (IsEncrypted) Add("em", EncryptionMethod.ToString().ToLower());

            base.Execute(dest);
        }

        #endregion
    }
}
