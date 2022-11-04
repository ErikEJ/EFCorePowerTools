using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using EdmxRuleGenerator.Extensions;

namespace EdmxRuleGenerator.EdmxModel;

public sealed class EdmxParser : NotifyPropertyChanged
{
    public static EdmxParsed Parse(string filePath)
    {
        return new EdmxParser(filePath).State;
    }

    private EdmxParser(string fileInstancePath)
    {
        State = new EdmxParsed(fileInstancePath);
        if (!File.Exists(fileInstancePath))
        {
            throw new InvalidDataException($"Could not find file {fileInstancePath}");
        }

        var edmxModel = EdmxSerializer.Deserialize(File.ReadAllText(fileInstancePath));
        var edmx = edmxModel.Runtime;

        // weave the model data together starting with enums, then on to the conceptual entities
        var enums = edmx.ConceptualModels.Schema.EnumTypes
            .Select(enumItem => new EnumType(enumItem, GetSchema(edmx.ConceptualModels.Schema.Namespace))).ToArray();
        State.EnumsByName = enums.ToDictionary(o => o.Name);
        State.EnumsBySchemaName = enums.Where(o => o.Schema?.Namespace.HasNonWhiteSpace() == true)
            .ToDictionary(o => o.Schema.Namespace + "." + o.Name);
        State.EnumsByExternalTypeName = enums.Where(o => o.ExternalTypeName.HasNonWhiteSpace())
            .ToDictionary(o => o.ExternalTypeName);

        foreach (var conceptualEntityType in edmx.ConceptualModels.Schema.EntityTypes)
        {
            var entity = new EntityType(conceptualEntityType, GetSchema(edmx.ConceptualModels.Schema.Namespace));
            Entities.Add(entity);

            var fullName = entity.FullName;
            var selfName = entity.SelfName;
            var setMapping = edmx.Mappings.Mapping.EntityContainerMapping.EntitySetMappings.FirstOrDefault(o =>
                o.EntityTypeMappings.Any(etm =>
                    string.Equals(etm.TypeName, fullName, StringComparison.OrdinalIgnoreCase)));
            entity.EntitySetMapping = setMapping;
            entity.MappingFragments = setMapping.EntityTypeMappings
                .FirstOrDefault(etm => string.Equals(etm.TypeName, fullName, StringComparison.OrdinalIgnoreCase))
                ?.MappingFragments;

            if (entity.MappingFragments?.Count > 0)
            {
                entity.StoreEntitySetNames = entity.MappingFragments.Select(o => o.StoreEntitySet).Distinct().ToArray();

                entity.StorageContainer = edmx.StorageModels.Schema.EntityContainers.FirstOrDefault(o =>
                    o.EntitySets.Any(es => entity.StoreEntitySetNames.Contains(es.Name)));
                entity.StorageEntitySet = entity.StorageContainer?.EntitySets
                    .FirstOrDefault(o => entity.StoreEntitySetNames.Contains(o.Name));
                if (entity.StorageEntitySet?.Name != null)
                {
                    entity.StorageEntity =
                        edmx.StorageModels.Schema.EntityTypes.FirstOrDefault(
                            o => o.Name == entity.StorageEntitySet.Name);
                }
            }

            // populate associations that hit this entity
            entity.Associations =
                edmx.ConceptualModels.Schema.Associations.Where(conceptualAssociation =>
                    conceptualAssociation.Ends.Any(conceptualEnd =>
                        conceptualEnd.Type == fullName || conceptualEnd.Type == selfName ||
                        conceptualEnd.Role == entity.Name)).ToList();

            // link properties to storage elements
            foreach (var conceptualProperty in conceptualEntityType.Properties)
            {
                var property = new EntityProperty(entity, conceptualProperty);
                entity.Properties.Add(property);
                if (entity.MappingFragments?.Count > 0)
                {
                    property.Mapping = entity.MappingFragments.SelectMany(o => o.ScalarProperties)
                        .Single(o => o.Name == property.PropertyName);
                    if (property.Mapping?.ColumnName != null && entity.StorageEntity?.Properties?.Count > 0)
                    {
                        property.StorageProperty =
                            entity.StorageEntity.Properties.Single(o => o.Name == property.Mapping.ColumnName);
                    }
                }

                Debug.Assert(property.StorageProperty != null);
                Props.Add(property);

                // link up enums: 
                if (EnumsByExternalTypeName.TryGetValue(property.TypeName, out var enumType))
                {
                    property.EnumType = enumType;
                    enumType.Properties.Add(property);
                }
                else if (entity.Schema?.Namespace?.HasNonWhiteSpace() == true &&
                         property.TypeName.StartsWith(entity.Schema.Namespace) &&
                         EnumsBySchemaName.TryGetValue(property.TypeName, out enumType))
                {
                    property.EnumType = enumType;
                    enumType.Properties.Add(property);
                }
                else if (
                    EnumsByName.TryGetValue(property.TypeName, out enumType))
                {
                    property.EnumType = enumType;
                    enumType.Properties.Add(property);
                }
            }

            // link navigations to association elements
            foreach (var conceptualProperty in conceptualEntityType.NavigationProperties)
            {
                var property = new NavigationProperty(entity, conceptualProperty);
                entity.NavigationProperties.Add(property);
                NavProps.Add(property);
                if (conceptualProperty.Relationship?.Length > 0)
                {
                    var relationship = conceptualProperty.Relationship;
                    relationship = relationship.Split('.').LastOrDefault(); // remove namespace
                    property.ConceptualAssociation = entity.Associations?.FirstOrDefault(o =>
                        string.Equals(o.Name, relationship, StringComparison.OrdinalIgnoreCase));
                    if (property.ConceptualAssociation == null)
                    {
                        // take a broader look. suspicious end Type in here
                        var match = edmx.ConceptualModels.Schema.Associations.FirstOrDefault(o =>
                            string.Equals(o.Name, relationship, StringComparison.OrdinalIgnoreCase));
                        if (match != null)
                        {
                            property.ConceptualAssociation = match;
                        }
                    }

                    Debug.Assert(property.ConceptualAssociation != null);
                }
            }
        }

        // with all entities loaded, perform association linking
        foreach (var entity in Entities)
        foreach (var property in entity.NavigationProperties)
        {
            if (!(entity.Associations?.Count > 0))
            {
                continue;
            }

            var ass = property.ConceptualAssociation;
            if (ass == null)
            {
                continue;
            }

            var endRoles = ass.Ends.Select(end => GetEndRole(ass, end, true)).ToArray();
            Debug.Assert(ass.Ends.Count == 0 || endRoles.Length > 0);
            var constraint = ass.ReferentialConstraint;
            if (constraint == null)
            {
                continue; // invalid association
            }

            property.Association = new Association(ass,
                GetSchema(edmx.ConceptualModels.Schema.Namespace),
                endRoles
                , new ReferentialConstraint(constraint,
                    GetProperties(GetEndRole(ass.Name, constraint.Principal.Role).Entity,
                        constraint.Principal.PropertyRefs.Select(o => o.Name).ToArray()),
                    GetProperties(GetEndRole(ass.Name, constraint.Dependent.Role).Entity,
                        constraint.Dependent.PropertyRefs.Select(o => o.Name).ToArray()))
            );
        }

        State.AssociationsByName = Entities.SelectMany(o => o.NavigationProperties)
            .Where(o => o.Association != null)
            .Select(o => o.Association)
            .GroupBy(o => o.Name)
            .ToDictionary(o => o.Key, o => o.First(), StringComparer.InvariantCultureIgnoreCase);
    }


