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

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// QueryInfoSettings
///
/// <summary>
/// ツールチップに関するユーザ設定を読み込むためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class QueryInfoSettings {
public:
    typedef std::basic_string<TCHAR> TString;

    QueryInfoSettings();
    QueryInfoSettings(const QueryInfoSettings& cp) = delete;
    QueryInfoSettings& operator=(const QueryInfoSettings&) = delete;
    virtual ~QueryInfoSettings() {}

    const bool ToolTip() const { return enabled_; }
    const int ToolTipCount() const { return count_; }
    void Load();

private:
    HKEY Open(const TString&);
    DWORD GetDword(HKEY, const TString&, DWORD);

    bool enabled_;
    int count_;
};

}}} // Cube::FileSystem::Ice
