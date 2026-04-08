CREATE PROCEDURE dbo.StoGetSomeData
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Temp
    (
        SomeName NVARCHAR(100),
        SomeValue NVARCHAR(100),
    );

    SELECT
        t.SomeName,
        t.SomeValue
    FROM #Temp t;

    DROP TABLE #Temp;
END;
GO