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
using System.Collections.Generic;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// CallbackBase
///
/// <summary>
/// Provides functionality to report the progress of compression or
/// extraction process.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal abstract class CallbackBase : DisposableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// CallbackBase
    ///
    /// <summary>
    /// Initializes a new instance of the CallbackBase class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="aggregator">User object to report the progress.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected CallbackBase(IProgress<Report> aggregator) => _inner = aggregator;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Count
    ///
    /// <summary>
    /// Gets or sets the number of processed files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long Count { get; protected set; }

    /* --------------------------------------------------------------------- */
    ///
    /// TotalCount
    ///
    /// <summary>
    /// Gets or sets the number of files to be processed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long TotalCount { get; protected set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Bytes
    ///
    /// <summary>
    /// Gets or sets the number of processed bytes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long Bytes { get; protected set; }

    /* --------------------------------------------------------------------- */
    ///
    /// TotalBytes
    ///
    /// <summary>
    /// Gets or sets the number of bytes to be processed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long TotalBytes { get; protected set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Exceptions
    ///
    /// <summary>
    /// Gets exceptions occurred during processing.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Stack<Exception> Exceptions { get; } = new();

    #endregion

    #region Methods

    #region Report

    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Reports the progress with the specified arguments.
    /// </summary>
    ///
    /// <param name="state">Progress state.</param>
    /// <param name="entity">Processing item.</param>
    ///
    /// <returns>
    /// Cancel if the Report.Cancel property is set to true; otherwise None.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    protected SevenZipCode Report(ProgressState state, Entity entity) =>
        Report(Make(state, entity, default));

    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Reports the progress with the specified arguments.
    /// </summary>
    ///
    /// <param name="error">Exception object.</param>
    /// <param name="entity">Processing item.</param>
    ///
    /// <returns>
    /// Cancel if the Report.Cancel property is set to true; otherwise None.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    protected SevenZipCode Report(Exception error, Entity entity) =>
        Report(Make(ProgressState.Failed, entity, error));

    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Notifies the specified report.
    /// </summary>
    ///
    /// <param name="src">Report object.</param>
    ///
    /// <returns>
    /// Cancel if the Report.Cancel property is set to true; otherwise None.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    protected SevenZipCode Report(Report src)
    {
        _inner?.Report(src);
        return src.Cancel ? SevenZipCode.Cancel : SevenZipCode.Success;
    }

    #endregion

    #region Run

    /* --------------------------------------------------------------------- */
    ///
    /// Run
    ///
    /// <summary>
    /// Invokes the specified function and report the progress.
    /// </summary>
    ///
    /// <param name="func">User function.</param>
    /// <param name="state">Progress state.</param>
    /// <param name="entity">Processing item.</param>
    ///
    /// <returns>
    /// Cancel if the Report.Cancel property is set to true;
    /// otherwise the value returned by the specified function.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    protected SevenZipCode Run(Func<SevenZipCode> func, ProgressState state, Entity entity) =>
        Run(func, state, () => entity);

    /* --------------------------------------------------------------------- */
    ///
    /// Run
    ///
    /// <summary>
    /// Invokes the specified function and report the progress.
    /// </summary>
    ///
    /// <param name="func">User function.</param>
    /// <param name="state">Progress state.</param>
    /// <param name="entity">Function to get the processing item.</param>
    ///
    /// <returns>
    /// Cancel if the Report.Cancel property is set to true;
    /// otherwise the value returned by the specified function.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    protected SevenZipCode Run(Func<SevenZipCode> func, ProgressState state, Func<Entity> entity)
    {
        try
        {
            var c0 = func();
            var c1 = Report(state, entity());
            return c1 == SevenZipCode.Cancel ? c1 : c0;
        }
        catch (Exception e)
        {
            Exceptions.Push(e);
            if (_inner is not null) return Report(e, entity());
            else if (e is SevenZipException se) return se.Code;
            else return SevenZipCode.UnknownError;
        }
    }

    #endregion

    /* --------------------------------------------------------------------- */
    ///
    /// Combine
    ///
    /// <summary>
    /// Combines the specified paths
    /// </summary>
    ///
    /// <param name="s0">First path.</param>
    /// <param name="s1">Second path.</param>
    ///
    /// <returns>Combined path.</returns>
    ///
    /* --------------------------------------------------------------------- */
    protected string Combine(string s0, string s1) => !s0.HasValue() ? s1 : Io.Combine(s0, s1);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Make
    ///
    /// <summary>
    /// Creates a new report with the specified arguments.
    /// </summary>
    ///
    /// <param name="state">Progress state.</param>
    /// <param name="entity">Processing item.</param>
    /// <param name="error">Exception object.</param>
    ///
    /// <returns>Report object.</returns>
    ///
    /* --------------------------------------------------------------------- */
    private Report Make(ProgressState state, Entity entity, Exception error) => new()
    {
        Cancel     = state == ProgressState.Failed,
        State      = state,
        Exception  = error,
        Target     = entity,
        Bytes      = Bytes,
        Count      = Count,
        TotalBytes = TotalBytes,
        TotalCount = TotalCount,
    };

    #endregion

    #region Fields
    private readonly IProgress<Report> _inner;
    #endregion
}
