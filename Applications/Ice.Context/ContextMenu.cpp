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
#include <shlwapi.h>
#include <shlobj.h>
#include <sstream>
#include "Encoding.h"
#include "ContextPresetMenu.h"

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
    objCount_(1),
    settings_(),
    items_(),
    files_()
{
    try { Settings().Load(); }
    catch (...) { /* TODO: Logging. */ }
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
/// CurrentDirectory
/// 
/// <summary>
/// 実行中の DLL が存在するディレクトリのパスを取得します。
/// </summary>
///
/// <returns>ディレクトリのパス</returns>
///
/* ------------------------------------------------------------------------- */
ContextMenu::TString ContextMenu::CurrentDirectory() const {
    TCHAR dest[2048] = {};
    GetModuleFileName(handle_, dest, sizeof(dest) / sizeof(dest[0]));
    PathRemoveFileSpec(dest);
    return TString(dest);
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
///
/// <returns>HRESULT</returns>
///
/// <remarks>IUnknown から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenu::Initialize(LPCITEMIDLIST /* folder */, LPDATAOBJECT data, HKEY /* key */) {
    FORMATETC fmt = { CF_HDROP, NULL, DVASPECT_CONTENT, -1, TYMED_HGLOBAL };
    STGMEDIUM stg = {};

    auto result = data->GetData(&fmt, &stg);
    if (FAILED(result)) return result;

    auto drop = static_cast<HDROP>(GlobalLock(stg.hGlobal));
    auto count = DragQueryFile(drop, static_cast<UINT>(-1), NULL, 0);

    for (auto i = 0u; i < count; ++i) {
        auto size = DragQueryFile(drop, i, NULL, 0);
        auto buffer = new TCHAR[size + 5];
        
        DragQueryFile(drop, i, buffer, size + 3);
        Files().push_back(TString(buffer));
        delete[] buffer;
    }

    GlobalUnlock(drop);
    ReleaseStgMedium(&stg);

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
STDMETHODIMP ContextMenu::QueryContextMenu(HMENU menu, UINT index, UINT first, UINT /* last */, UINT flags) {
    if ((flags & CMF_DEFAULTONLY) != 0) return NO_ERROR;
    if (Settings().Preset() == 0) return NO_ERROR;

    InsertMenu(menu, index++, MF_BYPOSITION | MF_SEPARATOR, 0, NULL);

    auto items = GetContextMenuItems(Settings().Preset(), Program());
    auto cmdid = first;
    Insert(items, menu, index, cmdid, first);

    if (cmdid - first > 0) InsertMenu(menu, index++, MF_BYPOSITION | MF_SEPARATOR, 0, NULL);

    MENUINFO mi = {};
    mi.cbSize  = sizeof(mi);
    mi.fMask   = MIM_STYLE;
    GetMenuInfo(menu, &mi);
    mi.dwStyle = (mi.dwStyle & ~MNS_NOCHECK) | MNS_CHECKORBMP;
    mi.fMask   = MIM_STYLE | MIM_APPLYTOSUBMENUS;
    SetMenuInfo(menu, &mi);

    return MAKE_SCODE(SEVERITY_SUCCESS, FACILITY_NULL, cmdid - first);
}

/* ------------------------------------------------------------------------- */
///
/// GetCommandString
/// 
/// <summary>
/// メニューに対応するコマンドラインを取得します。
/// </summary>
///
/// <param name="index">メニュー ID</param>
/// <param name="flags">フラグ</param>
/// <param name="buffer">コマンドラインを保存するバッファ</param>
/// <param name="size">バッファサイズ</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IContextMenu から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP ContextMenu::GetCommandString(UINT_PTR index, UINT flags, UINT FAR* /* reserved */, LPSTR buffer, UINT size) {
    auto pos = Items().find(static_cast<int>(index));
    if (pos == Items().end()) return S_FALSE;

    switch (flags)
    {
    case GCS_VERBA: {
#if defined(UNOCODE) || defined(_UNICODE)
        auto s = Encoding::UnicodeToMultiByte(pos->second.DisplayName());
#else
        auto s = pos->second.DisplayName();
#endif
        strncpy_s(buffer, size, s.c_str(), _TRUNCATE);
        break;
    }
    case GCS_VERBW: {
#if defined(UNOCODE) || defined(_UNICODE)
        auto s = pos->second.DisplayName();
#else
        auto s = Encoding::MultiByteToUnicode(pos->second.DisplayName());
#endif
        wcsncpy_s(reinterpret_cast<LPWSTR>(buffer), size, s.c_str(), _TRUNCATE);
        break;
    }
    case GCS_VALIDATEA:
    case GCS_VALIDATEW:
    case GCS_HELPTEXTA:
    case GCS_HELPTEXTW:
    default:
        break;
    }

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
    if (HIWORD(info->lpVerb)) return E_INVALIDARG;

    auto index = LOWORD(info->lpVerb);
    auto pos = Items().find(index);
    if (pos == Items().end()) return E_INVALIDARG;

    std::basic_ostringstream<TCHAR> ss;
    ss << _T("\"") << Program() << _T("\"");
    if (!pos->second.Arguments().empty()) ss << _T(" ") + pos->second.Arguments();
    for (auto file : Files()) ss << _T(" \"") + file + _T("\"");

    auto cmd = new TCHAR[ss.str().size() + 5];
    try {
        _tcscpy_s(cmd, ss.str().size() + 1, ss.str().c_str());
        PROCESS_INFORMATION pi = {};
        STARTUPINFO si = {};
        si.cb = sizeof(si);
        CreateProcess(NULL, cmd, NULL, NULL, FALSE, NORMAL_PRIORITY_CLASS, NULL, NULL, &si, &pi);
        CloseHandle(pi.hThread);
        CloseHandle(pi.hProcess);
    }
    catch (...) { /* TODO: Logging. */ }
    delete[] cmd;

    return S_OK;
}

/* ------------------------------------------------------------------------- */
///
/// Insert
/// 
/// <summary>
/// メニューを挿入します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
bool ContextMenu::Insert(ContextMenuItem& src, HMENU dest, UINT& index, UINT& cmdid, UINT first) {
    if (src.FileName().empty() && src.Children().empty()) return false;

    MENUITEMINFO info = {};
    info.cbSize     = sizeof(info);
    info.fMask      = MIIM_FTYPE | MIIM_STRING | MIIM_ID;
    info.fType      = MFT_STRING;
    info.wID        = cmdid;
    info.dwTypeData = const_cast<TCHAR*>(src.DisplayName().c_str());

    auto current = cmdid;
    if (!src.Children().empty()) {
        auto tmp      = cmdid + 1;
        auto subindex = 0u;
        auto hsub     = CreateMenu();

        if (!Insert(src.Children(), hsub, subindex, tmp, first)) return false;

        cmdid = tmp;
        info.fMask   |= MIIM_SUBMENU;
        info.hSubMenu = hsub;
    }
    else ++cmdid;

    Items().insert(std::make_pair(current - first, src));
    InsertMenuItem(dest, index++, TRUE, &info);

    return true;
}

/* ------------------------------------------------------------------------- */
///
/// Insert
/// 
/// <summary>
/// メニューを挿入します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
bool ContextMenu::Insert(ContextMenuItem::ContextMenuVector& src,
    HMENU dest, UINT& index, UINT& cmdid, UINT first) {
    auto current = cmdid;
    for (auto ctx : src) Insert(ctx, dest, index, cmdid, first);
    return cmdid > current;
}

}}} // Cube::FileSystem::Ice
