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

namespace Cube.FileSystem.Ice
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
        None,
        Archive,
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
        Others          =  0,
        Source          =  1,
        Runtime         =  2,
        Desktop         =  3,
        MyDocuments     =  4,
        Drop            = 10,
        Unknown         = -1,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteCondition
    ///
    /// <summary>
    /// 上書き方法を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum OverwriteCondition
    {
        Force           = 0,
        Confirm         = 1,
        ConfirmOld      = 2,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// RootDirectoryCondition
    ///
    /// <summary>
    /// ルートディレクトリの扱いを表す列挙型です。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum RootDirectoryCondition
    {
        None                = 0x00,
        SkipSingleDirectory = 0x02,
        SkipSingleFile      = 0x04,
        Create              = 0x01,
        CreateSmart         = Create | SkipSingleDirectory,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OpenDirectoryCondition
    ///
    /// <summary>
    /// 圧縮・展開後にディレクトリを開く処理を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum OpenDirectoryCondition
    {
        None            = 0x0000,
        SkipDesktop     = 0x0002,
        Open            = 0x0001,
        OpenNotDesktop  = Open | SkipDesktop,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PresetMenu
    ///
    /// <summary>
    /// 予め定義されているメニュー項目を表した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum PresetMenu
    {
        None                = 0x0000000,
        Archive             = 0x00000001,
        Extract             = 0x00000002,
        Settings            = 0x00000004,
        Mail                = 0x00000008,

        ExtractSource       = 0x00000010,
        ExtractDesktop      = 0x00000020,
        ExtractRuntime      = 0x00000040,
        ExtractMyDocuments  = 0x00000080,
        ExtractOptions      = 0x000000f0,

        ArchiveZip          = 0x00000100,
        ArchiveZipPassword  = 0x00000200,
        ArchiveSevenZip     = 0x00000400,
        ArchiveBZip2        = 0x00000800,
        ArchiveGZip         = 0x00001000,
        ArchiveDetail       = 0x00002000,
        ArchiveSfx          = 0x00004000,
        ArchiveOptions      = 0x00007f00,

        MailZip             = 0x00010000,
        MailZipPassword     = 0x00020000,
        MailSevenZip        = 0x00040000,
        MailBZip2           = 0x00080000,
        MailGZip            = 0x00100000,
        MailDetail          = 0x00200000,
        MailSfx             = 0x00400000,
        MailOptions         = 0x007f0000,

        DefaultContext      = 0x00007ff3,
        DefaultDesktop      = 0x00000107,
    }
}
