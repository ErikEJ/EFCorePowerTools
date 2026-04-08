using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class SqlClientAzureDependencySmokeTest
{
    [Test]
    public void ActiveDirectoryAuthenticationProviderIsAvailableAtRuntime()
    {
        // SqlClient 7 moved Entra/Azure authentication support out of the core package.
        // This smoke test verifies RevEng.Core.100 brings in the Azure extension package
        // so Entra auth modes remain available at runtime.
        var provider = new ActiveDirectoryAuthenticationProvider();

        Assert.Multiple(() =>
        {
            Assert.That(provider.IsSupported(SqlAuthenticationMethod.ActiveDirectoryDefault), Is.True);
            Assert.That(provider.IsSupported(SqlAuthenticationMethod.ActiveDirectoryManagedIdentity), Is.True);
            Assert.That(provider.IsSupported(SqlAuthenticationMethod.ActiveDirectoryInteractive), Is.True);
        });
    }
}
