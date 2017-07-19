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

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteMode
    /// 
    /// <summary>
    /// 上書き方法を示す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum OverwriteMode
    {
        Query        = 0x000,
        Cancel       = 0x002,
        Yes          = 0x006,
        No           = 0x007,
        Rename       = 0x010,

        Always       = 0x100,
        AlwaysYes    = Always | Yes,
        AlwaysNo     = Always | No,
        AlwaysRename = Always | Rename,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteInfo
    /// 
    /// <summary>
    /// 上書き対象となる項目の情報を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class OverwriteInfo
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteInfo
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="src">上書き元の情報</param>
        /// <param name="dest">上書き先の情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public OverwriteInfo(IInformation src, IInformation dest)
        {
            Source      = src;
            Destination = dest;
        }

        #endregion

        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        /// 
        /// <summary>
        /// 上書き元の情報を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IInformation Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// 上書き先の情報を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IInformation Destination { get; }

        #endregion
    }
}
