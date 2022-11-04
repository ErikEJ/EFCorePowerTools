using System.Collections.Generic;
using System.Linq;
using Bricelam.EntityFrameworkCore.Design;
using EdmxRuleGenerator.EdmxModel;
using EdmxRuleGenerator.Extensions;
using RevEng.Common.EnumMapping;
using RevEng.Common.PropertyRenaming;
using RevEng.Common.TableColumnRenaming;
using Schema = RevEng.Common.TableColumnRenaming.Schema;

namespace EdmxRuleGenerator;

public class EdmxRuleGenerator
{
    public EdmxRuleGenerator(EdmxParsed edmxParsed)
    {
        EdmxParsed = edmxParsed;
        Pluralizer = new Pluralizer();
    }

    public EdmxParsed EdmxParsed { get; }
    private Pluralizer Pluralizer { get; }


    public List<Schema> GetTableAndPropertyRenameRules()
    {
        var edmx = EdmxParsed;
        if (edmx?.Entities.IsNullOrEmpty() != false)
        {
            return new List<Schema>();
        }

        var rules = new List<Schema>();
        foreach (var grp in edmx.Entities.GroupBy(o => o.DbSchema))
        {
            if (grp.Key.IsNullOrWhiteSpace())
            {
                continue;
            }

            var rule = new Schema();
            rules.Add(rule);
            rule.SchemaName = grp.Key;
            rule.UseSchemaName = false; // will append schema name to entity name

            foreach (var entity in edmx.Entities.OrderBy(o => o.Name))
            {
                // if entity name is different than db, it has to go into output
                var tbl = new TableRenamer();
                var renamed = false;
                tbl.Name = entity.StorageEntity?.Name ?? entity.Name;
                tbl.NewName = entity.ConceptualEntity?.Name ?? tbl.Name;
                if (tbl.Name != tbl.NewName)
                {
                    tbl.NewName = entity.ConceptualEntity.Name;
                    renamed = true;
                }

                foreach (var property in entity.Properties)
                {
                    // if property name is different than db, it has to go into output
                    if (property.StorageProperty != null &&
                        property.PropertyName != property.StorageProperty.Name)
                    {
                        tbl.Columns ??= new List<ColumnNamer>();
                        tbl.Columns.Add(new ColumnNamer
                        {
                            Name = property.StorageProperty.Name, NewName = property.PropertyName
                        });
                        renamed = true;
                    }
                }

                if (renamed)
                {
                    rule.Tables.Add(tbl);
                }
            }
        }

        return rules;
    }

    public PropertyRenamingRoot GetNavigationRenameRules()
    {
        var edmx = EdmxParsed;
        var rule = new PropertyRenamingRoot();
        rule.Classes ??= new List<ClassRenamer>();

        if (edmx?.Entities.IsNullOrEmpty() != false)
        {
            return new PropertyRenamingRoot();
        }

        foreach (var entity in edmx.Entities.OrderBy(o => o.Name))
        {
            if (rule.Namespace == null)
            {
                rule.Namespace = entity.Namespace;
            }

            // if entity name is different than db, it has to go into output
            var tbl = new ClassRenamer();
            var renamed = false;
            tbl.Name = entity.StorageEntity?.Name ?? entity.Name;

            foreach (var navigation in entity.NavigationProperties)
            {
                if (navigation.Association == null)
                {
                    continue;
                }

                var ass = navigation.Association;
                var constraint = ass.ReferentialConstraints.FirstOrDefault();
                if (constraint == null)
                {
                    continue;
                }

                var deps = constraint.DependentProperties;
                var dep = deps?.FirstOrDefault();
                if (dep == null)
                {
                    continue;
                }

                var isMany = navigation.Multiplicity == Multiplicity.Many;

                var principalEntity = constraint.PrincipalEntity;
                var dependentEntity = constraint.DependentEntity;
                var isPrincipalEnd = principalEntity.Name == navigation.EntityName;
                var isDependentEnd = dependentEntity.Name == navigation.EntityName;

                var inverseEntity = isPrincipalEnd ? dependentEntity : principalEntity;
                var prefix = isDependentEnd ? string.Empty : inverseEntity.Name;
                string efCoreName;
                string altName;
                if (isMany)
                {
                    efCoreName = $"{prefix}{dep.PropertyName}Navigations";
                    altName = Pluralizer.Pluralize(navigation.ToRole.Entity.Name);
                }
                else
                {
                    efCoreName = $"{prefix}{dep.PropertyName}Navigation";
                    altName = navigation.ToRole.Entity.Name;
                }

                var newName = navigation.PropertyName;
                tbl.Properties ??= new List<PropertyRenamer>();
                tbl.Properties.Add(
                    new PropertyRenamer { Name = efCoreName, AlternateName = altName, NewName = newName });
                renamed = true;
            }

            if (renamed)
            {
                rule.Classes.Add(tbl);
            }
        }

        return rule;
    }

    public EnumMappingRoot GetEnumMappingRules()
    {
        var edmx = EdmxParsed;
        var rule = new EnumMappingRoot();
        rule.Classes ??= new List<EnumMappingClass>();

        if (edmx?.Entities.IsNullOrEmpty() != false)
        {
            return rule;
        }

        foreach (var entity in edmx.Entities.OrderBy(o => o.Name))
        {
            if (rule.Namespace == null)
            {
                rule.Namespace = entity.Namespace;
            }

            // if entity name is different than db, it has to go into output
            var tbl = new EnumMappingClass();
            var renamed = false;
            tbl.Name = entity.StorageEntity?.Name ?? entity.Name;

            foreach (var property in entity.Properties)
            {
                // if property name is different than db, it has to go into output
                if (property?.EnumType != null)
                {
                    tbl.Properties ??= new List<EnumMappingProperty>();
                    tbl.Properties.Add(new EnumMappingProperty
                    {
                        Name = property.StorageProperty.Name, EnumType = property.EnumType.FullName
                    });
                    renamed = true;
                }
            }

            if (renamed)
            {
                rule.Classes.Add(tbl);
            }
        }

        return rule;
    }
}
