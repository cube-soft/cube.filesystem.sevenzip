Cube.FileSystem.SevenZip
====

[![Package](https://badgen.net/nuget/v/cube.filesystem.sevenzip)](https://www.nuget.org/packages/cube.filesystem.sevenzip/)
[![AppVeyor](https://badgen.net/appveyor/ci/clown/cube-filesystem-sevenzip)](https://ci.appveyor.com/project/clown/cube-filesystem-sevenzip)
[![Codecov](https://badgen.net/codecov/c/github/cube-soft/cube.filesystem.sevenzip)](https://codecov.io/gh/cube-soft/cube.filesystem.sevenzip)

Cube.FileSystem.SevenZip is a wrapper library of the [7-Zip](http://www.7-zip.org/) via COM interface.
The repository also has an archiving or extracting application, which name is [CubeICE](https://www.cube-soft.jp/cubeice/).
These libraries and applications are available for .NET Framework 3.5, 4.5 or later.

## Usage

You can install the library through the NuGet package.
Add a dependency in your project file using the following syntax:

    <PackageReference Include="Cube.FileSystem.SevenZip" Version="4.0.2" />

Or select it from the NuGet packages UI on Visual Studio.

### Examples for archiving files

A simple example for archiving files is as follows.
Note that the statement "using Cube.FileSystem.SevenZip;" has been omitted in all samples.

```cs
using (var writer = new ArchiveWriter(Format.Zip))
{
    writer.Add(@"path\to\file");
    writer.Add(@"path\to\directory_including_files");
    writer.Option  = new ZipOption { CompressionLevel = CompressionLevel.Ultra };
    writer.Filters = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
    
    var progress = new Progress<Report>(e => DoSomething(e));
    writer.Save(@"path\to\save.zip", "password", progress);
}
```

You create an ArchiveWriter object with an archiving format (e.g. Zip, SevenZip, ...),
add files and/or directories you want to archive, set some additional options, and finally call the Save method.
When you create Tar based archives, you can use a TarOption object for selecting a compression method.

```cs
using (var writer = new ArchiveWriter(Format.Tar))
{
    writer.Option = new TarOption
    {
        CompressionMethod = CompressionMethod.BZip2, // GZip, BZip2, XZ or Copy
        CompressionLevel  = CompressionLevel.Ultra,
    };

    writer.Add(@"path\to\file");
    writer.Add(@"path\to\directory_including_files");
    writer.Save(@"path\to\save.tar.gz");
}
```

### Examples for extracting archives

If you want to extract all files from the archive, you create an ArchiveReader object
and call the Extract method. The 2nd argument of the constructor, that means the
password of the archive, can be set string or Cube.Query&lt;string&gt; object.
The latter is mainly used for implementing the interactive mode.

```cs
// Set password directly or using Query<string>
var password = new Cube.Query<string>(e =>
{
    e.Result = "password";
    e.Cancel = false;
});

using (var reader = new ArchiveReader(@"path\to\archive", password))
{
    var filter   = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };    
    var progress = new Progress<Report>(e => DoSomething(e));
    reader.Extract(@"path\to\directory", filter, progress);
}
```

Note that ArchiveWriter and ArchiveReader classes need to execute in the same thread from constructing to destroying.
Use Task.Run() in the whole transaction if you need to archive or extract files asynchronously.

## Dependencies

* [7-Zip](https://www.7-zip.org/) ... [Cube.Native.SevenZip](https://github.com/cube-soft/Cube.Native.SevenZip) is optimized for Japanese encoding.
* [AlphaFS](https://alphafs.alphaleonis.com/)
* [NLog](https://nlog-project.org/)

## References

Cube.FileSystem.SevenZip referred to the code of the following projects to implement some functions.

* [SevenZipSharp](https://www.nuget.org/packages/SevenZipSharp/)

## Contributing

1. Fork [Cube.FileSystem.SevenZip](https://github.com/cube-soft/Cube.FileSystem.SevenZip/fork) repository.
2. Create a feature branch from the master (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer some pre-released NuGet packages. Try the [rake clean](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/Rakefile) command when build errors occur.
3. Commit your changes.
4. Rebase your local changes against the master branch.
5. Run the dotnet test command or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create new Pull Request.

## License
 
Copyright © 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)
The project is licensed under the [Apache 2.0](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/License.txt).
Note that trade names, trademarks, service marks, or logo images distributed in CubeSoft applications are not allowed to reuse or modify all or parts of them.