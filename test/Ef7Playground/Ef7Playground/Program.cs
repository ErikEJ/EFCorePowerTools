using Ef7Playground.Models;

using Microsoft.EntityFrameworkCore;

using var db = new NorthwindContext();

var customers = db.Customers.ToList();

var rowsAffected = await db.Regions
    .Where(r => r.RegionId == 5)
    .ExecuteDeleteAsync();

Console.WriteLine($"deletion rows affected: {rowsAffected}");

rowsAffected = db.Regions
    .Where(r => r.RegionId == 5)
    .ExecuteUpdate(r => r.SetProperty(p => p.RegionDescription, p => "Updated"));

Console.WriteLine($"update rows affected: {rowsAffected}");

var regionIds = await db.Database
    .SqlQuery<int>($"SELECT RegionId FROM dbo.Region")
    .ToListAsync();

foreach (var regionId in regionIds)
{
    Console.WriteLine(regionId);
}



Console.WriteLine("EF Core 7 rocks!");

