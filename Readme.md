Cube.FileSystem.SevenZip
====

[![AppVeyor](https://ci.appveyor.com/api/projects/status/jao7f754rlookxxe?svg=true)](https://ci.appveyor.com/project/clown/cube-filesystem-sevenzip)

Cube.FileSystem.SevenZip is an I/O library, especially for archiving or extracting files.
The Cube.FileSystem.SevenZip project (files in the Libraries directory) is licensed under the GNU LGPLv3 and the other projects are Apache 2.0.

## Usage

Note that ArchiveWriter and ArchiveReader classes need to execute in the same thread from constructing to destroying.
Use Task.Run() in the whole transaction if you need to archive or extract files asynchronously.

```cs
using Cube.FileSystem.SevenZip;

// 1-1. Example for archiving files.
using (var writer = new ArchiveWriter(Format.Zip))
{
    writer.Option = new ZipOption(); // optional
    writer.Add(@"path\to\file");
    writer.Add(@"path\to\directory_including_files");
    writer.Save(@"path\to\save.zip", "password");
}

// 1-2. Example for archiving files to *.tar.*
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

// 2-1. Example for extracting all files.
// Set password directly or using Query<string, string>
var password = new Cube.Query<string, string>(e => e.Result = "password");
using (var reader = new ArchiveReader(@"path\to\archive", password))
{
    reader.Extract(@"path\to\directory");
}

// 2-2. Example for extracting some files.
using (var reader = new ArchiveReader(@"path\to\archive", "password"))
{
    var directory = @"path\to\directory";

    // Save as "path\to\directory\{item.FullName}"
    reader.Items[0].Extract(directory);
    reader.Items[3].Extract(directory);
}
```

## Todo

We will implement add and/or modify files in the existed archives in the future.

## Requirements

* [7-Zip](http://www.7-zip.org/) ... [cube-soft/7z](https://github.com/cube-soft/7z) is optimized for Japanese encoding.
* [AlphaFS](http://alphafs.alphaleonis.com/)
* [log4net](https://logging.apache.org/log4net/)
* [NUnit](http://nunit.org/)

## Thanks

* [babel](http://tricklib.com/cxx/ex/babel/) ... Used in the customized version of 7-Zip
* [SevenZipSharp](https://www.nuget.org/packages/SevenZipSharp/)

## License
 
Copyright (c) 2010 [CubeSoft, Inc.](http://www.cube-soft.jp/)
The Cube.FileSystem.SevenZip project is licensed under the [GNU LGPLv3](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/Libraries/License.txt)
and the other projects are [Apache 2.0](https://github.com/cube-soft/Cube.FileSystem.SevenZip/blob/master/License.txt).