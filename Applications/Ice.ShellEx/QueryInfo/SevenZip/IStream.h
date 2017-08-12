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

#include "Interface.h"

#define STREAM_INTERFACE_SUB(i, base, x) DECL_INTERFACE_SUB(i, base, 3, x)
#define STREAM_INTERFACE(i, x) STREAM_INTERFACE_SUB(i, IUnknown, x)

/* ------------------------------------------------------------------------- */
///
/// ISequentialInStrea
/// 
/// <summary>
/// 読み込み用ストリームのインターフェースを定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
STREAM_INTERFACE(ISequentialInStream, 0x01) {
    STDMETHOD(Read)(void *data, UInt32 size, UInt32 *processedSize) PURE;
};

/* ------------------------------------------------------------------------- */
///
/// ISequentialOutStream
/// 
/// <summary>
/// 書き込み用ストリームのインターフェースを定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
STREAM_INTERFACE(ISequentialOutStream, 0x02) {
    STDMETHOD(Write)(const void *data, UInt32 size, UInt32 *processedSize) PURE;
};

/* ------------------------------------------------------------------------- */
///
/// IInStream
/// 
/// <summary>
/// ランダムアクセス可能な読み込み用ストリームのインターフェースを
/// 定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
STREAM_INTERFACE_SUB(IInStream, ISequentialInStream, 0x03) {
    STDMETHOD(Seek)(Int64 offset, UInt32 seekOrigin, UInt64 *newPosition) PURE;
};

/* ------------------------------------------------------------------------- */
///
/// IOutStream
/// 
/// <summary>
/// ランダムアクセス可能な書き込み用ストリームのインターフェースを
/// 定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
STREAM_INTERFACE_SUB(IOutStream, ISequentialOutStream, 0x04) {
    STDMETHOD(Seek)(Int64 offset, UInt32 seekOrigin, UInt64 *newPosition) PURE;
    STDMETHOD(SetSize)(UInt64 newSize) PURE;
};
