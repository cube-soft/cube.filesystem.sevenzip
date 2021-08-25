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
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutViewModel
    ///
    /// <summary>
    /// Provides functionality to associate the ShortcutValue object
    /// and a view.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShortcutViewModel : Presentable<ShortcutSetting>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ShortcutViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade of models.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutViewModel(ShortcutSetting facade,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(facade, aggregator, context)
        {
            Facade.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Compress
        ///
        /// <summary>
        /// 圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Compress
        {
            get => Facade.Preset.HasFlag(Preset.Compress);
            set => Set(Preset.Compress, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 解凍の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Extract
        {
            get => Facade.Preset.HasFlag(Preset.Extract);
            set => Set(Preset.Extract, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Settings
        {
            get => Facade.Preset.HasFlag(Preset.Settings);
            set => Set(Preset.Settings, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressOption
        ///
        /// <summary>
        /// 圧縮オプションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Preset CompressOption
        {
            get => Facade.Preset & Preset.CompressMask;
            set
            {
                var strip = Facade.Preset & ~Preset.CompressMask;
                Facade.Preset = strip | value;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Sync
        ///
        /// <summary>
        /// ショートカットが実際に存在するかどうかの結果を設定値に反映
        /// させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Sync() => Facade.Load();

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// ユーザ設定に関わる処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Update() => Facade.Save();

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// PresetMenu に値を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(Preset value, bool check)
        {
            if (check) Facade.Preset |= value;
            else Facade.Preset &= ~value;
        }

        #endregion
    }
}
