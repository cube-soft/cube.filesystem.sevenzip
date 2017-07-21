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

namespace Cube.FileSystem.SevenZip
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
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ZipOptionSetter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="option">オプション</param>
        ///
        /* ----------------------------------------------------------------- */
        public ZipOptionSetter(ArchiveOption option) : base(option) { }

        #endregion

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
            if (Option is ZipOption zo)
            {
                var method = AllowedCompressionMethods.Contains(zo.CompressionMethod) ?
                             zo.CompressionMethod :
                             CompressionMethod.Default;
                if (method != CompressionMethod.Default) Add("m", method.ToString().ToLower());
                if (zo.IsEncrypted) Add("em", zo.EncryptionMethod.ToString().ToLower());
            }
            base.Execute(dest);
        }

        #endregion
    }
}
