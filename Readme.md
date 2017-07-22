Cube.FileSystem
====

Cube.FileSystem is an I/O library, especially for archiving or extracting files.

## Usage

```csharp:ArchiveSample
using Cube.FileSystem.SevenZip;

// Note that the ArchiveWriter class needs to execute in the same thread from constructing to destroying
Task.Run(() =>
{
    // Supported Format: Zip, SevenZip, BZip2, GZip, XZ
    using (var writer = new ArchiveWriter(Format.Zip))
    {
        writer.Option = new ZipOption(); // optional
        writer.Add("path\to\file");
        writer.Add("path\to\directory_including_files");
        writer.Save("path\to\save.zip", "password");
    }
});
```

```csharp:ExtractSample
using Cube.FileSystem.SevenZip;

// Note that the ArchiveReader class needs to execute in the same thread from constructing to destroying
Task.Run(() =>
{
    // Set password directly or using Query<string, string>
    var password = new Cube.Query<string, string>(e => e.Result = "password");
    using (var reader = new ArchiveReader("path/to/archive", password))
    {
        foreach (var item in reader.Items)
        {
            // Save as "path\to\directory\{item.FullName}"
            item.Extract(@"path\to\directory");
        }
    }
});
```

## Requirement

* [7-Zip](http://www.7-zip.org/) ... for Cube.FileSystem.SevenZip and derived projects
* [AlphaFS](http://alphafs.alphaleonis.com/)
* [Apache log4net](https://logging.apache.org/log4net/)
* [NUnit](http://nunit.org/) ... for unit tests

## Licence

* [GNU LGPLv3](https://github.com/cube-soft/Cube.FileSystem/blob/master/Libraries/SevenZip/License.txt) ... Cube.FileSystem.SevenZip
* [Apache 2.0](https://github.com/cube-soft/Cube.FileSystem/blob/master/License.txt) ... the others

## Author
 
[clown](https://gihub.com/clown), [CubeSoft, Inc.](http://www.cube-soft.jp/)