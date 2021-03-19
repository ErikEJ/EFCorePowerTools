
dotnet publish -o bin\Release\net5.0\publish -f net5.0 -c Release --no-self-contained

del bin\Release\net5.0\publish\efpt.exe

"C:\Program Files\7-Zip\7z.exe" a efpt60.exe.zip .\bin\Release\net5.0\publish\efpt.*

move /Y efpt60.exe.zip ..\lib\

pause
