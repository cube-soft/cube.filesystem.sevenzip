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
using System.Runtime.InteropServices;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// SP_DEVINFO_DATA
    /// 
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff552344.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal class SP_DEVINFO_DATA
    {
        public uint cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
        public Guid classGuid = Guid.Empty;
        public uint devInst = 0;
        public IntPtr reserved = IntPtr.Zero;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SP_DEVICE_INTERFACE_DATA
    /// 
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff552342.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal class SP_DEVICE_INTERFACE_DATA
    {
        public uint cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
        public Guid interfaceClassGuid = Guid.Empty;
        public uint flags = 0;
        public IntPtr reserved = IntPtr.Zero;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SP_DEVICE_INTERFACE_DETAIL_DATA
    /// 
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/hardware/ff552343.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public uint cbSize;
        public short devicePath;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// STORAGE_DEVICE_NUMBER
    /// 
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb968801.aspx
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [StructLayout(LayoutKind.Sequential)]
    internal struct STORAGE_DEVICE_NUMBER
    {
        public uint DeviceType;
        public uint DeviceNumber;
        public uint PartitionNumber;
    };
}
