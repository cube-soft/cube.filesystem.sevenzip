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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveUpdateCallback
    /// 
    /// <summary>
    /// 圧縮ファイルを作成する際のコールバック関数群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveUpdateCallback : IArchiveUpdateCallback, ICryptoGetTextPassword2
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveUpdateCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="items">圧縮するファイル一覧</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveUpdateCallback(IList<FileItem> items)
        {
            Items = items;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 圧縮するファイル一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IList<FileItem> Items { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// 圧縮ファイルに設定するパスワードを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Password { get; set; }

        #endregion

        #region Methods

        #region ICryptoGetTextPassword2

        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword2
        ///
        /// <summary>
        /// 圧縮ファイルに設定するパスワードを取得します。
        /// </summary>
        /// 
        /// <param name="enabled">
        /// パスワードを設定するかどうかを示す値
        /// </param>
        /// 
        /// <param name="password">パスワード</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword2(ref int enabled, out string password)
        {
            enabled  = string.IsNullOrEmpty(Password) ? 0 : 1;
            password = Password;
            return 0;
        }

        #endregion

        #region IArchiveUpdateCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// Gives the size of the unpacked archive files.
        /// </summary>
        /// 
        /// <param name="total">
        /// Size of the unpacked archive files (bytes)
        /// </param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong total) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        /// 
        /// <summary>
        /// SetCompleted 7-zip internal function.
        /// </summary>
        /// 
        /// <param name="value">completed value</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCompleted([In] ref ulong value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUpdateItemInfo
        /// 
        /// <summary>
        /// 追加する項目に関する情報を取得します。
        /// </summary>
        /// 
        /// <param name="index">File index</param>
        /// <param name="newdata">1 if new, 0 if not</param>
        /// <param name="newprop">1 if new, 0 if not</param>
        /// <param name="indexInArchive">-1 if doesn't matter</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        /// 
        /// <remarks>
        /// 追加や修正時の挙動が未実装なので要実装。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetUpdateItemInfo(uint index, ref int newdata, ref int newprop, ref uint indexInArchive)
        {
            newdata = 1;
            newprop = 1;
            indexInArchive = uint.MaxValue;
            return 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        /// 
        /// <summary>
        /// 各種プロパティを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="pid">プロパティの種類</param>
        /// <param name="value">プロパティの内容</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetProperty(uint index, ItemPropId pid, ref PropVariant value)
        {
            var src = Items[(int)index];
            switch (pid)
            {
                case ItemPropId.Path:
                    value.Set(src.Path);
                    break;
                case ItemPropId.Extension:
                    value.Set(src.Extension);
                    break;
                case ItemPropId.Attributes:
                    value.Set(src.Attributes);
                    break;
                case ItemPropId.IsDirectory:
                    value.Set(src.IsDirectory);
                    break;
                case ItemPropId.IsAnti:
                    value.Set(false);
                    break;
                case ItemPropId.CreationTime:
                    value.Set(src.CreationTime);
                    break;
                case ItemPropId.LastAccessTime:
                    value.Set(src.LastAccessTime);
                    break;
                case ItemPropId.LastWriteTime:
                    value.Set(src.LastWriteTime);
                    break;
                case ItemPropId.Size:
                    value.Set((ulong)src.Size);
                    break;
                default:
                    value.SetEmpty();
                    break;
            }
            return 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        /// 
        /// <summary>
        /// ストリームを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="stream">読み込み用ストリーム</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialInStream stream)
        {
            var item = Items[(int)index];
            System.Diagnostics.Debug.Assert(!item.IsDirectory);
            stream = new ArchiveStreamReader(item.GetStream());
            return 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        /// 
        /// <summary>
        /// 処理結果を設定します。
        /// </summary>
        /// 
        /// <param name="result">処理結果</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetOperationResult(OperationResult result) { }

        /* ----------------------------------------------------------------- */
        ///
        /// EnumProperties
        ///
        /// <summary>
        /// EnumProperties 7-zip internal function.
        /// </summary>
        /// 
        /// <param name="enumerator">The enumerator pointer.</param>
        /// 
        /* ----------------------------------------------------------------- */
        public long EnumProperties(IntPtr enumerator) => 0x80004001L; // Not implemented

        #endregion

        #endregion
    }
}
