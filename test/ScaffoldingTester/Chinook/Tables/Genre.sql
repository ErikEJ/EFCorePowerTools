CREATE TABLE [dbo].[Genre] (
    [GenreId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (120) NULL,
    CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED ([GenreId] ASC)
);


GO

