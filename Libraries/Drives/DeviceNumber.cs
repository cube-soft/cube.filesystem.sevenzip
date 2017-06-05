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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// DeviceNumber
    /// 
    /// <summary>
    /// デバイス番号を取得するためのクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// Cube.FileSystem.Drive オブジェクトの Index プロパティに相当する
    /// 値を取得します。デバイス名のみ把握している時などに使用します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal static class DeviceNumber
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        /// 
        /// <summary>
        /// 指定された名前に対応するデバイス番号を取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// デバイス名には \\.\DeviceName と言う書式を使用します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static uint Get(string deviceName)
            => GetDeviceNumber(deviceName);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetDeviceNumber
        /// 
        /// <summary>
        /// 指定された名前に対応するデバイス番号を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static uint GetDeviceNumber(string deviceName)
        {
            const uint FILE_SHARE_READ  = 0x00000001;
            const uint FILE_SHARE_WRITE = 0x00000002;

            var handle = IntPtr.Zero;

            try
            {
                handle = Kernel32.NativeMethods.CreateFile(deviceName, 0, FILE_SHARE_READ | FILE_SHARE_WRITE,
                    IntPtr.Zero, 3 /* OPEN_EXISTING */, 0, IntPtr.Zero);
                if (handle.ToInt64() <= 0) throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateFile");
                return GetDeviceNumber(handle);
                
            }
            finally { if (handle.ToInt64() > 0) Kernel32.NativeMethods.CloseHandle(handle); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDeviceNumber
        /// 
        /// <summary>
        /// 指定されたハンドルに対応するデバイス番号を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static uint GetDeviceNumber(IntPtr handle)
        {
            var buffer = IntPtr.Zero;

            try
            {
                var capacity = Marshal.SizeOf(typeof(STORAGE_DEVICE_NUMBER));
                var size = 0u;
                buffer = Marshal.AllocHGlobal(capacity);

                var result = Kernel32.NativeMethods.DeviceIoControl(handle,
                    0x002D1080 /* IOCTL_STORAGE_GET_DEVICE_NUMBER */,
                    IntPtr.Zero, 0, buffer, (uint)capacity, out size, IntPtr.Zero);
                if (!result || size == 0) throw new Win32Exception(Marshal.GetLastWin32Error(), "DeviceIoControl");

                var dest = (STORAGE_DEVICE_NUMBER)Marshal.PtrToStructure(buffer, typeof(STORAGE_DEVICE_NUMBER));
                return dest.DeviceNumber;
            }
            finally { if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer); }
        }

        #endregion
    }
}
