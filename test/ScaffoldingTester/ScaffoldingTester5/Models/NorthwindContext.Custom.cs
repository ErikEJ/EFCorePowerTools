using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ScaffoldingTester.Models
{
    public partial class NorthwindContext
    {
        public IQueryable<long> AsSplit(long[] source)
            => Split(string.Join(",", source.Select(x => x.ToString(CultureInfo.InvariantCulture))), ",")
#pragma warning disable CA1305 // EF Core translates this overload to SQL CONVERT(bigint); the provider overload is not translatable.
                .Select(s => Convert.ToInt64(s.Value));
#pragma warning restore CA1305

        public IQueryable<string> AsSplit(string[] source)
            => Split(string.Join(",", source), ",").Select(s => s.Value);

        [DbFunction(IsBuiltIn = true, Name = "STRING_SPLIT")]
        private IQueryable<StringSplitResult> Split(string source, string separator)
            => FromExpression(() => Split(source, separator));

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StringSplitResult>().HasNoKey();
        }
    }
}