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

#include <windows.h>
#include <tchar.h>
#include <sstream>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// LogStream
/// 
/// <summary>
/// 簡易的なログ出力用ストリームを表すクラスです。
/// </summary>
///
/// <remarks>
/// LogStream はコンストラクタで内部ストリームを生成し、デストラクタで
/// OutputDebugString() を用いてデバッグ画面に出力します。
/// </remarks>
///
/* ------------------------------------------------------------------------- */
class LogStream {
public:
    typedef std::basic_string<TCHAR> TString;
    typedef std::basic_ostringstream<TCHAR> TStream;

    // delete
    LogStream() = delete;
    LogStream(const LogStream&) = delete;
    LogStream& operator=(const LogStream&) = delete;


    /* --------------------------------------------------------------------- */
    ///
    /// LogStream
    /// 
    /// <summary>
    /// オブジェクトを初期化します。
    /// </summary>
    ///
    /// <param name="prefix">ログに出力する接頭辞</param>
    /// <param name="file">対象ファイル名</param>
    /// <param name="n">対象ファイルの行番号</param>
    ///
    /* --------------------------------------------------------------------- */
    LogStream(const TString& prefix, const TString& file, int n) : s_() {
        s_ << _T("[") << prefix << _T(":") << file << _T(":") << n << _T("] ");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ~LogStream
    /// 
    /// <summary>
    /// オブジェクトを破棄します。
    /// </summary>
    ///
    /// <remarks>
    /// デストラクタ実行のタイミングで、実際にログを出力します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    ~LogStream() { OutputDebugString(s_.str().c_str()); }

    /* --------------------------------------------------------------------- */
    ///
    /// operator<<
    /// 
    /// <summary>
    /// 値をログに出力します。
    /// </summary>
    ///
    /// <param name="value">出力内容</param>
    ///
    /// <returns>自身のオブジェクト</returns>
    ///
    /// <remarks>
    /// この演算子の実行時には内部ストリームに追加され、実際にログ出力
    /// されるのはデストラクタ実行時となります。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    template <class T>
    LogStream& operator<<(const T& value) {
        s_ << value;
        return *this;
    }

private:
    std::basic_ostringstream<TCHAR> s_;
};

}}}

/* ------------------------------------------------------------------------- */
///
/// CUBE_TRACE
/// 
/// <summary>
/// ログ出力用マクロです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
#define CUBE_LOG Cube::FileSystem::Ice::LogStream(_T("Cube"), _T(__FILE__), __LINE__)
