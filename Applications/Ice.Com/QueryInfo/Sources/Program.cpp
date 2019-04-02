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
#include "QueryInfo/QueryInfoFactory.h"
#include <initguid.h>
#include <shlguid.h>
#include <shlobj.h>
#include "Log.h"

// {CB8641A3-EBC7-4758-A302-AA6667B817C8}
DEFINE_GUID(CUBEICE_INFOTIP_CLSID, 0xcb8641a3, 0xebc7, 0x4758, 0xa3, 0x2, 0xaa, 0x66, 0x67, 0xb8, 0x17, 0xc8);

namespace {
    static HINSTANCE kInstance = nullptr;
    static ULONG kReferenceCount = 0;
}

/* ------------------------------------------------------------------------- */
///
/// DllMain
///
/// <summary>
/// DLL のエントリーポイントです。
/// </summary>
///
/// <param name="instance">プロセスハンドラ</param>
/// <param name="reason">実行された理由を示す値</param>
///
/// <returns>TRUE or FALSE</returns>
///
/* ------------------------------------------------------------------------- */
extern "C" int APIENTRY DllMain(HINSTANCE handle, DWORD reason, LPVOID /* reserved */) {
    switch (reason)
    {
    case DLL_PROCESS_ATTACH:
        kInstance = handle;
        break;
    case DLL_PROCESS_DETACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    default:
        break;
    }
    return TRUE;
}

/* ------------------------------------------------------------------------- */
///
/// DllCanUnloadNow
///
/// <summary>
/// DLL がアンロード可能かどうかを判別します。
/// </summary>
///
/// <returns>S_OK or S_FALSE</returns>
///
/* ------------------------------------------------------------------------- */
STDAPI DllCanUnloadNow()
{
    return (kReferenceCount == 0 ? S_OK : S_FALSE);
}

/* ------------------------------------------------------------------------- */
///
/// DllGetClassObject
///
/// <summary>
/// CLASS ID に対応するオブジェクトを取得します。
/// </summary>
///
/// <param name="clsid">クラス ID</param>
/// <param name="iid">インターフェイスを識別するための GUID</param>
/// <param name="obj">生成オブジェクト</param>
///
/// <returns>S_OK or S_FALSE</returns>
///
/* ------------------------------------------------------------------------- */
STDAPI DllGetClassObject(REFCLSID clsid, REFIID iid, LPVOID *obj) {
    *obj = nullptr;
    if (IsEqualCLSID(clsid, CUBEICE_INFOTIP_CLSID)) {
        auto info = new Cube::FileSystem::Ice::QueryInfoFactory(kInstance, kReferenceCount);
        if (!info) return E_OUTOFMEMORY;

        auto result = info->QueryInterface(iid, obj);
        info->Release();
        return result;
    }
    return CLASS_E_CLASSNOTAVAILABLE;
}
