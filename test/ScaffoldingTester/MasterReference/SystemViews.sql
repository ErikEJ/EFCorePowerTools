CREATE PROCEDURE [dbo].[GetSystemTables]
AS
BEGIN
    SELECT [name], [object_id] FROM [sys].[tables];
END;
GO

CREATE PROCEDURE [dbo].[GetMasterFiles]
AS
BEGIN
    SELECT [name], [database_id], [file_id] FROM [master].[sys].[master_files];
END;
