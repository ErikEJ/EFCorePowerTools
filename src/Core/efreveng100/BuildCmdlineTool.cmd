
dotnet publish -o bin\Release\net10.0\x64\publish -f net10.0 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

del bin\Release\net10.0\x64\publish\Microsoft.Data.Tools.Schema.Sql.dll
del bin\Release\net10.0\x64\publish\Microsoft.Data.Tools.Schema.Tasks.Sql.dll
del bin\Release\net10.0\x64\publish\Microsoft.Data.Tools.Sql.DesignServices.dll
del bin\Release\net10.0\x64\publish\Microsoft.Data.Tools.Utilities.dll
del bin\Release\net10.0\x64\publish\Microsoft.SqlServer.Dac.dll
del bin\Release\net10.0\x64\publish\Microsoft.SqlServer.Dac.Extensions.dll
del bin\Release\net10.0\x64\publish\Microsoft.SqlServer.Server.dll
del bin\Release\net10.0\x64\publish\Microsoft.SqlServer.TransactSql.ScriptDom.dll

rd bin\Release\net10.0\x64\publish\runtimes\browser-wasm /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-arm /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-arm64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-armel /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-mips64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-musl-arm /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-musl-arm64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-musl-riscv64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-musl-s390x /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-musl-x64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-ppc64le /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-riscv64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-s390x /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-x64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\linux-x86 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\maccatalyst-arm64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\maccatalyst-x64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\osx-arm64 /Q /S 
rd bin\Release\net10.0\x64\publish\runtimes\osx-x64 /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\unix /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\win-arm /Q /S
rd bin\Release\net10.0\x64\publish\runtimes\win-x86 /Q /S

rd bin\Release\net10.0\x64\publish\BuildHost-net472 /Q /S
rd bin\Release\net10.0\x64\publish\BuildHost-netcore /Q /S

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng100.exe.zip .\bin\Release\net10.0\x64\publish\*

move /Y efreveng100.exe.zip ..\..\GUI\lib\

goto end

:notbuilt
echo Build error

:end
