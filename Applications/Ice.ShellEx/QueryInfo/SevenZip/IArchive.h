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
#include "IStream.h"

#define ARCHIVE_INTERFACE_SUB(i, base, x) DECL_INTERFACE_SUB(i, base, 6, x)
#define ARCHIVE_INTERFACE(i, x) ARCHIVE_INTERFACE_SUB(i, IUnknown, x)

/* ------------------------------------------------------------------------- */
///
/// IProgress
/// 
/// <summary>
/// 進捗状況を通知するためのインターフェースを定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
DECL_INTERFACE(IProgress, 0, 5) {
    STDMETHOD(SetTotal)(UInt64 total) PURE;
    STDMETHOD(SetCompleted)(const UInt64 *value) PURE;
};

/* ------------------------------------------------------------------------- */
///
/// IArchiveOpenCallback
/// 
/// <summary>
/// IInArchive::Open に指定するコールバック関数のインターフェースを
/// 定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ARCHIVE_INTERFACE(IArchiveOpenCallback, 0x10) {
    STDMETHOD(SetTotal)(const UInt64* files, const UInt64* bytes) PURE;
    STDMETHOD(SetCompleted)(const UInt64* files, const UInt64* bytes) PURE;
};

/* ------------------------------------------------------------------------- */
///
/// IArchiveExtractCallback
/// 
/// <summary>
/// IInArchive::Extract に指定するコールバック関数のインターフェースを
/// 定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ARCHIVE_INTERFACE_SUB(IArchiveExtractCallback, IProgress, 0x20) {
    STDMETHOD(SetTotal)(UInt64 total) PURE;
    STDMETHOD(SetCompleted)(const UInt64 *value) PURE;
    STDMETHOD(GetStream)(UInt32 index, ISequentialOutStream** stream, Int32 mode) PURE;
    STDMETHOD(PrepareOperation)(Int32 mode) PURE;
    STDMETHOD(SetOperationResult)(Int32 result) PURE;
};

/* ------------------------------------------------------------------------- */
///
/// INTERFACE_IInArchive
/// 
/// <summary>
/// 既存の圧縮ファイルを扱うためのインターフェースを定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ARCHIVE_INTERFACE(IInArchive, 0x60) {
    STDMETHOD(Open)(IInStream* stream, const UInt64* chkpos, IArchiveOpenCallback* callback) PURE;
    STDMETHOD(Close)() PURE;
    STDMETHOD(GetNumberOfItems)(UInt32* n) PURE;
    STDMETHOD(GetProperty)(UInt32 index, PROPID pid, PROPVARIANT* value) PURE;
    STDMETHOD(Extract)(const UInt32* indices, UInt32 n, Int32 test, IArchiveExtractCallback* callback) PURE;
    STDMETHOD(GetArchiveProperty)(PROPID pid, PROPVARIANT* value) PURE;
    STDMETHOD(GetNumberOfProperties)(UInt32* n) PURE;
    STDMETHOD(GetPropertyInfo)(UInt32 index, BSTR* name, PROPID* pid, VARTYPE* vt) PURE;
    STDMETHOD(GetNumberOfArchiveProperties)(UInt32* n) PURE;
    STDMETHOD(GetArchivePropertyInfo)(UInt32 index, BSTR *name, PROPID *pid, VARTYPE *vt) PURE;
};
