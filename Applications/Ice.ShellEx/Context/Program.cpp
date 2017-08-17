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
#include "Config.h"
#include "Context/ContextMenuFactory.h"
#include "Context/ContextMenu.h"
#include <initguid.h>
#include <shlguid.h>

// {BE7948B2-982A-4FDF-9A72-5EBB9134A787}
DEFINE_GUID(CUBEICE_CTX_CLSID, 0xbe7948b2, 0x982a, 0x4fdf, 0x9a, 0x72, 0x5e, 0xbb, 0x91, 0x34, 0xa7, 0x87);

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
    if (IsEqualCLSID(clsid, CUBEICE_CTX_CLSID)) {
        auto menu = new Cube::FileSystem::Ice::ContextMenuFactory(kInstance, kReferenceCount);
        if (!menu) return E_OUTOFMEMORY;

        auto result = menu->QueryInterface(iid, obj);
        menu->Release();
        return result;
    }
    else obj = nullptr;

    return CLASS_E_CLASSNOTAVAILABLE;
}
