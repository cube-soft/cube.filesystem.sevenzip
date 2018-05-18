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
#include <cmath>
#include <iomanip>
#include <sstream>
#include <vector>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// Punct
/// 
/// <summary>
/// 3 桁毎に "," (Comma) を挿入します。
/// </summary>
///
/// <param name="value">対象となる値</param>
///
/// <returns>文字列</returns>
///
/* ------------------------------------------------------------------------- */
inline std::basic_string<TCHAR> Punct(long value) {
    auto number = std::abs(value); 
    auto sign = (value > 0) ? 1 : -1;
    
    std::vector<int> v;
    while (number / 1000) {
        v.push_back(number % 1000);
        number /= 1000;
    }

    std::basic_ostringstream<TCHAR> ss;
    ss << number * sign;
    for (auto pos = v.rbegin(); pos != v.rend(); ++pos) {
        ss << _T(",") << std::setfill(_T('0')) << std::setw(3) << *pos;
    }
    return ss.str();
}

/* ------------------------------------------------------------------------- */
///
/// PrettyByte
/// 
/// <summary>
/// バイト数を整形します。
/// </summary>
///
/// <param name="bytes">対象となる値</param>
///
/// <returns>文字列</returns>
///
/* ------------------------------------------------------------------------- */
inline std::basic_string<TCHAR> PrettyByte(long long bytes) {
    static const double digit = 1000.0;
    static const TCHAR units[][5] = {
        _T("Byte"), _T("KB"), _T("MB"), _T("GB"),
        _T("TB"), _T("PB"), _T("EB"), _T("ZB"), _T("YB")
    };

    auto value = static_cast<double>(bytes);
    auto n = 0;
    while (value > digit) {
        value /= digit;
        ++n;
        if (n > sizeof(units) / sizeof(units[0]) - 1) break;
    }

    TCHAR dest[64] = {};
    _stprintf_s(dest, _T("%.3g %s"), value, units[n]);
    return std::basic_string<TCHAR>(dest);
}

}}}