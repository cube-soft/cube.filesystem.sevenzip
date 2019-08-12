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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateValue
    ///
    /// <summary>
    /// Represents the settings about the associated files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class AssociateValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateSettings
        ///
        /// <summary>
        /// Initializes a new instance of the AssociateSettings class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateValue() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets or sets the collection of associated files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IDictionary<string, bool> Value { get; private set; }

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
        public bool Changed { get; set; } = false;

        #region DataMember

        /* ----------------------------------------------------------------- */
        ///
        /// IconIndex
        ///
        /// <summary>
        /// Gets or sets the icon index of the associated files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int IconIndex
        {
            get => _iconIndex;
            set { if (SetProperty(ref _iconIndex, value)) Changed = true; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SevenZip
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.7z files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "7z")]
        public bool SevenZip
        {
            get => GetProperty("7z");
            set => SetProperty(value, "7z");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Arj
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.arj files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "arj")]
        public bool Arj
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BZ2
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.bz2 files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "bz2")]
        public bool BZ2
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cab
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.cab files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "cab")]
        public bool Cab
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Chm
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.chm files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "chm")]
        public bool Chm
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cpio
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.cpio files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "cpio")]
        public bool Cpio
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Deb
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.deb files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "deb")]
        public bool Deb
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dmg
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.dmg files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "dmg")]
        public bool Dmg
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Flv
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.flv files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "flv")]
        public bool Flv
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GZ
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.gz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "gz")]
        public bool GZ
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// hfs
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.hfs files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "hfs")]
        public bool Hfs
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Iso
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.iso files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "iso")]
        public bool Iso
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Jar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.jar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "jar")]
        public bool Jar
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Lzh
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.lzh files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "lzh")]
        public bool Lzh
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Nupkg
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.nupkg files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "nupkg")]
        public bool Nupkg
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.rar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "rar")]
        public bool Rar
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rpm
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.rpm files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "rpm")]
        public bool Rpm
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Swf
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.swf files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "swf")]
        public bool Swf
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.tar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "tar")]
        public bool Tar
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tbz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.tbz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "tbz")]
        public bool Tbz
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tgz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.tgz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "tgz")]
        public bool Tgz
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Txz
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.txz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "txz")]
        public bool Txz
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Vhd
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.vhd files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "vhd")]
        public bool Vhd
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Vmdk
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.vmdk files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "vmdk")]
        public bool Vmdk
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Wim
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.wim files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "wim")]
        public bool Wim
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Xar
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.xar files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "xar")]
        public bool Xar
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// XZ
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.xz files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "xz")]
        public bool XZ
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Z
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.z files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "z")]
        public bool Z
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Zip
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to associate with
        /// the *.zip files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "zip")]
        public bool Zip
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// Occurs before deserializing.
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
            _iconIndex = 3;

            Value = new Dictionary<string, bool>
            {
                { "7z",          true  },
                { nameof(Arj),   false },
                { nameof(BZ2),   true  },
                { nameof(Cab),   false },
                { nameof(Chm),   false },
                { nameof(Cpio),  false },
                { nameof(Deb),   false },
                { nameof(Dmg),   false },
                { nameof(Flv),   false },
                { nameof(GZ),    true  },
                { nameof(Hfs),   false },
                { nameof(Iso),   false },
                { nameof(Jar),   false },
                { nameof(Lzh),   true  },
                { nameof(Nupkg), false },
                { nameof(Rar),   true  },
                { nameof(Rpm),   false },
                { nameof(Swf),   false },
                { nameof(Tar),   true  },
                { nameof(Tbz),   true  },
                { nameof(Tgz),   true  },
                { nameof(Txz),   true  },
                { nameof(Vhd),   false },
                { nameof(Vmdk),  false },
                { nameof(Wim),   false },
                { nameof(Xar),   false },
                { nameof(XZ),    true  },
                { nameof(Z),     false },
                { nameof(Zip),   true  },
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// プロパティの値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool GetProperty([CallerMemberName] string name = null)
            => Value.TryGetValue(name, out bool dest) ? dest : false;

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// プロパティの値を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool SetProperty(bool value, [CallerMemberName] string name = null)
        {
            if (Value.ContainsKey(name))
            {
                if (Value[name] == value) return false;
                Value[name] = value;
            }
            else Value.Add(name, value);
            OnPropertyChanged(new PropertyChangedEventArgs(name));
            Changed = true;
            return true;
        }

        #endregion

        #region Fields
        private int _iconIndex;
        #endregion
    }
}
