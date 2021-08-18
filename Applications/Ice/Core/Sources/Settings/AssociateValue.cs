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
        public IDictionary<string, bool> Value => Get(() => new Dictionary<string, bool>
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
        });

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
            get => Get(() => false);
            set => Set(value);
        }

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
            get => Get(() => 3);
            set { if (Set(value)) Changed = true; }
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
            get => GetAssociateValue("7z");
            set => SetAssociateValue(value, "7z");
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
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
            get => GetAssociateValue();
            set => SetAssociateValue(value);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetAssociateValue
        ///
        /// <summary>
        /// Gets the value associated with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool GetAssociateValue([CallerMemberName] string name = null) =>
            Value.TryGetValue(name, out var dest) ? dest : false;

        /* ----------------------------------------------------------------- */
        ///
        /// SetAssociateValue
        ///
        /// <summary>
        /// Sets the value associated with the specified name.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool SetAssociateValue(bool value, [CallerMemberName] string name = null)
        {
            if (Value.ContainsKey(name))
            {
                if (Value[name] == value) return false;
                Value[name] = value;
            }
            else Value.Add(name, value);
            Refresh(name);
            Changed = true;
            return true;
        }

        #endregion
    }
}
