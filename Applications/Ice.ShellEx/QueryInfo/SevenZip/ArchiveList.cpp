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
#include "ArchiveList.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ArchiveList
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="path">圧縮ファイルのパス</param>
///
/* ------------------------------------------------------------------------- */
ArchiveList::ArchiveList(const TString& path, HINSTANCE handle)
    : sevenzip_(handle), path_(path), count_(0) {}

/* ------------------------------------------------------------------------- */
///
/// Get
/// 
/// <summary>
/// インデックスに対応するファイル名を取得します。
/// </summary>
///
/// <param name="index">圧縮ファイル中のインデックス</param>
///
/// <returns>ファイル名</returns>
///
/* ------------------------------------------------------------------------- */
ArchiveList::TString ArchiveList::Get(int /* index */) const {
    return TString();
}

}}}