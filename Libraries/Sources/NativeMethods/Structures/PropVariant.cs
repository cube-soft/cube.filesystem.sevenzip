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
using System;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// PropVariant
    ///
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa380072.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Explicit)]
    internal struct PropVariant
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// VarType
        ///
        /// <summary>
        /// オブジェクトの種類を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public VarEnum VarType
        {
            get => (VarEnum)_vt;
            private set => _vt = (ushort)value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// VarType
        ///
        /// <summary>
        /// オブジェクトの内容を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object Object
        {
            get
            {
                switch (VarType)
                {
                    case VarEnum.VT_EMPTY:
                        return null;
                    case VarEnum.VT_FILETIME:
                        return DateTime.FromFileTime(_v64);
                    default:
                        var h = GCHandle.Alloc(this, GCHandleType.Pinned);
                        try { return Marshal.GetObjectForNativeVariant(h.AddrOfPinnedObject()); }
                        finally { h.Free(); }
                }
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// 各種フィールドをクリアします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Clear() => Ole32.NativeMethods.PropVariantClear(ref this);

        #region Set

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 真偽値を設定します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(bool value)
        {
            VarType = VarEnum.VT_BOOL;
            _v64u   = value ? 1UL : 0UL;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 整数値を設定します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(uint value)
        {
            VarType = VarEnum.VT_UI4;
            _v32u   = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 整数値を設定します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(ulong value)
        {
            VarType = VarEnum.VT_UI8;
            _v64u   = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// 文字列を設定します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(string value)
        {
            VarType = VarEnum.VT_BSTR;
            _vstr   = Marshal.StringToBSTR(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// DateTime オブジェクトを設定します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Set(DateTime value)
        {
            VarType = VarEnum.VT_FILETIME;
            _v64    = value.ToFileTime();
        }

        #endregion

        #region Create

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public static PropVariant Create(bool value)
        {
            var dest = new PropVariant();
            dest.Set(value);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public static PropVariant Create(uint value)
        {
            var dest = new PropVariant();
            dest.Set(value);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// オブジェクトを生成します。
        /// </summary>
        ///
        /// <param name="value">設定値</param>
        ///
        /* ----------------------------------------------------------------- */
        public static PropVariant Create(string value)
        {
            var dest = new PropVariant();
            dest.Set(value);
            return dest;
        }

        #endregion

        #endregion

        #region Fields
        [FieldOffset(0)] private ushort _vt;
        [FieldOffset(8)] private IntPtr _vstr;
        [FieldOffset(8)] private uint   _v32u;
        [FieldOffset(8)] private long   _v64;
        [FieldOffset(8)] private ulong  _v64u;
        [FieldOffset(8)] private PropArray _hack;
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PropArray
    ///
    /// <summary>
    /// x86/x64 のサイズの違いを埋める用途で使用するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal struct PropArray
    {
        uint _cElems;
        IntPtr _pElems;
    }
}
