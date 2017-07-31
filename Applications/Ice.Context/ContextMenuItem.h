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
#include <map>

namespace Cube {
namespace FileSystem {
namespace Ice {
    /* --------------------------------------------------------------------- */
    ///
    /// ContextMenuItem
    /// 
    /// <summary>
    /// コンテキストメニューの一項目を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class ContextMenuItem {
    public:
        typedef std::basic_string<TCHAR> TString;
        typedef std::vector<ContextMenuItem> ContextMenuVector;

        ContextMenuItem() = delete;
        ContextMenuItem(int, const TString&, const TString&, const TString&, const TString&);
        ContextMenuItem(int, const TString&, const TString&);
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

    /* --------------------------------------------------------------------- */
    ///
    /// PresetMenu
    /// 
    /// <summary>
    /// 予め定義されたメニューを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    namespace PresetMenu {
        enum {
            Archive             = 0x00000001,
            Extract             = 0x00000002,
            Settings            = 0x00000004,
            Mail                = 0x00000008,

            ExtractSource       = 0x00000010,
            ExtractDesktop      = 0x00000020,
            ExtractRuntime      = 0x00000040,
            ExtractMyDocuments  = 0x00000080,

            ArchiveZip          = 0x00000100,
            ArchiveZipPassword  = 0x00000200,
            ArchiveSevenZip     = 0x00000400,
            ArchiveBZip2        = 0x00000800,
            ArchiveGZip         = 0x00001000,
            ArchiveDetail       = 0x00002000,
            ArchiveSfx          = 0x00004000,

            MailZip             = 0x00010000,
            MailZipPassword     = 0x00020000,
            MailSevenZip        = 0x00040000,
            MailBZip2           = 0x00080000,
            MailGZip            = 0x00100000,
            MailDetail          = 0x00200000,
            MailSfx             = 0x00400000,
        };
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetContextMenuItem
    /// 
    /// <summary>
    /// PresetMenu に対応する ContextMenuItem オブジェクトを取得します。
    /// </summary>
    ///
    /// <param name="preset">PresetMenu ID</param>
    /// <param name="index">表示インデックス</param>
    /// <param name="exe">実行プログラム</param>
    ///
    /// <returns>コンテキストメニュー項目</returns>
    ///
    /* --------------------------------------------------------------------- */
    ContextMenuItem GetContextMenuItem(int preset, int index, const std::basic_string<TCHAR>& exe);
}}}
