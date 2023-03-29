// This file is part of Managed NTFS Data Streams project
//
// Copyright 2020 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Ice
{
    /// <summary>
    /// Provides functionality to convert into the file data stream type.
    /// </summary>
    public static class FileDataStreamTypeConverter
    {
        private static IReadOnlyDictionary<string, FileDataStreamType> TypeCache { get; } = GenerateTypeCache();

        /// <summary>
        /// Gets the stream type of the specified file.
        /// </summary>
        public static FileDataStreamType GetStreamType(string typeName)
            => TypeCache.TryGetValue(typeName, out var streamType) switch
            {
                true => streamType,
                _ => FileDataStreamType.Unknown
            };

        private static IReadOnlyDictionary<string, FileDataStreamType> GenerateTypeCache()
            => typeof(FileDataStreamType)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(x => new { value = (FileDataStreamType)x.GetValue(null), name = x.GetCustomAttribute<FileDataStreamTypeValueAttribute>() })
                .Where(x => x.name != null)
                .ToDictionary(x => x.name.TypeNameString, x => x.value);
    }
}
