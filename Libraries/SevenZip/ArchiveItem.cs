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
using System.Threading;
using System.Threading.Tasks;
using Cube.Log;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItem
    /// 
    /// <summary>
    /// 圧縮ファイルの 1 項目を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveItem : IArchiveItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="obj">実装オブジェクト</param>
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItem(object obj, string src, int index,
            IQuery<string, string> password)
        {
            Source = src;
            Index  = index;
            Password = password;

            if (obj is IInArchive raw) _raw = raw;
            else throw new ArgumentException("invalid object");
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 圧縮ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// 圧縮ファイル中のインデックスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワード取得用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IQuery<string, string> Password { get; }

        #region IArchiveItem

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// 圧縮ファイル中の相対パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path => Get<string>(ItemPropId.Path);

        /* ----------------------------------------------------------------- */
        ///
        /// Extension
        ///
        /// <summary>
        /// 拡張子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Extension => System.IO.Path.GetExtension(Path);

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// 暗号化されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted => Get<bool>(ItemPropId.Encrypted);

        /* ----------------------------------------------------------------- */
        ///
        /// IsDirectory
        ///
        /// <summary>
        /// ディレクトリかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsDirectory => Get<bool>(ItemPropId.IsDirectory);

        /* ----------------------------------------------------------------- */
        ///
        /// Attributes
        ///
        /// <summary>
        /// 属性を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Attributes => Get<uint>(ItemPropId.Attributes);

        /* ----------------------------------------------------------------- */
        ///
        /// Size
        ///
        /// <summary>
        /// 展開後のファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Size => (long)Get<ulong>(ItemPropId.Size);

        /* ----------------------------------------------------------------- */
        ///
        /// CreationTime
        ///
        /// <summary>
        /// 作成日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime CreationTime => Get<DateTime>(ItemPropId.CreationTime);

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// 最終更新日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime => Get<DateTime>(ItemPropId.LastWriteTime);

        /* ----------------------------------------------------------------- */
        ///
        /// LastAccessTime
        ///
        /// <summary>
        /// 最終アクセス日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastAccessTime => Get<DateTime>(ItemPropId.LastAccessTime);

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        /// 
        /// <param name="directory">保存ディレクトリ</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        /// 
        /// <param name="directory">保存ディレクトリ</param>
        /// <param name="progress">進捗報告用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, IProgress<ArchiveReport> progress)
        {
            if (IsDirectory) CreateDirectory(System.IO.Path.Combine(directory, Path));
            else ExtractFile(directory, progress);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFile
        ///
        /// <summary>
        /// ファイルを展開します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void ExtractFile(string directory, IProgress<ArchiveReport> progress)
        {
            var dest = System.IO.Path.Combine(directory, Path);
            CreateDirectory(System.IO.Path.GetDirectoryName(dest));

            var stream = new ArchiveStreamWriter(System.IO.File.Create(dest));
            var callback = new ArchiveExtractCallback(Source, 1, Size, _ => stream)
            {
                Password = Password,
                Progress = progress,
            };

            try { _raw.Extract(new[] { (uint)Index }, 1, 0, callback); }
            finally
            {
                stream.Dispose();
                ExtractFileResult(directory, callback.Result);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFileResult
        ///
        /// <summary>
        /// 展開後の処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ExtractFileResult(string directory, OperationResult result)
        {
            switch (result)
            {
                case OperationResult.OK:
                case OperationResult.Unknown:
                    break;
                case OperationResult.DataError:
                    if (Encrypted) EncryptionError();
                    else throw new System.IO.IOException(result.ToString());
                    break;
                case OperationResult.WrongPassword:
                    EncryptionError();
                    break;
                case OperationResult.UserCancel:
                    throw new UserCancelException();
                default:
                    throw new System.IO.IOException(result.ToString());
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void CreateDirectory(string path)
        {
            if (System.IO.Directory.Exists(path)) return;
            System.IO.Directory.CreateDirectory(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// 情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Get<T>(ItemPropId pid)
        {
            try
            {
                var var = new PropVariant();
                _raw.GetProperty((uint)Index, pid, ref var);

                var obj = var.Object;
                return (obj != null && obj is T) ? (T)obj : default(T);
            }
            catch (Exception err)
            {
                this.LogWarn(err.ToString(), err);
                return default(T);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionError
        ///
        /// <summary>
        /// パスワードエラーに関する処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void EncryptionError()
        {
            if (Password is PasswordQuery query) query.Reset();
            throw new EncryptionException();
        }

        #region Fields
        private IInArchive _raw;
        #endregion

        #endregion
    }
}
