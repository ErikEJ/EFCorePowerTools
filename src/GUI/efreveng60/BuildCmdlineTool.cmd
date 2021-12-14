
dotnet publish -o bin\Release\net6.0\publish -f net6.0 -r win-x64 -c Release --no-self-contained

if %errorlevel% equ 1 goto notbuilt

del bin\Release\net6.0\publish\Azure.Core.dll
del bin\Release\net6.0\publish\Azure.Identity.dll 
del bin\Release\net6.0\publish\Ben.Demystifier.dll
del bin\Release\net6.0\publish\Bricelam.EntityFrameworkCore.Pluralizer.dll
del bin\Release\net6.0\publish\DacFxStrongTypedCore.dll 
del bin\Release\net6.0\publish\DacFxStrongTypedCore.pdb 
del bin\Release\net6.0\publish\Humanizer.dll
del bin\Release\net6.0\publish\Microsoft.Build.dll 
del bin\Release\net6.0\publish\Microsoft.Build.Framework.dll
del bin\Release\net6.0\publish\Microsoft.Build.Utilities.Core.dll 
del bin\Release\net6.0\publish\Microsoft.Data.SqlClient.dll 
del bin\Release\net6.0\publish\Microsoft.Data.SqlClient.SNI.dll 
del bin\Release\net6.0\publish\Microsoft.Data.SqlClient.SNI.pdb 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Schema.Tasks.Sql.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Schema.Sql.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Sql.DesignServices.dll 
del bin\Release\net6.0\publish\Microsoft.Data.Tools.Utilities.dll 
del bin\Release\net6.0\publish\Microsoft.Identity.Client.dll 
del bin\Release\net6.0\publish\Microsoft.Identity.Client.Extensions.Msal.dll 
del bin\Release\net6.0\publish\Microsoft.IdentityModel.JsonWebTokens.dll 
del bin\Release\net6.0\publish\Microsoft.IdentityModel.Logging.dll 
del bin\Release\net6.0\publish\Microsoft.IdentityModel.Protocols.dll 
del bin\Release\net6.0\publish\Microsoft.IdentityModel.Protocols.OpenIdConnect.dll 
del bin\Release\net6.0\publish\Microsoft.IdentityModel.Tokens.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.Dac.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.Dac.Extensions.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.TransactSql.ScriptDom.dll 
del bin\Release\net6.0\publish\Microsoft.SqlServer.Types.dll 
del bin\Release\net6.0\publish\Microsoft.Win32.SystemEvents.dll
del bin\Release\net6.0\publish\NetTopologySuite.dll
del bin\Release\net6.0\publish\NetTopologySuite.IO.SqlServerBytes.dll
del bin\Release\net6.0\publish\RevEng.Core.Abstractions.dll 
del bin\Release\net6.0\publish\RevEng.Core.Abstractions.pdb 
del bin\Release\net6.0\publish\RevEng.Shared.dll
del bin\Release\net6.0\publish\RevEng.Shared.pdb
del bin\Release\net6.0\publish\System.CodeDom.dll
del bin\Release\net6.0\publish\System.Configuration.ConfigurationManager.dll
del bin\Release\net6.0\publish\System.Drawing.Common.dll
del bin\Release\net6.0\publish\System.IdentityModel.Tokens.Jwt.dll
del bin\Release\net6.0\publish\System.IO.Packaging.dll 
del bin\Release\net6.0\publish\System.Memory.Data.dll 
del bin\Release\net6.0\publish\System.Resources.Extensions.dll
del bin\Release\net6.0\publish\System.Runtime.Caching.dll
del bin\Release\net6.0\publish\System.Security.Cryptography.ProtectedData.dll
del bin\Release\net6.0\publish\System.Security.Permissions.dll
del bin\Release\net6.0\publish\System.Windows.Extensions.dll

"C:\Program Files\7-Zip\7z.exe" -mm=Deflate -mfb=258 -mpass=15 a efreveng60.exe.zip .\bin\Release\net6.0\publish\*

move /Y efreveng60.exe.zip ..\lib\

goto end

:notbuilt
echo Build error

:end

pause
