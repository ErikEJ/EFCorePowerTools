using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScaffoldingTester.Models;
using System;

namespace ScaffoldingTester
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var db = new NorthwindContext();

            var procs = new NorthwindContextProcedures(db);

            var sret = new OutputParameter<string>();
            var returned = new OutputParameter<string>();
            await procs.OutputFail(sret, returned);
            if (sret.Value != "yes")
            {
                Console.Error.WriteLine($"AssertEqual failed. Expected: \"yes\". Actual: {sret.Value}.");
            }

            var spacesResult = await procs.Spaces();
            if (spacesResult.Length != 1)
            {
                Console.Error.WriteLine($"AssertEqual failed. Expected: 1. Actual: {spacesResult.Length}.");
            }

            var output1 = new OutputParameter<string>();
            var output2 = new OutputParameter<string>();
            var output3 = new OutputParameter<int>();
            var result = await procs.TestMethodOutputNoResult(0, output1, output2, output3);
            if (result != null)
            {
                Console.Error.WriteLine($"AssertEqual failed. Expected: null.");
            }
            
            var result2 = await procs.CustOrderHistDupe("ALFKI");
            if (result2.Length != 11)
            {
                Console.Error.WriteLine($"AssertEqual failed. Expected: 11. Actual: {result2.Length}.");
            }

            var return1 = new OutputParameter<int>();
            var test = await procs.ReturnValue(return1);
            if (return1.Value != 42)
            {
                Console.Error.WriteLine($"AssertEqual failed. Expected: 42. Actual: {return1.Value}.");
            }

        }
    }
}
