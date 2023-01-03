
dotnet publish -o bin\Release\net6.0\publish -f net6.0 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

del bin\Release\net6.0\publish\Ben.Demystifier.dll
del bin\Release\net6.0\publish\Bricelam.EntityFrameworkCore.Pluralizer.dll
del bin\Release\net6.0\publish\DacFxStrongTypedCore.dll 
del bin\Release\net6.0\publish\DacFxStrongTypedCore.pdb 
del bin\Release\net6.0\publish\Humanizer.dll
del bin\Release\net6.0\publish\Microsoft.Build.dll 
del bin\Release\net6.0\publish\Microsoft.Build.Utilities.Core.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Schema.Tasks.Sql.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Schema.Sql.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Sql.DesignServices.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Utilities.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.Dac.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.Dac.Extensions.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.TransactSql.ScriptDom.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.Types.dll 
del bin\Release\net6.0\publish\NetTopologySuite.dll
del bin\Release\net6.0\publish\NetTopologySuite.IO.SqlServerBytes.dll

del bin\Release\net6.0\publish\sni.dll 
del bin\Release\net6.0\publish\System.Data.SqlClient.dll

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng60.exe.zip .\bin\Release\net6.0\publish\*

move /Y efreveng60.exe.zip ..\lib\

goto end

:notbuilt
echo Build error

:end

pause
