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
using System.Linq;
using IWshRuntimeLibrary;

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
            FileName = file;
            _io = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ショートカットのパスを取得します。
        /// </summary>
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
        public IList<string> Arguments { get; set; } = new List<string>();

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
            => !string.IsNullOrEmpty(FileName) && _io.Get(FullName).Exists;

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
            if (string.IsNullOrEmpty(Link) || !_io.Get(Link).Exists) return;

            var sh = new WshShell();
            var sc = sh.CreateShortcut(FullName) as IWshShortcut;

            try
            {
                var args = Arguments != null && Arguments.Count > 0 ?
                           Arguments.Aggregate((s, o) => s + $" \"{o}\"").Trim() :
                           string.Empty;

                sc.TargetPath       = Link;
                sc.Arguments        = args;
                sc.WorkingDirectory = "";
                sc.WindowStyle      = 1;
                sc.IconLocation     = IconLocation;
                sc.Save();
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sc);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sh);
            }
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

        #region Fields
        private Operator _io;
        #endregion
    }
}
