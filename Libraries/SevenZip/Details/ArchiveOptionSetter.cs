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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOptionSetter
    /// 
    /// <summary>
    /// 圧縮ファイルのオプション項目を設定するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveOptionSetter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveOptionSetter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="option">オプション</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveOptionSetter(ArchiveOption option)
        {
            Option = option;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Option
        ///
        /// <summary>
        /// オプション内容を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveOption Option { get; }

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
        public virtual void Execute(ISetProperties dest)
        {
            if (Option == null || dest == null) return;

            var values = CreateValues();

            try
            {
                var k = CreateNames();
                var v = values.AddrOfPinnedObject();
                var result = dest.SetProperties(k, v, (uint)k.Length);
                if (result != 0) throw new System.IO.IOException($"SetProperties:{result}");
            }
            finally { values.Free(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// オプションを追加します。
        /// </summary>
        /// 
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Add(string name, PropVariant value) => _dic.Add(name, value);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateNames
        ///
        /// <summary>
        /// ISetProperties オブジェクトに設定する名前一覧を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string[] CreateNames()
            => new[]
            {
                "x",
                "mt",
            }.Concat(_dic.Keys).ToArray();

        /* ----------------------------------------------------------------- */
        ///
        /// CreateValues
        ///
        /// <summary>
        /// ISetProperties オブジェクトに設定する値一覧を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private GCHandle CreateValues()
            => GCHandle.Alloc(new[]
                {
                    PropVariant.Create((uint)Option.CompressionLevel),
                    PropVariant.Create((uint)Option.ThreadCount),
                }.Concat(_dic.Values).ToArray(),
                GCHandleType.Pinned
            );

        #endregion

        #region Fields
        private IDictionary<string, PropVariant> _dic = new Dictionary<string, PropVariant>();
        #endregion
    }
}
