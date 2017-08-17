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
using Microsoft.Win32;
using Cube.Registries;

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
    /// <remarks>
    /// このクラスはレジストリの KEY_CLASSES_ROOT を編集します。
    /// したがって、実行するためには管理者権限が必要となります。
    /// </remarks>
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
        /// Arguments
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
        /// ToolTip
        /// 
        /// <summary>
        /// マウスオーバ時のツールチップ表示をカスタマイズするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool ToolTip { get; set; } = false;

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
            if (string.IsNullOrEmpty(extension)) return;
            if (enabled) Create(extension);
            else Delete(extension);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateToolTip
        /// 
        /// <summary>
        /// マウスオーバ時に表示されるツールチップ表示に関する設定を
        /// 更新します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void UpdateToolTip(RegistryKey key, bool enabled)
        {
            var guid = TooTipKey.ToString("B").ToUpper();
            var name = $@"shellex\{guid}";

            if (enabled)
            {
                var s = ToolTipHandler.ToString("B");
                using (var k = key.CreateSubKey(name)) k.SetValue("", s);
            }
            else key.DeleteSubKeyTree(name, false);
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
            if (string.IsNullOrEmpty(FileName)) return;

            var id   = extension.TrimStart('.');
            var root = Registry.ClassesRoot;
            var name = GetSubKeyName(id);
            using (var key = root.CreateSubKey(name)) Create(key, id);

            Create(extension, name);
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
        private void Create(RegistryKey key, string id)
        {
            key.SetValue("", $"{id} {Properties.Resources.FileSuffix}".ToUpper());

            using (var k = key.CreateSubKey("shell"))
            {
                k.SetValue("", "open");
                using (var cmd = k.CreateSubKey(@"open\command"))
                {
                    cmd.SetValue("", Command);
                }
            }

            using (var k = key.CreateSubKey("DefaultIcon")) k.SetValue("", IconLocation);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        /// 
        /// <summary>
        /// 拡張子を表すレジストリ項目と CubeICE を関連付けるための設定を
        /// 作成します。
        /// </summary>
        /// 
        /// <remarks>
        /// ToolTip の処理が未実装なため、現在は ToolTip に関連する項目は
        /// 強制的に削除しています。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        private void Create(string extension, string name)
        {
            var s = (extension[0] == '.') ? extension : $".{extension}";
            using (var key = Registry.ClassesRoot.CreateSubKey(s.ToLower()))
            {
                var prev = key.GetValue("") as string;
                if (!string.IsNullOrEmpty(prev) && prev != name)
                {
                    key.SetValue(nameof(PreArchiver), prev);
                }
                key.SetValue("", name);

                UpdateToolTip(key, ToolTip);
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
            var name = GetSubKeyName(extension);
            Delete(extension, name);
            Registry.ClassesRoot.DeleteSubKeyTree(name, false);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        /// 
        /// <summary>
        /// 拡張子を表すレジストリ項目と CubeICE を関連付けるための設定を
        /// 削除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Delete(string extension, string name)
        {
            var s = (extension[0] == '.') ? extension : $".{extension}";
            using (var key = Registry.ClassesRoot.CreateSubKey(s.ToLower()))
            {
                var prev = key.GetValue(nameof(PreArchiver), "") as string;
                if (!string.IsNullOrEmpty(prev))
                {
                    key.SetValue("", prev);
                    key.DeleteValue(nameof(PreArchiver), false);
                }
                else key.DeleteValue("", false);

                UpdateToolTip(key, false);
            }
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

        #region Fields
        private static readonly object PreArchiver = null;
        private static readonly Guid TooTipKey = new Guid("{00021500-0000-0000-c000-000000000046}");
        private static readonly Guid ToolTipHandler = new Guid("{cb8641a3-ebc7-4758-a302-aa6667b817c8}");
        #endregion
    }
}
