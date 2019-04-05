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
/// <param name="name">表示名</param>
/// <param name="filename">クリック時に実行されるプログラム</param>
/// <param name="arguments">コマンドライン引数</param>
/// <param name="icon">アイコンのパス</param>
///
/* ------------------------------------------------------------------------- */
ContextMenuItem::ContextMenuItem(
    const ContextMenuItem::TString& name,
    const ContextMenuItem::TString& filename,
    const ContextMenuItem::TString& arguments,
    const ContextMenuItem::TString& icon
) :
    index_(-1), name_(name), filename_(filename),
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
    const ContextMenuItem::TString& name,
    const ContextMenuItem::TString& icon
) :
    index_(-1), name_(name), filename_(),
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

}}} // Cube::FileSystem::Ice
