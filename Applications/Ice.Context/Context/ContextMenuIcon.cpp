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
#include "ContextMenuIcon.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ArgbContextMenuIcon
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ArgbContextMenuIcon::ArgbContextMenuIcon()
    : ux_(NULL), map_() {
    ux_ = LoadLibrary(_T("UXTHEME.DLL"));
}

/* ------------------------------------------------------------------------- */
///
/// ~ArgbContextMenuIcon
/// 
/// <summary>
/// オブジェクトを破棄します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ArgbContextMenuIcon::~ArgbContextMenuIcon() {
    for (auto& kv : map_) {
        if (kv.second) DeleteObject(kv.second);
    }
    map_.clear();
    if (ux_ != NULL) FreeLibrary(ux_);
    ux_ = NULL;
}

/* ------------------------------------------------------------------------- */
///
/// SetMenuIcon
/// 
/// <summary>
/// アイコンを設定します。
/// </summary>
///
/// <param name="src">アイコン用ファイルのパス</param>
/// <param name="dest">メニュー情報</param>
///
/* ------------------------------------------------------------------------- */
void ArgbContextMenuIcon::SetMenuIcon(const ArgbContextMenuIcon::TString& src, MENUITEMINFO& dest) {

}

}}}
