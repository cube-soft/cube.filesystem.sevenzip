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

using System.Collections.Generic;
using System.Windows.Forms;
using Cube.Generics.Extensions;

/* ------------------------------------------------------------------------- */
///
/// TreeViewExtension
///
/// <summary>
/// Provides extended methods of the TreeView class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class TreeViewExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Register
    ///
    /// <summary>
    /// Register the specified context menus to the specified
    /// TreeNodeCollection object.
    /// </summary>
    ///
    /// <param name="src">Object to be registered.</param>
    /// <param name="menu">List of menus to be registered.</param>
    ///
    /// <remarks>
    /// Arguments of Node objects with child elements are ignored.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static void Register(this TreeNodeCollection src, IEnumerable<Context> menu)
    {
        foreach (var item in menu)
        {
            var tag = new Context
            {
                Name      = item.Name,
                Arguments = string.Empty,
                IconIndex = item.IconIndex,
            };

            var node = new TreeNode
            {
                Text               = item.Name,
                ToolTipText        = item.Name,
                ImageIndex         = item.IconIndex,
                SelectedImageIndex = item.IconIndex,
                Tag                = tag,
            };

            node.Nodes.Register(item.Children);
            if (node.Nodes.Count == 0) tag.Arguments = item.Arguments;
            _ = src.Add(node);
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsLeaf
    ///
    /// <summary>
    /// Determines if it is a leaf object.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /// <returns>true for the leaf object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static bool IsLeaf(this TreeNode src) =>
        !string.IsNullOrEmpty(src.Tag.TryCast<Context>()?.Arguments);

    #endregion
}
