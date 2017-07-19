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
    public abstract class ArchiveItem : IInformation
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
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveItem(string src, int index,
            IQuery<string, string> password, Operator io)
        {
            Source   = src;
            Index    = index;
            Password = password;
            IO       = io;
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
        /// Encrypted
        ///
        /// <summary>
        /// 暗号化されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワード取得用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IQuery<string, string> Password { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// ファイル操作用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Operator IO { get; }

        #region IInformation

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// 存在するかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsDirectory
        ///
        /// <summary>
        /// ディレクトリかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsDirectory { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// NameWithoutExtension
        ///
        /// <summary>
        /// 拡張子を除いたファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string NameWithoutExtension { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Extension
        ///
        /// <summary>
        /// 拡張子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Extension { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DirectoryName
        ///
        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DirectoryName { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        ///
        /// <summary>
        /// 圧縮ファイル中の相対パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FullName { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// 展開後のファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attributes
        ///
        /// <summary>
        /// 属性を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public System.IO.FileAttributes Attributes { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// CreationTime
        ///
        /// <summary>
        /// 作成日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime CreationTime { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// 最終更新日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastAccessTime
        ///
        /// <summary>
        /// 最終アクセス日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastAccessTime { get; protected set; }

        #endregion

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
        public virtual void Refresh() { }

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
        public abstract void Extract(string directory, IProgress<ArchiveReport> progress);

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItemImpl
    /// 
    /// <summary>
    /// ArchiveItem の実装クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveItemImpl : ArchiveItem
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItemImpl
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="raw">実装オブジェクト</param>
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItemImpl(IInArchive raw, string src, int index,
            IQuery<string, string> password, Operator io)
            : base(src, index, password, io)
        {
            _raw = raw;

            Exists         = true;
            FullName       = Get<string>(ItemPropId.Path);
            Encrypted      = Get<bool>(ItemPropId.Encrypted);
            IsDirectory    = Get<bool>(ItemPropId.IsDirectory);
            Attributes     = GetAttributes();
            Length         = (long)Get<ulong>(ItemPropId.Size);
            CreationTime   = Get<DateTime>(ItemPropId.CreationTime);
            LastWriteTime  = Get<DateTime>(ItemPropId.LastWriteTime);
            LastAccessTime = Get<DateTime>(ItemPropId.LastAccessTime);

            var info = IO.Get(FullName);
            Name                 = info.Name;
            NameWithoutExtension = info.NameWithoutExtension;
            Extension            = info.Extension;
            DirectoryName        = info.DirectoryName;
        }

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
        /// <param name="progress">進捗報告用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void Extract(string directory, IProgress<ArchiveReport> progress)
        {
            if (IsDirectory)
            {
                var dir = IO.Combine(directory, FullName);
                if (!IO.Get(dir).Exists) IO.CreateDirectory(dir);
            }
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
            var dest = IO.Combine(directory, FullName);
            var dir  = IO.Get(dest).DirectoryName;
            if (!IO.Get(dir).Exists) IO.CreateDirectory(dir);

            var stream = new ArchiveStreamWriter(IO.Create(dest));
            var callback = new ArchiveExtractCallback(Source, 1, Length, _ => stream)
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
        /// GetAttributes
        ///
        /// <summary>
        /// 属性情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private System.IO.FileAttributes GetAttributes()
        {
            try { return (System.IO.FileAttributes)Get<uint>(ItemPropId.Attributes); }
            catch (Exception /* err */) { return System.IO.FileAttributes.Normal; }
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
