CREATE PROCEDURE dbo.StoGetSomeDataMultipleResults
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Summary
    (
        CategoryId INT NOT NULL,
        TotalCount INT NOT NULL
    );

    CREATE TABLE #Details
    (
        CategoryId INT NOT NULL,
        ItemName NVARCHAR(100) NULL
    );

    INSERT INTO #Summary
    (
        CategoryId,
        TotalCount
    )
    VALUES
    (
        @CategoryId,
        1
    );

    INSERT INTO #Details
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
    FROM #Summary s;

    SELECT
        d.CategoryId,
        d.ItemName
    FROM #Details d;
END;
GO
