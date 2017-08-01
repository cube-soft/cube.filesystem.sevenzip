#include "ContextMenuIcon.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ArgbContextMenuIcon
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ArgbContextMenuIcon::ArgbContextMenuIcon()
    : ux_(NULL), map_() {
    ux_ = LoadLibrary(_T("UXTHEME.DLL"));
}

/* ------------------------------------------------------------------------- */
///
/// ~ArgbContextMenuIcon
/// 
/// <summary>
/// オブジェクトを破棄します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ArgbContextMenuIcon::~ArgbContextMenuIcon() {
    for (auto& kv : map_) {
        if (kv.second) DeleteObject(kv.second);
    }
    map_.clear();
    if (ux_ != NULL) FreeLibrary(ux_);
    ux_ = NULL;
}

/* ------------------------------------------------------------------------- */
///
/// SetMenuIcon
/// 
/// <summary>
/// アイコンを設定します。
/// </summary>
///
/// <param name="src">アイコン用ファイルのパス</param>
/// <param name="dest">メニュー情報</param>
///
/* ------------------------------------------------------------------------- */
void ArgbContextMenuIcon::SetMenuIcon(const ArgbContextMenuIcon::TString& src, MENUITEMINFO& dest) {

}

}}}
