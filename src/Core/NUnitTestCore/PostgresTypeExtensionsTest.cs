using System;
using RevEng.Core.Routines.Extensions;
using Xunit;

namespace UnitTests
{
    public class PostgresTypeExtensionsTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanMapCitextToString(bool isNullable)
        {
            var result = PostgresNpgsqlTypeExtensions.GetClrType("citext", isNullable, "test");

            Assert.Equal(typeof(string), result);
        }
    }
}
