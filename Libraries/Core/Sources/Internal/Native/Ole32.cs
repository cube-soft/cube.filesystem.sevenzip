/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip.Ole32
{
    /* --------------------------------------------------------------------- */
    ///
    /// Ole32.NativeMethods
    ///
    /// <summary>
    /// Provides native methods defined in the ole32.dll.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class NativeMethods
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// PropVariantClear
        ///
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa380073.aspx
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DllImport(LibName)]
        public static extern int PropVariantClear(ref PropVariant pvar);

        #endregion

        #region Fields
        private const string LibName = "ole32.dll";
        #endregion
    }
}
