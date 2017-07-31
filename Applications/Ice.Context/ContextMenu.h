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

#include <shobjidl.h>

namespace Cube {
namespace FileSystem {
namespace Ice {
    /* --------------------------------------------------------------------- */
    ///
    /// ContextMenu
    /// 
    /// <summary>
    /// CubeICE に関するコンテキストメニューを表示するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class ContextMenu : public IContextMenu3, IShellExtInit {
    public:
        ContextMenu() = delete;
        ContextMenu(const ContextMenu& cp) = delete;
        ContextMenu(HINSTANCE handle, ULONG& count);
        virtual ~ContextMenu();

        STDMETHOD(QueryInterface)(REFIID iid, LPVOID * obj); // IUnknown
        STDMETHOD_(ULONG, AddRef)(void); // IUnknown
        STDMETHOD_(ULONG, Release)(void); // IUnknown
        STDMETHODIMP Initialize(LPCITEMIDLIST, LPDATAOBJECT, HKEY); // IShellExtInit
        STDMETHODIMP GetCommandString(UINT_PTR, UINT, UINT*, LPSTR, UINT); // IContextMenu
        STDMETHODIMP InvokeCommand(LPCMINVOKECOMMANDINFO); // IContextMenu
        STDMETHODIMP QueryContextMenu(HMENU, UINT, UINT, UINT, UINT); // IContextMenu
        STDMETHODIMP HandleMenuMsg(UINT, WPARAM, LPARAM) { return S_OK; } // IContextMenu2
        STDMETHODIMP HandleMenuMsg2(UINT, WPARAM, LPARAM, LRESULT*) { return S_OK; } // IContextMenu3

    private:
        HINSTANCE handle_;
        ULONG& dllCount_;
        ULONG objCount_;
    };
}}} // Cube::FileSystem::Ice
