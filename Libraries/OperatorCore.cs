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
using System.IO;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// IOperatorCore
    /// 
    /// <summary>
    /// ファイルまたはディレクトリに対する各種操作を定義した
    /// インターフェースです。
    /// </summary>
    /// 
    /// <remarks>
    /// IOperatorCore および実装クラスは、主に Operator の操作で使用する
    /// ライブラリを動的に変更するために使用されます。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public interface IOperatorCore
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// ファイルまたはディレクトリの情報を保持するオブジェクトを
        /// 取得します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// 
        /// <returns>IInformation オブジェクト</returns>
        /// 
        /* ----------------------------------------------------------------- */
        IInformation Get(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// GetFiles
        ///
        /// <summary>
        /// ディレクトリ下にあるファイルの一覧を取得します。
        /// </summary>
        /// 
        /// <param name="path">ディレクトリのパス</param>
        /// 
        /// <returns>ファイル一覧</returns>
        /// 
        /* ----------------------------------------------------------------- */
        string[] GetFiles(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectories
        ///
        /// <summary>
        /// ディレクトリ下にあるディレクトリの一覧を取得します。
        /// </summary>
        /// 
        /// <param name="path">ディレクトリのパス</param>
        /// 
        /// <returns>ディレクトリ一覧</returns>
        /// 
        /* ----------------------------------------------------------------- */
        string[] GetDirectories(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// ファイルまたはディレクトリに属性を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="attr">属性</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetAttributes(string path, FileAttributes attr);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCreationTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリに作成日時を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="time">作成日時</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetCreationTime(string path, DateTime time);

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastWriteTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリに最終更新日時を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="time">最終更新日時</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetLastWriteTime(string path, DateTime time);

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastAccessTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリに最終アクセス日時を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="time">最終アクセス日時</param>
        /// 
        /* ----------------------------------------------------------------- */
        void SetLastAccessTime(string path, DateTime time);

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

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// ファイルまたはディレクトリが存在するかどうかを判別します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        bool Exists(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// ファイルまたはディレクトリを削除します。
        /// </summary>
        /// 
        /// <param name="path">削除対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        void Delete(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// ファイルを新規作成します。
        /// </summary>
        /// 
        /// <param name="path">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        FileStream Create(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenRead
        ///
        /// <summary>
        /// ファイルを読み込み専用で開きます。
        /// </summary>
        /// 
        /// <param name="path">ファイルのパス</param>
        /// 
        /// <returns>読み込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        FileStream OpenRead(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWrite
        ///
        /// <summary>
        /// ファイルを新規作成、または上書き用で開きます。
        /// </summary>
        /// 
        /// <param name="path">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        FileStream OpenWrite(string path);

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを作成します。
        /// </summary>
        /// 
        /// <param name="path">ディレクトリのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        void CreateDirectory(string path);

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
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OperatorCore
    /// 
    /// <summary>
    /// 標準ライブラリを利用した IOperatorCore の実装クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class OperatorCore : IOperatorCore
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// ファイルまたはディレクトリの情報を保持するオブジェクトを
        /// 取得します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// 
        /// <returns>IInformation オブジェクト</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public IInformation Get(string path) => new Information(path);

        /* ----------------------------------------------------------------- */
        ///
        /// GetFiles
        ///
        /// <summary>
        /// ディレクトリ下にあるファイルの一覧を取得します。
        /// </summary>
        /// 
        /// <param name="path">パス</param>
        /// 
        /// <returns>ファイル一覧</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public string[] GetFiles(string path)
            => Directory.Exists(path) ? Directory.GetFiles(path) : null;

        /* ----------------------------------------------------------------- */
        ///
        /// GetDirectories
        ///
        /// <summary>
        /// ディレクトリ下にあるディレクトリの一覧を取得します。
        /// </summary>
        /// 
        /// <param name="path">パス</param>
        /// 
        /// <returns>ディレクトリ一覧</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public string[] GetDirectories(string path)
            => Directory.Exists(path) ? Directory.GetDirectories(path) : null;

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// ファイルまたはディレクトリに属性を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="attr">属性</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetAttributes(string path, FileAttributes attr)
        {
            if (File.Exists(path)) File.SetAttributes(path, attr);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCreationTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリに作成日時を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="time">作成日時</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCreationTime(string path, DateTime time)
        {
            if (Directory.Exists(path)) Directory.SetCreationTime(path, time);
            else if (File.Exists(path)) File.SetCreationTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastWriteTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリに最終更新日時を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="time">最終更新日時</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetLastWriteTime(string path, DateTime time)
        {
            if (Directory.Exists(path)) Directory.SetLastWriteTime(path, time);
            else if (File.Exists(path)) File.SetLastWriteTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastAccessTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリに最終アクセス日時を設定します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// <param name="time">最終アクセス日時</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetLastAccessTime(string path, DateTime time)
        {
            if (Directory.Exists(path)) Directory.SetLastAccessTime(path, time);
            else if (File.Exists(path)) File.SetLastAccessTime(path, time);
        }

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

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// ファイルまたはディレクトリが存在するかどうかを判別します。
        /// </summary>
        /// 
        /// <param name="path">対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Exists(string path)
            => File.Exists(path) || Directory.Exists(path);

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// ファイルまたはディレクトリを削除します。
        /// </summary>
        /// 
        /// <param name="path">削除対象となるパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void Delete(string path)
        {
            if (Get(path).IsDirectory) Directory.Delete(path, true);
            else File.Delete(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// ファイルを新規作成します。
        /// </summary>
        /// 
        /// <param name="path">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public FileStream Create(string path) => File.Create(path);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenRead
        ///
        /// <summary>
        /// ファイルを読み込み専用で開きます。
        /// </summary>
        /// 
        /// <param name="path">ファイルのパス</param>
        /// 
        /// <returns>読み込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public FileStream OpenRead(string path) => File.OpenRead(path);

        /* ----------------------------------------------------------------- */
        ///
        /// OpenWrite
        ///
        /// <summary>
        /// ファイルを新規作成、または上書き用で開きます。
        /// </summary>
        /// 
        /// <param name="path">ファイルのパス</param>
        /// 
        /// <returns>書き込み用ストリーム</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public FileStream OpenWrite(string path) => File.OpenWrite(path);

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを作成します。
        /// </summary>
        /// 
        /// <param name="path">ディレクトリのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

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
