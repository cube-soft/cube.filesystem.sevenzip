/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Mode
    ///
    /// <summary>
    /// 実行モードを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Mode
    {
        /// <summary>無し</summary>
        None,
        /// <summary>圧縮</summary>
        Archive,
        /// <summary>解凍</summary>
        Extract,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveLocation
    ///
    /// <summary>
    /// 保存場所を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum SaveLocation
    {
        /// <summary>その他</summary>
        Others = 0,
        /// <summary>元のファイルと同じ場所</summary>
        Source = 1,
        /// <summary>実行時に指定</summary>
        Runtime = 2,
        /// <summary>デスクトップ</summary>
        Desktop = 3,
        /// <summary>マイドキュメント</summary>
        MyDocuments = 4,
        /// <summary>ドラッグ&amp;ドロップで指定</summary>
        Drop = 10,
        /// <summary>不明</summary>
        Unknown = -1,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateDirectoryMethod
    ///
    /// <summary>
    /// ディレクトリの生成方法を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum CreateDirectoryMethod
    {
        /// <summary>作成しない</summary>
        None = 0x00,
        /// <summary>単一フォルダの場合スキップ</summary>
        SkipSingleDirectory = 0x02,
        /// <summary>単一ファイルの場合スキップ</summary>
        SkipSingleFile = 0x04,
        /// <summary>スキップオプション用マスク</summary>
        SkipOptions = SkipSingleDirectory | SkipSingleFile,
        /// <summary>作成する</summary>
        Create = 0x01,
        /// <summary>必要な場合に作成する</summary>
        CreateSmart = Create | SkipSingleDirectory,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OpenDirectoryMethod
    ///
    /// <summary>
    /// 圧縮・展開後にディレクトリを開く処理を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum OpenDirectoryMethod
    {
        /// <summary>開かない</summary>
        None = 0x0000,
        /// <summary>デスクトップの場合は開かない</summary>
        SkipDesktop = 0x0002,
        /// <summary>開く</summary>
        Open = 0x0001,
        /// <summary>デスクトップ以外の場合に開く</summary>
        OpenNotDesktop = Open | SkipDesktop,
    }
}
