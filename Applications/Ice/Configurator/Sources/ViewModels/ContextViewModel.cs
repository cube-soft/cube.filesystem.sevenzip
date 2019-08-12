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
namespace Cube.FileSystem.SevenZip.Ice.Configurator
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextViewModel
    ///
    /// <summary>
    /// ContextSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ContextViewModel : Presentable<ContextSettingValue>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ContextViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="facade">Model オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ContextViewModel(ContextSettingValue facade) : base(facade)
        {
            Facade.PropertyChanged += (s, e) => {
                OnPropertyChanged(e);
                if (e.PropertyName == nameof(ContextSettingValue.IsCustomized)) Refresh(nameof(PresetEnabled));
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
        public bool PresetEnabled => !Facade.IsCustomized;

        #region Archive

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// 圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Archive
        {
            get => Facade.Preset.HasFlag(PresetMenu.Archive);
            set => Set(PresetMenu.Archive, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveZip
        ///
        /// <summary>
        /// Zip で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveZip
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveZip);
            set => Set(PresetMenu.ArchiveZip, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveZipPassword
        ///
        /// <summary>
        /// パスワード付 Zip で圧縮の項目が有効かどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveZipPassword
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveZipPassword);
            set => Set(PresetMenu.ArchiveZipPassword, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveSevenZip
        ///
        /// <summary>
        /// 7z で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveSevenZip
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveSevenZip);
            set => Set(PresetMenu.ArchiveSevenZip, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveGZip
        ///
        /// <summary>
        /// GZip で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveGZip
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveGZip);
            set => Set(PresetMenu.ArchiveGZip, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveBZip2
        ///
        /// <summary>
        /// BZip2 で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveBZip2
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveBZip2);
            set => Set(PresetMenu.ArchiveBZip2, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveXZ
        ///
        /// <summary>
        /// XZ で圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveXZ
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveXz);
            set => Set(PresetMenu.ArchiveXz, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveSfx
        ///
        /// <summary>
        /// 自己解凍形式で圧縮の項目が有効かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveSfx
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveSfx);
            set => Set(PresetMenu.ArchiveSfx, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveDetails
        ///
        /// <summary>
        /// 詳細を設定して圧縮の項目が有効かどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ArchiveDetails
        {
            get => Facade.Preset.HasFlag(PresetMenu.ArchiveDetails);
            set => Set(PresetMenu.ArchiveDetails, value);
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
            var e = Query.NewMessage(Facade.IsCustomized ?
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
