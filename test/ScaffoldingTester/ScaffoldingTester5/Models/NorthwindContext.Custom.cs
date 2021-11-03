using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ScaffoldingTester.Models
{
    public partial class NorthwindContext
    {
        public IQueryable<string> AsSplit(string[] source, string separator)
            => Split(string.Join(separator, source), separator).Select(s => s.Value);

        [DbFunction(IsBuiltIn = true, Name = "STRING_SPLIT")]
        private IQueryable<StringSplitResult> Split(string source, string separator)
            => FromExpression(() => Split(source, separator));

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StringSplitResult>().HasNoKey();
        }
    }
}
