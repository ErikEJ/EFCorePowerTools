CREATE PROC [dbo].[USP_Error]
AS
BEGIN
    WITH
        TempResult
        AS
        (
            SELECT abc.Id [TestColumn]
            FROM [dbo].[Abc] abc
        )

    SELECT [TestColumn]
    FROM TempResult
END