/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip.Ice.Settings;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cube.Forms.Controls.Extensions;

/* ------------------------------------------------------------------------- */
///
/// CustomizeMenu
///
/// <summary>
/// Represents the context menu for the CustomizeWindow class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class CustomizeMenu : ContextMenuStrip
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// CustomizeMenu
    ///
    /// <summary>
    /// Initializes a new instance of the CustomizeMenu class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">List of menu items that can be added.</param>
    /// <param name="dest">Current menu.</param>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenu(TreeView src, TreeView dest)
    {
        _core  = new TreeViewBehavior(dest);
        Source = src;
        Target = dest;

        InitializeShortcutKeys();
        InitializeEvents();
        InitializeMenu();

        Target.ContextMenuStrip = this;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Get a TreeView object that represents a list of menus that can
    /// be added.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public TreeView Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Target
    ///
    /// <summary>
    /// Get a TreeView object that represents the current menu list.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public TreeView Target { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Editable
    ///
    /// <summary>
    /// Gets a value indicating whether the currently selected Node
    /// object is editable or not.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Editable => _core.Editable;

    /* --------------------------------------------------------------------- */
    ///
    /// Registered
    ///
    /// <summary>
    /// Gets a value indicating whether data has been registered or not.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Registered => _core.Registered;

    /* --------------------------------------------------------------------- */
    ///
    /// Result
    ///
    /// <summary>
    /// Get the result of editing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<Context> Result => _core.Result;

    /* --------------------------------------------------------------------- */
    ///
    /// AddMenu
    ///
    /// <summary>
    /// Get the Add menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenuItem AddMenu { get; } = new(Properties.Resources.MenuAdd);

    /* --------------------------------------------------------------------- */
    ///
    /// RemoveMenu
    ///
    /// <summary>
    /// Gets the Remove menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenuItem RemoveMenu { get; } = new(Properties.Resources.MenuRemove);

    /* --------------------------------------------------------------------- */
    ///
    /// RenameMenu
    ///
    /// <summary>
    /// Gets the Rename menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenuItem RenameMenu { get; } = new(Properties.Resources.MenuRename);

    /* --------------------------------------------------------------------- */
    ///
    /// UpMenu
    ///
    /// <summary>
    /// Gets the Up menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenuItem UpMenu { get; } = new(Properties.Resources.MenuUp);

    /* --------------------------------------------------------------------- */
    ///
    /// DownMenu
    ///
    /// <summary>
    /// Gets the Down menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenuItem DownMenu { get; } = new(Properties.Resources.MenuDown);

    /* --------------------------------------------------------------------- */
    ///
    /// NewCategoryMenu
    ///
    /// <summary>
    /// Gets the NewCategory menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CustomizeMenuItem NewCategoryMenu { get; } = new(Properties.Resources.MenuNewCategory);

    #endregion

    #region Events

    /* --------------------------------------------------------------------- */
    ///
    /// Updated
    ///
    /// <summary>
    /// Occurs when the state is updated.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public event EventHandler Updated;

    /* --------------------------------------------------------------------- */
    ///
    /// OnUpdated
    ///
    /// <summary>
    /// Raises the Updated event.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected virtual void OnUpdated(EventArgs e) => Updated?.Invoke(this, e);

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Register
    ///
    /// <summary>
    /// Registers the data.
    /// </summary>
    ///
    /// <param name="src">List of menu items that can be added.</param>
    /// <param name="dest">Current menu.</param>
    /// <param name="images">Image list to be used the class.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Register(IEnumerable<Context> src, IEnumerable<Context> dest, IEnumerable<Image> images)
    {
        Source.ImageList = images.ToImageList();
        Source.Nodes.Register(src);
        _core.Register(dest, images);
        if (Source.Nodes.Count > 0) Source.SelectedNode = Source.Nodes[0];
        Target.SelectedNode = Target.Nodes[0];
        RaiseUpdated();
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// InitializeEvents
    ///
    /// <summary>
    /// Initializes the events.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InitializeEvents()
    {
        AddMenu.Click         += (s, e) => _core.Add(Source.SelectedNode);
        RemoveMenu.Click      += (s, e) => _core.Remove();
        RenameMenu.Click      += (s, e) => _core.Rename();
        UpMenu.Click          += (s, e) => _core.Move(-1);
        DownMenu.Click        += (s, e) => _core.Move(1);
        NewCategoryMenu.Click += (s, e) => _core.Add();
        Target.NodeMouseClick += (s, e) => Target.SelectedNode = e.Node;
        Target.AfterSelect    += (s, e) => RaiseUpdated();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InitializeShortcutKeys
    ///
    /// <summary>
    /// Initializes the shortcut keys.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InitializeShortcutKeys()
    {
        AddMenu.ShortcutKeys         = Keys.Control | Keys.V;
        RemoveMenu.ShortcutKeys      = Keys.Control | Keys.D;
        RenameMenu.ShortcutKeys      = Keys.Control | Keys.R;
        UpMenu.ShortcutKeys          = Keys.Control | Keys.Up;
        DownMenu.ShortcutKeys        = Keys.Control | Keys.Down;
        NewCategoryMenu.ShortcutKeys = Keys.Control | Keys.N;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InitializeMenu
    ///
    /// <summary>
    /// Initializes the menus.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InitializeMenu() => Items.AddRange(new ToolStripItem[]
    {
        AddMenu,
        NewCategoryMenu,
        new ToolStripSeparator(),
        UpMenu,
        DownMenu,
        new ToolStripSeparator(),
        RemoveMenu,
        RenameMenu
    });

    /* --------------------------------------------------------------------- */
    ///
    /// RaiseUpdated
    ///
    /// <summary>
    /// Raises the Update event.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void RaiseUpdated()
    {
        RenameMenu.Enabled = Editable;
        RemoveMenu.Enabled = Editable;
        UpMenu.Enabled     = Editable;
        DownMenu.Enabled   = Editable;

        OnUpdated(EventArgs.Empty);
    }

    #endregion

    #region Fields
    private readonly TreeViewBehavior _core;
    #endregion
}
