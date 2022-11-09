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
namespace Cube.FileSystem.SevenZip.Ice.Tests;

using System;
using System.Threading;
using System.Windows.Forms;
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// TreeViewBehaviorTest
///
/// <summary>
/// Tests the TreeViewBehavior class.
/// </summary>
///
/// <remarks>
/// TreeViewBehavior に関する多くの通常テストは ContextViewModelTest
/// クラスで各種 ViewModel を介して実行しています。
/// </remarks>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class TreeViewBehaviorTest
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Create_Throws
    ///
    /// <summary>
    /// Tests the TreeViewBehavior class with an invalid TreeView object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test, RequiresThread(ApartmentState.STA)]
    public void Create_Throws()
    {
        Assert.That(() => new TreeViewBehavior(default), Throws.ArgumentException);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Add_Throws
    ///
    /// <summary>
    /// Tests the Add method with an invalid TreeView object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test, RequiresThread(ApartmentState.STA)]
    public void Add_Throws()
    {
        Assert.That(() => new TreeViewBehavior(new TreeView()).Add(), Throws.TypeOf<InvalidOperationException>());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DragDrop
    ///
    /// <summary>
    /// Tests the drag-and-drop operation.
    /// </summary>
    ///
    /// <remarks>
    /// 実際には、DragDrop イベントで最終的に実行される Move メソッドの
    /// 動作を確認しています。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DragDrop()
    {
        var tv   = Create();
        var root = tv.Source.Nodes[0];
        var src  = root.Nodes[0].Nodes[0];
        var dest = root.Nodes[1];

        Assert.That(src.Text,  Is.EqualTo("Zip"));
        Assert.That(dest.Text, Is.EqualTo("解凍"));

        tv.Move(src, dest);
        Assert.That(root.Nodes[0].Nodes.Count,   Is.EqualTo(6));
        Assert.That(root.Nodes[0].Nodes[0].Text, Is.EqualTo("Zip (パスワード)"));
        Assert.That(root.Nodes[1].Nodes.Count,   Is.EqualTo(5));
        Assert.That(root.Nodes[1].Nodes[4].Text, Is.EqualTo("Zip"));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DragDrop_Null
    ///
    /// <summary>
    /// Tests the drag-and-drop behavior when there is no TreeNode object
    /// at the drop point.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DragDrop_Null()
    {
        var tv   = Create();
        var root = tv.Source.Nodes[0];
        tv.Move(root.Nodes[0].Nodes[0], null);

        Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
        Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DragDrop_Same
    ///
    /// <summary>
    /// Tests the behavior when drag items and drop items are the same.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DragDrop_Same()
    {
        var tv = Create();
        var root = tv.Source.Nodes[0];
        var src  = root.Nodes[0];
        tv.Move(src, src);

        Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
        Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DragDrop_Root
    ///
    /// <summary>
    /// Tests the behavior when the drag item is a root menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DragDrop_Root()
    {
        var tv   = Create();
        var root = tv.Source.Nodes[0];
        tv.Move(root, root.Nodes[0].Nodes[0]);

        Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
        Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DragDrop_Parent
    ///
    /// <summary>
    /// Tests the behavior when the drop point is its parent element.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DragDrop_Parent()
    {
        var tv   = Create();
        var root = tv.Source.Nodes[0];
        var src  = root.Nodes[0].Nodes[6];
        tv.Move(src, src.Parent);

        Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(7));
        Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DragDrop_Parent
    ///
    /// <summary>
    /// Tests the behavior when the drop point is its grandparent element.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DragDrop_Grandparent()
    {
        var tv   = Create();
        var root = tv.Source.Nodes[0];
        var src  = root.Nodes[0].Nodes[3];
        tv.Move(src, src.Parent.Parent);

        Assert.That(root.Nodes.Count,          Is.EqualTo(3));
        Assert.That(root.Nodes[0].Nodes.Count, Is.EqualTo(6));
        Assert.That(root.Nodes[1].Nodes.Count, Is.EqualTo(4));
        Assert.That(root.Nodes[2].Text,        Is.EqualTo("BZip2"));
    }

    #endregion

    #region Others

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Creates a new instance of the TreeViewBehavior class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private TreeViewBehavior Create()
    {
        var m    = Preset.DefaultContext.ToContextCollection();
        var vm   = new CustomizeViewModel(m, new(), new(), e => { });
        var dest = new TreeViewBehavior(new TreeView());

        dest.Register(vm.Current, vm.Images);
        return dest;
    }

    #endregion
}
