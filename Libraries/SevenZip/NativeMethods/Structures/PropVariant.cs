/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
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
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct PropVariant
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// VarType
        ///
        /// <summary>
        /// vt フィールドを VarEnum に変換した結果を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public VarEnum VarType => (VarEnum)vt;

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
        public void Clear()
        {
            switch (VarType)
            {
                case VarEnum.VT_EMPTY:
                    break;
                case VarEnum.VT_NULL:
                case VarEnum.VT_I2:
                case VarEnum.VT_I4:
                case VarEnum.VT_R4:
                case VarEnum.VT_R8:
                case VarEnum.VT_CY:
                case VarEnum.VT_DATE:
                case VarEnum.VT_ERROR:
                case VarEnum.VT_BOOL:
                //case VarEnum.VT_DECIMAL:
                case VarEnum.VT_I1:
                case VarEnum.VT_UI1:
                case VarEnum.VT_UI2:
                case VarEnum.VT_UI4:
                case VarEnum.VT_I8:
                case VarEnum.VT_UI8:
                case VarEnum.VT_INT:
                case VarEnum.VT_UINT:
                case VarEnum.VT_HRESULT:
                case VarEnum.VT_FILETIME:
                    vt = 0;
                    break;
                default:
                    Ole32.NativeMethods.PropVariantClear(ref this);
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetObject
        ///
        /// <summary>
        /// オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public object GetObject()
        {
            switch (VarType)
            {
                case VarEnum.VT_EMPTY:    return null;
                case VarEnum.VT_FILETIME: return DateTime.FromFileTime(longValue);
                default: break;
            }

            var handle = GCHandle.Alloc(this, GCHandleType.Pinned);
            try { return Marshal.GetObjectForNativeVariant(handle.AddrOfPinnedObject()); }
            finally { handle.Free(); }
        }

        #endregion

        #region Fields

        [FieldOffset(0)]
        public ushort vt;

        [FieldOffset(8)]
        public IntPtr pointerValue;

        [FieldOffset(8)]
        public byte byteValue;

        [FieldOffset(8)]
        public long longValue;

        [FieldOffset(8)]
        public System.Runtime.InteropServices.ComTypes.FILETIME filetime;

        #endregion
    }
}
