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
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int CreateObjectDelegate(
        [In] ref Guid classID,
        [In] ref Guid interfaceID,
        [MarshalAs(UnmanagedType.Interface)] out object outObject // out IntPtr outObject
    );

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetHandlerPropertyDelegate(
        ArchivePropertyId propID,
        ref PropVariant value
    );

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetNumberOfFormatsDelegate(out uint numFormats);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetHandlerProperty2Delegate(
        uint formatIndex,
        ArchivePropertyId propID,
        ref PropVariant value
    );
}
