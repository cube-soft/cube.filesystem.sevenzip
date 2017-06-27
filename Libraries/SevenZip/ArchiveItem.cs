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
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItem(object obj, int index)
        {
            Index = index;
            if (obj is IInArchive raw) _raw = raw;
            else throw new ArgumentException("invalid object");
        }

        #endregion

        #region Properties

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

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// 進捗状況の通知時に発生するイベントです。
        /// </summary>
        /// 
        /// <remarks>
        /// 通知される値は、展開の完了したバイト数です。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public ValueEventHandler<long> Progress;

        /* ----------------------------------------------------------------- */
        ///
        /// OnProgress
        ///
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected virtual void OnProgress(ValueEventArgs<long> e)
            => Progress?.Invoke(this, e);

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
        /// <param name="directory">保存するディレクトリ</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, string.Empty);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        /// 
        /// <param name="directory">保存するディレクトリ</param>
        /// <param name="password">パスワード</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, string password)
        {
            var dest = System.IO.Path.Combine(directory, Path);
            if (IsDirectory)
            {
                CreateDirectory(dest);
                return;
            }

            CreateDirectory(System.IO.Path.GetDirectoryName(dest));
            using(var stream = new ArchiveStreamWriter(System.IO.File.Create(dest)))
            {
                var callback = new ArchiveExtractCallback(this, password, stream);
                callback.Progress += RaiseProgress;
                _raw.Extract(new[] { (uint)Index }, 1, 0, callback);
                callback.Progress -= RaiseProgress;
            }
        }

        #endregion

        #region Implementations

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
            catch (Exception /* err */) { return default(T); }
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
        /// RaiseProgress
        ///
        /// <summary>
        /// Progress イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseProgress(object sender, ValueEventArgs<long> e)
            => OnProgress(e);

        #region Fields
        private IInArchive _raw;
        #endregion

        #endregion
    }
}
