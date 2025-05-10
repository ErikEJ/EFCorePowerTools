CREATE PROCEDURE dbo.StoGetStatusMetricCounts
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #StatusCounts (
        Column1 VARCHAR(10) NOT NULL,
        Column2 VARCHAR(MAX) NOT NULL
    );

    SELECT
        Column1,
        Column2
    FROM #StatusCounts;
END;