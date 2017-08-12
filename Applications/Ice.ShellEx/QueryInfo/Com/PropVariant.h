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
#include <propidl.h>
#include <propvarutil.h>
#include <string>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// PropVariantWrapper
/// 
/// <summary>
/// PROPVARIANT のラッパークラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class PropVariantWrapper : public PROPVARIANT {
public:
    typedef std::basic_string<TCHAR> TString;

    PropVariantWrapper() { PropVariantInit(this);	}
    PropVariantWrapper(const PROPVARIANT& cp) { PropVariantCopy(this, &cp); }
    PropVariantWrapper(bool x) { InitPropVariantFromBoolean(x, this); }
    PropVariantWrapper(unsigned int x) { InitPropVariantFromUInt32(x, this); }
    PropVariantWrapper(unsigned long x) { InitPropVariantFromUInt32(x, this); }
    PropVariantWrapper(unsigned long long x) { InitPropVariantFromUInt64(x, this); }
    PropVariantWrapper(const TCHAR* x) { InitPropVariantFromBstr(x, this); }
    PropVariantWrapper(const TString& x) { InitPropVariantFromBstr(x.c_str(), this); }
    PropVariantWrapper(const FILETIME &x) { InitPropVariantFromFileTime(&x, this); }
    ~PropVariantWrapper() { PropVariantClear(this); }

    PropVariantWrapper &operator=(const PROPVARIANT& cp) { if (this != &cp) PropVariantCopy(this, &cp); return *this; }
    PropVariantWrapper &operator=(bool x) { InitPropVariantFromBoolean(x, this); return *this; }
    PropVariantWrapper &operator=(unsigned int x) { InitPropVariantFromUInt32(x, this); return *this; }
    PropVariantWrapper &operator=(unsigned long x) { InitPropVariantFromUInt32(x, this); return *this; }
    PropVariantWrapper &operator=(unsigned long long x) { InitPropVariantFromUInt64(x, this); return *this; }
    PropVariantWrapper &operator=(const wchar_t *x) { InitPropVariantFromBstr(x, this); return *this; }
    PropVariantWrapper &operator=(const TString& x) { InitPropVariantFromBstr(x.c_str(), this); return *this; }
    PropVariantWrapper &operator=(const FILETIME& x) { InitPropVariantFromFileTime(&x, this); return *this; }

private:
    /* --------------------------------------------------------------------- */
    ///
    /// InitPropVariantFromBstr
    ///
    /// <summary>
    /// 文字列で PROPVARIANT を初期化します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    void InitPropVariantFromBstr(const TCHAR* str, PROPVARIANT* var) {
        PropVariantClear(var);
        var->vt = VT_BSTR;
        var->bstrVal = SysAllocString(str);
    }
};

}}}