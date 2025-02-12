using System;
using System.Collections.Generic;

namespace SqlServer.Rules.Report
{
    public class IssueTypeComparer : IEqualityComparer<IssueType>
    {
        public bool Equals(IssueType x, IssueType y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            return x.Id == y.Id;
        }

        public int GetHashCode(IssueType obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            return obj.Id.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }
    }
}