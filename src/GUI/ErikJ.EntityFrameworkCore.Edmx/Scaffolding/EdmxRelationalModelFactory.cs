using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ErikJ.EntityFrameworkCore.Edmx.Scaffolding
{
    // This is the conceptual part of the model
    public class EdmxRelationalModelFactory : IDatabaseModelFactory
    {
        public DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }

        public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
