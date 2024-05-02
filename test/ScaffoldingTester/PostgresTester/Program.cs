namespace PostgresTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new Models.NorthwindContext())
            {
                var result = await db.Procedures.CustOrderHistAsync("ALFKI");
                if (result.Count != 11)
                {
                    Console.WriteLine($"AssertEqual failed. Expected: \"11\". Actual: {result.Count}.");
                }
            }
        }
    }
}
