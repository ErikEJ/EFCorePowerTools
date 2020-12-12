﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using ScaffoldingTester.Models;

namespace ScaffoldingTester.Models
{
    public partial class NorthwindContextProcedures
    {
        private readonly NorthwindContext _context;

        public NorthwindContextProcedures(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<CustOrderHistDboResult[]> CustOrderHistDbo(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "CustomerID",
                    Size = 10,
                    Value = CustomerID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<CustOrderHistDboResult>("EXEC @returnValue = [dbo].[CustOrderHist] @CustomerID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<CustOrderHistDupeResult[]> CustOrderHistDupe(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "CustomerID",
                    Size = 10,
                    Value = CustomerID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<CustOrderHistDupeResult>("EXEC @returnValue = [dupe].[CustOrderHist] @CustomerID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<CustOrdersDetailResult[]> CustOrdersDetail(int? OrderID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "OrderID",
                    Value = OrderID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<CustOrdersDetailResult>("EXEC @returnValue = [dbo].[CustOrdersDetail] @OrderID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<CustOrdersOrdersResult[]> CustOrdersOrders(string CustomerID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "CustomerID",
                    Size = 10,
                    Value = CustomerID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<CustOrdersOrdersResult>("EXEC @returnValue = [dbo].[CustOrdersOrders] @CustomerID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<EmployeeSalesbyCountryResult[]> EmployeeSalesbyCountry(DateTime? Beginning_Date, DateTime? Ending_Date, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Beginning_Date",
                    Value = Beginning_Date ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "Ending_Date",
                    Value = Ending_Date ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<EmployeeSalesbyCountryResult>("EXEC @returnValue = [dbo].[Employee Sales by Country] @Beginning_Date, @Ending_Date", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<OutputFailResult[]> OutputFail(OutputParameter<string> RESPONSESTATUS, OutputParameter<string> RESPONSEMESSSAGE, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterRESPONSESTATUS = new SqlParameter
            {
                ParameterName = "RESPONSESTATUS",
                Size = 20,
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.VarChar,
            };
            var parameterRESPONSEMESSSAGE = new SqlParameter
            {
                ParameterName = "RESPONSEMESSSAGE",
                Size = 200,
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.VarChar,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterRESPONSESTATUS,
                parameterRESPONSEMESSSAGE,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<OutputFailResult>("EXEC @returnValue = [dbo].[OutputFail] @RESPONSESTATUS OUTPUT, @RESPONSEMESSSAGE OUTPUT", sqlParameters, cancellationToken);

            RESPONSESTATUS.SetValue(parameterRESPONSESTATUS.Value);
            RESPONSEMESSSAGE.SetValue(parameterRESPONSEMESSSAGE.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<ReturnValueResult[]> ReturnValue(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ReturnValueResult>("EXEC @returnValue = [dbo].[ReturnValue]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<SalesbyYearResult[]> SalesbyYear(DateTime? Beginning_Date, DateTime? Ending_Date, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Beginning_Date",
                    Value = Beginning_Date ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "Ending_Date",
                    Value = Ending_Date ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SalesbyYearResult>("EXEC @returnValue = [dbo].[Sales by Year] @Beginning_Date, @Ending_Date", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<SalesByCategoryResult[]> SalesByCategory(string CategoryName, string OrdYear, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "CategoryName",
                    Size = 30,
                    Value = CategoryName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "OrdYear",
                    Size = 8,
                    Value = OrdYear ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SalesByCategoryResult>("EXEC @returnValue = [dbo].[SalesByCategory] @CategoryName, @OrdYear", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<SpacesResult[]> Spaces(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SpacesResult>("EXEC @returnValue = [dbo].[Spaces]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<TempObjectsResult[]> TempObjects(int? DetailID, OutputParameter<int?> returnCode, OutputParameter<string> result, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnCode = new SqlParameter
            {
                ParameterName = "returnCode",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterresult = new SqlParameter
            {
                ParameterName = "result",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.VarChar,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "DetailID",
                    Value = DetailID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnCode,
                parameterresult,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TempObjectsResult>("EXEC @returnValue = [dbo].[TempObjects] @DetailID, @returnCode OUTPUT, @result OUTPUT", sqlParameters, cancellationToken);

            returnCode.SetValue(parameterreturnCode.Value);
            result.SetValue(parameterresult.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<TenMostExpensiveProductsResult[]> TenMostExpensiveProducts(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TenMostExpensiveProductsResult>("EXEC @returnValue = [dbo].[Ten Most Expensive Products]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<TestResult[]> Test(string something, string something1, string something2, string something3, string something4, string something5, string something6, string something7, string something8, string something9, string something10, string something11, string something12, string something13, string something14, string something15, string something16, OutputParameter<int?> myOutput, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parametermyOutput = new SqlParameter
            {
                ParameterName = "myOutput",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parametermyOutput,
                new SqlParameter
                {
                    ParameterName = "something",
                    Size = 12,
                    Value = something ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something1",
                    Size = 40,
                    Value = something1 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something2",
                    Size = 40,
                    Value = something2 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something3",
                    Size = 40,
                    Value = something3 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something4",
                    Size = 40,
                    Value = something4 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something5",
                    Size = 40,
                    Value = something5 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something6",
                    Size = 10,
                    Value = something6 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something7",
                    Size = 40,
                    Value = something7 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something8",
                    Size = 3,
                    Value = something8 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something9",
                    Size = 1,
                    Value = something9 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something10",
                    Size = 16,
                    Value = something10 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something11",
                    Size = 20,
                    Value = something11 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something12",
                    Size = 30,
                    Value = something12 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something13",
                    Size = 241,
                    Value = something13 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something14",
                    Size = 241,
                    Value = something14 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something15",
                    Size = 241,
                    Value = something15 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "something16",
                    Size = 1000,
                    Value = something16 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TestResult>("EXEC @returnValue = [dbo].[Test] @myOutput OUTPUT, @something, @something1, @something2, @something3, @something4, @something5, @something6, @something7, @something8, @something9, @something10, @something11, @something12, @something13, @something14, @something15, @something16", sqlParameters, cancellationToken);

            myOutput.SetValue(parametermyOutput.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<TestMethodOutputNoParamsResult[]> TestMethodOutputNoParams(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TestMethodOutputNoParamsResult>("EXEC @returnValue = [dbo].[TestMethodOutputNoParams]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<TestMethodOutputNoResultResult[]> TestMethodOutputNoResult(int? testParameter1, OutputParameter<string> testParameter2, OutputParameter<string> testParameter3, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parametertestParameter2 = new SqlParameter
            {
                ParameterName = "testParameter2",
                Size = 255,
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.VarChar,
            };
            var parametertestParameter3 = new SqlParameter
            {
                ParameterName = "testParameter3",
                Size = 255,
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.VarChar,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "testParameter1",
                    Value = testParameter1 ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parametertestParameter2,
                parametertestParameter3,
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<TestMethodOutputNoResultResult>("EXEC @returnValue = [dbo].[TestMethodOutputNoResult] @testParameter1, @testParameter2 OUTPUT, @testParameter3 OUTPUT", sqlParameters, cancellationToken);

            testParameter2.SetValue(parametertestParameter2.Value);
            testParameter3.SetValue(parametertestParameter3.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