    private Schema GetSchema(string schemaNamespace)
    {
        if (schemaNamespace.IsNullOrEmpty() && Schemas.Count == 1)
        {
            return Schemas[0];
        }

        var s = Schemas.FirstOrDefault(o => o.Namespace == schemaNamespace);
        if (s == null && schemaNamespace.HasCharacters())
        {
            // add new schema
            s = new Schema(schemaNamespace, this);
            Schemas.Add(s);
        }

        return s;
    }

    private EntityType GetEntity(string fullName)
    {
        if (fullName.IsNullOrEmpty())
        {
            throw new ArgumentNullException();
        }

        var parts = fullName.Split(new[] { '.' }).ToList();
        var entityName = string.Empty;
        if (parts.Count > 1)
        {
            entityName = parts.Last();
            parts.RemoveAt(parts.Count - 1);
        }

        var ns = parts.Count > 0 ? parts.Join(".") : null;
        return GetEntity(ns, entityName);
    }

    private EntityType GetEntity(string schemaNamespace, string entityName)
    {
        var s = schemaNamespace.HasCharacters() || Schemas.Count == 1 ? GetSchema(schemaNamespace) : null;
        if (s == null)
        {
            var entList = Entities.Where(o => o.Name == entityName).ToArray();
            if (entList.Length != 1)
            {
                throw new InvalidDataException("Ambiguous entity name: " + entityName);
            }
        }

        var e = Entities.FirstOrDefault(o => o.Name == entityName);
        return e;
    }

