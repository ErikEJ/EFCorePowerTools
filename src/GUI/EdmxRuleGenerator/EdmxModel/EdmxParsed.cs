using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace EdmxRuleGenerator.EdmxModel;

[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Exported type")]
public sealed class EdmxParsed
{
    internal EdmxParsed(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }
    public Dictionary<string, Association> AssociationsByName { get; internal set; }
    public Dictionary<string, EnumType> EnumsByName { get; internal set; }
    public Dictionary<string, EnumType> EnumsBySchemaName { get; internal set; }
    public Dictionary<string, EnumType> EnumsByExternalTypeName { get; internal set; }

    public ObservableCollection<Schema> Schemas { get; } = new();

    public ObservableCollection<EntityType> Entities { get; } = new();

    public ObservableCollection<NavigationProperty> NavProps { get; } = new();

    public ObservableCollection<EntityProperty> Props { get; } = new();

    public ObservableCollection<EndRole> EndRoles { get; } = new();
}
