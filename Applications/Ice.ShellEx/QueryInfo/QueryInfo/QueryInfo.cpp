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
#include "QueryInfo.h"
#include "Resources.h"
#include "Format.h"
#include <sstream>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// QueryInfo
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="handle">プロセスハンドラ</param>
/// <param name="count">DLL の参照カウンタ</param>
///
/* ------------------------------------------------------------------------- */
QueryInfo::QueryInfo(HINSTANCE handle, ULONG& count) :
    handle_(handle),
    dllCount_(count),
    objCount_(1),
    settings_(),
    filename_()
{
    try { Settings().Load(); }
    catch (...) { /* TODO: Logging. */ }
    InterlockedIncrement(reinterpret_cast<LONG*>(&dllCount_));
}

/* ------------------------------------------------------------------------- */
///
/// ~QueryInfo
/// 
/// <summary>
/// オブジェクトを破棄します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
QueryInfo::~QueryInfo() {
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
STDMETHODIMP_(ULONG) QueryInfo::AddRef() {
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
STDMETHODIMP_(ULONG) QueryInfo::Release() {
    ULONG count = InterlockedDecrement(reinterpret_cast<LONG*>(&objCount_));
    if (count) return count;
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
STDMETHODIMP QueryInfo::QueryInterface(REFIID iid, LPVOID * obj) {
    if (!Settings().ToolTip()) *obj = nullptr;
    else if (IsEqualIID(iid, IID_IUnknown)) *obj = static_cast<IQueryInfo*>(this);
    else if (IsEqualIID(iid, IID_IQueryInfo)) *obj = static_cast<IQueryInfo*>(this);
    else if (IsEqualIID(iid, IID_IPersistFile)) *obj = static_cast<LPPERSISTFILE>(this);
    else *obj = nullptr;

    if (*obj == nullptr) return E_NOINTERFACE;
    AddRef();
    return NOERROR;
}

/* ------------------------------------------------------------------------- */
///
/// Load
///
/// <summary>
/// ファイルが選択された場合に、そのファイルに関する情報を読み込むために
/// 実行されます。
/// </summary>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IPersistFile から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP QueryInfo::Load(LPCOLESTR filename, DWORD /* mode */) {
    FileName() = filename;
    return S_OK;
}

/* ------------------------------------------------------------------------- */
///
/// GetInfoTip
///
/// <summary>
/// 圧縮ファイルにマウスオーバされた時にツールチップに表示する情報を
/// 取得します。
/// </summary>
///
/// <param name="flags">フラグ情報</param>
/// <param name="dest">結果を格納するオブジェクト</param>
///
/// <returns>HRESULT</returns>
///
/// <remarks>IQueryInfo から継承されます。</remarks>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP QueryInfo::GetInfoTip(DWORD /* flags */, LPWSTR* dest) {
    *dest = nullptr;

    try {
        auto result = CreateInfoTip();
        if (!result.empty()) {
            *dest = static_cast<LPWSTR>(CoTaskMemAlloc((result.size() + 1) * sizeof(WCHAR)));
            wcscpy_s(*dest, result.size() + 1, result.c_str());
        }
    }
    catch(...) { /* TODO: Logging */ }

    return (*dest != nullptr) ? S_OK : E_NOT_SET;
}

/* ------------------------------------------------------------------------- */
///
/// CreateInfoTip
///
/// <summary>
/// ツールチップ用文字列を取得します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
QueryInfo::TString QueryInfo::CreateInfoTip() {
    if (!Settings().ToolTip()) return TString();

    std::basic_ostringstream<TCHAR> ss;
    PutFileInfoTip(ss);
    return ss.str();
}

/* ------------------------------------------------------------------------- */
///
/// PutFileInfoTip
///
/// <summary>
/// ファイルの基本情報を出力します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
void QueryInfo::PutFileInfoTip(TStream& ss) {
    auto kind = PathFindExtension(FileName().c_str());
    if (kind != nullptr) {
        ss << CUBEICE_TIP_KIND << _T(": ") << &kind[1] << _T(" ")
           << CUBEICE_TIP_FILE << std::endl;
    }

    auto handle = CreateFile(FileName().c_str(), 0,
        FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    if (handle == INVALID_HANDLE_VALUE) return;

    try { PutFileInfoTip(ss, handle); }
    catch (...) { OutputDebugString(_T("QueryInfo::SetFileInfoTip")); }

    CloseHandle(handle);
}

/* ------------------------------------------------------------------------- */
///
/// PutFileInfoTip
///
/// <summary>
/// ファイルサイズおよび更新日時を出力します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
void QueryInfo::PutFileInfoTip(TStream& ss, HANDLE file) {
    LARGE_INTEGER size = {};
    GetFileSizeEx(file, &size);
    ss << CUBEICE_TIP_SIZE << _T(": ") << PrettyByte(size.QuadPart) << std::endl;

    FILETIME utc, local;
    GetFileTime(file, NULL, NULL, &utc);
    FileTimeToLocalFileTime(&utc, &local);

    SYSTEMTIME tm = {};
    FileTimeToSystemTime(&local, &tm);

    TCHAR stime[32] = {};
    _stprintf_s(stime, _T("%d/%02d/%02d %d:%02d"), tm.wYear, tm.wMonth, tm.wDay, tm.wHour, tm.wMinute);
    ss << CUBEICE_TIP_LASTWRITETIME << _T(": ") << stime;
}

}}}