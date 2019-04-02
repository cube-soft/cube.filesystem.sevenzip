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

#include <tchar.h>
#include <windows.h>
#include <string>
#include <vector>
#include "ContextMenuItem.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextSettings
///
/// <summary>
/// コンテキストメニューに関するユーザ設定を読み込むためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class ContextSettings {
public:
    typedef std::basic_string<TCHAR> TString;
    typedef ContextMenuItem::ContextMenuList ContextMenuList;

    ContextSettings();
    ContextSettings(const ContextSettings&) = delete;
    ContextSettings& operator=(const ContextSettings&) = delete;
    virtual ~ContextSettings() {}

    TString& Program() { return program_; }
    const TString& Program() const { return program_; }
    const int& Preset() const { return preset_; }
    const bool& IsCustomized() const { return customized_; }
    const ContextMenuList& Custom() const { return custom_; }
    ContextMenuList& Custom() { return custom_; }
    void Load();

private:
    void LoadCore(HKEY, ContextMenuList&);

    HKEY Open(HKEY, const TString&);
    HKEY Open(const TString&);
    std::vector<TString> GetSubKeyNames(HKEY hkey);
    DWORD GetDword(HKEY, const TString&, DWORD);
    TString GetString(HKEY, const TString&);
    TString GetIconLocation(DWORD);

    TString program_;
    int preset_;
    bool customized_;
    ContextMenuList custom_;
};

}}} // Cube::FileSystem::Ice
