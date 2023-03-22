
dotnet publish -o bin\Release\net7.0\publish -f net7.0 -c Release --no-self-contained

del bin\Release\net7.0\publish\efpt.exe

"C:\Program Files\7-Zip\7z.exe" a efpt70sc.exe.zip .\bin\Release\net7.0\publish\efpt.*

move /Y efpt70sc.exe.zip ..\lib\

pause
