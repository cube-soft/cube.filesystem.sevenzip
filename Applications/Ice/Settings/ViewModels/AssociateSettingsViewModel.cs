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
using System.ComponentModel;
using System.Linq;
using Cube.FileSystem.SevenZip.Ice;

namespace Cube.FileSystem.SevenZip.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateSettingsViewModel
    /// 
    /// <summary>
    /// AssociateSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class AssociateSettingsViewModel : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateSettingsViewModel
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="model">Model オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public AssociateSettingsViewModel(AssociateSettings model)
        {
            _model = new AssociateCommand(model);
            _model.Settings.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SevenZip
        /// 
        /// <summary>
        /// *.7z の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool SevenZip
        {
            get { return _model.Settings.SevenZip; }
            set { _model.Settings.SevenZip = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Arj
        /// 
        /// <summary>
        /// *.arj の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Arj
        {
            get { return _model.Settings.Arj; }
            set { _model.Settings.Arj = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BZ2
        /// 
        /// <summary>
        /// *.bz2 の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool BZ2
        {
            get { return _model.Settings.BZ2; }
            set { _model.Settings.BZ2 = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cab
        /// 
        /// <summary>
        /// *.cab の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Cab
        {
            get { return _model.Settings.Cab; }
            set { _model.Settings.Cab = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Chm
        /// 
        /// <summary>
        /// *.chm の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Chm
        {
            get { return _model.Settings.Chm; }
            set { _model.Settings.Chm = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cpio
        /// 
        /// <summary>
        /// *.cpio の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Cpio
        {
            get { return _model.Settings.Cpio; }
            set { _model.Settings.Cpio = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Deb
        /// 
        /// <summary>
        /// *.deb の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Deb
        {
            get { return _model.Settings.Deb; }
            set { _model.Settings.Deb = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dmg
        /// 
        /// <summary>
        /// *.dmg の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Dmg
        {
            get { return _model.Settings.Dmg; }
            set { _model.Settings.Dmg = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Flv
        /// 
        /// <summary>
        /// *.flv の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Flv
        {
            get { return _model.Settings.Flv; }
            set { _model.Settings.Flv = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GZ
        /// 
        /// <summary>
        /// *.gz の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool GZ
        {
            get { return _model.Settings.GZ; }
            set { _model.Settings.GZ = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Hfs
        /// 
        /// <summary>
        /// *.hfs の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Hfs
        {
            get { return _model.Settings.Hfs; }
            set { _model.Settings.Hfs = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Jar
        /// 
        /// <summary>
        /// *.jar の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Jar
        {
            get { return _model.Settings.Jar; }
            set { _model.Settings.Jar = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Iso
        /// 
        /// <summary>
        /// *.iso の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Iso
        {
            get { return _model.Settings.Iso; }
            set { _model.Settings.Iso = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Lzh
        /// 
        /// <summary>
        /// *.lzh の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Lzh
        {
            get { return _model.Settings.Lzh; }
            set { _model.Settings.Lzh = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Nupkg
        /// 
        /// <summary>
        /// *.nupkg の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Nupkg
        {
            get { return _model.Settings.Nupkg; }
            set { _model.Settings.Nupkg = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rar
        /// 
        /// <summary>
        /// *.rar の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Rar
        {
            get { return _model.Settings.Rar; }
            set { _model.Settings.Rar = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rpm
        /// 
        /// <summary>
        /// *.rpm の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Rpm
        {
            get { return _model.Settings.Rpm; }
            set { _model.Settings.Rpm = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Swf
        /// 
        /// <summary>
        /// *.swf の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Swf
        {
            get { return _model.Settings.Swf; }
            set { _model.Settings.Swf = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tar
        /// 
        /// <summary>
        /// *.tar の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Tar
        {
            get { return _model.Settings.Tar; }
            set { _model.Settings.Tar = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tbz
        /// 
        /// <summary>
        /// *.tbz の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Tbz
        {
            get { return _model.Settings.Tbz; }
            set { _model.Settings.Tbz = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tgz
        /// 
        /// <summary>
        /// *.tgz の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Tgz
        {
            get { return _model.Settings.Tgz; }
            set { _model.Settings.Tgz = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Txz
        /// 
        /// <summary>
        /// *.txz の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Txz
        {
            get { return _model.Settings.Txz; }
            set { _model.Settings.Txz = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Vhd
        /// 
        /// <summary>
        /// *.vhd の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Vhd
        {
            get { return _model.Settings.Vhd; }
            set { _model.Settings.Vhd = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Vmdk
        /// 
        /// <summary>
        /// *.vmdk の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Vmdk
        {
            get { return _model.Settings.Vmdk; }
            set { _model.Settings.Vmdk = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Wim
        /// 
        /// <summary>
        /// *.wim の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Wim
        {
            get { return _model.Settings.Wim; }
            set { _model.Settings.Wim = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Xar
        /// 
        /// <summary>
        /// *.xar の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Xar
        {
            get { return _model.Settings.Xar; }
            set { _model.Settings.Xar = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// XZ
        /// 
        /// <summary>
        /// *.xz の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool XZ
        {
            get { return _model.Settings.XZ; }
            set { _model.Settings.XZ = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Z
        /// 
        /// <summary>
        /// *.z の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Z
        {
            get { return _model.Settings.Z; }
            set { _model.Settings.Z = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Zip
        /// 
        /// <summary>
        /// *.zip の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Zip
        {
            get { return _model.Settings.Zip; }
            set { _model.Settings.Zip = value; }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        /// 
        /// <summary>
        /// 全ての項目を選択します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void SelectAll() => ApplyAll(true);

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        /// 
        /// <summary>
        /// 全ての項目の選択状態を解除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Clear() => ApplyAll(false);

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        /// 
        /// <summary>
        /// ファイルの関連付けを更新します。
        /// </summary>
        /// 
        /// <param name="force">強制モード</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Update(bool force) => _model.Execute(force);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ApplyAll
        /// 
        /// <summary>
        /// 全ての項目を有効または無効に設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void ApplyAll(bool enabled)
        {
            var items = _model.Settings.Value;
            foreach (var key in items.Keys.ToArray()) items[key] = enabled;

            var e = new PropertyChangedEventArgs(nameof(_model.Settings.Value));
            OnPropertyChanged(e);
        }

        #region Fields
        private AssociateCommand _model;
        #endregion

        #endregion
    }
}
