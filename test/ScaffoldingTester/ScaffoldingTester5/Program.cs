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
                var res = db.Shippers.ToList();

                var list = new[] { "ALFKI", "BERGS", "VAFFE" };

                var customersQuery = db.Orders
                    .Where(s => db.AsSplit(list, ",").Contains(s.CustomerId));

                Console.WriteLine(customersQuery.ToQueryString());

                var result = customersQuery.ToList();

                foreach (var item in result)
                {
                    Console.WriteLine(item.OrderDate);
                }
            }
        }
    }
}
