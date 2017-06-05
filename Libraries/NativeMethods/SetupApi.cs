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
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SetupApi
{
    /* --------------------------------------------------------------------- */
    ///
    /// SetupApi.NativeMethods
    /// 
    /// <summary>
    /// setupapi.dll に定義された関数を宣言するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiGetClassDevs
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff551069.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
            ref Guid classGuid,
            IntPtr enumerator,
            IntPtr hwndParent,
            uint flags
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiDestroyDeviceInfoList
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff550996.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern uint SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiEnumDeviceInterfaces
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff550996.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr deviceInfoSet,
            IntPtr deviceInfoData,
            ref Guid interfaceClassGuid,
            uint memberIndex,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData
        );

        /* ----------------------------------------------------------------- */
        ///
        /// SetupDiGetDeviceInterfaceDetail
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff551120.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr deviceInfoSet,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            ref uint requiredSize,
            SP_DEVINFO_DATA deviceInfoData
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CM_Get_Parent
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff538610.aspx
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern int CM_Get_Parent(
            ref int pdnDevInst,
            int dnDevInst,
            uint ulFlags
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CM_Request_Device_Eject
        /// 
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff539806.aspx
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DllImport(LibName, CharSet = CharSet.Unicode)]
        public static extern int CM_Request_Device_Eject(
            int dnDevInst,
            out VetoType pVetoType,
            StringBuilder pszVetoName,
            uint ulNameLength,
            uint ulFlags
        );

        #region Fields
        const string LibName = "setupapi.dll";
        #endregion
    }
}
