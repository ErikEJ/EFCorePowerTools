CREATE OR ALTER PROCEDURE [dbo].SP_GET_TOP_IDS
(
	@Top			int,
	@OverallCount	INT OUTPUT
)

AS
BEGIN
    SET @OverallCount = (SELECT COUNT(*) FROM dbo.Customers)
	SELECT TOP (@Top) CustomerId FROM dbo.Customers
END
