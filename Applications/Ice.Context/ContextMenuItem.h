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

#include <tchar.h>
#include <string>
#include <vector>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenuItem
/// 
/// <summary>
/// コンテキストメニューの一項目を表すクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class ContextMenuItem {
public:
    typedef std::basic_string<TCHAR> TString;
    typedef std::vector<ContextMenuItem> ContextMenuVector;

    ContextMenuItem() = delete;
    ContextMenuItem(const TString&, const TString&, const TString&, const TString&);
    ContextMenuItem(const TString&, const TString&);
    ContextMenuItem(const ContextMenuItem&);
    virtual ~ContextMenuItem() noexcept {}

    const int& Index() const { return index_; }
    const TString& DisplayName() const { return name_; }
    const TString& FileName() const { return filename_; }
    const TString& Arguments() const { return arguments_; }
    const TString& IconLocation() const { return icon_; }
    const ContextMenuVector& Children() const { return children_; }

    int& Index() { return index_; }
    TString& DisplayName() { return name_; }
    TString& FileName() { return filename_; }
    TString& Arguments() { return arguments_; }
    TString& IconLocation() { return icon_; }
    ContextMenuVector& Children() { return children_; }

    friend bool operator==(const ContextMenuItem& lhs, const ContextMenuItem& rhs) { return lhs.index_ == rhs.index_; }
    friend bool operator!=(const ContextMenuItem& lhs, const ContextMenuItem& rhs) { return lhs.index_ != rhs.index_; }
    friend bool operator< (const ContextMenuItem& lhs, const ContextMenuItem& rhs) { return lhs.index_ <  rhs.index_; }
    friend bool operator<=(const ContextMenuItem& lhs, const ContextMenuItem& rhs) { return lhs.index_ <= rhs.index_; }
    friend bool operator> (const ContextMenuItem& lhs, const ContextMenuItem& rhs) { return lhs.index_ >  rhs.index_; }
    friend bool operator>=(const ContextMenuItem& lhs, const ContextMenuItem& rhs) { return lhs.index_ >= rhs.index_; }

private:
    int index_;
    TString name_;
    TString filename_;
    TString arguments_;
    TString icon_;
    ContextMenuVector children_;
};

}}}
