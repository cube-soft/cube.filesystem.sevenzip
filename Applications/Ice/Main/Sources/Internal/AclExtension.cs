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
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using System.Security.AccessControl;

/* ------------------------------------------------------------------------- */
///
/// AclExtension
///
/// <summary>
/// Provides functionality to reset ACL (Access Control List) entries on
/// files and directories so that they inherit permissions from their
/// parent directory, matching the behavior of Windows Explorer.
/// </summary>
///
/// <remarks>
/// When 7-zip extracts files to a temporary directory and those files
/// are then moved to the final destination, they retain the ACL of the
/// temporary directory (including explicit entries) instead of inheriting
/// from the destination. This helper removes explicit entries and enables
/// inheritance, replicating what Windows Explorer does when copying.
/// </remarks>
///
/* ------------------------------------------------------------------------- */
internal static class AclExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// ResetAccessControl
    ///
    /// <summary>
    /// Resets the ACL of the specified path by enabling inheritance from
    /// the parent directory.
    /// </summary>
    ///
    /// <param name="path">File or directory path to reset.</param>
    ///
    /* --------------------------------------------------------------------- */
    public static void ResetAccessControl(this string path)
    {
        try
        {
            if (Io.IsDirectory(path)) ResetDirectory(path);
            else ResetFile(path);
        }
        catch (Exception err) { Logger.Warn($"{path}: {err.Message}"); }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// ResetFile
    ///
    /// <summary>
    /// Resets the ACL of a file by enabling inheritance and removing
    /// explicit access rules.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void ResetFile(string path)
    {
        var src = new System.IO.FileInfo(path);
        var acl = src.GetAccessControl(AccessControlSections.Access);

        var rules = acl.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount));
        foreach (FileSystemAccessRule e in rules) acl.RemoveAccessRule(e);

        acl.SetAccessRuleProtection(false, false);
        src.SetAccessControl(acl);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ResetDirectory
    ///
    /// <summary>
    /// Resets the ACL of a directory by enabling inheritance and removing
    /// explicit access rules.
    /// </summary>
    /// 
    /// <remarks>
    /// In the extraction flow, this method is not invoked because
    /// destination directories are newly created via Io.CreateDirectory,
    /// which already inherits the ACL from its parent.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static void ResetDirectory(string path)
    {
        var src = new System.IO.DirectoryInfo(path);
        var acl = src.GetAccessControl(AccessControlSections.Access);

        var rules = acl.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount));
        foreach (FileSystemAccessRule e in rules) acl.RemoveAccessRule(e);

        acl.SetAccessRuleProtection(false, false);
        src.SetAccessControl(acl);
    }

    #endregion
}
