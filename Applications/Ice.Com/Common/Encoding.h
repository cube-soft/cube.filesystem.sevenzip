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

#include <string>
#include <winnls.h>

namespace Cube {
namespace FileSystem {
namespace Ice {
namespace Encoding {

/* ------------------------------------------------------------------------- */
///
/// UnicodeToMultiByte
///
/// <summary>
/// Unicode をマルチバイト文字列に変換します。
/// </summary>
///
/// <param name="src">Unicode 文字列</param>
/// <param name="cp">文字コード</param>
///
/// <returns>マルチバイト文字列</returns>
///
/* ------------------------------------------------------------------------- */
inline std::basic_string<char> UnicodeToMultiByte(const std::basic_string<wchar_t>& src, UINT cp = CP_OEMCP) {
    char dest[2048] = {};
    auto count  = static_cast<int>(sizeof(dest) / sizeof(dest[0]));
    auto result = WideCharToMultiByte(cp, 0, src.c_str(), -1, dest, count, nullptr, nullptr);

    return result != 0 ?
           std::basic_string<char>(dest) :
           std::basic_string<char>();
}

/* ------------------------------------------------------------------------- */
///
/// MultiByteToUnicode
///
/// <summary>
/// マルチバイト文字列を Unicode に変換します。
/// </summary>
///
/// <param name="src">マルチバイト文字列</param>
/// <param name="cp">文字コード</param>
///
/// <returns>Unicode</returns>
///
/* ------------------------------------------------------------------------- */
inline std::basic_string<wchar_t> MultiByteToUnicode(const std::basic_string<char>& src, UINT cp = CP_OEMCP) {
    wchar_t dest[2048] = {};
    auto count  = static_cast<int>(sizeof(dest) / sizeof(dest[0]));
    auto result = MultiByteToWideChar(cp, 0, src.c_str(), -1, dest, count);

    return result != 0 ?
           std::basic_string<wchar_t>(dest) :
           std::basic_string<wchar_t>();
}

}}}}