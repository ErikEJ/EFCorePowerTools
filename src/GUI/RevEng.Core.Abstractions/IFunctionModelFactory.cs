using RevEng.Core.Abstractions.Metadata;

namespace RevEng.Core.Abstractions.Model
{
    public interface IFucntionModelFactory
    {
        FunctionModel Create(string connectionString, FunctionModelFactoryOptions options);
    }
}
