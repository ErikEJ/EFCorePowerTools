CREATE PROCEDURE dbo.StoGetSomeDataMultipleResults
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #OrderSummaryTable
    (
        CategoryId INT NOT NULL,
        TotalCount INT NOT NULL
    );

    CREATE TABLE #OrderDetailTable
    (
        CategoryId INT NOT NULL,
        ItemName NVARCHAR(100) NULL
    );

    INSERT INTO #OrderSummaryTable
    (
        CategoryId,
        TotalCount
    )
    VALUES
    (
        @CategoryId,
        1
    );

    INSERT INTO #OrderDetailTable
    (
        CategoryId,
        ItemName
    )
    VALUES
    (
        @CategoryId,
        N'Test Item'
    );

    SELECT
        s.CategoryId,
        s.TotalCount
    FROM #OrderSummaryTable s;

    SELECT
        d.CategoryId,
        d.ItemName
    FROM #OrderDetailTable d;
END;
GO
