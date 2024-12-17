CREATE TABLE [dbo].[Playlist] (
    [PlaylistId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (120) NULL,
    CONSTRAINT [PK_Playlist] PRIMARY KEY CLUSTERED ([PlaylistId] ASC)
);


GO

