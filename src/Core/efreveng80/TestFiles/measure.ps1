(Measure-Command { .\efreveng 8 'C:\Code\EFCorePowerTools\src\GUI\TestCore\Dacpac\NorthwindProcedures.dacpac' | Out-Default }).ToString()
(Measure-Command { .\efreveng 3 'Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True' | Out-Default }).ToString()
