using Microsoft.EntityFrameworkCore;

namespace DbContextExtensionsTester
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var context = new ChinookContext();

            var result = await context.SqlQueryValueAsync<int>("SELECT 1 AS Value");

            var result2 = await context.SqlQueryValueAsync<string>("SELECT 'test' AS Value");
        }
    }
}
