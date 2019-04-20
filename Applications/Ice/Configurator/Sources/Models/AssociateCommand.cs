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
using Cube.Log;
using System;
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Ice.Configurator
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
        /// Updates file associations from the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Execute()
        {
            try
            {
                if (Settings.Changed)
                {
                    var dir = Assembly.GetExecutingAssembly().GetReader().DirectoryName;
                    var exe = System.IO.Path.Combine(dir, Properties.Resources.FileAssociate);
                    System.Diagnostics.Process.Start(exe).WaitForExit();
                    Settings.Changed = false;
                }
            }
            catch (Exception err) { this.LogWarn(err); }
        }

        #endregion
    }
}
