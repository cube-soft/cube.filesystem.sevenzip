/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.FileSystem.SevenZip.Archives;
using Cube.Log;
using System;

namespace Cube.FileSystem.SevenZip
{
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
        /// <param name="archive">実装オブジェクト</param>
        /// <param name="format">圧縮ファイル形式</param>
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItemImpl(IInArchive archive, Format format, string src, int index,
            IQuery<string, string> password, IO io)
            : base(format, src, index, password, io)
        {
            _archive = archive;

            Exists         = true;
            RawName        = GetPath();
            Encrypted      = Get<bool>(ItemPropId.Encrypted);
            IsDirectory    = Get<bool>(ItemPropId.IsDirectory);
            Attributes     = (System.IO.FileAttributes)Get<uint>(ItemPropId.Attributes);
            Length         = (long)Get<ulong>(ItemPropId.Size);
            CreationTime   = Get<DateTime>(ItemPropId.CreationTime);
            LastWriteTime  = Get<DateTime>(ItemPropId.LastWriteTime);
            LastAccessTime = Get<DateTime>(ItemPropId.LastAccessTime);

            Filter = new PathFilter(RawName)
            {
                AllowParentDirectory  = false,
                AllowDriveLetter      = false,
                AllowCurrentDirectory = false,
                AllowInactivation     = false,
                AllowUnc              = false,
            };

            FullName = Filter.EscapedPath;
            if (string.IsNullOrEmpty(Filter.EscapedPath)) return;

            var info = IO.Get(Filter.EscapedPath);
            Name                 = info.Name;
            NameWithoutExtension = info.NameWithoutExtension;
            Extension            = info.Extension;
            DirectoryName        = info.DirectoryName;

            if (FullName != RawName) this.LogDebug($"Escape:{FullName}\tRaw:{RawName}");
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
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(FullName));

            if (IsDirectory)
            {
                this.CreateDirectory(directory, IO);
                return;
            }

            using (var cb = new ArchiveExtractCallback(Source, directory, new[] { this }, IO))
            {
                cb.TotalCount = 1;
                cb.TotalBytes = Length;
                cb.Password   = Password;
                cb.Progress   = progress;

                _archive.Extract(new[] { (uint)Index }, 1, 0, cb);
                ThrowIfError(cb.Result);
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
            var var = new PropVariant();
            _archive.GetProperty((uint)Index, pid, ref var);

            var obj = var.Object;
            return (obj != null && obj is T) ? (T)obj : default(T);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPath
        ///
        /// <summary>
        /// パスを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// TAR 系に関してはパスの情報を取得する事ができないため、元の
        /// ファイル名の拡張子を .tar に変更したものをパスにする事として
        /// います。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string GetPath()
        {
            var dest = Get<string>(ItemPropId.Path);
            if (!string.IsNullOrEmpty(dest)) return dest;

            var i0 = IO.Get(Source);
            var i1 = IO.Get(i0.NameWithoutExtension);

            var fmt = Formats.FromExtension(i1.Extension);
            if (fmt != Format.Unknown) return i1.Name;

            var name = (Index == 0) ? i1.Name : $"{i1.Name}({Index})";
            return IsTarExtension(i0.Extension) ? $"{name}.tar" : name;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsTarExtension
        ///
        /// <summary>
        /// TAR 系の拡張子かどうかを判別します。
        /// </summary>
        ///
        /// <remarks>
        /// tb2 および t*z と言う文字列の場合に TAR 系の拡張子と判別して
        /// います。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsTarExtension(string ext) =>
            ext == ".tb2" || (ext.Length == 4 && ext[0] == '.' && ext[1] == 't' && ext[3] == 'z');

        /* ----------------------------------------------------------------- */
        ///
        /// ThrowIfError
        ///
        /// <summary>
        /// エラーが発生していた場合に例外を送出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ThrowIfError(OperationResult result)
        {
            switch (result)
            {
                case OperationResult.OK:
                    break;
                case OperationResult.DataError:
                    if (Encrypted)
                    {
                        ResetPassword();
                        throw new EncryptionException();
                    }
                    else throw new System.IO.IOException($"{FullName} ({result})");
                case OperationResult.WrongPassword:
                    ResetPassword();
                    throw new EncryptionException();
                case OperationResult.UserCancel:
                    throw new OperationCanceledException();
                default:
                    throw new System.IO.IOException($"{FullName} ({result})");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResetPassword
        ///
        /// <summary>
        /// パスワードをリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetPassword()
        {
            System.Diagnostics.Debug.Assert(Password is PasswordQuery);
            ((PasswordQuery)Password).Reset();
        }

        #endregion

        #region Fields
        private IInArchive _archive;
        #endregion
    }
}
