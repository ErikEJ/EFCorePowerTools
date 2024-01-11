
dotnet publish -o bin\Release\net6.0\x64\publish -f net6.0 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

del bin\Release\net6.0\x64\publish\DacFxStrongTypedCore.161.dll
del bin\Release\net6.0\x64\publish\DacFxStrongTypedCore.161.pdb
del bin\Release\net6.0\x64\publish\Microsoft.Data.Tools.Schema.Sql.dll
del bin\Release\net6.0\x64\publish\Microsoft.Data.Tools.Schema.Tasks.Sql.dll
del bin\Release\net6.0\x64\publish\Microsoft.Data.Tools.Sql.DesignServices.dll
del bin\Release\net6.0\x64\publish\Microsoft.Data.Tools.Utilities.dll
del bin\Release\net6.0\x64\publish\Microsoft.SqlServer.Dac.dll
del bin\Release\net6.0\x64\publish\Microsoft.SqlServer.Dac.Extensions.dll
del bin\Release\net6.0\x64\publish\Microsoft.SqlServer.Server.dll
del bin\Release\net6.0\x64\publish\Microsoft.SqlServer.TransactSql.ScriptDom.dll

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng70.exe.zip .\bin\Release\net6.0\x64\publish\*

move /Y efreveng70.exe.zip ..\..\GUI\lib\

goto end

:notbuilt
echo Build error

:end
pause
