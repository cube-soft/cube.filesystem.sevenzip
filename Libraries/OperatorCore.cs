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
        public IInformation Get(string path) => new AlphaInformation(path);

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
    }

    /* --------------------------------------------------------------------- */
    ///
    /// StandardInformation
    /// 
    /// <summary>
    /// 標準ライブラリを利用した IInformation の実装クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    internal class StandardInformation : IInformation
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// StandardInformation
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">ファイルまたはディレクトリのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public StandardInformation(string path) : this(
            Directory.Exists(path) ?
            new DirectoryInfo(path) as FileSystemInfo:
            new FileInfo(path) as FileSystemInfo
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// StandardInformation
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="raw">実装オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public StandardInformation(FileSystemInfo raw)
        {
            RawObject = raw ?? throw new ArgumentException();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// ファイルまたはディレクトリが存在するかどうかを示す値を
        /// 取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool Exists => RawObject.Exists;

        /* ----------------------------------------------------------------- */
        ///
        /// IsDirectory
        ///
        /// <summary>
        /// ディレクトリかどうかを示す値を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool IsDirectory
            => (Attributes & FileAttributes.Directory) == FileAttributes.Directory;

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// ファイル名の部分を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Name => RawObject.Name;

        /* ----------------------------------------------------------------- */
        ///
        /// Extension
        ///
        /// <summary>
        /// 拡張子を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Extension => RawObject.Extension;

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        ///
        /// <summary>
        /// 完全なパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string FullName => RawObject.FullName;

        /* ----------------------------------------------------------------- */
        ///
        /// DirectoryName
        ///
        /// <summary>
        /// ファイルまたはディレクトリの親ディレクトリのパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string DirectoryName
            => TryCast()?.DirectoryName ?? Path.GetDirectoryName(FullName);

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// ファイルサイズを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long Length => TryCast()?.Length ?? 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Attributes
        ///
        /// <summary>
        /// ファイルまたはディレクトリの属性を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public FileAttributes Attributes => RawObject.Attributes;

        /* ----------------------------------------------------------------- */
        ///
        /// CreationTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリの作成日時を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public DateTime CreationTime => RawObject.CreationTime;

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリの最終更新日時を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime => RawObject.LastWriteTime;

        /* ----------------------------------------------------------------- */
        ///
        /// LastAccessTime
        ///
        /// <summary>
        /// ファイルまたはディレクトリの最終アクセス日時を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public DateTime LastAccessTime => RawObject.LastAccessTime;

        /* ----------------------------------------------------------------- */
        ///
        /// RawObject
        ///
        /// <summary>
        /// 実装オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public FileSystemInfo RawObject { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// オブジェクトを最新の状態に更新します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Refresh() => RawObject.Refresh();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// FileInfo
        ///
        /// <summary>
        /// FileInfo オブジェクトへのキャストを施行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private FileInfo TryCast() => RawObject as FileInfo;

        #endregion
    }
}
