Cube.FileSystem.SevenZip
====

[![Package](https://badgen.net/nuget/v/cube.filesystem.sevenzip)](https://www.nuget.org/packages/cube.filesystem.sevenzip/)
[![AppVeyor](https://badgen.net/appveyor/ci/clown/cube-filesystem-sevenzip)](https://ci.appveyor.com/project/clown/cube-filesystem-sevenzip)
[![Codecov](https://badgen.net/codecov/c/github/cube-soft/cube.filesystem.sevenzip)](https://codecov.io/gh/cube-soft/cube.filesystem.sevenzip)

Cube.FileSystem.SevenZip is a wrapper library of the [7-Zip](http://www.7-zip.org/) via COM interface. The project also has an application for compressing or extracting archives, which name is [CubeICE](https://www.cube-soft.jp/cubeice/). Libraries and applications are available for .NET Framework 3.5, 4.5 or later. Note that some projects are licensed under the GNU LGPLv3 and the others under the Apache 2.0. See [License.md](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/License.md) for more information.

## Usage

You can install the library through the NuGet package.
Add dependencies in your project file or select it from the NuGet packages UI on Visual Studio.

### Examples for archiving files

A simple example for archiving files is as follows.
Note that the statement "using Cube.FileSystem.SevenZip;" has been omitted in all samples.

```cs
// Set only what you need.
var files   = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
var options = new CompressionOption
{
    CompressionLevel  = CompressionLevel.Ultra,
    CompressionMethod = CompressionMethod.Lzma,
    EncryptionMethod  = EncryptionMethod.Aes256,
    Password          = "password",
    Filter            = new Filter(files).Match,
    CodePage          = CodePage.Utf8,
};

using (var writer = new ArchiveWriter(Format.Zip, options))
{
    writer.Add(@"path\to\file");
    writer.Add(@"path\to\directory_including_files");
    
    var progress = new Progress<Report>(e => DoSomething(e));
    writer.Save(@"path\to\save.zip", progress);
}
```

You create an ArchiveWriter object with an archiving format (e.g. Zip, SevenZip, ...),
add files and/or directories you want to archive, set some additional options, and finally call the Save method.
When you create Tar based archives, you can use a TarOption object for selecting a compression method.

```cs
var options = new CompressionOption
{
    CompressionLevel  = CompressionLevel.Ultra,
    CompressionMethod = CompressionMethod.BZip2, // GZip, BZip2, XZ or Copy
};

using (var writer = new ArchiveWriter(Format.Tar, options))
{
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

// Supports only the Filter property
var files   = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
var options = new ArchiveOption
{
    Filter = new Filter(files).Match
};

using (var reader = new ArchiveReader(@"path\to\archive", password, options))
{
    var progress = new Progress<Report>(e => DoSomething(e));
    reader.Save(@"path\to\directory", progress);
}
```

Note that ArchiveWriter and ArchiveReader classes need to execute in the same thread from constructing to destroying.
Use Task.Run() in the whole transaction if you need to archive or extract files asynchronously.

## Dependencies

* [7-Zip](https://www.7-zip.org/) ... [Cube.Native.SevenZip](https://github.com/cube-soft/Cube.Native.SevenZip) is optimized for Japanese encoding.
* [AlphaFS](https://alphafs.alphaleonis.com/) ... [Cube.FileSystem.AlphaFS](https://www.nuget.org/packages/Cube.FileSystem.AlphaFS/) is a wrapper library for using AlphaFS in Cube projects.

## Contributing

1. Fork [Cube.FileSystem.SevenZip](https://github.com/cube-soft/Cube.FileSystem.SevenZip/fork) repository.
2. Create a feature branch from the master (e.g. git checkout -b my-new-feature origin/master). Note that the master branch may refer some pre-released NuGet packages. Try the [rake clean](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/Rakefile) command when build errors occur.
3. Commit your changes.
4. Rebase your local changes against the master branch.
5. Run the dotnet test command or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create new Pull Request.

## License
 
Copyright © 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)
See [License.md](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/License.md) for more information.