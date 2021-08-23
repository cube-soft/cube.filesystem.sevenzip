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
    /// Preset
    ///
    /// <summary>
    /// 予め定義されているメニュー項目を表した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum Preset
    {
        /// <summary>無し</summary>
        None = 0x0000000,
        /// <summary>圧縮</summary>
        Compress = 0x00000001,
        /// <summary>解凍</summary>
        Extract = 0x00000002,
        /// <summary>設定</summary>
        Settings = 0x00000004,
        /// <summary>圧縮してメール添付</summary>
        Mail = 0x00000008,

        /// <summary>圧縮ファイルと同じ場所に解凍</summary>
        ExtractSource = 0x00000010,
        /// <summary>デスクトップに解凍</summary>
        ExtractDesktop = 0x00000020,
        /// <summary>実行時に場所を指定して解凍</summary>
        ExtractRuntime = 0x00000040,
        /// <summary>マイドキュメントに解凍</summary>
        ExtractMyDocuments = 0x00000080,
        /// <summary>解凍オプション用マスク</summary>
        ExtractMask = 0x000000f0,

        /// <summary>Zip に圧縮</summary>
        CompressZip = 0x00000100,
        /// <summary>パスワード付 Zip に圧縮</summary>
        CompressZipPassword = 0x00000200,
        /// <summary>7z に圧縮</summary>
        CompressSevenZip = 0x00000400,
        /// <summary>BZip2 に圧縮</summary>
        CompressBZip2 = 0x00000800,
        /// <summary>GZip に圧縮</summary>
        CompressGZip = 0x00001000,
        /// <summary>詳細を設定して圧縮</summary>
        CompressOthers = 0x00002000,
        /// <summary>自己解凍形式に圧縮</summary>
        CompressSfx = 0x00004000,
        /// <summary>XZ に圧縮</summary>
        CompressXz = 0x00008000,
        /// <summary>圧縮オプション用マスク</summary>
        CompressMask = 0x0000ff00,

        /// <summary>Zip に圧縮してメール添付</summary>
        MailZip = 0x00010000,
        /// <summary>パスワード付 Zip に圧縮してメール添付</summary>
        MailZipPassword = 0x00020000,
        /// <summary>7z に圧縮してメール添付</summary>
        MailSevenZip = 0x00040000,
        /// <summary>BZip2 に圧縮してメール添付</summary>
        MailBZip2 = 0x00080000,
        /// <summary>GZip に圧縮してメール添付</summary>
        MailGZip = 0x00100000,
        /// <summary>詳細を設定して圧縮してメール添付</summary>
        MailOthers = 0x00200000,
        /// <summary>自己解凍形式に圧縮してメール添付</summary>
        MailSfx = 0x00400000,
        /// <summary>XZ に圧縮してメール添付</summary>
        MailXz = 0x00800000,
        /// <summary>圧縮してメール添付オプション用マスク</summary>
        MailMask = 0x00ff0000,

        /// <summary>コンテキストメニューの初期設定</summary>
        DefaultContext = 0x00007ff3,
        /// <summary>ショートカットメニューの初期設定</summary>
        DefaultDesktop = 0x00000107,
    }
}
