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

#include "Config.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// QueryInfoFactory
/// 
/// <summary>
/// QueryInfo の生成用クラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class QueryInfoFactory : public IClassFactory {
public:
    QueryInfoFactory() = delete;
    QueryInfoFactory(HINSTANCE, ULONG&);
    QueryInfoFactory(const QueryInfoFactory&) = delete;
    QueryInfoFactory& operator=(const QueryInfoFactory&) = delete;
    virtual ~QueryInfoFactory();

    STDMETHOD(QueryInterface)(REFIID iid, LPVOID * obj); // IUnknown
    STDMETHOD_(ULONG, AddRef)(void); // IUnknown
    STDMETHOD_(ULONG, Release)(void); // IUnknown
    STDMETHODIMP CreateInstance(LPUNKNOWN, REFIID, LPVOID FAR*); // IClassFactory
    STDMETHODIMP LockServer(BOOL) { return NOERROR; } // IClassFactory

private:
    HINSTANCE handle_;
    ULONG& dllCount_;
    ULONG objCount_;
};

}}} // Cube::FileSystem::Ice
