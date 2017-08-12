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
#pragma once

#include "ContextMenuItem.h"
#include <tchar.h>
#include <string>
#include <vector>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// PresetMenu
/// 
/// <summary>
/// 予め定義されたメニューを表す列挙型です。
/// </summary>
///
/* ------------------------------------------------------------------------- */
namespace PresetMenu {
    enum {
        Archive             = 0x00000001,
        Extract             = 0x00000002,
        Settings            = 0x00000004,
        Mail                = 0x00000008,

        ExtractSource       = 0x00000010,
        ExtractDesktop      = 0x00000020,
        ExtractRuntime      = 0x00000040,
        ExtractMyDocuments  = 0x00000080,

        ArchiveZip          = 0x00000100,
        ArchiveZipPassword  = 0x00000200,
        ArchiveSevenZip     = 0x00000400,
        ArchiveBZip2        = 0x00000800,
        ArchiveGZip         = 0x00001000,
        ArchiveDetail       = 0x00002000,
        ArchiveSfx          = 0x00004000,

        MailZip             = 0x00010000,
        MailZipPassword     = 0x00020000,
        MailSevenZip        = 0x00040000,
        MailBZip2           = 0x00080000,
        MailGZip            = 0x00100000,
        MailDetail          = 0x00200000,
        MailSfx             = 0x00400000,

        Unknown             = 0xf0000000,
    };
}

/* ------------------------------------------------------------------------- */
///
/// GetContextMenuItems
/// 
/// <summary>
/// PresetMenu に対応するコンテキストメニュー一覧を取得します。
/// </summary>
///
/// <param name="menu">メニュー ID の集合</param>
/// <param name="exe">実行プログラム</param>
///
/// <returns>コンテキストメニュー一覧</returns>
///
/* ------------------------------------------------------------------------- */
std::vector<ContextMenuItem> GetContextMenuItems(int menu, const std::basic_string<TCHAR>& exe);

}}} // Cube::FileSystem::Ice
