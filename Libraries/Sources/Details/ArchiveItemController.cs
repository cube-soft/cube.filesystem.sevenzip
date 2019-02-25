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
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItemController
    ///
    /// <summary>
    /// Provides functionality to get properties of the archived item and
    /// execute the processing of the extraction.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveItemController : IRefreshable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItemController
        ///
        /// <summary>
        /// Initializes a new instance of the ArchvieItemController class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="archive">7-zip module.</param>
        /// <param name="index">Index in the archive.</param>
        /// <param name="password">Query to get password.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItemController(IInArchive archive, int index,
            IQuery<string> password, IO io)
        {
            Index     = index;
            _archive  = archive;
            _password = password;
            _io       = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index of the item in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RawName
        ///
        /// <summary>
        /// Gets the original path described in the archive.
        /// </summary>
        ///
        /// <remarks>
        /// RawName の内容に対して、Windows で使用不可能な文字列に対する
        /// エスケープ処理を実行した結果が FullName となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string RawName { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// Gets a value indicating whether the item is encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Refreshes information of the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(RefreshableInfo src)
        {
            RawName   = GetPath(src.Source);
            Encrypted = Get<bool>(src.Source, ItemPropId.Encrypted);

            src.Exists         = true;
            src.IsDirectory    = Get<bool>(src.Source, ItemPropId.IsDirectory);
            src.Attributes     = (System.IO.FileAttributes)Get<uint>(src.Source, ItemPropId.Attributes);
            src.Length         = (long)Get<ulong>(src.Source, ItemPropId.Size);
            src.CreationTime   = Get<DateTime>(src.Source, ItemPropId.CreationTime);
            src.LastWriteTime  = Get<DateTime>(src.Source, ItemPropId.LastWriteTime);
            src.LastAccessTime = Get<DateTime>(src.Source, ItemPropId.LastAccessTime);

            _filter = new PathFilter(RawName)
            {
                AllowParentDirectory  = false,
                AllowDriveLetter      = false,
                AllowCurrentDirectory = false,
                AllowInactivation     = false,
                AllowUnc              = false,
            };

            src.FullName = _filter.EscapedPath;
            if (string.IsNullOrEmpty(_filter.EscapedPath)) return;

            var info = _io.Get(_filter.EscapedPath);
            src.Name                 = info.Name;
            src.NameWithoutExtension = info.NameWithoutExtension;
            src.Extension            = info.Extension;
            src.DirectoryName        = info.DirectoryName;

            if (src.FullName != RawName) this.LogDebug($"Escape:{src.FullName}\tRaw:{RawName}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// Determines whether any one of the specified filename or
        /// directory name exists somewhere in the path.
        /// </summary>
        ///
        /// <param name="names">
        /// Collection of filenames and directory names.
        /// </param>
        ///
        /// <returns>true for match; otherwise, false.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Match(IEnumerable<string> names) => _filter.MatchAny(names);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified item and save to the specified directory.
        /// </summary>
        ///
        /// <param name="src">Item to extract.</param>
        /// <param name="directory">Saving directory.</param>
        /// <param name="progress">Object to notify progress.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(ArchiveItem src, string directory, IProgress<Report> progress)
        {
            Debug.Assert(!string.IsNullOrEmpty(src.FullName));

            if (src.IsDirectory)
            {
                src.CreateDirectory(directory, _io);
                return;
            }

            using (var cb = new ArchiveExtractCallback(src.FullName, directory, new[] { src }, _io))
            {
                cb.TotalCount = 1;
                cb.TotalBytes = src.Length;
                cb.Password   = _password;
                cb.Progress   = progress;

                _archive.Extract(new[] { (uint)Index }, 1, 0, cb);
                ThrowIfError(src, cb.Result);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets information corresponding to the specified ID.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Get<T>(string src, ItemPropId pid)
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
        /// Gets the path of the specified item.
        /// </summary>
        ///
        /// <remarks>
        /// TAR 系に関してはパスの情報を取得する事ができないため、元の
        /// ファイル名の拡張子を .tar に変更したものをパスにする事として
        /// います。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string GetPath(string src)
        {
            var dest = Get<string>(src, ItemPropId.Path);
            if (!string.IsNullOrEmpty(dest)) return dest;

            var i0  = _io.Get(src);
            var i1  = _io.Get(i0.NameWithoutExtension);
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
        /// Determines whether the specified extension is one of the TAR
        /// archives.
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
        /// Throws an exception if the specified result represents an error.
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
        /// Resets the password query.
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
        private readonly IQuery<string> _password;
        private PathFilter _filter;
        #endregion
    }
}
