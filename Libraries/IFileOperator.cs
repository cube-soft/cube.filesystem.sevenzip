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
namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// IFileOperator
    /// 
    /// <summary>
    /// ファイルまたはディレクトリに対する操作を定義したインターフェース
    /// です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IFileOperator
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
        bool Exists(string src);

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
        bool IsDirectory(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// ファイルまたはディレクトリを移動します。
        /// </summary>
        /// 
        /// <param name="src">移動前のパス</param>
        /// <param name="dest">移動後のパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        void Move(string src, string dest);

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// ファイルまたはディレクトリをコピーします。
        /// </summary>
        /// 
        /// <param name="src">コピー元のパス</param>
        /// <param name="dest">コピー先のパス</param>
        /// <param name="overwrite">上書きするかどうかを示す値</param>
        /// 
        /* ----------------------------------------------------------------- */
        void Copy(string src, string dest, bool overwrite);

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
        void Delete(string src);
    }
}
