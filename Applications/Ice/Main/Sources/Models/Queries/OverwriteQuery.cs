using System;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteMethod
    ///
    /// <summary>
    /// Specifies the method to overwrite a file or directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum OverwriteMethod
    {
        /// <summary>Ask the user</summary>
        Query = 0x000,
        /// <summary>Cancel</summary>
        Cancel = 0x002,
        /// <summary>Yes</summary>
        Yes = 0x006,
        /// <summary>No</summary>
        No = 0x007,
        /// <summary>Rename instead of overwriting</summary>
        Rename = 0x010,
        /// <summary>Mask for operations</summary>
        Operations = 0x01f,

        /// <summary>Same as the previous action</summary>
        Always = 0x100,
        /// <summary>Always yes</summary>
        AlwaysYes = Always | Yes,
        /// <summary>Always no</summary>
        AlwaysNo = Always | No,
        /// <summary>Always rename</summary>
        AlwaysRename = Always | Rename,
    }
}
