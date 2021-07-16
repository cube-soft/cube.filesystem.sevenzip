using Cube.Mixin.Observing;
using System;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    static class ArchiveHelper
    {
        public static IDisposable SetPassword<T>(this ArchiveViewModel<T> src, string value)
            where T : ArchiveFacade => src.Subscribe<QueryMessage<string, string>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        public static IDisposable SetDestination<T>(this ArchiveViewModel<T> src, string value)
            where T : ArchiveFacade => src.Subscribe<QueryMessage<SelectQuerySource, string>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        public static CancellationToken GetToken<T>(this ProgressViewModel<T> src)
            where T : ProgressFacade
        {
            var dest = new CancellationTokenSource();
            _ = src.Subscribe(e => { if (e == nameof(src.Busy) && !src.Busy) dest.Cancel(); });
            return dest.Token;
        }
    }
}
