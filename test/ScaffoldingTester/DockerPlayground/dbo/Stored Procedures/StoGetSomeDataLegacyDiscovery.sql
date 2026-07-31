CREATE PROCEDURE dbo.StoGetSomeDataLegacyDiscovery
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #OrderLegacyTable
    (
        OrderName NVARCHAR(100) NULL,
        OrderValue NVARCHAR(100) NULL
    );

    SELECT
        t.OrderName,
        t.OrderValue
    FROM #OrderLegacyTable t;
END;
GO
