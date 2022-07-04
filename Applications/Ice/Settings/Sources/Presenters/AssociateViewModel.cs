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
using System.Linq;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateViewModel
    ///
    /// <summary>
    /// Provides functionality to bind values to view components for the
    /// associate settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class AssociateViewModel : PresentableBase<AssociateSettingValue>
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
        /// <param name="src">Settings for the file association.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateViewModel(AssociateSettingValue src, Aggregator aggregator, SynchronizationContext context) :
            base(src, aggregator, context)
        {
            Assets.Add(new ObservableProxy(Facade, this));
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
            get => Facade.Changed;
            set => Facade.Changed = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IconIndex
        ///
        /// <summary>
        /// Gets or sets the index of the icon to be displayed for the
        /// associated file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int IconIndex
        {
            get => Facade.IconIndex;
            set => Facade.IconIndex = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SevenZip
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.7z files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SevenZip
        {
            get => Facade.SevenZip;
            set => Facade.SevenZip = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Arj
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.arj files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Arj
        {
            get => Facade.Arj;
            set => Facade.Arj = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bz2
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.bz2 files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Bz2
        {
            get => Facade.Bz2;
            set => Facade.Bz2 = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cab
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.cab files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Cab
        {
            get => Facade.Cab;
            set => Facade.Cab = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Chm
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.chm files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Chm
        {
            get => Facade.Chm;
            set => Facade.Chm = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cpio
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.cpio files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Cpio
        {
            get => Facade.Cpio;
            set => Facade.Cpio = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Deb
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.deb files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Deb
        {
            get => Facade.Deb;
            set => Facade.Deb = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dmg
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.dmg files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Dmg
        {
            get => Facade.Dmg;
            set => Facade.Dmg = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Flv
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.flv files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Flv
        {
            get => Facade.Flv;
            set => Facade.Flv = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Gz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.gz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Gz
        {
            get => Facade.Gz;
            set => Facade.Gz = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Hfs
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.hfs files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Hfs
        {
            get => Facade.Hfs;
            set => Facade.Hfs = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Jar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.jar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Jar
        {
            get => Facade.Jar;
            set => Facade.Jar = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Iso
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.iso files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Iso
        {
            get => Facade.Iso;
            set => Facade.Iso = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Lzh
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.lzh files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Lzh
        {
            get => Facade.Lzh;
            set => Facade.Lzh = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Nupkg
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.nupkg files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Nupkg
        {
            get => Facade.Nupkg;
            set => Facade.Nupkg = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.rar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Rar
        {
            get => Facade.Rar;
            set => Facade.Rar = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rpm
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.rpm files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Rpm
        {
            get => Facade.Rpm;
            set => Facade.Rpm = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Swf
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.swf files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Swf
        {
            get => Facade.Swf;
            set => Facade.Swf = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.tar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Tar
        {
            get => Facade.Tar;
            set => Facade.Tar = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tbz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.tbz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Tbz
        {
            get => Facade.Tbz;
            set => Facade.Tbz = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tgz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.tgz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Tgz
        {
            get => Facade.Tgz;
            set => Facade.Tgz = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Txz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.txz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Txz
        {
            get => Facade.Txz;
            set => Facade.Txz = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Vhd
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.vhd files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Vhd
        {
            get => Facade.Vhd;
            set => Facade.Vhd = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Vmdk
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.vmdk files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Vmdk
        {
            get => Facade.Vmdk;
            set => Facade.Vmdk = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Wim
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.wim files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Wim
        {
            get => Facade.Wim;
            set => Facade.Wim = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Xar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.xar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Xar
        {
            get => Facade.Xar;
            set => Facade.Xar = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Xz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.xz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Xz
        {
            get => Facade.Xz;
            set => Facade.Xz = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Z
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.z files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Z
        {
            get => Facade.Z;
            set => Facade.Z = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Zip
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to associate
        /// with the *.zip files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Zip
        {
            get => Facade.Zip;
            set => Facade.Zip = value;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SelectIcon
        ///
        /// <summary>
        /// Selects the icon for file association.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectIcon() =>
            Send(new AssociateIconMessage(Facade.IconIndex), e => Facade.IconIndex = e, true);

        /* ----------------------------------------------------------------- */
        ///
        /// SelectAll
        ///
        /// <summary>
        /// Selects all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SelectAll() => SetAll(true);

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Cancels the selection of all items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => SetAll(false);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the settings for the file association.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save() => AssociateAction.Invoke(Facade);

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
        /// SetAll
        ///
        /// <summary>
        /// Set all items to enabled or disabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetAll(bool enabled)
        {
            var items = Facade.Value;
            foreach (var key in items.Keys.ToArray()) items[key] = enabled;
            Refresh(nameof(Changed));
        }

        #endregion
    }
}
