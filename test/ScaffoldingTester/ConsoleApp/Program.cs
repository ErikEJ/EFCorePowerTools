using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChinookContext>();

            using var db = new ChinookContext(optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=Chinook;Integrated Security=True", x => x.UseNetTopologySuite()).Options);
            var result = db.Customers.ToList();
        }
    }
}
