CREATE TABLE [dbo].[Employee] (
    [EmployeeId] INT           IDENTITY (1, 1) NOT NULL,
    [LastName]   NVARCHAR (20) NOT NULL,
    [FirstName]  NVARCHAR (20) NOT NULL,
    [Title]      NVARCHAR (30) NULL,
    [ReportsTo]  INT           NULL,
    [BirthDate]  DATETIME      NULL,
    [HireDate]   DATETIME      NULL,
    [Address]    NVARCHAR (70) NULL,
    [City]       NVARCHAR (40) NULL,
    [State]      NVARCHAR (40) NULL,
    [Country]    NVARCHAR (40) NULL,
    [PostalCode] NVARCHAR (10) NULL,
    [Phone]      NVARCHAR (24) NULL,
    [Fax]        NVARCHAR (24) NULL,
    [Email]      NVARCHAR (60) NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([EmployeeId] ASC),
    CONSTRAINT [FK_EmployeeReportsTo] FOREIGN KEY ([ReportsTo]) REFERENCES [dbo].[Employee] ([EmployeeId])
);


GO

CREATE NONCLUSTERED INDEX [IFK_EmployeeReportsTo]
    ON [dbo].[Employee]([ReportsTo] ASC);


GO

