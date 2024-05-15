using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Useful extensions for DbContext.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4457:Parameter validation in \"async\"/\"await\" methods should be wrapped", Justification = "Broken analyzer?")]
    public static class DbContextExtensions
    {
        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.
        /// The type must be a simple primitive type. In additon, the name of the returned type in the SQL statement must be 'Value'
        ///
        /// As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional arguments. Any parameter values you supply will automatically be converted to a DbParameter.
        /// context.Database.SqlQuery&lt;Post&gt;("SELECT Id as Value FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor);
        /// Alternatively, you can also construct a DbParameter and supply it to SqlQuery. This allows you to use named parameters in the SQL query string.
        /// context.Database.SqlQuery&lt;Post&gt;("SELECT Name AS Value FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));.
        /// </summary>
        /// <typeparam name="T"> The type of object returned by the query. </typeparam>
        /// <param name="db"> The DbContext. </param>
        /// <param name="sql"> The SQL query string. </param>
        /// <param name="parameters">
        /// The parameters to apply to the SQL query string.
        /// </param>
        /// <param name="cancellationToken"> Cancellation token. </param>
        /// <returns>
        /// A <see cref="List{T}" /> object that will contain the result of the query.
        /// </returns>
        public static async Task<List<T>> SqlQueryValueAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default) // where T : class
        {
            ArgumentNullException.ThrowIfNull(db);

            if (parameters is null)
            {
                parameters = Array.Empty<object>();
            }

            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return await SqlQueryValueInternalAsync<T>(db, sql, parameters, cancellationToken).ConfigureAwait(false);
            }

            throw new NotSupportedException("Invalid operation, supplied type is not a value type");
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.
        /// The type can be any type that has properties that match the names of the columns returned
        /// from the query.
        ///
        /// As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional arguments. Any parameter values you supply will automatically be converted to a DbParameter.
        /// context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor);
        /// Alternatively, you can also construct a DbParameter and supply it to SqlQuery. This allows you to use named parameters in the SQL query string.
        /// context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));.
        /// </summary>
        /// <typeparam name="T"> The type of object returned by the query. </typeparam>
        /// <param name="db"> The DbContext.</param>
        /// <param name="sql"> The SQL query string. </param>
        /// <param name="parameters">
        /// The parameters to apply to the SQL query string.
        /// </param>
        /// <param name="cancellationToken"> Cancellation token. </param>
        /// <returns>
        /// A <see cref="List{T}" /> object that will contain the result of the query.
        /// </returns>
        public static async Task<List<T>> SqlQueryAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(db);

            return await SqlQueryInternalAsync<T>(db, sql, parameters, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<List<T>> SqlQueryInternalAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default)
            where T : class
        {
            if (parameters is null)
            {
                parameters = Array.Empty<object>();
            }

            using var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(), db.Database.CurrentTransaction);

            db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());

            return await db2.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        private static async Task<List<T>> SqlQueryValueInternalAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default) // where T : class
        {
            using var db2 = new ContextForQueryType<ValueReturn<T>>(db.Database.GetDbConnection(), db.Database.CurrentTransaction);

            db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());

            var result = await db2.Set<ValueReturn<T>>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken).ConfigureAwait(false);

            return result.Select(v => v.Value).ToList();
        }

#pragma warning disable CA1812
#pragma warning disable CA1852
        internal class ValueReturn<T>
#pragma warning restore CA1812
#pragma warning restore CA1852
        {
#pragma warning disable S1144 // Unused private types or members should be removed
            public T Value { get; private set; }
#pragma warning restore S1144 // Unused private types or members should be removed
        }

        private sealed class ContextForQueryType<T> : DbContext
            where T : class
        {
            private readonly DbConnection connection;
            private readonly IDbContextTransaction transaction;

            public ContextForQueryType(DbConnection connection, IDbContextTransaction tran)
            {
                this.connection = connection;
                transaction = tran;

                if (tran != null)
                {
                    Database.UseTransaction((tran as IInfrastructure<DbTransaction>).Instance);
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
                modelBuilder.Entity<ValueReturn<string>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<Guid>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<DateTimeOffset>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<DateTime>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<bool>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<byte>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<short>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<int>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<long>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<float>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<decimal>>().HasNoKey().ToView(null);
                modelBuilder.Entity<ValueReturn<double>>().HasNoKey().ToView(null);
                modelBuilder.Entity<T>().HasNoKey().ToView(null);
            }
        }
    }
}
