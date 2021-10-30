using ConsoleApp.Models;
using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new ChinookContext();
            db.Customers.ToList();
        }
    }
}
