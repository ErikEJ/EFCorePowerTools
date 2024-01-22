using System.Collections.Generic;

namespace RevEng.Common.Cli.Configuration;

public interface IEntity
{
    string Name { get; set; }

    bool? Exclude { get; set; }

    string ExclusionWildcard { get; set; }

    List<string> ExcludedColumns { get; set; }
}
