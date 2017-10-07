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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// Shortcut
    /// 
    /// <summary>
    /// ショートカットの作成および削除を行うクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Shortcut
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcut
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="file">ショートカットのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public Shortcut(string file) : this(file, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcut
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="file">ショートカットのパス</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public Shortcut(string file, Operator io)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentException();

            FileName = file.EndsWith(Extension) ?
                       file.Substring(0, file.Length - Extension.Length) :
                       file;
            _io = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Extension
        /// 
        /// <summary>
        /// ショートカットを示す拡張子を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static readonly string Extension = ".lnk";

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ショートカットのパスを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// このプロパティは末尾の .lnk は除外されます。拡張子を含めた
        /// パスが必要な場合は FullName を使用して下さい。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public string FileName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        /// 
        /// <summary>
        /// ショートカットの完全な名前を取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// FileName に .lnk が付加された値となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public string FullName => $"{FileName}.lnk";

        /* ----------------------------------------------------------------- */
        ///
        /// Link
        /// 
        /// <summary>
        /// リンク先のパスを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Link { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Arguments
        /// 
        /// <summary>
        /// リンク先のパスに付加される引数一覧を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Arguments { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// IconLocation
        /// 
        /// <summary>
        /// 表示されるアイコンのパスを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string IconLocation { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        /// 
        /// <summary>
        /// ショートカットが存在するかどうかを示す値を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Exists
            => !string.IsNullOrEmpty(FileName) && _io.Exists(FullName);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ショートカットを作成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Create()
        {
            if (string.IsNullOrEmpty(Link) || !_io.Exists(Link)) return;

            var guid = new Guid("00021401-0000-0000-C000-000000000046");
            var type = Type.GetTypeFromCLSID(guid);
            var sh   = Activator.CreateInstance(type) as IShellLink;

            Debug.Assert(sh != null);

            try
            {
                var args = Arguments != null && Arguments.Count() > 0 ?
                           Arguments.Aggregate((s, o) => s + $" \"{o}\"").Trim() :
                           string.Empty;

                sh.SetPath(Link);
                sh.SetArguments(args);
                sh.SetShowCmd(1); // SW_SHOWNORMAL
                sh.SetIconLocation(GetIconFileName(), GetIconIndex());

                Debug.Assert(sh is IPersistFile);
                ((IPersistFile)sh).Save(FullName, true);
            }
            finally { Marshal.ReleaseComObject(sh); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        /// 
        /// <summary>
        /// ショートカットを削除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Delete()
        {
            if (Exists) _io.Delete(FullName);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetIconFileName
        /// 
        /// <summary>
        /// アイコンのファイル名部分を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string GetIconFileName()
        {
            var index = IconLocation.LastIndexOf(',');
            return (index > 0) ? IconLocation.Substring(0, index) : IconLocation;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetIconIndex
        /// 
        /// <summary>
        /// アイコンのインデックス部分を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private int GetIconIndex()
        {
            var index = IconLocation.LastIndexOf(',');
            if (index > 0 && index < IconLocation.Length - 1)
            {
                int.TryParse(IconLocation.Substring(index + 1), out int dest);
                return dest;
            }
            else return 0;
        }

        #region Fields
        private Operator _io;
        #endregion

        #endregion
    }
}
