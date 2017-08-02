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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Cube.FileSystem.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateRegistrar
    /// 
    /// <summary>
    /// ファイルの関連付けに関するレジストリの更新を行うクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class AssociateRegistrar
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateRegistrar
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="file">ダブルクリック時に実行するファイル</param>
        /// 
        /* ----------------------------------------------------------------- */
        public AssociateRegistrar(string file)
        {
            FileName = file;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ダブルクリック時に実行されるファイルを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string FileName { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileName
        /// 
        /// <summary>
        /// ダブルクリック時に実行されるファイルの引数一覧を取得または
        /// 設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// Arguments で設定されたものに "%1" を加えたものが実際の
        /// 引数となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public IList<string> Arguments { get; } = new List<string>();

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
        /// Command
        /// 
        /// <summary>
        /// レジストリに登録されるコマンドラインを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Command
            => string.Format("{0}{1}\"%1\"",
                $"\"{FileName}\"",
                Arguments.Aggregate(" ", (s, o) => s + $"\"{o}\" ")
            );

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        /// 
        /// <summary>
        /// ファイルの関連付けを更新します。
        /// </summary>
        /// 
        /// <param name="extensions">
        /// ファイルの関連付けを定義したオブジェクト
        /// </param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Update(IDictionary<string, bool> extensions)
        {
            if (string.IsNullOrEmpty(FileName) || !System.IO.File.Exists(FileName)) return;
            foreach (var kv in extensions) Update(kv.Key, kv.Value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        /// 
        /// <summary>
        /// ファイルの関連付けを更新します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Update(string extension, bool enabled)
        {
            if (enabled) Create(extension);
            else Delete(extension);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// ファイルの関連付け用のレジストリ項目を作成して登録します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Create(string extension)
        {
            var id   = extension.TrimStart('.');
            var name = GetSubKeyName(id);

            using (var key = Registry.ClassesRoot.CreateSubKey(id))
            {
                key.SetValue("", $"{id} {Properties.Resources.FileSuffix}".ToUpper());
                using (var shell = key.CreateSubKey("shell"))
                {
                    shell.SetValue("", "open");
                    using (var cmd = shell.CreateSubKey("open/command")) cmd.SetValue("", Command);
                }
                using (var icon = key.CreateSubKey("DefaultIcon")) icon.SetValue("", IconLocation);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        /// 
        /// <summary>
        /// ファイルの関連付け用のレジストリ項目を削除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Delete(string extension)
        {
            var root = Registry.ClassesRoot;
            root.DeleteSubKeyTree(GetSubKeyName(extension), false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetSubKeyName
        /// 
        /// <summary>
        /// サブキー名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string GetSubKeyName(string id)
            => $"{System.IO.Path.GetFileNameWithoutExtension(FileName)}_{id}".ToLower();

        #endregion
    }
}
