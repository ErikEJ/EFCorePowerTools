
dotnet publish -o bin\Release\net6.0\publish -f net6.0 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng70.exe.zip .\bin\Release\net6.0\publish\*

move /Y efreveng70.exe.zip ..\lib\

goto end

:notbuilt
echo Build error

:end

pause
