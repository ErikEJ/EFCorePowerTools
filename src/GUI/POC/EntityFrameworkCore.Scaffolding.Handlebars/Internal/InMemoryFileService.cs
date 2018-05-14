// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Modifications copyright(C) 2017 Tony Sneed.

using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EntityFrameworkCore.Scaffolding.Handlebars.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InMemoryFileService : IFileService
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected readonly Dictionary<string, Dictionary<string, string>> NameToContentMap
            = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool DirectoryExists(string directoryName)
            => NameToContentMap.TryGetValue(directoryName, out _);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool FileExists(string directoryName, string fileName)
            => NameToContentMap.TryGetValue(directoryName, out var filesMap)
               && filesMap.TryGetValue(fileName, out _);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool IsFileReadOnly(string outputDirectoryName, string outputFileName) => false;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual string RetrieveFileContents(string directoryName, string fileName)
        {
            if (!NameToContentMap.TryGetValue(directoryName, out var filesMap))
            {
                throw new DirectoryNotFoundException("Could not find directory " + directoryName);
            }

            if (!filesMap.TryGetValue(fileName, out var contents))
            {
                throw new FileNotFoundException("Could not find file " + Path.Combine(directoryName, fileName));
            }

            return contents;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual string OutputFile(
            string directoryName,
            string fileName,
            string contents)
        {
            if (!NameToContentMap.TryGetValue(directoryName, out var filesMap))
            {
                filesMap = new Dictionary<string, string>();
                NameToContentMap[directoryName] = filesMap;
            }

            filesMap[fileName] = contents;

            return Path.Combine(directoryName, fileName);
        }
    }
}
