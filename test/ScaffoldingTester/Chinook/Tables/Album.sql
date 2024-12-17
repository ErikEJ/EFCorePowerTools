CREATE TABLE [dbo].[Album] (
    [AlbumId]  INT            IDENTITY (1, 1) NOT NULL,
    [Title]    NVARCHAR (160) NOT NULL,
    [ArtistId] INT            NOT NULL,
    CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED ([AlbumId] ASC),
    CONSTRAINT [FK_AlbumArtistId] FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artist] ([ArtistId])
);


GO

CREATE NONCLUSTERED INDEX [IFK_AlbumArtistId]
    ON [dbo].[Album]([ArtistId] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Title of album', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Album', @level2type = N'COLUMN', @level2name = N'Title';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Album table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Album';


GO

