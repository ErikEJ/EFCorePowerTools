
dotnet publish -o bin\Release\netcoreapp3.1\publish -f netcoreapp3.1 -c Release --no-self-contained

del bin\Release\netcoreapp3.1\publish\efpt.exe

"C:\Program Files\7-Zip\7z.exe" a efpt30.exe.zip .\bin\Release\netcoreapp3.1\publish\efpt.*

move /Y efpt30.exe.zip ..\lib\

pause
