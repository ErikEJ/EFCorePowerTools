using ScaffoldingTester.Models;
using System;

namespace ScaffoldingTester
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using (var db = new NorthwindContext())
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
