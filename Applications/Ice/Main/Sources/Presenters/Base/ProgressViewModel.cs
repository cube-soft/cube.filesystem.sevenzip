﻿/* ------------------------------------------------------------------------- */
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
using System.Threading;
using Cube.Observable.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ProgressViewModel
///
/// <summary>
/// Represents the ViewModel for the ProgressWindow.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public abstract class ProgressViewModel : PresentableBase<ProgressFacade>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ProgressViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the ProgressViewModel class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Model object.</param>
    /// <param name="aggregator">Message aggregator.</param>
    /// <param name="context">Synchronization context.</param>
    ///
    /* --------------------------------------------------------------------- */
    protected ProgressViewModel(ProgressFacade src, Aggregator aggregator, SynchronizationContext context) :
        base(src, aggregator, context)
    {
        Assets.Add(Facade.Subscribe(e =>  {
            var name = e == nameof(Facade.Report) ? nameof(Title) : e;
            Refresh(name);
        }));

        src.Error = e => {
            var m = Message.Error(e);
            Send(m);
            e.Cancel = m.Cancel;
        };
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Title
    ///
    /// <summary>
    /// Gets the title of the window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Title => GetTitle();

    /* --------------------------------------------------------------------- */
    ///
    /// Text
    ///
    /// <summary>
    /// Gets the text displayed in the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Text => GetText();

    /* --------------------------------------------------------------------- */
    ///
    /// Value
    ///
    /// <summary>
    /// Gets the current value of the progress bar.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Value
    {
        get
        {
            var src = Math.Max((int)(Facade.Report.GetRatio() * Unit), 1);
            var cmp = Get<int>();
            if (src > cmp) { _ = Set(src); return src; }
            else return cmp;
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Unit
    ///
    /// <summary>
    /// Gets the maximum value of the progress bar.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Unit => 1000;

    /* --------------------------------------------------------------------- */
    ///
    /// State
    ///
    /// <summary>
    /// Gets the current state;
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public TimerState State => Facade.State;

    /* --------------------------------------------------------------------- */
    ///
    /// Cancelable
    ///
    /// <summary>
    /// Gets a value indicating whether the current process can be
    /// canceled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Cancelable => Facade.Report.TotalCount > 0;

    /* --------------------------------------------------------------------- */
    ///
    /// Suspended
    ///
    /// <summary>
    /// Gets a value indicating whether the current process is suspended.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Suspended => State == TimerState.Suspend;

    /* --------------------------------------------------------------------- */
    ///
    /// Elapsed
    ///
    /// <summary>
    /// Gets the display string of the elapsed time.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Elapsed => string.Format("{0} : {1}",
        Properties.Resources.MessageElapseTime,
        Resource.GetTimeString(Facade.Elapsed)
    );

    /* --------------------------------------------------------------------- */
    ///
    /// Remaining
    ///
    /// <summary>
    /// Gets the display string of the remaining time.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Remaining => string.Format("{0} : {1}",
        Properties.Resources.MessageRemainTime,
        Resource.GetTimeString(Facade.Remaining)
    );

    /* --------------------------------------------------------------------- */
    ///
    /// Count
    ///
    /// <summary>
    /// Gets the string that represents the number of compressing or
    /// extracting files or directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Count => string.Format("{0} : {1:#,0} / {2:#,0}",
        Properties.Resources.MessageCount,
        Facade.Report.Count,
        Facade.Report.TotalCount
    );

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Start
    ///
    /// <summary>
    /// Starts the main operation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Start() => Quit(Facade.Start, false);

    /* --------------------------------------------------------------------- */
    ///
    /// SuspendOrResume
    ///
    /// <summary>
    /// Invokes the Suspend or Resume method according to the current
    /// state.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void SuspendOrResume() => Run(Facade.SuspendOrResume, true);

    /* --------------------------------------------------------------------- */
    ///
    /// GetTitle
    ///
    /// <summary>
    /// Gets the title of the window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected abstract string GetTitle();

    /* --------------------------------------------------------------------- */
    ///
    /// GetText
    ///
    /// <summary>
    /// Gets the text displayed in the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected abstract string GetText();

    /* --------------------------------------------------------------------- */
    ///
    /// OnMessage
    ///
    /// <summary>
    /// Converts the specified exception to a new instance of the
    /// DialogMessage class.
    /// </summary>
    ///
    /// <param name="src">Source exception.</param>
    ///
    /// <returns>DialogMessage object.</returns>
    ///
    /// <remarks>
    /// The Method is called from the Track methods.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    protected override DialogMessage OnMessage(Exception src) =>
        src is OperationCanceledException ? null : Message.From(src);

    #endregion
}
