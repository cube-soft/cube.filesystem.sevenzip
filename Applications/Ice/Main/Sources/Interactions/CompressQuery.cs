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
using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// CompressQuery
///
/// <summary>
/// Provides functionality to get the runtime settings for compression.
/// The class may query the user as needed.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class CompressQuery : Query<string, CompressQueryValue>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// CompressQuery
    ///
    /// <summary>
    /// Initializes a new instance of the CompressQuery class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="callback">Callback action for the request.</param>
    ///
    /* --------------------------------------------------------------------- */
    public CompressQuery(Action<QueryMessage<string, CompressQueryValue>> callback) :
        base(callback, Dispatcher.Vanilla) { }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the runtime settings for compression.
    /// The method may invoke the Query.Request method as needed.
    /// </summary>
    ///
    /// <param name="request">User request.</param>
    /// <param name="settings">Default compression settings.</param>
    ///
    /// <returns>Compression query value.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public CompressQueryValue Get(Request request, CompressSettingValue settings) =>
        _cache ??= GetValue(request, settings);

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetValue
    ///
    /// <summary>
    /// Creates a new instance of the CompressRuntimeSetting class
    /// with the specified arguments. The method may invoke the
    /// Query.Request method as needed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private CompressQueryValue GetValue(Request request, CompressSettingValue settings) => request.Format switch
    {
        Format.Tar      or
        Format.Zip      or
        Format.SevenZip or
        Format.Sfx         => new(settings) { Format = request.Format },
        Format.BZip2    or
        Format.GZip     or
        Format.XZ          => new(settings)
        {
            Format = Format.Tar,
            CompressionMethod = request.Format.ToMethod()
        },
        _ => Invoke(request, settings),
    };

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Asks the user to input the runtime settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private CompressQueryValue Invoke(Request request, CompressSettingValue settings)
    {
        var m = new QueryMessage<string, CompressQueryValue>(GetSource(request))
        {
            Value  = new(settings),
            Cancel = true,
        };

        if (m.Source.HasValue()) m.Value.Destination = m.Source;
        Request(m);
        if (m.Cancel) throw new OperationCanceledException();
        return m.Value;
    }


    /* --------------------------------------------------------------------- */
    ///
    /// GetSource
    ///
    /// <summary>
    /// Gets the source for query message.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetSource(Request request)
    {
        var src = request.Sources.First();
        if (src.HasValue())
        {
            var f = Io.Get(src);
            return Io.Combine(f.DirectoryName, $"{f.BaseName}.zip");
        }
        else return string.Empty;
    }

    #endregion

    #region Fields
    private CompressQueryValue _cache;
    #endregion
}
