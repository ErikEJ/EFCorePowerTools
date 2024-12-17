CREATE TABLE [dbo].[Artist] (
    [ArtistId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (120) NULL,
    CONSTRAINT [PK_Artist] PRIMARY KEY CLUSTERED ([ArtistId] ASC)
);


GO

