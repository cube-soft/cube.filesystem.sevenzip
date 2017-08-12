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

#include "SevenZipInterface.h"
#include <tchar.h>
#include <windows.h>
#include <memory>
#include <string>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// InStreamImp
/// 
/// <summary>
/// ファイルの入力ストリーム用クラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class InStreamImp {
public:
    typedef std::basic_string<TCHAR> TString;

    InStreamImp() = delete;
    InStreamImp(const InStreamImp&) = delete;
    InStreamImp& operator=(const InStreamImp&) = delete;
    ~InStreamImp() { CloseHandle(handle_); }

    /* --------------------------------------------------------------------- */
    ///
    /// InStreamImp
    /// 
    /// <summary>
    /// オブジェクトを初期化します。
    /// </summary>
    ///
    /// <param name="path">読み込みモードで開くファイル</param>
    ///
    /* --------------------------------------------------------------------- */
    InStreamImp(const TString& path) : handle_(nullptr) {
        handle_ = CreateFile(path.c_str(), GENERIC_READ, FILE_SHARE_READ,
            nullptr, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, nullptr);
        if (handle_ == INVALID_HANDLE_VALUE) throw std::runtime_error("Open");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Seek
    /// 
    /// <summary>
    /// ファイル位置を移動します。
    /// </summary>
    ///
    /// <param name="offset">移動量</param>
    /// <param name="newptr">新しいファイル位置</param>
    /// <param name="method">検索位置</param>
    ///
    /// <returns>成功したかどうかを示す値</returns>
    ///
    /* --------------------------------------------------------------------- */
    bool Seek(LARGE_INTEGER offset, PLARGE_INTEGER newptr, DWORD method) {
        return SetFilePointerEx(handle_, offset, newptr, method) != FALSE;
    }

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    bool Read(void* data, DWORD size, DWORD* proceed) {
        return ReadFile(handle_, data, size, proceed, nullptr) != FALSE;
    }

private:
    HANDLE handle_;
};

/* ------------------------------------------------------------------------- */
///
/// InStream
/// 
/// <summary>
/// IInStream の実装クラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class InStream : public IInStream {
public:
    typedef std::basic_string<TCHAR> TString;

    InStream() = delete;
    InStream(const TString&);
    InStream(const InStream&) = delete;
    InStream& operator=(const InStream&) = delete;
    ~InStream() = default;

    STDMETHOD(QueryInterface)(REFIID, LPVOID*) override; // IUnknown
    STDMETHOD_(ULONG, AddRef)() override; // IUnknown
    STDMETHOD_(ULONG, Release)() override; // IUnknown
    STDMETHOD(Read)(void*, UINT32, UINT32*) override; // ISequentialInStream
    STDMETHOD(Seek)(INT64, UINT32, UINT64*) override; // IInStream

private:
    long objCount_;
    std::shared_ptr<InStreamImp> imp_;
};

}}}