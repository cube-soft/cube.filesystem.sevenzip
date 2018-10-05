Cube.FileSystem.SevenZip
====

[![AppVeyor](https://ci.appveyor.com/api/projects/status/jao7f754rlookxxe?svg=true)](https://ci.appveyor.com/project/clown/cube-filesystem-sevenzip)
[![Codecov](https://codecov.io/gh/cube-soft/Cube.FileSystem.SevenZip/branch/master/graph/badge.svg)](https://codecov.io/gh/cube-soft/Cube.FileSystem.SevenZip)

Cube.FileSystem.SevenZip is an I/O library, especially for archiving or extracting files.
The Cube.FileSystem.SevenZip project (files in the Libraries directory) is licensed under the GNU LGPLv3 and the other projects are Apache 2.0.

## Usage

Note that ArchiveWriter and ArchiveReader classes need to execute in the same thread from constructing to destroying.
Use Task.Run() in the whole transaction if you need to archive or extract files asynchronously.

### Example for archiving files

The simplest example for archiving files is as follows.
Note that all the samples need the "using Cube.FileSystem.SevenZip;" statement.

```cs
using (var writer = new ArchiveWriter(Format.Zip))
{
    writer.Option = new ZipOption(); // optional
    writer.Add(@"path\to\file");
    writer.Add(@"path\to\directory_including_files");
    writer.Save(@"path\to\save.zip", "password");
}
```

You create an ArchiveWriter object with an archiving format (e.g. Zip, SevenZip, ...), 
add files and/or directories you want to archive, and finally call the Save method.
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

### Example for extracting archives

If you want to extract all files from the archive, you create an ArchiveReader object
and call the Extract method. The 2nd argument of the constructor, that means the
password of the archive, can be set string or Cube.Query&lt;string&gt; object.
The latter is mainly used for implementing the interactive mode.

```cs
// Set password directly or using Query<string>
var password = new Cube.Query<string>(e => e.Result = "password");
using (var reader = new ArchiveReader(@"path\to\archive", password))
{
    reader.Extract(@"path\to\directory");
}
```

ArchvieReader.Items property can access the each item of the archive.
If you want to extract only the specific files, write as follows.

```cs
using (var reader = new ArchiveReader(@"path\to\archive", "password"))
{
    var directory = @"path\to\directory";

    // Save as "path\to\directory\{item.FullName}"
    reader.Items[0].Extract(directory);
    reader.Items[3].Extract(directory);
}
```

## Todo

We will implement to add and/or modify files to existed archives in the future.

## Dependencies

* [7-Zip](http://www.7-zip.org/) ... [cube-soft/7z](https://github.com/cube-soft/7z) is optimized for Japanese encoding.
* [AlphaFS](http://alphafs.alphaleonis.com/)
* [Cube.Core](https://github.com/cube-soft/Cube.Core)
* [Cube.FileSystem](https://github.com/cube-soft/Cube.FileSystem)
* [log4net](https://logging.apache.org/log4net/)

## Thanks

* [babel](http://tricklib.com/cxx/ex/babel/) ... Used in the customized version of 7-Zip
* [SevenZipSharp](https://www.nuget.org/packages/SevenZipSharp/)

## Contributing

1. Fork [Cube.FileSystem.SevenZip](https://github.com/cube-soft/Cube.FileSystem.SevenZip/fork) repository.
2. Create a feature branch from the [stable](https://github.com/cube-soft/Cube.FileSystem.SevenZip/tree/stable) branch (git checkout -b my-new-feature origin/stable). The [master](https://github.com/cube-soft/Cube.FileSystem.SevenZip/tree/master) branch may refer some pre-released NuGet packages. See [AppVeyor.yml](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/AppVeyor.yml) if you want to build and commit in the master branch.
3. Commit your changes.
4. Rebase your local changes against the stable (or master) branch.
5. Run test suite with the [NUnit](http://nunit.org/) console or the Visual Studio (NUnit 3 test adapter) and confirm that it passes.
6. Create new Pull Request.

## License
 
Copyright &copy; 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)

The Cube.FileSystem.SevenZip project is licensed under the [GNU LGPLv3](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/Libraries/License.txt)
and the other projects are [Apache 2.0](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/License.txt).
Note that trade names, trademarks, service marks, or logo images distributed in CubeSoft applications are not allowed to reuse or modify all or parts of them.