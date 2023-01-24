
dotnet publish -o bin\Release\net6.0\x64\publish -f net6.0 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng60.exe.zip .\bin\Release\net6.0\x64\publish\*

move /Y efreveng60.exe.zip ..\lib\

dotnet publish -o bin\Release\net6.0\arm64\publish -f net6.0 -r win-arm64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng60arm.exe.zip .\bin\Release\net6.0\arm64\publish\*

move /Y efreveng60arm.exe.zip ..\lib\

goto end

:notbuilt
echo Build error

:end

pause
