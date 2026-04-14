CREATE PROCEDURE dbo.StoGetSomeDataDirect
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CAST(N'Direct Name' AS NVARCHAR(100)) AS SomeName,
        CAST(N'Direct Value' AS NVARCHAR(100)) AS SomeValue;
END;
GO
