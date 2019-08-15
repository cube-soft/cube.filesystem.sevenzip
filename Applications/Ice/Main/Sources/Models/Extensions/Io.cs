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
using Cube.Mixin.ByteFormat;
using Cube.Mixin.IO;
using System.Linq;
using System.Text;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// IoExtension
    ///
    /// <summary>
    /// Provides extended methods of the IO and Entity classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class IoExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the specified file or directory according to the
        /// specified method.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="src">File or directory information.</param>
        /// <param name="dest">File or directory information.</param>
        /// <param name="method">Overwrite method.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Move(this IO io, Entity src, Entity dest, OverwriteMethod method)
        {
            switch (method & OverwriteMethod.Operations)
            {
                case OverwriteMethod.Yes:
                    io.Move(src, dest);
                    break;
                case OverwriteMethod.Rename:
                    io.Move(src, io.Get(io.GetUniqueName(dest.FullName)));
                    break;
                case OverwriteMethod.No:
                case OverwriteMethod.Cancel:
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the specified file or directory.
        /// </summary>
        ///
        /// <param name="io">I/O handler.</param>
        /// <param name="src">File or directory information.</param>
        /// <param name="dest">File or directory information.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Move(this IO io, Entity src, Entity dest)
        {
            if (src.IsDirectory)
            {
                if (!dest.Exists)
                {
                    io.CreateDirectory(dest.FullName);
                    io.SetAttributes(dest.FullName, src.Attributes);
                    io.SetCreationTime(dest.FullName, src.CreationTime);
                    io.SetLastWriteTime(dest.FullName, src.LastWriteTime);
                    io.SetLastAccessTime(dest.FullName, src.LastAccessTime);
                }
            }
            else io.Move(src.FullName, dest.FullName, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetBaseName
        ///
        /// <summary>
        /// Gets the base-name from the specified arguments.
        /// </summary>
        ///
        /// <param name="src">File information.</param>
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetBaseName(this Entity src, Format format) =>
            new[] { Format.BZip2, Format.GZip, Format.XZ }.Contains(format) ?
            TrimExtension(src.BaseName) :
            src.BaseName;

        /* ----------------------------------------------------------------- */
        ///
        /// AppendLine
        ///
        /// <summary>
        /// Appends the string of the specified file information.
        /// </summary>
        ///
        /// <param name="src">String builder.</param>
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
        /// TrimExtension
        ///
        /// <summary>
        /// Trims the extension of the specified filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string TrimExtension(string src)
        {
            var index = src.LastIndexOf('.');
            return index < 0 ? src : src.Substring(0, index);
        }

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
