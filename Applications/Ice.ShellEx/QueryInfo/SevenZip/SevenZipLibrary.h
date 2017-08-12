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

#include "Com/ComPtr.h"
#include "Com/DynamicLinkLibrary.h"
#include "SevenZipInterface.h"
#include <tchar.h>
#include <windows.h>
#include <string>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// SevenZipLibrary
/// 
/// <summary>
/// 7z.dll をロードするためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class SevenZipLibrary {
public:
    typedef std::basic_string<TCHAR> TString;

    SevenZipLibrary() = delete;
    SevenZipLibrary(HINSTANCE);
    SevenZipLibrary(const SevenZipLibrary&) = delete;
    SevenZipLibrary& operator=(const SevenZipLibrary&) = delete;
    ~SevenZipLibrary() = default;

    ComPtr<IInArchive> GetInArchive(const CLSID*) const;

private:
    typedef HRESULT(WINAPI *CreateObjectFunc)(const GUID *clsID, const GUID *iid, void **outObject);

    DynamicLinkLibrary dll_;
    CreateObjectFunc create_;
};

}}}