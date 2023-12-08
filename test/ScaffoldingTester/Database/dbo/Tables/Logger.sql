CREATE TABLE [dbo].[Logger]
(
	[Id] NVARCHAR(10) NOT NULL, 
    [CompanyId] NCHAR(2) NOT NULL,
    [Location] GEOGRAPHY NULL,
    [Parents] HierarchyId NULL,
    [LocationChanged] DATETIMEOFFSET NULL, 
    [BatteryLevel] INT NULL, 
    [SignalStrength] INT NULL, 
    [MeasurementsChanged] DATETIMEOFFSET NULL,
    [TestCol] [sys].[sysname] NULL, 
    CONSTRAINT [PK_Loggers] PRIMARY KEY CLUSTERED ([Id])
);
