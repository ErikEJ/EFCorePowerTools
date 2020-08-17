using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Threading.Tasks;

public static class DbContextExtensions
{
    public static async Task<T[]> SqlQuery<T>(this DbContext db, string sql, params object[] parameters) where T : class
    {
        using (var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(), db.Database.CurrentTransaction))
        {
            return await db2.Set<T>().FromSqlRaw(sql, parameters).ToArrayAsync();
        }
    }

    private class ContextForQueryType<T> : DbContext where T : class
    {
        private readonly DbConnection connection;
        private readonly IDbContextTransaction transaction;

        public ContextForQueryType(DbConnection connection, IDbContextTransaction tran)
        {
            this.connection = connection;
            this.transaction = tran;

            if (tran != null)
            {
                this.Database.UseTransaction((tran as IInfrastructure<DbTransaction>).Instance);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (transaction != null)
            {
                optionsBuilder.UseSqlServer(connection);
            }
            else
            {
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>().HasNoKey().ToView(null);
        }
    }
}

public class OutputParameter<TValue>
{
    private bool _hasOperationFinished = false;

    public TValue _value;
    public TValue Value
    {
        get
        {
            if (!_hasOperationFinished)
                throw new InvalidOperationException("Operation has not finished.");

            return _value;
        }
    }

    internal void SetValueInternal(object value)
    {
        _hasOperationFinished = true;
        _value = (TValue)value;
    }
}
