
dotnet publish -o bin\Release\net5.0\publish -f net5.0 -r win-x64 -c Release --no-self-contained

rmdir bin\Release\net5.0\publish\cs /S /Q
rmdir bin\Release\net5.0\publish\de /S /Q
rmdir bin\Release\net5.0\publish\es /S /Q
rmdir bin\Release\net5.0\publish\fr /S /Q
rmdir bin\Release\net5.0\publish\it /S /Q
rmdir bin\Release\net5.0\publish\ja /S /Q
rmdir bin\Release\net5.0\publish\ko /S /Q
rmdir bin\Release\net5.0\publish\pl /S /Q
rmdir bin\Release\net5.0\publish\ru /S /Q
rmdir bin\Release\net5.0\publish\tr /S /Q
rmdir bin\Release\net5.0\publish\zh-Hans /S /Q
rmdir bin\Release\net5.0\publish\zh-Hant /S /Q
rmdir bin\Release\net5.0\publish\de-DE /S /Q
rmdir bin\Release\net5.0\publish\es-ES /S /Q
rmdir bin\Release\net5.0\publish\fr-FR /S /Q
rmdir bin\Release\net5.0\publish\it-IT /S /Q
rmdir bin\Release\net5.0\publish\ja-JP /S /Q
rmdir bin\Release\net5.0\publish\ko-KR /S /Q
rmdir bin\Release\net5.0\publish\pt-BR /S /Q
rmdir bin\Release\net5.0\publish\ru-RU /S /Q
rmdir bin\Release\net5.0\publish\zh-CN /S /Q
rmdir bin\Release\net5.0\publish\zh-TW /S /Q

del bin\Release\net5.0\publish\Ben.Demystifier.dll
del bin\Release\net5.0\publish\Bricelam.EntityFrameworkCore.Pluralizer.dll
del bin\Release\net5.0\publish\Humanizer.dll
del bin\Release\net5.0\publish\Microsoft.Data.SqlClient.dll
del bin\Release\net5.0\publish\Microsoft.Data.SqlClient.SNI.dll
del bin\Release\net5.0\publish\Microsoft.Identity.Client.dll
del bin\Release\net5.0\publish\Microsoft.IdentityModel.JsonWebTokens.dll
del bin\Release\net5.0\publish\Microsoft.IdentityModel.Logging.dll
del bin\Release\net5.0\publish\Microsoft.IdentityModel.Protocols.dll
del bin\Release\net5.0\publish\Microsoft.IdentityModel.Protocols.OpenIdConnect.dll
del bin\Release\net5.0\publish\Microsoft.IdentityModel.Tokens.dll
del bin\Release\net5.0\publish\Microsoft.Win32.SystemEvents.dll
del bin\Release\net5.0\publish\NetTopologySuite.dll
del bin\Release\net5.0\publish\NetTopologySuite.IO.SqlServerBytes.dll
del bin\Release\net5.0\publish\RevEng.Shared.dll
del bin\Release\net5.0\publish\RevEng.Shared.pdb
del bin\Release\net5.0\publish\System.CodeDom.dll
del bin\Release\net5.0\publish\System.Configuration.ConfigurationManager.dll
del bin\Release\net5.0\publish\System.Drawing.Common.dll
del bin\Release\net5.0\publish\System.IdentityModel.Tokens.Jwt.dll
del bin\Release\net5.0\publish\System.Runtime.Caching.dll
del bin\Release\net5.0\publish\System.Security.Cryptography.ProtectedData.dll
del bin\Release\net5.0\publish\System.Security.Permissions.dll
del bin\Release\net5.0\publish\System.Windows.Extensions.dll

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng60.exe.zip .\bin\Release\net5.0\publish\*

move /Y efreveng60.exe.zip ..\lib\

pause
