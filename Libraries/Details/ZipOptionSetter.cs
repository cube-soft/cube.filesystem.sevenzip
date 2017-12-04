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
using System.Linq;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ZipOptionSetter
    /// 
    /// <summary>
    /// ZIP 圧縮ファイルのオプション項目を設定するためのクラスです。
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
        /// SupportedMethods
        ///
        /// <summary>
        /// 設定可能な圧縮方法一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static CompressionMethod[] SupportedMethods
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
                AddCompressionMethod(zo);
                AddEncryptionMethod(zo);
                AddUseUtf8(zo);
            }
            base.Execute(dest);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// AddCompressionMethod
        ///
        /// <summary>
        /// 圧縮方式を追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void AddCompressionMethod(ZipOption zo)
        {
            var value = zo.CompressionMethod;
            if (!SupportedMethods.Contains(value)) return;
            Add("m", PropVariant.Create(value.ToString()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddEncryptionMethod
        ///
        /// <summary>
        /// 暗号化方式を追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void AddEncryptionMethod(ZipOption zo)
        {
            var value = zo.EncryptionMethod;
            if (value == EncryptionMethod.Default) return;
            Add("em", PropVariant.Create(value.ToString()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddUseUtf8
        ///
        /// <summary>
        /// ファイル名を UTF-8 に変換するかどうかを示す値を追加します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void AddUseUtf8(ZipOption zo)
        {
            var value = zo.UseUtf8;
            if (value) Add("cu", PropVariant.Create(value));
        }

        #endregion
    }
}
