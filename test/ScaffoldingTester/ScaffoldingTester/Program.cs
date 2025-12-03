using Microsoft.EntityFrameworkCore;
using ScaffoldingTester.Models;
using System;

namespace ScaffoldingTester
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;MultipleActiveResultSets=True", x => x.UseNetTopologySuite());
            using (var db = new NorthwindContext(optionsBuilder.Options))
            {
                var result = await db.GetProcedures().CustOrdersOrdersAsync("ALFKI");
                if (result.Count != 6)
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: \"6\". Actual: {result.Count}.");
                }
            }
        }
    }
}
