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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Cube.Mixin.Forms.Controls;
using Cube.Mixin.Generic;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// TreeViewBehavior
    ///
    /// <summary>
    /// Represents the behavior of the TreeView object for editing the
    /// context menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class TreeViewBehavior
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// TreeViewBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the TreeViewBehavior class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Target TreeView object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public TreeViewBehavior(TreeView src)
        {
            Source = src ?? throw new ArgumentException();
            Source.AllowDrop        = true;
            Source.ItemDrag        += WhenItemDrag;
            Source.DragOver        += WhenDragOver;
            Source.DragDrop        += WhenDragDrop;
            Source.BeforeLabelEdit += (s, e) => e.CancelEdit = !Editable;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Get the TreeView object to which the behavior applies.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TreeView Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// Retrieves the ContextMenu list representing the result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Context> Result => CreateResult();

        /* ----------------------------------------------------------------- */
        ///
        /// Registered
        ///
        /// <summary>
        /// Gets a value indicating whether the Register method has been
        /// executed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Registered { get; private set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// Editable
        ///
        /// <summary>
        /// Gets a value indicating whether the currently selected Node
        /// object is editable or not.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Editable =>
            Source.SelectedNode != null &&
            Source.SelectedNode != RootNode;

        /* ----------------------------------------------------------------- */
        ///
        /// RootNode
        ///
        /// <summary>
        /// Gets the root Node object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode RootNode
        {
            get
            {
                if (!Registered) throw new InvalidOperationException("unregistered");
                return Source.Nodes[0];
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Register
        ///
        /// <summary>
        /// Add the corresponding TreeNode list to the context menu for the
        /// target TreeView object.
        /// </summary>
        ///
        /// <param name="src">List of context menus to be registered.</param>
        /// <param name="images">List of display icons.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Register(IEnumerable<Context> src, IEnumerable<Image> images)
        {
            Source.ImageList = images.ToImageList();
            Source.Nodes.Clear();
            _ = Source.Nodes.Add(CreateRootNode(src));
            Registered = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Copies and adds the contents of the specified TreeNode object.
        /// </summary>
        ///
        /// <param name="src">Object to be added.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(TreeNode src)
        {
            if (src == null) return;

            var dest = GetTargetNode();
            Debug.Assert(dest != null);
            _ = dest.Nodes.Add(Copy(src));
            dest.Expand();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds a new TreeNode object. The added TreeNode object will be
        /// in a state to be renamed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Add()
        {
            var dest = GetTargetNode();
            var src  = new TreeNode
            {
                Text               = Properties.Resources.MenuNewCategory,
                ToolTipText        = Properties.Resources.MenuNewCategory,
                ImageIndex         = 0,
                SelectedImageIndex = 0,
                Tag                = new Context(),
            };

            Debug.Assert(dest != null);
            _ = dest.Nodes.Add(src);
            dest.Expand();
            Source.SelectedNode = src;
            Rename();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// Removes the currently selected TreeNode object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Remove()
        {
            if (Editable) Source.SelectedNode.Remove();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rename
        ///
        /// <summary>
        /// Invokes the renaming process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Rename() => Source.SelectedNode?.BeginEdit();

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves the currently selected TreeNode object by the specified
        /// amount.
        /// </summary>
        ///
        /// <param name="delta">Number to move.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(int delta)
        {
            if (!Editable) return;

            var src = Source.SelectedNode;
            Debug.Assert(src != null);
            var dest = src.Parent;
            Debug.Assert(dest != null);

            var index = dest.Nodes.IndexOf(Source.SelectedNode);
            if (index + delta < 0 || index + delta > dest.Nodes.Count - 1) return;

            src.Remove();
            dest.Nodes.Insert(index + delta, src);
            Source.SelectedNode = src;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Move src so that it becomes a child element of dest.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Target object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Move(TreeNode src, TreeNode dest)
        {
            if (!IsMovable(src, dest)) return;

            src.Remove();
            _ = dest.Nodes.Add(src);
            Source.SelectedNode = src;
            dest.Expand();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateResult
        ///
        /// <summary>
        /// Creates the process result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<Context> CreateResult()
        {
            var dest = new List<Context>();
            var root = RootNode;
            Debug.Assert(root != null);
            foreach (TreeNode n in root.Nodes) dest.Add(CreateMenu(n));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateMenu
        ///
        /// <summary>
        /// Creates a ContextMenu object based on the contents of TreeMode.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Context CreateMenu(TreeNode src)
        {
            var dest = GetContextMenu(src);
            dest.Name      = src.Text;
            dest.IconIndex = src.ImageIndex;
            foreach (TreeNode n in src.Nodes) dest.Children.Add(CreateMenu(n));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateRootNode
        ///
        /// <summary>
        /// Creates the root node.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode CreateRootNode(IEnumerable<Context> src)
        {
            var dest = new TreeNode
            {
                Text               = Properties.Resources.MenuTop,
                ToolTipText        = Properties.Resources.MenuTop,
                ImageIndex         = 0,
                SelectedImageIndex = 0,
            };
            dest.Nodes.Register(src);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTargetNode
        ///
        /// <summary>
        /// Retrieves the Node object to be processed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode GetTargetNode()
        {
            var dest = Source.SelectedNode ?? RootNode;
            Debug.Assert(dest != null);

            return string.IsNullOrEmpty(dest.Tag.TryCast<Context>()?.Arguments) ?
                   dest :
                   dest.Parent;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTargetNode
        ///
        /// <summary>
        /// Gets the Node object corresponding to the specified coordinates.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode GetTargetNode(int x, int y) =>
            Source.GetNodeAt(Source.PointToClient(new Point(x, y)));

        /* ----------------------------------------------------------------- */
        ///
        /// GetContextMenu
        ///
        /// <summary>
        /// Gets the ContextMenu object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Context GetContextMenu(TreeNode src)
        {
            var dest = src.Tag as Context;
            Debug.Assert(dest != null);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Copies the specified TreeNode object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private TreeNode Copy(TreeNode src)
        {
            var menu = GetContextMenu(src);
            var dest = new TreeNode
            {
                Text               = src.Text,
                ToolTipText        = src.ToolTipText,
                ImageIndex         = src.ImageIndex,
                SelectedImageIndex = src.SelectedImageIndex,
                Tag                = new Context
                {
                    Name      = menu.Name,
                    Arguments = menu.Arguments,
                    IconIndex = menu.IconIndex,
                },
            };

            foreach (TreeNode node in src.Nodes) dest.Nodes.Add(Copy(node));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsMovable
        ///
        /// <summary>
        /// Determines if it is possible to move from the src node to the
        /// dest node.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsMovable(TreeNode src, TreeNode dest)
        {
            Debug.Assert(src != null);

            if (dest == null            ||
                dest.Equals(src)        ||
                dest.Equals(src.Parent) ||
                dest.IsLeaf()
            ) return false;

            // Check if ancestor
            var node = dest;
            while (node != null && node != src) node = node.Parent;
            return node == null;
        }

        #region Drag&Drop

        /* ----------------------------------------------------------------- */
        ///
        /// WhenItemDrag
        ///
        /// <summary>
        /// Occurs when the Node object starts to be dragged.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenItemDrag(object s, ItemDragEventArgs e)
        {
            var src = e.Item.TryCast<TreeNode>();
            if (src.Text == Properties.Resources.MenuTop) return;
            Source.SelectedNode = src;
            Source.DoDragDrop(src, DragDropEffects.Move);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragOver
        ///
        /// <summary>
        /// Occurs when the mouse is over while dragging.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragOver(object s, DragEventArgs e)
        {
            var src  = e.Data.GetData(typeof(TreeNode)).TryCast<TreeNode>();
            var dest = GetTargetNode(e.X, e.Y);
            e.Effect = IsMovable(src, dest) ? DragDropEffects.Move : DragDropEffects.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDragDrop
        ///
        /// <summary>
        /// Occurs when a Node object is dropped.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDragDrop(object s, DragEventArgs e) => Move(
            e.Data.GetData(typeof(TreeNode)).TryCast<TreeNode>(),
            GetTargetNode(e.X, e.Y)
        );

        #endregion

        #endregion
    }
}
