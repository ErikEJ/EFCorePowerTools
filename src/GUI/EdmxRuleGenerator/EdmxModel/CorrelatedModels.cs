using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

// ReSharper disable InvertIf
// ReSharper disable MemberCanBeInternal
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace EdmxRuleGenerator.EdmxModel;

public class Schema : NotifyPropertyChanged
{
    public Schema(string ns, EdmxParser edmxParser)
    {
        Namespace = ns;
        EdmxParser = edmxParser ?? throw new ArgumentNullException(nameof(edmxParser));
        Entities = new ObservableCollection<EntityType>();
        Associations = new ObservableCollection<Association>();
    }

    public string Namespace { get; }
    public EdmxParser EdmxParser { get; }
    public ObservableCollection<EntityType> Entities { get; }
    public ObservableCollection<Association> Associations { get; }
    public override string ToString() { return $"Schema: {Namespace}"; }
}

public class EntityType : NotifyPropertyChanged
{
    public EntityType(ConceptualEntityType conceptualEntityType, Schema schema)
    {
        ConceptualEntity = conceptualEntityType ?? throw new ArgumentNullException(nameof(conceptualEntityType));
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        schema.Entities.Add(this);
    }

    public string Namespace => Schema.Namespace;
    public string FullName => $"{Namespace}.{Name}";
    public string SelfName => $"Self.{Name}";
    public string Name => ConceptualEntity.Name;

    public ObservableCollection<NavigationProperty> NavigationProperties { get; } = new();
    public ObservableCollection<EntityProperty> Properties { get; } = new();

    public IEnumerable<EntityPropertyBase> AllProperties =>
        Properties.OfType<EntityPropertyBase>().Union(NavigationProperties);

    public ObservableCollection<EndRole> EndRoles { get; } = new();
    public Schema Schema { get; }
    public StorageEntityType StorageEntity { get; set; }
    public ConceptualEntityType ConceptualEntity { get; set; }
    public List<MappingFragment> MappingFragments { get; set; }
    public string DbSchema => StorageEntitySet?.Schema ?? StorageEntitySet?.Schema1;
    public EntitySetMapping EntitySetMapping { get; set; }
    public string EntitySetName => EntitySetMapping?.Name;
    public string[] StoreEntitySetNames { get; set; }
    public StorageEntityContainer StorageContainer { get; set; }
    public StorageEntitySet StorageEntitySet { get; set; }
    public List<ConceptualAssociation> Associations { get; set; }

    public override string ToString() { return $"Entity: {Name}"; }
}

public class EntityPropertyBase : NotifyPropertyChanged
{
    public EntityPropertyBase(EntityType e, IConceptualProperty conceptualProperty)
    {
        ConceptualProperty = conceptualProperty;
        Entity = e;
    }

    public string PropertyName => ConceptualProperty.Name;

    public EntityType Entity { get; }
    public IConceptualProperty ConceptualProperty { get; }
    public string EntityName => Entity?.Name;
}

public class EntityProperty : EntityPropertyBase
{
    public EntityProperty(EntityType conceptualEntity, ConceptualProperty conceptualProperty) : base(conceptualEntity,
        conceptualProperty)
    {
    }

    /// <summary> the database type name </summary>
    public string DbTypeName => StorageProperty?.Type;

    /// <summary> the conceptual (code) type name </summary>
    public string TypeName => ConceptualProperty?.Type;

    private EnumType enumType;
    public EnumType EnumType { get => enumType; set => SetProperty(ref enumType, value, OnEnumTypeChanged); }
    private void OnEnumTypeChanged() { }

    public bool Nullable => StorageProperty?.Nullable?.StartsWith("t", StringComparison.OrdinalIgnoreCase) == true;
    public bool IsIdentity => StorageProperty?.StoreGeneratedPattern == "Identity";

    public StorageProperty StorageProperty { get; set; }
    public ScalarPropertyMapping Mapping { get; set; }
    public string DbColumnName => Mapping?.ColumnName;
    public new ConceptualProperty ConceptualProperty => (ConceptualProperty)base.ConceptualProperty;

    public override string ToString() { return $"Prop: {PropertyName}"; }
}

