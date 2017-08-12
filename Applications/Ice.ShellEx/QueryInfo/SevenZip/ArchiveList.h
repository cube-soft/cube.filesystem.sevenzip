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

#include "SevenZipLibrary.h"
#include "Stream.h"
#include <tchar.h>
#include <string>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ArchiveList
/// 
/// <summary>
/// 圧縮ファイルのファイル一覧を取得するためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class ArchiveList {
public:
    typedef std::basic_string<TCHAR> TString;

    ArchiveList() = delete;
    ArchiveList(const TString&, HINSTANCE);
    ArchiveList(const ArchiveList&) = delete;
    ArchiveList& operator=(const ArchiveList&) = delete;
    ~ArchiveList() = default;

    int Count() const { return static_cast<int>(count_); }
    TString Get(int) const;

private:
    TString path_;
    UInt32 count_;
    SevenZipLibrary sevenzip_;
    ComPtr<IInArchive> archive_;
    ComPtr<InStream> stream_;
};

}}}