CREATE TABLE dbo.TriggerTest (
    TriggerTestId  INT NOT NULL IDENTITY (1, 1)
);
GO

CREATE TRIGGER dbo.TrTriggerTestAfterInsertUpdate
ON dbo.TriggerTest
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
END
GO