public class NavigationProperty : EntityPropertyBase
{
    private Association association;

    public NavigationProperty(EntityType conceptualEntity, ConceptualNavigationProperty conceptualProperty)
        : base(conceptualEntity, conceptualProperty)
    {
    }

    public string Relationship => ConceptualNavigationProperty?.Relationship;
    public string FromRoleName => ConceptualNavigationProperty?.FromRole;
    public string ToRoleName => ConceptualNavigationProperty?.ToRole;

    public Association Association
    {
        get => association;
        set => SetProperty(ref association, value, OnAssociationChanged);
    }

    private void OnAssociationChanged(Association o)
    {
        if (o?.NavigationProperties.Contains(this) == true)
        {
            o.NavigationProperties.Remove(this);
        }

        if (association?.NavigationProperties.Contains(this) == false)
        {
            association.NavigationProperties.Add(this);
        }
    }

    public bool IsSelfReference => Association?.IsSelfReference == true;
    public Multiplicity Multiplicity => ToRole?.Multiplicity ?? Multiplicity.Unknown;

    /// <summary> this end role determines the the property value type or return type </summary>
    public EndRole ToRole => Association?.EndRoles.FirstOrDefault(o => o.Role == ToRoleName);

    /// <summary> this role is the source data type </summary>
    public EndRole FromRole => Association?.EndRoles.FirstOrDefault(o => o.Role == FromRoleName);

    public ConceptualNavigationProperty ConceptualNavigationProperty =>
        (ConceptualNavigationProperty)ConceptualProperty;

    public ConceptualAssociation ConceptualAssociation { get; set; }

    public override string ToString() { return $"NProp: {PropertyName}"; }
}

public class Association : NotifyPropertyChanged
{
    public Association(ConceptualAssociation conceptualAssociation, Schema schema1, IList<EndRole> roles,
        params ReferentialConstraint[] constraints)
    {
        ConceptualAssociation = conceptualAssociation;
        Schema = schema1 ?? throw new ArgumentNullException(nameof(schema1));
        schema1.Associations.Add(this);
        if (roles?.Count > 0)
        {
            EndRoles = roles;
            foreach (var o in roles)
            {
                o.Association = this;
            }
        }
        else
        {
            throw new InvalidDataException("new association has no roles: " + conceptualAssociation);
        }

        if (constraints?.Length > 0)
        {
            ReferentialConstraints = constraints;
            foreach (var o in constraints)
            {
                o.Association = this;
            }
        }
    }

    /// <summary> Gets this is the relationship name. </summary>
    public string Name => ConceptualAssociation.Name;

    public ConceptualAssociation ConceptualAssociation { get; }
    public IList<EndRole> EndRoles { get; }
    public IList<ReferentialConstraint> ReferentialConstraints { get; }
    public ObservableCollection<NavigationProperty> NavigationProperties { get; } = new();
    public Schema Schema { get; }

    public bool IsSelfReference => EndRoles.Count > 0 && EndRoles[0].Entity != null &&
                                   EndRoles.All(o => o.Entity == EndRoles[0].Entity);

    public override string ToString() { return $"Association: {Name}"; }
}

public class EnumTypeMember : NotifyPropertyChanged
{
    public EnumTypeMember(ConceptualEnumMember conceptualEnumMember)
    {
        ConceptualEnumMember = conceptualEnumMember;
        value = Int64.TryParse(conceptualEnumMember.Value, out var l) ? l : -1;
    }

    public ConceptualEnumMember ConceptualEnumMember { get; }

    /// <summary> this is the member name </summary>
    public string Name => ConceptualEnumMember.Name;

    private long value;
    public long Value { get => value; set => SetProperty(ref this.value, value); }

    public override string ToString() { return $"Enum Member: {Name} ({Value})"; }
}

public class EnumType : NotifyPropertyChanged
{
    public EnumType(ConceptualEnumType conceptualEnumType, Schema schema)
    {
        ConceptualEnumType = conceptualEnumType;
        Schema = schema;
        Members = conceptualEnumType.Members?.Select(o => new EnumTypeMember(o)).ToArray() ??
                  Array.Empty<EnumTypeMember>();
    }

