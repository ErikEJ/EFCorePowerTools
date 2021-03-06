using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RevEng.Core
{
    public class DatabaseModelCacheProvider
    {
        public DatabaseModel GetModelFromFileCache(IDatabaseModelFactory factory, string connectionString, DatabaseModelFactoryOptions factoryOptions, int cacheTtlSeconds)
        {
            return GetModel(factory, connectionString, factoryOptions, cacheTtlSeconds);
        }

        private static DatabaseModel GetModel(IDatabaseModelFactory factory, string connectionString, DatabaseModelFactoryOptions factoryOptions, int cacheTtlSeconds)
        {
            var name = GetHashString(connectionString) + ".json";
            var path = Path.Combine(Path.GetTempPath(), name);

            if (File.Exists(path) 
                && File.GetLastWriteTimeUtc(path) > DateTime.UtcNow.AddSeconds(-cacheTtlSeconds))
            {
                var modelFromCache = JsonConvert.DeserializeObject<DatabaseModel>(File.ReadAllText(path, Encoding.UTF8));

                var filtered = factoryOptions.Tables.ToHashSet();

                if (filtered.Count > 0)
                {
                    for (int i = modelFromCache.Tables.Count - 1; i >= 0; i--)
                    {
                        if (!filtered.Contains(modelFromCache.Tables[i].GetFullName(Shared.DatabaseType.SQLServer)))
                        {
                            modelFromCache.Tables.RemoveAt(i);
                        }
                    }
                }

                return modelFromCache;
            }

            var model = factory.Create(connectionString, factoryOptions);

            WriteCacheIfNoFiltersApplied(factoryOptions, path, model);

            return model;
        }

        private static void WriteCacheIfNoFiltersApplied(DatabaseModelFactoryOptions factoryOptions, string path, DatabaseModel model)
        {
            if (factoryOptions.Tables.Count() == 0)
            {
                File.WriteAllText(path, 
                    JsonConvert.SerializeObject(model,
                        Formatting.None, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            NullValueHandling = NullValueHandling.Ignore,
                        }),
                    Encoding.UTF8);
            }
        }

        private static string GetHashString(string inputString)
        {
            var sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
        }
    }
}
