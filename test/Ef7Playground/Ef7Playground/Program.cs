using Ef7Playground.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.Data.SqlClient;

using var connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=DateOnlyTest;Integrated Security=true;Encrypt=False");
await connection.OpenAsync();

using var command = connection.CreateCommand();

command.CommandText = @"
IF NOT EXISTS (SELECT * 
               FROM INFORMATION_SCHEMA.TABLES 
               WHERE TABLE_SCHEMA = 'dbo' 
                 AND TABLE_NAME = 'TestTable') 
    CREATE TABLE [dbo].[TestTable](
	    [Id] [bigint] NOT NULL,
	    [InvoiceDate] [date] NOT NULL
)";
await command.ExecuteNonQueryAsync();

command.CommandText = "INSERT INTO dbo.TestTable (Id, InvoiceDate) VALUES (@p1, @p2)";
command.Parameters.Add(new SqlParameter("@p1", 1));
command.Parameters.Add(new SqlParameter("@p2", new DateOnly(1964, 7, 25)));
await command.ExecuteNonQueryAsync();

command.Parameters.Clear();
command.CommandText = "SELECT [InvoiceDate] FROM dbo.TestTable WHERE [InvoiceDate] = @p1";
command.Parameters.Add(new SqlParameter("@p1", new DateOnly(1964, 7, 25)));
using var reader = await command.ExecuteReaderAsync();
while (reader.Read())
{
    var date = await reader.GetFieldValueAsync<DateOnly>(0);
    Console.WriteLine(date);
}

using var db = new NorthwindContext();

var customers = db.Customers.ToList();

var rowsAffected = await db.Regions
    .Where(r => r.RegionId == 5)
    .ExecuteDeleteAsync();

Console.WriteLine($"deletion rows affected: {rowsAffected}");

rowsAffected = db.Regions
    .Where(r => r.RegionId == 5)
    .ExecuteUpdate(r => r.SetProperty(p => p.RegionDescription, "Updated"));

Console.WriteLine($"update rows affected: {rowsAffected}");

var regionIds = await db.Database
    .SqlQuery<int>($"SELECT RegionId FROM dbo.Region")
    .ToListAsync();

foreach (var regionId in regionIds)
{
    Console.WriteLine(regionId);
}



Console.WriteLine("EF Core 7 rocks!");

