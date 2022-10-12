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
using System.Linq;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// AssociateRegistrarTest
///
/// <summary>
/// Tests the AssociateRegistrar class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class AssociateRegistrarTest : FileFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Command
    ///
    /// <summary>
    /// Tests the constructor and confirms the Command property.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Command()
    {
        var path = "C:\\Program Files\\CubeICE\\cubeice.exe";
        var cmd  = "\"C:\\Program Files\\CubeICE\\cubeice.exe\" \"%1\"";
        var src  = new AssociateRegistrar(path)
        {
            IconLocation = path,
            ToolTip      = false,
        };
        Assert.That(src.Command, Is.EqualTo(cmd));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Update
    ///
    /// <summary>
    /// Tests the Update method.
    /// </summary>
    ///
    /// <remarks>
    /// The Update method will try to modify the subkeys under
    /// HKEY_CLASSES_ROOT, and the operation will fail with normal access
    /// privileges. If run with administrator privileges, the CubeICE will
    /// unassociate all corresponding extensions.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Update()
    {
        try
        {
            var settings  = new SettingFolder();
            var path      = @"C:\Program Files\CubeICE\cubeice.exe";
            var registrar = new AssociateRegistrar(path)
            {
                IconLocation = $"{path},{settings.Value.Associate.IconIndex}",
                ToolTip = false,
            };

            registrar.Update(settings.Value.Associate.Value);

            foreach (var key in settings.Value.Associate.Value.Keys.ToArray())
            {
                settings.Value.Associate.Value[key] = false;
            }
            registrar.Update(settings.Value.Associate.Value);

            Assert.Pass("Administrator");
        }
        catch (UnauthorizedAccessException err) { Assert.Ignore(err.Message); }
    }

    #endregion
}
