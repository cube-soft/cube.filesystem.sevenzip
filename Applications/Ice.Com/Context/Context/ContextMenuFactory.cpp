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
#include "ContextMenuFactory.h"
#include "ContextMenu.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenuFactory
///
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="handle">プロセスハンドラ</param>
/// <param name="count">DLL の参照カウンタ</param>
///
/* ------------------------------------------------------------------------- */
ContextMenuFactory::ContextMenuFactory(HINSTANCE handle, ULONG& count) :
    handle_(handle), dllCount_(count), objCount_(1) {
    InterlockedIncrement(reinterpret_cast<LONG*>(&dllCount_));
}

/* ------------------------------------------------------------------------- */
///
/// ~ContextMenuFactory
///
/// <summary>
/// オブジェクトを破棄します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextMenuFactory::~ContextMenuFactory() {
    InterlockedDecrement(reinterpret_cast<LONG*>(&dllCount_));
}

/* ------------------------------------------------------------------------- */
///
/// AddRef
///
/// <summary>
/// オブジェクトへの参照を追加します。
/// </summary>
///
/// <returns>現在の参照カウント</returns>
///
/// <remarks>IUnknown から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP_(ULONG) ContextMenuFactory::AddRef() {
    return InterlockedIncrement(reinterpret_cast<LONG*>(&objCount_));
}

/* ------------------------------------------------------------------------- */
///
/// Release
///
/// <summary>
/// オブジェクトを開放します。
/// </summary>
///
/// <returns>現在の参照カウント</returns>
///
/// <remarks>IUnknown から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP_(ULONG) ContextMenuFactory::Release() {
    ULONG count = InterlockedDecrement(reinterpret_cast<LONG*>(&objCount_));
    if (count > 0) return count;
    delete this;
    return 0L;
}

/* ------------------------------------------------------------------------- */
///
/// QueryInterface
///
/// <summary>
/// 使用するオブジェクトを問い合わせます。
/// </summary>
///
/// <param name="iid">インターフェイスを識別するための GUID</param>
/// <param name="obj">生成オブジェクト</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IUnknown から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenuFactory::QueryInterface(REFIID iid, LPVOID * obj) {
    if (IsEqualIID(iid, IID_IUnknown)) *obj = static_cast<LPCLASSFACTORY>(this);
    else if (IsEqualIID(iid, IID_IClassFactory)) *obj = static_cast<LPCLASSFACTORY>(this);
    else *obj = nullptr;

    if (*obj == nullptr) E_NOINTERFACE;
    AddRef();
    return NOERROR;
}

/* ------------------------------------------------------------------------- */
///
/// CreateInstance
///
/// <summary>
/// オブジェクトを生成します。
/// </summary>
///
/// <param name="unknown">ハンドラ</param>
/// <param name="iid">インターフェイスを識別するための GUID</param>
/// <param name="obj">生成オブジェクト</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IClassFactory から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenuFactory::CreateInstance(LPUNKNOWN unknown, REFIID iid, LPVOID FAR *obj) {
    *obj = nullptr;

    if (unknown) return CLASS_E_NOAGGREGATION;

	OSVERSIONINFO osvi = {};
	osvi.dwOSVersionInfoSize = sizeof(osvi);
	GetVersionEx(&osvi);

    auto icon = osvi.dwMajorVersion >= 6 ? new ContextMenuIcon() : nullptr;
    auto menu = new ContextMenu(handle_, dllCount_, icon);
    if (!menu) return E_OUTOFMEMORY;

    auto result = menu->QueryInterface(iid, obj);
    menu->Release();

    return result;
}

}}} // Cube::FileSystem::Ice
