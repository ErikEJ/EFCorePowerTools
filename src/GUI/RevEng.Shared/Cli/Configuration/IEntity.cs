using System.Collections.Generic;

namespace RevEng.Common.Cli.Configuration;

public interface IEntity
{
    string Name { get; set; }

    bool? Exclude { get; set; }

    string ExclusionWildcard { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    List<string> ExcludedColumns { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}
