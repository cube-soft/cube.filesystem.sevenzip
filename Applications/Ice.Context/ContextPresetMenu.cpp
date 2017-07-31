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
#include "ContextPresetMenu.h"
#include "ContextMenuItem.h"
#include "Resources.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// GetMenuItem
/// 
/// <summary>
/// PresetMenu に対応する ContextMenuItem オブジェクトを取得します。
/// </summary>
///
/// <param name="id">メニュー ID</param>
/// <param name="exe">実行プログラム</param>
///
/// <returns>コンテキストメニュー項目</returns>
///
/* ------------------------------------------------------------------------- */
static ContextMenuItem GetContextMenuItem(int id, const std::basic_string<TCHAR>& exe) {
    switch (id)
    {
    case PresetMenu::Archive:
        return ContextMenuItem(CUBEICE_MENU_ARCHIVE, exe + _T(",1")); // Folder
    case PresetMenu::Extract:
        return ContextMenuItem(CUBEICE_MENU_EXTRACT, exe + _T(",2")); // Folder
    case PresetMenu::Mail:
        return ContextMenuItem(CUBEICE_MENU_MAIL, exe + _T(",1")); // Folder
    case PresetMenu::ArchiveZip:
        return ContextMenuItem(CUBEICE_MENU_ZIP, exe, _T("/c:zip"), _T(""));
    case PresetMenu::ArchiveZipPassword:
        return ContextMenuItem(CUBEICE_MENU_PASSWORD, exe, _T("/c:zip /p"), _T(""));
    case PresetMenu::ArchiveSevenZip:
        return ContextMenuItem(CUBEICE_MENU_7Z, exe, _T("/c:7z"), _T(""));
    case PresetMenu::ArchiveBZip2:
        return ContextMenuItem(CUBEICE_MENU_BZIP2, exe, _T("/c:bzip2"), _T(""));
    case PresetMenu::ArchiveGZip:
        return ContextMenuItem(CUBEICE_MENU_GZIP, exe, _T("/c:gzip"), _T(""));
    case PresetMenu::ArchiveSfx:
        return ContextMenuItem(CUBEICE_MENU_SFX, exe, _T("/c:exe"), _T(""));
    case PresetMenu::ArchiveDetail:
        return ContextMenuItem(CUBEICE_MENU_DETAIL, exe, _T("/c:detail"), _T(""));
    case PresetMenu::ExtractDesktop:
        return ContextMenuItem(CUBEICE_MENU_EDESKTOP, exe, _T("/x /o:desktop"), _T(""));
    case PresetMenu::ExtractMyDocuments:
        return ContextMenuItem(CUBEICE_MENU_EMYDOCS, exe, _T("/x /o:mydocuments"), _T(""));
    case PresetMenu::ExtractRuntime:
        return ContextMenuItem(CUBEICE_MENU_ERUNTIME, exe, _T("/x /o:runtime"), _T(""));
    case PresetMenu::ExtractSource:
        return ContextMenuItem(CUBEICE_MENU_ESOURCE, exe, _T("/x /o:source"), _T(""));
    case PresetMenu::MailBZip2:
        return ContextMenuItem(CUBEICE_MENU_BZIP2, exe, _T("/c:bzip2 /m"), _T(""));
    case PresetMenu::MailDetail:
        return ContextMenuItem(CUBEICE_MENU_DETAIL, exe, _T("/c:detail /m"), _T(""));
    case PresetMenu::MailGZip:
        return ContextMenuItem(CUBEICE_MENU_GZIP, exe, _T("/c:gzip /m"), _T(""));
    case PresetMenu::MailSevenZip:
        return ContextMenuItem(CUBEICE_MENU_7Z, exe, _T("/c:7z /m"), _T(""));
    case PresetMenu::MailSfx:
        return ContextMenuItem(CUBEICE_MENU_SFX, exe, _T("/c:exe /m"), _T(""));
    case PresetMenu::MailZip:
        return ContextMenuItem(CUBEICE_MENU_ZIP, exe, _T("/c:zip /m"), _T(""));
    case PresetMenu::MailZipPassword:
        return ContextMenuItem(CUBEICE_MENU_PASSWORD, exe, _T("/c:zip /p /m"), _T(""));
    default:
        break;
    }
    return ContextMenuItem(_T("CubeICE"), exe, _T(""), exe + _T(",0"));
}

/* ------------------------------------------------------------------------- */
///
/// AddContextMenuItem
/// 
/// <summary>
/// PresetMenu に対応する ContextMenuItem オブジェクトを追加します。
/// </summary>
///
/// <param name="src">追加先オブジェクト</param>
/// <param name="menu">メニュー ID の集合</param>
/// <param name="id">対象とするメニュー ID</param>
/// <param name="exe">実行プログラム</param>
///
/* ------------------------------------------------------------------------- */
static void AddContextMenuItem(ContextMenuItem& src, int menu, int id, const std::basic_string<TCHAR>& exe) {
    if ((menu & id) == 0) return;
    src.Children().push_back(GetContextMenuItem(id, exe));
}

/* ------------------------------------------------------------------------- */
///
/// GetContextMenuItems
/// 
/// <summary>
/// PresetMenu に対応するコンテキストメニューを取得します。
/// </summary>
///
/// <param name="menu">メニュー ID の集合</param>
/// <param name="exe">実行プログラム</param>
///
/* ------------------------------------------------------------------------- */
std::vector<ContextMenuItem> GetContextMenuItems(int menu, const std::basic_string<TCHAR>& exe) {
    std::vector<ContextMenuItem> dest;

    if ((menu & PresetMenu::Archive) != 0) {
        auto archive = GetContextMenuItem(PresetMenu::Archive, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveZip, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveZipPassword, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveSevenZip, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveBZip2, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveGZip, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveSfx, exe);
        AddContextMenuItem(archive, menu, PresetMenu::ArchiveDetail, exe);
        if (archive.Children().size() > 0) dest.push_back(archive);
    }

    if ((menu & PresetMenu::Extract) != 0) {
        auto extract = GetContextMenuItem(PresetMenu::Extract, exe);
        AddContextMenuItem(extract, menu, PresetMenu::ExtractSource, exe);
        AddContextMenuItem(extract, menu, PresetMenu::ExtractDesktop, exe);
        AddContextMenuItem(extract, menu, PresetMenu::ExtractMyDocuments, exe);
        AddContextMenuItem(extract, menu, PresetMenu::ExtractRuntime, exe);
        if (extract.Children().size() > 0) dest.push_back(extract);
    }

    if ((menu & PresetMenu::Mail) != 0) {
        auto mail = GetContextMenuItem(PresetMenu::Mail, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveZip, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveZipPassword, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveSevenZip, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveBZip2, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveGZip, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveSfx, exe);
        AddContextMenuItem(mail, menu, PresetMenu::ArchiveDetail, exe);
        if (mail.Children().size() > 0) dest.push_back(mail);
    }

    return dest;
}

}}} // Cube::FileSystem::Ice
