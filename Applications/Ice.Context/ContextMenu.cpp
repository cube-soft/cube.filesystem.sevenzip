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
#include "ContextMenu.h"
#include <shlobj.h>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenu
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="handle">プロセスハンドラ</param>
/// <param name="count">DLL の参照カウンタ</param>
///
/* ------------------------------------------------------------------------- */
ContextMenu::ContextMenu(HINSTANCE handle, ULONG& count) :
    handle_(handle),
    dllCount_(count),
    objCount_(0) {
    InterlockedIncrement(reinterpret_cast<LONG*>(&dllCount_));
}

/* ------------------------------------------------------------------------- */
///
/// ~ContextMenu
/// 
/// <summary>
/// オブジェクトを破棄します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextMenu::~ContextMenu() {
    InterlockedDecrement(reinterpret_cast<LONG*>(&dllCount_));
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
STDMETHODIMP ContextMenu::QueryInterface(REFIID iid, LPVOID * obj) {
    obj = IsEqualIID(iid, IID_IUnknown) ||
          IsEqualIID(iid, IID_IContextMenu)  ? (LPVOID*)static_cast<LPCONTEXTMENU>(this)    :
          IsEqualIID(iid, IID_IShellExtInit) ? (LPVOID*)static_cast<LPSHELLEXTINIT>(this)   :
          IsEqualIID(iid, IID_IQueryInfo)    ? (LPVOID*)reinterpret_cast<IQueryInfo*>(this) : NULL;

    AddRef();
    return obj != NULL ? NOERROR : E_NOINTERFACE;
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
STDMETHODIMP_(ULONG) ContextMenu::AddRef() {
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
STDMETHODIMP_(ULONG) ContextMenu::Release() {
    ULONG count = InterlockedDecrement(reinterpret_cast<LONG*>(&objCount_));
    if (count) return count;
    delete this;
    return 0L;
}

/* ------------------------------------------------------------------------- */
///
/// Initialize
/// 
/// <summary>
/// 拡張シェルの初期化を実行します。
/// </summary>
///
/// <param name="folder">パスがフォルダかどうかを示す値</param>
/// <param name="data">データオブジェクト</param>
/// <param name="key">Key ID</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IUnknown から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenu::Initialize(LPCITEMIDLIST folder, LPDATAOBJECT data, HKEY key) {
    return S_OK;
}

/* ------------------------------------------------------------------------- */
///
/// QueryContextMenu
/// 
/// <summary>
/// コンテキストメニューに表示する項目を問い合わせます。
/// </summary>
///
/// <param name="menu">メニューハンドラ</param>
/// <param name="index">メニューのインデックス</param>
/// <param name="first">メニューの開始 ID</param>
/// <param name="last">メニューの終了 ID</param>
/// <param name="flags">フラグ</param>
///
/// <returns>HRESULT</returns>
///
/// <summary>
/// IContextMenu から継承されます。挿入されるメニューの項目 ID は
/// [first, last] 範囲内の必要があります。
/// </summary>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenu::QueryContextMenu(HMENU menu, UINT index, UINT first, UINT last, UINT flags) {
    if ((flags & CMF_DEFAULTONLY) != 0) return NO_ERROR;

    return NO_ERROR;
}

/* ------------------------------------------------------------------------- */
///
/// GetCommandString
/// 
/// <summary>
/// メニューに対応するコマンドラインを取得します。
/// </summary>
///
/// <param name="id">メニュー ID</param>
/// <param name="flags">フラグ</param>
/// <param name="reserved">予約済み領域</param>
/// <param name="buffer">コマンドラインを保存するバッファ</param>
/// <param name="size">バッファサイズ</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IContextMenu から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenu::GetCommandString(UINT_PTR id, UINT flags, UINT FAR *reserved, LPSTR buffer, UINT size) {
    return S_OK;
}

/* ------------------------------------------------------------------------- */
///
/// InvokeCommand
/// 
/// <summary>
/// コマンドラインを実行します。
/// </summary>
///
/// <param name="info">コマンドライン情報</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IContextMenu から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenu::InvokeCommand(LPCMINVOKECOMMANDINFO info) {
    return S_OK;
}

}}} // Cube::FileSystem::Ice
