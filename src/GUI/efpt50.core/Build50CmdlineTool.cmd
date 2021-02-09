
dotnet publish -o bin\Release\netcoreapp3.1\publish -f netcoreapp3.1 -c Release --no-self-contained

del bin\Release\netcoreapp3.1\publish\efpt.exe

"C:\Program Files\7-Zip\7z.exe" a efpt50.exe.zip .\bin\Release\netcoreapp3.1\publish\efpt.*

move /Y efpt50.exe.zip ..\lib\

pause
