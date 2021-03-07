
dotnet publish -o bin\Release\netcoreapp3.1\publish -f netcoreapp3.1 -r win-x64 -c Release --no-self-contained

rmdir bin\Release\netcoreapp3.1\publish\cs /S /Q
rmdir bin\Release\netcoreapp3.1\publish\de /S /Q
rmdir bin\Release\netcoreapp3.1\publish\es /S /Q
rmdir bin\Release\netcoreapp3.1\publish\fr /S /Q
rmdir bin\Release\netcoreapp3.1\publish\it /S /Q
rmdir bin\Release\netcoreapp3.1\publish\ja /S /Q
rmdir bin\Release\netcoreapp3.1\publish\ko /S /Q
rmdir bin\Release\netcoreapp3.1\publish\pl /S /Q
rmdir bin\Release\netcoreapp3.1\publish\ru /S /Q
rmdir bin\Release\netcoreapp3.1\publish\tr /S /Q
rmdir bin\Release\netcoreapp3.1\publish\zh-Hans /S /Q
rmdir bin\Release\netcoreapp3.1\publish\zh-Hant /S /Q
rmdir bin\Release\netcoreapp3.1\publish\de-DE /S /Q
rmdir bin\Release\netcoreapp3.1\publish\es-ES /S /Q
rmdir bin\Release\netcoreapp3.1\publish\fr-FR /S /Q
rmdir bin\Release\netcoreapp3.1\publish\it-IT /S /Q
rmdir bin\Release\netcoreapp3.1\publish\ja-JP /S /Q
rmdir bin\Release\netcoreapp3.1\publish\ko-KR /S /Q
rmdir bin\Release\netcoreapp3.1\publish\pt-BR /S /Q
rmdir bin\Release\netcoreapp3.1\publish\ru-RU /S /Q
rmdir bin\Release\netcoreapp3.1\publish\zh-CN /S /Q
rmdir bin\Release\netcoreapp3.1\publish\zh-TW /S /Q

"C:\Program Files\7-Zip\7z.exe" a efreveng.exe.zip .\bin\Release\netcoreapp3.1\publish\*

move /Y efreveng.exe.zip ..\lib\

pause
