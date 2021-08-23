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
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateViewModel
    ///
    /// <summary>
    /// Provides functionality to associate the AssociateValue object
    /// and a view.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class AssociateViewModel : Presentable<AssociateCommand>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the AssociateViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Settings for file association.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateViewModel(
            AssociateSetting src,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(new AssociateCommand(src), aggregator, context)
        {
            Facade.Settings.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Changed
        ///
        /// <summary>
        /// Gets or sets a value indicating whether any of associations are
        /// changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Changed
        {
            get => Facade.Settings.Changed;
            set => Facade.Settings.Changed = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IconIndex
        ///
        /// <summary>
        /// 関連付けされたファイルに表示するアイコンのインデックスを
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int IconIndex
        {
            get => Facade.Settings.IconIndex;
            set => Facade.Settings.IconIndex = value;
        }

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
            get => Facade.Settings.SevenZip;
            set => Facade.Settings.SevenZip = value;
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
            get => Facade.Settings.Arj;
            set => Facade.Settings.Arj = value;
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
            get => Facade.Settings.BZ2;
            set => Facade.Settings.BZ2 = value;
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
            get => Facade.Settings.Cab;
            set => Facade.Settings.Cab = value;
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
            get => Facade.Settings.Chm;
            set => Facade.Settings.Chm = value;
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
            get => Facade.Settings.Cpio;
            set => Facade.Settings.Cpio = value;
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
            get => Facade.Settings.Deb;
            set => Facade.Settings.Deb = value;
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
            get => Facade.Settings.Dmg;
            set => Facade.Settings.Dmg = value;
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
            get => Facade.Settings.Flv;
            set => Facade.Settings.Flv = value;
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
            get => Facade.Settings.GZ;
            set => Facade.Settings.GZ = value;
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
            get => Facade.Settings.Hfs;
            set => Facade.Settings.Hfs = value;
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
            get => Facade.Settings.Jar;
            set => Facade.Settings.Jar = value;
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
            get => Facade.Settings.Iso;
            set => Facade.Settings.Iso = value;
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
            get => Facade.Settings.Lzh;
            set => Facade.Settings.Lzh = value;
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
            get => Facade.Settings.Nupkg;
            set => Facade.Settings.Nupkg = value;
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
            get => Facade.Settings.Rar;
            set => Facade.Settings.Rar = value;
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
            get => Facade.Settings.Rpm;
            set => Facade.Settings.Rpm = value;
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
            get => Facade.Settings.Swf;
            set => Facade.Settings.Swf = value;
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
            get => Facade.Settings.Tar;
            set => Facade.Settings.Tar = value;
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
            get => Facade.Settings.Tbz;
            set => Facade.Settings.Tbz = value;
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
            get => Facade.Settings.Tgz;
            set => Facade.Settings.Tgz = value;
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
            get => Facade.Settings.Txz;
            set => Facade.Settings.Txz = value;
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
            get => Facade.Settings.Vhd;
            set => Facade.Settings.Vhd = value;
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
            get => Facade.Settings.Vmdk;
            set => Facade.Settings.Vmdk = value;
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
            get => Facade.Settings.Wim;
            set => Facade.Settings.Wim = value;
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
            get => Facade.Settings.Xar;
            set => Facade.Settings.Xar = value;
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
            get => Facade.Settings.XZ;
            set => Facade.Settings.XZ = value;
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
            get => Facade.Settings.Z;
            set => Facade.Settings.Z = value;
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
            get => Facade.Settings.Zip;
            set => Facade.Settings.Zip = value;
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
        /* ----------------------------------------------------------------- */
        public void Update() => Facade.Execute();

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
        /// ApplyAll
        ///
        /// <summary>
        /// 全ての項目を有効または無効に設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ApplyAll(bool enabled)
        {
            var items = Facade.Settings.Value;
            foreach (var key in items.Keys.ToArray()) items[key] = enabled;

            var e = new PropertyChangedEventArgs(nameof(Facade.Settings.Value));
            OnPropertyChanged(e);
        }

        #endregion
    }
}
