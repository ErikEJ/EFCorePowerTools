CREATE TABLE dbo.UqTableTest (
    UqTableTestId  INT NOT NULL IDENTITY (1000, 1),
    DisplayName    VARCHAR(101) NOT NULL,
    EmployeeNumber INT     NULL,
    IsActive       BIT NOT NULL,
    CONSTRAINT PK_dbo_UqTableTest PRIMARY KEY CLUSTERED (UqTableTestId),
)
GO

CREATE UNIQUE NONCLUSTERED INDEX UQ_dbo_UqTableTest_EmployeeNumber
ON dbo.UqTableTest (EmployeeNumber)
WHERE ([EmployeeNumber] IS NOT NULL);
GO

CREATE UNIQUE NONCLUSTERED INDEX UQ_dbo_UqTableTest_DisplayName
ON dbo.UqTableTest (DisplayName) WHERE ([IsActive] = (1));
GO