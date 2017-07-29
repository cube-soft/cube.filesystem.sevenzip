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
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice.Settings
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
    public class AssociateSettingsViewModel
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
            _model = model;
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
            get { return _model.SevenZip; }
            set { _model.SevenZip = value; }
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
            get { return _model.Arj; }
            set { _model.Arj = value; }
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
            get { return _model.BZ2; }
            set { _model.BZ2 = value; }
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
            get { return _model.Cab; }
            set { _model.Cab = value; }
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
            get { return _model.Chm; }
            set { _model.Chm = value; }
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
            get { return _model.Cpio; }
            set { _model.Cpio = value; }
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
            get { return _model.Deb; }
            set { _model.Deb = value; }
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
            get { return _model.Dmg; }
            set { _model.Dmg = value; }
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
            get { return _model.Flv; }
            set { _model.Flv = value; }
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
            get { return _model.GZ; }
            set { _model.GZ = value; }
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
            get { return _model.Jar; }
            set { _model.Jar = value; }
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
            get { return _model.Iso; }
            set { _model.Iso = value; }
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
            get { return _model.Lzh; }
            set { _model.Lzh = value; }
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
            get { return _model.Rar; }
            set { _model.Rar = value; }
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
            get { return _model.Rpm; }
            set { _model.Rpm = value; }
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
            get { return _model.Swf; }
            set { _model.Swf = value; }
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
            get { return _model.Tar; }
            set { _model.Tar = value; }
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
            get { return _model.Tbz; }
            set { _model.Tbz = value; }
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
            get { return _model.Tgz; }
            set { _model.Tgz = value; }
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
            get { return _model.Txz; }
            set { _model.Txz = value; }
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
            get { return _model.Vhd; }
            set { _model.Vhd = value; }
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
            get { return _model.Vmdk; }
            set { _model.Vmdk = value; }
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
            get { return _model.Wim; }
            set { _model.Wim = value; }
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
            get { return _model.Xar; }
            set { _model.Xar = value; }
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
            get { return _model.XZ; }
            set { _model.XZ = value; }
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
            get { return _model.Z; }
            set { _model.Z = value; }
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
            get { return _model.Zip; }
            set { _model.Zip = value; }
        }

        #endregion

        #region Fields
        private AssociateSettings _model;
        #endregion
    }
}
