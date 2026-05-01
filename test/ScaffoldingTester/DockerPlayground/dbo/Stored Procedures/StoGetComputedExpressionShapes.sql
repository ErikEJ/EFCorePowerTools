CREATE PROCEDURE dbo.StoGetComputedExpressionShapes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CAST(1 + 2 AS INT) AS ArithmeticValue,
        CAST(CONCAT(N'a', N'b') AS NVARCHAR(20)) AS ConcatenatedText,
        CAST(CASE WHEN 1 = 1 THEN N'yes' ELSE N'no' END AS NVARCHAR(3)) AS CaseText,
        CAST(ISNULL(NULL, N'fallback') AS NVARCHAR(20)) AS IsNullText,
        CAST(COALESCE(NULL, 0) AS INT) AS CoalesceValue,
        CAST(GETDATE() AS DATETIME) AS CurrentDateTime,
        CAST(NEWID() AS UNIQUEIDENTIFIER) AS GeneratedId,
        CONVERT(VARCHAR(12), GETDATE(), 112) AS ConvertedDateText;
END;
GO
