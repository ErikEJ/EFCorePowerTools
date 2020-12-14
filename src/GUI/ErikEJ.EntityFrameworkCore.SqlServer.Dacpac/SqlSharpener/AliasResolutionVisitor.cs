﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlSharpener
{
    [Serializable]
    internal class AliasResolutionVisitor : TSqlFragmentVisitor
    {
        readonly Dictionary<string, string> aliases = new Dictionary<string, string>();
        public Dictionary<string, string> Aliases { get { return aliases; } }

        public override void Visit(NamedTableReference namedTableReference)
        {
            var alias = namedTableReference.Alias;
            if (alias != null)
            {
                var baseObjectName = string.Join(".", namedTableReference.SchemaObject.Identifiers.Select(x => x.Value));
                aliases.Add(alias.Value, baseObjectName);
            }
        }
    }
}
