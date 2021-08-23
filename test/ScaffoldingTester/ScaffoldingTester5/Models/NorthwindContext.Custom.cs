using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ScaffoldingTester.Models
{
    public partial class NorthwindContext
    {
        [DbFunction(IsBuiltIn = true, Name = "STRING_SPLIT")]
        public IQueryable<StringSplitResult> Split(string source, string separator)
            => FromExpression(() => Split(source, separator));

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StringSplitResult>().HasNoKey();
        }
    }
}
