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
	using System.Collections.Generic;
	using System.Globalization;
	using Microsoft.SqlServer.Dac.Model;
	    
	public static class UtilityMethods
	{

		/// <summary>
        /// Returns the set of possible <see cref="ModelTypeClass"/> for the <see cref="System.Type"/>.
        /// </summary>
        /// <param name="type">Class or Interface to find mapped <see cref="ModelTypeClass"/></param>
        /// <returns>The <see cref="ModelTypeClass"/> that map to the <paramref name="type"/> <see cref="ModelTypeClass"/></returns>
        /// <remarks>
        /// if <paramref name="type"/> is an interface the the returned <see cref="ModelTypeClass"/> all implement the type. If the <paramref name="type"/> is class type on a single <see cref="ModelTypeClass"/> will be returned.
        /// </remarks>
		public static IEnumerable<ModelTypeClass> GetModelElementTypes(Type type)
		{
			if(type.Namespace != typeof(TSqlTable).Namespace)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The type {0}.{1} must be in the namespace {2}", type.Namespace, type.Name, typeof(TSqlTable).Namespace) , "type");
			}
			switch(type.Name)
			{
				case "ISql90TSqlAggregate":
				case "ISql100TSqlAggregate":
				case "ISql110TSqlAggregate":
				case "ISql120TSqlAggregate":
				case "ISqlAzureV12TSqlAggregate":
				case "TSqlAggregate":				
					yield return TSqlAggregate.TypeClass;
					break;
				case "ISql90TSqlApplicationRole":
				case "ISql100TSqlApplicationRole":
				case "ISql110TSqlApplicationRole":
				case "ISql120TSqlApplicationRole":
				case "ISqlAzureV12TSqlApplicationRole":
				case "TSqlApplicationRole":				
					yield return TSqlApplicationRole.TypeClass;
					break;
				case "ISql90TSqlAssembly":
				case "ISql100TSqlAssembly":
				case "ISqlAzureTSqlAssembly":
				case "ISql110TSqlAssembly":
				case "ISql120TSqlAssembly":
				case "ISqlAzureV12TSqlAssembly":
				case "TSqlAssembly":				
					yield return TSqlAssembly.TypeClass;
					break;
				case "ISql90TSqlAssemblySource":
				case "ISql100TSqlAssemblySource":
				case "ISqlAzureTSqlAssemblySource":
				case "ISql110TSqlAssemblySource":
				case "ISql120TSqlAssemblySource":
				case "ISqlAzureV12TSqlAssemblySource":
				case "TSqlAssemblySource":				
					yield return TSqlAssemblySource.TypeClass;
					break;
				case "ISql90TSqlAsymmetricKey":
				case "ISql100TSqlAsymmetricKey":
				case "ISql110TSqlAsymmetricKey":
				case "ISql120TSqlAsymmetricKey":
				case "TSqlAsymmetricKey":				
					yield return TSqlAsymmetricKey.TypeClass;
					break;
				case "ISql100TSqlAuditAction":
				case "ISql110TSqlAuditAction":
				case "ISql120TSqlAuditAction":
				case "TSqlAuditAction":				
					yield return TSqlAuditAction.TypeClass;
					break;
				case "ISql100TSqlAuditActionGroup":
				case "ISql110TSqlAuditActionGroup":
				case "ISql120TSqlAuditActionGroup":
				case "TSqlAuditActionGroup":				
					yield return TSqlAuditActionGroup.TypeClass;
					break;
				case "ISql100TSqlAuditActionSpecification":
				case "ISql110TSqlAuditActionSpecification":
				case "ISql120TSqlAuditActionSpecification":
				case "TSqlAuditActionSpecification":				
					yield return TSqlAuditActionSpecification.TypeClass;
					break;
				case "ISql100TSqlBrokerPriority":
				case "ISql110TSqlBrokerPriority":
				case "ISql120TSqlBrokerPriority":
				case "TSqlBrokerPriority":				
					yield return TSqlBrokerPriority.TypeClass;
					break;
				case "ISql90TSqlBuiltInServerRole":
				case "ISql100TSqlBuiltInServerRole":
				case "ISqlAzureTSqlBuiltInServerRole":
				case "ISql110TSqlBuiltInServerRole":
				case "ISql120TSqlBuiltInServerRole":
				case "ISqlAzureV12TSqlBuiltInServerRole":
				case "TSqlBuiltInServerRole":				
					yield return TSqlBuiltInServerRole.TypeClass;
					break;
				case "ISql90TSqlCertificate":
				case "ISql100TSqlCertificate":
				case "ISql110TSqlCertificate":
				case "ISql120TSqlCertificate":
				case "TSqlCertificate":				
					yield return TSqlCertificate.TypeClass;
					break;
				case "ISql90TSqlCheckConstraint":
				case "ISql100TSqlCheckConstraint":
				case "ISqlAzureTSqlCheckConstraint":
				case "ISql110TSqlCheckConstraint":
				case "ISql120TSqlCheckConstraint":
				case "ISqlAzureV12TSqlCheckConstraint":
				case "TSqlCheckConstraint":				
					yield return TSqlCheckConstraint.TypeClass;
					break;
				case "ISql90TSqlClrTableOption":
				case "ISql100TSqlClrTableOption":
				case "ISql110TSqlClrTableOption":
				case "ISql120TSqlClrTableOption":
				case "ISqlAzureV12TSqlClrTableOption":
				case "TSqlClrTableOption":				
					yield return TSqlClrTableOption.TypeClass;
					break;
				case "ISql90TSqlClrTypeMethod":
				case "ISql100TSqlClrTypeMethod":
				case "ISqlAzureTSqlClrTypeMethod":
				case "ISql110TSqlClrTypeMethod":
				case "ISql120TSqlClrTypeMethod":
				case "ISqlAzureV12TSqlClrTypeMethod":
				case "TSqlClrTypeMethod":				
					yield return TSqlClrTypeMethod.TypeClass;
					break;
				case "ISql90TSqlClrTypeMethodParameter":
				case "ISql100TSqlClrTypeMethodParameter":
				case "ISqlAzureTSqlClrTypeMethodParameter":
				case "ISql110TSqlClrTypeMethodParameter":
				case "ISql120TSqlClrTypeMethodParameter":
				case "ISqlAzureV12TSqlClrTypeMethodParameter":
				case "TSqlClrTypeMethodParameter":				
					yield return TSqlClrTypeMethodParameter.TypeClass;
					break;
				case "ISql90TSqlClrTypeProperty":
				case "ISql100TSqlClrTypeProperty":
				case "ISqlAzureTSqlClrTypeProperty":
				case "ISql110TSqlClrTypeProperty":
				case "ISql120TSqlClrTypeProperty":
				case "ISqlAzureV12TSqlClrTypeProperty":
				case "TSqlClrTypeProperty":				
					yield return TSqlClrTypeProperty.TypeClass;
					break;
				case "ISql90TSqlColumn":
				case "ISql100TSqlColumn":
				case "ISqlAzureTSqlColumn":
				case "ISql110TSqlColumn":
				case "ISql120TSqlColumn":
				case "ISqlAzureV12TSqlColumn":
				case "TSqlColumn":				
					yield return TSqlColumn.TypeClass;
					break;
				case "ISql110TSqlColumnStoreIndex":
				case "ISql120TSqlColumnStoreIndex":
				case "ISqlAzureV12TSqlColumnStoreIndex":
				case "TSqlColumnStoreIndex":				
					yield return TSqlColumnStoreIndex.TypeClass;
					break;
				case "ISql90TSqlContract":
				case "ISql100TSqlContract":
				case "ISql110TSqlContract":
				case "ISql120TSqlContract":
				case "TSqlContract":				
					yield return TSqlContract.TypeClass;
					break;
				case "ISql90TSqlCredential":
				case "ISql100TSqlCredential":
				case "ISql110TSqlCredential":
				case "ISql120TSqlCredential":
				case "TSqlCredential":				
					yield return TSqlCredential.TypeClass;
					break;
				case "ISql100TSqlCryptographicProvider":
				case "ISql110TSqlCryptographicProvider":
				case "ISql120TSqlCryptographicProvider":
				case "TSqlCryptographicProvider":				
					yield return TSqlCryptographicProvider.TypeClass;
					break;
				case "ISql100TSqlDatabaseAuditSpecification":
				case "ISql110TSqlDatabaseAuditSpecification":
				case "ISql120TSqlDatabaseAuditSpecification":
				case "TSqlDatabaseAuditSpecification":				
					yield return TSqlDatabaseAuditSpecification.TypeClass;
					break;
				case "ISql90TSqlDatabaseDdlTrigger":
				case "ISql100TSqlDatabaseDdlTrigger":
				case "ISqlAzureTSqlDatabaseDdlTrigger":
				case "ISql110TSqlDatabaseDdlTrigger":
				case "ISql120TSqlDatabaseDdlTrigger":
				case "ISqlAzureV12TSqlDatabaseDdlTrigger":
				case "TSqlDatabaseDdlTrigger":				
					yield return TSqlDatabaseDdlTrigger.TypeClass;
					break;
				case "ISql100TSqlDatabaseEncryptionKey":
				case "ISql110TSqlDatabaseEncryptionKey":
				case "ISql120TSqlDatabaseEncryptionKey":
				case "TSqlDatabaseEncryptionKey":				
					yield return TSqlDatabaseEncryptionKey.TypeClass;
					break;
				case "ISql90TSqlDatabaseEventNotification":
				case "ISql100TSqlDatabaseEventNotification":
				case "ISql110TSqlDatabaseEventNotification":
				case "ISql120TSqlDatabaseEventNotification":
				case "TSqlDatabaseEventNotification":				
					yield return TSqlDatabaseEventNotification.TypeClass;
					break;
				case "ISql90TSqlDatabaseMirroringLanguageSpecifier":
				case "ISql100TSqlDatabaseMirroringLanguageSpecifier":
				case "ISql110TSqlDatabaseMirroringLanguageSpecifier":
				case "ISql120TSqlDatabaseMirroringLanguageSpecifier":
				case "TSqlDatabaseMirroringLanguageSpecifier":				
					yield return TSqlDatabaseMirroringLanguageSpecifier.TypeClass;
					break;
				case "ISql90TSqlDatabaseOptions":
				case "ISql100TSqlDatabaseOptions":
				case "ISqlAzureTSqlDatabaseOptions":
				case "ISql110TSqlDatabaseOptions":
				case "ISql120TSqlDatabaseOptions":
				case "ISqlAzureV12TSqlDatabaseOptions":
				case "TSqlDatabaseOptions":				
					yield return TSqlDatabaseOptions.TypeClass;
					break;
				case "ISql100TSqlDataCompressionOption":
				case "ISql110TSqlDataCompressionOption":
				case "ISql120TSqlDataCompressionOption":
				case "ISqlAzureV12TSqlDataCompressionOption":
				case "TSqlDataCompressionOption":				
					yield return TSqlDataCompressionOption.TypeClass;
					break;
				case "ISql90TSqlDataType":
				case "ISql100TSqlDataType":
				case "ISqlAzureTSqlDataType":
				case "ISql110TSqlDataType":
				case "ISql120TSqlDataType":
				case "ISqlAzureV12TSqlDataType":
				case "TSqlDataType":				
					yield return TSqlDataType.TypeClass;
					break;
				case "ISql90TSqlDefault":
				case "ISql100TSqlDefault":
				case "ISql110TSqlDefault":
				case "ISql120TSqlDefault":
				case "ISqlAzureV12TSqlDefault":
				case "TSqlDefault":				
					yield return TSqlDefault.TypeClass;
					break;
				case "ISql90TSqlDefaultConstraint":
				case "ISql100TSqlDefaultConstraint":
				case "ISqlAzureTSqlDefaultConstraint":
				case "ISql110TSqlDefaultConstraint":
				case "ISql120TSqlDefaultConstraint":
				case "ISqlAzureV12TSqlDefaultConstraint":
				case "TSqlDefaultConstraint":				
					yield return TSqlDefaultConstraint.TypeClass;
					break;
				case "ISql90TSqlDmlTrigger":
				case "ISql100TSqlDmlTrigger":
				case "ISqlAzureTSqlDmlTrigger":
				case "ISql110TSqlDmlTrigger":
				case "ISql120TSqlDmlTrigger":
				case "ISqlAzureV12TSqlDmlTrigger":
				case "TSqlDmlTrigger":				
					yield return TSqlDmlTrigger.TypeClass;
					break;
				case "ISql90TSqlEndpoint":
				case "ISql100TSqlEndpoint":
				case "ISql110TSqlEndpoint":
				case "ISql120TSqlEndpoint":
				case "TSqlEndpoint":				
					yield return TSqlEndpoint.TypeClass;
					break;
				case "ISql90TSqlErrorMessage":
				case "ISql100TSqlErrorMessage":
				case "ISql110TSqlErrorMessage":
				case "ISql120TSqlErrorMessage":
				case "TSqlErrorMessage":				
					yield return TSqlErrorMessage.TypeClass;
					break;
				case "ISql90TSqlEventGroup":
				case "ISql100TSqlEventGroup":
				case "ISqlAzureTSqlEventGroup":
				case "ISql110TSqlEventGroup":
				case "ISql120TSqlEventGroup":
				case "ISqlAzureV12TSqlEventGroup":
				case "TSqlEventGroup":				
					yield return TSqlEventGroup.TypeClass;
					break;
				case "ISql100TSqlEventSession":
				case "ISql110TSqlEventSession":
				case "ISql120TSqlEventSession":
				case "TSqlEventSession":				
					yield return TSqlEventSession.TypeClass;
					break;
				case "ISql100TSqlEventSessionAction":
				case "ISql110TSqlEventSessionAction":
				case "ISql120TSqlEventSessionAction":
				case "TSqlEventSessionAction":				
					yield return TSqlEventSessionAction.TypeClass;
					break;
				case "ISql100TSqlEventSessionDefinitions":
				case "ISql110TSqlEventSessionDefinitions":
				case "ISql120TSqlEventSessionDefinitions":
				case "TSqlEventSessionDefinitions":				
					yield return TSqlEventSessionDefinitions.TypeClass;
					break;
				case "ISql100TSqlEventSessionSetting":
				case "ISql110TSqlEventSessionSetting":
				case "ISql120TSqlEventSessionSetting":
				case "TSqlEventSessionSetting":				
					yield return TSqlEventSessionSetting.TypeClass;
					break;
				case "ISql100TSqlEventSessionTarget":
				case "ISql110TSqlEventSessionTarget":
				case "ISql120TSqlEventSessionTarget":
				case "TSqlEventSessionTarget":				
					yield return TSqlEventSessionTarget.TypeClass;
					break;
				case "ISql90TSqlEventTypeSpecifier":
				case "ISql100TSqlEventTypeSpecifier":
				case "ISqlAzureTSqlEventTypeSpecifier":
				case "ISql110TSqlEventTypeSpecifier":
				case "ISql120TSqlEventTypeSpecifier":
				case "ISqlAzureV12TSqlEventTypeSpecifier":
				case "TSqlEventTypeSpecifier":				
					yield return TSqlEventTypeSpecifier.TypeClass;
					break;
				case "ISql90TSqlExtendedProcedure":
				case "ISql100TSqlExtendedProcedure":
				case "ISql110TSqlExtendedProcedure":
				case "ISql120TSqlExtendedProcedure":
				case "TSqlExtendedProcedure":				
					yield return TSqlExtendedProcedure.TypeClass;
					break;
				case "ISql90TSqlExtendedProperty":
				case "ISql100TSqlExtendedProperty":
				case "ISql110TSqlExtendedProperty":
				case "ISql120TSqlExtendedProperty":
				case "ISqlAzureV12TSqlExtendedProperty":
				case "TSqlExtendedProperty":				
					yield return TSqlExtendedProperty.TypeClass;
					break;
				case "ISql90TSqlFilegroup":
				case "ISql100TSqlFilegroup":
				case "ISql110TSqlFilegroup":
				case "ISql120TSqlFilegroup":
				case "TSqlFilegroup":				
					yield return TSqlFilegroup.TypeClass;
					break;
				case "ISql110TSqlFileTable":
				case "ISql120TSqlFileTable":
				case "TSqlFileTable":				
					yield return TSqlFileTable.TypeClass;
					break;
				case "ISql90TSqlForeignKeyConstraint":
				case "ISql100TSqlForeignKeyConstraint":
				case "ISqlAzureTSqlForeignKeyConstraint":
				case "ISql110TSqlForeignKeyConstraint":
				case "ISql120TSqlForeignKeyConstraint":
				case "ISqlAzureV12TSqlForeignKeyConstraint":
				case "TSqlForeignKeyConstraint":				
					yield return TSqlForeignKeyConstraint.TypeClass;
					break;
				case "ISql90TSqlFullTextCatalog":
				case "ISql100TSqlFullTextCatalog":
				case "ISql110TSqlFullTextCatalog":
				case "ISql120TSqlFullTextCatalog":
				case "ISqlAzureV12TSqlFullTextCatalog":
				case "TSqlFullTextCatalog":				
					yield return TSqlFullTextCatalog.TypeClass;
					break;
				case "ISql90TSqlFullTextIndex":
				case "ISql100TSqlFullTextIndex":
				case "ISql110TSqlFullTextIndex":
				case "ISql120TSqlFullTextIndex":
				case "ISqlAzureV12TSqlFullTextIndex":
				case "TSqlFullTextIndex":				
					yield return TSqlFullTextIndex.TypeClass;
					break;
				case "ISql90TSqlFullTextIndexColumnSpecifier":
				case "ISql100TSqlFullTextIndexColumnSpecifier":
				case "ISqlAzureTSqlFullTextIndexColumnSpecifier":
				case "ISql110TSqlFullTextIndexColumnSpecifier":
				case "ISql120TSqlFullTextIndexColumnSpecifier":
				case "ISqlAzureV12TSqlFullTextIndexColumnSpecifier":
				case "TSqlFullTextIndexColumnSpecifier":				
					yield return TSqlFullTextIndexColumnSpecifier.TypeClass;
					break;
				case "ISql100TSqlFullTextStopList":
				case "ISql110TSqlFullTextStopList":
				case "ISql120TSqlFullTextStopList":
				case "ISqlAzureV12TSqlFullTextStopList":
				case "TSqlFullTextStopList":				
					yield return TSqlFullTextStopList.TypeClass;
					break;
				case "ISql90TSqlHttpProtocolSpecifier":
				case "ISql100TSqlHttpProtocolSpecifier":
				case "TSqlHttpProtocolSpecifier":				
					yield return TSqlHttpProtocolSpecifier.TypeClass;
					break;
				case "ISql90TSqlIndex":
				case "ISql100TSqlIndex":
				case "ISqlAzureTSqlIndex":
				case "ISql110TSqlIndex":
				case "ISql120TSqlIndex":
				case "ISqlAzureV12TSqlIndex":
				case "TSqlIndex":				
					yield return TSqlIndex.TypeClass;
					break;
				case "ISql90TSqlLinkedServer":
				case "ISql100TSqlLinkedServer":
				case "ISql110TSqlLinkedServer":
				case "ISql120TSqlLinkedServer":
				case "TSqlLinkedServer":				
					yield return TSqlLinkedServer.TypeClass;
					break;
				case "ISql90TSqlLinkedServerLogin":
				case "ISql100TSqlLinkedServerLogin":
				case "ISql110TSqlLinkedServerLogin":
				case "ISql120TSqlLinkedServerLogin":
				case "TSqlLinkedServerLogin":				
					yield return TSqlLinkedServerLogin.TypeClass;
					break;
				case "ISql90TSqlLogin":
				case "ISql100TSqlLogin":
				case "ISqlAzureTSqlLogin":
				case "ISql110TSqlLogin":
				case "ISql120TSqlLogin":
				case "ISqlAzureV12TSqlLogin":
				case "TSqlLogin":				
					yield return TSqlLogin.TypeClass;
					break;
				case "ISql90TSqlMasterKey":
				case "ISql100TSqlMasterKey":
				case "ISql110TSqlMasterKey":
				case "ISql120TSqlMasterKey":
				case "TSqlMasterKey":				
					yield return TSqlMasterKey.TypeClass;
					break;
				case "ISql90TSqlMessageType":
				case "ISql100TSqlMessageType":
				case "ISql110TSqlMessageType":
				case "ISql120TSqlMessageType":
				case "TSqlMessageType":				
					yield return TSqlMessageType.TypeClass;
					break;
				case "ISql90TSqlParameter":
				case "ISql100TSqlParameter":
				case "ISqlAzureTSqlParameter":
				case "ISql110TSqlParameter":
				case "ISql120TSqlParameter":
				case "ISqlAzureV12TSqlParameter":
				case "TSqlParameter":				
					yield return TSqlParameter.TypeClass;
					break;
				case "ISql90TSqlPartitionFunction":
				case "ISql100TSqlPartitionFunction":
				case "ISql110TSqlPartitionFunction":
				case "ISql120TSqlPartitionFunction":
				case "ISqlAzureV12TSqlPartitionFunction":
				case "TSqlPartitionFunction":				
					yield return TSqlPartitionFunction.TypeClass;
					break;
				case "ISql90TSqlPartitionScheme":
				case "ISql100TSqlPartitionScheme":
				case "ISql110TSqlPartitionScheme":
				case "ISql120TSqlPartitionScheme":
				case "ISqlAzureV12TSqlPartitionScheme":
				case "TSqlPartitionScheme":				
					yield return TSqlPartitionScheme.TypeClass;
					break;
				case "ISql90TSqlPartitionValue":
				case "ISql100TSqlPartitionValue":
				case "ISql110TSqlPartitionValue":
				case "ISql120TSqlPartitionValue":
				case "ISqlAzureV12TSqlPartitionValue":
				case "TSqlPartitionValue":				
					yield return TSqlPartitionValue.TypeClass;
					break;
				case "ISql90TSqlPermission":
				case "ISql100TSqlPermission":
				case "ISqlAzureTSqlPermission":
				case "ISql110TSqlPermission":
				case "ISql120TSqlPermission":
				case "ISqlAzureV12TSqlPermission":
				case "TSqlPermission":				
					yield return TSqlPermission.TypeClass;
					break;
				case "ISql90TSqlPrimaryKeyConstraint":
				case "ISql100TSqlPrimaryKeyConstraint":
				case "ISqlAzureTSqlPrimaryKeyConstraint":
				case "ISql110TSqlPrimaryKeyConstraint":
				case "ISql120TSqlPrimaryKeyConstraint":
				case "ISqlAzureV12TSqlPrimaryKeyConstraint":
				case "TSqlPrimaryKeyConstraint":				
					yield return TSqlPrimaryKeyConstraint.TypeClass;
					break;
				case "ISql90TSqlProcedure":
				case "ISql100TSqlProcedure":
				case "ISqlAzureTSqlProcedure":
				case "ISql110TSqlProcedure":
				case "ISql120TSqlProcedure":
				case "ISqlAzureV12TSqlProcedure":
				case "TSqlProcedure":				
					yield return TSqlProcedure.TypeClass;
					break;
				case "ISql110TSqlPromotedNodePathForSqlType":
				case "ISql120TSqlPromotedNodePathForSqlType":
				case "ISqlAzureV12TSqlPromotedNodePathForSqlType":
				case "TSqlPromotedNodePathForSqlType":				
					yield return TSqlPromotedNodePathForSqlType.TypeClass;
					break;
				case "ISql110TSqlPromotedNodePathForXQueryType":
				case "ISql120TSqlPromotedNodePathForXQueryType":
				case "ISqlAzureV12TSqlPromotedNodePathForXQueryType":
				case "TSqlPromotedNodePathForXQueryType":				
					yield return TSqlPromotedNodePathForXQueryType.TypeClass;
					break;
				case "ISql90TSqlQueue":
				case "ISql100TSqlQueue":
				case "ISql110TSqlQueue":
				case "ISql120TSqlQueue":
				case "TSqlQueue":				
					yield return TSqlQueue.TypeClass;
					break;
				case "ISql90TSqlQueueEventNotification":
				case "ISql100TSqlQueueEventNotification":
				case "ISql110TSqlQueueEventNotification":
				case "ISql120TSqlQueueEventNotification":
				case "TSqlQueueEventNotification":				
					yield return TSqlQueueEventNotification.TypeClass;
					break;
				case "ISql90TSqlRemoteServiceBinding":
				case "ISql100TSqlRemoteServiceBinding":
				case "ISql110TSqlRemoteServiceBinding":
				case "ISql120TSqlRemoteServiceBinding":
				case "TSqlRemoteServiceBinding":				
					yield return TSqlRemoteServiceBinding.TypeClass;
					break;
				case "ISql100TSqlResourceGovernor":
				case "ISql110TSqlResourceGovernor":
				case "ISql120TSqlResourceGovernor":
				case "TSqlResourceGovernor":				
					yield return TSqlResourceGovernor.TypeClass;
					break;
				case "ISql100TSqlResourcePool":
				case "ISql110TSqlResourcePool":
				case "ISql120TSqlResourcePool":
				case "TSqlResourcePool":				
					yield return TSqlResourcePool.TypeClass;
					break;
				case "ISql90TSqlRole":
				case "ISql100TSqlRole":
				case "ISqlAzureTSqlRole":
				case "ISql110TSqlRole":
				case "ISql120TSqlRole":
				case "ISqlAzureV12TSqlRole":
				case "TSqlRole":				
					yield return TSqlRole.TypeClass;
					break;
				case "ISql90TSqlRoleMembership":
				case "ISql100TSqlRoleMembership":
				case "ISqlAzureTSqlRoleMembership":
				case "ISql110TSqlRoleMembership":
				case "ISql120TSqlRoleMembership":
				case "ISqlAzureV12TSqlRoleMembership":
				case "TSqlRoleMembership":				
					yield return TSqlRoleMembership.TypeClass;
					break;
				case "ISql90TSqlRoute":
				case "ISql100TSqlRoute":
				case "ISql110TSqlRoute":
				case "ISql120TSqlRoute":
				case "TSqlRoute":				
					yield return TSqlRoute.TypeClass;
					break;
				case "ISql90TSqlRule":
				case "ISql100TSqlRule":
				case "ISql110TSqlRule":
				case "ISql120TSqlRule":
				case "ISqlAzureV12TSqlRule":
				case "TSqlRule":				
					yield return TSqlRule.TypeClass;
					break;
				case "ISql90TSqlScalarFunction":
				case "ISql100TSqlScalarFunction":
				case "ISqlAzureTSqlScalarFunction":
				case "ISql110TSqlScalarFunction":
				case "ISql120TSqlScalarFunction":
				case "ISqlAzureV12TSqlScalarFunction":
				case "TSqlScalarFunction":				
					yield return TSqlScalarFunction.TypeClass;
					break;
				case "ISql90TSqlSchema":
				case "ISql100TSqlSchema":
				case "ISqlAzureTSqlSchema":
				case "ISql110TSqlSchema":
				case "ISql120TSqlSchema":
				case "ISqlAzureV12TSqlSchema":
				case "TSqlSchema":				
					yield return TSqlSchema.TypeClass;
					break;
				case "ISql110TSqlSearchProperty":
				case "ISql120TSqlSearchProperty":
				case "TSqlSearchProperty":				
					yield return TSqlSearchProperty.TypeClass;
					break;
				case "ISql110TSqlSearchPropertyList":
				case "ISql120TSqlSearchPropertyList":
				case "TSqlSearchPropertyList":				
					yield return TSqlSearchPropertyList.TypeClass;
					break;
				case "ISql110TSqlSelectiveXmlIndex":
				case "ISql120TSqlSelectiveXmlIndex":
				case "ISqlAzureV12TSqlSelectiveXmlIndex":
				case "TSqlSelectiveXmlIndex":				
					yield return TSqlSelectiveXmlIndex.TypeClass;
					break;
				case "ISql110TSqlSequence":
				case "ISql120TSqlSequence":
				case "ISqlAzureV12TSqlSequence":
				case "TSqlSequence":				
					yield return TSqlSequence.TypeClass;
					break;
				case "ISql100TSqlServerAudit":
				case "ISql110TSqlServerAudit":
				case "ISql120TSqlServerAudit":
				case "TSqlServerAudit":				
					yield return TSqlServerAudit.TypeClass;
					break;
				case "ISql100TSqlServerAuditSpecification":
				case "ISql110TSqlServerAuditSpecification":
				case "ISql120TSqlServerAuditSpecification":
				case "TSqlServerAuditSpecification":				
					yield return TSqlServerAuditSpecification.TypeClass;
					break;
				case "ISql90TSqlServerDdlTrigger":
				case "ISql100TSqlServerDdlTrigger":
				case "ISql110TSqlServerDdlTrigger":
				case "ISql120TSqlServerDdlTrigger":
				case "TSqlServerDdlTrigger":				
					yield return TSqlServerDdlTrigger.TypeClass;
					break;
				case "ISql90TSqlServerEventNotification":
				case "ISql100TSqlServerEventNotification":
				case "ISql110TSqlServerEventNotification":
				case "ISql120TSqlServerEventNotification":
				case "TSqlServerEventNotification":				
					yield return TSqlServerEventNotification.TypeClass;
					break;
				case "ISql90TSqlServerOptions":
				case "ISql100TSqlServerOptions":
				case "ISqlAzureTSqlServerOptions":
				case "ISql110TSqlServerOptions":
				case "ISql120TSqlServerOptions":
				case "ISqlAzureV12TSqlServerOptions":
				case "TSqlServerOptions":				
					yield return TSqlServerOptions.TypeClass;
					break;
				case "ISql90TSqlServerRoleMembership":
				case "ISql100TSqlServerRoleMembership":
				case "ISqlAzureTSqlServerRoleMembership":
				case "ISql110TSqlServerRoleMembership":
				case "ISql120TSqlServerRoleMembership":
				case "ISqlAzureV12TSqlServerRoleMembership":
				case "TSqlServerRoleMembership":				
					yield return TSqlServerRoleMembership.TypeClass;
					break;
				case "ISql90TSqlService":
				case "ISql100TSqlService":
				case "ISql110TSqlService":
				case "ISql120TSqlService":
				case "TSqlService":				
					yield return TSqlService.TypeClass;
					break;
				case "ISql90TSqlServiceBrokerLanguageSpecifier":
				case "ISql100TSqlServiceBrokerLanguageSpecifier":
				case "ISql110TSqlServiceBrokerLanguageSpecifier":
				case "ISql120TSqlServiceBrokerLanguageSpecifier":
				case "TSqlServiceBrokerLanguageSpecifier":				
					yield return TSqlServiceBrokerLanguageSpecifier.TypeClass;
					break;
				case "ISql90TSqlSignature":
				case "ISql100TSqlSignature":
				case "ISql110TSqlSignature":
				case "ISql120TSqlSignature":
				case "TSqlSignature":				
					yield return TSqlSignature.TypeClass;
					break;
				case "ISql90TSqlSignatureEncryptionMechanism":
				case "ISql100TSqlSignatureEncryptionMechanism":
				case "ISql110TSqlSignatureEncryptionMechanism":
				case "ISql120TSqlSignatureEncryptionMechanism":
				case "TSqlSignatureEncryptionMechanism":				
					yield return TSqlSignatureEncryptionMechanism.TypeClass;
					break;
				case "ISql90TSqlSoapLanguageSpecifier":
				case "ISql100TSqlSoapLanguageSpecifier":
				case "TSqlSoapLanguageSpecifier":				
					yield return TSqlSoapLanguageSpecifier.TypeClass;
					break;
				case "ISql90TSqlSoapMethodSpecification":
				case "ISql100TSqlSoapMethodSpecification":
				case "TSqlSoapMethodSpecification":				
					yield return TSqlSoapMethodSpecification.TypeClass;
					break;
				case "ISql100TSqlSpatialIndex":
				case "ISqlAzureTSqlSpatialIndex":
				case "ISql110TSqlSpatialIndex":
				case "ISql120TSqlSpatialIndex":
				case "ISqlAzureV12TSqlSpatialIndex":
				case "TSqlSpatialIndex":				
					yield return TSqlSpatialIndex.TypeClass;
					break;
				case "ISql90TSqlSqlFile":
				case "ISql100TSqlSqlFile":
				case "ISql110TSqlSqlFile":
				case "ISql120TSqlSqlFile":
				case "TSqlSqlFile":				
					yield return TSqlSqlFile.TypeClass;
					break;
				case "ISql90TSqlStatistics":
				case "ISql100TSqlStatistics":
				case "ISqlAzureTSqlStatistics":
				case "ISql110TSqlStatistics":
				case "ISql120TSqlStatistics":
				case "ISqlAzureV12TSqlStatistics":
				case "TSqlStatistics":				
					yield return TSqlStatistics.TypeClass;
					break;
				case "ISql90TSqlSymmetricKey":
				case "ISql100TSqlSymmetricKey":
				case "ISql110TSqlSymmetricKey":
				case "ISql120TSqlSymmetricKey":
				case "TSqlSymmetricKey":				
					yield return TSqlSymmetricKey.TypeClass;
					break;
				case "ISql90TSqlSymmetricKeyPassword":
				case "ISql100TSqlSymmetricKeyPassword":
				case "ISql110TSqlSymmetricKeyPassword":
				case "ISql120TSqlSymmetricKeyPassword":
				case "TSqlSymmetricKeyPassword":				
					yield return TSqlSymmetricKeyPassword.TypeClass;
					break;
				case "ISql90TSqlSynonym":
				case "ISql100TSqlSynonym":
				case "ISqlAzureTSqlSynonym":
				case "ISql110TSqlSynonym":
				case "ISql120TSqlSynonym":
				case "ISqlAzureV12TSqlSynonym":
				case "TSqlSynonym":				
					yield return TSqlSynonym.TypeClass;
					break;
				case "ISql90TSqlTable":
				case "ISql100TSqlTable":
				case "ISqlAzureTSqlTable":
				case "ISql110TSqlTable":
				case "ISql120TSqlTable":
				case "ISqlAzureV12TSqlTable":
				case "TSqlTable":				
					yield return TSqlTable.TypeClass;
					break;
				case "ISql100TSqlTableType":
				case "ISqlAzureTSqlTableType":
				case "ISql110TSqlTableType":
				case "ISql120TSqlTableType":
				case "ISqlAzureV12TSqlTableType":
				case "TSqlTableType":				
					yield return TSqlTableType.TypeClass;
					break;
				case "ISql100TSqlTableTypeCheckConstraint":
				case "ISqlAzureTSqlTableTypeCheckConstraint":
				case "ISql110TSqlTableTypeCheckConstraint":
				case "ISql120TSqlTableTypeCheckConstraint":
				case "ISqlAzureV12TSqlTableTypeCheckConstraint":
				case "TSqlTableTypeCheckConstraint":				
					yield return TSqlTableTypeCheckConstraint.TypeClass;
					break;
				case "ISql100TSqlTableTypeColumn":
				case "ISqlAzureTSqlTableTypeColumn":
				case "ISql110TSqlTableTypeColumn":
				case "ISql120TSqlTableTypeColumn":
				case "ISqlAzureV12TSqlTableTypeColumn":
				case "TSqlTableTypeColumn":				
					yield return TSqlTableTypeColumn.TypeClass;
					break;
				case "ISql100TSqlTableTypeDefaultConstraint":
				case "ISqlAzureTSqlTableTypeDefaultConstraint":
				case "ISql110TSqlTableTypeDefaultConstraint":
				case "ISql120TSqlTableTypeDefaultConstraint":
				case "ISqlAzureV12TSqlTableTypeDefaultConstraint":
				case "TSqlTableTypeDefaultConstraint":				
					yield return TSqlTableTypeDefaultConstraint.TypeClass;
					break;
				case "ISql120TSqlTableTypeIndex":
				case "ISqlAzureV12TSqlTableTypeIndex":
				case "TSqlTableTypeIndex":				
					yield return TSqlTableTypeIndex.TypeClass;
					break;
				case "ISql100TSqlTableTypePrimaryKeyConstraint":
				case "ISqlAzureTSqlTableTypePrimaryKeyConstraint":
				case "ISql110TSqlTableTypePrimaryKeyConstraint":
				case "ISql120TSqlTableTypePrimaryKeyConstraint":
				case "ISqlAzureV12TSqlTableTypePrimaryKeyConstraint":
				case "TSqlTableTypePrimaryKeyConstraint":				
					yield return TSqlTableTypePrimaryKeyConstraint.TypeClass;
					break;
				case "ISql100TSqlTableTypeUniqueConstraint":
				case "ISqlAzureTSqlTableTypeUniqueConstraint":
				case "ISql110TSqlTableTypeUniqueConstraint":
				case "ISql120TSqlTableTypeUniqueConstraint":
				case "ISqlAzureV12TSqlTableTypeUniqueConstraint":
				case "TSqlTableTypeUniqueConstraint":				
					yield return TSqlTableTypeUniqueConstraint.TypeClass;
					break;
				case "ISql90TSqlTableValuedFunction":
				case "ISql100TSqlTableValuedFunction":
				case "ISqlAzureTSqlTableValuedFunction":
				case "ISql110TSqlTableValuedFunction":
				case "ISql120TSqlTableValuedFunction":
				case "ISqlAzureV12TSqlTableValuedFunction":
				case "TSqlTableValuedFunction":				
					yield return TSqlTableValuedFunction.TypeClass;
					break;
				case "ISql90TSqlTcpProtocolSpecifier":
				case "ISql100TSqlTcpProtocolSpecifier":
				case "ISql110TSqlTcpProtocolSpecifier":
				case "ISql120TSqlTcpProtocolSpecifier":
				case "TSqlTcpProtocolSpecifier":				
					yield return TSqlTcpProtocolSpecifier.TypeClass;
					break;
				case "ISql90TSqlUniqueConstraint":
				case "ISql100TSqlUniqueConstraint":
				case "ISqlAzureTSqlUniqueConstraint":
				case "ISql110TSqlUniqueConstraint":
				case "ISql120TSqlUniqueConstraint":
				case "ISqlAzureV12TSqlUniqueConstraint":
				case "TSqlUniqueConstraint":				
					yield return TSqlUniqueConstraint.TypeClass;
					break;
				case "ISql90TSqlUser":
				case "ISql100TSqlUser":
				case "ISqlAzureTSqlUser":
				case "ISql110TSqlUser":
				case "ISql120TSqlUser":
				case "ISqlAzureV12TSqlUser":
				case "TSqlUser":				
					yield return TSqlUser.TypeClass;
					break;
				case "ISql110TSqlUserDefinedServerRole":
				case "ISql120TSqlUserDefinedServerRole":
				case "TSqlUserDefinedServerRole":				
					yield return TSqlUserDefinedServerRole.TypeClass;
					break;
				case "ISql90TSqlUserDefinedType":
				case "ISql100TSqlUserDefinedType":
				case "ISqlAzureTSqlUserDefinedType":
				case "ISql110TSqlUserDefinedType":
				case "ISql120TSqlUserDefinedType":
				case "ISqlAzureV12TSqlUserDefinedType":
				case "TSqlUserDefinedType":				
					yield return TSqlUserDefinedType.TypeClass;
					break;
				case "ISql90TSqlView":
				case "ISql100TSqlView":
				case "ISqlAzureTSqlView":
				case "ISql110TSqlView":
				case "ISql120TSqlView":
				case "ISqlAzureV12TSqlView":
				case "TSqlView":				
					yield return TSqlView.TypeClass;
					break;
				case "ISql100TSqlWorkloadGroup":
				case "ISql110TSqlWorkloadGroup":
				case "ISql120TSqlWorkloadGroup":
				case "TSqlWorkloadGroup":				
					yield return TSqlWorkloadGroup.TypeClass;
					break;
				case "ISql90TSqlXmlIndex":
				case "ISql100TSqlXmlIndex":
				case "ISql110TSqlXmlIndex":
				case "ISql120TSqlXmlIndex":
				case "ISqlAzureV12TSqlXmlIndex":
				case "TSqlXmlIndex":				
					yield return TSqlXmlIndex.TypeClass;
					break;
				case "ISql110TSqlXmlNamespace":
				case "ISql120TSqlXmlNamespace":
				case "ISqlAzureV12TSqlXmlNamespace":
				case "TSqlXmlNamespace":				
					yield return TSqlXmlNamespace.TypeClass;
					break;
				case "ISql90TSqlXmlSchemaCollection":
				case "ISql100TSqlXmlSchemaCollection":
				case "ISql110TSqlXmlSchemaCollection":
				case "ISql120TSqlXmlSchemaCollection":
				case "ISqlAzureV12TSqlXmlSchemaCollection":
				case "TSqlXmlSchemaCollection":				
					yield return TSqlXmlSchemaCollection.TypeClass;
					break;
				case "IEndpointLanguageSpecifier":
					yield return TSqlDatabaseMirroringLanguageSpecifier.TypeClass;
					yield return TSqlServiceBrokerLanguageSpecifier.TypeClass;
					yield return TSqlSoapLanguageSpecifier.TypeClass;
					break;
				case "IExtendedPropertyHost":
					yield return TSqlCheckConstraint.TypeClass;
					yield return TSqlDefaultConstraint.TypeClass;
					yield return TSqlForeignKeyConstraint.TypeClass;
					yield return TSqlPrimaryKeyConstraint.TypeClass;
					yield return TSqlUniqueConstraint.TypeClass;
					break;
				case "IProtocolSpecifier":
					yield return TSqlHttpProtocolSpecifier.TypeClass;
					yield return TSqlTcpProtocolSpecifier.TypeClass;
					break;
				case "ISpecifiesIndex":
					yield return TSqlFileTable.TypeClass;
					yield return TSqlTable.TypeClass;
					yield return TSqlTableValuedFunction.TypeClass;
					yield return TSqlView.TypeClass;
					break;
				case "ISpecifiesStorage":
					yield return TSqlFileTable.TypeClass;
					yield return TSqlIndex.TypeClass;
					yield return TSqlPrimaryKeyConstraint.TypeClass;
					yield return TSqlTable.TypeClass;
					yield return TSqlUniqueConstraint.TypeClass;
					break;
				case "ISqlColumnSource":
					yield return TSqlFileTable.TypeClass;
					yield return TSqlQueue.TypeClass;
					yield return TSqlTable.TypeClass;
					yield return TSqlTableValuedFunction.TypeClass;
					yield return TSqlView.TypeClass;
					break;
				case "ISqlDatabaseSecurityPrincipal":
					yield return TSqlApplicationRole.TypeClass;
					yield return TSqlRole.TypeClass;
					yield return TSqlUser.TypeClass;
					break;
				case "ISqlDataType":
					yield return TSqlDataType.TypeClass;
					yield return TSqlTableType.TypeClass;
					yield return TSqlUserDefinedType.TypeClass;
					break;
				case "ISqlIndex":
					yield return TSqlColumnStoreIndex.TypeClass;
					yield return TSqlIndex.TypeClass;
					yield return TSqlSelectiveXmlIndex.TypeClass;
					yield return TSqlSpatialIndex.TypeClass;
					yield return TSqlTableTypeIndex.TypeClass;
					yield return TSqlXmlIndex.TypeClass;
					break;
				case "ISqlPromotedNodePath":
					yield return TSqlPromotedNodePathForSqlType.TypeClass;
					yield return TSqlPromotedNodePathForXQueryType.TypeClass;
					break;
				case "ISqlSecurable":
					yield return TSqlAssembly.TypeClass;
					yield return TSqlAsymmetricKey.TypeClass;
					yield return TSqlCertificate.TypeClass;
					yield return TSqlColumn.TypeClass;
					yield return TSqlContract.TypeClass;
					yield return TSqlDatabaseOptions.TypeClass;
					yield return TSqlDefault.TypeClass;
					yield return TSqlEndpoint.TypeClass;
					yield return TSqlFullTextCatalog.TypeClass;
					yield return TSqlFullTextStopList.TypeClass;
					yield return TSqlMessageType.TypeClass;
					yield return TSqlRemoteServiceBinding.TypeClass;
					yield return TSqlRoute.TypeClass;
					yield return TSqlSchema.TypeClass;
					yield return TSqlSearchPropertyList.TypeClass;
					yield return TSqlServerOptions.TypeClass;
					yield return TSqlService.TypeClass;
					yield return TSqlSymmetricKey.TypeClass;
					yield return TSqlTableType.TypeClass;
					yield return TSqlUserDefinedType.TypeClass;
					yield return TSqlXmlSchemaCollection.TypeClass;
					break;
				case "ITableTypeConstraint":
					yield return TSqlTableTypeCheckConstraint.TypeClass;
					yield return TSqlTableTypeDefaultConstraint.TypeClass;
					yield return TSqlTableTypePrimaryKeyConstraint.TypeClass;
					yield return TSqlTableTypeUniqueConstraint.TypeClass;
					break;
				default:
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The type {0}.{1} is not mapped", type.Namespace, type.Name));
			}
		}
		
		/// <summary>
        /// Determines if instances of <see cref="TSqlModelElement"/> for the <paramref name="typeClass"/> model 
        /// element class will derive from or implement the interface type <paramref name="type"/>.
        /// </summary>
        /// <param name="typeClass">Model element type class to interrogate.</param>
        /// <param name="type">Candidate <see cref="System.Type"/> for comparison</param>
        /// <returns>True if <see cref="TSqlModelElement"/> implement or extend the type <paramref name="type"/>.</returns>		
		public static bool ImplementsType(ModelTypeClass typeClass, Type type)
		{
			if(typeClass == null)
			{
				throw new ArgumentNullException("typeClass");
			}
			else if(type == null)
			{
				throw new ArgumentNullException("type");
			}

			switch(typeClass.Name)
			{
			case "Column":
				if (typeof(TSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlColumn).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableValuedFunction":
				if (typeof(TSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableValuedFunction).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlColumnSource).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "ApplicationRole":
				if (typeof(TSqlApplicationRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlApplicationRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlApplicationRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlApplicationRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlApplicationRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlApplicationRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlDatabaseSecurityPrincipal).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Index":
				if (typeof(TSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesStorage).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Assembly":
				if (typeof(TSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlAssembly).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "AsymmetricKey":
				if (typeof(TSqlAsymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlAsymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlAsymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlAsymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlAsymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "DataType":
				if (typeof(TSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Certificate":
				if (typeof(TSqlCertificate).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlCertificate).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlCertificate).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlCertificate).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlCertificate).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "CheckConstraint":
				if (typeof(TSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IExtendedPropertyHost).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "ColumnStoreIndex":
				if (typeof(TSqlColumnStoreIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlColumnStoreIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlColumnStoreIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlColumnStoreIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Contract":
				if (typeof(TSqlContract).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlContract).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlContract).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlContract).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlContract).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "DatabaseMirroringLanguageSpecifier":
				if (typeof(TSqlDatabaseMirroringLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlDatabaseMirroringLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlDatabaseMirroringLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlDatabaseMirroringLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlDatabaseMirroringLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IEndpointLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "DatabaseOptions":
				if (typeof(TSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlDatabaseOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Default":
				if (typeof(TSqlDefault).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlDefault).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlDefault).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlDefault).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlDefault).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlDefault).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "DefaultConstraint":
				if (typeof(TSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IExtendedPropertyHost).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Endpoint":
				if (typeof(TSqlEndpoint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlEndpoint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlEndpoint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlEndpoint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlEndpoint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "ForeignKeyConstraint":
				if (typeof(TSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlForeignKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IExtendedPropertyHost).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "FullTextCatalog":
				if (typeof(TSqlFullTextCatalog).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlFullTextCatalog).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlFullTextCatalog).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlFullTextCatalog).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlFullTextCatalog).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlFullTextCatalog).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "FullTextStopList":
				if (typeof(TSqlFullTextStopList).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlFullTextStopList).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlFullTextStopList).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlFullTextStopList).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlFullTextStopList).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "HttpProtocolSpecifier":
				if (typeof(TSqlHttpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlHttpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlHttpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "MessageType":
				if (typeof(TSqlMessageType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlMessageType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlMessageType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlMessageType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlMessageType).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "PrimaryKeyConstraint":
				if (typeof(TSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlPrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IExtendedPropertyHost).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesStorage).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Queue":
				if (typeof(TSqlQueue).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlQueue).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlQueue).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlQueue).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlQueue).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlColumnSource).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "RemoteServiceBinding":
				if (typeof(TSqlRemoteServiceBinding).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlRemoteServiceBinding).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlRemoteServiceBinding).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlRemoteServiceBinding).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlRemoteServiceBinding).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Role":
				if (typeof(TSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlRole).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlDatabaseSecurityPrincipal).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Route":
				if (typeof(TSqlRoute).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlRoute).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlRoute).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlRoute).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlRoute).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Schema":
				if (typeof(TSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlSchema).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "SearchPropertyList":
				if (typeof(TSqlSearchPropertyList).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlSearchPropertyList).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlSearchPropertyList).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "ServerOptions":
				if (typeof(TSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlServerOptions).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Service":
				if (typeof(TSqlService).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlService).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlService).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlService).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlService).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "ServiceBrokerLanguageSpecifier":
				if (typeof(TSqlServiceBrokerLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlServiceBrokerLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlServiceBrokerLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlServiceBrokerLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlServiceBrokerLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IEndpointLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "SoapLanguageSpecifier":
				if (typeof(TSqlSoapLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlSoapLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlSoapLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IEndpointLanguageSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "SpatialIndex":
				if (typeof(TSqlSpatialIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlSpatialIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlSpatialIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlSpatialIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlSpatialIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlSpatialIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "SymmetricKey":
				if (typeof(TSqlSymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlSymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlSymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlSymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlSymmetricKey).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "Table":
				if (typeof(TSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlColumnSource).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesStorage).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "FileTable":
				if (typeof(TSqlFileTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlFileTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlFileTable).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlColumnSource).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesStorage).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableType":
				if (typeof(TSqlTableType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTableType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTableType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTableType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableType).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableTypeCheckConstraint":
				if (typeof(TSqlTableTypeCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTableTypeCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTableTypeCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTableTypeCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableTypeCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableTypeCheckConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ITableTypeConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableTypeDefaultConstraint":
				if (typeof(TSqlTableTypeDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTableTypeDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTableTypeDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTableTypeDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableTypeDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableTypeDefaultConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ITableTypeConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableTypeIndex":
				if (typeof(TSqlTableTypeIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableTypeIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableTypeIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableTypePrimaryKeyConstraint":
				if (typeof(TSqlTableTypePrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTableTypePrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTableTypePrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTableTypePrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableTypePrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableTypePrimaryKeyConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ITableTypeConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TableTypeUniqueConstraint":
				if (typeof(TSqlTableTypeUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTableTypeUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlTableTypeUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTableTypeUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTableTypeUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlTableTypeUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ITableTypeConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "TcpProtocolSpecifier":
				if (typeof(TSqlTcpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlTcpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlTcpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlTcpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlTcpProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IProtocolSpecifier).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "UniqueConstraint":
				if (typeof(TSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlUniqueConstraint).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(IExtendedPropertyHost).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesStorage).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "User":
				if (typeof(TSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlUser).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlDatabaseSecurityPrincipal).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "UserDefinedType":
				if (typeof(TSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlUserDefinedType).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlDataType).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "View":
				if (typeof(TSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureTSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlView).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlColumnSource).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISpecifiesIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "XmlIndex":
				if (typeof(TSqlXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "SelectiveXmlIndex":
				if (typeof(TSqlSelectiveXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlSelectiveXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlSelectiveXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlSelectiveXmlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlIndex).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "PromotedNodePathForXQueryType":
				if (typeof(TSqlPromotedNodePathForXQueryType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlPromotedNodePathForXQueryType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlPromotedNodePathForXQueryType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlPromotedNodePathForXQueryType).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlPromotedNodePath).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "PromotedNodePathForSqlType":
				if (typeof(TSqlPromotedNodePathForSqlType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlPromotedNodePathForSqlType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlPromotedNodePathForSqlType).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlPromotedNodePathForSqlType).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlPromotedNodePath).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			case "XmlSchemaCollection":
				if (typeof(TSqlXmlSchemaCollection).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql90TSqlXmlSchemaCollection).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql100TSqlXmlSchemaCollection).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql110TSqlXmlSchemaCollection).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISql120TSqlXmlSchemaCollection).IsAssignableFrom(type))
				{
					return true;
				}
				else if (typeof(ISqlAzureV12TSqlXmlSchemaCollection).IsAssignableFrom(type))
				{
					return true;
				}
				else if(typeof(ISqlSecurable).IsAssignableFrom(type))
				{
					return true;
				}
				// no interfaces mapped.
				return false;
			}
			// no type mapping found 
			return false;
		}
	}
}