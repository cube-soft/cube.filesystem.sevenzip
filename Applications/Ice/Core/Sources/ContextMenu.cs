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
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextMenu
    ///
    /// <summary>
    /// コンテキストメニューを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ContextMenu : SerializableBase
    {
        #region Constructors

        /* --------------------------------------------------------------------- */
        ///
        /// ContextMenu
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        public ContextMenu()
        {
            Reset();
        }

        #endregion

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// メニューの表示名を取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// メニュー実行時の引数を取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public string Arguments
        {
            get => _arguments;
            set => SetProperty(ref _arguments, value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// IconIndex
        ///
        /// <summary>
        /// メニューのアイコンを示すインデックスを取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public int IconIndex
        {
            get => _iconIndex;
            set => SetProperty(ref _iconIndex, value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Children
        ///
        /// <summary>
        /// 子要素を取得または設定します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public IList<ContextMenu> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// デシリアライズ直前に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 設定をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _name         = string.Empty;
            _arguments    = string.Empty;
            _iconIndex    = 0;
            _children     = new List<ContextMenu>();
        }

        #endregion

        #region Fields
        private string _name;
        private string _arguments;
        private int _iconIndex;
        private IList<ContextMenu> _children;
        #endregion
    }
}
