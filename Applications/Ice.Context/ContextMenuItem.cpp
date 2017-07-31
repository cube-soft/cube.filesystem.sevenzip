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
#include "ContextMenuItem.h"
#include "Resources.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenuItem
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="index">表示順序を示すインデックス</param>
/// <param name="name">表示名</param>
/// <param name="filename">クリック時に実行されるプログラム</param>
/// <param name="arguments">コマンドライン引数</param>
/// <param name="icon">アイコンのパス</param>
///
/* ------------------------------------------------------------------------- */
ContextMenuItem::ContextMenuItem(
    int index,
    const ContextMenuItem::TString& name,
    const ContextMenuItem::TString& filename,
    const ContextMenuItem::TString& arguments,
    const ContextMenuItem::TString& icon
) :
    index_(index), name_(name), filename_(filename),
    arguments_(arguments), icon_(icon), children_() {}

/* ------------------------------------------------------------------------- */
///
/// ContextMenuItem
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="index">表示順序を示すインデックス</param>
/// <param name="name">表示名</param>
/// <param name="icon">アイコンのパス</param>
///
/// <remarks>
/// ディレクトリのようにクリック時に実行するプログラムが存在しない場合に
/// 使用されます。
/// </remarks>
///
/* ------------------------------------------------------------------------- */
ContextMenuItem::ContextMenuItem(
    int index,
    const ContextMenuItem::TString& name,
    const ContextMenuItem::TString& icon
) :
    index_(index), name_(name), filename_(),
    arguments_(), icon_(icon), children_() {}

/* ------------------------------------------------------------------------- */
///
/// ContextMenuItem
/// 
/// <summary>
/// オブジェクトをコピーします。
/// </summary>
///
/// <param name="cp">コピー元オブジェクト</param>
///
/* ------------------------------------------------------------------------- */
ContextMenuItem::ContextMenuItem(const ContextMenuItem& cp) :
    index_(cp.index_), name_(cp.name_), filename_(cp.filename_),
    arguments_(cp.arguments_), icon_(cp.icon_), children_(cp.children_) {}

/* ------------------------------------------------------------------------- */
///
/// GetMenuItem
/// 
/// <summary>
/// PresetMenu に対応する ContextMenuItem オブジェクトを取得します。
/// </summary>
///
/// <param name="preset">PresetMenu ID</param>
/// <param name="index">表示インデックス</param>
/// <param name="exe">実行プログラム</param>
///
/// <returns>コンテキストメニュー項目</returns>
///
/* ------------------------------------------------------------------------- */
ContextMenuItem GetContextMenuItem(int preset, int index, const std::basic_string<TCHAR>& exe) {
    switch (preset)
    {
    case PresetMenu::Archive:
        return ContextMenuItem(index, CUBEICE_MENU_ARCHIVE,  exe + _T(",1")); // Folder
    case PresetMenu::Extract:
        return ContextMenuItem(index, CUBEICE_MENU_EXTRACT,  exe + _T(",2")); // Folder
    case PresetMenu::Mail:
        return ContextMenuItem(index, CUBEICE_MENU_MAIL,     exe + _T(",1")); // Folder
    case PresetMenu::ArchiveZip:
        return ContextMenuItem(index, CUBEICE_MENU_ZIP,      exe, _T("/c:zip"), _T(""));
    case PresetMenu::ArchiveZipPassword:
        return ContextMenuItem(index, CUBEICE_MENU_PASSWORD, exe, _T("/c:zip /p"), _T(""));
    case PresetMenu::ArchiveSevenZip:
        return ContextMenuItem(index, CUBEICE_MENU_7Z,       exe, _T("/c:7z"), _T(""));
    case PresetMenu::ArchiveBZip2:
        return ContextMenuItem(index, CUBEICE_MENU_BZIP2,    exe, _T("/c:bzip2"), _T(""));
    case PresetMenu::ArchiveGZip:
        return ContextMenuItem(index, CUBEICE_MENU_GZIP,     exe, _T("/c:gzip"), _T(""));
    case PresetMenu::ArchiveSfx:
        return ContextMenuItem(index, CUBEICE_MENU_SFX,      exe, _T("/c:exe"), _T(""));
    case PresetMenu::ArchiveDetail:
        return ContextMenuItem(index, CUBEICE_MENU_DETAIL,   exe, _T("/c:detail"), _T(""));
    case PresetMenu::ExtractDesktop:
        return ContextMenuItem(index, CUBEICE_MENU_EDESKTOP, exe, _T("/x /o:desktop"), _T(""));
    case PresetMenu::ExtractMyDocuments:
        return ContextMenuItem(index, CUBEICE_MENU_EMYDOCS,  exe, _T("/x /o:mydocuments"), _T(""));
    case PresetMenu::ExtractRuntime:
        return ContextMenuItem(index, CUBEICE_MENU_ERUNTIME, exe, _T("/x /o:runtime"), _T(""));
    case PresetMenu::ExtractSource:
        return ContextMenuItem(index, CUBEICE_MENU_ESOURCE,  exe, _T("/x /o:source"), _T(""));
    case PresetMenu::MailBZip2:
        return ContextMenuItem(index, CUBEICE_MENU_BZIP2,    exe, _T("/c:bzip2 /m"), _T(""));
    case PresetMenu::MailDetail:
        return ContextMenuItem(index, CUBEICE_MENU_DETAIL,   exe, _T("/c:detail /m"), _T(""));
    case PresetMenu::MailGZip:
        return ContextMenuItem(index, CUBEICE_MENU_GZIP,     exe, _T("/c:gzip /m"), _T(""));
    case PresetMenu::MailSevenZip:
        return ContextMenuItem(index, CUBEICE_MENU_7Z,       exe, _T("/c:7z /m"), _T(""));
    case PresetMenu::MailSfx:
        return ContextMenuItem(index, CUBEICE_MENU_SFX,      exe, _T("/c:exe /m"), _T(""));
    case PresetMenu::MailZip:
        return ContextMenuItem(index, CUBEICE_MENU_ZIP,      exe, _T("/c:zip /m"), _T(""));
    case PresetMenu::MailZipPassword:
        return ContextMenuItem(index, CUBEICE_MENU_PASSWORD, exe, _T("/c:zip /p /m"), _T(""));
    default:
        break;
    }
    return ContextMenuItem(index, _T("CubeICE"), exe, _T(""), exe + _T(",0"));
}

}}} // Cube::FileSystem::Ice
