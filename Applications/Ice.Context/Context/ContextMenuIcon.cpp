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
ArgbContextMenuIcon::ArgbContextMenuIcon() :
    ux_(NULL),
    map_(),
    fnGet_(NULL),
    fnBegin_(NULL),
    fnEnd_(NULL)
{
    try {
        ux_      = LoadLibrary(_T("UXTHEME.DLL"));
        fnGet_   = reinterpret_cast<FnGetBufferedPaintBits>(GetProcAddress(ux_, "GetBufferedPaintBits"));
        fnBegin_ = reinterpret_cast<FnBeginBufferedPaint>(GetProcAddress(ux_, "BeginBufferedPaint"));
        fnEnd_   = reinterpret_cast<FnEndBufferedPaint>(GetProcAddress(ux_, "EndBufferedPaint"));
    }
    catch (...) { /* TODO: Logging. */ }
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
    ux_      = NULL;
    fnGet_   = NULL;
    fnBegin_ = NULL;
    fnEnd_   = NULL;
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
void ArgbContextMenuIcon::SetMenuIcon(const TString& src, MENUITEMINFO& dest) {
    if (!ux_ || !fnGet_ || !fnBegin_ || !fnEnd_ || src.empty()) return;
    auto bmp = CreateBitmap(src);
    dest.fMask   |= MIIM_BITMAP;
    dest.hbmpItem = bmp;
}

/* ------------------------------------------------------------------------- */
///
/// CreateBitmap
/// 
/// <summary>
/// HBITMAP オブジェクトを生成します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
HBITMAP ArgbContextMenuIcon::CreateBitmap(const TString& src) {
    auto pos = map_.find(src);
    if (pos != map_.end()) return pos->second;

    auto kv = Split(src);

    HICON hicon, hdummy;
    if (ExtractIconEx(kv.first.c_str(), kv.second, &hdummy, &hicon, 1) < 1) return NULL;
    DestroyIcon(hdummy);

    auto hdc  = CreateCompatibleDC(NULL);
    auto dest = CreateBitmap(hdc, hicon, SIZE{ 16, 16 });
    if (dest != NULL) map_.insert(std::make_pair(src, dest));

    DeleteDC(hdc);
    DestroyIcon(hicon);

    return dest;
}

/* ------------------------------------------------------------------------- */
///
/// CreateBitmap
/// 
/// <summary>
/// HBITMAP オブジェクトを生成します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
HBITMAP ArgbContextMenuIcon::CreateBitmap(HDC hsrc, HICON hicon, const SIZE& size) {
    auto bmi  = CreateBitmapInfo(size);
    auto dest = CreateDIBSection(hsrc, &bmi, DIB_RGB_COLORS, NULL, NULL, 0);
    if (!dest) return NULL;
    auto prev = static_cast<HBITMAP>(SelectObject(hsrc, static_cast<HGDIOBJ>(dest)));

    BLENDFUNCTION alpha = { AC_SRC_OVER, 0, 255, AC_SRC_ALPHA };
    BP_PAINTPARAMS bpp  = {};
    bpp.cbSize          = sizeof(bpp);
    bpp.dwFlags         = BPPF_ERASE;
    bpp.pBlendFunction  = &alpha;

    RECT rect;
    SetRect(&rect, 0, 0, size.cx, size.cy);
    HDC hdc;
    auto pb = fnBegin_(hsrc, &rect, BPBF_DIB, &bpp, &hdc);
    if (pb != NULL) {
        auto result = DrawIconEx(hdc, 0, 0, hicon, size.cx, size.cy, 0, NULL, DI_NORMAL);
        if (result) ConvertToArgb(pb, hsrc, hicon, size);
        fnEnd_(pb, TRUE);
    }
    SelectObject(hsrc, static_cast<HGDIOBJ>(prev));

    return dest;
}

/* ------------------------------------------------------------------------- */
///
/// BITMAPINFO
/// 
/// <summary>
/// BITMAPINFO オブジェクトを生成します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
BITMAPINFO ArgbContextMenuIcon::CreateBitmapInfo(const SIZE& size) {
    BITMAPINFO dest = {};

    dest.bmiHeader.biSize        = sizeof(dest);
    dest.bmiHeader.biPlanes      = 1;
    dest.bmiHeader.biCompression = BI_RGB;
    dest.bmiHeader.biWidth       = size.cx;
    dest.bmiHeader.biHeight      = size.cy;
    dest.bmiHeader.biBitCount    = 32;

    return dest;
}

/* ------------------------------------------------------------------------- */
///
/// ConvertToArgb
/// 
/// <summary>
/// ARGB 画像に変換します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
void ArgbContextMenuIcon::ConvertToArgb(HPAINTBUFFER pb, HDC hsrc, HICON hicon, const SIZE& size)
{
    RGBQUAD *quad;
    int row;
    if (!SUCCEEDED(fnGet_(pb, &quad, &row))) return;

    auto colors = reinterpret_cast<Argb*>(quad);
    if (HasAlphaBit(colors, size, row)) return;

    ICONINFO info = {};
    if (!GetIconInfo(hicon, &info)) return;

    if (info.hbmMask) ConvertToArgb(hsrc, info.hbmMask, colors, size, row);
    DeleteObject(info.hbmColor);
    DeleteObject(info.hbmMask);
}

/* ------------------------------------------------------------------------- */
///
/// ConvertToArgb
/// 
/// <summary>
/// ARGB 画像に変換します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
void ArgbContextMenuIcon::ConvertToArgb(HDC hsrc, HBITMAP hbmp, Argb* colors, const SIZE &size, int row)
{
    auto bmi  = CreateBitmapInfo(size);
    auto heap = GetProcessHeap();
    auto bits = HeapAlloc(heap, 0, size.cx * 4 * size.cy);
    if (bits == NULL) return;

    if (GetDIBits(hsrc, hbmp, 0, size.cy, bits, &bmi, DIB_RGB_COLORS) == size.cy) {
        auto delta = row - size.cx;
        auto mask  = static_cast<Argb*>(bits);

        for (auto y = size.cy; y > 0; --y) {
            for (ULONG x = size.cx; x > 0; --x) {
                if (*mask++) *colors++ = 0;
                else *colors++ |= 0xFF000000;
            }
            colors += delta;
        }
    }

    HeapFree(heap, 0, bits);
}

/* ------------------------------------------------------------------------- */
///
/// HasAlphaBit
/// 
/// <summary>
/// アルファ値を保持しているかどうかを判別します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
bool ArgbContextMenuIcon::HasAlphaBit(const Argb* colors, const SIZE& size, int row) {
    auto delta = row - size.cx;
    for (auto y = size.cy; y > 0; --y) {
        for (auto x = size.cx; x > 0; --x) {
            if ((*colors++ & 0xFF000000) != 0) return true;
        }
        colors += delta;
    }
    return false;
}

/* ------------------------------------------------------------------------- */
///
/// Split
/// 
/// <summary>
/// パスとインデックスに分割します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
std::pair<ArgbContextMenuIcon::TString, int> ArgbContextMenuIcon::Split(const ArgbContextMenuIcon::TString& src) {
    try {
        auto pos = src.find(_T(","));
        if (pos != ArgbContextMenuIcon::TString::npos) {
            auto path = src.substr(0, pos);
            auto s = src.substr(pos + 1);
            auto index = _ttoi(s.c_str());
            return std::make_pair(path, index);
        }
    }
    catch (...) { /* ignore */ }
    return std::make_pair(src, 0);
}

}}}
