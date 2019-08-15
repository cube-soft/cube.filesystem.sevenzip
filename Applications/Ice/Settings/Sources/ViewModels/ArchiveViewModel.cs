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
    /// ArchiveViewModel
    ///
    /// <summary>
    /// Represents the base class of the compressing and extracting
    /// view-models.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ArchiveViewModel<TModel> : Presentable<TModel>
        where TModel : ArchiveValue
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade of models.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveViewModel(TModel facade,
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
        /// SaveOthers
        ///
        /// <summary>
        /// SaveLocation.Others かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveOthers
        {
            get => Facade.SaveLocation == SaveLocation.Others;
            set => SetSaveLocation(SaveLocation.Others, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveSource
        ///
        /// <summary>
        /// SaveLocation.Source かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveSource
        {
            get => Facade.SaveLocation == SaveLocation.Source;
            set => SetSaveLocation(SaveLocation.Source, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveRuntime
        ///
        /// <summary>
        /// SaveLocation.Runtime かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveRuntime
        {
            get => Facade.SaveLocation == SaveLocation.Query;
            set => SetSaveLocation(SaveLocation.Query, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectory
        ///
        /// <summary>
        /// 保存ディレクトリのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SaveDirectory
        {
            get => Facade.SaveDirectory;
            set => Facade.SaveDirectory = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        ///
        /// <summary>
        /// 特定のファイルまたはディレクトリをフィルタリングするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Filtering
        {
            get => Facade.Filtering;
            set => Facade.Filtering = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// 圧縮処理終了後にフォルダを開くかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool OpenDirectory
        {
            get => Facade.OpenMethod.HasFlag(OpenMethod.Open);
            set
            {
                if (value) Facade.OpenMethod |= OpenMethod.Open;
                else Facade.OpenMethod &= ~OpenMethod.Open;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SkipDesktop
        ///
        /// <summary>
        /// 後処理時に対象がデスクトップの場合にスキップするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SkipDesktop
        {
            get => Facade.OpenMethod.HasFlag(OpenMethod.SkipDesktop);
            set
            {
                if (value) Facade.OpenMethod |= OpenMethod.SkipDesktop;
                else Facade.OpenMethod &= ~OpenMethod.SkipDesktop;
            }
        }

        #endregion

        #region Implementations

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

        /* ----------------------------------------------------------------- */
        ///
        /// SetSaveLocation
        ///
        /// <summary>
        /// SaveLocation の値を設定します。
        /// </summary>
        ///
        /// <remarks>
        /// SaveLocation は GUI 上は RadioButton で表現されています。
        /// そこで、SetSaveLocation() では Checked = true のタイミングで
        /// 値の内容を更新する事とします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetSaveLocation(SaveLocation value, bool check)
        {
            if (!check || Facade.SaveLocation == value) return;
            Facade.SaveLocation = value;
        }

        #endregion
    }
}
