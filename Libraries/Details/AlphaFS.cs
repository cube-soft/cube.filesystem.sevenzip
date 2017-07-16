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
using Alphaleonis.Win32.Filesystem;

namespace Cube.FileSystem.Details
{
    /* --------------------------------------------------------------------- */
    ///
    /// AlphaFS
    /// 
    /// <summary>
    /// AlphaFS を利用した IFileOperator の実装クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class AlphaFS : IFileOperator
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// ファイルまたはディレクトリが存在するかどうかを判別します。
        /// </summary>
        /// 
        /// <param name="src">判別対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Exists(string src)
            => !string.IsNullOrEmpty(src) &&
               (File.Exists(src) || Directory.Exists(src));

        /* ----------------------------------------------------------------- */
        ///
        /// IsDirectory
        ///
        /// <summary>
        /// ディレクトリかどうか判別します。
        /// </summary>
        /// 
        /// <param name="src">判別対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public bool IsDirectory(string src)
        {
            if (string.IsNullOrEmpty(src)) return false;
            var attr = File.GetAttributes(src);
            var flag = attr & System.IO.FileAttributes.Directory;
            return flag == System.IO.FileAttributes.Directory;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを作成します。
        /// </summary>
        /// 
        /// <param name="src">ディレクトリのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void CreateDirectory(string src) => Directory.CreateDirectory(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// ファイルまたはディレクトリを削除します。
        /// </summary>
        /// 
        /// <param name="src">削除対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Delete(string src)
        {
            if (IsDirectory(src)) Directory.Delete(src, true);
            else File.Delete(src);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// ファイルを移動します。
        /// </summary>
        /// 
        /// <param name="src">移動前のパス</param>
        /// <param name="dest">移動後のパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Move(string src, string dest) => File.Move(src, dest);

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// ファイルをコピーします。
        /// </summary>
        /// 
        /// <param name="src">コピー元のパス</param>
        /// <param name="dest">コピー先のパス</param>
        /// <param name="overwrite">上書きするかどうかを示す値</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Copy(string src, string dest, bool overwrite)
            => File.Copy(src, dest, overwrite);
    }
}
