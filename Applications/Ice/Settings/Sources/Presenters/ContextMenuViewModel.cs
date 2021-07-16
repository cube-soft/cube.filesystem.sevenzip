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
    /// ContextMenuViewModel
    ///
    /// <summary>
    /// Provides functionality to associate the ContextMenuValue object
    /// and a view.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ContextMenuViewModel : Presentable<ContextMenuValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ContextMenuViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ContextMenuViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade of models.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ContextMenuViewModel(ContextMenuValue facade,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(facade, aggregator, context)
        {
            Facade.PropertyChanged += (s, e) => {
                OnPropertyChanged(e);
                if (e.PropertyName == nameof(ContextMenuValue.UseCustom)) Refresh(nameof(PresetEnabled));
            };
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// PresetEnabled
        ///
        /// <summary>
        /// プリセット項目が有効化されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool PresetEnabled => !Facade.UseCustom;

        #region Compress

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
            get => Facade.Preset.HasFlag(PresetMenu.Compress);
            set => Set(PresetMenu.Compress, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressZip
        ///
        /// <summary>
        /// Zip で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressZip
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressZip);
            set => Set(PresetMenu.CompressZip, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressZipPassword
        ///
        /// <summary>
        /// パスワード付 Zip で圧縮の項目が有効かどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressZipPassword
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressZipPassword);
            set => Set(PresetMenu.CompressZipPassword, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressSevenZip
        ///
        /// <summary>
        /// 7z で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressSevenZip
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressSevenZip);
            set => Set(PresetMenu.CompressSevenZip, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressGZip
        ///
        /// <summary>
        /// GZip で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressGZip
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressGZip);
            set => Set(PresetMenu.CompressGZip, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressBZip2
        ///
        /// <summary>
        /// BZip2 で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressBZip2
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressBZip2);
            set => Set(PresetMenu.CompressBZip2, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressXZ
        ///
        /// <summary>
        /// XZ で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressXZ
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressXz);
            set => Set(PresetMenu.CompressXz, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressSfx
        ///
        /// <summary>
        /// 自己解凍形式で圧縮の項目が有効かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressSfx
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressSfx);
            set => Set(PresetMenu.CompressSfx, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressOthers
        ///
        /// <summary>
        /// 詳細を設定して圧縮の項目が有効かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CompressOthers
        {
            get => Facade.Preset.HasFlag(PresetMenu.CompressOthers);
            set => Set(PresetMenu.CompressOthers, value);
        }

        #endregion

        #region Extract

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
            get => Facade.Preset.HasFlag(PresetMenu.Extract);
            set => Set(PresetMenu.Extract, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractSource
        ///
        /// <summary>
        /// ここに解凍の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ExtractSource
        {
            get => Facade.Preset.HasFlag(PresetMenu.ExtractSource);
            set => Set(PresetMenu.ExtractSource, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractDesktop
        ///
        /// <summary>
        /// デスクトップに解凍の項目が有効かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ExtractDesktop
        {
            get => Facade.Preset.HasFlag(PresetMenu.ExtractDesktop);
            set => Set(PresetMenu.ExtractDesktop, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractMyDocuments
        ///
        /// <summary>
        /// マイドキュメントに解凍の項目が有効かどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ExtractMyDocuments
        {
            get => Facade.Preset.HasFlag(PresetMenu.ExtractMyDocuments);
            set => Set(PresetMenu.ExtractMyDocuments, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractRuntime
        ///
        /// <summary>
        /// 場所を指定して解凍の項目が有効かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ExtractRuntime
        {
            get => Facade.Preset.HasFlag(PresetMenu.ExtractRuntime);
            set => Set(PresetMenu.ExtractRuntime, value);
        }

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// コンテキストメニューを規定値に再設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => Facade.Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Customize
        ///
        /// <summary>
        /// カスタマイズを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Customize()
        {
            var e = Query.NewMessage(Facade.UseCustom ?
                Facade.Custom :
                Facade.Preset.ToContextMenuGroup()
            );

            // Views.ShowCustomizeView(e);
            if (!e.Cancel) Facade.Customize(e.Value);
        }

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
        private void Set(PresetMenu value, bool check)
        {
            if (check) Facade.Preset |= value;
            else Facade.Preset &= ~value;
        }

        #endregion
    }
}
