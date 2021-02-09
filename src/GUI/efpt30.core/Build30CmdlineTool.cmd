
dotnet publish -o bin\Release\netcoreapp3.0\publish -f netcoreapp3.0 -c Release --no-self-contained

"C:\Program Files\7-Zip\7z.exe" a efpt30.exe.zip .\bin\Release\netcoreapp3.0\publish\efpt.*

move /Y efpt30.exe.zip ..\lib\

pause
