﻿using EFCorePowerTools.Shared.Models;
using Microsoft.SqlServer.Dac.Extensions.Prototype;
using Microsoft.SqlServer.Dac.Model;
using ReverseEngineer20.DacpacConsolidate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReverseEngineer20
{
    public class DacpacTableListBuilder
    {
        private readonly string _dacpacPath;

        public DacpacTableListBuilder(string dacpacPath)
        {
            if (string.IsNullOrEmpty(dacpacPath))
            {
                throw new ArgumentException(@"invalid path", nameof(dacpacPath));
            }
            if (!File.Exists(dacpacPath))
            {
                throw new ArgumentException("Dacpac file not found");
            }
            _dacpacPath = dacpacPath;
        }

        public List<TableInformationModel> GetTableDefinitions(bool includeViews)
        {
            var consolidator = new DacpacConsolidator();

            var dacpacPath = consolidator.Consolidate(_dacpacPath);

            using (var model = new TSqlTypedModel(dacpacPath))
            {
                var result = model.GetObjects<TSqlTable>(DacQueryScopes.UserDefined)
                            .Select(m => new TableInformationModel($"[{m.Name.Parts[0]}].[{m.Name.Parts[1]}]", m.PrimaryKeyConstraints.Any(), false))
                            .ToList();

                if (includeViews)
                {
                    var views = model.GetObjects<TSqlView>(DacQueryScopes.UserDefined)
                            .Select(m => new TableInformationModel($"[{m.Name.Parts[0]}].[{m.Name.Parts[1]}]", true, true))
                            .ToList();

                    result = result.Concat(views).ToList();
                }

                return result;
            }
        }
    }
}
