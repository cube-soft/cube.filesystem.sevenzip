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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// Device
    /// 
    /// <summary>
    /// デバイスに関する情報を保持するクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class Device
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Device
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Device(Drive drive)
        {
            Drive = drive;
            InitializeProperties();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Drive
        /// 
        /// <summary>
        /// 対象となるドライブを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Drive Drive { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        /// 
        /// <summary>
        /// デバイスのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        /// 
        /// <summary>
        /// デバイス番号を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Index { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Handle
        /// 
        /// <summary>
        /// ハンドル (DeviceInstance) を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Handle { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        /// 
        /// <summary>
        /// デバイスを取り外します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: たまに（成功するはずなのに）失敗するので何度か試行すべき
        /// と言う指摘があったので、その辺りの調査を行う。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Detach()
        {
            switch (Drive.Type)
            {
                case DriveType.CD:
                case DriveType.Dvd:
                    DetachMedia();
                    break;
                case DriveType.FloppyDisk:
                    break;
                case DriveType.HardDisk:
                case DriveType.RemovableDisk:
                    DetachDevice();
                    break;
                case DriveType.Network:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// DetachDevice
        /// 
        /// <summary>
        /// デバイスを取り外します。
        /// </summary>
        /// 
        /// <remarks>
        /// TODO: たまに（成功するはずなのに）失敗するので何度か試行すべき
        /// と言う指摘があったので、その辺りの調査を行う。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void DetachDevice()
        {
            var parent = 0;
            SetupApi.NativeMethods.CM_Get_Parent(ref parent, (int)Handle, 0);

            var veto = VetoType.Unknown;
            var name = new StringBuilder(10 * 1024);
            var status = SetupApi.NativeMethods.CM_Request_Device_Eject(parent,
                out veto, name, (uint)name.Capacity, 0);
            if (status != 0) throw new VetoException(veto, name.ToString());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DetachMedia
        /// 
        /// <summary>
        /// メディアを取り出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DetachMedia()
            => WinMM.NativeMethods.mciSendString("set CDAudio door open", null, 0, IntPtr.Zero);

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeProperties
        /// 
        /// <summary>
        /// 各種プロパティを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeProperties()
        {
            var handle = IntPtr.Zero;

            try
            {
                var guid = GetClassGuid(Drive);
                if (guid == Guid.Empty) return;

                handle = GetDeviceHandle(guid);
                for (uint i = 0; true; ++i)
                {
                    var data = new SP_DEVICE_INTERFACE_DATA();
                    if (!SetupApi.NativeMethods.SetupDiEnumDeviceInterfaces(handle, IntPtr.Zero, ref guid, i, data))
                    {
                        var errno = Marshal.GetLastWin32Error();
                        if (errno == 259 /* ERROR_NO_MORE_ITEMS */) break;
                        throw new Win32Exception(errno, "SetupDiEnumDeviceInterfaces");
                    }

                    var devinfo = new SP_DEVINFO_DATA();
                    var path = GetDevicePath(handle, data, devinfo);
                    var index = DeviceNumber.Get(path);
                    if (Drive.Index == index)
                    {
                        Path = path;
                        Index = index;
                        Handle = devinfo.devInst;
                        break;
                    }
                }
            }
            finally { if (handle != IntPtr.Zero) SetupApi.NativeMethods.SetupDiDestroyDeviceInfoList(handle); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetClassGuid
        /// 
        /// <summary>
        /// 利用クラスに対応する GUID オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Guid GetClassGuid(Drive drive)
        {
            const string GUID_DEVINTERFACE_DISK   = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";
            const string GUID_DEVINTERFACE_FLOPPY = "53f56311-b6bf-11d0-94f2-00a0c91efb8b";
            const string GUID_DEVINTERFACE_CDROM  = "53f56308-b6bf-11d0-94f2-00a0c91efb8b";

            switch (drive.Type)
            {
                case DriveType.CD:
                case DriveType.Dvd:
                    return new Guid(GUID_DEVINTERFACE_CDROM);
                case DriveType.FloppyDisk:
                    return new Guid(GUID_DEVINTERFACE_FLOPPY);
                case DriveType.HardDisk:
                case DriveType.RemovableDisk:
                    return new Guid(GUID_DEVINTERFACE_DISK);
                case DriveType.Network:
                    return Guid.Empty;
                default:
                    return Guid.Empty;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDeviceHandle
        /// 
        /// <summary>
        /// デバイス情報にアクセスするためのハンドルを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IntPtr GetDeviceHandle(Guid guid)
        {
            const uint DIGCF_PRESENT = 0x00000002;
            const uint DIGCF_DEVICEINTERFACE = 0x00000010;

            var dest  = SetupApi.NativeMethods.SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (dest == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error(), "SetupDiGetClassDevs");
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDevicePath
        /// 
        /// <summary>
        /// デバイスのパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string GetDevicePath(IntPtr handle, SP_DEVICE_INTERFACE_DATA data, SP_DEVINFO_DATA devinfo)
        {
            var buffer = IntPtr.Zero;

            try
            {
                var size = GetRequiredSize(handle, data, devinfo);
                var detail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                if (IntPtr.Size == 8) detail.cbSize = 8; // x64
                else detail.cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
                buffer = Marshal.AllocHGlobal((int)size);
                Marshal.StructureToPtr(detail, buffer, false);

                var status = SetupApi.NativeMethods.SetupDiGetDeviceInterfaceDetail(handle, data, buffer, size, ref size, devinfo);
                if (!status) throw new Win32Exception(Marshal.GetLastWin32Error(), "SetupDiGetDeviceInterfaceDetail");

                var pos = new IntPtr(buffer.ToInt64() + Marshal.SizeOf(typeof(uint)));
                return Marshal.PtrToStringAuto(pos);
            }
            finally { if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRequiredSize
        /// 
        /// <summary>
        /// SetupDiGetDeviceInterfaceDetail を実行するために必要なバッファ
        /// サイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private uint GetRequiredSize(IntPtr handle, SP_DEVICE_INTERFACE_DATA data, SP_DEVINFO_DATA devinfo)
        {
            var dest = 0u;
            if (!SetupApi.NativeMethods.SetupDiGetDeviceInterfaceDetail(handle, data, IntPtr.Zero, 0, ref dest, devinfo))
            {
                var errno = Marshal.GetLastWin32Error();
                if (errno != 122 /* ERROR_INSUFFICIENT_BUFFER */)
                {
                    throw new Win32Exception(errno, "SetupDiGetDeviceInterfaceDetail");
                }
            }
            return dest;
        }

        #endregion
    }
}
