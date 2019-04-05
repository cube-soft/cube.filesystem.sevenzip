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
#include <initguid.h>
#include "SevenZipLibrary.h"
#include <shlwapi.h>
#include <stdexcept>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// GetSevenZipPath
///
/// <summary>
/// 7z.dll のパスを取得します。
/// </summary>
///
/// <param name="handle">実行モジュールのハンドラ</param>
///
/// <returns>7z.dll のパス</returns>
///
/* ------------------------------------------------------------------------- */
static std::basic_string<TCHAR> GetSevenZipPath(HINSTANCE handle) {
    TCHAR dir[MAX_PATH] = {};
    GetModuleFileName(handle, dir, sizeof(dir) / sizeof(dir[0]));
    PathRemoveFileSpec(dir);

    TCHAR dest[MAX_PATH] = {};
    PathCombine(dest, dir, _T("7z.dll"));
    return dest;
}

/* ------------------------------------------------------------------------- */
///
/// SevenZipLibrary
///
/// <summary>
/// 7z.dll をロードするためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
SevenZipLibrary::SevenZipLibrary(HINSTANCE handle) :
    dll_(GetSevenZipPath(handle)),
    create_(dll_.GetProcAddress<CreateObjectFunc>("CreateObject")) {}

/* ------------------------------------------------------------------------- */
///
/// GetInArchive
///
/// <summary>
/// 圧縮ファイルに対応する IInArchive オブジェクトを取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ComPtr<IInArchive> SevenZipLibrary::GetInArchive(const CLSID* clsid) const {
    IInArchive* dest;

    if (create_(clsid, &IID_IInArchive, reinterpret_cast<void**>(&dest)) != S_OK) {
        throw std::runtime_error("CreateObject");
    }

    return ComPtr<IInArchive>(dest);
}

}}}