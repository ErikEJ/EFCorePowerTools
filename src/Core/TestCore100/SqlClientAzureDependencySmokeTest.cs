using Microsoft.Data.SqlClient;
using Xunit;

namespace UnitTests;
public class SqlClientAzureDependencySmokeTest
{
    [Fact]
    public void ActiveDirectoryAuthenticationProviderIsAvailableAtRuntime()
    {
        // SqlClient 7 moved Entra/Azure authentication support out of the core package.
        // This smoke test verifies RevEng.Core.100 brings in the Azure extension package
        // so Entra auth modes remain available at runtime.
        var provider = new ActiveDirectoryAuthenticationProvider();

        Assert.True(provider.IsSupported(SqlAuthenticationMethod.ActiveDirectoryDefault));
        Assert.True(provider.IsSupported(SqlAuthenticationMethod.ActiveDirectoryManagedIdentity));
        Assert.True(provider.IsSupported(SqlAuthenticationMethod.ActiveDirectoryInteractive));
    }
}