    public ConceptualEnumType ConceptualEnumType { get; }

    /// <summary> this is the enum name, no namespace </summary>
    public string Name => ConceptualEnumType.Name;

    public string FullName => $"{Schema.Namespace}.{ConceptualEnumType.Name}";

    public EnumTypeMember[] Members { get; }

    public string UnderlyingType => ConceptualEnumType.UnderlyingType;
    public bool IsFlags => ConceptualEnumType.IsFlags?.ToLower().StartsWith("t") == true;
    public string ExternalTypeName => ConceptualEnumType.ExternalTypeName;
    public Schema Schema { get; }

    /// <summary> properties that use this enum </summary>
    public ObservableCollection<EntityProperty> Properties { get; } = new();

    public override string ToString() { return $"EnumType: {Name} (Ext: {ExternalTypeName})"; }
}

public enum Multiplicity
{
    Unknown = 0,
    ZeroOne,
    One,
    Many
}

public class EndRole : NotifyPropertyChanged
{
    private Association associations;

    public EndRole(ConceptualAssociation conceptualAssociation, ConceptualEnd conceptualEnd, EntityType entityType)
    {
        ConceptualAssociation = conceptualAssociation;
        ConceptualEnd = conceptualEnd ?? throw new ArgumentNullException(nameof(conceptualEnd));
        Entity = entityType ?? throw new ArgumentNullException(nameof(entityType));

        Multiplicity = conceptualEnd.Multiplicity switch
        {
            "0..1" => Multiplicity.ZeroOne,
            "1" => Multiplicity.One,
            "*" => Multiplicity.Many,
            _ => throw new InvalidDataException("Multiplicity unexpected: " + conceptualEnd.Multiplicity)
        };
        entityType.EndRoles.Add(this);
    }

    public string Role => ConceptualEnd.Role;
    public string AssociationName => ConceptualAssociation.Name;
    public ConceptualAssociation ConceptualAssociation { get; }
    public ConceptualEnd ConceptualEnd { get; }
    public EntityType Entity { get; }
    public Multiplicity Multiplicity { get; }

    public Association Association
    {
        get => associations;
        set => SetProperty(ref associations, value);
    }

    public override string ToString() { return $"End: {Role}"; }
}

public class ReferentialConstraint : NotifyPropertyChanged
{
    public ReferentialConstraint(
        ConceptualReferentialConstraint conceptualReferentialConstraint,
        EntityPropertyBase[] principalProps,
        EntityPropertyBase[] depProps)
    {
        ConceptualReferentialConstraint = conceptualReferentialConstraint;
        if (principalProps == null || principalProps.Length <= 0)
        {
            throw new ArgumentNullException(nameof(principalProps));
        }

        PrincipalProperties = principalProps?.Cast<EntityProperty>()?.ToArray() ?? Array.Empty<EntityProperty>();
        DependentProperties = depProps?.Cast<EntityProperty>()?.ToArray() ?? Array.Empty<EntityProperty>();
        PrincipalEntity = PrincipalProperties.FirstOrDefault()?.Entity;
        DependentEntity = DependentProperties.FirstOrDefault()?.Entity;
    }

    public ConceptualReferentialConstraint ConceptualReferentialConstraint { get; }

    public string PrincipalRole => ConceptualReferentialConstraint.Principal.Role;
    public EntityProperty[] PrincipalProperties { get; }

    public string DependentRole => ConceptualReferentialConstraint.Dependent.Role;
    public EntityProperty[] DependentProperties { get; }


    private Association association;

    public Association Association
    {
        get => association;
        set => SetProperty(ref association, value, OnAssociationChanged);
    }

    private void OnAssociationChanged(Association o)
    {
        if (o?.ReferentialConstraints.Contains(this) == true)
        {
            o.ReferentialConstraints.Remove(this);
        }

        if (association?.ReferentialConstraints.Contains(this) == false)
        {
            association.ReferentialConstraints.Add(this);
        }
    }

    public EntityType PrincipalEntity { get; }
    public EntityType DependentEntity { get; }

    public override string ToString() { return $"Ref: {PrincipalRole}"; }
}
