using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ScaffoldingTester.Models
{
    public partial class NorthwindContext
    {
        public IQueryable<long> AsSplit(long[] source)
            => Split(string.Join(",", source.Select(x => Convert.ToString(x))), ",").Select(s => Convert.ToInt64(s.Value));

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
