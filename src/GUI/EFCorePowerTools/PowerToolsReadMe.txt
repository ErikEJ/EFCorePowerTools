Congratulations, 'EF Core Power Tools' has now generated a DbContext and Entity classes for you.

You need to configure your app now - here are some hints:

## ASP.NET Core:

1. Register your data context class in your "Program.cs" file.

     builder.Services.AddDbContext<[ContextName]>(
        options => options.Use[ProviderName](builder.Configuration.GetConnectionString("DefaultConnection")[UseList]);

2. Add "ConnectionStrings" to your configuration file (secrets.json, appsettings.Development.json or appsettings.json).

    {
        "ConnectionStrings": {
            "DefaultConnection": "[ConnectionString]"
        }
    }
