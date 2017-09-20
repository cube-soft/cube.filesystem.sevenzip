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
#include "Stream.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// InStream
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/// <param name="path">読み込みモードで開くファイル</param>
///
/* ------------------------------------------------------------------------- */
InStream::InStream(const TString& path) :
    objCount_(0), imp_(new InStreamImp(path)) {
    AddRef();
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
STDMETHODIMP_(ULONG) InStream::AddRef() {
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
STDMETHODIMP_(ULONG) InStream::Release() {
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
STDMETHODIMP InStream::QueryInterface(REFIID iid, LPVOID* obj) {
    if (IsEqualIID(iid, IID_IUnknown)) *obj = static_cast<IInStream*>(this);
    else if (IsEqualIID(iid, IID_IInStream)) *obj = static_cast<IInStream*>(this);
    else if (IsEqualIID(iid, IID_ISequentialInStream)) *obj = static_cast<ISequentialInStream*>(this);
    else *obj = nullptr;

    if (*obj == nullptr) return E_NOINTERFACE;
    AddRef();
    return NOERROR;
}

/* ------------------------------------------------------------------------- */
///
/// Seek
/// 
/// <summary>
/// ファイル位置を移動します。
/// </summary>
///
/// <param name="offset">移動量</param>
/// <param name="method">検索位置</param>
/// <param name="newptr">新しいファイル位置</param>
///
/// <returns>成功したかどうかを示す値</returns>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP InStream::Seek(INT64 offset, UINT32 method, UINT64* newptr) {
    LARGE_INTEGER loff;
    loff.QuadPart = offset;

    LARGE_INTEGER lnew;
    auto result = imp_->Seek(loff, &lnew, method);
    if (result && newptr) *newptr = lnew.QuadPart;

    return result ? S_OK : E_FAIL;
}

/* ------------------------------------------------------------------------- */
///
/// Read
/// 
/// <summary>
/// ファイルからデータを読み込みます。
/// </summary>
///
/// <param name="data">読み込み結果を格納するオブジェクト</param>
/// <param name="size">読み込みサイズ</param>
/// <param name="proceed">実際に読み込まれたサイズ</param>
///
/// <returns>成功したかどうかを示す値</returns>
///
/* ------------------------------------------------------------------------- */
STDMETHODIMP InStream::Read(void* data, UINT32 size, UINT32* proceed) {
    if (!data) return E_FAIL;

    DWORD actual = 0;
    auto result = imp_->Read(data, size, &actual);
    if (result && proceed) *proceed = actual;
    return result ? S_OK : E_FAIL;
}

}}}