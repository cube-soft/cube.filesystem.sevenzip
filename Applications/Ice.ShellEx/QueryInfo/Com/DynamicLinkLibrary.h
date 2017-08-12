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
/// DynamicLinkLibrary
/// 
/// <summary>
/// *.dll をロードするためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class DynamicLinkLibrary {
public:
    typedef std::basic_string<TCHAR> TString;

    DynamicLinkLibrary() = delete;
    DynamicLinkLibrary(const TString& path) : handle_(LoadLibrary(path.c_str())) {}
    DynamicLinkLibrary(const DynamicLinkLibrary&) = delete;
    DynamicLinkLibrary& operator=(const DynamicLinkLibrary) = delete;
    ~DynamicLinkLibrary() { FreeLibrary(handle_); }

    template <class T>
    T GetProcAddress(const std::basic_string<char>& name) {
        return reinterpret_cast<T>(::GetProcAddress(handle_, name.c_str()));
    }

private:
    HMODULE handle_;
};

}}}