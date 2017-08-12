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

#include <unknwn.h>
#include <memory>
#include <utility>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ComPtr
/// 
/// <summary>
/// COM オブジェクト用のスマートポインタを表すクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
template<class T>
class ComPtr {
public:
	ComPtr() : ptr_(nullptr) {}
	explicit ComPtr(T* ptr) : ptr_(nullptr) { Attach(ptr); }
	ComPtr(const ComPtr& cp) : ptr_(nullptr) { Reset(cp.ptr_); }
	~ComPtr() { Release(); }

    operator bool() const { return ptr_; }
    operator T*() { return ptr_; }
    T* operator->() const { return ptr_; }
   
    /* --------------------------------------------------------------------- */
    ///
    /// operator=
    /// 
    /// <summary>
    /// オブジェクトをコピーします。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    ComPtr& operator=(const ComPtr& cp)
	{
		if(this != &cp) {
			Release();
			ptr_ = cp.ptr_;
			AddRef();
		}
		return *this;
	}

    /* --------------------------------------------------------------------- */
    ///
    /// QueryInterface
    /// 
    /// <summary>
    /// COM オブジェクトの QueryInterface を実行します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    template<class U>
    ComPtr<U> QueryInterface(REFIID riid) const {
        if (!ptr_) return ComPtr<U>();

        ComPtr<U> result;
        ptr_->QueryInterface(riid, reinterpret_cast<void**>(result.GetAddress()));
        return result;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Reset
    /// 
    /// <summary>
    /// 内部ポインタをリセットします。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    void Reset(T *p = nullptr) {
        T *prev = ptr_;
        ptr_ = p;
        AddRef();
        if (prev) prev->Release();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Attach
    /// 
    /// <summary>
    /// 内部ポインタに関連付けます。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    void Attach(T *ptr) {
		Reset(ptr);
		ptr_->Release();
	}

    /* --------------------------------------------------------------------- */
    ///
    /// Detach
    /// 
    /// <summary>
    /// 内部ポインタとの関連付けを解除します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    T* Detach() {
		T* prev = ptr_;
		ptr_ = nullptr;
		return prev;
	}

    /* --------------------------------------------------------------------- */
    ///
    /// GetAddress
    /// 
    /// <summary>
    /// 内部ポインタのアドレスを取得します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    T** GetAddress() {
		Release();
		return &ptr_;
	}

private:
	void AddRef() { if (ptr_) ptr_->AddRef(); }
	void Release() { if(ptr_) { ptr_->Release(); ptr_ = nullptr; } }

    T* ptr_;
};

}}}
