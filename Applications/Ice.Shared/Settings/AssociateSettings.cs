/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Cube.FileSystem.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateSettings
    /// 
    /// <summary>
    /// ファイルの関連付けに関するユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class AssociateSettings : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateSettings
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public AssociateSettings()
        {
            Value = new Dictionary<string, bool>
            {
                { "7z",         true  },
                { nameof(Arj),  false },
                { nameof(BZ2),  true  },
                { nameof(Cab),  false },
                { nameof(Chm),  false },
                { nameof(Cpio), false },
                { nameof(Deb),  false },
                { nameof(Dmg),  false },
                { nameof(Flv),  false },
                { nameof(GZ),   true  },
                { nameof(Hfs),  false },
                { nameof(Iso),  false },
                { nameof(Jar),  false },
                { nameof(Lzh),  true  },
                { nameof(Rar),  true  },
                { nameof(Rpm),  false },
                { nameof(Swf),  false },
                { nameof(Tar),  true  },
                { nameof(Tbz),  true  },
                { nameof(Tgz),  true  },
                { nameof(Txz),  true  },
                { nameof(Vhd),  false },
                { nameof(Vmdk), false },
                { nameof(Wim),  false },
                { nameof(Xar),  false },
                { nameof(XZ),   true  },
                { nameof(Z),    false },
                { nameof(Zip),  true  },
            };
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        /// 
        /// <summary>
        /// ファイルの関連付け一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IDictionary<string, bool> Value { get; }

        #region DataMember

        /* ----------------------------------------------------------------- */
        ///
        /// SevenZip
        /// 
        /// <summary>
        /// *.7z の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "7z")]
        public bool SevenZip
        {
            get { return GetProperty("7z"); }
            set { SetProperty(value, "7z"); }
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
        [DataMember(Name = "arj")]
        public bool Arj
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "bz2")]
        public bool BZ2
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "cab")]
        public bool Cab
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "chm")]
        public bool Chm
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "cpio")]
        public bool Cpio
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "deb")]
        public bool Deb
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "dmg")]
        public bool Dmg
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "flv")]
        public bool Flv
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "gz")]
        public bool GZ
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// hfs
        /// 
        /// <summary>
        /// *.hfs の関連付け状態を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "hfs")]
        public bool Hfs
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "iso")]
        public bool Iso
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "jar")]
        public bool Jar
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "lzh")]
        public bool Lzh
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "rar")]
        public bool Rar
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "rpm")]
        public bool Rpm
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "swf")]
        public bool Swf
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "tar")]
        public bool Tar
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "tbz")]
        public bool Tbz
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "tgz")]
        public bool Tgz
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "txz")]
        public bool Txz
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "vhd")]
        public bool Vhd
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "vmdk")]
        public bool Vmdk
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "wim")]
        public bool Wim
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "xar")]
        public bool Xar
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "xz")]
        public bool XZ
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "z")]
        public bool Z
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
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
        [DataMember(Name = "zip")]
        public bool Zip
        {
            get { return GetProperty(); }
            set { SetProperty(value); }
        }

        #endregion

        #endregion

        #region Implementations

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
            => Value.ContainsKey(name) ? Value[name] : false;

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
            return true;
        }

        #endregion
    }
}
