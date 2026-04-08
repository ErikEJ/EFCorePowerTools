CREATE PROCEDURE dbo.StoGetSomeDataWithParameters
    @CategoryId INT,
    @SearchTerm NVARCHAR(50) = NULL,
    @Amount DECIMAL(10,2) = 0
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Temp
    (
        CategoryId INT NOT NULL,
        SearchTerm NVARCHAR(50) NULL,
        Amount DECIMAL(10,2) NOT NULL
    );

    INSERT INTO #Temp
    (
        CategoryId,
        SearchTerm,
        Amount
    )
    VALUES
    (
        @CategoryId,
        @SearchTerm,
        @Amount
    );

    SELECT
        t.CategoryId,
        t.SearchTerm,
        t.Amount
    FROM #Temp t;

    DROP TABLE #Temp;
END;
GO
