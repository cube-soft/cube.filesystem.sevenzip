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

#include "ContextMenuIcon.h"
#include "ContextMenuItem.h"
#include "ContextSettings.h"
#include "../Resources.h"
#include <shobjidl.h>
#include <tchar.h>
#include <map>
#include <memory>
#include <string>
#include <vector>

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ContextMenu
/// 
/// <summary>
/// CubeICE に関するコンテキストメニューを表示するためのクラスです。
/// </summary>
///
/* ------------------------------------------------------------------------- */
class ContextMenu : public IContextMenu3, IShellExtInit {
public:
    typedef std::basic_string<TCHAR> TString;

    ContextMenu() = delete;
    ContextMenu(const ContextMenu&) = delete;
    ContextMenu(HINSTANCE, ULONG&, ContextMenuIcon*);
    virtual ~ContextMenu();

    TString CurrentDirectory() const;
    TString Program() const { return CurrentDirectory() + _T("\\") CUBEICE_PROGRAM; }

    const ContextSettings& Settings() const { return settings_; }
    const std::map<int, ContextMenuItem>& Items() const { return items_; }
    const std::vector<TString>& Files() const { return files_; }

    ContextSettings& Settings() { return settings_; }
    std::map<int, ContextMenuItem>& Items() { return items_; }
    std::vector<TString>& Files() { return files_; }

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
    bool Insert(ContextMenuItem&, HMENU, UINT&, UINT&, UINT);
    bool Insert(ContextMenuItem::ContextMenuVector&, HMENU, UINT&, UINT&, UINT);
    void UpdateStyle(HMENU);

    HINSTANCE handle_;
    ULONG& dllCount_;
    ULONG objCount_;
    ContextSettings settings_;
    std::unique_ptr<ContextMenuIcon> icon_;
    std::map<int, ContextMenuItem> items_;
    std::vector<TString> files_;
};

}}} // Cube::FileSystem::Ice
