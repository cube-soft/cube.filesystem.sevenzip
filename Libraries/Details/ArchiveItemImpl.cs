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
using Cube.Generics;
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItemController
    ///
    /// <summary>
    /// ArchiveItem の情報を更新するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveItemController : Information.RefreshController
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItemController
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="archive">実装オブジェクト</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItemController(IInArchive archive, IQuery<string, string> password, IO io)
        {
            _archive  = archive;
            _password = password;
            _io       = io;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// 指定されたファイル名またはディレクトリ名のいずれか 1 つでも
        /// パス中のどこかに存在するかどうかを判別します。
        /// </summary>
        ///
        /// <param name="names">
        /// 判別するファイル名またはディレクトリ名一覧
        /// </param>
        ///
        /// <returns>存在するかどうかを示す値</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Match(IEnumerable<string> names) => _filter.MatchAny(names);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        ///
        /// <param name="src">展開項目</param>
        /// <param name="directory">保存ディレクトリ</param>
        /// <param name="progress">進捗報告用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(ArchiveItem src, string directory, IProgress<ArchiveReport> progress)
        {
            Debug.Assert(!string.IsNullOrEmpty(src.FullName));

            if (src.IsDirectory)
            {
                src.CreateDirectory(directory, _io);
                return;
            }

            using (var cb = new ArchiveExtractCallback(src.Source, directory, new[] { src }, _io))
            {
                cb.TotalCount = 1;
                cb.TotalBytes = src.Length;
                cb.Password   = _password;
                cb.Progress   = progress;

                _archive.Extract(new[] { (uint)src.Index }, 1, 0, cb);
                ThrowIfError(src, cb.Result);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 情報を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Invoke(Information src)
        {
            var cvt = src.TryCast<ArchiveItem>();
            Debug.Assert(cvt != null);

            cvt.Exists         = true;
            cvt.RawName        = GetPath(cvt);
            cvt.Encrypted      = Get<bool>(cvt, ItemPropId.Encrypted);
            cvt.IsDirectory    = Get<bool>(cvt, ItemPropId.IsDirectory);
            cvt.Attributes     = (System.IO.FileAttributes)Get<uint>(cvt, ItemPropId.Attributes);
            cvt.Length         = (long)Get<ulong>(cvt, ItemPropId.Size);
            cvt.CreationTime   = Get<DateTime>(cvt, ItemPropId.CreationTime);
            cvt.LastWriteTime  = Get<DateTime>(cvt, ItemPropId.LastWriteTime);
            cvt.LastAccessTime = Get<DateTime>(cvt, ItemPropId.LastAccessTime);

            _filter = new PathFilter(cvt.RawName)
            {
                AllowParentDirectory  = false,
                AllowDriveLetter      = false,
                AllowCurrentDirectory = false,
                AllowInactivation     = false,
                AllowUnc              = false,
            };

            cvt.FullName = _filter.EscapedPath;
            if (string.IsNullOrEmpty(_filter.EscapedPath)) return;

            var info = _io.Get(_filter.EscapedPath);
            cvt.Name                 = info.Name;
            cvt.NameWithoutExtension = info.NameWithoutExtension;
            cvt.Extension            = info.Extension;
            cvt.DirectoryName        = info.DirectoryName;

            if (cvt.FullName != cvt.RawName) this.LogDebug($"Escape:{cvt.FullName}\tRaw:{cvt.RawName}");
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
        private T Get<T>(ArchiveItem src, ItemPropId pid)
        {
            var var = new PropVariant();
            _archive.GetProperty((uint)src.Index, pid, ref var);

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
        private string GetPath(ArchiveItem src)
        {
            var dest = Get<string>(src, ItemPropId.Path);
            if (!string.IsNullOrEmpty(dest)) return dest;

            var i0 = _io.Get(src.Source);
            var i1 = _io.Get(i0.NameWithoutExtension);

            var fmt = Formats.FromExtension(i1.Extension);
            if (fmt != Format.Unknown) return i1.Name;

            var name = (src.Index == 0) ? i1.Name : $"{i1.Name}({src.Index})";
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
        private void ThrowIfError(ArchiveItem src, OperationResult result)
        {
            switch (result)
            {
                case OperationResult.OK:
                    break;
                case OperationResult.DataError:
                    if (src.Encrypted)
                    {
                        ResetPassword();
                        throw new EncryptionException();
                    }
                    else throw new System.IO.IOException($"{src.FullName} ({result})");
                case OperationResult.WrongPassword:
                    ResetPassword();
                    throw new EncryptionException();
                case OperationResult.UserCancel:
                    throw new OperationCanceledException();
                default:
                    throw new System.IO.IOException($"{src.FullName} ({result})");
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
            Debug.Assert(_password is PasswordQuery);
            ((PasswordQuery)_password).Reset();
        }

        #endregion

        #region Fields
        private readonly IInArchive _archive;
        private readonly IO _io;
        private readonly IQuery<string, string> _password;
        private PathFilter _filter;
        #endregion
    }
}
