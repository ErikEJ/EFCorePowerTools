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
        public DatabaseModel GetModelFromFileCache(IDatabaseModelFactory factory, string connectionString, DatabaseModelFactoryOptions factoryOptions)
        {
            var model = GetModel(factory, connectionString, factoryOptions);
            
            //TODO Filter tables/views! 

            return model;
        }

        private static DatabaseModel GetModel(IDatabaseModelFactory factory, string connectionString, DatabaseModelFactoryOptions factoryOptions)
        {
            var name = GetHashString(connectionString) + ".json";
            var path = Path.Combine(Path.GetTempPath(), name);

            //TODO Make cache optional via setting
            if (File.Exists(path) 
                && File.GetLastWriteTimeUtc(path) > DateTime.UtcNow.AddSeconds(-90))
            {
                return JsonConvert.DeserializeObject<DatabaseModel>(File.ReadAllText(path, Encoding.UTF8));
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
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
