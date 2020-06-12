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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

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
            Debug.Assert(Option != null && dest != null);

            var src = new Dictionary<string, PropVariant>(_dic);
            if (Option.CodePage != CodePage.Oem)
            {
                src.Add("cp", PropVariant.Create((uint)Option.CodePage));
            }

            var values = CreateValues(src.Values);

            try
            {
                var k = CreateNames(src.Keys);
                var v = values.AddrOfPinnedObject();
                var result = dest.SetProperties(k, v, (uint)k.Length);
                Debug.Assert(result == 0);
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
        private string[] CreateNames(IEnumerable<string> src) => new[]
        {
            "x",
            "mt",
        }.Concat(src).ToArray();

        /* ----------------------------------------------------------------- */
        ///
        /// CreateValues
        ///
        /// <summary>
        /// ISetProperties オブジェクトに設定する値一覧を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private GCHandle CreateValues(IEnumerable<PropVariant> src) => GCHandle.Alloc(
            new[]
            {
                PropVariant.Create((uint)Option.CompressionLevel),
                PropVariant.Create((uint)Option.ThreadCount),
            }.Concat(src).ToArray(),
            GCHandleType.Pinned
        );

        #endregion

        #region Fields
        private readonly IDictionary<string, PropVariant> _dic = new Dictionary<string, PropVariant>();
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOptionSetterExtension
    ///
    /// <summary>
    /// Provides extended methods for the ArchiveOptionSetter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ArchiveOptionSetterExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Converts from the specified object to the new instance of
        /// the ArchiveOptionSetter class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static ArchiveOptionSetter Convert(this ArchiveOption src, Format format)
        {
            if (src == null) return null;
            switch (format)
            {
                case Format.Zip:      return new ZipOptionSetter(src);
                case Format.SevenZip: return new SevenZipOptionSetter(src);
                case Format.Sfx:      return new SevenZipOptionSetter(src);
                case Format.Tar:      return null;
                default:              return new ArchiveOptionSetter(src);
            }
        }

        #endregion
    }
}
