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

                var list = "ALFKI;BERGS;VAFFE";

                var customersQuery = db.Orders
                    .Where(s => db.Split(list, ";").Any(split => split.Value == s.CustomerId));

                var sql = customersQuery.ToQueryString();

                var result = customersQuery.ToList();

                foreach (var item in result)
                {
                    Console.WriteLine(item.OrderDate);
                }
            }
        }
    }
}
