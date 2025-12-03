
dotnet publish -o bin\Release\net10.0\publish -f net10.0 -c Release --no-self-contained

del bin\Release\net10.0\publish\efpt.exe

"C:\Program Files\7-Zip\7z.exe" a efpt100.exe.zip .\bin\Release\net10.0\publish\efpt.*

move /Y efpt100.exe.zip ..\..\GUI\lib\

pause
