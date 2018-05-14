//------------------------------------------------------------------------------
//<copyright company="Microsoft">
//
//    The MIT License (MIT)
//    
//    Copyright (c) 2015 Microsoft
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//</copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.Dac.Extensions.Prototype
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.SqlServer.Server;
	using Microsoft.SqlServer.Dac.Model;
    public partial class TSqlModelElement
    {
        ///<summary>
	    ///  Returns a strongly-typed wrapper for the TSqlObject instance.
	    ///</summary>
	    public static ISqlModelElementReference AdaptInstance(ModelRelationshipInstance obj, Func<ModelRelationshipInstance,ISqlModelElementReference> createUnresolvedElement)
        {
            ModelTypeClass objectType = null;
		
            if(obj == null)
		    {
			    throw new ArgumentNullException("obj");
		    }	

            if (obj.Object != null)
            {
                objectType = obj.Object.ObjectType;
            }
            else
            {
                return createUnresolvedElement(obj);
            }
		
		    switch(objectType.Name)
		    {
		    case "Column":     
                return new TSqlColumnReference(obj, objectType);
		    case "TableValuedFunction":     
                return new TSqlTableValuedFunctionReference(obj, objectType);
		    case "ScalarFunction":     
                return new TSqlScalarFunctionReference(obj, objectType);
		    case "ClrTableOption":     
                return new TSqlClrTableOptionReference(obj, objectType);
		    case "Aggregate":     
                return new TSqlAggregateReference(obj, objectType);
		    case "ApplicationRole":     
                return new TSqlApplicationRoleReference(obj, objectType);
		    case "Index":     
                return new TSqlIndexReference(obj, objectType);
		    case "Assembly":     
                return new TSqlAssemblyReference(obj, objectType);
		    case "AssemblySource":     
                return new TSqlAssemblySourceReference(obj, objectType);
		    case "AsymmetricKey":     
                return new TSqlAsymmetricKeyReference(obj, objectType);
		    case "AuditAction":     
                return new TSqlAuditActionReference(obj, objectType);
		    case "AuditActionGroup":     
                return new TSqlAuditActionGroupReference(obj, objectType);
		    case "AuditActionSpecification":     
                return new TSqlAuditActionSpecificationReference(obj, objectType);
		    case "BrokerPriority":     
                return new TSqlBrokerPriorityReference(obj, objectType);
		    case "BuiltInServerRole":     
                return new TSqlBuiltInServerRoleReference(obj, objectType);
		    case "DataType":     
                return new TSqlDataTypeReference(obj, objectType);
		    case "Certificate":     
                return new TSqlCertificateReference(obj, objectType);
		    case "CheckConstraint":     
                return new TSqlCheckConstraintReference(obj, objectType);
		    case "ClrTypeMethod":     
                return new TSqlClrTypeMethodReference(obj, objectType);
		    case "ClrTypeMethodParameter":     
                return new TSqlClrTypeMethodParameterReference(obj, objectType);
		    case "ClrTypeProperty":     
                return new TSqlClrTypePropertyReference(obj, objectType);
		    case "ColumnStoreIndex":     
                return new TSqlColumnStoreIndexReference(obj, objectType);
		    case "Contract":     
                return new TSqlContractReference(obj, objectType);
		    case "Credential":     
                return new TSqlCredentialReference(obj, objectType);
		    case "CryptographicProvider":     
                return new TSqlCryptographicProviderReference(obj, objectType);
		    case "DatabaseAuditSpecification":     
                return new TSqlDatabaseAuditSpecificationReference(obj, objectType);
		    case "DatabaseDdlTrigger":     
                return new TSqlDatabaseDdlTriggerReference(obj, objectType);
		    case "DatabaseEncryptionKey":     
                return new TSqlDatabaseEncryptionKeyReference(obj, objectType);
		    case "DatabaseEventNotification":     
                return new TSqlDatabaseEventNotificationReference(obj, objectType);
		    case "DatabaseMirroringLanguageSpecifier":     
                return new TSqlDatabaseMirroringLanguageSpecifierReference(obj, objectType);
		    case "DatabaseOptions":     
                return new TSqlDatabaseOptionsReference(obj, objectType);
		    case "DataCompressionOption":     
                return new TSqlDataCompressionOptionReference(obj, objectType);
		    case "Default":     
                return new TSqlDefaultReference(obj, objectType);
		    case "DefaultConstraint":     
                return new TSqlDefaultConstraintReference(obj, objectType);
		    case "DmlTrigger":     
                return new TSqlDmlTriggerReference(obj, objectType);
		    case "Endpoint":     
                return new TSqlEndpointReference(obj, objectType);
		    case "ErrorMessage":     
                return new TSqlErrorMessageReference(obj, objectType);
		    case "EventGroup":     
                return new TSqlEventGroupReference(obj, objectType);
		    case "EventSession":     
                return new TSqlEventSessionReference(obj, objectType);
		    case "EventSessionAction":     
                return new TSqlEventSessionActionReference(obj, objectType);
		    case "EventSessionDefinitions":     
                return new TSqlEventSessionDefinitionsReference(obj, objectType);
		    case "EventSessionSetting":     
                return new TSqlEventSessionSettingReference(obj, objectType);
		    case "EventSessionTarget":     
                return new TSqlEventSessionTargetReference(obj, objectType);
		    case "EventTypeSpecifier":     
                return new TSqlEventTypeSpecifierReference(obj, objectType);
		    case "ExtendedProcedure":     
                return new TSqlExtendedProcedureReference(obj, objectType);
		    case "ExtendedProperty":     
                return new TSqlExtendedPropertyReference(obj, objectType);
		    case "SqlFile":     
                return new TSqlSqlFileReference(obj, objectType);
		    case "Filegroup":     
                return new TSqlFilegroupReference(obj, objectType);
		    case "ForeignKeyConstraint":     
                return new TSqlForeignKeyConstraintReference(obj, objectType);
		    case "FullTextCatalog":     
                return new TSqlFullTextCatalogReference(obj, objectType);
		    case "FullTextIndex":     
                return new TSqlFullTextIndexReference(obj, objectType);
		    case "FullTextIndexColumnSpecifier":     
                return new TSqlFullTextIndexColumnSpecifierReference(obj, objectType);
		    case "FullTextStopList":     
                return new TSqlFullTextStopListReference(obj, objectType);
		    case "HttpProtocolSpecifier":     
                return new TSqlHttpProtocolSpecifierReference(obj, objectType);
		    case "LinkedServer":     
                return new TSqlLinkedServerReference(obj, objectType);
		    case "LinkedServerLogin":     
                return new TSqlLinkedServerLoginReference(obj, objectType);
		    case "Login":     
                return new TSqlLoginReference(obj, objectType);
		    case "MasterKey":     
                return new TSqlMasterKeyReference(obj, objectType);
		    case "MessageType":     
                return new TSqlMessageTypeReference(obj, objectType);
		    case "PartitionFunction":     
                return new TSqlPartitionFunctionReference(obj, objectType);
		    case "PartitionScheme":     
                return new TSqlPartitionSchemeReference(obj, objectType);
		    case "PartitionValue":     
                return new TSqlPartitionValueReference(obj, objectType);
		    case "Permission":     
                return new TSqlPermissionReference(obj, objectType);
		    case "PrimaryKeyConstraint":     
                return new TSqlPrimaryKeyConstraintReference(obj, objectType);
		    case "Procedure":     
                return new TSqlProcedureReference(obj, objectType);
		    case "Queue":     
                return new TSqlQueueReference(obj, objectType);
		    case "QueueEventNotification":     
                return new TSqlQueueEventNotificationReference(obj, objectType);
		    case "RemoteServiceBinding":     
                return new TSqlRemoteServiceBindingReference(obj, objectType);
		    case "ResourceGovernor":     
                return new TSqlResourceGovernorReference(obj, objectType);
		    case "ResourcePool":     
                return new TSqlResourcePoolReference(obj, objectType);
		    case "Role":     
                return new TSqlRoleReference(obj, objectType);
		    case "RoleMembership":     
                return new TSqlRoleMembershipReference(obj, objectType);
		    case "Route":     
                return new TSqlRouteReference(obj, objectType);
		    case "Rule":     
                return new TSqlRuleReference(obj, objectType);
		    case "Schema":     
                return new TSqlSchemaReference(obj, objectType);
		    case "SearchProperty":     
                return new TSqlSearchPropertyReference(obj, objectType);
		    case "SearchPropertyList":     
                return new TSqlSearchPropertyListReference(obj, objectType);
		    case "Sequence":     
                return new TSqlSequenceReference(obj, objectType);
		    case "ServerAudit":     
                return new TSqlServerAuditReference(obj, objectType);
		    case "ServerAuditSpecification":     
                return new TSqlServerAuditSpecificationReference(obj, objectType);
		    case "ServerDdlTrigger":     
                return new TSqlServerDdlTriggerReference(obj, objectType);
		    case "ServerEventNotification":     
                return new TSqlServerEventNotificationReference(obj, objectType);
		    case "ServerOptions":     
                return new TSqlServerOptionsReference(obj, objectType);
		    case "ServerRoleMembership":     
                return new TSqlServerRoleMembershipReference(obj, objectType);
		    case "Service":     
                return new TSqlServiceReference(obj, objectType);
		    case "ServiceBrokerLanguageSpecifier":     
                return new TSqlServiceBrokerLanguageSpecifierReference(obj, objectType);
		    case "Signature":     
                return new TSqlSignatureReference(obj, objectType);
		    case "SignatureEncryptionMechanism":     
                return new TSqlSignatureEncryptionMechanismReference(obj, objectType);
		    case "SoapLanguageSpecifier":     
                return new TSqlSoapLanguageSpecifierReference(obj, objectType);
		    case "SoapMethodSpecification":     
                return new TSqlSoapMethodSpecificationReference(obj, objectType);
		    case "SpatialIndex":     
                return new TSqlSpatialIndexReference(obj, objectType);
		    case "Statistics":     
                return new TSqlStatisticsReference(obj, objectType);
		    case "Parameter":     
                return new TSqlParameterReference(obj, objectType);
		    case "SymmetricKey":     
                return new TSqlSymmetricKeyReference(obj, objectType);
		    case "SymmetricKeyPassword":     
                return new TSqlSymmetricKeyPasswordReference(obj, objectType);
		    case "Synonym":     
                return new TSqlSynonymReference(obj, objectType);
		    case "Table":     
                return new TSqlTableReference(obj, objectType);
		    case "FileTable":     
                return new TSqlFileTableReference(obj, objectType);
		    case "TableType":     
                return new TSqlTableTypeReference(obj, objectType);
		    case "TableTypeCheckConstraint":     
                return new TSqlTableTypeCheckConstraintReference(obj, objectType);
		    case "TableTypeColumn":     
                return new TSqlTableTypeColumnReference(obj, objectType);
		    case "TableTypeDefaultConstraint":     
                return new TSqlTableTypeDefaultConstraintReference(obj, objectType);
		    case "TableTypeIndex":     
                return new TSqlTableTypeIndexReference(obj, objectType);
		    case "TableTypePrimaryKeyConstraint":     
                return new TSqlTableTypePrimaryKeyConstraintReference(obj, objectType);
		    case "TableTypeUniqueConstraint":     
                return new TSqlTableTypeUniqueConstraintReference(obj, objectType);
		    case "TcpProtocolSpecifier":     
                return new TSqlTcpProtocolSpecifierReference(obj, objectType);
		    case "UniqueConstraint":     
                return new TSqlUniqueConstraintReference(obj, objectType);
		    case "User":     
                return new TSqlUserReference(obj, objectType);
		    case "UserDefinedServerRole":     
                return new TSqlUserDefinedServerRoleReference(obj, objectType);
		    case "UserDefinedType":     
                return new TSqlUserDefinedTypeReference(obj, objectType);
		    case "View":     
                return new TSqlViewReference(obj, objectType);
		    case "WorkloadGroup":     
                return new TSqlWorkloadGroupReference(obj, objectType);
		    case "XmlIndex":     
                return new TSqlXmlIndexReference(obj, objectType);
		    case "SelectiveXmlIndex":     
                return new TSqlSelectiveXmlIndexReference(obj, objectType);
		    case "XmlNamespace":     
                return new TSqlXmlNamespaceReference(obj, objectType);
		    case "PromotedNodePathForXQueryType":     
                return new TSqlPromotedNodePathForXQueryTypeReference(obj, objectType);
		    case "PromotedNodePathForSqlType":     
                return new TSqlPromotedNodePathForSqlTypeReference(obj, objectType);
		    case "XmlSchemaCollection":     
                return new TSqlXmlSchemaCollectionReference(obj, objectType);
		    default:
			    throw new ArgumentException("No type mapping exists for " + objectType.Name);
		    }
	    }


	    ///<summary>
	    ///  Returns a strongly-typed wrapper for the TSqlObject instance.
	    ///</summary>
	    public static ISqlModelElement AdaptInstance(TSqlObject obj)
        {
		    if(obj == null)
		    {
			    throw new ArgumentNullException("obj");
		    }	
		
		    switch(obj.ObjectType.Name)
		    {
		    case "Column":     
                return new TSqlColumn(obj);
		    case "TableValuedFunction":     
                return new TSqlTableValuedFunction(obj);
		    case "ScalarFunction":     
                return new TSqlScalarFunction(obj);
		    case "ClrTableOption":     
                return new TSqlClrTableOption(obj);
		    case "Aggregate":     
                return new TSqlAggregate(obj);
		    case "ApplicationRole":     
                return new TSqlApplicationRole(obj);
		    case "Index":     
                return new TSqlIndex(obj);
		    case "Assembly":     
                return new TSqlAssembly(obj);
		    case "AssemblySource":     
                return new TSqlAssemblySource(obj);
		    case "AsymmetricKey":     
                return new TSqlAsymmetricKey(obj);
		    case "AuditAction":     
                return new TSqlAuditAction(obj);
		    case "AuditActionGroup":     
                return new TSqlAuditActionGroup(obj);
		    case "AuditActionSpecification":     
                return new TSqlAuditActionSpecification(obj);
		    case "BrokerPriority":     
                return new TSqlBrokerPriority(obj);
		    case "BuiltInServerRole":     
                return new TSqlBuiltInServerRole(obj);
		    case "DataType":     
                return new TSqlDataType(obj);
		    case "Certificate":     
                return new TSqlCertificate(obj);
		    case "CheckConstraint":     
                return new TSqlCheckConstraint(obj);
		    case "ClrTypeMethod":     
                return new TSqlClrTypeMethod(obj);
		    case "ClrTypeMethodParameter":     
                return new TSqlClrTypeMethodParameter(obj);
		    case "ClrTypeProperty":     
                return new TSqlClrTypeProperty(obj);
		    case "ColumnStoreIndex":     
                return new TSqlColumnStoreIndex(obj);
		    case "Contract":     
                return new TSqlContract(obj);
		    case "Credential":     
                return new TSqlCredential(obj);
		    case "CryptographicProvider":     
                return new TSqlCryptographicProvider(obj);
		    case "DatabaseAuditSpecification":     
                return new TSqlDatabaseAuditSpecification(obj);
		    case "DatabaseDdlTrigger":     
                return new TSqlDatabaseDdlTrigger(obj);
		    case "DatabaseEncryptionKey":     
                return new TSqlDatabaseEncryptionKey(obj);
		    case "DatabaseEventNotification":     
                return new TSqlDatabaseEventNotification(obj);
		    case "DatabaseMirroringLanguageSpecifier":     
                return new TSqlDatabaseMirroringLanguageSpecifier(obj);
		    case "DatabaseOptions":     
                return new TSqlDatabaseOptions(obj);
		    case "DataCompressionOption":     
                return new TSqlDataCompressionOption(obj);
		    case "Default":     
                return new TSqlDefault(obj);
		    case "DefaultConstraint":     
                return new TSqlDefaultConstraint(obj);
		    case "DmlTrigger":     
                return new TSqlDmlTrigger(obj);
		    case "Endpoint":     
                return new TSqlEndpoint(obj);
		    case "ErrorMessage":     
                return new TSqlErrorMessage(obj);
		    case "EventGroup":     
                return new TSqlEventGroup(obj);
		    case "EventSession":     
                return new TSqlEventSession(obj);
		    case "EventSessionAction":     
                return new TSqlEventSessionAction(obj);
		    case "EventSessionDefinitions":     
                return new TSqlEventSessionDefinitions(obj);
		    case "EventSessionSetting":     
                return new TSqlEventSessionSetting(obj);
		    case "EventSessionTarget":     
                return new TSqlEventSessionTarget(obj);
		    case "EventTypeSpecifier":     
                return new TSqlEventTypeSpecifier(obj);
		    case "ExtendedProcedure":     
                return new TSqlExtendedProcedure(obj);
		    case "ExtendedProperty":     
                return new TSqlExtendedProperty(obj);
		    case "SqlFile":     
                return new TSqlSqlFile(obj);
		    case "Filegroup":     
                return new TSqlFilegroup(obj);
		    case "ForeignKeyConstraint":     
                return new TSqlForeignKeyConstraint(obj);
		    case "FullTextCatalog":     
                return new TSqlFullTextCatalog(obj);
		    case "FullTextIndex":     
                return new TSqlFullTextIndex(obj);
		    case "FullTextIndexColumnSpecifier":     
                return new TSqlFullTextIndexColumnSpecifier(obj);
		    case "FullTextStopList":     
                return new TSqlFullTextStopList(obj);
		    case "HttpProtocolSpecifier":     
                return new TSqlHttpProtocolSpecifier(obj);
		    case "LinkedServer":     
                return new TSqlLinkedServer(obj);
		    case "LinkedServerLogin":     
                return new TSqlLinkedServerLogin(obj);
		    case "Login":     
                return new TSqlLogin(obj);
		    case "MasterKey":     
                return new TSqlMasterKey(obj);
		    case "MessageType":     
                return new TSqlMessageType(obj);
		    case "PartitionFunction":     
                return new TSqlPartitionFunction(obj);
		    case "PartitionScheme":     
                return new TSqlPartitionScheme(obj);
		    case "PartitionValue":     
                return new TSqlPartitionValue(obj);
		    case "Permission":     
                return new TSqlPermission(obj);
		    case "PrimaryKeyConstraint":     
                return new TSqlPrimaryKeyConstraint(obj);
		    case "Procedure":     
                return new TSqlProcedure(obj);
		    case "Queue":     
                return new TSqlQueue(obj);
		    case "QueueEventNotification":     
                return new TSqlQueueEventNotification(obj);
		    case "RemoteServiceBinding":     
                return new TSqlRemoteServiceBinding(obj);
		    case "ResourceGovernor":     
                return new TSqlResourceGovernor(obj);
		    case "ResourcePool":     
                return new TSqlResourcePool(obj);
		    case "Role":     
                return new TSqlRole(obj);
		    case "RoleMembership":     
                return new TSqlRoleMembership(obj);
		    case "Route":     
                return new TSqlRoute(obj);
		    case "Rule":     
                return new TSqlRule(obj);
		    case "Schema":     
                return new TSqlSchema(obj);
		    case "SearchProperty":     
                return new TSqlSearchProperty(obj);
		    case "SearchPropertyList":     
                return new TSqlSearchPropertyList(obj);
		    case "Sequence":     
                return new TSqlSequence(obj);
		    case "ServerAudit":     
                return new TSqlServerAudit(obj);
		    case "ServerAuditSpecification":     
                return new TSqlServerAuditSpecification(obj);
		    case "ServerDdlTrigger":     
                return new TSqlServerDdlTrigger(obj);
		    case "ServerEventNotification":     
                return new TSqlServerEventNotification(obj);
		    case "ServerOptions":     
                return new TSqlServerOptions(obj);
		    case "ServerRoleMembership":     
                return new TSqlServerRoleMembership(obj);
		    case "Service":     
                return new TSqlService(obj);
		    case "ServiceBrokerLanguageSpecifier":     
                return new TSqlServiceBrokerLanguageSpecifier(obj);
		    case "Signature":     
                return new TSqlSignature(obj);
		    case "SignatureEncryptionMechanism":     
                return new TSqlSignatureEncryptionMechanism(obj);
		    case "SoapLanguageSpecifier":     
                return new TSqlSoapLanguageSpecifier(obj);
		    case "SoapMethodSpecification":     
                return new TSqlSoapMethodSpecification(obj);
		    case "SpatialIndex":     
                return new TSqlSpatialIndex(obj);
		    case "Statistics":     
                return new TSqlStatistics(obj);
		    case "Parameter":     
                return new TSqlParameter(obj);
		    case "SymmetricKey":     
                return new TSqlSymmetricKey(obj);
		    case "SymmetricKeyPassword":     
                return new TSqlSymmetricKeyPassword(obj);
		    case "Synonym":     
                return new TSqlSynonym(obj);
		    case "Table":     
                return new TSqlTable(obj);
		    case "FileTable":     
                return new TSqlFileTable(obj);
		    case "TableType":     
                return new TSqlTableType(obj);
		    case "TableTypeCheckConstraint":     
                return new TSqlTableTypeCheckConstraint(obj);
		    case "TableTypeColumn":     
                return new TSqlTableTypeColumn(obj);
		    case "TableTypeDefaultConstraint":     
                return new TSqlTableTypeDefaultConstraint(obj);
		    case "TableTypeIndex":     
                return new TSqlTableTypeIndex(obj);
		    case "TableTypePrimaryKeyConstraint":     
                return new TSqlTableTypePrimaryKeyConstraint(obj);
		    case "TableTypeUniqueConstraint":     
                return new TSqlTableTypeUniqueConstraint(obj);
		    case "TcpProtocolSpecifier":     
                return new TSqlTcpProtocolSpecifier(obj);
		    case "UniqueConstraint":     
                return new TSqlUniqueConstraint(obj);
		    case "User":     
                return new TSqlUser(obj);
		    case "UserDefinedServerRole":     
                return new TSqlUserDefinedServerRole(obj);
		    case "UserDefinedType":     
                return new TSqlUserDefinedType(obj);
		    case "View":     
                return new TSqlView(obj);
		    case "WorkloadGroup":     
                return new TSqlWorkloadGroup(obj);
		    case "XmlIndex":     
                return new TSqlXmlIndex(obj);
		    case "SelectiveXmlIndex":     
                return new TSqlSelectiveXmlIndex(obj);
		    case "XmlNamespace":     
                return new TSqlXmlNamespace(obj);
		    case "PromotedNodePathForXQueryType":     
                return new TSqlPromotedNodePathForXQueryType(obj);
		    case "PromotedNodePathForSqlType":     
                return new TSqlPromotedNodePathForSqlType(obj);
		    case "XmlSchemaCollection":     
                return new TSqlXmlSchemaCollection(obj);
		    default:
			    throw new ArgumentException("No type mapping exists for " + obj.ObjectType.Name);
		    }
	    }
    }

    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Column"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.aspx">Column</see>
	///
	public partial class TSqlColumnReference : TSqlColumn, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlColumnReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Column"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.aspx">Column</see>
	///
	public partial class TSqlColumn : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Column.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlColumn to adapt instances of <see cref="T:Column"/>
		/// </summary>
		public TSqlColumn(TSqlObject obj)  : base(obj, Column.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlColumn to adapt instances of <see cref="T:Column"/>
		/// </summary>
		protected TSqlColumn()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Column.Collation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.collation.aspx">Column.Collation</see>
		///
		public String Collation 
		{
			get { return Element.GetProperty<String>(Column.Collation);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.expression.aspx">Column.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(Column.Expression);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IdentityIncrement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.identityincrement.aspx">Column.IdentityIncrement</see>
		///
		public String IdentityIncrement 
		{
			get { return Element.GetProperty<String>(Column.IdentityIncrement);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IdentitySeed"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.identityseed.aspx">Column.IdentitySeed</see>
		///
		public String IdentitySeed 
		{
			get { return Element.GetProperty<String>(Column.IdentitySeed);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IsFileStream"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.isfilestream.aspx">Column.IsFileStream</see>
		///
		public Boolean IsFileStream 
		{
			get { return Element.GetProperty<Boolean>(Column.IsFileStream);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IsIdentity"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.isidentity.aspx">Column.IsIdentity</see>
		///
		public Boolean IsIdentity 
		{
			get { return Element.GetProperty<Boolean>(Column.IsIdentity);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IsIdentityNotForReplication"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.isidentitynotforreplication.aspx">Column.IsIdentityNotForReplication</see>
		///
		public Boolean IsIdentityNotForReplication 
		{
			get { return Element.GetProperty<Boolean>(Column.IsIdentityNotForReplication);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IsMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.ismax.aspx">Column.IsMax</see>
		///
		public Boolean IsMax 
		{
			get { return Element.GetProperty<Boolean>(Column.IsMax);}
		}


		///
		/// Property wrapper for <see cref="M:Column.IsRowGuidCol"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.isrowguidcol.aspx">Column.IsRowGuidCol</see>
		///
		public Boolean IsRowGuidCol 
		{
			get { return Element.GetProperty<Boolean>(Column.IsRowGuidCol);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Length"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.length.aspx">Column.Length</see>
		///
		public Int32 Length 
		{
			get { return Element.GetProperty<Int32>(Column.Length);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Nullable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.nullable.aspx">Column.Nullable</see>
		///
		public Boolean Nullable 
		{
			get { return Element.GetProperty<Boolean>(Column.Nullable);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Persisted"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.persisted.aspx">Column.Persisted</see>
		///
		public Boolean Persisted 
		{
			get { return Element.GetProperty<Boolean>(Column.Persisted);}
		}


		///
		/// Property wrapper for <see cref="M:Column.PersistedNullable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.persistednullable.aspx">Column.PersistedNullable</see>
		///
		public Boolean? PersistedNullable 
		{
			get { return Element.GetProperty<Boolean?>(Column.PersistedNullable);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Precision"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.precision.aspx">Column.Precision</see>
		///
		public Int32 Precision 
		{
			get { return Element.GetProperty<Int32>(Column.Precision);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Scale"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.scale.aspx">Column.Scale</see>
		///
		public Int32 Scale 
		{
			get { return Element.GetProperty<Int32>(Column.Scale);}
		}


		///
		/// Property wrapper for <see cref="M:Column.Sparse"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.sparse.aspx">Column.Sparse</see>
		///
		public Boolean Sparse 
		{
			get { return Element.GetProperty<Boolean>(Column.Sparse);}
		}


		///
		/// Property wrapper for <see cref="M:Column.XmlStyle"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.xmlstyle.aspx">Column.XmlStyle</see>
		///
		public XmlStyle XmlStyle 
		{
			get { return Element.GetProperty<XmlStyle>(Column.XmlStyle);}
		}


		///
		/// Metadata property wrapper for <see cref="M:Column.ColumnType"/>
		///
		public ColumnType ColumnType 
		{
			get { return Element.GetMetadata<ColumnType>(Column.ColumnType);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.datatype.aspx">Column.DataType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> DataType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Column.DataType).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlDataTypeElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.expressiondependencies.aspx">Column.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Column.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.column.xmlschemacollection.aspx">Column.XmlSchemaCollection</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference> XmlSchemaCollection 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Column.XmlSchemaCollection).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference(o, Microsoft.SqlServer.Dac.Model.XmlSchemaCollection.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableValuedFunction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.aspx">TableValuedFunction</see>
	///
	public partial class TSqlTableValuedFunctionReference : TSqlTableValuedFunction, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableValuedFunctionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableValuedFunction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.aspx">TableValuedFunction</see>
	///
	public partial class TSqlTableValuedFunction : TSqlModelElement,ISqlColumnSource,ISpecifiesIndex	{
		private static ModelTypeClass typeClass = TableValuedFunction.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableValuedFunction to adapt instances of <see cref="T:TableValuedFunction"/>
		/// </summary>
		public TSqlTableValuedFunction(TSqlObject obj)  : base(obj, TableValuedFunction.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableValuedFunction to adapt instances of <see cref="T:TableValuedFunction"/>
		/// </summary>
		protected TSqlTableValuedFunction()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableValuedFunction.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.ansinullson.aspx">TableValuedFunction.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(TableValuedFunction.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.CalledOnNullInput"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.calledonnullinput.aspx">TableValuedFunction.CalledOnNullInput</see>
		///
		public Boolean CalledOnNullInput 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.CalledOnNullInput);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.classname.aspx">TableValuedFunction.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(TableValuedFunction.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.DataAccess"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.dataaccess.aspx">TableValuedFunction.DataAccess</see>
		///
		public DataAccessKind? DataAccess 
		{
			get { return Element.GetProperty<DataAccessKind?>(TableValuedFunction.DataAccess);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.Deterministic"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.deterministic.aspx">TableValuedFunction.Deterministic</see>
		///
		public Boolean? Deterministic 
		{
			get { return Element.GetProperty<Boolean?>(TableValuedFunction.Deterministic);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.ExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.executeascaller.aspx">TableValuedFunction.ExecuteAsCaller</see>
		///
		public Boolean ExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.ExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.executeasowner.aspx">TableValuedFunction.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.executeasself.aspx">TableValuedFunction.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.FillRowMethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.fillrowmethodname.aspx">TableValuedFunction.FillRowMethodName</see>
		///
		public String FillRowMethodName 
		{
			get { return Element.GetProperty<String>(TableValuedFunction.FillRowMethodName);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.IsReplicated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.isreplicated.aspx">TableValuedFunction.IsReplicated</see>
		///
		public Boolean IsReplicated 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.IsReplicated);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.MethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.methodname.aspx">TableValuedFunction.MethodName</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(TableValuedFunction.MethodName);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.Precise"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.precise.aspx">TableValuedFunction.Precise</see>
		///
		public Boolean? Precise 
		{
			get { return Element.GetProperty<Boolean?>(TableValuedFunction.Precise);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.quotedidentifieron.aspx">TableValuedFunction.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(TableValuedFunction.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.ReturnsNullOnNullInput"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.returnsnullonnullinput.aspx">TableValuedFunction.ReturnsNullOnNullInput</see>
		///
		public Boolean ReturnsNullOnNullInput 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.ReturnsNullOnNullInput);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.ReturnTableVariableName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.returntablevariablename.aspx">TableValuedFunction.ReturnTableVariableName</see>
		///
		public String ReturnTableVariableName 
		{
			get { return Element.GetProperty<String>(TableValuedFunction.ReturnTableVariableName);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.SystemDataAccess"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.systemdataaccess.aspx">TableValuedFunction.SystemDataAccess</see>
		///
		public SystemDataAccessKind? SystemDataAccess 
		{
			get { return Element.GetProperty<SystemDataAccessKind?>(TableValuedFunction.SystemDataAccess);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.withencryption.aspx">TableValuedFunction.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.WithEncryption);}
		}


		///
		/// Property wrapper for <see cref="M:TableValuedFunction.WithSchemaBinding"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.withschemabinding.aspx">TableValuedFunction.WithSchemaBinding</see>
		///
		public Boolean WithSchemaBinding 
		{
			get { return Element.GetProperty<Boolean>(TableValuedFunction.WithSchemaBinding);}
		}


		///
		/// Metadata property wrapper for <see cref="M:TableValuedFunction.FunctionType"/>
		///
		public FunctionType FunctionType 
		{
			get { return Element.GetMetadata<FunctionType>(TableValuedFunction.FunctionType);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssembly"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.assembly.aspx">TableValuedFunction.Assembly</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssembly> Assembly 
		{
			get 
			{
				return Element.GetReferenced(TableValuedFunction.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssembly(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.bodydependencies.aspx">TableValuedFunction.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableValuedFunction.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.columns.aspx">TableValuedFunction.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn> Columns 
		{
			get 
			{
				return Element.GetReferenced(TableValuedFunction.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.login.aspx">TableValuedFunction.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableValuedFunction.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.parameters.aspx">TableValuedFunction.Parameters</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter> Parameters 
		{
			get 
			{
				return Element.GetReferenced(TableValuedFunction.Parameters).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.returntype.aspx">TableValuedFunction.ReturnType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> ReturnType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableValuedFunction.ReturnType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.schema.aspx">TableValuedFunction.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableValuedFunction.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlClrTableOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.tableoption.aspx">TableValuedFunction.TableOption</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlClrTableOption> TableOption 
		{
			get 
			{
				return Element.GetReferenced(TableValuedFunction.TableOption).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlClrTableOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tablevaluedfunction.user.aspx">TableValuedFunction.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableValuedFunction.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ScalarFunction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.aspx">ScalarFunction</see>
	///
	public partial class TSqlScalarFunctionReference : TSqlScalarFunction, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlScalarFunctionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ScalarFunction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.aspx">ScalarFunction</see>
	///
	public partial class TSqlScalarFunction : TSqlModelElement	{
		private static ModelTypeClass typeClass = ScalarFunction.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlScalarFunction to adapt instances of <see cref="T:ScalarFunction"/>
		/// </summary>
		public TSqlScalarFunction(TSqlObject obj)  : base(obj, ScalarFunction.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlScalarFunction to adapt instances of <see cref="T:ScalarFunction"/>
		/// </summary>
		protected TSqlScalarFunction()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ScalarFunction.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.ansinullson.aspx">ScalarFunction.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(ScalarFunction.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.CalledOnNullInput"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.calledonnullinput.aspx">ScalarFunction.CalledOnNullInput</see>
		///
		public Boolean CalledOnNullInput 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.CalledOnNullInput);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.classname.aspx">ScalarFunction.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(ScalarFunction.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.DataAccess"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.dataaccess.aspx">ScalarFunction.DataAccess</see>
		///
		public DataAccessKind? DataAccess 
		{
			get { return Element.GetProperty<DataAccessKind?>(ScalarFunction.DataAccess);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.Deterministic"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.deterministic.aspx">ScalarFunction.Deterministic</see>
		///
		public Boolean? Deterministic 
		{
			get { return Element.GetProperty<Boolean?>(ScalarFunction.Deterministic);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.ExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.executeascaller.aspx">ScalarFunction.ExecuteAsCaller</see>
		///
		public Boolean ExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.ExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.executeasowner.aspx">ScalarFunction.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.executeasself.aspx">ScalarFunction.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.FillRowMethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.fillrowmethodname.aspx">ScalarFunction.FillRowMethodName</see>
		///
		public String FillRowMethodName 
		{
			get { return Element.GetProperty<String>(ScalarFunction.FillRowMethodName);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.IsReplicated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.isreplicated.aspx">ScalarFunction.IsReplicated</see>
		///
		public Boolean IsReplicated 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.IsReplicated);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.MethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.methodname.aspx">ScalarFunction.MethodName</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(ScalarFunction.MethodName);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.Precise"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.precise.aspx">ScalarFunction.Precise</see>
		///
		public Boolean? Precise 
		{
			get { return Element.GetProperty<Boolean?>(ScalarFunction.Precise);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.quotedidentifieron.aspx">ScalarFunction.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(ScalarFunction.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.ReturnsNullOnNullInput"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.returnsnullonnullinput.aspx">ScalarFunction.ReturnsNullOnNullInput</see>
		///
		public Boolean ReturnsNullOnNullInput 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.ReturnsNullOnNullInput);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.SystemDataAccess"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.systemdataaccess.aspx">ScalarFunction.SystemDataAccess</see>
		///
		public SystemDataAccessKind? SystemDataAccess 
		{
			get { return Element.GetProperty<SystemDataAccessKind?>(ScalarFunction.SystemDataAccess);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.withencryption.aspx">ScalarFunction.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.WithEncryption);}
		}


		///
		/// Property wrapper for <see cref="M:ScalarFunction.WithSchemaBinding"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.withschemabinding.aspx">ScalarFunction.WithSchemaBinding</see>
		///
		public Boolean WithSchemaBinding 
		{
			get { return Element.GetProperty<Boolean>(ScalarFunction.WithSchemaBinding);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssembly"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.assembly.aspx">ScalarFunction.Assembly</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssembly> Assembly 
		{
			get 
			{
				return Element.GetReferenced(ScalarFunction.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssembly(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.bodydependencies.aspx">ScalarFunction.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ScalarFunction.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.login.aspx">ScalarFunction.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ScalarFunction.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.parameters.aspx">ScalarFunction.Parameters</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter> Parameters 
		{
			get 
			{
				return Element.GetReferenced(ScalarFunction.Parameters).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.returntype.aspx">ScalarFunction.ReturnType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> ReturnType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ScalarFunction.ReturnType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.schema.aspx">ScalarFunction.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ScalarFunction.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.scalarfunction.user.aspx">ScalarFunction.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ScalarFunction.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTableOption"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtableoption.aspx">ClrTableOption</see>
	///
	public partial class TSqlClrTableOptionReference : TSqlClrTableOption, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlClrTableOptionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTableOption"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtableoption.aspx">ClrTableOption</see>
	///
	public partial class TSqlClrTableOption : TSqlModelElement	{
		private static ModelTypeClass typeClass = ClrTableOption.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlClrTableOption to adapt instances of <see cref="T:ClrTableOption"/>
		/// </summary>
		public TSqlClrTableOption(TSqlObject obj)  : base(obj, ClrTableOption.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlClrTableOption to adapt instances of <see cref="T:ClrTableOption"/>
		/// </summary>
		protected TSqlClrTableOption()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ClrTableOption.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtableoption.classname.aspx">ClrTableOption.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(ClrTableOption.ClassName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ClrTableOptionOrderColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtableoption.ordercolumns.aspx">ClrTableOption.OrderColumns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ClrTableOptionOrderColumnsReference> OrderColumns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ClrTableOption.OrderColumns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.ClrTableOptionOrderColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Aggregate"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.aspx">Aggregate</see>
	///
	public partial class TSqlAggregateReference : TSqlAggregate, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAggregateReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Aggregate"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.aspx">Aggregate</see>
	///
	public partial class TSqlAggregate : TSqlModelElement	{
		private static ModelTypeClass typeClass = Aggregate.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAggregate to adapt instances of <see cref="T:Aggregate"/>
		/// </summary>
		public TSqlAggregate(TSqlObject obj)  : base(obj, Aggregate.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAggregate to adapt instances of <see cref="T:Aggregate"/>
		/// </summary>
		protected TSqlAggregate()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Aggregate.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.classname.aspx">Aggregate.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(Aggregate.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:Aggregate.Format"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.format.aspx">Aggregate.Format</see>
		///
		public Format Format 
		{
			get { return Element.GetProperty<Format>(Aggregate.Format);}
		}


		///
		/// Property wrapper for <see cref="M:Aggregate.InvariantToDuplicates"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.invarianttoduplicates.aspx">Aggregate.InvariantToDuplicates</see>
		///
		public Boolean? InvariantToDuplicates 
		{
			get { return Element.GetProperty<Boolean?>(Aggregate.InvariantToDuplicates);}
		}


		///
		/// Property wrapper for <see cref="M:Aggregate.InvariantToNulls"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.invarianttonulls.aspx">Aggregate.InvariantToNulls</see>
		///
		public Boolean? InvariantToNulls 
		{
			get { return Element.GetProperty<Boolean?>(Aggregate.InvariantToNulls);}
		}


		///
		/// Property wrapper for <see cref="M:Aggregate.MaxByteSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.maxbytesize.aspx">Aggregate.MaxByteSize</see>
		///
		public Int32? MaxByteSize 
		{
			get { return Element.GetProperty<Int32?>(Aggregate.MaxByteSize);}
		}


		///
		/// Property wrapper for <see cref="M:Aggregate.NullIfEmpty"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.nullifempty.aspx">Aggregate.NullIfEmpty</see>
		///
		public Boolean? NullIfEmpty 
		{
			get { return Element.GetProperty<Boolean?>(Aggregate.NullIfEmpty);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.assembly.aspx">Aggregate.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Aggregate.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.parameters.aspx">Aggregate.Parameters</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter> Parameters 
		{
			get 
			{
				return Element.GetReferenced(Aggregate.Parameters).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.returntype.aspx">Aggregate.ReturnType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> ReturnType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Aggregate.ReturnType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.aggregate.schema.aspx">Aggregate.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Aggregate.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ApplicationRole"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.applicationrole.aspx">ApplicationRole</see>
	///
	public partial class TSqlApplicationRoleReference : TSqlApplicationRole, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlApplicationRoleReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ApplicationRole"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.applicationrole.aspx">ApplicationRole</see>
	///
	public partial class TSqlApplicationRole : TSqlModelElement,ISqlDatabaseSecurityPrincipal	{
		private static ModelTypeClass typeClass = ApplicationRole.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlApplicationRole to adapt instances of <see cref="T:ApplicationRole"/>
		/// </summary>
		public TSqlApplicationRole(TSqlObject obj)  : base(obj, ApplicationRole.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlApplicationRole to adapt instances of <see cref="T:ApplicationRole"/>
		/// </summary>
		protected TSqlApplicationRole()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ApplicationRole.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.applicationrole.password.aspx">ApplicationRole.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(ApplicationRole.Password);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.applicationrole.defaultschema.aspx">ApplicationRole.DefaultSchema</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> DefaultSchema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ApplicationRole.DefaultSchema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Index"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.aspx">Index</see>
	///
	public partial class TSqlIndexReference : TSqlIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Index"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.aspx">Index</see>
	///
	public partial class TSqlIndex : TSqlModelElement,ISqlIndex,ISpecifiesStorage	{
		private static ModelTypeClass typeClass = Index.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlIndex to adapt instances of <see cref="T:Index"/>
		/// </summary>
		public TSqlIndex(TSqlObject obj)  : base(obj, Index.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlIndex to adapt instances of <see cref="T:Index"/>
		/// </summary>
		protected TSqlIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Index.AllowPageLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.allowpagelocks.aspx">Index.AllowPageLocks</see>
		///
		public Boolean AllowPageLocks 
		{
			get { return Element.GetProperty<Boolean>(Index.AllowPageLocks);}
		}


		///
		/// Property wrapper for <see cref="M:Index.AllowRowLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.allowrowlocks.aspx">Index.AllowRowLocks</see>
		///
		public Boolean AllowRowLocks 
		{
			get { return Element.GetProperty<Boolean>(Index.AllowRowLocks);}
		}


		///
		/// Property wrapper for <see cref="M:Index.BucketCount"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.bucketcount.aspx">Index.BucketCount</see>
		///
		public Int32? BucketCount 
		{
			get { return Element.GetProperty<Int32?>(Index.BucketCount);}
		}


		///
		/// Property wrapper for <see cref="M:Index.Clustered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.clustered.aspx">Index.Clustered</see>
		///
		public Boolean Clustered 
		{
			get { return Element.GetProperty<Boolean>(Index.Clustered);}
		}


		///
		/// Property wrapper for <see cref="M:Index.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.disabled.aspx">Index.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(Index.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:Index.FileStreamNull"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.filestreamnull.aspx">Index.FileStreamNull</see>
		///
		public Boolean? FileStreamNull 
		{
			get { return Element.GetProperty<Boolean?>(Index.FileStreamNull);}
		}


		///
		/// Property wrapper for <see cref="M:Index.FillFactor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.fillfactor.aspx">Index.FillFactor</see>
		///
		public Int32? FillFactor 
		{
			get { return Element.GetProperty<Int32?>(Index.FillFactor);}
		}


		///
		/// Property wrapper for <see cref="M:Index.FilterPredicate"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.filterpredicate.aspx">Index.FilterPredicate</see>
		///
		public String FilterPredicate 
		{
			get { return (String)Element.GetProperty(Index.FilterPredicate);}
		}


		///
		/// Property wrapper for <see cref="M:Index.Hash"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.hash.aspx">Index.Hash</see>
		///
		public Boolean Hash 
		{
			get { return Element.GetProperty<Boolean>(Index.Hash);}
		}


		///
		/// Property wrapper for <see cref="M:Index.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.ignoreduplicatekey.aspx">Index.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(Index.IgnoreDuplicateKey);}
		}


		///
		/// Property wrapper for <see cref="M:Index.IncrementalStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.incrementalstatistics.aspx">Index.IncrementalStatistics</see>
		///
		public Boolean IncrementalStatistics 
		{
			get { return Element.GetProperty<Boolean>(Index.IncrementalStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:Index.RecomputeStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.recomputestatistics.aspx">Index.RecomputeStatistics</see>
		///
		public Boolean RecomputeStatistics 
		{
			get { return Element.GetProperty<Boolean>(Index.RecomputeStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:Index.Unique"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.unique.aspx">Index.Unique</see>
		///
		public Boolean Unique 
		{
			get { return Element.GetProperty<Boolean>(Index.Unique);}
		}


		///
		/// Property wrapper for <see cref="M:Index.WithPadIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.withpadindex.aspx">Index.WithPadIndex</see>
		///
		public Boolean WithPadIndex 
		{
			get { return Element.GetProperty<Boolean>(Index.WithPadIndex);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.bodydependencies.aspx">Index.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.IndexColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.columns.aspx">Index.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IndexColumnsReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.IndexColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.datacompressionoptions.aspx">Index.DataCompressionOptions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption> DataCompressionOptions 
		{
			get 
			{
				return Element.GetReferenced(Index.DataCompressionOptions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.filegroup.aspx">Index.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.filestreamfilegroup.aspx">Index.FileStreamFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> FileStreamFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.FileStreamFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.filestreampartitionscheme.aspx">Index.FileStreamPartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> FileStreamPartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.FileStreamPartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.includedcolumns.aspx">Index.IncludedColumns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> IncludedColumns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.IncludedColumns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.indexedobject.aspx">Index.IndexedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> IndexedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.IndexedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISpecifiesIndexElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.partitioncolumn.aspx">Index.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.index.partitionscheme.aspx">Index.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Index.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Assembly"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.aspx">Assembly</see>
	///
	public partial class TSqlAssemblyReference : TSqlAssembly, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAssemblyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Assembly"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.aspx">Assembly</see>
	///
	public partial class TSqlAssembly : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Assembly.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAssembly to adapt instances of <see cref="T:Assembly"/>
		/// </summary>
		public TSqlAssembly(TSqlObject obj)  : base(obj, Assembly.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAssembly to adapt instances of <see cref="T:Assembly"/>
		/// </summary>
		protected TSqlAssembly()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Assembly.PermissionSet"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.permissionset.aspx">Assembly.PermissionSet</see>
		///
		public AssemblyPermissionSet PermissionSet 
		{
			get { return Element.GetProperty<AssemblyPermissionSet>(Assembly.PermissionSet);}
		}


		///
		/// Property wrapper for <see cref="M:Assembly.Visible"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.visible.aspx">Assembly.Visible</see>
		///
		public Boolean Visible 
		{
			get { return Element.GetProperty<Boolean>(Assembly.Visible);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblySource"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.assemblysources.aspx">Assembly.AssemblySources</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblySource> AssemblySources 
		{
			get 
			{
				return Element.GetReferenced(Assembly.AssemblySources).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblySource(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.authorizer.aspx">Assembly.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Assembly.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assembly.referencedassemblies.aspx">Assembly.ReferencedAssemblies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> ReferencedAssemblies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Assembly.ReferencedAssemblies).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AssemblySource"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assemblysource.aspx">AssemblySource</see>
	///
	public partial class TSqlAssemblySourceReference : TSqlAssemblySource, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAssemblySourceReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AssemblySource"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assemblysource.aspx">AssemblySource</see>
	///
	public partial class TSqlAssemblySource : TSqlModelElement	{
		private static ModelTypeClass typeClass = AssemblySource.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAssemblySource to adapt instances of <see cref="T:AssemblySource"/>
		/// </summary>
		public TSqlAssemblySource(TSqlObject obj)  : base(obj, AssemblySource.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAssemblySource to adapt instances of <see cref="T:AssemblySource"/>
		/// </summary>
		protected TSqlAssemblySource()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:AssemblySource.Source"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.assemblysource.source.aspx">AssemblySource.Source</see>
		///
		public String Source 
		{
			get { return (String)Element.GetProperty(AssemblySource.Source);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AsymmetricKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.aspx">AsymmetricKey</see>
	///
	public partial class TSqlAsymmetricKeyReference : TSqlAsymmetricKey, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAsymmetricKeyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AsymmetricKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.aspx">AsymmetricKey</see>
	///
	public partial class TSqlAsymmetricKey : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = AsymmetricKey.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAsymmetricKey to adapt instances of <see cref="T:AsymmetricKey"/>
		/// </summary>
		public TSqlAsymmetricKey(TSqlObject obj)  : base(obj, AsymmetricKey.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAsymmetricKey to adapt instances of <see cref="T:AsymmetricKey"/>
		/// </summary>
		protected TSqlAsymmetricKey()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:AsymmetricKey.Algorithm"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.algorithm.aspx">AsymmetricKey.Algorithm</see>
		///
		public AsymmetricKeyAlgorithm Algorithm 
		{
			get { return Element.GetProperty<AsymmetricKeyAlgorithm>(AsymmetricKey.Algorithm);}
		}


		///
		/// Property wrapper for <see cref="M:AsymmetricKey.CreationDisposition"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.creationdisposition.aspx">AsymmetricKey.CreationDisposition</see>
		///
		public SymmetricKeyCreationDisposition CreationDisposition 
		{
			get { return Element.GetProperty<SymmetricKeyCreationDisposition>(AsymmetricKey.CreationDisposition);}
		}


		///
		/// Property wrapper for <see cref="M:AsymmetricKey.EncryptedWithPassword"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.encryptedwithpassword.aspx">AsymmetricKey.EncryptedWithPassword</see>
		///
		public Boolean EncryptedWithPassword 
		{
			get { return Element.GetProperty<Boolean>(AsymmetricKey.EncryptedWithPassword);}
		}


		///
		/// Property wrapper for <see cref="M:AsymmetricKey.ExecutableFile"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.executablefile.aspx">AsymmetricKey.ExecutableFile</see>
		///
		public String ExecutableFile 
		{
			get { return Element.GetProperty<String>(AsymmetricKey.ExecutableFile);}
		}


		///
		/// Property wrapper for <see cref="M:AsymmetricKey.File"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.file.aspx">AsymmetricKey.File</see>
		///
		public String File 
		{
			get { return Element.GetProperty<String>(AsymmetricKey.File);}
		}


		///
		/// Property wrapper for <see cref="M:AsymmetricKey.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.password.aspx">AsymmetricKey.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(AsymmetricKey.Password);}
		}


		///
		/// Property wrapper for <see cref="M:AsymmetricKey.ProviderKeyName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.providerkeyname.aspx">AsymmetricKey.ProviderKeyName</see>
		///
		public String ProviderKeyName 
		{
			get { return Element.GetProperty<String>(AsymmetricKey.ProviderKeyName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.assembly.aspx">AsymmetricKey.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(AsymmetricKey.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.authorizer.aspx">AsymmetricKey.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(AsymmetricKey.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.asymmetrickey.provider.aspx">AsymmetricKey.Provider</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> Provider 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(AsymmetricKey.Provider).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AuditAction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditaction.aspx">AuditAction</see>
	///
	public partial class TSqlAuditActionReference : TSqlAuditAction, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAuditActionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AuditAction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditaction.aspx">AuditAction</see>
	///
	public partial class TSqlAuditAction : TSqlModelElement	{
		private static ModelTypeClass typeClass = AuditAction.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAuditAction to adapt instances of <see cref="T:AuditAction"/>
		/// </summary>
		public TSqlAuditAction(TSqlObject obj)  : base(obj, AuditAction.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAuditAction to adapt instances of <see cref="T:AuditAction"/>
		/// </summary>
		protected TSqlAuditAction()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:AuditAction.Action"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditaction.action.aspx">AuditAction.Action</see>
		///
		public DatabaseAuditAction Action 
		{
			get { return Element.GetProperty<DatabaseAuditAction>(AuditAction.Action);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AuditActionGroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactiongroup.aspx">AuditActionGroup</see>
	///
	public partial class TSqlAuditActionGroupReference : TSqlAuditActionGroup, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAuditActionGroupReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AuditActionGroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactiongroup.aspx">AuditActionGroup</see>
	///
	public partial class TSqlAuditActionGroup : TSqlModelElement	{
		private static ModelTypeClass typeClass = AuditActionGroup.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAuditActionGroup to adapt instances of <see cref="T:AuditActionGroup"/>
		/// </summary>
		public TSqlAuditActionGroup(TSqlObject obj)  : base(obj, AuditActionGroup.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAuditActionGroup to adapt instances of <see cref="T:AuditActionGroup"/>
		/// </summary>
		protected TSqlAuditActionGroup()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:AuditActionGroup.ActionGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactiongroup.actiongroup.aspx">AuditActionGroup.ActionGroup</see>
		///
		public AuditActionGroupType ActionGroup 
		{
			get { return Element.GetProperty<AuditActionGroupType>(AuditActionGroup.ActionGroup);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AuditActionSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactionspecification.aspx">AuditActionSpecification</see>
	///
	public partial class TSqlAuditActionSpecificationReference : TSqlAuditActionSpecification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlAuditActionSpecificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:AuditActionSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactionspecification.aspx">AuditActionSpecification</see>
	///
	public partial class TSqlAuditActionSpecification : TSqlModelElement	{
		private static ModelTypeClass typeClass = AuditActionSpecification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlAuditActionSpecification to adapt instances of <see cref="T:AuditActionSpecification"/>
		/// </summary>
		public TSqlAuditActionSpecification(TSqlObject obj)  : base(obj, AuditActionSpecification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlAuditActionSpecification to adapt instances of <see cref="T:AuditActionSpecification"/>
		/// </summary>
		protected TSqlAuditActionSpecification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditAction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactionspecification.auditactions.aspx">AuditActionSpecification.AuditActions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditAction> AuditActions 
		{
			get 
			{
				return Element.GetReferenced(AuditActionSpecification.AuditActions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditAction(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactionspecification.principals.aspx">AuditActionSpecification.Principals</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> Principals 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(AuditActionSpecification.Principals).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlDatabaseSecurityPrincipalElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.auditactionspecification.securedobject.aspx">AuditActionSpecification.SecuredObject</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> SecuredObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(AuditActionSpecification.SecuredObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlSecurableElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:BrokerPriority"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.brokerpriority.aspx">BrokerPriority</see>
	///
	public partial class TSqlBrokerPriorityReference : TSqlBrokerPriority, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlBrokerPriorityReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:BrokerPriority"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.brokerpriority.aspx">BrokerPriority</see>
	///
	public partial class TSqlBrokerPriority : TSqlModelElement	{
		private static ModelTypeClass typeClass = BrokerPriority.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlBrokerPriority to adapt instances of <see cref="T:BrokerPriority"/>
		/// </summary>
		public TSqlBrokerPriority(TSqlObject obj)  : base(obj, BrokerPriority.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlBrokerPriority to adapt instances of <see cref="T:BrokerPriority"/>
		/// </summary>
		protected TSqlBrokerPriority()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:BrokerPriority.PriorityLevel"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.brokerpriority.prioritylevel.aspx">BrokerPriority.PriorityLevel</see>
		///
		public Int32 PriorityLevel 
		{
			get { return Element.GetProperty<Int32>(BrokerPriority.PriorityLevel);}
		}


		///
		/// Property wrapper for <see cref="M:BrokerPriority.RemoteServiceName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.brokerpriority.remoteservicename.aspx">BrokerPriority.RemoteServiceName</see>
		///
		public String RemoteServiceName 
		{
			get { return Element.GetProperty<String>(BrokerPriority.RemoteServiceName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.brokerpriority.contractname.aspx">BrokerPriority.ContractName</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ContractName 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(BrokerPriority.ContractName).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.brokerpriority.localservicename.aspx">BrokerPriority.LocalServiceName</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> LocalServiceName 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(BrokerPriority.LocalServiceName).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:BuiltInServerRole"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.builtinserverrole.aspx">BuiltInServerRole</see>
	///
	public partial class TSqlBuiltInServerRoleReference : TSqlBuiltInServerRole, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlBuiltInServerRoleReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:BuiltInServerRole"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.builtinserverrole.aspx">BuiltInServerRole</see>
	///
	public partial class TSqlBuiltInServerRole : TSqlModelElement	{
		private static ModelTypeClass typeClass = BuiltInServerRole.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlBuiltInServerRole to adapt instances of <see cref="T:BuiltInServerRole"/>
		/// </summary>
		public TSqlBuiltInServerRole(TSqlObject obj)  : base(obj, BuiltInServerRole.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlBuiltInServerRole to adapt instances of <see cref="T:BuiltInServerRole"/>
		/// </summary>
		protected TSqlBuiltInServerRole()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DataType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.aspx">DataType</see>
	///
	public partial class TSqlDataTypeReference : TSqlDataType, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDataTypeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DataType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.aspx">DataType</see>
	///
	public partial class TSqlDataType : TSqlModelElement,ISqlDataType	{
		private static ModelTypeClass typeClass = DataType.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDataType to adapt instances of <see cref="T:DataType"/>
		/// </summary>
		public TSqlDataType(TSqlObject obj)  : base(obj, DataType.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDataType to adapt instances of <see cref="T:DataType"/>
		/// </summary>
		protected TSqlDataType()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DataType.SqlDataType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.sqldatatype.aspx">DataType.SqlDataType</see>
		///
		public SqlDataType SqlDataType 
		{
			get { return Element.GetProperty<SqlDataType>(DataType.SqlDataType);}
		}


		///
		/// Property wrapper for <see cref="M:DataType.UddtIsMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.uddtismax.aspx">DataType.UddtIsMax</see>
		///
		public Boolean UddtIsMax 
		{
			get { return Element.GetProperty<Boolean>(DataType.UddtIsMax);}
		}


		///
		/// Property wrapper for <see cref="M:DataType.UddtLength"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.uddtlength.aspx">DataType.UddtLength</see>
		///
		public Int32 UddtLength 
		{
			get { return Element.GetProperty<Int32>(DataType.UddtLength);}
		}


		///
		/// Property wrapper for <see cref="M:DataType.UddtNullable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.uddtnullable.aspx">DataType.UddtNullable</see>
		///
		public Boolean UddtNullable 
		{
			get { return Element.GetProperty<Boolean>(DataType.UddtNullable);}
		}


		///
		/// Property wrapper for <see cref="M:DataType.UddtPrecision"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.uddtprecision.aspx">DataType.UddtPrecision</see>
		///
		public Int32 UddtPrecision 
		{
			get { return Element.GetProperty<Int32>(DataType.UddtPrecision);}
		}


		///
		/// Property wrapper for <see cref="M:DataType.UddtScale"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.uddtscale.aspx">DataType.UddtScale</see>
		///
		public Int32 UddtScale 
		{
			get { return Element.GetProperty<Int32>(DataType.UddtScale);}
		}


		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.schema.aspx">DataType.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DataType.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datatype.type.aspx">DataType.Type</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> Type 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DataType.Type).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Certificate"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.aspx">Certificate</see>
	///
	public partial class TSqlCertificateReference : TSqlCertificate, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlCertificateReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Certificate"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.aspx">Certificate</see>
	///
	public partial class TSqlCertificate : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Certificate.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlCertificate to adapt instances of <see cref="T:Certificate"/>
		/// </summary>
		public TSqlCertificate(TSqlObject obj)  : base(obj, Certificate.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlCertificate to adapt instances of <see cref="T:Certificate"/>
		/// </summary>
		protected TSqlCertificate()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Certificate.ActiveForBeginDialog"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.activeforbegindialog.aspx">Certificate.ActiveForBeginDialog</see>
		///
		public Boolean ActiveForBeginDialog 
		{
			get { return Element.GetProperty<Boolean>(Certificate.ActiveForBeginDialog);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.EncryptedWithPassword"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.encryptedwithpassword.aspx">Certificate.EncryptedWithPassword</see>
		///
		public Boolean EncryptedWithPassword 
		{
			get { return Element.GetProperty<Boolean>(Certificate.EncryptedWithPassword);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.EncryptionPassword"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.encryptionpassword.aspx">Certificate.EncryptionPassword</see>
		///
		public String EncryptionPassword 
		{
			get { return Element.GetProperty<String>(Certificate.EncryptionPassword);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.ExistingKeysFilePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.existingkeysfilepath.aspx">Certificate.ExistingKeysFilePath</see>
		///
		public String ExistingKeysFilePath 
		{
			get { return Element.GetProperty<String>(Certificate.ExistingKeysFilePath);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.ExpiryDate"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.expirydate.aspx">Certificate.ExpiryDate</see>
		///
		public String ExpiryDate 
		{
			get { return Element.GetProperty<String>(Certificate.ExpiryDate);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.IsExistingKeyFileExecutable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.isexistingkeyfileexecutable.aspx">Certificate.IsExistingKeyFileExecutable</see>
		///
		public Boolean IsExistingKeyFileExecutable 
		{
			get { return Element.GetProperty<Boolean>(Certificate.IsExistingKeyFileExecutable);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.PrivateKeyDecryptionPassword"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.privatekeydecryptionpassword.aspx">Certificate.PrivateKeyDecryptionPassword</see>
		///
		public String PrivateKeyDecryptionPassword 
		{
			get { return Element.GetProperty<String>(Certificate.PrivateKeyDecryptionPassword);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.PrivateKeyEncryptionPassword"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.privatekeyencryptionpassword.aspx">Certificate.PrivateKeyEncryptionPassword</see>
		///
		public String PrivateKeyEncryptionPassword 
		{
			get { return Element.GetProperty<String>(Certificate.PrivateKeyEncryptionPassword);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.PrivateKeyFilePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.privatekeyfilepath.aspx">Certificate.PrivateKeyFilePath</see>
		///
		public String PrivateKeyFilePath 
		{
			get { return Element.GetProperty<String>(Certificate.PrivateKeyFilePath);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.StartDate"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.startdate.aspx">Certificate.StartDate</see>
		///
		public String StartDate 
		{
			get { return Element.GetProperty<String>(Certificate.StartDate);}
		}


		///
		/// Property wrapper for <see cref="M:Certificate.Subject"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.subject.aspx">Certificate.Subject</see>
		///
		public String Subject 
		{
			get { return Element.GetProperty<String>(Certificate.Subject);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.authorizer.aspx">Certificate.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Certificate.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.certificate.existingkeysassembly.aspx">Certificate.ExistingKeysAssembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> ExistingKeysAssembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Certificate.ExistingKeysAssembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:CheckConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.aspx">CheckConstraint</see>
	///
	public partial class TSqlCheckConstraintReference : TSqlCheckConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlCheckConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:CheckConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.aspx">CheckConstraint</see>
	///
	public partial class TSqlCheckConstraint : TSqlModelElement,IExtendedPropertyHost	{
		private static ModelTypeClass typeClass = CheckConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlCheckConstraint to adapt instances of <see cref="T:CheckConstraint"/>
		/// </summary>
		public TSqlCheckConstraint(TSqlObject obj)  : base(obj, CheckConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlCheckConstraint to adapt instances of <see cref="T:CheckConstraint"/>
		/// </summary>
		protected TSqlCheckConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:CheckConstraint.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.disabled.aspx">CheckConstraint.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(CheckConstraint.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:CheckConstraint.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.expression.aspx">CheckConstraint.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(CheckConstraint.Expression);}
		}


		///
		/// Property wrapper for <see cref="M:CheckConstraint.NotForReplication"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.notforreplication.aspx">CheckConstraint.NotForReplication</see>
		///
		public Boolean NotForReplication 
		{
			get { return Element.GetProperty<Boolean>(CheckConstraint.NotForReplication);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.expressiondependencies.aspx">CheckConstraint.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(CheckConstraint.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.checkconstraint.host.aspx">CheckConstraint.Host</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference> Host 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(CheckConstraint.Host).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference(o, Microsoft.SqlServer.Dac.Model.Table.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTypeMethod"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethod.aspx">ClrTypeMethod</see>
	///
	public partial class TSqlClrTypeMethodReference : TSqlClrTypeMethod, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlClrTypeMethodReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTypeMethod"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethod.aspx">ClrTypeMethod</see>
	///
	public partial class TSqlClrTypeMethod : TSqlModelElement	{
		private static ModelTypeClass typeClass = ClrTypeMethod.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlClrTypeMethod to adapt instances of <see cref="T:ClrTypeMethod"/>
		/// </summary>
		public TSqlClrTypeMethod(TSqlObject obj)  : base(obj, ClrTypeMethod.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlClrTypeMethod to adapt instances of <see cref="T:ClrTypeMethod"/>
		/// </summary>
		protected TSqlClrTypeMethod()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ClrTypeMethod.Name"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethod.name.aspx">ClrTypeMethod.Name</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(ClrTypeMethod.Name);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethod.parameters.aspx">ClrTypeMethod.Parameters</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter> Parameters 
		{
			get 
			{
				return Element.GetReferenced(ClrTypeMethod.Parameters).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethod.returntype.aspx">ClrTypeMethod.ReturnType</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataType> ReturnType 
		{
			get 
			{
				return Element.GetReferenced(ClrTypeMethod.ReturnType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataType(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTypeMethodParameter"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethodparameter.aspx">ClrTypeMethodParameter</see>
	///
	public partial class TSqlClrTypeMethodParameterReference : TSqlClrTypeMethodParameter, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlClrTypeMethodParameterReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTypeMethodParameter"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethodparameter.aspx">ClrTypeMethodParameter</see>
	///
	public partial class TSqlClrTypeMethodParameter : TSqlModelElement	{
		private static ModelTypeClass typeClass = ClrTypeMethodParameter.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlClrTypeMethodParameter to adapt instances of <see cref="T:ClrTypeMethodParameter"/>
		/// </summary>
		public TSqlClrTypeMethodParameter(TSqlObject obj)  : base(obj, ClrTypeMethodParameter.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlClrTypeMethodParameter to adapt instances of <see cref="T:ClrTypeMethodParameter"/>
		/// </summary>
		protected TSqlClrTypeMethodParameter()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ClrTypeMethodParameter.IsOutput"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethodparameter.isoutput.aspx">ClrTypeMethodParameter.IsOutput</see>
		///
		public Boolean IsOutput 
		{
			get { return Element.GetProperty<Boolean>(ClrTypeMethodParameter.IsOutput);}
		}


		///
		/// Property wrapper for <see cref="M:ClrTypeMethodParameter.Name"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethodparameter.name.aspx">ClrTypeMethodParameter.Name</see>
		///
		public String ParameterName 
		{
			get { return Element.GetProperty<String>(ClrTypeMethodParameter.Name);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypemethodparameter.datatype.aspx">ClrTypeMethodParameter.DataType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> DataType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ClrTypeMethodParameter.DataType).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlDataTypeElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTypeProperty"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypeproperty.aspx">ClrTypeProperty</see>
	///
	public partial class TSqlClrTypePropertyReference : TSqlClrTypeProperty, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlClrTypePropertyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ClrTypeProperty"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypeproperty.aspx">ClrTypeProperty</see>
	///
	public partial class TSqlClrTypeProperty : TSqlModelElement	{
		private static ModelTypeClass typeClass = ClrTypeProperty.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlClrTypeProperty to adapt instances of <see cref="T:ClrTypeProperty"/>
		/// </summary>
		public TSqlClrTypeProperty(TSqlObject obj)  : base(obj, ClrTypeProperty.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlClrTypeProperty to adapt instances of <see cref="T:ClrTypeProperty"/>
		/// </summary>
		protected TSqlClrTypeProperty()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ClrTypeProperty.Name"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypeproperty.name.aspx">ClrTypeProperty.Name</see>
		///
		public String PropertyName 
		{
			get { return Element.GetProperty<String>(ClrTypeProperty.Name);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.clrtypeproperty.clrtype.aspx">ClrTypeProperty.ClrType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> ClrType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ClrTypeProperty.ClrType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ColumnStoreIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.aspx">ColumnStoreIndex</see>
	///
	public partial class TSqlColumnStoreIndexReference : TSqlColumnStoreIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlColumnStoreIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ColumnStoreIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.aspx">ColumnStoreIndex</see>
	///
	public partial class TSqlColumnStoreIndex : TSqlModelElement,ISqlIndex	{
		private static ModelTypeClass typeClass = ColumnStoreIndex.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlColumnStoreIndex to adapt instances of <see cref="T:ColumnStoreIndex"/>
		/// </summary>
		public TSqlColumnStoreIndex(TSqlObject obj)  : base(obj, ColumnStoreIndex.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlColumnStoreIndex to adapt instances of <see cref="T:ColumnStoreIndex"/>
		/// </summary>
		protected TSqlColumnStoreIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ColumnStoreIndex.Clustered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.clustered.aspx">ColumnStoreIndex.Clustered</see>
		///
		public Boolean Clustered 
		{
			get { return Element.GetProperty<Boolean>(ColumnStoreIndex.Clustered);}
		}


		///
		/// Property wrapper for <see cref="M:ColumnStoreIndex.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.disabled.aspx">ColumnStoreIndex.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(ColumnStoreIndex.Disabled);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ColumnStoreIndexColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.columns.aspx">ColumnStoreIndex.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ColumnStoreIndexColumnsReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ColumnStoreIndex.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.ColumnStoreIndexColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.datacompressionoptions.aspx">ColumnStoreIndex.DataCompressionOptions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption> DataCompressionOptions 
		{
			get 
			{
				return Element.GetReferenced(ColumnStoreIndex.DataCompressionOptions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.filegroup.aspx">ColumnStoreIndex.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ColumnStoreIndex.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.indexedobject.aspx">ColumnStoreIndex.IndexedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> IndexedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ColumnStoreIndex.IndexedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISpecifiesIndexElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.partitioncolumn.aspx">ColumnStoreIndex.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ColumnStoreIndex.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.columnstoreindex.partitionscheme.aspx">ColumnStoreIndex.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ColumnStoreIndex.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Contract"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.contract.aspx">Contract</see>
	///
	public partial class TSqlContractReference : TSqlContract, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlContractReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Contract"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.contract.aspx">Contract</see>
	///
	public partial class TSqlContract : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Contract.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlContract to adapt instances of <see cref="T:Contract"/>
		/// </summary>
		public TSqlContract(TSqlObject obj)  : base(obj, Contract.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlContract to adapt instances of <see cref="T:Contract"/>
		/// </summary>
		protected TSqlContract()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.contract.authorizer.aspx">Contract.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Contract.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ContractMessagesReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.contract.messages.aspx">Contract.Messages</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ContractMessagesReference> Messages 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Contract.Messages).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.ContractMessagesReference(o, Microsoft.SqlServer.Dac.Model.MessageType.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Credential"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.credential.aspx">Credential</see>
	///
	public partial class TSqlCredentialReference : TSqlCredential, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlCredentialReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Credential"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.credential.aspx">Credential</see>
	///
	public partial class TSqlCredential : TSqlModelElement	{
		private static ModelTypeClass typeClass = Credential.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlCredential to adapt instances of <see cref="T:Credential"/>
		/// </summary>
		public TSqlCredential(TSqlObject obj)  : base(obj, Credential.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlCredential to adapt instances of <see cref="T:Credential"/>
		/// </summary>
		protected TSqlCredential()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Credential.Identity"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.credential.identity.aspx">Credential.Identity</see>
		///
		public String Identity 
		{
			get { return Element.GetProperty<String>(Credential.Identity);}
		}


		///
		/// Property wrapper for <see cref="M:Credential.Secret"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.credential.secret.aspx">Credential.Secret</see>
		///
		public String Secret 
		{
			get { return Element.GetProperty<String>(Credential.Secret);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCryptographicProviderReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.credential.cryptographicprovider.aspx">Credential.CryptographicProvider</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCryptographicProviderReference> CryptographicProvider 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Credential.CryptographicProvider).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCryptographicProviderReference(o, Microsoft.SqlServer.Dac.Model.CryptographicProvider.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:CryptographicProvider"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.cryptographicprovider.aspx">CryptographicProvider</see>
	///
	public partial class TSqlCryptographicProviderReference : TSqlCryptographicProvider, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlCryptographicProviderReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:CryptographicProvider"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.cryptographicprovider.aspx">CryptographicProvider</see>
	///
	public partial class TSqlCryptographicProvider : TSqlModelElement	{
		private static ModelTypeClass typeClass = CryptographicProvider.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlCryptographicProvider to adapt instances of <see cref="T:CryptographicProvider"/>
		/// </summary>
		public TSqlCryptographicProvider(TSqlObject obj)  : base(obj, CryptographicProvider.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlCryptographicProvider to adapt instances of <see cref="T:CryptographicProvider"/>
		/// </summary>
		protected TSqlCryptographicProvider()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:CryptographicProvider.DllPath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.cryptographicprovider.dllpath.aspx">CryptographicProvider.DllPath</see>
		///
		public String DllPath 
		{
			get { return Element.GetProperty<String>(CryptographicProvider.DllPath);}
		}


		///
		/// Property wrapper for <see cref="M:CryptographicProvider.Enabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.cryptographicprovider.enabled.aspx">CryptographicProvider.Enabled</see>
		///
		public Boolean Enabled 
		{
			get { return Element.GetProperty<Boolean>(CryptographicProvider.Enabled);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseAuditSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseauditspecification.aspx">DatabaseAuditSpecification</see>
	///
	public partial class TSqlDatabaseAuditSpecificationReference : TSqlDatabaseAuditSpecification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDatabaseAuditSpecificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseAuditSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseauditspecification.aspx">DatabaseAuditSpecification</see>
	///
	public partial class TSqlDatabaseAuditSpecification : TSqlModelElement	{
		private static ModelTypeClass typeClass = DatabaseAuditSpecification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseAuditSpecification to adapt instances of <see cref="T:DatabaseAuditSpecification"/>
		/// </summary>
		public TSqlDatabaseAuditSpecification(TSqlObject obj)  : base(obj, DatabaseAuditSpecification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseAuditSpecification to adapt instances of <see cref="T:DatabaseAuditSpecification"/>
		/// </summary>
		protected TSqlDatabaseAuditSpecification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DatabaseAuditSpecification.WithState"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseauditspecification.withstate.aspx">DatabaseAuditSpecification.WithState</see>
		///
		public Boolean WithState 
		{
			get { return Element.GetProperty<Boolean>(DatabaseAuditSpecification.WithState);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditActionGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseauditspecification.auditactiongroups.aspx">DatabaseAuditSpecification.AuditActionGroups</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditActionGroup> AuditActionGroups 
		{
			get 
			{
				return Element.GetReferenced(DatabaseAuditSpecification.AuditActionGroups).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditActionGroup(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditAction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseauditspecification.auditactions.aspx">DatabaseAuditSpecification.AuditActions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditAction> AuditActions 
		{
			get 
			{
				return Element.GetReferenced(DatabaseAuditSpecification.AuditActions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditAction(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlServerAuditReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseauditspecification.serveraudit.aspx">DatabaseAuditSpecification.ServerAudit</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlServerAuditReference> ServerAudit 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseAuditSpecification.ServerAudit).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlServerAuditReference(o, Microsoft.SqlServer.Dac.Model.ServerAudit.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseDdlTrigger"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.aspx">DatabaseDdlTrigger</see>
	///
	public partial class TSqlDatabaseDdlTriggerReference : TSqlDatabaseDdlTrigger, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDatabaseDdlTriggerReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseDdlTrigger"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.aspx">DatabaseDdlTrigger</see>
	///
	public partial class TSqlDatabaseDdlTrigger : TSqlModelElement	{
		private static ModelTypeClass typeClass = DatabaseDdlTrigger.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseDdlTrigger to adapt instances of <see cref="T:DatabaseDdlTrigger"/>
		/// </summary>
		public TSqlDatabaseDdlTrigger(TSqlObject obj)  : base(obj, DatabaseDdlTrigger.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseDdlTrigger to adapt instances of <see cref="T:DatabaseDdlTrigger"/>
		/// </summary>
		protected TSqlDatabaseDdlTrigger()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.ansinullson.aspx">DatabaseDdlTrigger.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(DatabaseDdlTrigger.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.classname.aspx">DatabaseDdlTrigger.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(DatabaseDdlTrigger.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.disabled.aspx">DatabaseDdlTrigger.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(DatabaseDdlTrigger.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.ExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.executeascaller.aspx">DatabaseDdlTrigger.ExecuteAsCaller</see>
		///
		public Boolean ExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(DatabaseDdlTrigger.ExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.executeasowner.aspx">DatabaseDdlTrigger.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(DatabaseDdlTrigger.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.executeasself.aspx">DatabaseDdlTrigger.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(DatabaseDdlTrigger.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.MethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.methodname.aspx">DatabaseDdlTrigger.MethodName</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(DatabaseDdlTrigger.MethodName);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.quotedidentifieron.aspx">DatabaseDdlTrigger.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(DatabaseDdlTrigger.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.TriggerType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.triggertype.aspx">DatabaseDdlTrigger.TriggerType</see>
		///
		public TriggerType TriggerType 
		{
			get { return Element.GetProperty<TriggerType>(DatabaseDdlTrigger.TriggerType);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseDdlTrigger.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.withencryption.aspx">DatabaseDdlTrigger.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(DatabaseDdlTrigger.WithEncryption);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.assembly.aspx">DatabaseDdlTrigger.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseDdlTrigger.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.bodydependencies.aspx">DatabaseDdlTrigger.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseDdlTrigger.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.eventgroup.aspx">DatabaseDdlTrigger.EventGroup</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup> EventGroup 
		{
			get 
			{
				return Element.GetReferenced(DatabaseDdlTrigger.EventGroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Model.EventType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.eventtype.aspx">DatabaseDdlTrigger.EventType</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> EventType 
		{
			get 
			{
				return Element.GetReferenced(DatabaseDdlTrigger.EventType).Cast<Microsoft.SqlServer.Dac.Model.EventType>();
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.login.aspx">DatabaseDdlTrigger.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseDdlTrigger.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseddltrigger.user.aspx">DatabaseDdlTrigger.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseDdlTrigger.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseEncryptionKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseencryptionkey.aspx">DatabaseEncryptionKey</see>
	///
	public partial class TSqlDatabaseEncryptionKeyReference : TSqlDatabaseEncryptionKey, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDatabaseEncryptionKeyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseEncryptionKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseencryptionkey.aspx">DatabaseEncryptionKey</see>
	///
	public partial class TSqlDatabaseEncryptionKey : TSqlModelElement	{
		private static ModelTypeClass typeClass = DatabaseEncryptionKey.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseEncryptionKey to adapt instances of <see cref="T:DatabaseEncryptionKey"/>
		/// </summary>
		public TSqlDatabaseEncryptionKey(TSqlObject obj)  : base(obj, DatabaseEncryptionKey.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseEncryptionKey to adapt instances of <see cref="T:DatabaseEncryptionKey"/>
		/// </summary>
		protected TSqlDatabaseEncryptionKey()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DatabaseEncryptionKey.Algorithm"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseencryptionkey.algorithm.aspx">DatabaseEncryptionKey.Algorithm</see>
		///
		public SymmetricKeyAlgorithm Algorithm 
		{
			get { return Element.GetProperty<SymmetricKeyAlgorithm>(DatabaseEncryptionKey.Algorithm);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseencryptionkey.asymmetrickey.aspx">DatabaseEncryptionKey.AsymmetricKey</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference> AsymmetricKey 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseEncryptionKey.AsymmetricKey).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference(o, Microsoft.SqlServer.Dac.Model.AsymmetricKey.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseencryptionkey.certificate.aspx">DatabaseEncryptionKey.Certificate</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> Certificate 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseEncryptionKey.Certificate).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseEventNotification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.aspx">DatabaseEventNotification</see>
	///
	public partial class TSqlDatabaseEventNotificationReference : TSqlDatabaseEventNotification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDatabaseEventNotificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseEventNotification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.aspx">DatabaseEventNotification</see>
	///
	public partial class TSqlDatabaseEventNotification : TSqlModelElement	{
		private static ModelTypeClass typeClass = DatabaseEventNotification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseEventNotification to adapt instances of <see cref="T:DatabaseEventNotification"/>
		/// </summary>
		public TSqlDatabaseEventNotification(TSqlObject obj)  : base(obj, DatabaseEventNotification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseEventNotification to adapt instances of <see cref="T:DatabaseEventNotification"/>
		/// </summary>
		protected TSqlDatabaseEventNotification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DatabaseEventNotification.BrokerInstanceSpecifier"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.brokerinstancespecifier.aspx">DatabaseEventNotification.BrokerInstanceSpecifier</see>
		///
		public String BrokerInstanceSpecifier 
		{
			get { return Element.GetProperty<String>(DatabaseEventNotification.BrokerInstanceSpecifier);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseEventNotification.BrokerService"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.brokerservice.aspx">DatabaseEventNotification.BrokerService</see>
		///
		public String BrokerService 
		{
			get { return Element.GetProperty<String>(DatabaseEventNotification.BrokerService);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseEventNotification.WithFanIn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.withfanin.aspx">DatabaseEventNotification.WithFanIn</see>
		///
		public Boolean WithFanIn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseEventNotification.WithFanIn);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.eventgroup.aspx">DatabaseEventNotification.EventGroup</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup> EventGroup 
		{
			get 
			{
				return Element.GetReferenced(DatabaseEventNotification.EventGroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Model.EventType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseeventnotification.eventtype.aspx">DatabaseEventNotification.EventType</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> EventType 
		{
			get 
			{
				return Element.GetReferenced(DatabaseEventNotification.EventType).Cast<Microsoft.SqlServer.Dac.Model.EventType>();
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseMirroringLanguageSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.aspx">DatabaseMirroringLanguageSpecifier</see>
	///
	public partial class TSqlDatabaseMirroringLanguageSpecifierReference : TSqlDatabaseMirroringLanguageSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDatabaseMirroringLanguageSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseMirroringLanguageSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.aspx">DatabaseMirroringLanguageSpecifier</see>
	///
	public partial class TSqlDatabaseMirroringLanguageSpecifier : TSqlModelElement,IEndpointLanguageSpecifier	{
		private static ModelTypeClass typeClass = DatabaseMirroringLanguageSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseMirroringLanguageSpecifier to adapt instances of <see cref="T:DatabaseMirroringLanguageSpecifier"/>
		/// </summary>
		public TSqlDatabaseMirroringLanguageSpecifier(TSqlObject obj)  : base(obj, DatabaseMirroringLanguageSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseMirroringLanguageSpecifier to adapt instances of <see cref="T:DatabaseMirroringLanguageSpecifier"/>
		/// </summary>
		protected TSqlDatabaseMirroringLanguageSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.encryptionalgorithmpart1.aspx">DatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1</see>
		///
		public ServiceBrokerEncryptionAlgorithm EncryptionAlgorithmPart1 
		{
			get { return Element.GetProperty<ServiceBrokerEncryptionAlgorithm>(DatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.encryptionalgorithmpart2.aspx">DatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2</see>
		///
		public ServiceBrokerEncryptionAlgorithm EncryptionAlgorithmPart2 
		{
			get { return Element.GetProperty<ServiceBrokerEncryptionAlgorithm>(DatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseMirroringLanguageSpecifier.EncryptionMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.encryptionmode.aspx">DatabaseMirroringLanguageSpecifier.EncryptionMode</see>
		///
		public EncryptionMode EncryptionMode 
		{
			get { return Element.GetProperty<EncryptionMode>(DatabaseMirroringLanguageSpecifier.EncryptionMode);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseMirroringLanguageSpecifier.RoleType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.roletype.aspx">DatabaseMirroringLanguageSpecifier.RoleType</see>
		///
		public DatabaseMirroringRole RoleType 
		{
			get { return Element.GetProperty<DatabaseMirroringRole>(DatabaseMirroringLanguageSpecifier.RoleType);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseMirroringLanguageSpecifier.UseCertificateFirst"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.usecertificatefirst.aspx">DatabaseMirroringLanguageSpecifier.UseCertificateFirst</see>
		///
		public Boolean UseCertificateFirst 
		{
			get { return Element.GetProperty<Boolean>(DatabaseMirroringLanguageSpecifier.UseCertificateFirst);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.windowsauthenticationmode.aspx">DatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode</see>
		///
		public AuthenticationModes WindowsAuthenticationMode 
		{
			get { return Element.GetProperty<AuthenticationModes>(DatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databasemirroringlanguagespecifier.authenticationcertificate.aspx">DatabaseMirroringLanguageSpecifier.AuthenticationCertificate</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> AuthenticationCertificate 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseMirroringLanguageSpecifier.AuthenticationCertificate).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseOptions"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.aspx">DatabaseOptions</see>
	///
	public partial class TSqlDatabaseOptionsReference : TSqlDatabaseOptions, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDatabaseOptionsReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DatabaseOptions"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.aspx">DatabaseOptions</see>
	///
	public partial class TSqlDatabaseOptions : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = DatabaseOptions.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseOptions to adapt instances of <see cref="T:DatabaseOptions"/>
		/// </summary>
		public TSqlDatabaseOptions(TSqlObject obj)  : base(obj, DatabaseOptions.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDatabaseOptions to adapt instances of <see cref="T:DatabaseOptions"/>
		/// </summary>
		protected TSqlDatabaseOptions()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AllowSnapshotIsolation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.allowsnapshotisolation.aspx">DatabaseOptions.AllowSnapshotIsolation</see>
		///
		public Boolean AllowSnapshotIsolation 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AllowSnapshotIsolation);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AnsiNullDefaultOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.ansinulldefaulton.aspx">DatabaseOptions.AnsiNullDefaultOn</see>
		///
		public Boolean AnsiNullDefaultOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AnsiNullDefaultOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.ansinullson.aspx">DatabaseOptions.AnsiNullsOn</see>
		///
		public Boolean AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AnsiPaddingOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.ansipaddingon.aspx">DatabaseOptions.AnsiPaddingOn</see>
		///
		public Boolean AnsiPaddingOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AnsiPaddingOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AnsiWarningsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.ansiwarningson.aspx">DatabaseOptions.AnsiWarningsOn</see>
		///
		public Boolean AnsiWarningsOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AnsiWarningsOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ArithAbortOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.arithaborton.aspx">DatabaseOptions.ArithAbortOn</see>
		///
		public Boolean ArithAbortOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.ArithAbortOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AutoClose"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.autoclose.aspx">DatabaseOptions.AutoClose</see>
		///
		public Boolean AutoClose 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AutoClose);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AutoCreateStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.autocreatestatistics.aspx">DatabaseOptions.AutoCreateStatistics</see>
		///
		public Boolean AutoCreateStatistics 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AutoCreateStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AutoCreateStatisticsIncremental"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.autocreatestatisticsincremental.aspx">DatabaseOptions.AutoCreateStatisticsIncremental</see>
		///
		public Boolean AutoCreateStatisticsIncremental 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AutoCreateStatisticsIncremental);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AutoShrink"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.autoshrink.aspx">DatabaseOptions.AutoShrink</see>
		///
		public Boolean AutoShrink 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AutoShrink);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AutoUpdateStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.autoupdatestatistics.aspx">DatabaseOptions.AutoUpdateStatistics</see>
		///
		public Boolean AutoUpdateStatistics 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AutoUpdateStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.AutoUpdateStatisticsAsync"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.autoupdatestatisticsasync.aspx">DatabaseOptions.AutoUpdateStatisticsAsync</see>
		///
		public Boolean AutoUpdateStatisticsAsync 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.AutoUpdateStatisticsAsync);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ChangeTrackingAutoCleanup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.changetrackingautocleanup.aspx">DatabaseOptions.ChangeTrackingAutoCleanup</see>
		///
		public Boolean ChangeTrackingAutoCleanup 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.ChangeTrackingAutoCleanup);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ChangeTrackingEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.changetrackingenabled.aspx">DatabaseOptions.ChangeTrackingEnabled</see>
		///
		public Boolean ChangeTrackingEnabled 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.ChangeTrackingEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ChangeTrackingRetentionPeriod"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.changetrackingretentionperiod.aspx">DatabaseOptions.ChangeTrackingRetentionPeriod</see>
		///
		public Int32 ChangeTrackingRetentionPeriod 
		{
			get { return Element.GetProperty<Int32>(DatabaseOptions.ChangeTrackingRetentionPeriod);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ChangeTrackingRetentionUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.changetrackingretentionunit.aspx">DatabaseOptions.ChangeTrackingRetentionUnit</see>
		///
		public TimeUnit ChangeTrackingRetentionUnit 
		{
			get { return Element.GetProperty<TimeUnit>(DatabaseOptions.ChangeTrackingRetentionUnit);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.Collation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.collation.aspx">DatabaseOptions.Collation</see>
		///
		public String Collation 
		{
			get { return Element.GetProperty<String>(DatabaseOptions.Collation);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.CompatibilityLevel"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.compatibilitylevel.aspx">DatabaseOptions.CompatibilityLevel</see>
		///
		public Int32 CompatibilityLevel 
		{
			get { return Element.GetProperty<Int32>(DatabaseOptions.CompatibilityLevel);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ConcatNullYieldsNull"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.concatnullyieldsnull.aspx">DatabaseOptions.ConcatNullYieldsNull</see>
		///
		public Boolean ConcatNullYieldsNull 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.ConcatNullYieldsNull);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.Containment"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.containment.aspx">DatabaseOptions.Containment</see>
		///
		public Containment Containment 
		{
			get { return Element.GetProperty<Containment>(DatabaseOptions.Containment);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.CursorCloseOnCommit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.cursorcloseoncommit.aspx">DatabaseOptions.CursorCloseOnCommit</see>
		///
		public Boolean CursorCloseOnCommit 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.CursorCloseOnCommit);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.CursorDefaultGlobalScope"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.cursordefaultglobalscope.aspx">DatabaseOptions.CursorDefaultGlobalScope</see>
		///
		public Boolean CursorDefaultGlobalScope 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.CursorDefaultGlobalScope);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.DatabaseStateOffline"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.databasestateoffline.aspx">DatabaseOptions.DatabaseStateOffline</see>
		///
		public Boolean DatabaseStateOffline 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.DatabaseStateOffline);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.DateCorrelationOptimizationOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.datecorrelationoptimizationon.aspx">DatabaseOptions.DateCorrelationOptimizationOn</see>
		///
		public Boolean DateCorrelationOptimizationOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.DateCorrelationOptimizationOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.DBChainingOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.dbchainingon.aspx">DatabaseOptions.DBChainingOn</see>
		///
		public Boolean DBChainingOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.DBChainingOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.DefaultFullTextLanguage"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.defaultfulltextlanguage.aspx">DatabaseOptions.DefaultFullTextLanguage</see>
		///
		public String DefaultFullTextLanguage 
		{
			get { return Element.GetProperty<String>(DatabaseOptions.DefaultFullTextLanguage);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.DefaultLanguage"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.defaultlanguage.aspx">DatabaseOptions.DefaultLanguage</see>
		///
		public String DefaultLanguage 
		{
			get { return Element.GetProperty<String>(DatabaseOptions.DefaultLanguage);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.DelayedDurabilityMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.delayeddurabilitymode.aspx">DatabaseOptions.DelayedDurabilityMode</see>
		///
		public DelayedDurabilityMode DelayedDurabilityMode 
		{
			get { return Element.GetProperty<DelayedDurabilityMode>(DatabaseOptions.DelayedDurabilityMode);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.FileStreamDirectoryName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.filestreamdirectoryname.aspx">DatabaseOptions.FileStreamDirectoryName</see>
		///
		public String FileStreamDirectoryName 
		{
			get { return Element.GetProperty<String>(DatabaseOptions.FileStreamDirectoryName);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.FullTextEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.fulltextenabled.aspx">DatabaseOptions.FullTextEnabled</see>
		///
		public Boolean FullTextEnabled 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.FullTextEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.HonorBrokerPriority"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.honorbrokerpriority.aspx">DatabaseOptions.HonorBrokerPriority</see>
		///
		public Boolean HonorBrokerPriority 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.HonorBrokerPriority);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.MemoryOptimizedElevateToSnapshot"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.memoryoptimizedelevatetosnapshot.aspx">DatabaseOptions.MemoryOptimizedElevateToSnapshot</see>
		///
		public Boolean MemoryOptimizedElevateToSnapshot 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.MemoryOptimizedElevateToSnapshot);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.NestedTriggersOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.nestedtriggerson.aspx">DatabaseOptions.NestedTriggersOn</see>
		///
		public Boolean NestedTriggersOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.NestedTriggersOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.NonTransactedFileStreamAccess"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.nontransactedfilestreamaccess.aspx">DatabaseOptions.NonTransactedFileStreamAccess</see>
		///
		public NonTransactedFileStreamAccess NonTransactedFileStreamAccess 
		{
			get { return Element.GetProperty<NonTransactedFileStreamAccess>(DatabaseOptions.NonTransactedFileStreamAccess);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.NumericRoundAbortOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.numericroundaborton.aspx">DatabaseOptions.NumericRoundAbortOn</see>
		///
		public Boolean NumericRoundAbortOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.NumericRoundAbortOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.PageVerifyMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.pageverifymode.aspx">DatabaseOptions.PageVerifyMode</see>
		///
		public PageVerifyMode PageVerifyMode 
		{
			get { return Element.GetProperty<PageVerifyMode>(DatabaseOptions.PageVerifyMode);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ParameterizationOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.parameterizationoption.aspx">DatabaseOptions.ParameterizationOption</see>
		///
		public ParameterizationOption ParameterizationOption 
		{
			get { return Element.GetProperty<ParameterizationOption>(DatabaseOptions.ParameterizationOption);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.quotedidentifieron.aspx">DatabaseOptions.QuotedIdentifierOn</see>
		///
		public Boolean QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ReadOnly"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.readonly.aspx">DatabaseOptions.ReadOnly</see>
		///
		public Boolean ReadOnly 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.ReadOnly);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.RecoveryMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.recoverymode.aspx">DatabaseOptions.RecoveryMode</see>
		///
		public RecoveryMode RecoveryMode 
		{
			get { return Element.GetProperty<RecoveryMode>(DatabaseOptions.RecoveryMode);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.RecursiveTriggersOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.recursivetriggerson.aspx">DatabaseOptions.RecursiveTriggersOn</see>
		///
		public Boolean RecursiveTriggersOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.RecursiveTriggersOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.ServiceBrokerOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.servicebrokeroption.aspx">DatabaseOptions.ServiceBrokerOption</see>
		///
		public ServiceBrokerOption ServiceBrokerOption 
		{
			get { return Element.GetProperty<ServiceBrokerOption>(DatabaseOptions.ServiceBrokerOption);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.SupplementalLoggingOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.supplementalloggingon.aspx">DatabaseOptions.SupplementalLoggingOn</see>
		///
		public Boolean SupplementalLoggingOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.SupplementalLoggingOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.TargetRecoveryTimePeriod"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.targetrecoverytimeperiod.aspx">DatabaseOptions.TargetRecoveryTimePeriod</see>
		///
		public Int32 TargetRecoveryTimePeriod 
		{
			get { return Element.GetProperty<Int32>(DatabaseOptions.TargetRecoveryTimePeriod);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.TargetRecoveryTimeUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.targetrecoverytimeunit.aspx">DatabaseOptions.TargetRecoveryTimeUnit</see>
		///
		public TimeUnit TargetRecoveryTimeUnit 
		{
			get { return Element.GetProperty<TimeUnit>(DatabaseOptions.TargetRecoveryTimeUnit);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.TornPageProtectionOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.tornpageprotectionon.aspx">DatabaseOptions.TornPageProtectionOn</see>
		///
		public Boolean TornPageProtectionOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.TornPageProtectionOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.TransactionIsolationReadCommittedSnapshot"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.transactionisolationreadcommittedsnapshot.aspx">DatabaseOptions.TransactionIsolationReadCommittedSnapshot</see>
		///
		public Boolean TransactionIsolationReadCommittedSnapshot 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.TransactionIsolationReadCommittedSnapshot);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.TransformNoiseWords"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.transformnoisewords.aspx">DatabaseOptions.TransformNoiseWords</see>
		///
		public Boolean TransformNoiseWords 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.TransformNoiseWords);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.Trustworthy"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.trustworthy.aspx">DatabaseOptions.Trustworthy</see>
		///
		public Boolean Trustworthy 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.Trustworthy);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.TwoDigitYearCutoff"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.twodigityearcutoff.aspx">DatabaseOptions.TwoDigitYearCutoff</see>
		///
		public Int16 TwoDigitYearCutoff 
		{
			get { return Element.GetProperty<Int16>(DatabaseOptions.TwoDigitYearCutoff);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.UserAccessOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.useraccessoption.aspx">DatabaseOptions.UserAccessOption</see>
		///
		public UserAccessOption UserAccessOption 
		{
			get { return Element.GetProperty<UserAccessOption>(DatabaseOptions.UserAccessOption);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.VardecimalStorageFormatOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.vardecimalstorageformaton.aspx">DatabaseOptions.VardecimalStorageFormatOn</see>
		///
		public Boolean VardecimalStorageFormatOn 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.VardecimalStorageFormatOn);}
		}


		///
		/// Property wrapper for <see cref="M:DatabaseOptions.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.withencryption.aspx">DatabaseOptions.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(DatabaseOptions.WithEncryption);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.defaultfilegroup.aspx">DatabaseOptions.DefaultFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> DefaultFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseOptions.DefaultFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.databaseoptions.defaultfilestreamfilegroup.aspx">DatabaseOptions.DefaultFileStreamFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> DefaultFileStreamFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DatabaseOptions.DefaultFileStreamFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DataCompressionOption"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datacompressionoption.aspx">DataCompressionOption</see>
	///
	public partial class TSqlDataCompressionOptionReference : TSqlDataCompressionOption, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDataCompressionOptionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DataCompressionOption"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datacompressionoption.aspx">DataCompressionOption</see>
	///
	public partial class TSqlDataCompressionOption : TSqlModelElement	{
		private static ModelTypeClass typeClass = DataCompressionOption.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDataCompressionOption to adapt instances of <see cref="T:DataCompressionOption"/>
		/// </summary>
		public TSqlDataCompressionOption(TSqlObject obj)  : base(obj, DataCompressionOption.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDataCompressionOption to adapt instances of <see cref="T:DataCompressionOption"/>
		/// </summary>
		protected TSqlDataCompressionOption()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DataCompressionOption.CompressionLevel"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datacompressionoption.compressionlevel.aspx">DataCompressionOption.CompressionLevel</see>
		///
		public CompressionLevel CompressionLevel 
		{
			get { return Element.GetProperty<CompressionLevel>(DataCompressionOption.CompressionLevel);}
		}


		///
		/// Property wrapper for <see cref="M:DataCompressionOption.PartitionNumber"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.datacompressionoption.partitionnumber.aspx">DataCompressionOption.PartitionNumber</see>
		///
		public Int32 PartitionNumber 
		{
			get { return Element.GetProperty<Int32>(DataCompressionOption.PartitionNumber);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Default"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.default.aspx">Default</see>
	///
	public partial class TSqlDefaultReference : TSqlDefault, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDefaultReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Default"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.default.aspx">Default</see>
	///
	public partial class TSqlDefault : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Default.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDefault to adapt instances of <see cref="T:Default"/>
		/// </summary>
		public TSqlDefault(TSqlObject obj)  : base(obj, Default.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDefault to adapt instances of <see cref="T:Default"/>
		/// </summary>
		protected TSqlDefault()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Default.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.default.expression.aspx">Default.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(Default.Expression);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.default.boundobjects.aspx">Default.BoundObjects</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BoundObjects 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Default.BoundObjects).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.default.schema.aspx">Default.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Default.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DefaultConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.aspx">DefaultConstraint</see>
	///
	public partial class TSqlDefaultConstraintReference : TSqlDefaultConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDefaultConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DefaultConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.aspx">DefaultConstraint</see>
	///
	public partial class TSqlDefaultConstraint : TSqlModelElement,IExtendedPropertyHost	{
		private static ModelTypeClass typeClass = DefaultConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDefaultConstraint to adapt instances of <see cref="T:DefaultConstraint"/>
		/// </summary>
		public TSqlDefaultConstraint(TSqlObject obj)  : base(obj, DefaultConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDefaultConstraint to adapt instances of <see cref="T:DefaultConstraint"/>
		/// </summary>
		protected TSqlDefaultConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DefaultConstraint.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.disabled.aspx">DefaultConstraint.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(DefaultConstraint.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:DefaultConstraint.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.expression.aspx">DefaultConstraint.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(DefaultConstraint.Expression);}
		}


		///
		/// Property wrapper for <see cref="M:DefaultConstraint.WithValues"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.withvalues.aspx">DefaultConstraint.WithValues</see>
		///
		public Boolean WithValues 
		{
			get { return Element.GetProperty<Boolean>(DefaultConstraint.WithValues);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.expressiondependencies.aspx">DefaultConstraint.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DefaultConstraint.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.host.aspx">DefaultConstraint.Host</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference> Host 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DefaultConstraint.Host).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference(o, Microsoft.SqlServer.Dac.Model.Table.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.defaultconstraint.targetcolumn.aspx">DefaultConstraint.TargetColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> TargetColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DefaultConstraint.TargetColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DmlTrigger"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.aspx">DmlTrigger</see>
	///
	public partial class TSqlDmlTriggerReference : TSqlDmlTrigger, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlDmlTriggerReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:DmlTrigger"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.aspx">DmlTrigger</see>
	///
	public partial class TSqlDmlTrigger : TSqlModelElement	{
		private static ModelTypeClass typeClass = DmlTrigger.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlDmlTrigger to adapt instances of <see cref="T:DmlTrigger"/>
		/// </summary>
		public TSqlDmlTrigger(TSqlObject obj)  : base(obj, DmlTrigger.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlDmlTrigger to adapt instances of <see cref="T:DmlTrigger"/>
		/// </summary>
		protected TSqlDmlTrigger()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:DmlTrigger.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.ansinullson.aspx">DmlTrigger.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(DmlTrigger.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.classname.aspx">DmlTrigger.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(DmlTrigger.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.DeleteOrderRestriction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.deleteorderrestriction.aspx">DmlTrigger.DeleteOrderRestriction</see>
		///
		public OrderRestriction DeleteOrderRestriction 
		{
			get { return Element.GetProperty<OrderRestriction>(DmlTrigger.DeleteOrderRestriction);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.disabled.aspx">DmlTrigger.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.ExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.executeascaller.aspx">DmlTrigger.ExecuteAsCaller</see>
		///
		public Boolean ExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.ExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.executeasowner.aspx">DmlTrigger.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.executeasself.aspx">DmlTrigger.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.InsertOrderRestriction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.insertorderrestriction.aspx">DmlTrigger.InsertOrderRestriction</see>
		///
		public OrderRestriction InsertOrderRestriction 
		{
			get { return Element.GetProperty<OrderRestriction>(DmlTrigger.InsertOrderRestriction);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.IsDeleteTrigger"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.isdeletetrigger.aspx">DmlTrigger.IsDeleteTrigger</see>
		///
		public Boolean IsDeleteTrigger 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.IsDeleteTrigger);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.IsInsertTrigger"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.isinserttrigger.aspx">DmlTrigger.IsInsertTrigger</see>
		///
		public Boolean IsInsertTrigger 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.IsInsertTrigger);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.IsUpdateTrigger"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.isupdatetrigger.aspx">DmlTrigger.IsUpdateTrigger</see>
		///
		public Boolean IsUpdateTrigger 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.IsUpdateTrigger);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.MethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.methodname.aspx">DmlTrigger.MethodName</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(DmlTrigger.MethodName);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.NotForReplication"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.notforreplication.aspx">DmlTrigger.NotForReplication</see>
		///
		public Boolean NotForReplication 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.NotForReplication);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.quotedidentifieron.aspx">DmlTrigger.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(DmlTrigger.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.TriggerType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.triggertype.aspx">DmlTrigger.TriggerType</see>
		///
		public TriggerType TriggerType 
		{
			get { return Element.GetProperty<TriggerType>(DmlTrigger.TriggerType);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.UpdateOrderRestriction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.updateorderrestriction.aspx">DmlTrigger.UpdateOrderRestriction</see>
		///
		public OrderRestriction UpdateOrderRestriction 
		{
			get { return Element.GetProperty<OrderRestriction>(DmlTrigger.UpdateOrderRestriction);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.WithAppend"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.withappend.aspx">DmlTrigger.WithAppend</see>
		///
		public Boolean WithAppend 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.WithAppend);}
		}


		///
		/// Property wrapper for <see cref="M:DmlTrigger.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.withencryption.aspx">DmlTrigger.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(DmlTrigger.WithEncryption);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.assembly.aspx">DmlTrigger.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DmlTrigger.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.bodydependencies.aspx">DmlTrigger.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DmlTrigger.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.login.aspx">DmlTrigger.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DmlTrigger.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.triggerobject.aspx">DmlTrigger.TriggerObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> TriggerObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DmlTrigger.TriggerObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.dmltrigger.user.aspx">DmlTrigger.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(DmlTrigger.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Endpoint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.aspx">Endpoint</see>
	///
	public partial class TSqlEndpointReference : TSqlEndpoint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEndpointReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Endpoint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.aspx">Endpoint</see>
	///
	public partial class TSqlEndpoint : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Endpoint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEndpoint to adapt instances of <see cref="T:Endpoint"/>
		/// </summary>
		public TSqlEndpoint(TSqlObject obj)  : base(obj, Endpoint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEndpoint to adapt instances of <see cref="T:Endpoint"/>
		/// </summary>
		protected TSqlEndpoint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Endpoint.Payload"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.payload.aspx">Endpoint.Payload</see>
		///
		public Payload Payload 
		{
			get { return Element.GetProperty<Payload>(Endpoint.Payload);}
		}


		///
		/// Property wrapper for <see cref="M:Endpoint.Protocol"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.protocol.aspx">Endpoint.Protocol</see>
		///
		public Protocol Protocol 
		{
			get { return Element.GetProperty<Protocol>(Endpoint.Protocol);}
		}


		///
		/// Property wrapper for <see cref="M:Endpoint.State"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.state.aspx">Endpoint.State</see>
		///
		public State State 
		{
			get { return Element.GetProperty<State>(Endpoint.State);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.authorizer.aspx">Endpoint.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Endpoint.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.payloadspecifier.aspx">Endpoint.PayloadSpecifier</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier> PayloadSpecifier 
		{
			get 
			{
				return Element.GetReferenced(Endpoint.PayloadSpecifier).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier "/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.endpoint.protocolspecifier.aspx">Endpoint.ProtocolSpecifier</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier > ProtocolSpecifier 
		{
			get 
			{
				return Element.GetReferenced(Endpoint.ProtocolSpecifier).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier )TSqlModelElement.AdaptInstance(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ErrorMessage"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.aspx">ErrorMessage</see>
	///
	public partial class TSqlErrorMessageReference : TSqlErrorMessage, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlErrorMessageReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ErrorMessage"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.aspx">ErrorMessage</see>
	///
	public partial class TSqlErrorMessage : TSqlModelElement	{
		private static ModelTypeClass typeClass = ErrorMessage.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlErrorMessage to adapt instances of <see cref="T:ErrorMessage"/>
		/// </summary>
		public TSqlErrorMessage(TSqlObject obj)  : base(obj, ErrorMessage.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlErrorMessage to adapt instances of <see cref="T:ErrorMessage"/>
		/// </summary>
		protected TSqlErrorMessage()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ErrorMessage.Language"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.language.aspx">ErrorMessage.Language</see>
		///
		public String Language 
		{
			get { return Element.GetProperty<String>(ErrorMessage.Language);}
		}


		///
		/// Property wrapper for <see cref="M:ErrorMessage.MessageNumber"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.messagenumber.aspx">ErrorMessage.MessageNumber</see>
		///
		public Int32 MessageNumber 
		{
			get { return Element.GetProperty<Int32>(ErrorMessage.MessageNumber);}
		}


		///
		/// Property wrapper for <see cref="M:ErrorMessage.MessageText"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.messagetext.aspx">ErrorMessage.MessageText</see>
		///
		public String MessageText 
		{
			get { return Element.GetProperty<String>(ErrorMessage.MessageText);}
		}


		///
		/// Property wrapper for <see cref="M:ErrorMessage.Severity"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.severity.aspx">ErrorMessage.Severity</see>
		///
		public Int32 Severity 
		{
			get { return Element.GetProperty<Int32>(ErrorMessage.Severity);}
		}


		///
		/// Property wrapper for <see cref="M:ErrorMessage.WithLog"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.errormessage.withlog.aspx">ErrorMessage.WithLog</see>
		///
		public Boolean WithLog 
		{
			get { return Element.GetProperty<Boolean>(ErrorMessage.WithLog);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventGroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventgroup.aspx">EventGroup</see>
	///
	public partial class TSqlEventGroupReference : TSqlEventGroup, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventGroupReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventGroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventgroup.aspx">EventGroup</see>
	///
	public partial class TSqlEventGroup : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventGroup.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventGroup to adapt instances of <see cref="T:EventGroup"/>
		/// </summary>
		public TSqlEventGroup(TSqlObject obj)  : base(obj, EventGroup.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventGroup to adapt instances of <see cref="T:EventGroup"/>
		/// </summary>
		protected TSqlEventGroup()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventGroup.Group"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventgroup.group.aspx">EventGroup.Group</see>
		///
		public EventGroupType Group 
		{
			get { return Element.GetProperty<EventGroupType>(EventGroup.Group);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSession"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.aspx">EventSession</see>
	///
	public partial class TSqlEventSessionReference : TSqlEventSession, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventSessionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSession"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.aspx">EventSession</see>
	///
	public partial class TSqlEventSession : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventSession.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventSession to adapt instances of <see cref="T:EventSession"/>
		/// </summary>
		public TSqlEventSession(TSqlObject obj)  : base(obj, EventSession.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventSession to adapt instances of <see cref="T:EventSession"/>
		/// </summary>
		protected TSqlEventSession()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventSession.EventRetentionMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.eventretentionmode.aspx">EventSession.EventRetentionMode</see>
		///
		public EventRetentionMode EventRetentionMode 
		{
			get { return Element.GetProperty<EventRetentionMode>(EventSession.EventRetentionMode);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.MaxDispatchLatency"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.maxdispatchlatency.aspx">EventSession.MaxDispatchLatency</see>
		///
		public Int32 MaxDispatchLatency 
		{
			get { return Element.GetProperty<Int32>(EventSession.MaxDispatchLatency);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.MaxEventSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.maxeventsize.aspx">EventSession.MaxEventSize</see>
		///
		public Int32 MaxEventSize 
		{
			get { return Element.GetProperty<Int32>(EventSession.MaxEventSize);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.MaxEventSizeUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.maxeventsizeunit.aspx">EventSession.MaxEventSizeUnit</see>
		///
		public MemoryUnit MaxEventSizeUnit 
		{
			get { return Element.GetProperty<MemoryUnit>(EventSession.MaxEventSizeUnit);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.MaxMemory"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.maxmemory.aspx">EventSession.MaxMemory</see>
		///
		public Int32 MaxMemory 
		{
			get { return Element.GetProperty<Int32>(EventSession.MaxMemory);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.MaxMemoryUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.maxmemoryunit.aspx">EventSession.MaxMemoryUnit</see>
		///
		public MemoryUnit MaxMemoryUnit 
		{
			get { return Element.GetProperty<MemoryUnit>(EventSession.MaxMemoryUnit);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.MemoryPartitionMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.memorypartitionmode.aspx">EventSession.MemoryPartitionMode</see>
		///
		public MemoryPartitionMode MemoryPartitionMode 
		{
			get { return Element.GetProperty<MemoryPartitionMode>(EventSession.MemoryPartitionMode);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.StartupState"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.startupstate.aspx">EventSession.StartupState</see>
		///
		public Boolean StartupState 
		{
			get { return Element.GetProperty<Boolean>(EventSession.StartupState);}
		}


		///
		/// Property wrapper for <see cref="M:EventSession.TrackCausality"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.trackcausality.aspx">EventSession.TrackCausality</see>
		///
		public Boolean TrackCausality 
		{
			get { return Element.GetProperty<Boolean>(EventSession.TrackCausality);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.eventdefinitions.aspx">EventSession.EventDefinitions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> EventDefinitions 
		{
			get 
			{
				return Element.GetReferenced(EventSession.EventDefinitions).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsession.eventtargets.aspx">EventSession.EventTargets</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> EventTargets 
		{
			get 
			{
				return Element.GetReferenced(EventSession.EventTargets).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionAction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionaction.aspx">EventSessionAction</see>
	///
	public partial class TSqlEventSessionActionReference : TSqlEventSessionAction, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventSessionActionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionAction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionaction.aspx">EventSessionAction</see>
	///
	public partial class TSqlEventSessionAction : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventSessionAction.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionAction to adapt instances of <see cref="T:EventSessionAction"/>
		/// </summary>
		public TSqlEventSessionAction(TSqlObject obj)  : base(obj, EventSessionAction.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionAction to adapt instances of <see cref="T:EventSessionAction"/>
		/// </summary>
		protected TSqlEventSessionAction()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventSessionAction.ActionName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionaction.actionname.aspx">EventSessionAction.ActionName</see>
		///
		public String ActionName 
		{
			get { return Element.GetProperty<String>(EventSessionAction.ActionName);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionAction.EventModuleGuid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionaction.eventmoduleguid.aspx">EventSessionAction.EventModuleGuid</see>
		///
		public String EventModuleGuid 
		{
			get { return Element.GetProperty<String>(EventSessionAction.EventModuleGuid);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionAction.EventPackageName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionaction.eventpackagename.aspx">EventSessionAction.EventPackageName</see>
		///
		public String EventPackageName 
		{
			get { return Element.GetProperty<String>(EventSessionAction.EventPackageName);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionDefinitions"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.aspx">EventSessionDefinitions</see>
	///
	public partial class TSqlEventSessionDefinitionsReference : TSqlEventSessionDefinitions, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventSessionDefinitionsReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionDefinitions"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.aspx">EventSessionDefinitions</see>
	///
	public partial class TSqlEventSessionDefinitions : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventSessionDefinitions.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionDefinitions to adapt instances of <see cref="T:EventSessionDefinitions"/>
		/// </summary>
		public TSqlEventSessionDefinitions(TSqlObject obj)  : base(obj, EventSessionDefinitions.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionDefinitions to adapt instances of <see cref="T:EventSessionDefinitions"/>
		/// </summary>
		protected TSqlEventSessionDefinitions()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventSessionDefinitions.EventModuleGuid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.eventmoduleguid.aspx">EventSessionDefinitions.EventModuleGuid</see>
		///
		public String EventModuleGuid 
		{
			get { return Element.GetProperty<String>(EventSessionDefinitions.EventModuleGuid);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionDefinitions.EventName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.eventname.aspx">EventSessionDefinitions.EventName</see>
		///
		public String EventName 
		{
			get { return Element.GetProperty<String>(EventSessionDefinitions.EventName);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionDefinitions.EventPackageName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.eventpackagename.aspx">EventSessionDefinitions.EventPackageName</see>
		///
		public String EventPackageName 
		{
			get { return Element.GetProperty<String>(EventSessionDefinitions.EventPackageName);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionDefinitions.WhereExpression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.whereexpression.aspx">EventSessionDefinitions.WhereExpression</see>
		///
		public String WhereExpression 
		{
			get { return (String)Element.GetProperty(EventSessionDefinitions.WhereExpression);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventSessionAction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.actions.aspx">EventSessionDefinitions.Actions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventSessionAction> Actions 
		{
			get 
			{
				return Element.GetReferenced(EventSessionDefinitions.Actions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventSessionAction(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiondefinitions.attributesettings.aspx">EventSessionDefinitions.AttributeSettings</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> AttributeSettings 
		{
			get 
			{
				return Element.GetReferenced(EventSessionDefinitions.AttributeSettings).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionSetting"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionsetting.aspx">EventSessionSetting</see>
	///
	public partial class TSqlEventSessionSettingReference : TSqlEventSessionSetting, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventSessionSettingReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionSetting"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionsetting.aspx">EventSessionSetting</see>
	///
	public partial class TSqlEventSessionSetting : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventSessionSetting.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionSetting to adapt instances of <see cref="T:EventSessionSetting"/>
		/// </summary>
		public TSqlEventSessionSetting(TSqlObject obj)  : base(obj, EventSessionSetting.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionSetting to adapt instances of <see cref="T:EventSessionSetting"/>
		/// </summary>
		protected TSqlEventSessionSetting()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventSessionSetting.SettingName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionsetting.settingname.aspx">EventSessionSetting.SettingName</see>
		///
		public String SettingName 
		{
			get { return Element.GetProperty<String>(EventSessionSetting.SettingName);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionSetting.SettingValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessionsetting.settingvalue.aspx">EventSessionSetting.SettingValue</see>
		///
		public String SettingValue 
		{
			get { return (String)Element.GetProperty(EventSessionSetting.SettingValue);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionTarget"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiontarget.aspx">EventSessionTarget</see>
	///
	public partial class TSqlEventSessionTargetReference : TSqlEventSessionTarget, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventSessionTargetReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventSessionTarget"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiontarget.aspx">EventSessionTarget</see>
	///
	public partial class TSqlEventSessionTarget : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventSessionTarget.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionTarget to adapt instances of <see cref="T:EventSessionTarget"/>
		/// </summary>
		public TSqlEventSessionTarget(TSqlObject obj)  : base(obj, EventSessionTarget.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventSessionTarget to adapt instances of <see cref="T:EventSessionTarget"/>
		/// </summary>
		protected TSqlEventSessionTarget()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventSessionTarget.EventModuleGuid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiontarget.eventmoduleguid.aspx">EventSessionTarget.EventModuleGuid</see>
		///
		public String EventModuleGuid 
		{
			get { return Element.GetProperty<String>(EventSessionTarget.EventModuleGuid);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionTarget.EventPackageName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiontarget.eventpackagename.aspx">EventSessionTarget.EventPackageName</see>
		///
		public String EventPackageName 
		{
			get { return Element.GetProperty<String>(EventSessionTarget.EventPackageName);}
		}


		///
		/// Property wrapper for <see cref="M:EventSessionTarget.TargetName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiontarget.targetname.aspx">EventSessionTarget.TargetName</see>
		///
		public String TargetName 
		{
			get { return Element.GetProperty<String>(EventSessionTarget.TargetName);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventsessiontarget.parametersettings.aspx">EventSessionTarget.ParameterSettings</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ParameterSettings 
		{
			get 
			{
				return Element.GetReferenced(EventSessionTarget.ParameterSettings).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventTypeSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventtypespecifier.aspx">EventTypeSpecifier</see>
	///
	public partial class TSqlEventTypeSpecifierReference : TSqlEventTypeSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlEventTypeSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:EventTypeSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventtypespecifier.aspx">EventTypeSpecifier</see>
	///
	public partial class TSqlEventTypeSpecifier : TSqlModelElement	{
		private static ModelTypeClass typeClass = EventTypeSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlEventTypeSpecifier to adapt instances of <see cref="T:EventTypeSpecifier"/>
		/// </summary>
		public TSqlEventTypeSpecifier(TSqlObject obj)  : base(obj, EventTypeSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlEventTypeSpecifier to adapt instances of <see cref="T:EventTypeSpecifier"/>
		/// </summary>
		protected TSqlEventTypeSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:EventTypeSpecifier.EventType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventtypespecifier.eventtype.aspx">EventTypeSpecifier.EventType</see>
		///
		public EventType EventType 
		{
			get { return Element.GetProperty<EventType>(EventTypeSpecifier.EventType);}
		}


		///
		/// Property wrapper for <see cref="M:EventTypeSpecifier.Order"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.eventtypespecifier.order.aspx">EventTypeSpecifier.Order</see>
		///
		public OrderRestriction Order 
		{
			get { return Element.GetProperty<OrderRestriction>(EventTypeSpecifier.Order);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ExtendedProcedure"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.aspx">ExtendedProcedure</see>
	///
	public partial class TSqlExtendedProcedureReference : TSqlExtendedProcedure, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlExtendedProcedureReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ExtendedProcedure"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.aspx">ExtendedProcedure</see>
	///
	public partial class TSqlExtendedProcedure : TSqlModelElement	{
		private static ModelTypeClass typeClass = ExtendedProcedure.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlExtendedProcedure to adapt instances of <see cref="T:ExtendedProcedure"/>
		/// </summary>
		public TSqlExtendedProcedure(TSqlObject obj)  : base(obj, ExtendedProcedure.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlExtendedProcedure to adapt instances of <see cref="T:ExtendedProcedure"/>
		/// </summary>
		protected TSqlExtendedProcedure()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ExtendedProcedure.ExeccuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.execcuteascaller.aspx">ExtendedProcedure.ExeccuteAsCaller</see>
		///
		public Boolean ExeccuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(ExtendedProcedure.ExeccuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:ExtendedProcedure.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.executeasowner.aspx">ExtendedProcedure.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(ExtendedProcedure.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:ExtendedProcedure.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.executeasself.aspx">ExtendedProcedure.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(ExtendedProcedure.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:ExtendedProcedure.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.withencryption.aspx">ExtendedProcedure.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(ExtendedProcedure.WithEncryption);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.login.aspx">ExtendedProcedure.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ExtendedProcedure.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.parameters.aspx">ExtendedProcedure.Parameters</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter> Parameters 
		{
			get 
			{
				return Element.GetReferenced(ExtendedProcedure.Parameters).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter(o));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.schema.aspx">ExtendedProcedure.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ExtendedProcedure.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedprocedure.user.aspx">ExtendedProcedure.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ExtendedProcedure.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ExtendedProperty"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedproperty.aspx">ExtendedProperty</see>
	///
	public partial class TSqlExtendedPropertyReference : TSqlExtendedProperty, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlExtendedPropertyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ExtendedProperty"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedproperty.aspx">ExtendedProperty</see>
	///
	public partial class TSqlExtendedProperty : TSqlModelElement	{
		private static ModelTypeClass typeClass = ExtendedProperty.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlExtendedProperty to adapt instances of <see cref="T:ExtendedProperty"/>
		/// </summary>
		public TSqlExtendedProperty(TSqlObject obj)  : base(obj, ExtendedProperty.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlExtendedProperty to adapt instances of <see cref="T:ExtendedProperty"/>
		/// </summary>
		protected TSqlExtendedProperty()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ExtendedProperty.Value"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedproperty.value.aspx">ExtendedProperty.Value</see>
		///
		public String Value 
		{
			get { return (String)Element.GetProperty(ExtendedProperty.Value);}
		}


		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.extendedproperty.host.aspx">ExtendedProperty.Host</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost> Host 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ExtendedProperty.Host).Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost>();
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SqlFile"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.aspx">SqlFile</see>
	///
	public partial class TSqlSqlFileReference : TSqlSqlFile, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSqlFileReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SqlFile"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.aspx">SqlFile</see>
	///
	public partial class TSqlSqlFile : TSqlModelElement	{
		private static ModelTypeClass typeClass = SqlFile.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSqlFile to adapt instances of <see cref="T:SqlFile"/>
		/// </summary>
		public TSqlSqlFile(TSqlObject obj)  : base(obj, SqlFile.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSqlFile to adapt instances of <see cref="T:SqlFile"/>
		/// </summary>
		protected TSqlSqlFile()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SqlFile.FileGrowth"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.filegrowth.aspx">SqlFile.FileGrowth</see>
		///
		public Int32? FileGrowth 
		{
			get { return Element.GetProperty<Int32?>(SqlFile.FileGrowth);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.FileGrowthUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.filegrowthunit.aspx">SqlFile.FileGrowthUnit</see>
		///
		public MemoryUnit FileGrowthUnit 
		{
			get { return Element.GetProperty<MemoryUnit>(SqlFile.FileGrowthUnit);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.FileName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.filename.aspx">SqlFile.FileName</see>
		///
		public String FileName 
		{
			get { return Element.GetProperty<String>(SqlFile.FileName);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.IsLogFile"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.islogfile.aspx">SqlFile.IsLogFile</see>
		///
		public Boolean IsLogFile 
		{
			get { return Element.GetProperty<Boolean>(SqlFile.IsLogFile);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.MaxSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.maxsize.aspx">SqlFile.MaxSize</see>
		///
		public Int32? MaxSize 
		{
			get { return Element.GetProperty<Int32?>(SqlFile.MaxSize);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.MaxSizeUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.maxsizeunit.aspx">SqlFile.MaxSizeUnit</see>
		///
		public MemoryUnit MaxSizeUnit 
		{
			get { return Element.GetProperty<MemoryUnit>(SqlFile.MaxSizeUnit);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.Offline"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.offline.aspx">SqlFile.Offline</see>
		///
		public Boolean Offline 
		{
			get { return Element.GetProperty<Boolean>(SqlFile.Offline);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.Size"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.size.aspx">SqlFile.Size</see>
		///
		public Int32? Size 
		{
			get { return Element.GetProperty<Int32?>(SqlFile.Size);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.SizeUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.sizeunit.aspx">SqlFile.SizeUnit</see>
		///
		public MemoryUnit SizeUnit 
		{
			get { return Element.GetProperty<MemoryUnit>(SqlFile.SizeUnit);}
		}


		///
		/// Property wrapper for <see cref="M:SqlFile.Unlimited"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.unlimited.aspx">SqlFile.Unlimited</see>
		///
		public Boolean Unlimited 
		{
			get { return Element.GetProperty<Boolean>(SqlFile.Unlimited);}
		}


		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sqlfile.filegroup.aspx">SqlFile.Filegroup</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SqlFile.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Filegroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filegroup.aspx">Filegroup</see>
	///
	public partial class TSqlFilegroupReference : TSqlFilegroup, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlFilegroupReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Filegroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filegroup.aspx">Filegroup</see>
	///
	public partial class TSqlFilegroup : TSqlModelElement	{
		private static ModelTypeClass typeClass = Filegroup.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlFilegroup to adapt instances of <see cref="T:Filegroup"/>
		/// </summary>
		public TSqlFilegroup(TSqlObject obj)  : base(obj, Filegroup.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlFilegroup to adapt instances of <see cref="T:Filegroup"/>
		/// </summary>
		protected TSqlFilegroup()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Filegroup.ContainsFileStream"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filegroup.containsfilestream.aspx">Filegroup.ContainsFileStream</see>
		///
		public Boolean ContainsFileStream 
		{
			get { return Element.GetProperty<Boolean>(Filegroup.ContainsFileStream);}
		}


		///
		/// Property wrapper for <see cref="M:Filegroup.ContainsMemoryOptimizedData"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filegroup.containsmemoryoptimizeddata.aspx">Filegroup.ContainsMemoryOptimizedData</see>
		///
		public Boolean ContainsMemoryOptimizedData 
		{
			get { return Element.GetProperty<Boolean>(Filegroup.ContainsMemoryOptimizedData);}
		}


		///
		/// Property wrapper for <see cref="M:Filegroup.ReadOnly"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filegroup.readonly.aspx">Filegroup.ReadOnly</see>
		///
		public Boolean ReadOnly 
		{
			get { return Element.GetProperty<Boolean>(Filegroup.ReadOnly);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ForeignKeyConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.aspx">ForeignKeyConstraint</see>
	///
	public partial class TSqlForeignKeyConstraintReference : TSqlForeignKeyConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlForeignKeyConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ForeignKeyConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.aspx">ForeignKeyConstraint</see>
	///
	public partial class TSqlForeignKeyConstraint : TSqlModelElement,IExtendedPropertyHost	{
		private static ModelTypeClass typeClass = ForeignKeyConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlForeignKeyConstraint to adapt instances of <see cref="T:ForeignKeyConstraint"/>
		/// </summary>
		public TSqlForeignKeyConstraint(TSqlObject obj)  : base(obj, ForeignKeyConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlForeignKeyConstraint to adapt instances of <see cref="T:ForeignKeyConstraint"/>
		/// </summary>
		protected TSqlForeignKeyConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ForeignKeyConstraint.DeleteAction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.deleteaction.aspx">ForeignKeyConstraint.DeleteAction</see>
		///
		public ForeignKeyAction DeleteAction 
		{
			get { return Element.GetProperty<ForeignKeyAction>(ForeignKeyConstraint.DeleteAction);}
		}


		///
		/// Property wrapper for <see cref="M:ForeignKeyConstraint.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.disabled.aspx">ForeignKeyConstraint.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(ForeignKeyConstraint.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:ForeignKeyConstraint.NotForReplication"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.notforreplication.aspx">ForeignKeyConstraint.NotForReplication</see>
		///
		public Boolean NotForReplication 
		{
			get { return Element.GetProperty<Boolean>(ForeignKeyConstraint.NotForReplication);}
		}


		///
		/// Property wrapper for <see cref="M:ForeignKeyConstraint.UpdateAction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.updateaction.aspx">ForeignKeyConstraint.UpdateAction</see>
		///
		public ForeignKeyAction UpdateAction 
		{
			get { return Element.GetProperty<ForeignKeyAction>(ForeignKeyConstraint.UpdateAction);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.columns.aspx">ForeignKeyConstraint.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ForeignKeyConstraint.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.foreigncolumns.aspx">ForeignKeyConstraint.ForeignColumns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> ForeignColumns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ForeignKeyConstraint.ForeignColumns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.foreigntable.aspx">ForeignKeyConstraint.ForeignTable</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference> ForeignTable 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ForeignKeyConstraint.ForeignTable).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference(o, Microsoft.SqlServer.Dac.Model.Table.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.foreignkeyconstraint.host.aspx">ForeignKeyConstraint.Host</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference> Host 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ForeignKeyConstraint.Host).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference(o, Microsoft.SqlServer.Dac.Model.Table.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextCatalog"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.aspx">FullTextCatalog</see>
	///
	public partial class TSqlFullTextCatalogReference : TSqlFullTextCatalog, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlFullTextCatalogReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextCatalog"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.aspx">FullTextCatalog</see>
	///
	public partial class TSqlFullTextCatalog : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = FullTextCatalog.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlFullTextCatalog to adapt instances of <see cref="T:FullTextCatalog"/>
		/// </summary>
		public TSqlFullTextCatalog(TSqlObject obj)  : base(obj, FullTextCatalog.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlFullTextCatalog to adapt instances of <see cref="T:FullTextCatalog"/>
		/// </summary>
		protected TSqlFullTextCatalog()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:FullTextCatalog.AccentSensitivity"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.accentsensitivity.aspx">FullTextCatalog.AccentSensitivity</see>
		///
		public Boolean? AccentSensitivity 
		{
			get { return Element.GetProperty<Boolean?>(FullTextCatalog.AccentSensitivity);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextCatalog.IsDefault"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.isdefault.aspx">FullTextCatalog.IsDefault</see>
		///
		public Boolean IsDefault 
		{
			get { return Element.GetProperty<Boolean>(FullTextCatalog.IsDefault);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextCatalog.Path"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.path.aspx">FullTextCatalog.Path</see>
		///
		public String Path 
		{
			get { return Element.GetProperty<String>(FullTextCatalog.Path);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.authorizer.aspx">FullTextCatalog.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextCatalog.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextcatalog.filegroup.aspx">FullTextCatalog.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextCatalog.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.aspx">FullTextIndex</see>
	///
	public partial class TSqlFullTextIndexReference : TSqlFullTextIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlFullTextIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.aspx">FullTextIndex</see>
	///
	public partial class TSqlFullTextIndex : TSqlModelElement	{
		private static ModelTypeClass typeClass = FullTextIndex.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlFullTextIndex to adapt instances of <see cref="T:FullTextIndex"/>
		/// </summary>
		public TSqlFullTextIndex(TSqlObject obj)  : base(obj, FullTextIndex.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlFullTextIndex to adapt instances of <see cref="T:FullTextIndex"/>
		/// </summary>
		protected TSqlFullTextIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:FullTextIndex.ChangeTracking"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.changetracking.aspx">FullTextIndex.ChangeTracking</see>
		///
		public ChangeTrackingOption ChangeTracking 
		{
			get { return Element.GetProperty<ChangeTrackingOption>(FullTextIndex.ChangeTracking);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextIndex.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.disabled.aspx">FullTextIndex.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(FullTextIndex.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextIndex.Replicated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.replicated.aspx">FullTextIndex.Replicated</see>
		///
		public Boolean Replicated 
		{
			get { return Element.GetProperty<Boolean>(FullTextIndex.Replicated);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextIndex.StopListOff"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.stoplistoff.aspx">FullTextIndex.StopListOff</see>
		///
		public Boolean StopListOff 
		{
			get { return Element.GetProperty<Boolean>(FullTextIndex.StopListOff);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextIndex.UseSystemStopList"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.usesystemstoplist.aspx">FullTextIndex.UseSystemStopList</see>
		///
		public Boolean UseSystemStopList 
		{
			get { return Element.GetProperty<Boolean>(FullTextIndex.UseSystemStopList);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.catalog.aspx">FullTextIndex.Catalog</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference> Catalog 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndex.Catalog).Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference>();
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFullTextIndexColumnSpecifier"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.columns.aspx">FullTextIndex.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFullTextIndexColumnSpecifier> Columns 
		{
			get 
			{
				return Element.GetReferenced(FullTextIndex.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFullTextIndexColumnSpecifier(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.filegroup.aspx">FullTextIndex.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndex.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.indexedobject.aspx">FullTextIndex.IndexedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> IndexedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndex.IndexedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISpecifiesIndexElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSearchPropertyListReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.searchpropertylist.aspx">FullTextIndex.SearchPropertyList</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSearchPropertyListReference> SearchPropertyList 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndex.SearchPropertyList).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSearchPropertyListReference(o, Microsoft.SqlServer.Dac.Model.SearchPropertyList.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.stoplist.aspx">FullTextIndex.StopList</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> StopList 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndex.StopList).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindex.uniqueindexname.aspx">FullTextIndex.UniqueIndexName</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> UniqueIndexName 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndex.UniqueIndexName).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextIndexColumnSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindexcolumnspecifier.aspx">FullTextIndexColumnSpecifier</see>
	///
	public partial class TSqlFullTextIndexColumnSpecifierReference : TSqlFullTextIndexColumnSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlFullTextIndexColumnSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextIndexColumnSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindexcolumnspecifier.aspx">FullTextIndexColumnSpecifier</see>
	///
	public partial class TSqlFullTextIndexColumnSpecifier : TSqlModelElement	{
		private static ModelTypeClass typeClass = FullTextIndexColumnSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlFullTextIndexColumnSpecifier to adapt instances of <see cref="T:FullTextIndexColumnSpecifier"/>
		/// </summary>
		public TSqlFullTextIndexColumnSpecifier(TSqlObject obj)  : base(obj, FullTextIndexColumnSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlFullTextIndexColumnSpecifier to adapt instances of <see cref="T:FullTextIndexColumnSpecifier"/>
		/// </summary>
		protected TSqlFullTextIndexColumnSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:FullTextIndexColumnSpecifier.LanguageId"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindexcolumnspecifier.languageid.aspx">FullTextIndexColumnSpecifier.LanguageId</see>
		///
		public Int32? LanguageId 
		{
			get { return Element.GetProperty<Int32?>(FullTextIndexColumnSpecifier.LanguageId);}
		}


		///
		/// Property wrapper for <see cref="M:FullTextIndexColumnSpecifier.PartOfStatisticalSemantics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindexcolumnspecifier.partofstatisticalsemantics.aspx">FullTextIndexColumnSpecifier.PartOfStatisticalSemantics</see>
		///
		public Boolean PartOfStatisticalSemantics 
		{
			get { return Element.GetProperty<Boolean>(FullTextIndexColumnSpecifier.PartOfStatisticalSemantics);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindexcolumnspecifier.column.aspx">FullTextIndexColumnSpecifier.Column</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Column 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndexColumnSpecifier.Column).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextindexcolumnspecifier.typecolumn.aspx">FullTextIndexColumnSpecifier.TypeColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> TypeColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextIndexColumnSpecifier.TypeColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextStopList"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextstoplist.aspx">FullTextStopList</see>
	///
	public partial class TSqlFullTextStopListReference : TSqlFullTextStopList, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlFullTextStopListReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FullTextStopList"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextstoplist.aspx">FullTextStopList</see>
	///
	public partial class TSqlFullTextStopList : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = FullTextStopList.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlFullTextStopList to adapt instances of <see cref="T:FullTextStopList"/>
		/// </summary>
		public TSqlFullTextStopList(TSqlObject obj)  : base(obj, FullTextStopList.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlFullTextStopList to adapt instances of <see cref="T:FullTextStopList"/>
		/// </summary>
		protected TSqlFullTextStopList()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.fulltextstoplist.authorizer.aspx">FullTextStopList.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FullTextStopList.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:HttpProtocolSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.aspx">HttpProtocolSpecifier</see>
	///
	public partial class TSqlHttpProtocolSpecifierReference : TSqlHttpProtocolSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlHttpProtocolSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:HttpProtocolSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.aspx">HttpProtocolSpecifier</see>
	///
	public partial class TSqlHttpProtocolSpecifier : TSqlModelElement,IProtocolSpecifier	{
		private static ModelTypeClass typeClass = HttpProtocolSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlHttpProtocolSpecifier to adapt instances of <see cref="T:HttpProtocolSpecifier"/>
		/// </summary>
		public TSqlHttpProtocolSpecifier(TSqlObject obj)  : base(obj, HttpProtocolSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlHttpProtocolSpecifier to adapt instances of <see cref="T:HttpProtocolSpecifier"/>
		/// </summary>
		protected TSqlHttpProtocolSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.AuthenticationMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.authenticationmode.aspx">HttpProtocolSpecifier.AuthenticationMode</see>
		///
		public AuthenticationModes AuthenticationMode 
		{
			get { return Element.GetProperty<AuthenticationModes>(HttpProtocolSpecifier.AuthenticationMode);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.AuthenticationRealm"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.authenticationrealm.aspx">HttpProtocolSpecifier.AuthenticationRealm</see>
		///
		public String AuthenticationRealm 
		{
			get { return Element.GetProperty<String>(HttpProtocolSpecifier.AuthenticationRealm);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.ClearPort"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.clearport.aspx">HttpProtocolSpecifier.ClearPort</see>
		///
		public Int32? ClearPort 
		{
			get { return Element.GetProperty<Int32?>(HttpProtocolSpecifier.ClearPort);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.CompressionEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.compressionenabled.aspx">HttpProtocolSpecifier.CompressionEnabled</see>
		///
		public Boolean CompressionEnabled 
		{
			get { return Element.GetProperty<Boolean>(HttpProtocolSpecifier.CompressionEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.DefaultLogonDomain"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.defaultlogondomain.aspx">HttpProtocolSpecifier.DefaultLogonDomain</see>
		///
		public String DefaultLogonDomain 
		{
			get { return Element.GetProperty<String>(HttpProtocolSpecifier.DefaultLogonDomain);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.ListeningOnAllNoneReservedSites"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.listeningonallnonereservedsites.aspx">HttpProtocolSpecifier.ListeningOnAllNoneReservedSites</see>
		///
		public Boolean ListeningOnAllNoneReservedSites 
		{
			get { return Element.GetProperty<Boolean>(HttpProtocolSpecifier.ListeningOnAllNoneReservedSites);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.ListeningOnAllSites"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.listeningonallsites.aspx">HttpProtocolSpecifier.ListeningOnAllSites</see>
		///
		public Boolean ListeningOnAllSites 
		{
			get { return Element.GetProperty<Boolean>(HttpProtocolSpecifier.ListeningOnAllSites);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.Path"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.path.aspx">HttpProtocolSpecifier.Path</see>
		///
		public String Path 
		{
			get { return Element.GetProperty<String>(HttpProtocolSpecifier.Path);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.Ports"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.ports.aspx">HttpProtocolSpecifier.Ports</see>
		///
		public HttpPorts Ports 
		{
			get { return Element.GetProperty<HttpPorts>(HttpProtocolSpecifier.Ports);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.SslPort"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.sslport.aspx">HttpProtocolSpecifier.SslPort</see>
		///
		public Int32? SslPort 
		{
			get { return Element.GetProperty<Int32?>(HttpProtocolSpecifier.SslPort);}
		}


		///
		/// Property wrapper for <see cref="M:HttpProtocolSpecifier.Website"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.httpprotocolspecifier.website.aspx">HttpProtocolSpecifier.Website</see>
		///
		public String Website 
		{
			get { return Element.GetProperty<String>(HttpProtocolSpecifier.Website);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:LinkedServer"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.aspx">LinkedServer</see>
	///
	public partial class TSqlLinkedServerReference : TSqlLinkedServer, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlLinkedServerReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:LinkedServer"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.aspx">LinkedServer</see>
	///
	public partial class TSqlLinkedServer : TSqlModelElement	{
		private static ModelTypeClass typeClass = LinkedServer.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlLinkedServer to adapt instances of <see cref="T:LinkedServer"/>
		/// </summary>
		public TSqlLinkedServer(TSqlObject obj)  : base(obj, LinkedServer.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlLinkedServer to adapt instances of <see cref="T:LinkedServer"/>
		/// </summary>
		protected TSqlLinkedServer()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:LinkedServer.Catalog"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.catalog.aspx">LinkedServer.Catalog</see>
		///
		public String Catalog 
		{
			get { return Element.GetProperty<String>(LinkedServer.Catalog);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.CollationCompatible"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.collationcompatible.aspx">LinkedServer.CollationCompatible</see>
		///
		public Boolean CollationCompatible 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.CollationCompatible);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.CollationName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.collationname.aspx">LinkedServer.CollationName</see>
		///
		public String CollationName 
		{
			get { return Element.GetProperty<String>(LinkedServer.CollationName);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.ConnectTimeout"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.connecttimeout.aspx">LinkedServer.ConnectTimeout</see>
		///
		public Int32 ConnectTimeout 
		{
			get { return Element.GetProperty<Int32>(LinkedServer.ConnectTimeout);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.DataAccess"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.dataaccess.aspx">LinkedServer.DataAccess</see>
		///
		public Boolean DataAccess 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.DataAccess);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.DataSource"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.datasource.aspx">LinkedServer.DataSource</see>
		///
		public String DataSource 
		{
			get { return Element.GetProperty<String>(LinkedServer.DataSource);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.IsDistributor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.isdistributor.aspx">LinkedServer.IsDistributor</see>
		///
		public Boolean IsDistributor 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.IsDistributor);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.IsPublisher"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.ispublisher.aspx">LinkedServer.IsPublisher</see>
		///
		public Boolean IsPublisher 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.IsPublisher);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.IsSubscriber"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.issubscriber.aspx">LinkedServer.IsSubscriber</see>
		///
		public Boolean IsSubscriber 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.IsSubscriber);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.LazySchemaValidationEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.lazyschemavalidationenabled.aspx">LinkedServer.LazySchemaValidationEnabled</see>
		///
		public Boolean LazySchemaValidationEnabled 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.LazySchemaValidationEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.Location"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.location.aspx">LinkedServer.Location</see>
		///
		public String Location 
		{
			get { return Element.GetProperty<String>(LinkedServer.Location);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.ProductName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.productname.aspx">LinkedServer.ProductName</see>
		///
		public String ProductName 
		{
			get { return Element.GetProperty<String>(LinkedServer.ProductName);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.ProviderName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.providername.aspx">LinkedServer.ProviderName</see>
		///
		public String ProviderName 
		{
			get { return Element.GetProperty<String>(LinkedServer.ProviderName);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.ProviderString"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.providerstring.aspx">LinkedServer.ProviderString</see>
		///
		public String ProviderString 
		{
			get { return Element.GetProperty<String>(LinkedServer.ProviderString);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.QueryTimeout"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.querytimeout.aspx">LinkedServer.QueryTimeout</see>
		///
		public Int32 QueryTimeout 
		{
			get { return Element.GetProperty<Int32>(LinkedServer.QueryTimeout);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.RemoteProcTransactionPromotionEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.remoteproctransactionpromotionenabled.aspx">LinkedServer.RemoteProcTransactionPromotionEnabled</see>
		///
		public Boolean RemoteProcTransactionPromotionEnabled 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.RemoteProcTransactionPromotionEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.RpcEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.rpcenabled.aspx">LinkedServer.RpcEnabled</see>
		///
		public Boolean RpcEnabled 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.RpcEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.RpcOutEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.rpcoutenabled.aspx">LinkedServer.RpcOutEnabled</see>
		///
		public Boolean RpcOutEnabled 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.RpcOutEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServer.UseRemoteCollation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserver.useremotecollation.aspx">LinkedServer.UseRemoteCollation</see>
		///
		public Boolean UseRemoteCollation 
		{
			get { return Element.GetProperty<Boolean>(LinkedServer.UseRemoteCollation);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:LinkedServerLogin"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.aspx">LinkedServerLogin</see>
	///
	public partial class TSqlLinkedServerLoginReference : TSqlLinkedServerLogin, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlLinkedServerLoginReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:LinkedServerLogin"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.aspx">LinkedServerLogin</see>
	///
	public partial class TSqlLinkedServerLogin : TSqlModelElement	{
		private static ModelTypeClass typeClass = LinkedServerLogin.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlLinkedServerLogin to adapt instances of <see cref="T:LinkedServerLogin"/>
		/// </summary>
		public TSqlLinkedServerLogin(TSqlObject obj)  : base(obj, LinkedServerLogin.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlLinkedServerLogin to adapt instances of <see cref="T:LinkedServerLogin"/>
		/// </summary>
		protected TSqlLinkedServerLogin()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:LinkedServerLogin.LinkedServerLoginName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.linkedserverloginname.aspx">LinkedServerLogin.LinkedServerLoginName</see>
		///
		public String LinkedServerLoginName 
		{
			get { return Element.GetProperty<String>(LinkedServerLogin.LinkedServerLoginName);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServerLogin.LinkedServerPassword"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.linkedserverpassword.aspx">LinkedServerLogin.LinkedServerPassword</see>
		///
		public String LinkedServerPassword 
		{
			get { return Element.GetProperty<String>(LinkedServerLogin.LinkedServerPassword);}
		}


		///
		/// Property wrapper for <see cref="M:LinkedServerLogin.UseSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.useself.aspx">LinkedServerLogin.UseSelf</see>
		///
		public Boolean UseSelf 
		{
			get { return Element.GetProperty<Boolean>(LinkedServerLogin.UseSelf);}
		}


		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLinkedServerReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.linkedserver.aspx">LinkedServerLogin.LinkedServer</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLinkedServerReference> LinkedServer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(LinkedServerLogin.LinkedServer).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLinkedServerReference(o, Microsoft.SqlServer.Dac.Model.LinkedServer.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.linkedserverlogin.locallogin.aspx">LinkedServerLogin.LocalLogin</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> LocalLogin 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(LinkedServerLogin.LocalLogin).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Login"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.aspx">Login</see>
	///
	public partial class TSqlLoginReference : TSqlLogin, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlLoginReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Login"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.aspx">Login</see>
	///
	public partial class TSqlLogin : TSqlModelElement	{
		private static ModelTypeClass typeClass = Login.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlLogin to adapt instances of <see cref="T:Login"/>
		/// </summary>
		public TSqlLogin(TSqlObject obj)  : base(obj, Login.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlLogin to adapt instances of <see cref="T:Login"/>
		/// </summary>
		protected TSqlLogin()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Login.CheckExpiration"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.checkexpiration.aspx">Login.CheckExpiration</see>
		///
		public Boolean CheckExpiration 
		{
			get { return Element.GetProperty<Boolean>(Login.CheckExpiration);}
		}


		///
		/// Property wrapper for <see cref="M:Login.CheckPolicy"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.checkpolicy.aspx">Login.CheckPolicy</see>
		///
		public Boolean CheckPolicy 
		{
			get { return Element.GetProperty<Boolean>(Login.CheckPolicy);}
		}


		///
		/// Property wrapper for <see cref="M:Login.DefaultDatabase"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.defaultdatabase.aspx">Login.DefaultDatabase</see>
		///
		public String DefaultDatabase 
		{
			get { return Element.GetProperty<String>(Login.DefaultDatabase);}
		}


		///
		/// Property wrapper for <see cref="M:Login.DefaultLanguage"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.defaultlanguage.aspx">Login.DefaultLanguage</see>
		///
		public String DefaultLanguage 
		{
			get { return Element.GetProperty<String>(Login.DefaultLanguage);}
		}


		///
		/// Property wrapper for <see cref="M:Login.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.disabled.aspx">Login.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(Login.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:Login.EncryptionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.encryptionoption.aspx">Login.EncryptionOption</see>
		///
		public LoginEncryptionOption EncryptionOption 
		{
			get { return Element.GetProperty<LoginEncryptionOption>(Login.EncryptionOption);}
		}


		///
		/// Property wrapper for <see cref="M:Login.MappedToWindowsLogin"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.mappedtowindowslogin.aspx">Login.MappedToWindowsLogin</see>
		///
		public Boolean MappedToWindowsLogin 
		{
			get { return Element.GetProperty<Boolean>(Login.MappedToWindowsLogin);}
		}


		///
		/// Property wrapper for <see cref="M:Login.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.password.aspx">Login.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(Login.Password);}
		}


		///
		/// Property wrapper for <see cref="M:Login.PasswordHashed"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.passwordhashed.aspx">Login.PasswordHashed</see>
		///
		public Boolean PasswordHashed 
		{
			get { return Element.GetProperty<Boolean>(Login.PasswordHashed);}
		}


		///
		/// Property wrapper for <see cref="M:Login.PasswordMustChange"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.passwordmustchange.aspx">Login.PasswordMustChange</see>
		///
		public Boolean PasswordMustChange 
		{
			get { return Element.GetProperty<Boolean>(Login.PasswordMustChange);}
		}


		///
		/// Property wrapper for <see cref="M:Login.Sid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.sid.aspx">Login.Sid</see>
		///
		public String Sid 
		{
			get { return Element.GetProperty<String>(Login.Sid);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.asymmetrickey.aspx">Login.AsymmetricKey</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference> AsymmetricKey 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Login.AsymmetricKey).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference(o, Microsoft.SqlServer.Dac.Model.AsymmetricKey.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.certificate.aspx">Login.Certificate</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> Certificate 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Login.Certificate).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCredentialReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.login.credential.aspx">Login.Credential</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCredentialReference> Credential 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Login.Credential).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCredentialReference(o, Microsoft.SqlServer.Dac.Model.Credential.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:MasterKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.masterkey.aspx">MasterKey</see>
	///
	public partial class TSqlMasterKeyReference : TSqlMasterKey, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlMasterKeyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:MasterKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.masterkey.aspx">MasterKey</see>
	///
	public partial class TSqlMasterKey : TSqlModelElement	{
		private static ModelTypeClass typeClass = MasterKey.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlMasterKey to adapt instances of <see cref="T:MasterKey"/>
		/// </summary>
		public TSqlMasterKey(TSqlObject obj)  : base(obj, MasterKey.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlMasterKey to adapt instances of <see cref="T:MasterKey"/>
		/// </summary>
		protected TSqlMasterKey()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:MasterKey.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.masterkey.password.aspx">MasterKey.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(MasterKey.Password);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:MessageType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.messagetype.aspx">MessageType</see>
	///
	public partial class TSqlMessageTypeReference : TSqlMessageType, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlMessageTypeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:MessageType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.messagetype.aspx">MessageType</see>
	///
	public partial class TSqlMessageType : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = MessageType.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlMessageType to adapt instances of <see cref="T:MessageType"/>
		/// </summary>
		public TSqlMessageType(TSqlObject obj)  : base(obj, MessageType.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlMessageType to adapt instances of <see cref="T:MessageType"/>
		/// </summary>
		protected TSqlMessageType()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:MessageType.ValidationMethod"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.messagetype.validationmethod.aspx">MessageType.ValidationMethod</see>
		///
		public ValidationMethod ValidationMethod 
		{
			get { return Element.GetProperty<ValidationMethod>(MessageType.ValidationMethod);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.messagetype.authorizer.aspx">MessageType.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(MessageType.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.messagetype.xmlschemacollection.aspx">MessageType.XmlSchemaCollection</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference> XmlSchemaCollection 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(MessageType.XmlSchemaCollection).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference(o, Microsoft.SqlServer.Dac.Model.XmlSchemaCollection.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PartitionFunction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionfunction.aspx">PartitionFunction</see>
	///
	public partial class TSqlPartitionFunctionReference : TSqlPartitionFunction, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPartitionFunctionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PartitionFunction"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionfunction.aspx">PartitionFunction</see>
	///
	public partial class TSqlPartitionFunction : TSqlModelElement	{
		private static ModelTypeClass typeClass = PartitionFunction.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPartitionFunction to adapt instances of <see cref="T:PartitionFunction"/>
		/// </summary>
		public TSqlPartitionFunction(TSqlObject obj)  : base(obj, PartitionFunction.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPartitionFunction to adapt instances of <see cref="T:PartitionFunction"/>
		/// </summary>
		protected TSqlPartitionFunction()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:PartitionFunction.Range"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionfunction.range.aspx">PartitionFunction.Range</see>
		///
		public PartitionRange Range 
		{
			get { return Element.GetProperty<PartitionRange>(PartitionFunction.Range);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionfunction.boundaryvalues.aspx">PartitionFunction.BoundaryValues</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionValue> BoundaryValues 
		{
			get 
			{
				return Element.GetReferenced(PartitionFunction.BoundaryValues).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionValue(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionfunction.parametertype.aspx">PartitionFunction.ParameterType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> ParameterType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PartitionFunction.ParameterType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PartitionScheme"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionscheme.aspx">PartitionScheme</see>
	///
	public partial class TSqlPartitionSchemeReference : TSqlPartitionScheme, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPartitionSchemeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PartitionScheme"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionscheme.aspx">PartitionScheme</see>
	///
	public partial class TSqlPartitionScheme : TSqlModelElement	{
		private static ModelTypeClass typeClass = PartitionScheme.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPartitionScheme to adapt instances of <see cref="T:PartitionScheme"/>
		/// </summary>
		public TSqlPartitionScheme(TSqlObject obj)  : base(obj, PartitionScheme.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPartitionScheme to adapt instances of <see cref="T:PartitionScheme"/>
		/// </summary>
		protected TSqlPartitionScheme()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:PartitionScheme.AllToOneFilegroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionscheme.alltoonefilegroup.aspx">PartitionScheme.AllToOneFilegroup</see>
		///
		public Boolean AllToOneFilegroup 
		{
			get { return Element.GetProperty<Boolean>(PartitionScheme.AllToOneFilegroup);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.PartitionSchemeFilegroupsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionscheme.filegroups.aspx">PartitionScheme.Filegroups</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.PartitionSchemeFilegroupsReference> Filegroups 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PartitionScheme.Filegroups).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.PartitionSchemeFilegroupsReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionFunctionReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionscheme.partitionfunction.aspx">PartitionScheme.PartitionFunction</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionFunctionReference> PartitionFunction 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PartitionScheme.PartitionFunction).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionFunctionReference(o, Microsoft.SqlServer.Dac.Model.PartitionFunction.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PartitionValue"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionvalue.aspx">PartitionValue</see>
	///
	public partial class TSqlPartitionValueReference : TSqlPartitionValue, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPartitionValueReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PartitionValue"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionvalue.aspx">PartitionValue</see>
	///
	public partial class TSqlPartitionValue : TSqlModelElement	{
		private static ModelTypeClass typeClass = PartitionValue.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPartitionValue to adapt instances of <see cref="T:PartitionValue"/>
		/// </summary>
		public TSqlPartitionValue(TSqlObject obj)  : base(obj, PartitionValue.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPartitionValue to adapt instances of <see cref="T:PartitionValue"/>
		/// </summary>
		protected TSqlPartitionValue()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:PartitionValue.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionvalue.expression.aspx">PartitionValue.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(PartitionValue.Expression);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.partitionvalue.expressiondependencies.aspx">PartitionValue.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PartitionValue.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Permission"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.aspx">Permission</see>
	///
	public partial class TSqlPermissionReference : TSqlPermission, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPermissionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Permission"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.aspx">Permission</see>
	///
	public partial class TSqlPermission : TSqlModelElement	{
		private static ModelTypeClass typeClass = Permission.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPermission to adapt instances of <see cref="T:Permission"/>
		/// </summary>
		public TSqlPermission(TSqlObject obj)  : base(obj, Permission.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPermission to adapt instances of <see cref="T:Permission"/>
		/// </summary>
		protected TSqlPermission()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Permission.PermissionAction"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.permissionaction.aspx">Permission.PermissionAction</see>
		///
		public PermissionAction PermissionAction 
		{
			get { return Element.GetProperty<PermissionAction>(Permission.PermissionAction);}
		}


		///
		/// Property wrapper for <see cref="M:Permission.PermissionType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.permissiontype.aspx">Permission.PermissionType</see>
		///
		public PermissionType PermissionType 
		{
			get { return Element.GetProperty<PermissionType>(Permission.PermissionType);}
		}


		///
		/// Property wrapper for <see cref="M:Permission.WithAllPrivileges"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.withallprivileges.aspx">Permission.WithAllPrivileges</see>
		///
		public Boolean WithAllPrivileges 
		{
			get { return Element.GetProperty<Boolean>(Permission.WithAllPrivileges);}
		}


		///
		/// Property wrapper for <see cref="M:Permission.WithGrantOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.withgrantoption.aspx">Permission.WithGrantOption</see>
		///
		public Boolean WithGrantOption 
		{
			get { return Element.GetProperty<Boolean>(Permission.WithGrantOption);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.excludedcolumns.aspx">Permission.ExcludedColumns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> ExcludedColumns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Permission.ExcludedColumns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.grantee.aspx">Permission.Grantee</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> Grantee 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Permission.Grantee).Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal>();
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.grantor.aspx">Permission.Grantor</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> Grantor 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Permission.Grantor).Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal>();
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.revokedgrantoptioncolumns.aspx">Permission.RevokedGrantOptionColumns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> RevokedGrantOptionColumns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Permission.RevokedGrantOptionColumns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.permission.securedobject.aspx">Permission.SecuredObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> SecuredObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Permission.SecuredObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlSecurableElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PrimaryKeyConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.aspx">PrimaryKeyConstraint</see>
	///
	public partial class TSqlPrimaryKeyConstraintReference : TSqlPrimaryKeyConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPrimaryKeyConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PrimaryKeyConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.aspx">PrimaryKeyConstraint</see>
	///
	public partial class TSqlPrimaryKeyConstraint : TSqlModelElement,IExtendedPropertyHost,ISpecifiesStorage	{
		private static ModelTypeClass typeClass = PrimaryKeyConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPrimaryKeyConstraint to adapt instances of <see cref="T:PrimaryKeyConstraint"/>
		/// </summary>
		public TSqlPrimaryKeyConstraint(TSqlObject obj)  : base(obj, PrimaryKeyConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPrimaryKeyConstraint to adapt instances of <see cref="T:PrimaryKeyConstraint"/>
		/// </summary>
		protected TSqlPrimaryKeyConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.AllowPageLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.allowpagelocks.aspx">PrimaryKeyConstraint.AllowPageLocks</see>
		///
		public Boolean AllowPageLocks 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.AllowPageLocks);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.AllowRowLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.allowrowlocks.aspx">PrimaryKeyConstraint.AllowRowLocks</see>
		///
		public Boolean AllowRowLocks 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.AllowRowLocks);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.BucketCount"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.bucketcount.aspx">PrimaryKeyConstraint.BucketCount</see>
		///
		public Int32? BucketCount 
		{
			get { return Element.GetProperty<Int32?>(PrimaryKeyConstraint.BucketCount);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.Clustered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.clustered.aspx">PrimaryKeyConstraint.Clustered</see>
		///
		public Boolean Clustered 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.Clustered);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.disabled.aspx">PrimaryKeyConstraint.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.FileStreamNull"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.filestreamnull.aspx">PrimaryKeyConstraint.FileStreamNull</see>
		///
		public Boolean? FileStreamNull 
		{
			get { return Element.GetProperty<Boolean?>(PrimaryKeyConstraint.FileStreamNull);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.FillFactor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.fillfactor.aspx">PrimaryKeyConstraint.FillFactor</see>
		///
		public Int32? FillFactor 
		{
			get { return Element.GetProperty<Int32?>(PrimaryKeyConstraint.FillFactor);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.Hash"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.hash.aspx">PrimaryKeyConstraint.Hash</see>
		///
		public Boolean Hash 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.Hash);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.ignoreduplicatekey.aspx">PrimaryKeyConstraint.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.IgnoreDuplicateKey);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.RecomputeStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.recomputestatistics.aspx">PrimaryKeyConstraint.RecomputeStatistics</see>
		///
		public Boolean RecomputeStatistics 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.RecomputeStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:PrimaryKeyConstraint.WithPadIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.withpadindex.aspx">PrimaryKeyConstraint.WithPadIndex</see>
		///
		public Boolean WithPadIndex 
		{
			get { return Element.GetProperty<Boolean>(PrimaryKeyConstraint.WithPadIndex);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.PrimaryKeyConstraintColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.columns.aspx">PrimaryKeyConstraint.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.PrimaryKeyConstraintColumnsReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.PrimaryKeyConstraintColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.datacompressionoptions.aspx">PrimaryKeyConstraint.DataCompressionOptions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption> DataCompressionOptions 
		{
			get 
			{
				return Element.GetReferenced(PrimaryKeyConstraint.DataCompressionOptions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.filegroup.aspx">PrimaryKeyConstraint.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.filestreamfilegroup.aspx">PrimaryKeyConstraint.FileStreamFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> FileStreamFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.FileStreamFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.filestreampartitionscheme.aspx">PrimaryKeyConstraint.FileStreamPartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> FileStreamPartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.FileStreamPartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.host.aspx">PrimaryKeyConstraint.Host</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference> Host 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.Host).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference(o, Microsoft.SqlServer.Dac.Model.Table.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.partitioncolumn.aspx">PrimaryKeyConstraint.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.primarykeyconstraint.partitionscheme.aspx">PrimaryKeyConstraint.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PrimaryKeyConstraint.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Procedure"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.aspx">Procedure</see>
	///
	public partial class TSqlProcedureReference : TSqlProcedure, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlProcedureReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Procedure"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.aspx">Procedure</see>
	///
	public partial class TSqlProcedure : TSqlModelElement	{
		private static ModelTypeClass typeClass = Procedure.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlProcedure to adapt instances of <see cref="T:Procedure"/>
		/// </summary>
		public TSqlProcedure(TSqlObject obj)  : base(obj, Procedure.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlProcedure to adapt instances of <see cref="T:Procedure"/>
		/// </summary>
		protected TSqlProcedure()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Procedure.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.ansinullson.aspx">Procedure.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(Procedure.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.classname.aspx">Procedure.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(Procedure.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.ExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.executeascaller.aspx">Procedure.ExecuteAsCaller</see>
		///
		public Boolean ExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(Procedure.ExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.executeasowner.aspx">Procedure.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(Procedure.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.executeasself.aspx">Procedure.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(Procedure.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.ForReplication"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.forreplication.aspx">Procedure.ForReplication</see>
		///
		public Boolean ForReplication 
		{
			get { return Element.GetProperty<Boolean>(Procedure.ForReplication);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.IsReplicated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.isreplicated.aspx">Procedure.IsReplicated</see>
		///
		public Boolean IsReplicated 
		{
			get { return Element.GetProperty<Boolean>(Procedure.IsReplicated);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.MethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.methodname.aspx">Procedure.MethodName</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(Procedure.MethodName);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.quotedidentifieron.aspx">Procedure.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(Procedure.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.withencryption.aspx">Procedure.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(Procedure.WithEncryption);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.WithNativeCompilation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.withnativecompilation.aspx">Procedure.WithNativeCompilation</see>
		///
		public Boolean WithNativeCompilation 
		{
			get { return Element.GetProperty<Boolean>(Procedure.WithNativeCompilation);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.WithRecompile"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.withrecompile.aspx">Procedure.WithRecompile</see>
		///
		public Boolean WithRecompile 
		{
			get { return Element.GetProperty<Boolean>(Procedure.WithRecompile);}
		}


		///
		/// Property wrapper for <see cref="M:Procedure.WithSchemaBinding"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.withschemabinding.aspx">Procedure.WithSchemaBinding</see>
		///
		public Boolean WithSchemaBinding 
		{
			get { return Element.GetProperty<Boolean>(Procedure.WithSchemaBinding);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.assembly.aspx">Procedure.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Procedure.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.bodydependencies.aspx">Procedure.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Procedure.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.login.aspx">Procedure.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Procedure.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.parameters.aspx">Procedure.Parameters</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter> Parameters 
		{
			get 
			{
				return Element.GetReferenced(Procedure.Parameters).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlParameter(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlProcedureReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.parentprocedure.aspx">Procedure.ParentProcedure</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlProcedureReference> ParentProcedure 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Procedure.ParentProcedure).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlProcedureReference(o, Microsoft.SqlServer.Dac.Model.Procedure.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.schema.aspx">Procedure.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Procedure.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.procedure.user.aspx">Procedure.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Procedure.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Queue"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.aspx">Queue</see>
	///
	public partial class TSqlQueueReference : TSqlQueue, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlQueueReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Queue"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.aspx">Queue</see>
	///
	public partial class TSqlQueue : TSqlModelElement,ISqlColumnSource	{
		private static ModelTypeClass typeClass = Queue.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlQueue to adapt instances of <see cref="T:Queue"/>
		/// </summary>
		public TSqlQueue(TSqlObject obj)  : base(obj, Queue.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlQueue to adapt instances of <see cref="T:Queue"/>
		/// </summary>
		protected TSqlQueue()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Queue.ActivationExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.activationexecuteascaller.aspx">Queue.ActivationExecuteAsCaller</see>
		///
		public Boolean ActivationExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(Queue.ActivationExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.ActivationExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.activationexecuteasowner.aspx">Queue.ActivationExecuteAsOwner</see>
		///
		public Boolean ActivationExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(Queue.ActivationExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.ActivationExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.activationexecuteasself.aspx">Queue.ActivationExecuteAsSelf</see>
		///
		public Boolean ActivationExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(Queue.ActivationExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.ActivationMaxQueueReaders"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.activationmaxqueuereaders.aspx">Queue.ActivationMaxQueueReaders</see>
		///
		public Int32? ActivationMaxQueueReaders 
		{
			get { return Element.GetProperty<Int32?>(Queue.ActivationMaxQueueReaders);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.ActivationStatusOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.activationstatuson.aspx">Queue.ActivationStatusOn</see>
		///
		public Boolean? ActivationStatusOn 
		{
			get { return Element.GetProperty<Boolean?>(Queue.ActivationStatusOn);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.PoisonMessageHandlingStatusOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.poisonmessagehandlingstatuson.aspx">Queue.PoisonMessageHandlingStatusOn</see>
		///
		public Boolean PoisonMessageHandlingStatusOn 
		{
			get { return Element.GetProperty<Boolean>(Queue.PoisonMessageHandlingStatusOn);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.RetentionOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.retentionon.aspx">Queue.RetentionOn</see>
		///
		public Boolean RetentionOn 
		{
			get { return Element.GetProperty<Boolean>(Queue.RetentionOn);}
		}


		///
		/// Property wrapper for <see cref="M:Queue.StatusOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.statuson.aspx">Queue.StatusOn</see>
		///
		public Boolean StatusOn 
		{
			get { return Element.GetProperty<Boolean>(Queue.StatusOn);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlProcedureReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.activationprocedure.aspx">Queue.ActivationProcedure</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlProcedureReference> ActivationProcedure 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.ActivationProcedure).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlProcedureReference(o, Microsoft.SqlServer.Dac.Model.Procedure.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.columns.aspx">Queue.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn> Columns 
		{
			get 
			{
				return Element.GetReferenced(Queue.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.filegroup.aspx">Queue.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.login.aspx">Queue.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.partitioncolumn.aspx">Queue.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.partitionscheme.aspx">Queue.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.schema.aspx">Queue.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queue.user.aspx">Queue.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Queue.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:QueueEventNotification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.aspx">QueueEventNotification</see>
	///
	public partial class TSqlQueueEventNotificationReference : TSqlQueueEventNotification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlQueueEventNotificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:QueueEventNotification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.aspx">QueueEventNotification</see>
	///
	public partial class TSqlQueueEventNotification : TSqlModelElement	{
		private static ModelTypeClass typeClass = QueueEventNotification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlQueueEventNotification to adapt instances of <see cref="T:QueueEventNotification"/>
		/// </summary>
		public TSqlQueueEventNotification(TSqlObject obj)  : base(obj, QueueEventNotification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlQueueEventNotification to adapt instances of <see cref="T:QueueEventNotification"/>
		/// </summary>
		protected TSqlQueueEventNotification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:QueueEventNotification.BrokerInstanceSpecifier"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.brokerinstancespecifier.aspx">QueueEventNotification.BrokerInstanceSpecifier</see>
		///
		public String BrokerInstanceSpecifier 
		{
			get { return Element.GetProperty<String>(QueueEventNotification.BrokerInstanceSpecifier);}
		}


		///
		/// Property wrapper for <see cref="M:QueueEventNotification.BrokerService"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.brokerservice.aspx">QueueEventNotification.BrokerService</see>
		///
		public String BrokerService 
		{
			get { return Element.GetProperty<String>(QueueEventNotification.BrokerService);}
		}


		///
		/// Property wrapper for <see cref="M:QueueEventNotification.WithFanIn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.withfanin.aspx">QueueEventNotification.WithFanIn</see>
		///
		public Boolean WithFanIn 
		{
			get { return Element.GetProperty<Boolean>(QueueEventNotification.WithFanIn);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.eventgroup.aspx">QueueEventNotification.EventGroup</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup> EventGroup 
		{
			get 
			{
				return Element.GetReferenced(QueueEventNotification.EventGroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Model.EventType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.eventtype.aspx">QueueEventNotification.EventType</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> EventType 
		{
			get 
			{
				return Element.GetReferenced(QueueEventNotification.EventType).Cast<Microsoft.SqlServer.Dac.Model.EventType>();
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlQueueReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.queueeventnotification.queue.aspx">QueueEventNotification.Queue</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlQueueReference> Queue 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(QueueEventNotification.Queue).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlQueueReference(o, Microsoft.SqlServer.Dac.Model.Queue.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:RemoteServiceBinding"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.remoteservicebinding.aspx">RemoteServiceBinding</see>
	///
	public partial class TSqlRemoteServiceBindingReference : TSqlRemoteServiceBinding, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlRemoteServiceBindingReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:RemoteServiceBinding"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.remoteservicebinding.aspx">RemoteServiceBinding</see>
	///
	public partial class TSqlRemoteServiceBinding : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = RemoteServiceBinding.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlRemoteServiceBinding to adapt instances of <see cref="T:RemoteServiceBinding"/>
		/// </summary>
		public TSqlRemoteServiceBinding(TSqlObject obj)  : base(obj, RemoteServiceBinding.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlRemoteServiceBinding to adapt instances of <see cref="T:RemoteServiceBinding"/>
		/// </summary>
		protected TSqlRemoteServiceBinding()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:RemoteServiceBinding.Anonymous"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.remoteservicebinding.anonymous.aspx">RemoteServiceBinding.Anonymous</see>
		///
		public Boolean Anonymous 
		{
			get { return Element.GetProperty<Boolean>(RemoteServiceBinding.Anonymous);}
		}


		///
		/// Property wrapper for <see cref="M:RemoteServiceBinding.Service"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.remoteservicebinding.service.aspx">RemoteServiceBinding.Service</see>
		///
		public String Service 
		{
			get { return Element.GetProperty<String>(RemoteServiceBinding.Service);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.remoteservicebinding.authorizer.aspx">RemoteServiceBinding.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(RemoteServiceBinding.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.remoteservicebinding.user.aspx">RemoteServiceBinding.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(RemoteServiceBinding.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ResourceGovernor"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcegovernor.aspx">ResourceGovernor</see>
	///
	public partial class TSqlResourceGovernorReference : TSqlResourceGovernor, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlResourceGovernorReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ResourceGovernor"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcegovernor.aspx">ResourceGovernor</see>
	///
	public partial class TSqlResourceGovernor : TSqlModelElement	{
		private static ModelTypeClass typeClass = ResourceGovernor.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlResourceGovernor to adapt instances of <see cref="T:ResourceGovernor"/>
		/// </summary>
		public TSqlResourceGovernor(TSqlObject obj)  : base(obj, ResourceGovernor.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlResourceGovernor to adapt instances of <see cref="T:ResourceGovernor"/>
		/// </summary>
		protected TSqlResourceGovernor()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ResourceGovernor.Enabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcegovernor.enabled.aspx">ResourceGovernor.Enabled</see>
		///
		public Boolean? Enabled 
		{
			get { return Element.GetProperty<Boolean?>(ResourceGovernor.Enabled);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcegovernor.classifierfunction.aspx">ResourceGovernor.ClassifierFunction</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ClassifierFunction 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ResourceGovernor.ClassifierFunction).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ResourcePool"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.aspx">ResourcePool</see>
	///
	public partial class TSqlResourcePoolReference : TSqlResourcePool, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlResourcePoolReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ResourcePool"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.aspx">ResourcePool</see>
	///
	public partial class TSqlResourcePool : TSqlModelElement	{
		private static ModelTypeClass typeClass = ResourcePool.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlResourcePool to adapt instances of <see cref="T:ResourcePool"/>
		/// </summary>
		public TSqlResourcePool(TSqlObject obj)  : base(obj, ResourcePool.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlResourcePool to adapt instances of <see cref="T:ResourcePool"/>
		/// </summary>
		protected TSqlResourcePool()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ResourcePool.CapCpuPercent"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.capcpupercent.aspx">ResourcePool.CapCpuPercent</see>
		///
		public Int32 CapCpuPercent 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.CapCpuPercent);}
		}


		///
		/// Property wrapper for <see cref="M:ResourcePool.MaxCpuPercent"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.maxcpupercent.aspx">ResourcePool.MaxCpuPercent</see>
		///
		public Int32 MaxCpuPercent 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.MaxCpuPercent);}
		}


		///
		/// Property wrapper for <see cref="M:ResourcePool.MaxIopsPerVolume"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.maxiopspervolume.aspx">ResourcePool.MaxIopsPerVolume</see>
		///
		public Int32 MaxIopsPerVolume 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.MaxIopsPerVolume);}
		}


		///
		/// Property wrapper for <see cref="M:ResourcePool.MaxMemoryPercent"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.maxmemorypercent.aspx">ResourcePool.MaxMemoryPercent</see>
		///
		public Int32 MaxMemoryPercent 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.MaxMemoryPercent);}
		}


		///
		/// Property wrapper for <see cref="M:ResourcePool.MinCpuPercent"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.mincpupercent.aspx">ResourcePool.MinCpuPercent</see>
		///
		public Int32 MinCpuPercent 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.MinCpuPercent);}
		}


		///
		/// Property wrapper for <see cref="M:ResourcePool.MinIopsPerVolume"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.miniopspervolume.aspx">ResourcePool.MinIopsPerVolume</see>
		///
		public Int32 MinIopsPerVolume 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.MinIopsPerVolume);}
		}


		///
		/// Property wrapper for <see cref="M:ResourcePool.MinMemoryPercent"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.resourcepool.minmemorypercent.aspx">ResourcePool.MinMemoryPercent</see>
		///
		public Int32 MinMemoryPercent 
		{
			get { return Element.GetProperty<Int32>(ResourcePool.MinMemoryPercent);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Role"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.role.aspx">Role</see>
	///
	public partial class TSqlRoleReference : TSqlRole, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlRoleReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Role"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.role.aspx">Role</see>
	///
	public partial class TSqlRole : TSqlModelElement,ISqlDatabaseSecurityPrincipal	{
		private static ModelTypeClass typeClass = Role.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlRole to adapt instances of <see cref="T:Role"/>
		/// </summary>
		public TSqlRole(TSqlObject obj)  : base(obj, Role.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlRole to adapt instances of <see cref="T:Role"/>
		/// </summary>
		protected TSqlRole()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.role.authorizer.aspx">Role.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Role.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:RoleMembership"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rolemembership.aspx">RoleMembership</see>
	///
	public partial class TSqlRoleMembershipReference : TSqlRoleMembership, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlRoleMembershipReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:RoleMembership"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rolemembership.aspx">RoleMembership</see>
	///
	public partial class TSqlRoleMembership : TSqlModelElement	{
		private static ModelTypeClass typeClass = RoleMembership.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlRoleMembership to adapt instances of <see cref="T:RoleMembership"/>
		/// </summary>
		public TSqlRoleMembership(TSqlObject obj)  : base(obj, RoleMembership.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlRoleMembership to adapt instances of <see cref="T:RoleMembership"/>
		/// </summary>
		protected TSqlRoleMembership()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rolemembership.member.aspx">RoleMembership.Member</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> Member 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(RoleMembership.Member).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlDatabaseSecurityPrincipalElement(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlRoleReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rolemembership.role.aspx">RoleMembership.Role</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlRoleReference> Role 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(RoleMembership.Role).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlRoleReference(o, Microsoft.SqlServer.Dac.Model.Role.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Route"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.aspx">Route</see>
	///
	public partial class TSqlRouteReference : TSqlRoute, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlRouteReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Route"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.aspx">Route</see>
	///
	public partial class TSqlRoute : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Route.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlRoute to adapt instances of <see cref="T:Route"/>
		/// </summary>
		public TSqlRoute(TSqlObject obj)  : base(obj, Route.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlRoute to adapt instances of <see cref="T:Route"/>
		/// </summary>
		protected TSqlRoute()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Route.Address"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.address.aspx">Route.Address</see>
		///
		public String Address 
		{
			get { return Element.GetProperty<String>(Route.Address);}
		}


		///
		/// Property wrapper for <see cref="M:Route.BrokerInstance"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.brokerinstance.aspx">Route.BrokerInstance</see>
		///
		public String BrokerInstance 
		{
			get { return Element.GetProperty<String>(Route.BrokerInstance);}
		}


		///
		/// Property wrapper for <see cref="M:Route.Lifetime"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.lifetime.aspx">Route.Lifetime</see>
		///
		public Int32? Lifetime 
		{
			get { return Element.GetProperty<Int32?>(Route.Lifetime);}
		}


		///
		/// Property wrapper for <see cref="M:Route.MirrorAddress"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.mirroraddress.aspx">Route.MirrorAddress</see>
		///
		public String MirrorAddress 
		{
			get { return Element.GetProperty<String>(Route.MirrorAddress);}
		}


		///
		/// Property wrapper for <see cref="M:Route.ServiceName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.servicename.aspx">Route.ServiceName</see>
		///
		public String ServiceName 
		{
			get { return Element.GetProperty<String>(Route.ServiceName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.route.authorizer.aspx">Route.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Route.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Rule"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rule.aspx">Rule</see>
	///
	public partial class TSqlRuleReference : TSqlRule, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlRuleReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Rule"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rule.aspx">Rule</see>
	///
	public partial class TSqlRule : TSqlModelElement	{
		private static ModelTypeClass typeClass = Rule.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlRule to adapt instances of <see cref="T:Rule"/>
		/// </summary>
		public TSqlRule(TSqlObject obj)  : base(obj, Rule.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlRule to adapt instances of <see cref="T:Rule"/>
		/// </summary>
		protected TSqlRule()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Rule.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rule.expression.aspx">Rule.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(Rule.Expression);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rule.boundobjects.aspx">Rule.BoundObjects</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BoundObjects 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Rule.BoundObjects).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.rule.schema.aspx">Rule.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Rule.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Schema"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.schema.aspx">Schema</see>
	///
	public partial class TSqlSchemaReference : TSqlSchema, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSchemaReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Schema"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.schema.aspx">Schema</see>
	///
	public partial class TSqlSchema : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Schema.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSchema to adapt instances of <see cref="T:Schema"/>
		/// </summary>
		public TSqlSchema(TSqlObject obj)  : base(obj, Schema.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSchema to adapt instances of <see cref="T:Schema"/>
		/// </summary>
		protected TSqlSchema()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.schema.authorizer.aspx">Schema.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Schema.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SearchProperty"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchproperty.aspx">SearchProperty</see>
	///
	public partial class TSqlSearchPropertyReference : TSqlSearchProperty, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSearchPropertyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SearchProperty"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchproperty.aspx">SearchProperty</see>
	///
	public partial class TSqlSearchProperty : TSqlModelElement	{
		private static ModelTypeClass typeClass = SearchProperty.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSearchProperty to adapt instances of <see cref="T:SearchProperty"/>
		/// </summary>
		public TSqlSearchProperty(TSqlObject obj)  : base(obj, SearchProperty.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSearchProperty to adapt instances of <see cref="T:SearchProperty"/>
		/// </summary>
		protected TSqlSearchProperty()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SearchProperty.Description"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchproperty.description.aspx">SearchProperty.Description</see>
		///
		public String Description 
		{
			get { return Element.GetProperty<String>(SearchProperty.Description);}
		}


		///
		/// Property wrapper for <see cref="M:SearchProperty.Identifier"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchproperty.identifier.aspx">SearchProperty.Identifier</see>
		///
		public Int32 Identifier 
		{
			get { return Element.GetProperty<Int32>(SearchProperty.Identifier);}
		}


		///
		/// Property wrapper for <see cref="M:SearchProperty.PropertySetGuid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchproperty.propertysetguid.aspx">SearchProperty.PropertySetGuid</see>
		///
		public String PropertySetGuid 
		{
			get { return Element.GetProperty<String>(SearchProperty.PropertySetGuid);}
		}


		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSearchPropertyListReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchproperty.searchpropertylist.aspx">SearchProperty.SearchPropertyList</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSearchPropertyListReference> SearchPropertyList 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SearchProperty.SearchPropertyList).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSearchPropertyListReference(o, Microsoft.SqlServer.Dac.Model.SearchPropertyList.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SearchPropertyList"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchpropertylist.aspx">SearchPropertyList</see>
	///
	public partial class TSqlSearchPropertyListReference : TSqlSearchPropertyList, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSearchPropertyListReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SearchPropertyList"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchpropertylist.aspx">SearchPropertyList</see>
	///
	public partial class TSqlSearchPropertyList : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = SearchPropertyList.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSearchPropertyList to adapt instances of <see cref="T:SearchPropertyList"/>
		/// </summary>
		public TSqlSearchPropertyList(TSqlObject obj)  : base(obj, SearchPropertyList.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSearchPropertyList to adapt instances of <see cref="T:SearchPropertyList"/>
		/// </summary>
		protected TSqlSearchPropertyList()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.searchpropertylist.authorizer.aspx">SearchPropertyList.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SearchPropertyList.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Sequence"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.aspx">Sequence</see>
	///
	public partial class TSqlSequenceReference : TSqlSequence, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSequenceReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Sequence"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.aspx">Sequence</see>
	///
	public partial class TSqlSequence : TSqlModelElement	{
		private static ModelTypeClass typeClass = Sequence.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSequence to adapt instances of <see cref="T:Sequence"/>
		/// </summary>
		public TSqlSequence(TSqlObject obj)  : base(obj, Sequence.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSequence to adapt instances of <see cref="T:Sequence"/>
		/// </summary>
		protected TSqlSequence()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Sequence.CacheSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.cachesize.aspx">Sequence.CacheSize</see>
		///
		public Int32? CacheSize 
		{
			get { return Element.GetProperty<Int32?>(Sequence.CacheSize);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.IncrementValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.incrementvalue.aspx">Sequence.IncrementValue</see>
		///
		public String IncrementValue 
		{
			get { return Element.GetProperty<String>(Sequence.IncrementValue);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.IsCached"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.iscached.aspx">Sequence.IsCached</see>
		///
		public Boolean IsCached 
		{
			get { return Element.GetProperty<Boolean>(Sequence.IsCached);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.IsCycling"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.iscycling.aspx">Sequence.IsCycling</see>
		///
		public Boolean IsCycling 
		{
			get { return Element.GetProperty<Boolean>(Sequence.IsCycling);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.MaxValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.maxvalue.aspx">Sequence.MaxValue</see>
		///
		public String MaxValue 
		{
			get { return Element.GetProperty<String>(Sequence.MaxValue);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.MinValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.minvalue.aspx">Sequence.MinValue</see>
		///
		public String MinValue 
		{
			get { return Element.GetProperty<String>(Sequence.MinValue);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.NoMaxValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.nomaxvalue.aspx">Sequence.NoMaxValue</see>
		///
		public Boolean NoMaxValue 
		{
			get { return Element.GetProperty<Boolean>(Sequence.NoMaxValue);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.NoMinValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.nominvalue.aspx">Sequence.NoMinValue</see>
		///
		public Boolean NoMinValue 
		{
			get { return Element.GetProperty<Boolean>(Sequence.NoMinValue);}
		}


		///
		/// Property wrapper for <see cref="M:Sequence.StartValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.startvalue.aspx">Sequence.StartValue</see>
		///
		public String StartValue 
		{
			get { return Element.GetProperty<String>(Sequence.StartValue);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.datatype.aspx">Sequence.DataType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> DataType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Sequence.DataType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.sequence.schema.aspx">Sequence.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Sequence.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerAudit"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.aspx">ServerAudit</see>
	///
	public partial class TSqlServerAuditReference : TSqlServerAudit, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServerAuditReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerAudit"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.aspx">ServerAudit</see>
	///
	public partial class TSqlServerAudit : TSqlModelElement	{
		private static ModelTypeClass typeClass = ServerAudit.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServerAudit to adapt instances of <see cref="T:ServerAudit"/>
		/// </summary>
		public TSqlServerAudit(TSqlObject obj)  : base(obj, ServerAudit.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServerAudit to adapt instances of <see cref="T:ServerAudit"/>
		/// </summary>
		protected TSqlServerAudit()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ServerAudit.AuditGuid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.auditguid.aspx">ServerAudit.AuditGuid</see>
		///
		public String AuditGuid 
		{
			get { return Element.GetProperty<String>(ServerAudit.AuditGuid);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.AuditTarget"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.audittarget.aspx">ServerAudit.AuditTarget</see>
		///
		public AuditTarget AuditTarget 
		{
			get { return Element.GetProperty<AuditTarget>(ServerAudit.AuditTarget);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.disabled.aspx">ServerAudit.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(ServerAudit.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.FilePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.filepath.aspx">ServerAudit.FilePath</see>
		///
		public String FilePath 
		{
			get { return Element.GetProperty<String>(ServerAudit.FilePath);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.MaxFiles"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.maxfiles.aspx">ServerAudit.MaxFiles</see>
		///
		public Int32? MaxFiles 
		{
			get { return Element.GetProperty<Int32?>(ServerAudit.MaxFiles);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.MaxRolloverFiles"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.maxrolloverfiles.aspx">ServerAudit.MaxRolloverFiles</see>
		///
		public Int32? MaxRolloverFiles 
		{
			get { return Element.GetProperty<Int32?>(ServerAudit.MaxRolloverFiles);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.MaxSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.maxsize.aspx">ServerAudit.MaxSize</see>
		///
		public Int32? MaxSize 
		{
			get { return Element.GetProperty<Int32?>(ServerAudit.MaxSize);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.MaxSizeUnit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.maxsizeunit.aspx">ServerAudit.MaxSizeUnit</see>
		///
		public MemoryUnit MaxSizeUnit 
		{
			get { return Element.GetProperty<MemoryUnit>(ServerAudit.MaxSizeUnit);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.OnFailure"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.onfailure.aspx">ServerAudit.OnFailure</see>
		///
		public FailureAction OnFailure 
		{
			get { return Element.GetProperty<FailureAction>(ServerAudit.OnFailure);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.PredicateExpression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.predicateexpression.aspx">ServerAudit.PredicateExpression</see>
		///
		public String PredicateExpression 
		{
			get { return (String)Element.GetProperty(ServerAudit.PredicateExpression);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.QueueDelay"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.queuedelay.aspx">ServerAudit.QueueDelay</see>
		///
		public Int32 QueueDelay 
		{
			get { return Element.GetProperty<Int32>(ServerAudit.QueueDelay);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.ReserveDiskSpace"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.reservediskspace.aspx">ServerAudit.ReserveDiskSpace</see>
		///
		public Boolean ReserveDiskSpace 
		{
			get { return Element.GetProperty<Boolean>(ServerAudit.ReserveDiskSpace);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.UnlimitedFileSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.unlimitedfilesize.aspx">ServerAudit.UnlimitedFileSize</see>
		///
		public Boolean UnlimitedFileSize 
		{
			get { return Element.GetProperty<Boolean>(ServerAudit.UnlimitedFileSize);}
		}


		///
		/// Property wrapper for <see cref="M:ServerAudit.UnlimitedMaxRolloverFiles"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveraudit.unlimitedmaxrolloverfiles.aspx">ServerAudit.UnlimitedMaxRolloverFiles</see>
		///
		public Boolean UnlimitedMaxRolloverFiles 
		{
			get { return Element.GetProperty<Boolean>(ServerAudit.UnlimitedMaxRolloverFiles);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerAuditSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverauditspecification.aspx">ServerAuditSpecification</see>
	///
	public partial class TSqlServerAuditSpecificationReference : TSqlServerAuditSpecification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServerAuditSpecificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerAuditSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverauditspecification.aspx">ServerAuditSpecification</see>
	///
	public partial class TSqlServerAuditSpecification : TSqlModelElement	{
		private static ModelTypeClass typeClass = ServerAuditSpecification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServerAuditSpecification to adapt instances of <see cref="T:ServerAuditSpecification"/>
		/// </summary>
		public TSqlServerAuditSpecification(TSqlObject obj)  : base(obj, ServerAuditSpecification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServerAuditSpecification to adapt instances of <see cref="T:ServerAuditSpecification"/>
		/// </summary>
		protected TSqlServerAuditSpecification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ServerAuditSpecification.StateOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverauditspecification.stateon.aspx">ServerAuditSpecification.StateOn</see>
		///
		public Boolean StateOn 
		{
			get { return Element.GetProperty<Boolean>(ServerAuditSpecification.StateOn);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditActionGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverauditspecification.auditactiongroups.aspx">ServerAuditSpecification.AuditActionGroups</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditActionGroup> AuditActionGroups 
		{
			get 
			{
				return Element.GetReferenced(ServerAuditSpecification.AuditActionGroups).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAuditActionGroup(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlServerAuditReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverauditspecification.serveraudit.aspx">ServerAuditSpecification.ServerAudit</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlServerAuditReference> ServerAudit 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerAuditSpecification.ServerAudit).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlServerAuditReference(o, Microsoft.SqlServer.Dac.Model.ServerAudit.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerDdlTrigger"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.aspx">ServerDdlTrigger</see>
	///
	public partial class TSqlServerDdlTriggerReference : TSqlServerDdlTrigger, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServerDdlTriggerReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerDdlTrigger"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.aspx">ServerDdlTrigger</see>
	///
	public partial class TSqlServerDdlTrigger : TSqlModelElement	{
		private static ModelTypeClass typeClass = ServerDdlTrigger.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServerDdlTrigger to adapt instances of <see cref="T:ServerDdlTrigger"/>
		/// </summary>
		public TSqlServerDdlTrigger(TSqlObject obj)  : base(obj, ServerDdlTrigger.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServerDdlTrigger to adapt instances of <see cref="T:ServerDdlTrigger"/>
		/// </summary>
		protected TSqlServerDdlTrigger()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.ansinullson.aspx">ServerDdlTrigger.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(ServerDdlTrigger.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.classname.aspx">ServerDdlTrigger.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(ServerDdlTrigger.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.disabled.aspx">ServerDdlTrigger.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(ServerDdlTrigger.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.ExecuteAsCaller"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.executeascaller.aspx">ServerDdlTrigger.ExecuteAsCaller</see>
		///
		public Boolean ExecuteAsCaller 
		{
			get { return Element.GetProperty<Boolean>(ServerDdlTrigger.ExecuteAsCaller);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.ExecuteAsOwner"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.executeasowner.aspx">ServerDdlTrigger.ExecuteAsOwner</see>
		///
		public Boolean ExecuteAsOwner 
		{
			get { return Element.GetProperty<Boolean>(ServerDdlTrigger.ExecuteAsOwner);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.ExecuteAsSelf"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.executeasself.aspx">ServerDdlTrigger.ExecuteAsSelf</see>
		///
		public Boolean ExecuteAsSelf 
		{
			get { return Element.GetProperty<Boolean>(ServerDdlTrigger.ExecuteAsSelf);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.IsLogon"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.islogon.aspx">ServerDdlTrigger.IsLogon</see>
		///
		public Boolean IsLogon 
		{
			get { return Element.GetProperty<Boolean>(ServerDdlTrigger.IsLogon);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.MethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.methodname.aspx">ServerDdlTrigger.MethodName</see>
		///
		public String MethodName 
		{
			get { return Element.GetProperty<String>(ServerDdlTrigger.MethodName);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.quotedidentifieron.aspx">ServerDdlTrigger.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(ServerDdlTrigger.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.TriggerType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.triggertype.aspx">ServerDdlTrigger.TriggerType</see>
		///
		public TriggerType TriggerType 
		{
			get { return Element.GetProperty<TriggerType>(ServerDdlTrigger.TriggerType);}
		}


		///
		/// Property wrapper for <see cref="M:ServerDdlTrigger.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.withencryption.aspx">ServerDdlTrigger.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(ServerDdlTrigger.WithEncryption);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.assembly.aspx">ServerDdlTrigger.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerDdlTrigger.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.bodydependencies.aspx">ServerDdlTrigger.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerDdlTrigger.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.eventgroup.aspx">ServerDdlTrigger.EventGroup</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup> EventGroup 
		{
			get 
			{
				return Element.GetReferenced(ServerDdlTrigger.EventGroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Model.EventType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.eventtype.aspx">ServerDdlTrigger.EventType</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> EventType 
		{
			get 
			{
				return Element.GetReferenced(ServerDdlTrigger.EventType).Cast<Microsoft.SqlServer.Dac.Model.EventType>();
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.login.aspx">ServerDdlTrigger.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerDdlTrigger.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverddltrigger.user.aspx">ServerDdlTrigger.User</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference> User 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerDdlTrigger.User).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlUserReference(o, Microsoft.SqlServer.Dac.Model.User.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerEventNotification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.aspx">ServerEventNotification</see>
	///
	public partial class TSqlServerEventNotificationReference : TSqlServerEventNotification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServerEventNotificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerEventNotification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.aspx">ServerEventNotification</see>
	///
	public partial class TSqlServerEventNotification : TSqlModelElement	{
		private static ModelTypeClass typeClass = ServerEventNotification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServerEventNotification to adapt instances of <see cref="T:ServerEventNotification"/>
		/// </summary>
		public TSqlServerEventNotification(TSqlObject obj)  : base(obj, ServerEventNotification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServerEventNotification to adapt instances of <see cref="T:ServerEventNotification"/>
		/// </summary>
		protected TSqlServerEventNotification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ServerEventNotification.BrokerInstanceSpecifier"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.brokerinstancespecifier.aspx">ServerEventNotification.BrokerInstanceSpecifier</see>
		///
		public String BrokerInstanceSpecifier 
		{
			get { return Element.GetProperty<String>(ServerEventNotification.BrokerInstanceSpecifier);}
		}


		///
		/// Property wrapper for <see cref="M:ServerEventNotification.BrokerService"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.brokerservice.aspx">ServerEventNotification.BrokerService</see>
		///
		public String BrokerService 
		{
			get { return Element.GetProperty<String>(ServerEventNotification.BrokerService);}
		}


		///
		/// Property wrapper for <see cref="M:ServerEventNotification.WithFanIn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.withfanin.aspx">ServerEventNotification.WithFanIn</see>
		///
		public Boolean WithFanIn 
		{
			get { return Element.GetProperty<Boolean>(ServerEventNotification.WithFanIn);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.eventgroup.aspx">ServerEventNotification.EventGroup</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup> EventGroup 
		{
			get 
			{
				return Element.GetReferenced(ServerEventNotification.EventGroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlEventGroup(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Model.EventType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servereventnotification.eventtype.aspx">ServerEventNotification.EventType</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> EventType 
		{
			get 
			{
				return Element.GetReferenced(ServerEventNotification.EventType).Cast<Microsoft.SqlServer.Dac.Model.EventType>();
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerOptions"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveroptions.aspx">ServerOptions</see>
	///
	public partial class TSqlServerOptionsReference : TSqlServerOptions, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServerOptionsReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerOptions"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serveroptions.aspx">ServerOptions</see>
	///
	public partial class TSqlServerOptions : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = ServerOptions.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServerOptions to adapt instances of <see cref="T:ServerOptions"/>
		/// </summary>
		public TSqlServerOptions(TSqlObject obj)  : base(obj, ServerOptions.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServerOptions to adapt instances of <see cref="T:ServerOptions"/>
		/// </summary>
		protected TSqlServerOptions()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerRoleMembership"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverrolemembership.aspx">ServerRoleMembership</see>
	///
	public partial class TSqlServerRoleMembershipReference : TSqlServerRoleMembership, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServerRoleMembershipReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServerRoleMembership"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverrolemembership.aspx">ServerRoleMembership</see>
	///
	public partial class TSqlServerRoleMembership : TSqlModelElement	{
		private static ModelTypeClass typeClass = ServerRoleMembership.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServerRoleMembership to adapt instances of <see cref="T:ServerRoleMembership"/>
		/// </summary>
		public TSqlServerRoleMembership(TSqlObject obj)  : base(obj, ServerRoleMembership.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServerRoleMembership to adapt instances of <see cref="T:ServerRoleMembership"/>
		/// </summary>
		protected TSqlServerRoleMembership()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverrolemembership.member.aspx">ServerRoleMembership.Member</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> Member 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerRoleMembership.Member).Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal>();
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlRoleReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.serverrolemembership.role.aspx">ServerRoleMembership.Role</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlRoleReference> Role 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServerRoleMembership.Role).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlRoleReference(o, Microsoft.SqlServer.Dac.Model.Role.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Service"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.service.aspx">Service</see>
	///
	public partial class TSqlServiceReference : TSqlService, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServiceReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Service"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.service.aspx">Service</see>
	///
	public partial class TSqlService : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = Service.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlService to adapt instances of <see cref="T:Service"/>
		/// </summary>
		public TSqlService(TSqlObject obj)  : base(obj, Service.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlService to adapt instances of <see cref="T:Service"/>
		/// </summary>
		protected TSqlService()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Service.UseDefaultContract"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.service.usedefaultcontract.aspx">Service.UseDefaultContract</see>
		///
		public Boolean UseDefaultContract 
		{
			get { return Element.GetProperty<Boolean>(Service.UseDefaultContract);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.service.authorizer.aspx">Service.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Service.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlContractReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.service.contracts.aspx">Service.Contracts</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlContractReference> Contracts 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Service.Contracts).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlContractReference(o, Microsoft.SqlServer.Dac.Model.Contract.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlQueueReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.service.queue.aspx">Service.Queue</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlQueueReference> Queue 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Service.Queue).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlQueueReference(o, Microsoft.SqlServer.Dac.Model.Queue.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServiceBrokerLanguageSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.aspx">ServiceBrokerLanguageSpecifier</see>
	///
	public partial class TSqlServiceBrokerLanguageSpecifierReference : TSqlServiceBrokerLanguageSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlServiceBrokerLanguageSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:ServiceBrokerLanguageSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.aspx">ServiceBrokerLanguageSpecifier</see>
	///
	public partial class TSqlServiceBrokerLanguageSpecifier : TSqlModelElement,IEndpointLanguageSpecifier	{
		private static ModelTypeClass typeClass = ServiceBrokerLanguageSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlServiceBrokerLanguageSpecifier to adapt instances of <see cref="T:ServiceBrokerLanguageSpecifier"/>
		/// </summary>
		public TSqlServiceBrokerLanguageSpecifier(TSqlObject obj)  : base(obj, ServiceBrokerLanguageSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlServiceBrokerLanguageSpecifier to adapt instances of <see cref="T:ServiceBrokerLanguageSpecifier"/>
		/// </summary>
		protected TSqlServiceBrokerLanguageSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.encryptionalgorithmpart1.aspx">ServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1</see>
		///
		public ServiceBrokerEncryptionAlgorithm EncryptionAlgorithmPart1 
		{
			get { return Element.GetProperty<ServiceBrokerEncryptionAlgorithm>(ServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1);}
		}


		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.encryptionalgorithmpart2.aspx">ServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2</see>
		///
		public ServiceBrokerEncryptionAlgorithm EncryptionAlgorithmPart2 
		{
			get { return Element.GetProperty<ServiceBrokerEncryptionAlgorithm>(ServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2);}
		}


		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.EncryptionMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.encryptionmode.aspx">ServiceBrokerLanguageSpecifier.EncryptionMode</see>
		///
		public EncryptionMode EncryptionMode 
		{
			get { return Element.GetProperty<EncryptionMode>(ServiceBrokerLanguageSpecifier.EncryptionMode);}
		}


		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.MessageForwardingEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.messageforwardingenabled.aspx">ServiceBrokerLanguageSpecifier.MessageForwardingEnabled</see>
		///
		public Boolean MessageForwardingEnabled 
		{
			get { return Element.GetProperty<Boolean>(ServiceBrokerLanguageSpecifier.MessageForwardingEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.MessageForwardSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.messageforwardsize.aspx">ServiceBrokerLanguageSpecifier.MessageForwardSize</see>
		///
		public Int32 MessageForwardSize 
		{
			get { return Element.GetProperty<Int32>(ServiceBrokerLanguageSpecifier.MessageForwardSize);}
		}


		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.UseCertificateFirst"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.usecertificatefirst.aspx">ServiceBrokerLanguageSpecifier.UseCertificateFirst</see>
		///
		public Boolean UseCertificateFirst 
		{
			get { return Element.GetProperty<Boolean>(ServiceBrokerLanguageSpecifier.UseCertificateFirst);}
		}


		///
		/// Property wrapper for <see cref="M:ServiceBrokerLanguageSpecifier.WindowsAuthenticationMode"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.windowsauthenticationmode.aspx">ServiceBrokerLanguageSpecifier.WindowsAuthenticationMode</see>
		///
		public AuthenticationModes WindowsAuthenticationMode 
		{
			get { return Element.GetProperty<AuthenticationModes>(ServiceBrokerLanguageSpecifier.WindowsAuthenticationMode);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.servicebrokerlanguagespecifier.authenticationcertificate.aspx">ServiceBrokerLanguageSpecifier.AuthenticationCertificate</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> AuthenticationCertificate 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(ServiceBrokerLanguageSpecifier.AuthenticationCertificate).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Signature"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signature.aspx">Signature</see>
	///
	public partial class TSqlSignatureReference : TSqlSignature, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSignatureReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Signature"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signature.aspx">Signature</see>
	///
	public partial class TSqlSignature : TSqlModelElement	{
		private static ModelTypeClass typeClass = Signature.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSignature to adapt instances of <see cref="T:Signature"/>
		/// </summary>
		public TSqlSignature(TSqlObject obj)  : base(obj, Signature.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSignature to adapt instances of <see cref="T:Signature"/>
		/// </summary>
		protected TSqlSignature()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Signature.IsCounterSignature"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signature.iscountersignature.aspx">Signature.IsCounterSignature</see>
		///
		public Boolean IsCounterSignature 
		{
			get { return Element.GetProperty<Boolean>(Signature.IsCounterSignature);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signature.encryptionmechanism.aspx">Signature.EncryptionMechanism</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> EncryptionMechanism 
		{
			get 
			{
				return Element.GetReferenced(Signature.EncryptionMechanism).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signature.signedobject.aspx">Signature.SignedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> SignedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Signature.SignedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SignatureEncryptionMechanism"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signatureencryptionmechanism.aspx">SignatureEncryptionMechanism</see>
	///
	public partial class TSqlSignatureEncryptionMechanismReference : TSqlSignatureEncryptionMechanism, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSignatureEncryptionMechanismReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SignatureEncryptionMechanism"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signatureencryptionmechanism.aspx">SignatureEncryptionMechanism</see>
	///
	public partial class TSqlSignatureEncryptionMechanism : TSqlModelElement	{
		private static ModelTypeClass typeClass = SignatureEncryptionMechanism.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSignatureEncryptionMechanism to adapt instances of <see cref="T:SignatureEncryptionMechanism"/>
		/// </summary>
		public TSqlSignatureEncryptionMechanism(TSqlObject obj)  : base(obj, SignatureEncryptionMechanism.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSignatureEncryptionMechanism to adapt instances of <see cref="T:SignatureEncryptionMechanism"/>
		/// </summary>
		protected TSqlSignatureEncryptionMechanism()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SignatureEncryptionMechanism.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signatureencryptionmechanism.password.aspx">SignatureEncryptionMechanism.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(SignatureEncryptionMechanism.Password);}
		}


		///
		/// Property wrapper for <see cref="M:SignatureEncryptionMechanism.SignedBlob"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signatureencryptionmechanism.signedblob.aspx">SignatureEncryptionMechanism.SignedBlob</see>
		///
		public String SignedBlob 
		{
			get { return Element.GetProperty<String>(SignatureEncryptionMechanism.SignedBlob);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signatureencryptionmechanism.asymmetrickey.aspx">SignatureEncryptionMechanism.AsymmetricKey</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference> AsymmetricKey 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SignatureEncryptionMechanism.AsymmetricKey).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference(o, Microsoft.SqlServer.Dac.Model.AsymmetricKey.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.signatureencryptionmechanism.certificate.aspx">SignatureEncryptionMechanism.Certificate</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> Certificate 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SignatureEncryptionMechanism.Certificate).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SoapLanguageSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.aspx">SoapLanguageSpecifier</see>
	///
	public partial class TSqlSoapLanguageSpecifierReference : TSqlSoapLanguageSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSoapLanguageSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SoapLanguageSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.aspx">SoapLanguageSpecifier</see>
	///
	public partial class TSqlSoapLanguageSpecifier : TSqlModelElement,IEndpointLanguageSpecifier	{
		private static ModelTypeClass typeClass = SoapLanguageSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSoapLanguageSpecifier to adapt instances of <see cref="T:SoapLanguageSpecifier"/>
		/// </summary>
		public TSqlSoapLanguageSpecifier(TSqlObject obj)  : base(obj, SoapLanguageSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSoapLanguageSpecifier to adapt instances of <see cref="T:SoapLanguageSpecifier"/>
		/// </summary>
		protected TSqlSoapLanguageSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.BatchesEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.batchesenabled.aspx">SoapLanguageSpecifier.BatchesEnabled</see>
		///
		public Boolean BatchesEnabled 
		{
			get { return Element.GetProperty<Boolean>(SoapLanguageSpecifier.BatchesEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.CharacterSet"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.characterset.aspx">SoapLanguageSpecifier.CharacterSet</see>
		///
		public CharacterSet CharacterSet 
		{
			get { return Element.GetProperty<CharacterSet>(SoapLanguageSpecifier.CharacterSet);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.DatabaseName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.databasename.aspx">SoapLanguageSpecifier.DatabaseName</see>
		///
		public String DatabaseName 
		{
			get { return Element.GetProperty<String>(SoapLanguageSpecifier.DatabaseName);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.HeaderLimit"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.headerlimit.aspx">SoapLanguageSpecifier.HeaderLimit</see>
		///
		public Int32 HeaderLimit 
		{
			get { return Element.GetProperty<Int32>(SoapLanguageSpecifier.HeaderLimit);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.IsDefaultDatabase"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.isdefaultdatabase.aspx">SoapLanguageSpecifier.IsDefaultDatabase</see>
		///
		public Boolean IsDefaultDatabase 
		{
			get { return Element.GetProperty<Boolean>(SoapLanguageSpecifier.IsDefaultDatabase);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.IsDefaultNamespace"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.isdefaultnamespace.aspx">SoapLanguageSpecifier.IsDefaultNamespace</see>
		///
		public Boolean IsDefaultNamespace 
		{
			get { return Element.GetProperty<Boolean>(SoapLanguageSpecifier.IsDefaultNamespace);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.IsDefaultWsdlSpName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.isdefaultwsdlspname.aspx">SoapLanguageSpecifier.IsDefaultWsdlSpName</see>
		///
		public Boolean IsDefaultWsdlSpName 
		{
			get { return Element.GetProperty<Boolean>(SoapLanguageSpecifier.IsDefaultWsdlSpName);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.LoginType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.logintype.aspx">SoapLanguageSpecifier.LoginType</see>
		///
		public SoapLoginType LoginType 
		{
			get { return Element.GetProperty<SoapLoginType>(SoapLanguageSpecifier.LoginType);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.Namespace"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.namespace.aspx">SoapLanguageSpecifier.Namespace</see>
		///
		public String Namespace 
		{
			get { return Element.GetProperty<String>(SoapLanguageSpecifier.Namespace);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.SchemaType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.schematype.aspx">SoapLanguageSpecifier.SchemaType</see>
		///
		public SoapSchema SchemaType 
		{
			get { return Element.GetProperty<SoapSchema>(SoapLanguageSpecifier.SchemaType);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.SessionsEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.sessionsenabled.aspx">SoapLanguageSpecifier.SessionsEnabled</see>
		///
		public Boolean SessionsEnabled 
		{
			get { return Element.GetProperty<Boolean>(SoapLanguageSpecifier.SessionsEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.SessionTimeout"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.sessiontimeout.aspx">SoapLanguageSpecifier.SessionTimeout</see>
		///
		public Int32 SessionTimeout 
		{
			get { return Element.GetProperty<Int32>(SoapLanguageSpecifier.SessionTimeout);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.SessionTimeoutNever"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.sessiontimeoutnever.aspx">SoapLanguageSpecifier.SessionTimeoutNever</see>
		///
		public Boolean SessionTimeoutNever 
		{
			get { return Element.GetProperty<Boolean>(SoapLanguageSpecifier.SessionTimeoutNever);}
		}


		///
		/// Property wrapper for <see cref="M:SoapLanguageSpecifier.WsdlSpName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.wsdlspname.aspx">SoapLanguageSpecifier.WsdlSpName</see>
		///
		public String WsdlSpName 
		{
			get { return Element.GetProperty<String>(SoapLanguageSpecifier.WsdlSpName);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSoapMethodSpecification"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soaplanguagespecifier.webmethods.aspx">SoapLanguageSpecifier.WebMethods</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSoapMethodSpecification> WebMethods 
		{
			get 
			{
				return Element.GetReferenced(SoapLanguageSpecifier.WebMethods).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSoapMethodSpecification(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SoapMethodSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.aspx">SoapMethodSpecification</see>
	///
	public partial class TSqlSoapMethodSpecificationReference : TSqlSoapMethodSpecification, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSoapMethodSpecificationReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SoapMethodSpecification"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.aspx">SoapMethodSpecification</see>
	///
	public partial class TSqlSoapMethodSpecification : TSqlModelElement	{
		private static ModelTypeClass typeClass = SoapMethodSpecification.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSoapMethodSpecification to adapt instances of <see cref="T:SoapMethodSpecification"/>
		/// </summary>
		public TSqlSoapMethodSpecification(TSqlObject obj)  : base(obj, SoapMethodSpecification.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSoapMethodSpecification to adapt instances of <see cref="T:SoapMethodSpecification"/>
		/// </summary>
		protected TSqlSoapMethodSpecification()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SoapMethodSpecification.Format"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.format.aspx">SoapMethodSpecification.Format</see>
		///
		public SoapFormat Format 
		{
			get { return Element.GetProperty<SoapFormat>(SoapMethodSpecification.Format);}
		}


		///
		/// Property wrapper for <see cref="M:SoapMethodSpecification.SchemaType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.schematype.aspx">SoapMethodSpecification.SchemaType</see>
		///
		public SoapSchema SchemaType 
		{
			get { return Element.GetProperty<SoapSchema>(SoapMethodSpecification.SchemaType);}
		}


		///
		/// Property wrapper for <see cref="M:SoapMethodSpecification.WebMethodAlias"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.webmethodalias.aspx">SoapMethodSpecification.WebMethodAlias</see>
		///
		public String WebMethodAlias 
		{
			get { return Element.GetProperty<String>(SoapMethodSpecification.WebMethodAlias);}
		}


		///
		/// Property wrapper for <see cref="M:SoapMethodSpecification.WebMethodNamespace"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.webmethodnamespace.aspx">SoapMethodSpecification.WebMethodNamespace</see>
		///
		public String WebMethodNamespace 
		{
			get { return Element.GetProperty<String>(SoapMethodSpecification.WebMethodNamespace);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.soapmethodspecification.relatedmethod.aspx">SoapMethodSpecification.RelatedMethod</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> RelatedMethod 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SoapMethodSpecification.RelatedMethod).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SpatialIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.aspx">SpatialIndex</see>
	///
	public partial class TSqlSpatialIndexReference : TSqlSpatialIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSpatialIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SpatialIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.aspx">SpatialIndex</see>
	///
	public partial class TSqlSpatialIndex : TSqlModelElement,ISqlIndex	{
		private static ModelTypeClass typeClass = SpatialIndex.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSpatialIndex to adapt instances of <see cref="T:SpatialIndex"/>
		/// </summary>
		public TSqlSpatialIndex(TSqlObject obj)  : base(obj, SpatialIndex.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSpatialIndex to adapt instances of <see cref="T:SpatialIndex"/>
		/// </summary>
		protected TSqlSpatialIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SpatialIndex.AllowPageLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.allowpagelocks.aspx">SpatialIndex.AllowPageLocks</see>
		///
		public Boolean AllowPageLocks 
		{
			get { return Element.GetProperty<Boolean>(SpatialIndex.AllowPageLocks);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.AllowRowLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.allowrowlocks.aspx">SpatialIndex.AllowRowLocks</see>
		///
		public Boolean AllowRowLocks 
		{
			get { return Element.GetProperty<Boolean>(SpatialIndex.AllowRowLocks);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.CellsPerObject"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.cellsperobject.aspx">SpatialIndex.CellsPerObject</see>
		///
		public Int32? CellsPerObject 
		{
			get { return Element.GetProperty<Int32?>(SpatialIndex.CellsPerObject);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.DataCompression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.datacompression.aspx">SpatialIndex.DataCompression</see>
		///
		public CompressionLevel DataCompression 
		{
			get { return Element.GetProperty<CompressionLevel>(SpatialIndex.DataCompression);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.disabled.aspx">SpatialIndex.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(SpatialIndex.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.FillFactor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.fillfactor.aspx">SpatialIndex.FillFactor</see>
		///
		public Int32 FillFactor 
		{
			get { return Element.GetProperty<Int32>(SpatialIndex.FillFactor);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.GridLevel1Density"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.gridlevel1density.aspx">SpatialIndex.GridLevel1Density</see>
		///
		public Degree GridLevel1Density 
		{
			get { return Element.GetProperty<Degree>(SpatialIndex.GridLevel1Density);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.GridLevel2Density"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.gridlevel2density.aspx">SpatialIndex.GridLevel2Density</see>
		///
		public Degree GridLevel2Density 
		{
			get { return Element.GetProperty<Degree>(SpatialIndex.GridLevel2Density);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.GridLevel3Density"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.gridlevel3density.aspx">SpatialIndex.GridLevel3Density</see>
		///
		public Degree GridLevel3Density 
		{
			get { return Element.GetProperty<Degree>(SpatialIndex.GridLevel3Density);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.GridLevel4Density"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.gridlevel4density.aspx">SpatialIndex.GridLevel4Density</see>
		///
		public Degree GridLevel4Density 
		{
			get { return Element.GetProperty<Degree>(SpatialIndex.GridLevel4Density);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.ignoreduplicatekey.aspx">SpatialIndex.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(SpatialIndex.IgnoreDuplicateKey);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.RecomputeStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.recomputestatistics.aspx">SpatialIndex.RecomputeStatistics</see>
		///
		public Boolean RecomputeStatistics 
		{
			get { return Element.GetProperty<Boolean>(SpatialIndex.RecomputeStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.Tessellation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.tessellation.aspx">SpatialIndex.Tessellation</see>
		///
		public Tessellation Tessellation 
		{
			get { return Element.GetProperty<Tessellation>(SpatialIndex.Tessellation);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.WithPadIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.withpadindex.aspx">SpatialIndex.WithPadIndex</see>
		///
		public Boolean WithPadIndex 
		{
			get { return Element.GetProperty<Boolean>(SpatialIndex.WithPadIndex);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.XMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.xmax.aspx">SpatialIndex.XMax</see>
		///
		public Double? XMax 
		{
			get { return Element.GetProperty<Double?>(SpatialIndex.XMax);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.XMin"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.xmin.aspx">SpatialIndex.XMin</see>
		///
		public Double? XMin 
		{
			get { return Element.GetProperty<Double?>(SpatialIndex.XMin);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.YMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.ymax.aspx">SpatialIndex.YMax</see>
		///
		public Double? YMax 
		{
			get { return Element.GetProperty<Double?>(SpatialIndex.YMax);}
		}


		///
		/// Property wrapper for <see cref="M:SpatialIndex.YMin"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.ymin.aspx">SpatialIndex.YMin</see>
		///
		public Double? YMin 
		{
			get { return Element.GetProperty<Double?>(SpatialIndex.YMin);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.column.aspx">SpatialIndex.Column</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Column 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SpatialIndex.Column).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.filegroup.aspx">SpatialIndex.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SpatialIndex.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.indexedobject.aspx">SpatialIndex.IndexedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> IndexedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SpatialIndex.IndexedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISpecifiesIndexElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.partitioncolumn.aspx">SpatialIndex.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SpatialIndex.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.spatialindex.partitionscheme.aspx">SpatialIndex.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SpatialIndex.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Statistics"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.aspx">Statistics</see>
	///
	public partial class TSqlStatisticsReference : TSqlStatistics, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlStatisticsReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Statistics"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.aspx">Statistics</see>
	///
	public partial class TSqlStatistics : TSqlModelElement	{
		private static ModelTypeClass typeClass = Statistics.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlStatistics to adapt instances of <see cref="T:Statistics"/>
		/// </summary>
		public TSqlStatistics(TSqlObject obj)  : base(obj, Statistics.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlStatistics to adapt instances of <see cref="T:Statistics"/>
		/// </summary>
		protected TSqlStatistics()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Statistics.FilterPredicate"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.filterpredicate.aspx">Statistics.FilterPredicate</see>
		///
		public String FilterPredicate 
		{
			get { return (String)Element.GetProperty(Statistics.FilterPredicate);}
		}


		///
		/// Property wrapper for <see cref="M:Statistics.Incremental"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.incremental.aspx">Statistics.Incremental</see>
		///
		public Boolean Incremental 
		{
			get { return Element.GetProperty<Boolean>(Statistics.Incremental);}
		}


		///
		/// Property wrapper for <see cref="M:Statistics.NoRecompute"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.norecompute.aspx">Statistics.NoRecompute</see>
		///
		public Boolean NoRecompute 
		{
			get { return Element.GetProperty<Boolean>(Statistics.NoRecompute);}
		}


		///
		/// Property wrapper for <see cref="M:Statistics.SampleSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.samplesize.aspx">Statistics.SampleSize</see>
		///
		public Int32 SampleSize 
		{
			get { return Element.GetProperty<Int32>(Statistics.SampleSize);}
		}


		///
		/// Property wrapper for <see cref="M:Statistics.SamplingStyle"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.samplingstyle.aspx">Statistics.SamplingStyle</see>
		///
		public SamplingStyle SamplingStyle 
		{
			get { return Element.GetProperty<SamplingStyle>(Statistics.SamplingStyle);}
		}


		///
		/// Property wrapper for <see cref="M:Statistics.StatsStream"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.statsstream.aspx">Statistics.StatsStream</see>
		///
		public String StatsStream 
		{
			get { return Element.GetProperty<String>(Statistics.StatsStream);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.columns.aspx">Statistics.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Statistics.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.expressiondependencies.aspx">Statistics.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Statistics.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.statistics.onobject.aspx">Statistics.OnObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> OnObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Statistics.OnObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Parameter"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.aspx">Parameter</see>
	///
	public partial class TSqlParameterReference : TSqlParameter, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlParameterReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Parameter"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.aspx">Parameter</see>
	///
	public partial class TSqlParameter : TSqlModelElement	{
		private static ModelTypeClass typeClass = Parameter.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlParameter to adapt instances of <see cref="T:Parameter"/>
		/// </summary>
		public TSqlParameter(TSqlObject obj)  : base(obj, Parameter.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlParameter to adapt instances of <see cref="T:Parameter"/>
		/// </summary>
		protected TSqlParameter()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Parameter.DefaultExpression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.defaultexpression.aspx">Parameter.DefaultExpression</see>
		///
		public String DefaultExpression 
		{
			get { return (String)Element.GetProperty(Parameter.DefaultExpression);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.IsMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.ismax.aspx">Parameter.IsMax</see>
		///
		public Boolean IsMax 
		{
			get { return Element.GetProperty<Boolean>(Parameter.IsMax);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.IsOutput"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.isoutput.aspx">Parameter.IsOutput</see>
		///
		public Boolean IsOutput 
		{
			get { return Element.GetProperty<Boolean>(Parameter.IsOutput);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.Length"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.length.aspx">Parameter.Length</see>
		///
		public Int32 Length 
		{
			get { return Element.GetProperty<Int32>(Parameter.Length);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.Precision"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.precision.aspx">Parameter.Precision</see>
		///
		public Int32 Precision 
		{
			get { return Element.GetProperty<Int32>(Parameter.Precision);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.ReadOnly"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.readonly.aspx">Parameter.ReadOnly</see>
		///
		public Boolean ReadOnly 
		{
			get { return Element.GetProperty<Boolean>(Parameter.ReadOnly);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.Scale"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.scale.aspx">Parameter.Scale</see>
		///
		public Int32 Scale 
		{
			get { return Element.GetProperty<Int32>(Parameter.Scale);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.Varying"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.varying.aspx">Parameter.Varying</see>
		///
		public Boolean Varying 
		{
			get { return Element.GetProperty<Boolean>(Parameter.Varying);}
		}


		///
		/// Property wrapper for <see cref="M:Parameter.XmlStyle"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.xmlstyle.aspx">Parameter.XmlStyle</see>
		///
		public XmlStyle XmlStyle 
		{
			get { return Element.GetProperty<XmlStyle>(Parameter.XmlStyle);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.datatype.aspx">Parameter.DataType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> DataType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Parameter.DataType).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlDataTypeElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.parameter.xmlschemacollection.aspx">Parameter.XmlSchemaCollection</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference> XmlSchemaCollection 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Parameter.XmlSchemaCollection).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference(o, Microsoft.SqlServer.Dac.Model.XmlSchemaCollection.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SymmetricKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.aspx">SymmetricKey</see>
	///
	public partial class TSqlSymmetricKeyReference : TSqlSymmetricKey, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSymmetricKeyReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SymmetricKey"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.aspx">SymmetricKey</see>
	///
	public partial class TSqlSymmetricKey : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = SymmetricKey.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSymmetricKey to adapt instances of <see cref="T:SymmetricKey"/>
		/// </summary>
		public TSqlSymmetricKey(TSqlObject obj)  : base(obj, SymmetricKey.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSymmetricKey to adapt instances of <see cref="T:SymmetricKey"/>
		/// </summary>
		protected TSqlSymmetricKey()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SymmetricKey.Algorithm"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.algorithm.aspx">SymmetricKey.Algorithm</see>
		///
		public SymmetricKeyAlgorithm Algorithm 
		{
			get { return Element.GetProperty<SymmetricKeyAlgorithm>(SymmetricKey.Algorithm);}
		}


		///
		/// Property wrapper for <see cref="M:SymmetricKey.CreationDisposition"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.creationdisposition.aspx">SymmetricKey.CreationDisposition</see>
		///
		public SymmetricKeyCreationDisposition CreationDisposition 
		{
			get { return Element.GetProperty<SymmetricKeyCreationDisposition>(SymmetricKey.CreationDisposition);}
		}


		///
		/// Property wrapper for <see cref="M:SymmetricKey.IdentityValue"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.identityvalue.aspx">SymmetricKey.IdentityValue</see>
		///
		public String IdentityValue 
		{
			get { return Element.GetProperty<String>(SymmetricKey.IdentityValue);}
		}


		///
		/// Property wrapper for <see cref="M:SymmetricKey.KeySource"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.keysource.aspx">SymmetricKey.KeySource</see>
		///
		public String KeySource 
		{
			get { return Element.GetProperty<String>(SymmetricKey.KeySource);}
		}


		///
		/// Property wrapper for <see cref="M:SymmetricKey.ProviderKeyName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.providerkeyname.aspx">SymmetricKey.ProviderKeyName</see>
		///
		public String ProviderKeyName 
		{
			get { return Element.GetProperty<String>(SymmetricKey.ProviderKeyName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.asymmetrickeys.aspx">SymmetricKey.AsymmetricKeys</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference> AsymmetricKeys 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SymmetricKey.AsymmetricKeys).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference(o, Microsoft.SqlServer.Dac.Model.AsymmetricKey.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.authorizer.aspx">SymmetricKey.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SymmetricKey.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.certificates.aspx">SymmetricKey.Certificates</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> Certificates 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SymmetricKey.Certificates).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.passwords.aspx">SymmetricKey.Passwords</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> Passwords 
		{
			get 
			{
				return Element.GetReferenced(SymmetricKey.Passwords).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.provider.aspx">SymmetricKey.Provider</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> Provider 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SymmetricKey.Provider).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSymmetricKeyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickey.symmetrickeys.aspx">SymmetricKey.SymmetricKeys</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSymmetricKeyReference> SymmetricKeys 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SymmetricKey.SymmetricKeys).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSymmetricKeyReference(o, Microsoft.SqlServer.Dac.Model.SymmetricKey.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SymmetricKeyPassword"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickeypassword.aspx">SymmetricKeyPassword</see>
	///
	public partial class TSqlSymmetricKeyPasswordReference : TSqlSymmetricKeyPassword, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSymmetricKeyPasswordReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SymmetricKeyPassword"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickeypassword.aspx">SymmetricKeyPassword</see>
	///
	public partial class TSqlSymmetricKeyPassword : TSqlModelElement	{
		private static ModelTypeClass typeClass = SymmetricKeyPassword.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSymmetricKeyPassword to adapt instances of <see cref="T:SymmetricKeyPassword"/>
		/// </summary>
		public TSqlSymmetricKeyPassword(TSqlObject obj)  : base(obj, SymmetricKeyPassword.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSymmetricKeyPassword to adapt instances of <see cref="T:SymmetricKeyPassword"/>
		/// </summary>
		protected TSqlSymmetricKeyPassword()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SymmetricKeyPassword.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.symmetrickeypassword.password.aspx">SymmetricKeyPassword.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(SymmetricKeyPassword.Password);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Synonym"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.synonym.aspx">Synonym</see>
	///
	public partial class TSqlSynonymReference : TSqlSynonym, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSynonymReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Synonym"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.synonym.aspx">Synonym</see>
	///
	public partial class TSqlSynonym : TSqlModelElement	{
		private static ModelTypeClass typeClass = Synonym.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSynonym to adapt instances of <see cref="T:Synonym"/>
		/// </summary>
		public TSqlSynonym(TSqlObject obj)  : base(obj, Synonym.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSynonym to adapt instances of <see cref="T:Synonym"/>
		/// </summary>
		protected TSqlSynonym()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Synonym.ForObjectName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.synonym.forobjectname.aspx">Synonym.ForObjectName</see>
		///
		public String ForObjectName 
		{
			get { return (String)Element.GetProperty(Synonym.ForObjectName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.synonym.forobject.aspx">Synonym.ForObject</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ForObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Synonym.ForObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.synonym.schema.aspx">Synonym.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Synonym.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Table"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.aspx">Table</see>
	///
	public partial class TSqlTableReference : TSqlTable, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:Table"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.aspx">Table</see>
	///
	public partial class TSqlTable : TSqlModelElement,ISqlColumnSource,ISpecifiesIndex,ISpecifiesStorage	{
		private static ModelTypeClass typeClass = Table.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTable to adapt instances of <see cref="T:Table"/>
		/// </summary>
		public TSqlTable(TSqlObject obj)  : base(obj, Table.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTable to adapt instances of <see cref="T:Table"/>
		/// </summary>
		protected TSqlTable()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:Table.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.ansinullson.aspx">Table.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(Table.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:Table.ChangeDataCaptureEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.changedatacaptureenabled.aspx">Table.ChangeDataCaptureEnabled</see>
		///
		public Boolean ChangeDataCaptureEnabled 
		{
			get { return Element.GetProperty<Boolean>(Table.ChangeDataCaptureEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:Table.ChangeTrackingEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.changetrackingenabled.aspx">Table.ChangeTrackingEnabled</see>
		///
		public Boolean ChangeTrackingEnabled 
		{
			get { return Element.GetProperty<Boolean>(Table.ChangeTrackingEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:Table.DataPages"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.datapages.aspx">Table.DataPages</see>
		///
		public Int64? DataPages 
		{
			get { return Element.GetProperty<Int64?>(Table.DataPages);}
		}


		///
		/// Property wrapper for <see cref="M:Table.DataSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.datasize.aspx">Table.DataSize</see>
		///
		public Double? DataSize 
		{
			get { return Element.GetProperty<Double?>(Table.DataSize);}
		}


		///
		/// Property wrapper for <see cref="M:Table.Durability"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.durability.aspx">Table.Durability</see>
		///
		public Durability Durability 
		{
			get { return Element.GetProperty<Durability>(Table.Durability);}
		}


		///
		/// Property wrapper for <see cref="M:Table.FileStreamNull"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.filestreamnull.aspx">Table.FileStreamNull</see>
		///
		public Boolean? FileStreamNull 
		{
			get { return Element.GetProperty<Boolean?>(Table.FileStreamNull);}
		}


		///
		/// Property wrapper for <see cref="M:Table.IndexSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.indexsize.aspx">Table.IndexSize</see>
		///
		public Double? IndexSize 
		{
			get { return Element.GetProperty<Double?>(Table.IndexSize);}
		}


		///
		/// Property wrapper for <see cref="M:Table.IsReplicated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.isreplicated.aspx">Table.IsReplicated</see>
		///
		public Boolean IsReplicated 
		{
			get { return Element.GetProperty<Boolean>(Table.IsReplicated);}
		}


		///
		/// Property wrapper for <see cref="M:Table.LargeValueTypesOutOfRow"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.largevaluetypesoutofrow.aspx">Table.LargeValueTypesOutOfRow</see>
		///
		public Boolean LargeValueTypesOutOfRow 
		{
			get { return Element.GetProperty<Boolean>(Table.LargeValueTypesOutOfRow);}
		}


		///
		/// Property wrapper for <see cref="M:Table.LockEscalation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.lockescalation.aspx">Table.LockEscalation</see>
		///
		public LockEscalationMethod LockEscalation 
		{
			get { return Element.GetProperty<LockEscalationMethod>(Table.LockEscalation);}
		}


		///
		/// Property wrapper for <see cref="M:Table.MemoryOptimized"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.memoryoptimized.aspx">Table.MemoryOptimized</see>
		///
		public Boolean MemoryOptimized 
		{
			get { return Element.GetProperty<Boolean>(Table.MemoryOptimized);}
		}


		///
		/// Property wrapper for <see cref="M:Table.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.quotedidentifieron.aspx">Table.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(Table.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:Table.RowCount"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.rowcount.aspx">Table.RowCount</see>
		///
		public Int64? RowCount 
		{
			get { return Element.GetProperty<Int64?>(Table.RowCount);}
		}


		///
		/// Property wrapper for <see cref="M:Table.TableLockOnBulkLoad"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.tablelockonbulkload.aspx">Table.TableLockOnBulkLoad</see>
		///
		public Boolean TableLockOnBulkLoad 
		{
			get { return Element.GetProperty<Boolean>(Table.TableLockOnBulkLoad);}
		}


		///
		/// Property wrapper for <see cref="M:Table.TextInRowSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.textinrowsize.aspx">Table.TextInRowSize</see>
		///
		public Int32 TextInRowSize 
		{
			get { return Element.GetProperty<Int32>(Table.TextInRowSize);}
		}


		///
		/// Property wrapper for <see cref="M:Table.TrackColumnsUpdated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.trackcolumnsupdated.aspx">Table.TrackColumnsUpdated</see>
		///
		public Boolean TrackColumnsUpdated 
		{
			get { return Element.GetProperty<Boolean>(Table.TrackColumnsUpdated);}
		}


		///
		/// Property wrapper for <see cref="M:Table.UsedPages"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.usedpages.aspx">Table.UsedPages</see>
		///
		public Int64? UsedPages 
		{
			get { return Element.GetProperty<Int64?>(Table.UsedPages);}
		}


		///
		/// Property wrapper for <see cref="M:Table.VardecimalStorageFormatEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.vardecimalstorageformatenabled.aspx">Table.VardecimalStorageFormatEnabled</see>
		///
		public Boolean VardecimalStorageFormatEnabled 
		{
			get { return Element.GetProperty<Boolean>(Table.VardecimalStorageFormatEnabled);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.columns.aspx">Table.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn> Columns 
		{
			get 
			{
				return Element.GetReferenced(Table.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.datacompressionoptions.aspx">Table.DataCompressionOptions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption> DataCompressionOptions 
		{
			get 
			{
				return Element.GetReferenced(Table.DataCompressionOptions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.filegroup.aspx">Table.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.filegroupfortextimage.aspx">Table.FilegroupForTextImage</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> FilegroupForTextImage 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.FilegroupForTextImage).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.filestreamfilegroup.aspx">Table.FileStreamFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> FileStreamFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.FileStreamFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.filestreampartitionscheme.aspx">Table.FileStreamPartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> FileStreamPartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.FileStreamPartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.partitioncolumn.aspx">Table.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.partitionscheme.aspx">Table.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.table.schema.aspx">Table.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(Table.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FileTable"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.aspx">FileTable</see>
	///
	public partial class TSqlFileTableReference : TSqlFileTable, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlFileTableReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:FileTable"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.aspx">FileTable</see>
	///
	public partial class TSqlFileTable : TSqlModelElement,ISqlColumnSource,ISpecifiesIndex,ISpecifiesStorage	{
		private static ModelTypeClass typeClass = FileTable.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlFileTable to adapt instances of <see cref="T:FileTable"/>
		/// </summary>
		public TSqlFileTable(TSqlObject obj)  : base(obj, FileTable.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlFileTable to adapt instances of <see cref="T:FileTable"/>
		/// </summary>
		protected TSqlFileTable()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:FileTable.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.ansinullson.aspx">FileTable.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(FileTable.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.FileStreamNull"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filestreamnull.aspx">FileTable.FileStreamNull</see>
		///
		public Boolean? FileStreamNull 
		{
			get { return Element.GetProperty<Boolean?>(FileTable.FileStreamNull);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.FileTableCollateFilename"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filetablecollatefilename.aspx">FileTable.FileTableCollateFilename</see>
		///
		public String FileTableCollateFilename 
		{
			get { return Element.GetProperty<String>(FileTable.FileTableCollateFilename);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.FileTableDirectory"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filetabledirectory.aspx">FileTable.FileTableDirectory</see>
		///
		public String FileTableDirectory 
		{
			get { return Element.GetProperty<String>(FileTable.FileTableDirectory);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.FileTableNamespaceEnabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filetablenamespaceenabled.aspx">FileTable.FileTableNamespaceEnabled</see>
		///
		public Boolean FileTableNamespaceEnabled 
		{
			get { return Element.GetProperty<Boolean>(FileTable.FileTableNamespaceEnabled);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.LockEscalation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.lockescalation.aspx">FileTable.LockEscalation</see>
		///
		public LockEscalationMethod LockEscalation 
		{
			get { return Element.GetProperty<LockEscalationMethod>(FileTable.LockEscalation);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.quotedidentifieron.aspx">FileTable.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(FileTable.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:FileTable.TableLockOnBulkLoad"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.tablelockonbulkload.aspx">FileTable.TableLockOnBulkLoad</see>
		///
		public Boolean TableLockOnBulkLoad 
		{
			get { return Element.GetProperty<Boolean>(FileTable.TableLockOnBulkLoad);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.columns.aspx">FileTable.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn> Columns 
		{
			get 
			{
				return Element.GetReferenced(FileTable.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.datacompressionoptions.aspx">FileTable.DataCompressionOptions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption> DataCompressionOptions 
		{
			get 
			{
				return Element.GetReferenced(FileTable.DataCompressionOptions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filegroup.aspx">FileTable.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FileTable.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filestreamfilegroup.aspx">FileTable.FileStreamFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> FileStreamFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FileTable.FileStreamFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.filestreampartitionscheme.aspx">FileTable.FileStreamPartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> FileStreamPartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FileTable.FileStreamPartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.partitioncolumn.aspx">FileTable.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FileTable.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.partitionscheme.aspx">FileTable.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FileTable.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.filetable.schema.aspx">FileTable.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(FileTable.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.aspx">TableType</see>
	///
	public partial class TSqlTableTypeReference : TSqlTableType, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.aspx">TableType</see>
	///
	public partial class TSqlTableType : TSqlModelElement,ISqlSecurable,ISqlDataType	{
		private static ModelTypeClass typeClass = TableType.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableType to adapt instances of <see cref="T:TableType"/>
		/// </summary>
		public TSqlTableType(TSqlObject obj)  : base(obj, TableType.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableType to adapt instances of <see cref="T:TableType"/>
		/// </summary>
		protected TSqlTableType()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableType.MemoryOptimized"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.memoryoptimized.aspx">TableType.MemoryOptimized</see>
		///
		public Boolean MemoryOptimized 
		{
			get { return Element.GetProperty<Boolean>(TableType.MemoryOptimized);}
		}


		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeColumn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.columns.aspx">TableType.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeColumn> Columns 
		{
			get 
			{
				return Element.GetReferenced(TableType.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeColumn(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.constraints.aspx">TableType.Constraints</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint> Constraints 
		{
			get 
			{
				return Element.GetReferenced(TableType.Constraints).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.indexes.aspx">TableType.Indexes</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeIndex> Indexes 
		{
			get 
			{
				return Element.GetReferenced(TableType.Indexes).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeIndex)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletype.schema.aspx">TableType.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableType.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeCheckConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecheckconstraint.aspx">TableTypeCheckConstraint</see>
	///
	public partial class TSqlTableTypeCheckConstraintReference : TSqlTableTypeCheckConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypeCheckConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeCheckConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecheckconstraint.aspx">TableTypeCheckConstraint</see>
	///
	public partial class TSqlTableTypeCheckConstraint : TSqlModelElement,ITableTypeConstraint	{
		private static ModelTypeClass typeClass = TableTypeCheckConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeCheckConstraint to adapt instances of <see cref="T:TableTypeCheckConstraint"/>
		/// </summary>
		public TSqlTableTypeCheckConstraint(TSqlObject obj)  : base(obj, TableTypeCheckConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeCheckConstraint to adapt instances of <see cref="T:TableTypeCheckConstraint"/>
		/// </summary>
		protected TSqlTableTypeCheckConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableTypeCheckConstraint.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecheckconstraint.expression.aspx">TableTypeCheckConstraint.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(TableTypeCheckConstraint.Expression);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecheckconstraint.expressiondependencies.aspx">TableTypeCheckConstraint.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeCheckConstraint.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeColumn"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.aspx">TableTypeColumn</see>
	///
	public partial class TSqlTableTypeColumnReference : TSqlTableTypeColumn, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypeColumnReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeColumn"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.aspx">TableTypeColumn</see>
	///
	public partial class TSqlTableTypeColumn : TSqlModelElement	{
		private static ModelTypeClass typeClass = TableTypeColumn.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeColumn to adapt instances of <see cref="T:TableTypeColumn"/>
		/// </summary>
		public TSqlTableTypeColumn(TSqlObject obj)  : base(obj, TableTypeColumn.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeColumn to adapt instances of <see cref="T:TableTypeColumn"/>
		/// </summary>
		protected TSqlTableTypeColumn()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Collation"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.collation.aspx">TableTypeColumn.Collation</see>
		///
		public String Collation 
		{
			get { return Element.GetProperty<String>(TableTypeColumn.Collation);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.expression.aspx">TableTypeColumn.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(TableTypeColumn.Expression);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.IdentityIncrement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.identityincrement.aspx">TableTypeColumn.IdentityIncrement</see>
		///
		public String IdentityIncrement 
		{
			get { return Element.GetProperty<String>(TableTypeColumn.IdentityIncrement);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.IdentitySeed"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.identityseed.aspx">TableTypeColumn.IdentitySeed</see>
		///
		public String IdentitySeed 
		{
			get { return Element.GetProperty<String>(TableTypeColumn.IdentitySeed);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.IsIdentity"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.isidentity.aspx">TableTypeColumn.IsIdentity</see>
		///
		public Boolean IsIdentity 
		{
			get { return Element.GetProperty<Boolean>(TableTypeColumn.IsIdentity);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.IsMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.ismax.aspx">TableTypeColumn.IsMax</see>
		///
		public Boolean IsMax 
		{
			get { return Element.GetProperty<Boolean>(TableTypeColumn.IsMax);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.IsRowGuidCol"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.isrowguidcol.aspx">TableTypeColumn.IsRowGuidCol</see>
		///
		public Boolean IsRowGuidCol 
		{
			get { return Element.GetProperty<Boolean>(TableTypeColumn.IsRowGuidCol);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Length"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.length.aspx">TableTypeColumn.Length</see>
		///
		public Int32 Length 
		{
			get { return Element.GetProperty<Int32>(TableTypeColumn.Length);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Nullable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.nullable.aspx">TableTypeColumn.Nullable</see>
		///
		public Boolean Nullable 
		{
			get { return Element.GetProperty<Boolean>(TableTypeColumn.Nullable);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Persisted"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.persisted.aspx">TableTypeColumn.Persisted</see>
		///
		public Boolean Persisted 
		{
			get { return Element.GetProperty<Boolean>(TableTypeColumn.Persisted);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.PersistedNullable"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.persistednullable.aspx">TableTypeColumn.PersistedNullable</see>
		///
		public Boolean? PersistedNullable 
		{
			get { return Element.GetProperty<Boolean?>(TableTypeColumn.PersistedNullable);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Precision"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.precision.aspx">TableTypeColumn.Precision</see>
		///
		public Int32 Precision 
		{
			get { return Element.GetProperty<Int32>(TableTypeColumn.Precision);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.Scale"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.scale.aspx">TableTypeColumn.Scale</see>
		///
		public Int32 Scale 
		{
			get { return Element.GetProperty<Int32>(TableTypeColumn.Scale);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeColumn.XmlStyle"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.xmlstyle.aspx">TableTypeColumn.XmlStyle</see>
		///
		public XmlStyle XmlStyle 
		{
			get { return Element.GetProperty<XmlStyle>(TableTypeColumn.XmlStyle);}
		}


		///
		/// Metadata property wrapper for <see cref="M:TableTypeColumn.TableTypeColumnType"/>
		///
		public TableTypeColumnType TableTypeColumnType 
		{
			get { return Element.GetMetadata<TableTypeColumnType>(TableTypeColumn.TableTypeColumnType);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.datatype.aspx">TableTypeColumn.DataType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> DataType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeColumn.DataType).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlDataTypeElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.expressiondependencies.aspx">TableTypeColumn.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeColumn.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypecolumn.xmlschemacollection.aspx">TableTypeColumn.XmlSchemaCollection</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference> XmlSchemaCollection 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeColumn.XmlSchemaCollection).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlSchemaCollectionReference(o, Microsoft.SqlServer.Dac.Model.XmlSchemaCollection.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeDefaultConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypedefaultconstraint.aspx">TableTypeDefaultConstraint</see>
	///
	public partial class TSqlTableTypeDefaultConstraintReference : TSqlTableTypeDefaultConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypeDefaultConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeDefaultConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypedefaultconstraint.aspx">TableTypeDefaultConstraint</see>
	///
	public partial class TSqlTableTypeDefaultConstraint : TSqlModelElement,ITableTypeConstraint	{
		private static ModelTypeClass typeClass = TableTypeDefaultConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeDefaultConstraint to adapt instances of <see cref="T:TableTypeDefaultConstraint"/>
		/// </summary>
		public TSqlTableTypeDefaultConstraint(TSqlObject obj)  : base(obj, TableTypeDefaultConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeDefaultConstraint to adapt instances of <see cref="T:TableTypeDefaultConstraint"/>
		/// </summary>
		protected TSqlTableTypeDefaultConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableTypeDefaultConstraint.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypedefaultconstraint.expression.aspx">TableTypeDefaultConstraint.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(TableTypeDefaultConstraint.Expression);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypedefaultconstraint.expressiondependencies.aspx">TableTypeDefaultConstraint.ExpressionDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ExpressionDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeDefaultConstraint.ExpressionDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypedefaultconstraint.targetcolumn.aspx">TableTypeDefaultConstraint.TargetColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> TargetColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeDefaultConstraint.TargetColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeindex.aspx">TableTypeIndex</see>
	///
	public partial class TSqlTableTypeIndexReference : TSqlTableTypeIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypeIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeindex.aspx">TableTypeIndex</see>
	///
	public partial class TSqlTableTypeIndex : TSqlModelElement,ISqlIndex	{
		private static ModelTypeClass typeClass = TableTypeIndex.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeIndex to adapt instances of <see cref="T:TableTypeIndex"/>
		/// </summary>
		public TSqlTableTypeIndex(TSqlObject obj)  : base(obj, TableTypeIndex.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeIndex to adapt instances of <see cref="T:TableTypeIndex"/>
		/// </summary>
		protected TSqlTableTypeIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableTypeIndex.BucketCount"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeindex.bucketcount.aspx">TableTypeIndex.BucketCount</see>
		///
		public Int32? BucketCount 
		{
			get { return Element.GetProperty<Int32?>(TableTypeIndex.BucketCount);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeIndex.Hash"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeindex.hash.aspx">TableTypeIndex.Hash</see>
		///
		public Boolean Hash 
		{
			get { return Element.GetProperty<Boolean>(TableTypeIndex.Hash);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeIndex.IsDisabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeindex.isdisabled.aspx">TableTypeIndex.IsDisabled</see>
		///
		public Boolean IsDisabled 
		{
			get { return Element.GetProperty<Boolean>(TableTypeIndex.IsDisabled);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeindex.columns.aspx">TableTypeIndex.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeIndex.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypePrimaryKeyConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.aspx">TableTypePrimaryKeyConstraint</see>
	///
	public partial class TSqlTableTypePrimaryKeyConstraintReference : TSqlTableTypePrimaryKeyConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypePrimaryKeyConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypePrimaryKeyConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.aspx">TableTypePrimaryKeyConstraint</see>
	///
	public partial class TSqlTableTypePrimaryKeyConstraint : TSqlModelElement,ITableTypeConstraint	{
		private static ModelTypeClass typeClass = TableTypePrimaryKeyConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableTypePrimaryKeyConstraint to adapt instances of <see cref="T:TableTypePrimaryKeyConstraint"/>
		/// </summary>
		public TSqlTableTypePrimaryKeyConstraint(TSqlObject obj)  : base(obj, TableTypePrimaryKeyConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableTypePrimaryKeyConstraint to adapt instances of <see cref="T:TableTypePrimaryKeyConstraint"/>
		/// </summary>
		protected TSqlTableTypePrimaryKeyConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableTypePrimaryKeyConstraint.BucketCount"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.bucketcount.aspx">TableTypePrimaryKeyConstraint.BucketCount</see>
		///
		public Int32? BucketCount 
		{
			get { return Element.GetProperty<Int32?>(TableTypePrimaryKeyConstraint.BucketCount);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypePrimaryKeyConstraint.Clustered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.clustered.aspx">TableTypePrimaryKeyConstraint.Clustered</see>
		///
		public Boolean Clustered 
		{
			get { return Element.GetProperty<Boolean>(TableTypePrimaryKeyConstraint.Clustered);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypePrimaryKeyConstraint.Hash"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.hash.aspx">TableTypePrimaryKeyConstraint.Hash</see>
		///
		public Boolean Hash 
		{
			get { return Element.GetProperty<Boolean>(TableTypePrimaryKeyConstraint.Hash);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypePrimaryKeyConstraint.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.ignoreduplicatekey.aspx">TableTypePrimaryKeyConstraint.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(TableTypePrimaryKeyConstraint.IgnoreDuplicateKey);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TableTypePrimaryKeyConstraintColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeprimarykeyconstraint.columns.aspx">TableTypePrimaryKeyConstraint.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TableTypePrimaryKeyConstraintColumnsReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypePrimaryKeyConstraint.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TableTypePrimaryKeyConstraintColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeUniqueConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeuniqueconstraint.aspx">TableTypeUniqueConstraint</see>
	///
	public partial class TSqlTableTypeUniqueConstraintReference : TSqlTableTypeUniqueConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTableTypeUniqueConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TableTypeUniqueConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeuniqueconstraint.aspx">TableTypeUniqueConstraint</see>
	///
	public partial class TSqlTableTypeUniqueConstraint : TSqlModelElement,ITableTypeConstraint	{
		private static ModelTypeClass typeClass = TableTypeUniqueConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeUniqueConstraint to adapt instances of <see cref="T:TableTypeUniqueConstraint"/>
		/// </summary>
		public TSqlTableTypeUniqueConstraint(TSqlObject obj)  : base(obj, TableTypeUniqueConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTableTypeUniqueConstraint to adapt instances of <see cref="T:TableTypeUniqueConstraint"/>
		/// </summary>
		protected TSqlTableTypeUniqueConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TableTypeUniqueConstraint.Clustered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeuniqueconstraint.clustered.aspx">TableTypeUniqueConstraint.Clustered</see>
		///
		public Boolean Clustered 
		{
			get { return Element.GetProperty<Boolean>(TableTypeUniqueConstraint.Clustered);}
		}


		///
		/// Property wrapper for <see cref="M:TableTypeUniqueConstraint.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeuniqueconstraint.ignoreduplicatekey.aspx">TableTypeUniqueConstraint.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(TableTypeUniqueConstraint.IgnoreDuplicateKey);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TableTypeUniqueConstraintColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tabletypeuniqueconstraint.columns.aspx">TableTypeUniqueConstraint.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TableTypeUniqueConstraintColumnsReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(TableTypeUniqueConstraint.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TableTypeUniqueConstraintColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TcpProtocolSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tcpprotocolspecifier.aspx">TcpProtocolSpecifier</see>
	///
	public partial class TSqlTcpProtocolSpecifierReference : TSqlTcpProtocolSpecifier, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlTcpProtocolSpecifierReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:TcpProtocolSpecifier"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tcpprotocolspecifier.aspx">TcpProtocolSpecifier</see>
	///
	public partial class TSqlTcpProtocolSpecifier : TSqlModelElement,IProtocolSpecifier	{
		private static ModelTypeClass typeClass = TcpProtocolSpecifier.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlTcpProtocolSpecifier to adapt instances of <see cref="T:TcpProtocolSpecifier"/>
		/// </summary>
		public TSqlTcpProtocolSpecifier(TSqlObject obj)  : base(obj, TcpProtocolSpecifier.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlTcpProtocolSpecifier to adapt instances of <see cref="T:TcpProtocolSpecifier"/>
		/// </summary>
		protected TSqlTcpProtocolSpecifier()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:TcpProtocolSpecifier.ListenerIPv4"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tcpprotocolspecifier.listeneripv4.aspx">TcpProtocolSpecifier.ListenerIPv4</see>
		///
		public String ListenerIPv4 
		{
			get { return Element.GetProperty<String>(TcpProtocolSpecifier.ListenerIPv4);}
		}


		///
		/// Property wrapper for <see cref="M:TcpProtocolSpecifier.ListenerIPv6"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tcpprotocolspecifier.listeneripv6.aspx">TcpProtocolSpecifier.ListenerIPv6</see>
		///
		public String ListenerIPv6 
		{
			get { return Element.GetProperty<String>(TcpProtocolSpecifier.ListenerIPv6);}
		}


		///
		/// Property wrapper for <see cref="M:TcpProtocolSpecifier.ListenerPort"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tcpprotocolspecifier.listenerport.aspx">TcpProtocolSpecifier.ListenerPort</see>
		///
		public Int32 ListenerPort 
		{
			get { return Element.GetProperty<Int32>(TcpProtocolSpecifier.ListenerPort);}
		}


		///
		/// Property wrapper for <see cref="M:TcpProtocolSpecifier.ListeningOnAllIPs"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.tcpprotocolspecifier.listeningonallips.aspx">TcpProtocolSpecifier.ListeningOnAllIPs</see>
		///
		public Boolean ListeningOnAllIPs 
		{
			get { return Element.GetProperty<Boolean>(TcpProtocolSpecifier.ListeningOnAllIPs);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:UniqueConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.aspx">UniqueConstraint</see>
	///
	public partial class TSqlUniqueConstraintReference : TSqlUniqueConstraint, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlUniqueConstraintReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:UniqueConstraint"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.aspx">UniqueConstraint</see>
	///
	public partial class TSqlUniqueConstraint : TSqlModelElement,IExtendedPropertyHost,ISpecifiesStorage	{
		private static ModelTypeClass typeClass = UniqueConstraint.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlUniqueConstraint to adapt instances of <see cref="T:UniqueConstraint"/>
		/// </summary>
		public TSqlUniqueConstraint(TSqlObject obj)  : base(obj, UniqueConstraint.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlUniqueConstraint to adapt instances of <see cref="T:UniqueConstraint"/>
		/// </summary>
		protected TSqlUniqueConstraint()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:UniqueConstraint.AllowPageLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.allowpagelocks.aspx">UniqueConstraint.AllowPageLocks</see>
		///
		public Boolean AllowPageLocks 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.AllowPageLocks);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.AllowRowLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.allowrowlocks.aspx">UniqueConstraint.AllowRowLocks</see>
		///
		public Boolean AllowRowLocks 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.AllowRowLocks);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.Clustered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.clustered.aspx">UniqueConstraint.Clustered</see>
		///
		public Boolean Clustered 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.Clustered);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.disabled.aspx">UniqueConstraint.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.FileStreamNull"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.filestreamnull.aspx">UniqueConstraint.FileStreamNull</see>
		///
		public Boolean? FileStreamNull 
		{
			get { return Element.GetProperty<Boolean?>(UniqueConstraint.FileStreamNull);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.FillFactor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.fillfactor.aspx">UniqueConstraint.FillFactor</see>
		///
		public Int32? FillFactor 
		{
			get { return Element.GetProperty<Int32?>(UniqueConstraint.FillFactor);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.ignoreduplicatekey.aspx">UniqueConstraint.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.IgnoreDuplicateKey);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.RecomputeStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.recomputestatistics.aspx">UniqueConstraint.RecomputeStatistics</see>
		///
		public Boolean RecomputeStatistics 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.RecomputeStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:UniqueConstraint.WithPadIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.withpadindex.aspx">UniqueConstraint.WithPadIndex</see>
		///
		public Boolean WithPadIndex 
		{
			get { return Element.GetProperty<Boolean>(UniqueConstraint.WithPadIndex);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.UniqueConstraintColumnsReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.columns.aspx">UniqueConstraint.Columns</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.UniqueConstraintColumnsReference> Columns 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.UniqueConstraintColumnsReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.datacompressionoptions.aspx">UniqueConstraint.DataCompressionOptions</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption> DataCompressionOptions 
		{
			get 
			{
				return Element.GetReferenced(UniqueConstraint.DataCompressionOptions).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataCompressionOption(o));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.filegroup.aspx">UniqueConstraint.Filegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> Filegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.Filegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.filestreamfilegroup.aspx">UniqueConstraint.FileStreamFilegroup</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference> FileStreamFilegroup 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.FileStreamFilegroup).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference(o, Microsoft.SqlServer.Dac.Model.Filegroup.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.filestreampartitionscheme.aspx">UniqueConstraint.FileStreamPartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> FileStreamPartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.FileStreamPartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.host.aspx">UniqueConstraint.Host</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference> Host 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.Host).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableReference(o, Microsoft.SqlServer.Dac.Model.Table.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.partitioncolumn.aspx">UniqueConstraint.PartitionColumn</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> PartitionColumn 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.PartitionColumn).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.uniqueconstraint.partitionscheme.aspx">UniqueConstraint.PartitionScheme</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference> PartitionScheme 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UniqueConstraint.PartitionScheme).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlPartitionSchemeReference(o, Microsoft.SqlServer.Dac.Model.PartitionScheme.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:User"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.aspx">User</see>
	///
	public partial class TSqlUserReference : TSqlUser, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlUserReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:User"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.aspx">User</see>
	///
	public partial class TSqlUser : TSqlModelElement,ISqlDatabaseSecurityPrincipal	{
		private static ModelTypeClass typeClass = User.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlUser to adapt instances of <see cref="T:User"/>
		/// </summary>
		public TSqlUser(TSqlObject obj)  : base(obj, User.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlUser to adapt instances of <see cref="T:User"/>
		/// </summary>
		protected TSqlUser()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:User.AuthenticationType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.authenticationtype.aspx">User.AuthenticationType</see>
		///
		public AuthenticationType AuthenticationType 
		{
			get { return Element.GetProperty<AuthenticationType>(User.AuthenticationType);}
		}


		///
		/// Property wrapper for <see cref="M:User.DefaultLanguage"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.defaultlanguage.aspx">User.DefaultLanguage</see>
		///
		public String DefaultLanguage 
		{
			get { return Element.GetProperty<String>(User.DefaultLanguage);}
		}


		///
		/// Property wrapper for <see cref="M:User.Password"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.password.aspx">User.Password</see>
		///
		public String Password 
		{
			get { return Element.GetProperty<String>(User.Password);}
		}


		///
		/// Property wrapper for <see cref="M:User.Sid"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.sid.aspx">User.Sid</see>
		///
		public String Sid 
		{
			get { return Element.GetProperty<String>(User.Sid);}
		}


		///
		/// Property wrapper for <see cref="M:User.WithoutLogin"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.withoutlogin.aspx">User.WithoutLogin</see>
		///
		public Boolean WithoutLogin 
		{
			get { return Element.GetProperty<Boolean>(User.WithoutLogin);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.asymmetrickey.aspx">User.AsymmetricKey</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference> AsymmetricKey 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(User.AsymmetricKey).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAsymmetricKeyReference(o, Microsoft.SqlServer.Dac.Model.AsymmetricKey.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.certificate.aspx">User.Certificate</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference> Certificate 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(User.Certificate).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlCertificateReference(o, Microsoft.SqlServer.Dac.Model.Certificate.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.defaultschema.aspx">User.DefaultSchema</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> DefaultSchema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(User.DefaultSchema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.user.login.aspx">User.Login</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference> Login 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(User.Login).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlLoginReference(o, Microsoft.SqlServer.Dac.Model.Login.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:UserDefinedServerRole"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedserverrole.aspx">UserDefinedServerRole</see>
	///
	public partial class TSqlUserDefinedServerRoleReference : TSqlUserDefinedServerRole, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlUserDefinedServerRoleReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:UserDefinedServerRole"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedserverrole.aspx">UserDefinedServerRole</see>
	///
	public partial class TSqlUserDefinedServerRole : TSqlModelElement	{
		private static ModelTypeClass typeClass = UserDefinedServerRole.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlUserDefinedServerRole to adapt instances of <see cref="T:UserDefinedServerRole"/>
		/// </summary>
		public TSqlUserDefinedServerRole(TSqlObject obj)  : base(obj, UserDefinedServerRole.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlUserDefinedServerRole to adapt instances of <see cref="T:UserDefinedServerRole"/>
		/// </summary>
		protected TSqlUserDefinedServerRole()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedserverrole.authorizer.aspx">UserDefinedServerRole.Authorizer</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> Authorizer 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UserDefinedServerRole.Authorizer).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlObjectAuthorizerElement(ri)));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:UserDefinedType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.aspx">UserDefinedType</see>
	///
	public partial class TSqlUserDefinedTypeReference : TSqlUserDefinedType, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlUserDefinedTypeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:UserDefinedType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.aspx">UserDefinedType</see>
	///
	public partial class TSqlUserDefinedType : TSqlModelElement,ISqlSecurable,ISqlDataType	{
		private static ModelTypeClass typeClass = UserDefinedType.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlUserDefinedType to adapt instances of <see cref="T:UserDefinedType"/>
		/// </summary>
		public TSqlUserDefinedType(TSqlObject obj)  : base(obj, UserDefinedType.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlUserDefinedType to adapt instances of <see cref="T:UserDefinedType"/>
		/// </summary>
		protected TSqlUserDefinedType()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:UserDefinedType.ByteOrdered"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.byteordered.aspx">UserDefinedType.ByteOrdered</see>
		///
		public Boolean? ByteOrdered 
		{
			get { return Element.GetProperty<Boolean?>(UserDefinedType.ByteOrdered);}
		}


		///
		/// Property wrapper for <see cref="M:UserDefinedType.ClassName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.classname.aspx">UserDefinedType.ClassName</see>
		///
		public String ClassName 
		{
			get { return Element.GetProperty<String>(UserDefinedType.ClassName);}
		}


		///
		/// Property wrapper for <see cref="M:UserDefinedType.FixedLength"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.fixedlength.aspx">UserDefinedType.FixedLength</see>
		///
		public Boolean? FixedLength 
		{
			get { return Element.GetProperty<Boolean?>(UserDefinedType.FixedLength);}
		}


		///
		/// Property wrapper for <see cref="M:UserDefinedType.Format"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.format.aspx">UserDefinedType.Format</see>
		///
		public Format Format 
		{
			get { return Element.GetProperty<Format>(UserDefinedType.Format);}
		}


		///
		/// Property wrapper for <see cref="M:UserDefinedType.MaxByteSize"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.maxbytesize.aspx">UserDefinedType.MaxByteSize</see>
		///
		public Int32? MaxByteSize 
		{
			get { return Element.GetProperty<Int32?>(UserDefinedType.MaxByteSize);}
		}


		///
		/// Property wrapper for <see cref="M:UserDefinedType.ValidationMethodName"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.validationmethodname.aspx">UserDefinedType.ValidationMethodName</see>
		///
		public String ValidationMethodName 
		{
			get { return Element.GetProperty<String>(UserDefinedType.ValidationMethodName);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.assembly.aspx">UserDefinedType.Assembly</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference> Assembly 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UserDefinedType.Assembly).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlAssemblyReference(o, Microsoft.SqlServer.Dac.Model.Assembly.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.methods.aspx">UserDefinedType.Methods</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> Methods 
		{
			get 
			{
				return Element.GetReferenced(UserDefinedType.Methods).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.properties.aspx">UserDefinedType.Properties</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> Properties 
		{
			get 
			{
				return Element.GetReferenced(UserDefinedType.Properties).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.userdefinedtype.schema.aspx">UserDefinedType.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(UserDefinedType.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:View"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.aspx">View</see>
	///
	public partial class TSqlViewReference : TSqlView, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlViewReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:View"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.aspx">View</see>
	///
	public partial class TSqlView : TSqlModelElement,ISqlColumnSource,ISpecifiesIndex	{
		private static ModelTypeClass typeClass = View.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlView to adapt instances of <see cref="T:View"/>
		/// </summary>
		public TSqlView(TSqlObject obj)  : base(obj, View.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlView to adapt instances of <see cref="T:View"/>
		/// </summary>
		protected TSqlView()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:View.AnsiNullsOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.ansinullson.aspx">View.AnsiNullsOn</see>
		///
		public Boolean? AnsiNullsOn 
		{
			get { return Element.GetProperty<Boolean?>(View.AnsiNullsOn);}
		}


		///
		/// Property wrapper for <see cref="M:View.QuotedIdentifierOn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.quotedidentifieron.aspx">View.QuotedIdentifierOn</see>
		///
		public Boolean? QuotedIdentifierOn 
		{
			get { return Element.GetProperty<Boolean?>(View.QuotedIdentifierOn);}
		}


		///
		/// Property wrapper for <see cref="M:View.Replicated"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.replicated.aspx">View.Replicated</see>
		///
		public Boolean Replicated 
		{
			get { return Element.GetProperty<Boolean>(View.Replicated);}
		}


		///
		/// Property wrapper for <see cref="M:View.SelectStatement"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.selectstatement.aspx">View.SelectStatement</see>
		///
		public String SelectStatement 
		{
			get { return (String)Element.GetProperty(View.SelectStatement);}
		}


		///
		/// Property wrapper for <see cref="M:View.WithCheckOption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.withcheckoption.aspx">View.WithCheckOption</see>
		///
		public Boolean WithCheckOption 
		{
			get { return Element.GetProperty<Boolean>(View.WithCheckOption);}
		}


		///
		/// Property wrapper for <see cref="M:View.WithEncryption"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.withencryption.aspx">View.WithEncryption</see>
		///
		public Boolean WithEncryption 
		{
			get { return Element.GetProperty<Boolean>(View.WithEncryption);}
		}


		///
		/// Property wrapper for <see cref="M:View.WithSchemaBinding"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.withschemabinding.aspx">View.WithSchemaBinding</see>
		///
		public Boolean WithSchemaBinding 
		{
			get { return Element.GetProperty<Boolean>(View.WithSchemaBinding);}
		}


		///
		/// Property wrapper for <see cref="M:View.WithViewMetadata"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.withviewmetadata.aspx">View.WithViewMetadata</see>
		///
		public Boolean WithViewMetadata 
		{
			get { return Element.GetProperty<Boolean>(View.WithViewMetadata);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.bodydependencies.aspx">View.BodyDependencies</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> BodyDependencies 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(View.BodyDependencies).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference(ri)));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.columns.aspx">View.Columns</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn> Columns 
		{
			get 
			{
				return Element.GetReferenced(View.Columns).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumn(o));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.view.schema.aspx">View.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(View.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:WorkloadGroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.aspx">WorkloadGroup</see>
	///
	public partial class TSqlWorkloadGroupReference : TSqlWorkloadGroup, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlWorkloadGroupReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:WorkloadGroup"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.aspx">WorkloadGroup</see>
	///
	public partial class TSqlWorkloadGroup : TSqlModelElement	{
		private static ModelTypeClass typeClass = WorkloadGroup.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlWorkloadGroup to adapt instances of <see cref="T:WorkloadGroup"/>
		/// </summary>
		public TSqlWorkloadGroup(TSqlObject obj)  : base(obj, WorkloadGroup.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlWorkloadGroup to adapt instances of <see cref="T:WorkloadGroup"/>
		/// </summary>
		protected TSqlWorkloadGroup()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:WorkloadGroup.GroupMaxRequests"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.groupmaxrequests.aspx">WorkloadGroup.GroupMaxRequests</see>
		///
		public Int32 GroupMaxRequests 
		{
			get { return Element.GetProperty<Int32>(WorkloadGroup.GroupMaxRequests);}
		}


		///
		/// Property wrapper for <see cref="M:WorkloadGroup.Importance"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.importance.aspx">WorkloadGroup.Importance</see>
		///
		public Degree Importance 
		{
			get { return Element.GetProperty<Degree>(WorkloadGroup.Importance);}
		}


		///
		/// Property wrapper for <see cref="M:WorkloadGroup.MaxDop"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.maxdop.aspx">WorkloadGroup.MaxDop</see>
		///
		public Int32 MaxDop 
		{
			get { return Element.GetProperty<Int32>(WorkloadGroup.MaxDop);}
		}


		///
		/// Property wrapper for <see cref="M:WorkloadGroup.RequestMaxCpuTimeSec"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.requestmaxcputimesec.aspx">WorkloadGroup.RequestMaxCpuTimeSec</see>
		///
		public Int32 RequestMaxCpuTimeSec 
		{
			get { return Element.GetProperty<Int32>(WorkloadGroup.RequestMaxCpuTimeSec);}
		}


		///
		/// Property wrapper for <see cref="M:WorkloadGroup.RequestMaxMemoryGrantPercent"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.requestmaxmemorygrantpercent.aspx">WorkloadGroup.RequestMaxMemoryGrantPercent</see>
		///
		public Int32 RequestMaxMemoryGrantPercent 
		{
			get { return Element.GetProperty<Int32>(WorkloadGroup.RequestMaxMemoryGrantPercent);}
		}


		///
		/// Property wrapper for <see cref="M:WorkloadGroup.RequestMemoryGrantTimeoutSec"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.requestmemorygranttimeoutsec.aspx">WorkloadGroup.RequestMemoryGrantTimeoutSec</see>
		///
		public Int32 RequestMemoryGrantTimeoutSec 
		{
			get { return Element.GetProperty<Int32>(WorkloadGroup.RequestMemoryGrantTimeoutSec);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlResourcePoolReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.workloadgroup.resourcepool.aspx">WorkloadGroup.ResourcePool</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlResourcePoolReference> ResourcePool 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(WorkloadGroup.ResourcePool).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlResourcePoolReference(o, Microsoft.SqlServer.Dac.Model.ResourcePool.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:XmlIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.aspx">XmlIndex</see>
	///
	public partial class TSqlXmlIndexReference : TSqlXmlIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlXmlIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:XmlIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.aspx">XmlIndex</see>
	///
	public partial class TSqlXmlIndex : TSqlModelElement,ISqlIndex	{
		private static ModelTypeClass typeClass = XmlIndex.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlXmlIndex to adapt instances of <see cref="T:XmlIndex"/>
		/// </summary>
		public TSqlXmlIndex(TSqlObject obj)  : base(obj, XmlIndex.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlXmlIndex to adapt instances of <see cref="T:XmlIndex"/>
		/// </summary>
		protected TSqlXmlIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:XmlIndex.AllowPageLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.allowpagelocks.aspx">XmlIndex.AllowPageLocks</see>
		///
		public Boolean AllowPageLocks 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.AllowPageLocks);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.AllowRowLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.allowrowlocks.aspx">XmlIndex.AllowRowLocks</see>
		///
		public Boolean AllowRowLocks 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.AllowRowLocks);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.disabled.aspx">XmlIndex.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.FillFactor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.fillfactor.aspx">XmlIndex.FillFactor</see>
		///
		public Int32? FillFactor 
		{
			get { return Element.GetProperty<Int32?>(XmlIndex.FillFactor);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.ignoreduplicatekey.aspx">XmlIndex.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.IgnoreDuplicateKey);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.IsPrimary"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.isprimary.aspx">XmlIndex.IsPrimary</see>
		///
		public Boolean IsPrimary 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.IsPrimary);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.RecomputeStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.recomputestatistics.aspx">XmlIndex.RecomputeStatistics</see>
		///
		public Boolean RecomputeStatistics 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.RecomputeStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.SecondaryXmlIndexType"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.secondaryxmlindextype.aspx">XmlIndex.SecondaryXmlIndexType</see>
		///
		public SecondaryXmlIndexType SecondaryXmlIndexType 
		{
			get { return Element.GetProperty<SecondaryXmlIndexType>(XmlIndex.SecondaryXmlIndexType);}
		}


		///
		/// Property wrapper for <see cref="M:XmlIndex.WithPadIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.withpadindex.aspx">XmlIndex.WithPadIndex</see>
		///
		public Boolean WithPadIndex 
		{
			get { return Element.GetProperty<Boolean>(XmlIndex.WithPadIndex);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.column.aspx">XmlIndex.Column</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Column 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(XmlIndex.Column).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.indexedobject.aspx">XmlIndex.IndexedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> IndexedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(XmlIndex.IndexedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISpecifiesIndexElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlIndexReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlindex.primaryxmlindex.aspx">XmlIndex.PrimaryXmlIndex</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlIndexReference> PrimaryXmlIndex 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(XmlIndex.PrimaryXmlIndex).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlIndexReference(o, Microsoft.SqlServer.Dac.Model.XmlIndex.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SelectiveXmlIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.aspx">SelectiveXmlIndex</see>
	///
	public partial class TSqlSelectiveXmlIndexReference : TSqlSelectiveXmlIndex, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlSelectiveXmlIndexReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:SelectiveXmlIndex"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.aspx">SelectiveXmlIndex</see>
	///
	public partial class TSqlSelectiveXmlIndex : TSqlModelElement,ISqlIndex	{
		private static ModelTypeClass typeClass = SelectiveXmlIndex.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlSelectiveXmlIndex to adapt instances of <see cref="T:SelectiveXmlIndex"/>
		/// </summary>
		public TSqlSelectiveXmlIndex(TSqlObject obj)  : base(obj, SelectiveXmlIndex.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlSelectiveXmlIndex to adapt instances of <see cref="T:SelectiveXmlIndex"/>
		/// </summary>
		protected TSqlSelectiveXmlIndex()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.AllowPageLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.allowpagelocks.aspx">SelectiveXmlIndex.AllowPageLocks</see>
		///
		public Boolean AllowPageLocks 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.AllowPageLocks);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.AllowRowLocks"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.allowrowlocks.aspx">SelectiveXmlIndex.AllowRowLocks</see>
		///
		public Boolean AllowRowLocks 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.AllowRowLocks);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.Disabled"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.disabled.aspx">SelectiveXmlIndex.Disabled</see>
		///
		public Boolean Disabled 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.Disabled);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.FillFactor"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.fillfactor.aspx">SelectiveXmlIndex.FillFactor</see>
		///
		public Int32? FillFactor 
		{
			get { return Element.GetProperty<Int32?>(SelectiveXmlIndex.FillFactor);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.IgnoreDuplicateKey"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.ignoreduplicatekey.aspx">SelectiveXmlIndex.IgnoreDuplicateKey</see>
		///
		public Boolean IgnoreDuplicateKey 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.IgnoreDuplicateKey);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.IsPrimary"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.isprimary.aspx">SelectiveXmlIndex.IsPrimary</see>
		///
		public Boolean IsPrimary 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.IsPrimary);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.RecomputeStatistics"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.recomputestatistics.aspx">SelectiveXmlIndex.RecomputeStatistics</see>
		///
		public Boolean RecomputeStatistics 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.RecomputeStatistics);}
		}


		///
		/// Property wrapper for <see cref="M:SelectiveXmlIndex.WithPadIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.withpadindex.aspx">SelectiveXmlIndex.WithPadIndex</see>
		///
		public Boolean WithPadIndex 
		{
			get { return Element.GetProperty<Boolean>(SelectiveXmlIndex.WithPadIndex);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.column.aspx">SelectiveXmlIndex.Column</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference> Column 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SelectiveXmlIndex.Column).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference(o, Microsoft.SqlServer.Dac.Model.Column.TypeClass));
			}
		}

		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.indexedobject.aspx">SelectiveXmlIndex.IndexedObject</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> IndexedObject 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SelectiveXmlIndex.IndexedObject).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISpecifiesIndexElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.primarypromotedpath.aspx">SelectiveXmlIndex.PrimaryPromotedPath</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> PrimaryPromotedPath 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SelectiveXmlIndex.PrimaryPromotedPath).Select(o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath)TSqlModelElement.AdaptInstance(o, (ri) => new Microsoft.SqlServer.Dac.Extensions.Prototype.UnresolvedISqlPromotedNodePathElement(ri)));
			}
		}

		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSelectiveXmlIndexReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.primaryselectivexmlindex.aspx">SelectiveXmlIndex.PrimarySelectiveXmlIndex</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSelectiveXmlIndexReference> PrimarySelectiveXmlIndex 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(SelectiveXmlIndex.PrimarySelectiveXmlIndex).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSelectiveXmlIndexReference(o, Microsoft.SqlServer.Dac.Model.SelectiveXmlIndex.TypeClass));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.promotedpaths.aspx">SelectiveXmlIndex.PromotedPaths</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> PromotedPaths 
		{
			get 
			{
				return Element.GetReferenced(SelectiveXmlIndex.PromotedPaths).Select( o => (Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath)TSqlModelElement.AdaptInstance(o));
			}
		}

		///
		/// Composing relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlNamespace"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.selectivexmlindex.xmlnamespaces.aspx">SelectiveXmlIndex.XmlNamespaces</see>
		/// Relationship Type:Composing
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlNamespace> XmlNamespaces 
		{
			get 
			{
				return Element.GetReferenced(SelectiveXmlIndex.XmlNamespaces).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlXmlNamespace(o));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:XmlNamespace"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlnamespace.aspx">XmlNamespace</see>
	///
	public partial class TSqlXmlNamespaceReference : TSqlXmlNamespace, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlXmlNamespaceReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:XmlNamespace"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlnamespace.aspx">XmlNamespace</see>
	///
	public partial class TSqlXmlNamespace : TSqlModelElement	{
		private static ModelTypeClass typeClass = XmlNamespace.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlXmlNamespace to adapt instances of <see cref="T:XmlNamespace"/>
		/// </summary>
		public TSqlXmlNamespace(TSqlObject obj)  : base(obj, XmlNamespace.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlXmlNamespace to adapt instances of <see cref="T:XmlNamespace"/>
		/// </summary>
		protected TSqlXmlNamespace()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:XmlNamespace.NamespaceUri"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlnamespace.namespaceuri.aspx">XmlNamespace.NamespaceUri</see>
		///
		public String NamespaceUri 
		{
			get { return Element.GetProperty<String>(XmlNamespace.NamespaceUri);}
		}


		///
		/// Property wrapper for <see cref="M:XmlNamespace.Prefix"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlnamespace.prefix.aspx">XmlNamespace.Prefix</see>
		///
		public String Prefix 
		{
			get { return Element.GetProperty<String>(XmlNamespace.Prefix);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PromotedNodePathForXQueryType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforxquerytype.aspx">PromotedNodePathForXQueryType</see>
	///
	public partial class TSqlPromotedNodePathForXQueryTypeReference : TSqlPromotedNodePathForXQueryType, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPromotedNodePathForXQueryTypeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PromotedNodePathForXQueryType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforxquerytype.aspx">PromotedNodePathForXQueryType</see>
	///
	public partial class TSqlPromotedNodePathForXQueryType : TSqlModelElement,ISqlPromotedNodePath	{
		private static ModelTypeClass typeClass = PromotedNodePathForXQueryType.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPromotedNodePathForXQueryType to adapt instances of <see cref="T:PromotedNodePathForXQueryType"/>
		/// </summary>
		public TSqlPromotedNodePathForXQueryType(TSqlObject obj)  : base(obj, PromotedNodePathForXQueryType.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPromotedNodePathForXQueryType to adapt instances of <see cref="T:PromotedNodePathForXQueryType"/>
		/// </summary>
		protected TSqlPromotedNodePathForXQueryType()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:PromotedNodePathForXQueryType.IsSingleton"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforxquerytype.issingleton.aspx">PromotedNodePathForXQueryType.IsSingleton</see>
		///
		public Boolean IsSingleton 
		{
			get { return Element.GetProperty<Boolean>(PromotedNodePathForXQueryType.IsSingleton);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForXQueryType.MaxLength"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforxquerytype.maxlength.aspx">PromotedNodePathForXQueryType.MaxLength</see>
		///
		public Int32? MaxLength 
		{
			get { return Element.GetProperty<Int32?>(PromotedNodePathForXQueryType.MaxLength);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForXQueryType.NodePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforxquerytype.nodepath.aspx">PromotedNodePathForXQueryType.NodePath</see>
		///
		public String NodePath 
		{
			get { return Element.GetProperty<String>(PromotedNodePathForXQueryType.NodePath);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForXQueryType.Type"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforxquerytype.type.aspx">PromotedNodePathForXQueryType.Type</see>
		///
		public String Type 
		{
			get { return Element.GetProperty<String>(PromotedNodePathForXQueryType.Type);}
		}


}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PromotedNodePathForSqlType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.aspx">PromotedNodePathForSqlType</see>
	///
	public partial class TSqlPromotedNodePathForSqlTypeReference : TSqlPromotedNodePathForSqlType, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlPromotedNodePathForSqlTypeReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:PromotedNodePathForSqlType"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.aspx">PromotedNodePathForSqlType</see>
	///
	public partial class TSqlPromotedNodePathForSqlType : TSqlModelElement,ISqlPromotedNodePath	{
		private static ModelTypeClass typeClass = PromotedNodePathForSqlType.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlPromotedNodePathForSqlType to adapt instances of <see cref="T:PromotedNodePathForSqlType"/>
		/// </summary>
		public TSqlPromotedNodePathForSqlType(TSqlObject obj)  : base(obj, PromotedNodePathForSqlType.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlPromotedNodePathForSqlType to adapt instances of <see cref="T:PromotedNodePathForSqlType"/>
		/// </summary>
		protected TSqlPromotedNodePathForSqlType()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:PromotedNodePathForSqlType.IsMax"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.ismax.aspx">PromotedNodePathForSqlType.IsMax</see>
		///
		public Boolean IsMax 
		{
			get { return Element.GetProperty<Boolean>(PromotedNodePathForSqlType.IsMax);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForSqlType.IsSingleton"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.issingleton.aspx">PromotedNodePathForSqlType.IsSingleton</see>
		///
		public Boolean IsSingleton 
		{
			get { return Element.GetProperty<Boolean>(PromotedNodePathForSqlType.IsSingleton);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForSqlType.Length"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.length.aspx">PromotedNodePathForSqlType.Length</see>
		///
		public Int32 Length 
		{
			get { return Element.GetProperty<Int32>(PromotedNodePathForSqlType.Length);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForSqlType.NodePath"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.nodepath.aspx">PromotedNodePathForSqlType.NodePath</see>
		///
		public String NodePath 
		{
			get { return Element.GetProperty<String>(PromotedNodePathForSqlType.NodePath);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForSqlType.Precision"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.precision.aspx">PromotedNodePathForSqlType.Precision</see>
		///
		public Int32 Precision 
		{
			get { return Element.GetProperty<Int32>(PromotedNodePathForSqlType.Precision);}
		}


		///
		/// Property wrapper for <see cref="M:PromotedNodePathForSqlType.Scale"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.scale.aspx">PromotedNodePathForSqlType.Scale</see>
		///
		public Int32 Scale 
		{
			get { return Element.GetProperty<Int32>(PromotedNodePathForSqlType.Scale);}
		}


		///
		/// Peer relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.promotednodepathforsqltype.datatype.aspx">PromotedNodePathForSqlType.DataType</see>
		/// Relationship Type:Peer
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference> DataType 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(PromotedNodePathForSqlType.DataType).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlDataTypeReference(o, Microsoft.SqlServer.Dac.Model.DataType.TypeClass));
			}
		}

}


    ///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:XmlSchemaCollection"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlschemacollection.aspx">XmlSchemaCollection</see>
	///
	public partial class TSqlXmlSchemaCollectionReference : TSqlXmlSchemaCollection, ISqlModelElementReference
	{		
        private ModelRelationshipInstance relationshipInstance;
        private ModelTypeClass predefinedTypeClass;
        public TSqlXmlSchemaCollectionReference (ModelRelationshipInstance relationshipReference, ModelTypeClass typeClass)
		{
            relationshipInstance = relationshipReference;
            if (relationshipInstance.Object != null && relationshipInstance.Object.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                ModelMessages.InvalidObjectType, relationshipInstance.Object.ObjectType.Name, typeClass.Name),
                "typeClass");
            }
            predefinedTypeClass = typeClass;
        }

        public override ObjectIdentifier Name
        {
            get
            {
                return relationshipInstance.ObjectName;
            }
        }

        public override ModelTypeClass ObjectType
        {
            get
            {
                if (IsResolved())
                {
                    return base.ObjectType;
                }
                else
                {
                    // when object is unresolved default to the predefined ModelTypClass
                    return predefinedTypeClass;
                }
            }
        }

        public bool IsResolved()
        {
            return relationshipInstance.Object != null;
        }

        public override TSqlObject Element
        {
            get
            {
                // Verify the Element is resolved.
                if (!IsResolved())
                {
                    throw new UnresolvedElementException(
                       string.Format(CultureInfo.CurrentUICulture,
                       ModelMessages.UnresolvedObject,
                       relationshipInstance.ObjectName));
                }
                return relationshipInstance.Object;
            }
        }
        
        public T GetMetadataProperty<T>(ModelPropertyClass property)
        {
            return relationshipInstance.GetProperty<T>(property);
        }
	}

	///
	/// Adapter class for instances of <see cref="T:TSqlObject"/> with an <see cref="T:TSqlObject M:ObjectType"> equal to <see cref="T:XmlSchemaCollection"/>
	///
	/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlschemacollection.aspx">XmlSchemaCollection</see>
	///
	public partial class TSqlXmlSchemaCollection : TSqlModelElement,ISqlSecurable	{
		private static ModelTypeClass typeClass = XmlSchemaCollection.TypeClass;

		/// <summary>
		///	Create a strongly-typed class TSqlXmlSchemaCollection to adapt instances of <see cref="T:XmlSchemaCollection"/>
		/// </summary>
		public TSqlXmlSchemaCollection(TSqlObject obj)  : base(obj, XmlSchemaCollection.TypeClass)
		{
		}


		/// <summary>
		///	Create a strongly-typed class TSqlXmlSchemaCollection to adapt instances of <see cref="T:XmlSchemaCollection"/>
		/// </summary>
		protected TSqlXmlSchemaCollection()
		{
		}

		public static ModelTypeClass TypeClass 
		{
			get { return typeClass;}
		}

		///
		/// Property wrapper for <see cref="M:XmlSchemaCollection.Expression"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlschemacollection.expression.aspx">XmlSchemaCollection.Expression</see>
		///
		public String Expression 
		{
			get { return (String)Element.GetProperty(XmlSchemaCollection.Expression);}
		}


		///
		/// Hierarchical relationship returning instances of <see cref=":T Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference"/>
		/// <see href="http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.dac.model.xmlschemacollection.schema.aspx">XmlSchemaCollection.Schema</see>
		/// Relationship Type:Hierarchical
		public IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference> Schema 
		{
			get 
			{
				return Element.GetReferencedRelationshipInstances(XmlSchemaCollection.Schema).Select(o => new Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlSchemaReference(o, Microsoft.SqlServer.Dac.Model.Schema.TypeClass));
			}
		}

}


    // Derived Types with Metadata properties.

    public partial class ClrTableOptionOrderColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  ClrTableOptionOrderColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:ClrTableOption.OrderColumns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(ClrTableOption.OrderColumnsRelationship.Ascending);}
    	}

    }

    public partial class IndexColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  IndexColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:Index.Columns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(Index.ColumnsRelationship.Ascending);}
    	}

    }

    public partial class ColumnStoreIndexColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  ColumnStoreIndexColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:ColumnStoreIndex.Columns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(ColumnStoreIndex.ColumnsRelationship.Ascending);}
    	}

    }

    public partial class ContractMessagesReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlMessageTypeReference
    {
        public  ContractMessagesReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:Contract.Messages"/>		
	    ///
	    public Boolean Default 
	    {
            get { return this.GetMetadataProperty<Boolean>(Contract.MessagesRelationship.Default);}
    	}
        ///
	    /// Metadata property wrapper for <see cref="M:Contract.Messages"/>		
	    ///
	    public Int32 SentBy 
	    {
            get { return this.GetMetadataProperty<Int32>(Contract.MessagesRelationship.SentBy);}
    	}

    }

    public partial class PartitionSchemeFilegroupsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlFilegroupReference
    {
        public  PartitionSchemeFilegroupsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:PartitionScheme.Filegroups"/>		
	    ///
	    public Boolean IsDefault 
	    {
            get { return this.GetMetadataProperty<Boolean>(PartitionScheme.FilegroupsRelationship.IsDefault);}
    	}

    }

    public partial class PrimaryKeyConstraintColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  PrimaryKeyConstraintColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:PrimaryKeyConstraint.Columns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(PrimaryKeyConstraint.ColumnsRelationship.Ascending);}
    	}

    }

    public partial class TableTypePrimaryKeyConstraintColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  TableTypePrimaryKeyConstraintColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:TableTypePrimaryKeyConstraint.Columns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(TableTypePrimaryKeyConstraint.ColumnsRelationship.Ascending);}
    	}

    }

    public partial class TableTypeUniqueConstraintColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  TableTypeUniqueConstraintColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:TableTypeUniqueConstraint.Columns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(TableTypeUniqueConstraint.ColumnsRelationship.Ascending);}
    	}

    }

    public partial class UniqueConstraintColumnsReference : Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlColumnReference
    {
        public  UniqueConstraintColumnsReference (ModelRelationshipInstance relationshipReference,  ModelTypeClass typeClass) : base(relationshipReference, typeClass) { }

        ///
	    /// Metadata property wrapper for <see cref="M:UniqueConstraint.Columns"/>		
	    ///
	    public Boolean Ascending 
	    {
            get { return this.GetMetadataProperty<Boolean>(UniqueConstraint.ColumnsRelationship.Ascending);}
    	}

    }
}