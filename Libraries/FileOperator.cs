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
using System.IO;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// IFileOperator
    /// 
    /// <summary>
    /// ファイルまたはディレクトリに対する各種操作を定義した
    /// インターフェースです。
    /// </summary>
    /// 
    /// <remarks>
    /// IFileOperator および各種実装クラスは、主に FileHandler の操作で
    /// 使用するライブラリを動的に変更するために使用されます。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public interface IFileOperator
    {
        #region File or Directory

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

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// ファイルを新規作成します。
        /// </summary>
        /// 
        /// <param name="src">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        FileStream Create(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenRead
        ///
        /// <summary>
        /// ファイルを読み込み専用で開きます。
        /// </summary>
        /// 
        /// <param name="src">ファイルのパス</param>
        /// 
        /// <returns>読み込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        FileStream OpenRead(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenRead
        ///
        /// <summary>
        /// ファイルを新規作成、または上書き用で開きます。
        /// </summary>
        /// 
        /// <param name="src">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        FileStream OpenWrite(string src);

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
        void CreateDirectory(string src);

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

        #endregion

        #region Path

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        string GetFileName(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileNameWithoutExtension
        ///
        /// <summary>
        /// 拡張子なしのファイル名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        string GetFileNameWithoutExtension(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        ///
        /// <summary>
        /// 拡張子を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        string GetExtension(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        string GetDirectoryName(string src);

        /* ----------------------------------------------------------------- */
        ///
        /// Combine
        ///
        /// <summary>
        /// パスを結合します。
        /// </summary>
        /// 
        /// <param name="directory">ディレクトリを示すパス</param>
        /// <param name="filename">ファイル名</param>
        /// 
        /* ----------------------------------------------------------------- */
        string Combine(params string[] paths);

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FileOperator
    /// 
    /// <summary>
    /// 標準ライブラリを利用した IFileOperator の実装クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class FileOperator : IFileOperator
    {
        #region File or Directory

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
            var flag = attr & FileAttributes.Directory;
            return flag == FileAttributes.Directory;
        }

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
        /// Create
        ///
        /// <summary>
        /// ファイルを新規作成します。
        /// </summary>
        /// 
        /// <param name="src">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public FileStream Create(string src) => File.Create(src);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenRead
        ///
        /// <summary>
        /// ファイルを読み込み専用で開きます。
        /// </summary>
        /// 
        /// <param name="src">ファイルのパス</param>
        /// 
        /// <returns>読み込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public FileStream OpenRead(string src) => File.OpenRead(src);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenRead
        ///
        /// <summary>
        /// ファイルを新規作成、または上書き用で開きます。
        /// </summary>
        /// 
        /// <param name="src">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public FileStream OpenWrite(string src) => File.OpenWrite(src);

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

        #endregion

        #region Path

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string GetFileName(string src) => Path.GetFileName(src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileNameWithoutExtension
        ///
        /// <summary>
        /// 拡張子なしのファイル名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string GetFileNameWithoutExtension(string src)
            => Path.GetFileNameWithoutExtension(src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        ///
        /// <summary>
        /// 拡張子を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string GetExtension(string src) => Path.GetExtension(src);

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectoryName
        ///
        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        /// 
        /// <param name="src">パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string GetDirectoryName(string src) => Path.GetDirectoryName(src);

        /* ----------------------------------------------------------------- */
        ///
        /// Combine
        ///
        /// <summary>
        /// パスを結合します。
        /// </summary>
        /// 
        /// <param name="directory">ディレクトリを示すパス</param>
        /// <param name="filename">ファイル名</param>
        /// 
        /* ----------------------------------------------------------------- */
        public string Combine(params string[] paths) => Path.Combine(paths);

        #endregion
    }
}
