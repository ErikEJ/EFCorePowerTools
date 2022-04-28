You need to do some setup for your app after the `Reverse Engineer` created the Entity Model classes for you. Here are some hints:

## ASP.NET Core 6.0

1. Register your data context class to `IServiceCollection` in your `Program.cs` file.

    ```cs
    builder.Services.Add[ProviderName]<[ContextName]>(builder.Configuration.GetConnectionString("DefaultConnection"));
    ```

2. Add `ConnectionStrings`

    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "[ConnectionString]"
        }
    }
    ```

## ASP.NET Core 5.0

1. Register your data context class to `IServiceCollection` in your `Startup.cs` file.

    ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<[ContextName]>(options => {
            options.Use[ProviderName](Configuration.GetConnectionString("DefaultConnection"));
        });
    }
    ```

2. Add `ConnectionStrings`

    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "[ConnectionString]"
        }
    }
    ```

