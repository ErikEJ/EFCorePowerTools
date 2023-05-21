using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Useful extensions for DbContext.
    /// </summary>
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
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// A <see cref="IAsyncEnumerable{T}" /> object that will contain the result of the query.
        /// </returns>
        public static IAsyncEnumerable<T> SqlQueryValueAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default) // where T : class
        {
            ArgumentNullException.ThrowIfNull(db);

            if (parameters is null)
            {
                parameters = Array.Empty<object>();
            }

            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return SqlQueryValueInternalAsync<T>(db, sql, parameters, cancellationToken);
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
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// A <see cref="IAsyncEnumerable{T}" /> object that will contain the result of the query.
        /// </returns>
        public static IAsyncEnumerable<T> SqlQueryAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(db);

            return SqlQueryInternalAsync<T>(db, sql, parameters, cancellationToken);
        }

        private static async IAsyncEnumerable<T> SqlQueryInternalAsync<T>(this DbContext db, string sql, object[] parameters = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where T : class
        {
            if (parameters is null)
            {
                parameters = Array.Empty<object>();
            }

            ContextForQueryType<T> db2 = null;

            try
            {
                db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(), db.Database.CurrentTransaction);

                await using (db2.ConfigureAwait(false))
                {
                    db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());

                    var result = db2.Set<T>()
                        .FromSqlRaw(sql, parameters)
                        .AsAsyncEnumerable()
                        .WithCancellation(cancellationToken)
                        .ConfigureAwait(false);

                    await foreach (T item in result)
                    {
                        yield return item;
                    }
                }
            }
            finally
            {
                await db2.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async IAsyncEnumerable<T> SqlQueryValueInternalAsync<T>(this DbContext db, string sql, object[] parameters = null, [EnumeratorCancellation]CancellationToken cancellationToken = default) // where T : class
        {
            ContextForQueryType<ValueReturn<T>> db2 = null;

            try
            {
                db2 = new ContextForQueryType<ValueReturn<T>>(db.Database.GetDbConnection(), db.Database.CurrentTransaction);

                await using (db2.ConfigureAwait(false))
                {
                    db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());

                    var result = db2.Set<ValueReturn<T>>()
                        .FromSqlRaw(sql, parameters)
                        .AsAsyncEnumerable()
                        .WithCancellation(cancellationToken)
                        .ConfigureAwait(false);

                    await foreach (ValueReturn<T> item in result)
                    {
                        yield return item.Value;
                    }
                }
            }
            finally
            {
                await db2.DisposeAsync().ConfigureAwait(false);
            }
        }

#pragma warning disable CA1812
        internal sealed class ValueReturn<T>
#pragma warning restore CA1812
        {
            public T Value { get; private set; }
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
