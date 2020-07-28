using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ScaffoldProcedures.Models
{
    public class BloggingContextProcedures
    {
        // Use ContextName
        private readonly DbContext _context;

        public BloggingContextProcedures(DbContext bloggingContext)
        {
            _context = bloggingContext;
        }

        // Use parameter collection
        // Get normalized proc name
        public async Task<SpGetPostsForBlogResult[]> SpGetPostsForBlog(int take, OutputParameter<int> overallCount)
        {
            var parameterTake = new SqlParameter
            {
                ParameterName = "Take",
                SqlDbType = System.Data.SqlDbType.Int, // Get from model
                Direction = System.Data.ParameterDirection.Input,
                Value = take
            };

            var outParameterOverallCount = new SqlParameter
            {
                ParameterName = "OverallCount",
                SqlDbType = System.Data.SqlDbType.Int, // Get from model
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _context.SqlQuery<SpGetPostsForBlogResult>("exec [dbo].[SP_GET_POST_URLS] @Take, @OverallCount OUTPUT",
                parameterTake,
                outParameterOverallCount);
                //.ToArrayAsync(); // ToArray should only be used for stored procedures to ensure that out parameters are set

            overallCount.SetValueInternal(outParameterOverallCount.Value);

            return result;
        }

    }
}
