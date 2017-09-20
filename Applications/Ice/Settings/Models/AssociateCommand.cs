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
using System.Reflection;
using Cube.FileSystem.Ice;
using Cube.Log;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateCommand
    /// 
    /// <summary>
    /// ファイルの関連付けの更新を実行するコマンドです。
    /// </summary>
    /// 
    /// <remarks>
    /// 関連付けの更新には管理者権限が必要なため、外部プログラムを
    /// 利用します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal class AssociateCommand
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateCommand
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="settings">設定用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public AssociateCommand(AssociateSettings settings)
        {
            Settings = settings;
            Reset();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        /// 
        /// <summary>
        /// ファイルの関連付けに関するユーザ設定を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public AssociateSettings Settings { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        /// 
        /// <summary>
        /// ファイルの関連付けを状態を更新します。
        /// </summary>
        /// 
        /// <param name="force">強制的に更新するかどうかを示す値</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Execute(bool force)
        {
            try
            {
                if (!force && !IsChanged()) return;

                var asm = Assembly.GetExecutingAssembly().Location;
                var dir = System.IO.Path.GetDirectoryName(asm);
                var exe = System.IO.Path.Combine(dir, Properties.Resources.FileAssociate);

                var process = System.Diagnostics.Process.Start(exe);
                process.WaitForExit();

                Reset();
            }
            catch (Exception err) { this.LogWarn(err.ToString(), err); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// IsChanged
        /// 
        /// <summary>
        /// ファイルの関連付け状態を表す値が変更されたかどうかを判別
        /// します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private bool IsChanged()
        {
            foreach (var item in Settings.Value)
            {
                if (!_prev.ContainsKey(item.Key) || _prev[item.Key] != item.Value) return true;
            }
            return false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        /// 
        /// <summary>
        /// 起点となる値を再設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _prev.Clear();
            foreach (var item in Settings.Value) _prev.Add(item);
        }

        #region Fields
        private IDictionary<string, bool> _prev = new Dictionary<string, bool>();
        #endregion

        #endregion
    }
}
