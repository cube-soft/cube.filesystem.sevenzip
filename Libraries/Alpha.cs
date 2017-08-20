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
using Alphaleonis.Win32.Filesystem;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// Alpha
    /// 
    /// <summary>
    /// AlphaFS を利用した IOperatorCore の実装クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Alpha : IOperatorCore
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
        public void SetAttributes(string path, System.IO.FileAttributes attr)
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
            => Directory.Exists(path) || File.Exists(path);

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
        public System.IO.FileStream Create(string path) => File.Create(path);

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
        public System.IO.FileStream OpenRead(string path) => File.OpenRead(path);

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
        public System.IO.FileStream OpenWrite(string path) => File.OpenWrite(path);

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

    /* --------------------------------------------------------------------- */
    ///
    /// AlphaInformation
    /// 
    /// <summary>
    /// AlphaFS を利用した IInformation の実装クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    internal class AlphaInformation : IInformation
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AlphaInformation
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">ファイルまたはディレクトリのパス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public AlphaInformation(string path) : this(
            Directory.Exists(path) ?
            new DirectoryInfo(path) as FileSystemInfo:
            new FileInfo(path) as FileSystemInfo
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// AlphaInformation
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="raw">実装オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public AlphaInformation(FileSystemInfo raw)
        {
            RawObject = raw;
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
            => (Attributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Name => RawObject.Name;

        /* ----------------------------------------------------------------- */
        ///
        /// NameWithoutExtension
        ///
        /// <summary>
        /// 拡張子を除いたファイル名を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string NameWithoutExtension => Path.GetFileNameWithoutExtension(Name);

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
        public System.IO.FileAttributes Attributes => RawObject.Attributes;

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
        public void Refresh()
        {
            if (RawObject is FileInfo fi) fi.Refresh();
            else if (RawObject is DirectoryInfo di) di.Refresh();
        }

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