    private EntityPropertyBase[] GetProperties(EntityType entity, string[] propNames)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var props = entity.AllProperties.Where(o =>
                propNames.Any(n => string.Compare(o.PropertyName, n, StringComparison.InvariantCultureIgnoreCase) == 0))
            .ToArray();
        return props;
    }

    private EndRole GetEndRole(string association, string role)
    {
        var e = EndRoles.FirstOrDefault(o => o.AssociationName == association && o.Role == role);
        if (e == null)
        {
            throw new InvalidDataException("Role not found: " + role);
        }

        return e;
    }

    private EndRole GetEndRole(ConceptualAssociation association, ConceptualEnd end, bool autoAdd)
    {
        var entityType = GetEntity(end.Type);
        if (entityType == null)
        {
            return null;
        }

        var e = EndRoles.FirstOrDefault(o =>
            o.AssociationName == association.Name && o.Role == end.Role && o.Entity.Name == entityType.Name);
        if (e != null || !autoAdd)
        {
            return e;
        }

        e = new EndRole(association, end, entityType);
        EndRoles.Add(e);
        return e;
    }

    #region edmx editing

    public void SetNavPropName(string entityname, string propname, string newNavPropName)
    {
        ReplaceXmlAttributeValueByIndex(FilePath,
            $"descendant::edm:Schema/edm:EntityType[@Name='{entityname}']/edm:NavigationProperty[@Name='{propname}']",
            "Name", newNavPropName);
    }

    private static void ReplaceXmlAttributeValueByIndex(string fullFilePath, string nodeName, string attrName,
        string valueToAdd)
    {
        var fileInfo = new FileInfo(fullFilePath) { IsReadOnly = false };
        fileInfo.Refresh();

        var xmldoc = new XmlDocument();
        xmldoc.Load(fullFilePath);
        var nsm = new XmlNamespaceManager(xmldoc.NameTable);
        nsm.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2009/11/edmx");
        nsm.AddNamespace("ssdl", "http://schemas.microsoft.com/ado/2009/11/edm/ssdl");
        nsm.AddNamespace("edm", "http://schemas.microsoft.com/ado/2009/11/edm");
        try
        {
            var tnode = xmldoc.SelectSingleNode(nodeName, nsm);
            var nodes = xmldoc.SelectNodes(nodeName, nsm);
            var node = tnode as XmlElement;
            //node.Attributes[index].Value = valueToAdd;
            node.SetAttribute(attrName, valueToAdd); // Set to new value
        }
        catch (Exception ex)
        {
            //add code to see the error
        } //"descendant::edm:Schema/edm:EntityType"

        xmldoc.Save(fullFilePath);
    }

    #endregion

    #region properties

    private EdmxParsed State { get; }
    private string FilePath => State.FilePath;
    private Dictionary<string, Association> AssociationsByName => State.AssociationsByName;
    private Dictionary<string, EnumType> EnumsByName => State.EnumsByName;
    private Dictionary<string, EnumType> EnumsBySchemaName => State.EnumsBySchemaName;
    private Dictionary<string, EnumType> EnumsByExternalTypeName => State.EnumsByExternalTypeName;
    private ObservableCollection<Schema> Schemas => State.Schemas;
    private ObservableCollection<EntityType> Entities => State.Entities;
    private ObservableCollection<NavigationProperty> NavProps => State.NavProps;
    private ObservableCollection<EntityProperty> Props => State.Props;
    private ObservableCollection<EndRole> EndRoles => State.EndRoles;

    #endregion
}
