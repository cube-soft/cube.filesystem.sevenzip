Cube.FileSystem
====

Cube.FileSystem is an I/O library, especially for archiving or extracting files.
The Cube.FileSystem.SevenZip project is licensed under the GNU LGPLv3 and the other projects are Apache 2.0.

## Usage

Note that ArchiveWriter and ArchiveReader classes (Cube.FileSystem.SevenZip) need to execute in the same thread from constructing to destroying.
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

    writer.Add(@"path\to\directory_including_files");
    writer.Save(@"path\to\save.tar.gz");
}

// 2. Example for extracting files.
// Set password directly or using Query<string, string>
var password = new Cube.Query<string, string>(e => e.Result = "password");
using (var reader = new ArchiveReader(@"path\to\archive", password))
{
    foreach (var item in reader.Items)
    {
        // Save as "path\to\directory\{item.FullName}"
        item.Extract(@"path\to\directory");
    }
}
```

## Requirement

* [7-Zip](http://www.7-zip.org/) ... [cube-soft/7z](https://github.com/cube-soft/7z) is optimized for Japanese encoding.
* [AlphaFS](http://alphafs.alphaleonis.com/)
* [Apache log4net](https://logging.apache.org/log4net/)
* [NUnit](http://nunit.org/)

## License

* [GNU LGPLv3](https://github.com/cube-soft/Cube.FileSystem/blob/master/Libraries/SevenZip/License.txt) ... Cube.FileSystem.SevenZip
* [Apache 2.0](https://github.com/cube-soft/Cube.FileSystem/blob/master/License.txt) ... Other projects

## Author
 
[clown](https://gihub.com/clown) and [CubeSoft, Inc.](http://www.cube-soft.jp/)