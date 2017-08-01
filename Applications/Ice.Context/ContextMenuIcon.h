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
#include <map>
#include <string>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenuIcon
/// 
/// <summary>
/// コンテキストメニュー・アイコンの基底クラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class ContextMenuIcon {
public:
    typedef std::basic_string<TCHAR> TString;
    virtual void SetMenuIcon(const TString&, MENUITEMINFO&) = 0;
};

/* ------------------------------------------------------------------------- */
///
/// ArgbContextMenuIcon
/// 
/// <summary>
/// α値を処理可能なアイコンクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class ArgbContextMenuIcon : public ContextMenuIcon {
public:
    ArgbContextMenuIcon();
    ArgbContextMenuIcon(const ArgbContextMenuIcon&) = delete;
    virtual ~ArgbContextMenuIcon();
    void SetMenuIcon(const TString&, MENUITEMINFO&) override;

private:
    HMODULE ux_;
    std::map<TString, HBITMAP> map_;
};

}}}
