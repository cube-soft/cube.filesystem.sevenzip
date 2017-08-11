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

#include <shlobj.h>
#include <tchar.h>
#include <string>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// QueryInfo
/// 
/// <summary>
/// 圧縮ファイルの情報を表示するためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class QueryInfo : public IQueryInfo, IPersistFile {
public:
    typedef std::basic_string<TCHAR> TString;

    QueryInfo() = delete;
    QueryInfo(const QueryInfo&) = delete;
    QueryInfo(HINSTANCE, ULONG&);
    QueryInfo& operator=(const QueryInfo&) = delete;
    virtual ~QueryInfo();

    STDMETHOD(QueryInterface)(REFIID iid, LPVOID * obj); // IUnknown
    STDMETHOD_(ULONG, AddRef)(void); // IUnknown
    STDMETHOD_(ULONG, Release)(void); // IUnknown
    STDMETHODIMP GetInfoTip(DWORD, LPWSTR*); // IQueryInfo
    STDMETHODIMP GetInfoFlags(DWORD*) { return E_NOTIMPL; } // IQueryInfo
    STDMETHODIMP GetClassID(CLSID*) { return E_NOTIMPL; } // IPersistFile
    STDMETHODIMP IsDirty() { return E_NOTIMPL; } // IPersistFile
    STDMETHODIMP Load(LPCOLESTR, DWORD); // IPersistFile
    STDMETHODIMP Save(LPCOLESTR, BOOL) { return E_NOTIMPL; } // IPersistFile
    STDMETHODIMP SaveCompleted(LPCOLESTR) { return E_NOTIMPL; } // IPersistFile
    STDMETHODIMP GetCurFile(LPOLESTR*) { return E_NOTIMPL; } // IPersistFile

private:
    HINSTANCE handle_;
    ULONG& dllCount_;
    ULONG objCount_;
};

}}}