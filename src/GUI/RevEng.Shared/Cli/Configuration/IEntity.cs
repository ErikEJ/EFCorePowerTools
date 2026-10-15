using System.Collections.Generic;

namespace RevEng.Common.Cli.Configuration;

public interface IEntity
{
    public string Name { get; set; }

    public bool? Exclude { get; set; }

    public string ExclusionWildcard { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    public List<string> ExcludedColumns { get; set; }

    public List<string> ExcludedIndexes { get; set; }

#pragma warning restore CA2227 // Collection properties should be read only
}