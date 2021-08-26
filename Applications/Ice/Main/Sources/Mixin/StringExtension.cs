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
using System.Text;
using Cube.Mixin.ByteFormat;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// StringExtension
    ///
    /// <summary>
    /// Provides extended methods of the string and related classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class StringExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// AppendLine
        ///
        /// <summary>
        /// Appends the display string according to the specified file
        /// information.
        /// </summary>
        ///
        /// <param name="src">Source builder object.</param>
        /// <param name="entity">File information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static StringBuilder AppendLine(this StringBuilder src, Entity entity) =>
            entity == null ?
            src.AppendLine(Properties.Resources.MessageUnknownFile) :
            src.AppendLine(entity.FullName)
               .AppendBytes(entity)
               .AppendLine()
               .AppendTime(entity)
               .AppendLine();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// AppendBytes
        ///
        /// <summary>
        /// Appends the string that represents the bytes of the specified
        /// file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static StringBuilder AppendBytes(this StringBuilder src, Entity entity) =>
            src.AppendFormat("{0} : {1}",
                Properties.Resources.MessageBytes,
                entity.Length.ToPrettyBytes()
            );

        /* ----------------------------------------------------------------- */
        ///
        /// AppendTime
        ///
        /// <summary>
        /// Appends the string that represents the last updated time of
        /// the specified file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static StringBuilder AppendTime(this StringBuilder src, Entity entity) =>
            src.AppendFormat("{0} : {1}",
                Properties.Resources.MessageLastWriteTime,
                entity.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss")
            );

        #endregion
    }
}
