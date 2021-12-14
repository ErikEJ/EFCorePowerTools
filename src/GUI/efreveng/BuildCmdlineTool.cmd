
dotnet publish -o bin\Release\netcoreapp3.1\publish -f netcoreapp3.1 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

rem del bin\Release\netcoreapp3.1\publish\Microsoft.Data.Tools.Schema.Sql.dll 
rem del bin\Release\netcoreapp3.1\publish\Microsoft.Data.Tools.Utilities.dll 
rem del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.Dac.dll 
rem del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.Dac.Extensions.dll 
rem del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.TransactSql.ScriptDom.dll 
rem del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.Types.dll 

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng.exe.zip .\bin\Release\netcoreapp3.1\publish\*

move /Y efreveng.exe.zip ..\lib\

goto end

:notbuilt
echo Build error

:end

pause
