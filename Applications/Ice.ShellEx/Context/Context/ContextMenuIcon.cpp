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
#include "Log.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenuIcon
/// 
/// <summary>
/// オブジェクトを初期化します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextMenuIcon::ContextMenuIcon() :
    ux_(nullptr),
    map_(),
    fnGet_(nullptr),
    fnBegin_(nullptr),
    fnEnd_(nullptr)
{
    try {
        ux_      = LoadLibrary(_T("UXTHEME.DLL"));
        fnGet_   = reinterpret_cast<FnGetBufferedPaintBits>(GetProcAddress(ux_, "GetBufferedPaintBits"));
        fnBegin_ = reinterpret_cast<FnBeginBufferedPaint>(GetProcAddress(ux_, "BeginBufferedPaint"));
        fnEnd_   = reinterpret_cast<FnEndBufferedPaint>(GetProcAddress(ux_, "EndBufferedPaint"));
    }
    catch (...) { CUBE_LOG << _T("LoadLibrary error"); }
}

/* ------------------------------------------------------------------------- */
///
/// ~ContextMenuIcon
/// 
/// <summary>
/// オブジェクトを破棄します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
ContextMenuIcon::~ContextMenuIcon() {
    for (auto& kv : map_) {
        if (kv.second) DeleteObject(kv.second);
    }
    map_.clear();

    if (ux_ != nullptr) FreeLibrary(ux_);
    ux_      = nullptr;
    fnGet_   = nullptr;
    fnBegin_ = nullptr;
    fnEnd_   = nullptr;
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
void ContextMenuIcon::SetMenuIcon(const TString& src, MENUITEMINFO& dest) {
    if (!ux_ || !fnGet_ || !fnBegin_ || !fnEnd_ || src.empty()) return;

    auto bmp = CreateBitmap(src);
    if (bmp == nullptr) return;

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
HBITMAP ContextMenuIcon::CreateBitmap(const TString& src) {
    auto pos = map_.find(src);
    if (pos != map_.end()) return pos->second;

    auto kv = Split(src);

    HICON hicon;
    if (ExtractIconEx(kv.first.c_str(), kv.second, nullptr, &hicon, 1) < 1) return nullptr;

    auto hdc  = CreateCompatibleDC(nullptr);
    auto dest = CreateBitmap(hdc, hicon, SIZE{ 16, 16 });
    if (dest != nullptr) map_.insert(std::make_pair(src, dest));

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
HBITMAP ContextMenuIcon::CreateBitmap(HDC hsrc, HICON hicon, const SIZE& size) {
    auto bmi  = CreateBitmapInfo(size);
    auto dest = CreateDIBSection(hsrc, &bmi, DIB_RGB_COLORS, nullptr, nullptr, 0);
    if (!dest) return nullptr;
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
    if (pb != nullptr) {
        auto result = DrawIconEx(hdc, 0, 0, hicon, size.cx, size.cy, 0, nullptr, DI_NORMAL);
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
BITMAPINFO ContextMenuIcon::CreateBitmapInfo(const SIZE& size) {
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
void ContextMenuIcon::ConvertToArgb(HPAINTBUFFER pb, HDC hsrc, HICON hicon, const SIZE& size)
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
void ContextMenuIcon::ConvertToArgb(HDC hsrc, HBITMAP hbmp, Argb* colors, const SIZE &size, int row)
{
    auto bmi  = CreateBitmapInfo(size);
    auto heap = GetProcessHeap();
    auto bits = HeapAlloc(heap, 0, size.cx * 4 * size.cy);
    if (bits == nullptr) return;

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
bool ContextMenuIcon::HasAlphaBit(const Argb* colors, const SIZE& size, int row) {
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
std::pair<ContextMenuIcon::TString, int> ContextMenuIcon::Split(const TString& src) {
    try {
        auto pos = src.find(_T(","));
        if (pos != TString::npos) {
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
