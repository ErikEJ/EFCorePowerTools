using Microsoft.EntityFrameworkCore;
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
                var retVal = new OutputParameter<int>();
                var multi = await db.GetProcedures().MultiSetAsync(new DateTime(2021, 12, 24), 7.6m, retVal);

                //var list = new[] { "ALFKI", "BERGS", "VAFFE" };

                var list = new[] { 10253L, 10255L, 10260L };

                var customersQuery = db.Orders
                    .Where(s => db.AsSplit(list).Contains(s.OrderId))
                    .Select(o => new { o.OrderDate, o.CustomerId });

#if DEBUG
                Console.WriteLine(customersQuery.ToQueryString());
#endif
                var result = customersQuery.ToList();

                foreach (var item in result)
                {
                    Console.WriteLine($"{item.CustomerId} : {item.OrderDate}");
                }

                var productCount = new OutputParameter<int?>();
                var description = new OutputParameter<string>();
                var outputRes = db.GetProcedures()
                    .OutputScenariosAsync(2021, productCount, description);
            }
        }
    }
}
