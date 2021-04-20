using ScaffoldingTester.Models;
using System;
using System.Linq;

namespace ScaffoldingTester
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using (var db = new NorthwindContext())
            {

                //var procs = new NorthwindContextProcedures(db);

                var sret = new OutputParameter<string>();
                var returned = new OutputParameter<string>();
                await db.GetProcedures().OutputFailAsync(sret, returned);
                if (sret.Value != "yes")
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: \"yes\". Actual: {sret.Value}.");
                }

                var spacesResult = await db.GetProcedures().SpacesAsync();
                if (spacesResult.Length != 1)
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: 1. Actual: {spacesResult.Length}.");
                }

                var output1 = new OutputParameter<string>();
                var output2 = new OutputParameter<string>();
                var output3 = new OutputParameter<int>();
                var result = await db.GetProcedures().TestMethodOutputNoResultAsync(0, null, output1, output2, output3);

                var result2 = await db.GetProcedures().CustOrderHistDupeAsync("ALFKI");
                if (result2.Length != 11)
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: 11. Actual: {result2.Length}.");
                }

                var return1 = new OutputParameter<int>();
                var test = await db.GetProcedures().ReturnValueAsync(return1);
                if (return1.Value != 42)
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: 42. Actual: {return1.Value}.");
                }

                var rowsResult = await db.GetProcedures().CategoryUpdateAsync("Beverages", 1);
                if (rowsResult != 1)
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: 1. Actual: {rowsResult}.");
                }

                var rowsResult2 = await db.GetProcedures().CategoryUpdateAsync("Beverages", int.MinValue);
                if (rowsResult2 != 0)
                {
                    Console.Error.WriteLine($"AssertEqual failed. Expected: 0. Actual: {rowsResult2}.");
                }

                var udfTest = db.Categories
                    .Where(c => c.CategoryName == NorthwindContext.GetCustInfo("x", null))
                    .ToList();
            }
        }
    }
}
