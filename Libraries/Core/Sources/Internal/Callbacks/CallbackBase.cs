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

/* ------------------------------------------------------------------------- */
///
/// CallbackBase
///
/// <summary>
/// Represents the base class of other callback classes.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal abstract class CallbackBase : DisposableBase
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Progress
    ///
    /// <summary>
    /// Gets or sets the object to report the progress.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IProgress<ProgressInfo> Progress { get; init; }

    /* --------------------------------------------------------------------- */
    ///
    /// Result
    ///
    /// <summary>
    /// Gets the operation result.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipErrorCode Result { get; protected set; } = SevenZipErrorCode.OK;

    /* --------------------------------------------------------------------- */
    ///
    /// Exception
    ///
    /// <summary>
    /// Get the exceptions that occurred during processing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Exception Exception { get; private set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Gets or sets the content of the progress report.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected ProgressInfo Report { get; } = new();

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the specified callback and optionally reports the
    /// progress.
    /// </summary>
    ///
    /// <param name="callback">Callback action.</param>
    /// <param name="report">Reports or not the progress.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected void Invoke(Action callback, bool report) =>
        Invoke(() => { callback(); return true; }, report);

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the specified callback and optionally reports the
    /// progress.
    /// </summary>
    ///
    /// <param name="callback">Callback function.</param>
    /// <param name="report">Reports or not the progress.</param>
    ///
    /// <returns>Result of the callback function.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected T Invoke<T>(Func<T> callback, bool report)
    {
        try
        {
            var dest = callback();
            if (report && Result == SevenZipErrorCode.OK) Progress?.Report(Copy(Report));
            return dest;
        }
        catch (Exception err)
        {
            Result    = err is OperationCanceledException ?
                        SevenZipErrorCode.UserCancel :
                        SevenZipErrorCode.DataError;
            Exception = err;
            throw;
        }
        finally { Report.State = ProgressState.Progress; }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Copy
    ///
    /// <summary>
    /// Creates a copied instance of the Report class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private ProgressInfo Copy(ProgressInfo src) => new()
    {
        State      = src.State,
        Current    = src.Current,
        Count      = src.Count,
        Bytes      = src.Bytes,
        TotalCount = src.TotalCount,
        TotalBytes = src.TotalBytes,
    };

    #endregion
}
