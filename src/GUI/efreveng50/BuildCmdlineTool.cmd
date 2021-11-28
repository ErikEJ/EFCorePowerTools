
dotnet publish -o bin\Release\netcoreapp3.1\publish -f netcoreapp3.1 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

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

del bin\Release\netcoreapp3.1\publish\Ben.Demystifier.dll
del bin\Release\netcoreapp3.1\publish\Bricelam.EntityFrameworkCore.Pluralizer.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.DotNet.PlatformAbstractions.dll
del bin\Release\netcoreapp3.1\publish\DacFxStrongTypedCore.dll 
del bin\Release\netcoreapp3.1\publish\DacFxStrongTypedCore.pdb 
del bin\Release\netcoreapp3.1\publish\ErikEJ.EntityFrameworkCore.SqlServer.Dacpac.dll 
del bin\Release\netcoreapp3.1\publish\ErikEJ.EntityFrameworkCore.SqlServer.Dacpac.pdb 
del bin\Release\netcoreapp3.1\publish\Humanizer.dll 
del bin\Release\netcoreapp3.1\publish\JetBrains.Annotations.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Build.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Build.Framework.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Data.SqlClient.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Data.SqlClient.SNI.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Data.SqlClient.SNI.pdb 
del bin\Release\netcoreapp3.1\publish\Microsoft.Data.Tools.Schema.Sql.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Data.Tools.Utilities.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Identity.Client.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.IdentityModel.JsonWebTokens.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.IdentityModel.Logging.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.IdentityModel.Protocols.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.IdentityModel.Protocols.OpenIdConnect.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.IdentityModel.Tokens.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.Dac.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.Dac.Extensions.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.TransactSql.ScriptDom.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.SqlServer.Types.dll 
del bin\Release\netcoreapp3.1\publish\Microsoft.Win32.SystemEvents.dll 
del bin\Release\netcoreapp3.1\publish\NetTopologySuite.dll 
del bin\Release\netcoreapp3.1\publish\NetTopologySuite.IO.PostGis.dll 
del bin\Release\netcoreapp3.1\publish\NetTopologySuite.IO.SqlServerBytes.dll 
del bin\Release\netcoreapp3.1\publish\RevEng.Shared.dll 
del bin\Release\netcoreapp3.1\publish\RevEng.Shared.pdb 
del bin\Release\netcoreapp3.1\publish\System.ComponentModel.Composition.dll
del bin\Release\netcoreapp3.1\publish\System.CodeDom.dll
del bin\Release\netcoreapp3.1\publish\System.Configuration.ConfigurationManager.dll 
del bin\Release\netcoreapp3.1\publish\System.Drawing.Common.dll 
del bin\Release\netcoreapp3.1\publish\System.IdentityModel.Tokens.Jwt.dll 
del bin\Release\netcoreapp3.1\publish\System.Runtime.Caching.dll 
del bin\Release\netcoreapp3.1\publish\System.Security.Cryptography.ProtectedData.dll 
del bin\Release\netcoreapp3.1\publish\System.Security.Permissions.dll 
del bin\Release\netcoreapp3.1\publish\System.Windows.Extensions.dll
del bin\Release\netcoreapp3.1\publish\System.IO.Packaging.dll
del bin\Release\netcoreapp3.1\publish\System.Resources.Extensions.dll

del bin\Release\netcoreapp3.1\Azure.Core.dll 
del bin\Release\netcoreapp3.1\Azure.Identity.dll 
del bin\Release\netcoreapp3.1\Microsoft.Build.Utilities.Core.dll 
del bin\Release\netcoreapp3.1\Microsoft.Data.Tools.Schema.Tasks.Sql.dll 
del bin\Release\netcoreapp3.1\Microsoft.Data.Tools.Sql.DesignServices.dll 
del bin\Release\netcoreapp3.1\Microsoft.Identity.Client.Extensions.Msal.dll 
del bin\Release\netcoreapp3.1\RevEng.Core.Abstractions.dll 
del bin\Release\netcoreapp3.1\RevEng.Core.Abstractions.pdb 
del bin\Release\netcoreapp3.1\System.Collections.Immutable.dll 
del bin\Release\netcoreapp3.1\System.Memory.Data.dll 
del bin\Release\netcoreapp3.1\System.Reflection.Metadata.dll

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng50.exe.zip .\bin\Release\netcoreapp3.1\publish\*

move /Y efreveng50.exe.zip ..\lib\

goto end

:notbuilt
echo Build error

:end
pause