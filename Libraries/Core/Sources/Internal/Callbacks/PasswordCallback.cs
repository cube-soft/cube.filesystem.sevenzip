/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip;

using System;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// PasswordCallback
///
/// <summary>
/// Provides callback functions to query the password when extracting
/// archived files.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal abstract class PasswordCallback : CallbackBase, ICryptoGetTextPassword
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// PasswordCallback
    ///
    /// <summary>
    /// Initializes a new instance of the PasswordCallback with the specified
    /// arguments.
    /// </summary>
    ///
    /// <param name="src">Source archive.</param>
    /// <param name="progress">User object to report the progress.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected PasswordCallback(string src, IProgress<Report> progress) : base(progress) => Source = src;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the path of the archive file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Gets or sets the object to query the password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IQuery<string> Password { get; init; }

    /* --------------------------------------------------------------------- */
    ///
    /// PasswordTimes
    ///
    /// <summary>
    /// Get the number of times the password was requested.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int PasswordTimes { get; private set; }

    #endregion

    #region ICryptoGetTextPassword

    /* --------------------------------------------------------------------- */
    ///
    /// CryptoGetTextPassword
    ///
    /// <summary>
    /// Gets the password of the provided archive.
    /// </summary>
    ///
    /// <param name="value">Password result.</param>
    ///
    /// <returns>Operation result</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode CryptoGetTextPassword(out string value)
    {
        PasswordTimes++;
        value = string.Empty;
        if (Password is null) return SevenZipCode.WrongPassword;

        var e = Query.NewMessage(Source);
        Password.Request(e);
        if (e.Cancel) return SevenZipCode.Cancel;

        var done = e.Value.HasValue();
        if (done) value = e.Value;
        return done ? SevenZipCode.Success : SevenZipCode.WrongPassword;
    }

    #endregion
}
