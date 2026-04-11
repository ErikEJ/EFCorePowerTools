CREATE PROCEDURE dbo.StoGetSomeData
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #OrderTable
    (
        SomeName NVARCHAR(100),
        SomeValue NVARCHAR(100),
    );

    SELECT
        t.SomeName,
        t.SomeValue
    FROM #OrderTable t;

    DROP TABLE #OrderTable;
END;
GO
