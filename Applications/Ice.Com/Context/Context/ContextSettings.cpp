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
#include "ContextSettings.h"
#include "ContextPresetMenu.h"
#include "Log.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextSettings
///
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextSettings::ContextSettings() :
    program_(),
    preset_(0x00007ff3),
    customized_(false),
    custom_() {}

/* ------------------------------------------------------------------------- */
///
/// Load
///
/// <summary>
/// ユーザ設定を読み込みます。
/// </summary>
///
/* ------------------------------------------------------------------------- */
void ContextSettings::Load() {
    try {
        auto hkey = Open(_T("Software\\CubeSoft\\CubeICE\\v3\\Context"));
        if (hkey == nullptr) return;

        auto ps = GetDword(hkey, _T("Preset"), static_cast<DWORD>(PresetMenu::Unknown));
        if (ps != PresetMenu::Unknown) preset_ = ps;

        auto cs = GetDword(hkey, _T("IsCustomized"), 0);
        customized_ = (cs != 0);
        if (customized_) {
            auto custom = Open(hkey, _T("Custom"));
            if (custom != nullptr) LoadCore(custom, Custom());
        }

        RegCloseKey(hkey);
    }
    catch (...) { CUBE_LOG << _T("Registry error"); }
}

/* ------------------------------------------------------------------------- */
///
/// LoadCore
///
/// <summary>
/// カスタマイズ項目を取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
void ContextSettings::LoadCore(HKEY hkey, ContextMenuList& dest) {
    for (auto s : GetSubKeyNames(hkey)) {
        auto current = Open(hkey, s);
        if (current == nullptr) continue;

        auto name = GetString(current, _T("Name"));
        auto args = GetString(current, _T("Arguments"));
        auto icon = GetIconLocation(GetDword(current, _T("IconIndex"), 0));

        ContextMenuItem item(name, Program(), args, icon);
        auto children = Open(current, _T("Children"));
        if (children != nullptr) LoadCore(children, item.Children());
        dest.push_back(item);
    }
}

/* ------------------------------------------------------------------------- */
///
/// Open
///
/// <summary>
/// 指定されたキー下にあるサブキーを読み込み専用で開きます。
/// </summary>
///
/* ------------------------------------------------------------------------- */
HKEY ContextSettings::Open(HKEY hkey, const TString& name) {
    HKEY dest;
    auto result = RegOpenKeyEx(hkey, name.c_str(), 0, KEY_READ, &dest);
    return result == ERROR_SUCCESS ? dest : nullptr;
}

/* ------------------------------------------------------------------------- */
///
/// Open
///
/// <summary>
/// HKEY_CURRENT_USER 下にあるサブキーを読み込み専用で開きます。
/// </summary>
///
/* ------------------------------------------------------------------------- */
HKEY ContextSettings::Open(const TString& name) {
    return Open(HKEY_CURRENT_USER, name);
}

/* ------------------------------------------------------------------------- */
///
/// GetSubKeyNames
///
/// <summary>
/// サブキー名の一覧を取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
std::vector<ContextSettings::TString> ContextSettings::GetSubKeyNames(HKEY hkey) {
    std::vector<TString> dest;
    DWORD count  = 0;
    DWORD maxlen = 0;

    auto s0 = ::RegQueryInfoKey(hkey, NULL, NULL, NULL, &count, &maxlen, NULL, NULL, NULL, NULL, NULL, NULL);
    if (s0 != ERROR_SUCCESS) return dest;

    dest.reserve(count);
    for (auto i = 0u; i < count; ++i) {
        std::vector<TCHAR> buffer(maxlen + 1, 0);
        auto size = static_cast<DWORD>(buffer.size());
        auto s1 = ::RegEnumKeyEx(hkey, i, reinterpret_cast<TCHAR*>(&buffer[0]), &size, NULL, NULL, NULL, NULL);
        if (s1 != ERROR_SUCCESS) break;
        dest.push_back(TString(reinterpret_cast<TCHAR*>(&buffer[0])));
    }
    return dest;
}

/* ------------------------------------------------------------------------- */
///
/// GetDword
///
/// <summary>
/// DWORD の値を取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
DWORD ContextSettings::GetDword(HKEY hkey, const TString& name, DWORD alternate) {
    DWORD dest = 0;
    DWORD size = sizeof(dest);
    auto result = RegQueryValueEx(hkey, name.c_str(), nullptr, nullptr, reinterpret_cast<LPBYTE>(&dest), &size);
    return result == ERROR_SUCCESS ? dest : alternate;
}

/* ------------------------------------------------------------------------- */
///
/// GetString
///
/// <summary>
/// 文字列の値を取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextSettings::TString ContextSettings::GetString(HKEY hkey, const TString& name) {
    TCHAR dest[1024] = {};
    DWORD size = sizeof(dest);
    auto result = RegQueryValueEx(hkey, name.c_str(), nullptr, nullptr, reinterpret_cast<LPBYTE>(&dest), &size);
    return result == ERROR_SUCCESS ? TString(dest) : TString();
}

/* ------------------------------------------------------------------------- */
///
/// GetIconLocation
///
/// <summary>
/// アイコンを示す文字列を取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextSettings::TString ContextSettings::GetIconLocation(DWORD index) {
    if (Program().empty() || index == 0) return TString();
    std::basic_stringstream<TCHAR> ss;
    ss << Program() << _T(",") << index;
    return ss.str();
}

}}} // Cube::FileSystem::Ice