CREATE TABLE [dbo].[PlaylistTrack] (
    [PlaylistId] INT NOT NULL,
    [TrackId]    INT NOT NULL,
    CONSTRAINT [PK_PlaylistTrack] PRIMARY KEY NONCLUSTERED ([PlaylistId] ASC, [TrackId] ASC),
    CONSTRAINT [FK_PlaylistTrackPlaylistId] FOREIGN KEY ([PlaylistId]) REFERENCES [dbo].[Playlist] ([PlaylistId]),
    CONSTRAINT [FK_PlaylistTrackTrackId] FOREIGN KEY ([TrackId]) REFERENCES [dbo].[Track] ([TrackId])
);


GO

CREATE NONCLUSTERED INDEX [IFK_PlaylistTrackTrackId]
    ON [dbo].[PlaylistTrack]([TrackId] ASC);


GO

