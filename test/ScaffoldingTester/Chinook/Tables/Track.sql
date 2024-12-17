CREATE TABLE [dbo].[Track] (
    [TrackId]      INT             IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (200)  NOT NULL,
    [AlbumId]      INT             NULL,
    [MediaTypeId]  INT             NOT NULL,
    [GenreId]      INT             NULL,
    [Composer]     NVARCHAR (220)  NULL,
    [Milliseconds] INT             NOT NULL,
    [Bytes]        INT             NULL,
    [UnitPrice]    NUMERIC (10, 2) NOT NULL,
    CONSTRAINT [PK_Track] PRIMARY KEY CLUSTERED ([TrackId] ASC),
    CONSTRAINT [FK_TrackAlbumId] FOREIGN KEY ([AlbumId]) REFERENCES [dbo].[Album] ([AlbumId]),
    CONSTRAINT [FK_TrackGenreId] FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genre] ([GenreId]),
    CONSTRAINT [FK_TrackMediaTypeId] FOREIGN KEY ([MediaTypeId]) REFERENCES [dbo].[MediaType] ([MediaTypeId])
);


GO

CREATE NONCLUSTERED INDEX [IFK_TrackAlbumId]
    ON [dbo].[Track]([AlbumId] ASC);


GO

CREATE NONCLUSTERED INDEX [IFK_TrackGenreId]
    ON [dbo].[Track]([GenreId] ASC);


GO

CREATE NONCLUSTERED INDEX [IFK_TrackMediaTypeId]
    ON [dbo].[Track]([MediaTypeId] ASC);


GO

