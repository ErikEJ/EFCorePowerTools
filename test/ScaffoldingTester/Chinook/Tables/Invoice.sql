CREATE TABLE [dbo].[Invoice] (
    [InvoiceId]         INT             IDENTITY (1, 1) NOT NULL,
    [CustomerId]        INT             NOT NULL,
    [InvoiceDate]       DATETIME        NOT NULL,
    [BillingAddress]    NVARCHAR (70)   NULL,
    [BillingCity]       NVARCHAR (40)   NULL,
    [BillingState]      NVARCHAR (40)   NULL,
    [BillingCountry]    NVARCHAR (40)   NULL,
    [BillingPostalCode] NVARCHAR (10)   NULL,
    [Total]             NUMERIC (10, 2) NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    CONSTRAINT [FK_InvoiceCustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);


GO

CREATE NONCLUSTERED INDEX [IFK_InvoiceCustomerId]
    ON [dbo].[Invoice]([CustomerId] ASC);


GO

