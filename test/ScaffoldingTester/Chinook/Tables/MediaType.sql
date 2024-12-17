CREATE TABLE [dbo].[MediaType] (
    [MediaTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (120) NULL,
    CONSTRAINT [PK_MediaType] PRIMARY KEY CLUSTERED ([MediaTypeId] ASC)
);


GO

