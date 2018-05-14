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
	using Microsoft.SqlServer.Server;
	using Microsoft.SqlServer.Dac.Model;
	using System.Collections.Generic;
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlColumn"/>.
	/// </summary>
	public partial class TSqlColumn : ISql90TSqlColumn
	{		
		String ISql90TSqlColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql90TSqlColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql90TSqlColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql90TSqlColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql90TSqlColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql90TSqlColumn.IsIdentityNotForReplication 
		{
			get { return this.IsIdentityNotForReplication;}
		}
		Boolean ISql90TSqlColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql90TSqlColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql90TSqlColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql90TSqlColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql90TSqlColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql90TSqlColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql90TSqlColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql90TSqlColumn.Scale 
		{
			get { return this.Scale;}
		}
		XmlStyle ISql90TSqlColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql90TSqlColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlSchemaCollectionReference> ISql90TSqlColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlTableValuedFunction"/>.
	/// </summary>
	public partial class TSqlTableValuedFunction : ISql90TSqlTableValuedFunction
	{		
		Boolean? ISql90TSqlTableValuedFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql90TSqlTableValuedFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql90TSqlTableValuedFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql90TSqlTableValuedFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql90TSqlTableValuedFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql90TSqlTableValuedFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql90TSqlTableValuedFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlTableValuedFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql90TSqlTableValuedFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql90TSqlTableValuedFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql90TSqlTableValuedFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql90TSqlTableValuedFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql90TSqlTableValuedFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql90TSqlTableValuedFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		String ISql90TSqlTableValuedFunction.ReturnTableVariableName 
		{
			get { return this.ReturnTableVariableName;}
		}
		SystemDataAccessKind? ISql90TSqlTableValuedFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql90TSqlTableValuedFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql90TSqlTableValuedFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssembly> ISql90TSqlTableValuedFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlTableValuedFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn> ISql90TSqlTableValuedFunction.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlTableValuedFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter> ISql90TSqlTableValuedFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference> ISql90TSqlTableValuedFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlTableValuedFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlClrTableOption> ISql90TSqlTableValuedFunction.TableOption 
		{
			get 
			{
				return this.TableOption.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlClrTableOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlTableValuedFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlScalarFunction"/>.
	/// </summary>
	public partial class TSqlScalarFunction : ISql90TSqlScalarFunction
	{		
		Boolean? ISql90TSqlScalarFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql90TSqlScalarFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql90TSqlScalarFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql90TSqlScalarFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql90TSqlScalarFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql90TSqlScalarFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql90TSqlScalarFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlScalarFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql90TSqlScalarFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql90TSqlScalarFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql90TSqlScalarFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql90TSqlScalarFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql90TSqlScalarFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql90TSqlScalarFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		SystemDataAccessKind? ISql90TSqlScalarFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql90TSqlScalarFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql90TSqlScalarFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssembly> ISql90TSqlScalarFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlScalarFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlScalarFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter> ISql90TSqlScalarFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference> ISql90TSqlScalarFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlScalarFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlScalarFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlClrTableOption"/>.
	/// </summary>
	public partial class TSqlClrTableOption : ISql90TSqlClrTableOption
	{		
		String ISql90TSqlClrTableOption.ClassName 
		{
			get { return this.ClassName;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlAggregate"/>.
	/// </summary>
	public partial class TSqlAggregate : ISql90TSqlAggregate
	{		
		String ISql90TSqlAggregate.ClassName 
		{
			get { return this.ClassName;}
		}
		Format ISql90TSqlAggregate.Format 
		{
			get { return this.Format;}
		}
		Boolean? ISql90TSqlAggregate.InvariantToDuplicates 
		{
			get { return this.InvariantToDuplicates;}
		}
		Boolean? ISql90TSqlAggregate.InvariantToNulls 
		{
			get { return this.InvariantToNulls;}
		}
		Int32? ISql90TSqlAggregate.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		Boolean? ISql90TSqlAggregate.NullIfEmpty 
		{
			get { return this.NullIfEmpty;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlAggregate.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter> ISql90TSqlAggregate.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference> ISql90TSqlAggregate.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlAggregate.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlApplicationRole"/>.
	/// </summary>
	public partial class TSqlApplicationRole : ISql90TSqlApplicationRole
	{		
		String ISql90TSqlApplicationRole.Password 
		{
			get { return this.Password;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlApplicationRole.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlIndex"/>.
	/// </summary>
	public partial class TSqlIndex : ISql90TSqlIndex
	{		
		Boolean ISql90TSqlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql90TSqlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql90TSqlIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql90TSqlIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql90TSqlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql90TSqlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql90TSqlIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql90TSqlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql90TSqlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql90TSqlIndex.Unique 
		{
			get { return this.Unique;}
		}
		Boolean ISql90TSqlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlIndex.IncludedColumns 
		{
			get 
			{
				return this.IncludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql90TSqlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference> ISql90TSqlIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlAssembly"/>.
	/// </summary>
	public partial class TSqlAssembly : ISql90TSqlAssembly
	{		
		AssemblyPermissionSet ISql90TSqlAssembly.PermissionSet 
		{
			get { return this.PermissionSet;}
		}
		Boolean ISql90TSqlAssembly.Visible 
		{
			get { return this.Visible;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblySource> ISql90TSqlAssembly.AssemblySources 
		{
			get 
			{
				return this.AssemblySources.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblySource>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlAssembly.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlAssembly.ReferencedAssemblies 
		{
			get 
			{
				return this.ReferencedAssemblies.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlAssemblySource"/>.
	/// </summary>
	public partial class TSqlAssemblySource : ISql90TSqlAssemblySource
	{		
		String ISql90TSqlAssemblySource.Source 
		{
			get { return this.Source;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlAsymmetricKey"/>.
	/// </summary>
	public partial class TSqlAsymmetricKey : ISql90TSqlAsymmetricKey
	{		
		AsymmetricKeyAlgorithm ISql90TSqlAsymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		Boolean ISql90TSqlAsymmetricKey.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql90TSqlAsymmetricKey.ExecutableFile 
		{
			get { return this.ExecutableFile;}
		}
		String ISql90TSqlAsymmetricKey.File 
		{
			get { return this.File;}
		}
		String ISql90TSqlAsymmetricKey.Password 
		{
			get { return this.Password;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlAsymmetricKey.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlAsymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlBuiltInServerRole"/>.
	/// </summary>
	public partial class TSqlBuiltInServerRole : ISql90TSqlBuiltInServerRole
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDataType"/>.
	/// </summary>
	public partial class TSqlDataType : ISql90TSqlDataType
	{		
		SqlDataType ISql90TSqlDataType.SqlDataType 
		{
			get { return this.SqlDataType;}
		}
		Boolean ISql90TSqlDataType.UddtIsMax 
		{
			get { return this.UddtIsMax;}
		}
		Int32 ISql90TSqlDataType.UddtLength 
		{
			get { return this.UddtLength;}
		}
		Boolean ISql90TSqlDataType.UddtNullable 
		{
			get { return this.UddtNullable;}
		}
		Int32 ISql90TSqlDataType.UddtPrecision 
		{
			get { return this.UddtPrecision;}
		}
		Int32 ISql90TSqlDataType.UddtScale 
		{
			get { return this.UddtScale;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlDataType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference> ISql90TSqlDataType.Type 
		{
			get 
			{
				return this.Type.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlCertificate"/>.
	/// </summary>
	public partial class TSqlCertificate : ISql90TSqlCertificate
	{		
		Boolean ISql90TSqlCertificate.ActiveForBeginDialog 
		{
			get { return this.ActiveForBeginDialog;}
		}
		Boolean ISql90TSqlCertificate.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql90TSqlCertificate.EncryptionPassword 
		{
			get { return this.EncryptionPassword;}
		}
		String ISql90TSqlCertificate.ExistingKeysFilePath 
		{
			get { return this.ExistingKeysFilePath;}
		}
		String ISql90TSqlCertificate.ExpiryDate 
		{
			get { return this.ExpiryDate;}
		}
		Boolean ISql90TSqlCertificate.IsExistingKeyFileExecutable 
		{
			get { return this.IsExistingKeyFileExecutable;}
		}
		String ISql90TSqlCertificate.PrivateKeyDecryptionPassword 
		{
			get { return this.PrivateKeyDecryptionPassword;}
		}
		String ISql90TSqlCertificate.PrivateKeyEncryptionPassword 
		{
			get { return this.PrivateKeyEncryptionPassword;}
		}
		String ISql90TSqlCertificate.PrivateKeyFilePath 
		{
			get { return this.PrivateKeyFilePath;}
		}
		String ISql90TSqlCertificate.StartDate 
		{
			get { return this.StartDate;}
		}
		String ISql90TSqlCertificate.Subject 
		{
			get { return this.Subject;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlCertificate.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlCertificate.ExistingKeysAssembly 
		{
			get 
			{
				return this.ExistingKeysAssembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlCheckConstraint"/>.
	/// </summary>
	public partial class TSqlCheckConstraint : ISql90TSqlCheckConstraint
	{		
		Boolean ISql90TSqlCheckConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql90TSqlCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql90TSqlCheckConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference> ISql90TSqlCheckConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlClrTypeMethod"/>.
	/// </summary>
	public partial class TSqlClrTypeMethod : ISql90TSqlClrTypeMethod
	{		
		String ISql90TSqlClrTypeMethod.MethodName 
		{
			get { return this.MethodName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter> ISql90TSqlClrTypeMethod.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataType> ISql90TSqlClrTypeMethod.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataType>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlClrTypeMethodParameter"/>.
	/// </summary>
	public partial class TSqlClrTypeMethodParameter : ISql90TSqlClrTypeMethodParameter
	{		
		Boolean ISql90TSqlClrTypeMethodParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		String ISql90TSqlClrTypeMethodParameter.ParameterName 
		{
			get { return this.ParameterName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql90TSqlClrTypeMethodParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlClrTypeProperty"/>.
	/// </summary>
	public partial class TSqlClrTypeProperty : ISql90TSqlClrTypeProperty
	{		
		String ISql90TSqlClrTypeProperty.PropertyName 
		{
			get { return this.PropertyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference> ISql90TSqlClrTypeProperty.ClrType 
		{
			get 
			{
				return this.ClrType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlContract"/>.
	/// </summary>
	public partial class TSqlContract : ISql90TSqlContract
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlContract.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlMessageTypeReference> ISql90TSqlContract.Messages 
		{
			get 
			{
				return this.Messages.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlMessageTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlCredential"/>.
	/// </summary>
	public partial class TSqlCredential : ISql90TSqlCredential
	{		
		String ISql90TSqlCredential.Identity 
		{
			get { return this.Identity;}
		}
		String ISql90TSqlCredential.Secret 
		{
			get { return this.Secret;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDatabaseDdlTrigger"/>.
	/// </summary>
	public partial class TSqlDatabaseDdlTrigger : ISql90TSqlDatabaseDdlTrigger
	{		
		Boolean? ISql90TSqlDatabaseDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql90TSqlDatabaseDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql90TSqlDatabaseDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql90TSqlDatabaseDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql90TSqlDatabaseDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlDatabaseDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql90TSqlDatabaseDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql90TSqlDatabaseDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql90TSqlDatabaseDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql90TSqlDatabaseDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlDatabaseDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlDatabaseDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup> ISql90TSqlDatabaseDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql90TSqlDatabaseDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlDatabaseDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlDatabaseDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDatabaseEventNotification"/>.
	/// </summary>
	public partial class TSqlDatabaseEventNotification : ISql90TSqlDatabaseEventNotification
	{		
		String ISql90TSqlDatabaseEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql90TSqlDatabaseEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql90TSqlDatabaseEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup> ISql90TSqlDatabaseEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql90TSqlDatabaseEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDatabaseMirroringLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlDatabaseMirroringLanguageSpecifier : ISql90TSqlDatabaseMirroringLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql90TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql90TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql90TSqlDatabaseMirroringLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		DatabaseMirroringRole ISql90TSqlDatabaseMirroringLanguageSpecifier.RoleType 
		{
			get { return this.RoleType;}
		}
		Boolean ISql90TSqlDatabaseMirroringLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql90TSqlDatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference> ISql90TSqlDatabaseMirroringLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDatabaseOptions"/>.
	/// </summary>
	public partial class TSqlDatabaseOptions : ISql90TSqlDatabaseOptions
	{		
		Boolean ISql90TSqlDatabaseOptions.AllowSnapshotIsolation 
		{
			get { return this.AllowSnapshotIsolation;}
		}
		Boolean ISql90TSqlDatabaseOptions.AnsiNullDefaultOn 
		{
			get { return this.AnsiNullDefaultOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.AnsiPaddingOn 
		{
			get { return this.AnsiPaddingOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.AnsiWarningsOn 
		{
			get { return this.AnsiWarningsOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.ArithAbortOn 
		{
			get { return this.ArithAbortOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.AutoClose 
		{
			get { return this.AutoClose;}
		}
		Boolean ISql90TSqlDatabaseOptions.AutoCreateStatistics 
		{
			get { return this.AutoCreateStatistics;}
		}
		Boolean ISql90TSqlDatabaseOptions.AutoShrink 
		{
			get { return this.AutoShrink;}
		}
		Boolean ISql90TSqlDatabaseOptions.AutoUpdateStatistics 
		{
			get { return this.AutoUpdateStatistics;}
		}
		Boolean ISql90TSqlDatabaseOptions.AutoUpdateStatisticsAsync 
		{
			get { return this.AutoUpdateStatisticsAsync;}
		}
		String ISql90TSqlDatabaseOptions.Collation 
		{
			get { return this.Collation;}
		}
		Int32 ISql90TSqlDatabaseOptions.CompatibilityLevel 
		{
			get { return this.CompatibilityLevel;}
		}
		Boolean ISql90TSqlDatabaseOptions.ConcatNullYieldsNull 
		{
			get { return this.ConcatNullYieldsNull;}
		}
		Boolean ISql90TSqlDatabaseOptions.CursorCloseOnCommit 
		{
			get { return this.CursorCloseOnCommit;}
		}
		Boolean ISql90TSqlDatabaseOptions.CursorDefaultGlobalScope 
		{
			get { return this.CursorDefaultGlobalScope;}
		}
		Boolean ISql90TSqlDatabaseOptions.DatabaseStateOffline 
		{
			get { return this.DatabaseStateOffline;}
		}
		Boolean ISql90TSqlDatabaseOptions.DateCorrelationOptimizationOn 
		{
			get { return this.DateCorrelationOptimizationOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.DBChainingOn 
		{
			get { return this.DBChainingOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.FullTextEnabled 
		{
			get { return this.FullTextEnabled;}
		}
		Boolean ISql90TSqlDatabaseOptions.MemoryOptimizedElevateToSnapshot 
		{
			get { return this.MemoryOptimizedElevateToSnapshot;}
		}
		Boolean ISql90TSqlDatabaseOptions.NumericRoundAbortOn 
		{
			get { return this.NumericRoundAbortOn;}
		}
		PageVerifyMode ISql90TSqlDatabaseOptions.PageVerifyMode 
		{
			get { return this.PageVerifyMode;}
		}
		ParameterizationOption ISql90TSqlDatabaseOptions.ParameterizationOption 
		{
			get { return this.ParameterizationOption;}
		}
		Boolean ISql90TSqlDatabaseOptions.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		RecoveryMode ISql90TSqlDatabaseOptions.RecoveryMode 
		{
			get { return this.RecoveryMode;}
		}
		Boolean ISql90TSqlDatabaseOptions.RecursiveTriggersOn 
		{
			get { return this.RecursiveTriggersOn;}
		}
		ServiceBrokerOption ISql90TSqlDatabaseOptions.ServiceBrokerOption 
		{
			get { return this.ServiceBrokerOption;}
		}
		Boolean ISql90TSqlDatabaseOptions.SupplementalLoggingOn 
		{
			get { return this.SupplementalLoggingOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.TornPageProtectionOn 
		{
			get { return this.TornPageProtectionOn;}
		}
		Boolean ISql90TSqlDatabaseOptions.TransactionIsolationReadCommittedSnapshot 
		{
			get { return this.TransactionIsolationReadCommittedSnapshot;}
		}
		Boolean ISql90TSqlDatabaseOptions.Trustworthy 
		{
			get { return this.Trustworthy;}
		}
		UserAccessOption ISql90TSqlDatabaseOptions.UserAccessOption 
		{
			get { return this.UserAccessOption;}
		}
		Boolean ISql90TSqlDatabaseOptions.VardecimalStorageFormatOn 
		{
			get { return this.VardecimalStorageFormatOn;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlDatabaseOptions.DefaultFilegroup 
		{
			get 
			{
				return this.DefaultFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDefault"/>.
	/// </summary>
	public partial class TSqlDefault : ISql90TSqlDefault
	{		
		String ISql90TSqlDefault.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlDefault.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlDefault.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlDefaultConstraint : ISql90TSqlDefaultConstraint
	{		
		Boolean ISql90TSqlDefaultConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql90TSqlDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql90TSqlDefaultConstraint.WithValues 
		{
			get { return this.WithValues;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference> ISql90TSqlDefaultConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlDmlTrigger"/>.
	/// </summary>
	public partial class TSqlDmlTrigger : ISql90TSqlDmlTrigger
	{		
		Boolean? ISql90TSqlDmlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql90TSqlDmlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		OrderRestriction ISql90TSqlDmlTrigger.DeleteOrderRestriction 
		{
			get { return this.DeleteOrderRestriction;}
		}
		Boolean ISql90TSqlDmlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql90TSqlDmlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql90TSqlDmlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlDmlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		OrderRestriction ISql90TSqlDmlTrigger.InsertOrderRestriction 
		{
			get { return this.InsertOrderRestriction;}
		}
		Boolean ISql90TSqlDmlTrigger.IsDeleteTrigger 
		{
			get { return this.IsDeleteTrigger;}
		}
		Boolean ISql90TSqlDmlTrigger.IsInsertTrigger 
		{
			get { return this.IsInsertTrigger;}
		}
		Boolean ISql90TSqlDmlTrigger.IsUpdateTrigger 
		{
			get { return this.IsUpdateTrigger;}
		}
		String ISql90TSqlDmlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean ISql90TSqlDmlTrigger.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		Boolean? ISql90TSqlDmlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql90TSqlDmlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		OrderRestriction ISql90TSqlDmlTrigger.UpdateOrderRestriction 
		{
			get { return this.UpdateOrderRestriction;}
		}
		Boolean ISql90TSqlDmlTrigger.WithAppend 
		{
			get { return this.WithAppend;}
		}
		Boolean ISql90TSqlDmlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlDmlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlDmlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlDmlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlDmlTrigger.TriggerObject 
		{
			get 
			{
				return this.TriggerObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlDmlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlEndpoint"/>.
	/// </summary>
	public partial class TSqlEndpoint : ISql90TSqlEndpoint
	{		
		Payload ISql90TSqlEndpoint.Payload 
		{
			get { return this.Payload;}
		}
		Protocol ISql90TSqlEndpoint.Protocol 
		{
			get { return this.Protocol;}
		}
		State ISql90TSqlEndpoint.State 
		{
			get { return this.State;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlEndpoint.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier> ISql90TSqlEndpoint.PayloadSpecifier 
		{
			get 
			{
				return this.PayloadSpecifier;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier > ISql90TSqlEndpoint.ProtocolSpecifier 
		{
			get 
			{
				return this.ProtocolSpecifier;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlErrorMessage"/>.
	/// </summary>
	public partial class TSqlErrorMessage : ISql90TSqlErrorMessage
	{		
		String ISql90TSqlErrorMessage.Language 
		{
			get { return this.Language;}
		}
		Int32 ISql90TSqlErrorMessage.MessageNumber 
		{
			get { return this.MessageNumber;}
		}
		String ISql90TSqlErrorMessage.MessageText 
		{
			get { return this.MessageText;}
		}
		Int32 ISql90TSqlErrorMessage.Severity 
		{
			get { return this.Severity;}
		}
		Boolean ISql90TSqlErrorMessage.WithLog 
		{
			get { return this.WithLog;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlEventGroup"/>.
	/// </summary>
	public partial class TSqlEventGroup : ISql90TSqlEventGroup
	{		
		EventGroupType ISql90TSqlEventGroup.Group 
		{
			get { return this.Group;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlEventTypeSpecifier"/>.
	/// </summary>
	public partial class TSqlEventTypeSpecifier : ISql90TSqlEventTypeSpecifier
	{		
		EventType ISql90TSqlEventTypeSpecifier.EventType 
		{
			get { return this.EventType;}
		}
		OrderRestriction ISql90TSqlEventTypeSpecifier.Order 
		{
			get { return this.Order;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlExtendedProcedure"/>.
	/// </summary>
	public partial class TSqlExtendedProcedure : ISql90TSqlExtendedProcedure
	{		
		Boolean ISql90TSqlExtendedProcedure.ExeccuteAsCaller 
		{
			get { return this.ExeccuteAsCaller;}
		}
		Boolean ISql90TSqlExtendedProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlExtendedProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql90TSqlExtendedProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlExtendedProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter> ISql90TSqlExtendedProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlExtendedProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlExtendedProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlExtendedProperty"/>.
	/// </summary>
	public partial class TSqlExtendedProperty : ISql90TSqlExtendedProperty
	{		
		String ISql90TSqlExtendedProperty.Value 
		{
			get { return this.Value;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost> ISql90TSqlExtendedProperty.Host 
		{
			get 
			{
				return this.Host;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSqlFile"/>.
	/// </summary>
	public partial class TSqlSqlFile : ISql90TSqlSqlFile
	{		
		Int32? ISql90TSqlSqlFile.FileGrowth 
		{
			get { return this.FileGrowth;}
		}
		MemoryUnit ISql90TSqlSqlFile.FileGrowthUnit 
		{
			get { return this.FileGrowthUnit;}
		}
		String ISql90TSqlSqlFile.FileName 
		{
			get { return this.FileName;}
		}
		Boolean ISql90TSqlSqlFile.IsLogFile 
		{
			get { return this.IsLogFile;}
		}
		Int32? ISql90TSqlSqlFile.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql90TSqlSqlFile.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		Boolean ISql90TSqlSqlFile.Offline 
		{
			get { return this.Offline;}
		}
		Int32? ISql90TSqlSqlFile.Size 
		{
			get { return this.Size;}
		}
		MemoryUnit ISql90TSqlSqlFile.SizeUnit 
		{
			get { return this.SizeUnit;}
		}
		Boolean ISql90TSqlSqlFile.Unlimited 
		{
			get { return this.Unlimited;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlSqlFile.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlFilegroup"/>.
	/// </summary>
	public partial class TSqlFilegroup : ISql90TSqlFilegroup
	{		
		Boolean ISql90TSqlFilegroup.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlForeignKeyConstraint"/>.
	/// </summary>
	public partial class TSqlForeignKeyConstraint : ISql90TSqlForeignKeyConstraint
	{		
		ForeignKeyAction ISql90TSqlForeignKeyConstraint.DeleteAction 
		{
			get { return this.DeleteAction;}
		}
		Boolean ISql90TSqlForeignKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql90TSqlForeignKeyConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		ForeignKeyAction ISql90TSqlForeignKeyConstraint.UpdateAction 
		{
			get { return this.UpdateAction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlForeignKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlForeignKeyConstraint.ForeignColumns 
		{
			get 
			{
				return this.ForeignColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference> ISql90TSqlForeignKeyConstraint.ForeignTable 
		{
			get 
			{
				return this.ForeignTable.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference> ISql90TSqlForeignKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlFullTextCatalog"/>.
	/// </summary>
	public partial class TSqlFullTextCatalog : ISql90TSqlFullTextCatalog
	{		
		Boolean? ISql90TSqlFullTextCatalog.AccentSensitivity 
		{
			get { return this.AccentSensitivity;}
		}
		Boolean ISql90TSqlFullTextCatalog.IsDefault 
		{
			get { return this.IsDefault;}
		}
		String ISql90TSqlFullTextCatalog.Path 
		{
			get { return this.Path;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlFullTextCatalog.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlFullTextCatalog.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlFullTextIndex"/>.
	/// </summary>
	public partial class TSqlFullTextIndex : ISql90TSqlFullTextIndex
	{		
		ChangeTrackingOption ISql90TSqlFullTextIndex.ChangeTracking 
		{
			get { return this.ChangeTracking;}
		}
		Boolean ISql90TSqlFullTextIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql90TSqlFullTextIndex.Replicated 
		{
			get { return this.Replicated;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference> ISql90TSqlFullTextIndex.Catalog 
		{
			get 
			{
				return this.Catalog;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFullTextIndexColumnSpecifier> ISql90TSqlFullTextIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFullTextIndexColumnSpecifier>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql90TSqlFullTextIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlFullTextIndex.UniqueIndexName 
		{
			get 
			{
				return this.UniqueIndexName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlFullTextIndexColumnSpecifier"/>.
	/// </summary>
	public partial class TSqlFullTextIndexColumnSpecifier : ISql90TSqlFullTextIndexColumnSpecifier
	{		
		Int32? ISql90TSqlFullTextIndexColumnSpecifier.LanguageId 
		{
			get { return this.LanguageId;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlFullTextIndexColumnSpecifier.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlFullTextIndexColumnSpecifier.TypeColumn 
		{
			get 
			{
				return this.TypeColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlHttpProtocolSpecifier"/>.
	/// </summary>
	public partial class TSqlHttpProtocolSpecifier : ISql90TSqlHttpProtocolSpecifier
	{		
		AuthenticationModes ISql90TSqlHttpProtocolSpecifier.AuthenticationMode 
		{
			get { return this.AuthenticationMode;}
		}
		String ISql90TSqlHttpProtocolSpecifier.AuthenticationRealm 
		{
			get { return this.AuthenticationRealm;}
		}
		Int32? ISql90TSqlHttpProtocolSpecifier.ClearPort 
		{
			get { return this.ClearPort;}
		}
		Boolean ISql90TSqlHttpProtocolSpecifier.CompressionEnabled 
		{
			get { return this.CompressionEnabled;}
		}
		String ISql90TSqlHttpProtocolSpecifier.DefaultLogonDomain 
		{
			get { return this.DefaultLogonDomain;}
		}
		Boolean ISql90TSqlHttpProtocolSpecifier.ListeningOnAllNoneReservedSites 
		{
			get { return this.ListeningOnAllNoneReservedSites;}
		}
		Boolean ISql90TSqlHttpProtocolSpecifier.ListeningOnAllSites 
		{
			get { return this.ListeningOnAllSites;}
		}
		String ISql90TSqlHttpProtocolSpecifier.Path 
		{
			get { return this.Path;}
		}
		HttpPorts ISql90TSqlHttpProtocolSpecifier.Ports 
		{
			get { return this.Ports;}
		}
		Int32? ISql90TSqlHttpProtocolSpecifier.SslPort 
		{
			get { return this.SslPort;}
		}
		String ISql90TSqlHttpProtocolSpecifier.Website 
		{
			get { return this.Website;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlLinkedServer"/>.
	/// </summary>
	public partial class TSqlLinkedServer : ISql90TSqlLinkedServer
	{		
		String ISql90TSqlLinkedServer.Catalog 
		{
			get { return this.Catalog;}
		}
		Boolean ISql90TSqlLinkedServer.CollationCompatible 
		{
			get { return this.CollationCompatible;}
		}
		String ISql90TSqlLinkedServer.CollationName 
		{
			get { return this.CollationName;}
		}
		Int32 ISql90TSqlLinkedServer.ConnectTimeout 
		{
			get { return this.ConnectTimeout;}
		}
		Boolean ISql90TSqlLinkedServer.DataAccess 
		{
			get { return this.DataAccess;}
		}
		String ISql90TSqlLinkedServer.DataSource 
		{
			get { return this.DataSource;}
		}
		Boolean ISql90TSqlLinkedServer.IsDistributor 
		{
			get { return this.IsDistributor;}
		}
		Boolean ISql90TSqlLinkedServer.IsPublisher 
		{
			get { return this.IsPublisher;}
		}
		Boolean ISql90TSqlLinkedServer.IsSubscriber 
		{
			get { return this.IsSubscriber;}
		}
		Boolean ISql90TSqlLinkedServer.LazySchemaValidationEnabled 
		{
			get { return this.LazySchemaValidationEnabled;}
		}
		String ISql90TSqlLinkedServer.Location 
		{
			get { return this.Location;}
		}
		String ISql90TSqlLinkedServer.ProductName 
		{
			get { return this.ProductName;}
		}
		String ISql90TSqlLinkedServer.ProviderName 
		{
			get { return this.ProviderName;}
		}
		String ISql90TSqlLinkedServer.ProviderString 
		{
			get { return this.ProviderString;}
		}
		Int32 ISql90TSqlLinkedServer.QueryTimeout 
		{
			get { return this.QueryTimeout;}
		}
		Boolean ISql90TSqlLinkedServer.RpcEnabled 
		{
			get { return this.RpcEnabled;}
		}
		Boolean ISql90TSqlLinkedServer.RpcOutEnabled 
		{
			get { return this.RpcOutEnabled;}
		}
		Boolean ISql90TSqlLinkedServer.UseRemoteCollation 
		{
			get { return this.UseRemoteCollation;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlLinkedServerLogin"/>.
	/// </summary>
	public partial class TSqlLinkedServerLogin : ISql90TSqlLinkedServerLogin
	{		
		String ISql90TSqlLinkedServerLogin.LinkedServerLoginName 
		{
			get { return this.LinkedServerLoginName;}
		}
		String ISql90TSqlLinkedServerLogin.LinkedServerPassword 
		{
			get { return this.LinkedServerPassword;}
		}
		Boolean ISql90TSqlLinkedServerLogin.UseSelf 
		{
			get { return this.UseSelf;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLinkedServerReference> ISql90TSqlLinkedServerLogin.LinkedServer 
		{
			get 
			{
				return this.LinkedServer.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLinkedServerReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlLinkedServerLogin.LocalLogin 
		{
			get 
			{
				return this.LocalLogin.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlLogin"/>.
	/// </summary>
	public partial class TSqlLogin : ISql90TSqlLogin
	{		
		Boolean ISql90TSqlLogin.CheckExpiration 
		{
			get { return this.CheckExpiration;}
		}
		Boolean ISql90TSqlLogin.CheckPolicy 
		{
			get { return this.CheckPolicy;}
		}
		String ISql90TSqlLogin.DefaultDatabase 
		{
			get { return this.DefaultDatabase;}
		}
		String ISql90TSqlLogin.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		Boolean ISql90TSqlLogin.Disabled 
		{
			get { return this.Disabled;}
		}
		LoginEncryptionOption ISql90TSqlLogin.EncryptionOption 
		{
			get { return this.EncryptionOption;}
		}
		Boolean ISql90TSqlLogin.MappedToWindowsLogin 
		{
			get { return this.MappedToWindowsLogin;}
		}
		String ISql90TSqlLogin.Password 
		{
			get { return this.Password;}
		}
		Boolean ISql90TSqlLogin.PasswordHashed 
		{
			get { return this.PasswordHashed;}
		}
		Boolean ISql90TSqlLogin.PasswordMustChange 
		{
			get { return this.PasswordMustChange;}
		}
		String ISql90TSqlLogin.Sid 
		{
			get { return this.Sid;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference> ISql90TSqlLogin.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference> ISql90TSqlLogin.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCredentialReference> ISql90TSqlLogin.Credential 
		{
			get 
			{
				return this.Credential.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCredentialReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlMasterKey"/>.
	/// </summary>
	public partial class TSqlMasterKey : ISql90TSqlMasterKey
	{		
		String ISql90TSqlMasterKey.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlMessageType"/>.
	/// </summary>
	public partial class TSqlMessageType : ISql90TSqlMessageType
	{		
		ValidationMethod ISql90TSqlMessageType.ValidationMethod 
		{
			get { return this.ValidationMethod;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlMessageType.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlSchemaCollectionReference> ISql90TSqlMessageType.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlPartitionFunction"/>.
	/// </summary>
	public partial class TSqlPartitionFunction : ISql90TSqlPartitionFunction
	{		
		PartitionRange ISql90TSqlPartitionFunction.Range 
		{
			get { return this.Range;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionValue> ISql90TSqlPartitionFunction.BoundaryValues 
		{
			get 
			{
				return this.BoundaryValues.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionValue>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference> ISql90TSqlPartitionFunction.ParameterType 
		{
			get 
			{
				return this.ParameterType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlPartitionScheme"/>.
	/// </summary>
	public partial class TSqlPartitionScheme : ISql90TSqlPartitionScheme
	{		
		Boolean ISql90TSqlPartitionScheme.AllToOneFilegroup 
		{
			get { return this.AllToOneFilegroup;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlPartitionScheme.Filegroups 
		{
			get 
			{
				return this.Filegroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionFunctionReference> ISql90TSqlPartitionScheme.PartitionFunction 
		{
			get 
			{
				return this.PartitionFunction.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionFunctionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlPartitionValue"/>.
	/// </summary>
	public partial class TSqlPartitionValue : ISql90TSqlPartitionValue
	{		
		String ISql90TSqlPartitionValue.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlPartitionValue.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlPermission"/>.
	/// </summary>
	public partial class TSqlPermission : ISql90TSqlPermission
	{		
		PermissionAction ISql90TSqlPermission.PermissionAction 
		{
			get { return this.PermissionAction;}
		}
		PermissionType ISql90TSqlPermission.PermissionType 
		{
			get { return this.PermissionType;}
		}
		Boolean ISql90TSqlPermission.WithAllPrivileges 
		{
			get { return this.WithAllPrivileges;}
		}
		Boolean ISql90TSqlPermission.WithGrantOption 
		{
			get { return this.WithGrantOption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlPermission.ExcludedColumns 
		{
			get 
			{
				return this.ExcludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql90TSqlPermission.Grantee 
		{
			get 
			{
				return this.Grantee;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql90TSqlPermission.Grantor 
		{
			get 
			{
				return this.Grantor;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlPermission.RevokedGrantOptionColumns 
		{
			get 
			{
				return this.RevokedGrantOptionColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql90TSqlPermission.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlPrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlPrimaryKeyConstraint : ISql90TSqlPrimaryKeyConstraint
	{		
		Boolean ISql90TSqlPrimaryKeyConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql90TSqlPrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql90TSqlPrimaryKeyConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql90TSqlPrimaryKeyConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlPrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlPrimaryKeyConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference> ISql90TSqlPrimaryKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlPrimaryKeyConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference> ISql90TSqlPrimaryKeyConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlProcedure"/>.
	/// </summary>
	public partial class TSqlProcedure : ISql90TSqlProcedure
	{		
		Boolean? ISql90TSqlProcedure.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql90TSqlProcedure.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql90TSqlProcedure.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql90TSqlProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql90TSqlProcedure.ForReplication 
		{
			get { return this.ForReplication;}
		}
		Boolean ISql90TSqlProcedure.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql90TSqlProcedure.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql90TSqlProcedure.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql90TSqlProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql90TSqlProcedure.WithRecompile 
		{
			get { return this.WithRecompile;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlProcedure.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlProcedure.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter> ISql90TSqlProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlProcedureReference> ISql90TSqlProcedure.ParentProcedure 
		{
			get 
			{
				return this.ParentProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlProcedureReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlQueue"/>.
	/// </summary>
	public partial class TSqlQueue : ISql90TSqlQueue
	{		
		Boolean ISql90TSqlQueue.ActivationExecuteAsCaller 
		{
			get { return this.ActivationExecuteAsCaller;}
		}
		Boolean ISql90TSqlQueue.ActivationExecuteAsOwner 
		{
			get { return this.ActivationExecuteAsOwner;}
		}
		Boolean ISql90TSqlQueue.ActivationExecuteAsSelf 
		{
			get { return this.ActivationExecuteAsSelf;}
		}
		Int32? ISql90TSqlQueue.ActivationMaxQueueReaders 
		{
			get { return this.ActivationMaxQueueReaders;}
		}
		Boolean? ISql90TSqlQueue.ActivationStatusOn 
		{
			get { return this.ActivationStatusOn;}
		}
		Boolean ISql90TSqlQueue.RetentionOn 
		{
			get { return this.RetentionOn;}
		}
		Boolean ISql90TSqlQueue.StatusOn 
		{
			get { return this.StatusOn;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlProcedureReference> ISql90TSqlQueue.ActivationProcedure 
		{
			get 
			{
				return this.ActivationProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlProcedureReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn> ISql90TSqlQueue.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlQueue.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlQueue.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlQueue.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference> ISql90TSqlQueue.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlQueue.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlQueue.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlQueueEventNotification"/>.
	/// </summary>
	public partial class TSqlQueueEventNotification : ISql90TSqlQueueEventNotification
	{		
		String ISql90TSqlQueueEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql90TSqlQueueEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql90TSqlQueueEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup> ISql90TSqlQueueEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql90TSqlQueueEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlQueueReference> ISql90TSqlQueueEventNotification.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlRemoteServiceBinding"/>.
	/// </summary>
	public partial class TSqlRemoteServiceBinding : ISql90TSqlRemoteServiceBinding
	{		
		Boolean ISql90TSqlRemoteServiceBinding.Anonymous 
		{
			get { return this.Anonymous;}
		}
		String ISql90TSqlRemoteServiceBinding.Service 
		{
			get { return this.Service;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlRemoteServiceBinding.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlRemoteServiceBinding.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlRole"/>.
	/// </summary>
	public partial class TSqlRole : ISql90TSqlRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlRoleMembership"/>.
	/// </summary>
	public partial class TSqlRoleMembership : ISql90TSqlRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql90TSqlRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlRoleReference> ISql90TSqlRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlRoute"/>.
	/// </summary>
	public partial class TSqlRoute : ISql90TSqlRoute
	{		
		String ISql90TSqlRoute.Address 
		{
			get { return this.Address;}
		}
		String ISql90TSqlRoute.BrokerInstance 
		{
			get { return this.BrokerInstance;}
		}
		Int32? ISql90TSqlRoute.Lifetime 
		{
			get { return this.Lifetime;}
		}
		String ISql90TSqlRoute.MirrorAddress 
		{
			get { return this.MirrorAddress;}
		}
		String ISql90TSqlRoute.ServiceName 
		{
			get { return this.ServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlRoute.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlRule"/>.
	/// </summary>
	public partial class TSqlRule : ISql90TSqlRule
	{		
		String ISql90TSqlRule.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlRule.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlRule.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSchema"/>.
	/// </summary>
	public partial class TSqlSchema : ISql90TSqlSchema
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlSchema.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlServerDdlTrigger"/>.
	/// </summary>
	public partial class TSqlServerDdlTrigger : ISql90TSqlServerDdlTrigger
	{		
		Boolean? ISql90TSqlServerDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql90TSqlServerDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql90TSqlServerDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql90TSqlServerDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql90TSqlServerDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql90TSqlServerDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql90TSqlServerDdlTrigger.IsLogon 
		{
			get { return this.IsLogon;}
		}
		String ISql90TSqlServerDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql90TSqlServerDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql90TSqlServerDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql90TSqlServerDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlServerDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlServerDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup> ISql90TSqlServerDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql90TSqlServerDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlServerDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference> ISql90TSqlServerDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlServerEventNotification"/>.
	/// </summary>
	public partial class TSqlServerEventNotification : ISql90TSqlServerEventNotification
	{		
		String ISql90TSqlServerEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql90TSqlServerEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql90TSqlServerEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup> ISql90TSqlServerEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql90TSqlServerEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlServerOptions"/>.
	/// </summary>
	public partial class TSqlServerOptions : ISql90TSqlServerOptions
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlServerRoleMembership"/>.
	/// </summary>
	public partial class TSqlServerRoleMembership : ISql90TSqlServerRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> ISql90TSqlServerRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlRoleReference> ISql90TSqlServerRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlService"/>.
	/// </summary>
	public partial class TSqlService : ISql90TSqlService
	{		
		Boolean ISql90TSqlService.UseDefaultContract 
		{
			get { return this.UseDefaultContract;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlService.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlContractReference> ISql90TSqlService.Contracts 
		{
			get 
			{
				return this.Contracts.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlContractReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlQueueReference> ISql90TSqlService.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlServiceBrokerLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlServiceBrokerLanguageSpecifier : ISql90TSqlServiceBrokerLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql90TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql90TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql90TSqlServiceBrokerLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		Boolean ISql90TSqlServiceBrokerLanguageSpecifier.MessageForwardingEnabled 
		{
			get { return this.MessageForwardingEnabled;}
		}
		Int32 ISql90TSqlServiceBrokerLanguageSpecifier.MessageForwardSize 
		{
			get { return this.MessageForwardSize;}
		}
		Boolean ISql90TSqlServiceBrokerLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql90TSqlServiceBrokerLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference> ISql90TSqlServiceBrokerLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSignature"/>.
	/// </summary>
	public partial class TSqlSignature : ISql90TSqlSignature
	{		
		Boolean ISql90TSqlSignature.IsCounterSignature 
		{
			get { return this.IsCounterSignature;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql90TSqlSignature.EncryptionMechanism 
		{
			get 
			{
				return this.EncryptionMechanism;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlSignature.SignedObject 
		{
			get 
			{
				return this.SignedObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSignatureEncryptionMechanism"/>.
	/// </summary>
	public partial class TSqlSignatureEncryptionMechanism : ISql90TSqlSignatureEncryptionMechanism
	{		
		String ISql90TSqlSignatureEncryptionMechanism.Password 
		{
			get { return this.Password;}
		}
		String ISql90TSqlSignatureEncryptionMechanism.SignedBlob 
		{
			get { return this.SignedBlob;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference> ISql90TSqlSignatureEncryptionMechanism.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference> ISql90TSqlSignatureEncryptionMechanism.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSoapLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlSoapLanguageSpecifier : ISql90TSqlSoapLanguageSpecifier
	{		
		Boolean ISql90TSqlSoapLanguageSpecifier.BatchesEnabled 
		{
			get { return this.BatchesEnabled;}
		}
		CharacterSet ISql90TSqlSoapLanguageSpecifier.CharacterSet 
		{
			get { return this.CharacterSet;}
		}
		String ISql90TSqlSoapLanguageSpecifier.DatabaseName 
		{
			get { return this.DatabaseName;}
		}
		Int32 ISql90TSqlSoapLanguageSpecifier.HeaderLimit 
		{
			get { return this.HeaderLimit;}
		}
		Boolean ISql90TSqlSoapLanguageSpecifier.IsDefaultDatabase 
		{
			get { return this.IsDefaultDatabase;}
		}
		Boolean ISql90TSqlSoapLanguageSpecifier.IsDefaultNamespace 
		{
			get { return this.IsDefaultNamespace;}
		}
		Boolean ISql90TSqlSoapLanguageSpecifier.IsDefaultWsdlSpName 
		{
			get { return this.IsDefaultWsdlSpName;}
		}
		SoapLoginType ISql90TSqlSoapLanguageSpecifier.LoginType 
		{
			get { return this.LoginType;}
		}
		String ISql90TSqlSoapLanguageSpecifier.Namespace 
		{
			get { return this.Namespace;}
		}
		SoapSchema ISql90TSqlSoapLanguageSpecifier.SchemaType 
		{
			get { return this.SchemaType;}
		}
		Boolean ISql90TSqlSoapLanguageSpecifier.SessionsEnabled 
		{
			get { return this.SessionsEnabled;}
		}
		Int32 ISql90TSqlSoapLanguageSpecifier.SessionTimeout 
		{
			get { return this.SessionTimeout;}
		}
		Boolean ISql90TSqlSoapLanguageSpecifier.SessionTimeoutNever 
		{
			get { return this.SessionTimeoutNever;}
		}
		String ISql90TSqlSoapLanguageSpecifier.WsdlSpName 
		{
			get { return this.WsdlSpName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSoapMethodSpecification> ISql90TSqlSoapLanguageSpecifier.WebMethods 
		{
			get 
			{
				return this.WebMethods.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSoapMethodSpecification>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSoapMethodSpecification"/>.
	/// </summary>
	public partial class TSqlSoapMethodSpecification : ISql90TSqlSoapMethodSpecification
	{		
		SoapFormat ISql90TSqlSoapMethodSpecification.Format 
		{
			get { return this.Format;}
		}
		SoapSchema ISql90TSqlSoapMethodSpecification.SchemaType 
		{
			get { return this.SchemaType;}
		}
		String ISql90TSqlSoapMethodSpecification.WebMethodAlias 
		{
			get { return this.WebMethodAlias;}
		}
		String ISql90TSqlSoapMethodSpecification.WebMethodNamespace 
		{
			get { return this.WebMethodNamespace;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlSoapMethodSpecification.RelatedMethod 
		{
			get 
			{
				return this.RelatedMethod;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlStatistics"/>.
	/// </summary>
	public partial class TSqlStatistics : ISql90TSqlStatistics
	{		
		Boolean ISql90TSqlStatistics.NoRecompute 
		{
			get { return this.NoRecompute;}
		}
		Int32 ISql90TSqlStatistics.SampleSize 
		{
			get { return this.SampleSize;}
		}
		SamplingStyle ISql90TSqlStatistics.SamplingStyle 
		{
			get { return this.SamplingStyle;}
		}
		String ISql90TSqlStatistics.StatsStream 
		{
			get { return this.StatsStream;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlStatistics.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlStatistics.OnObject 
		{
			get 
			{
				return this.OnObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlParameter"/>.
	/// </summary>
	public partial class TSqlParameter : ISql90TSqlParameter
	{		
		String ISql90TSqlParameter.DefaultExpression 
		{
			get { return this.DefaultExpression;}
		}
		Boolean ISql90TSqlParameter.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql90TSqlParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		Int32 ISql90TSqlParameter.Length 
		{
			get { return this.Length;}
		}
		Int32 ISql90TSqlParameter.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql90TSqlParameter.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql90TSqlParameter.Varying 
		{
			get { return this.Varying;}
		}
		XmlStyle ISql90TSqlParameter.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql90TSqlParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlSchemaCollectionReference> ISql90TSqlParameter.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSymmetricKey"/>.
	/// </summary>
	public partial class TSqlSymmetricKey : ISql90TSqlSymmetricKey
	{		
		SymmetricKeyAlgorithm ISql90TSqlSymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		String ISql90TSqlSymmetricKey.IdentityValue 
		{
			get { return this.IdentityValue;}
		}
		String ISql90TSqlSymmetricKey.KeySource 
		{
			get { return this.KeySource;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference> ISql90TSqlSymmetricKey.AsymmetricKeys 
		{
			get 
			{
				return this.AsymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql90TSqlSymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference> ISql90TSqlSymmetricKey.Certificates 
		{
			get 
			{
				return this.Certificates.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql90TSqlSymmetricKey.Passwords 
		{
			get 
			{
				return this.Passwords;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSymmetricKeyReference> ISql90TSqlSymmetricKey.SymmetricKeys 
		{
			get 
			{
				return this.SymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSymmetricKeyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSymmetricKeyPassword"/>.
	/// </summary>
	public partial class TSqlSymmetricKeyPassword : ISql90TSqlSymmetricKeyPassword
	{		
		String ISql90TSqlSymmetricKeyPassword.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlSynonym"/>.
	/// </summary>
	public partial class TSqlSynonym : ISql90TSqlSynonym
	{		
		String ISql90TSqlSynonym.ForObjectName 
		{
			get { return this.ForObjectName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlSynonym.ForObject 
		{
			get 
			{
				return this.ForObject;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlSynonym.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlTable"/>.
	/// </summary>
	public partial class TSqlTable : ISql90TSqlTable
	{		
		Boolean? ISql90TSqlTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Int64? ISql90TSqlTable.DataPages 
		{
			get { return this.DataPages;}
		}
		Double? ISql90TSqlTable.DataSize 
		{
			get { return this.DataSize;}
		}
		Double? ISql90TSqlTable.IndexSize 
		{
			get { return this.IndexSize;}
		}
		Boolean ISql90TSqlTable.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		Boolean ISql90TSqlTable.LargeValueTypesOutOfRow 
		{
			get { return this.LargeValueTypesOutOfRow;}
		}
		Boolean? ISql90TSqlTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Int64? ISql90TSqlTable.RowCount 
		{
			get { return this.RowCount;}
		}
		Boolean ISql90TSqlTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}
		Int32 ISql90TSqlTable.TextInRowSize 
		{
			get { return this.TextInRowSize;}
		}
		Int64? ISql90TSqlTable.UsedPages 
		{
			get { return this.UsedPages;}
		}
		Boolean ISql90TSqlTable.VardecimalStorageFormatEnabled 
		{
			get { return this.VardecimalStorageFormatEnabled;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn> ISql90TSqlTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlTable.FilegroupForTextImage 
		{
			get 
			{
				return this.FilegroupForTextImage.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference> ISql90TSqlTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlTcpProtocolSpecifier"/>.
	/// </summary>
	public partial class TSqlTcpProtocolSpecifier : ISql90TSqlTcpProtocolSpecifier
	{		
		String ISql90TSqlTcpProtocolSpecifier.ListenerIPv4 
		{
			get { return this.ListenerIPv4;}
		}
		String ISql90TSqlTcpProtocolSpecifier.ListenerIPv6 
		{
			get { return this.ListenerIPv6;}
		}
		Int32 ISql90TSqlTcpProtocolSpecifier.ListenerPort 
		{
			get { return this.ListenerPort;}
		}
		Boolean ISql90TSqlTcpProtocolSpecifier.ListeningOnAllIPs 
		{
			get { return this.ListeningOnAllIPs;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlUniqueConstraint : ISql90TSqlUniqueConstraint
	{		
		Boolean ISql90TSqlUniqueConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql90TSqlUniqueConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql90TSqlUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql90TSqlUniqueConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql90TSqlUniqueConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql90TSqlUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql90TSqlUniqueConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql90TSqlUniqueConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference> ISql90TSqlUniqueConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference> ISql90TSqlUniqueConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlUniqueConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference> ISql90TSqlUniqueConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlUser"/>.
	/// </summary>
	public partial class TSqlUser : ISql90TSqlUser
	{		
		Boolean ISql90TSqlUser.WithoutLogin 
		{
			get { return this.WithoutLogin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference> ISql90TSqlUser.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference> ISql90TSqlUser.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlUser.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference> ISql90TSqlUser.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlUserDefinedType"/>.
	/// </summary>
	public partial class TSqlUserDefinedType : ISql90TSqlUserDefinedType
	{		
		Boolean? ISql90TSqlUserDefinedType.ByteOrdered 
		{
			get { return this.ByteOrdered;}
		}
		String ISql90TSqlUserDefinedType.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean? ISql90TSqlUserDefinedType.FixedLength 
		{
			get { return this.FixedLength;}
		}
		Format ISql90TSqlUserDefinedType.Format 
		{
			get { return this.Format;}
		}
		Int32? ISql90TSqlUserDefinedType.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		String ISql90TSqlUserDefinedType.ValidationMethodName 
		{
			get { return this.ValidationMethodName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference> ISql90TSqlUserDefinedType.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql90TSqlUserDefinedType.Methods 
		{
			get 
			{
				return this.Methods;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql90TSqlUserDefinedType.Properties 
		{
			get 
			{
				return this.Properties;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlUserDefinedType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlView"/>.
	/// </summary>
	public partial class TSqlView : ISql90TSqlView
	{		
		Boolean? ISql90TSqlView.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISql90TSqlView.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql90TSqlView.Replicated 
		{
			get { return this.Replicated;}
		}
		String ISql90TSqlView.SelectStatement 
		{
			get { return this.SelectStatement;}
		}
		Boolean ISql90TSqlView.WithCheckOption 
		{
			get { return this.WithCheckOption;}
		}
		Boolean ISql90TSqlView.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql90TSqlView.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}
		Boolean ISql90TSqlView.WithViewMetadata 
		{
			get { return this.WithViewMetadata;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql90TSqlView.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn> ISql90TSqlView.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlView.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlXmlIndex"/>.
	/// </summary>
	public partial class TSqlXmlIndex : ISql90TSqlXmlIndex
	{		
		Boolean ISql90TSqlXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql90TSqlXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql90TSqlXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql90TSqlXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql90TSqlXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISql90TSqlXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		SecondaryXmlIndexType ISql90TSqlXmlIndex.SecondaryXmlIndexType 
		{
			get { return this.SecondaryXmlIndexType;}
		}
		Boolean ISql90TSqlXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference> ISql90TSqlXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql90TSqlXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlIndexReference> ISql90TSqlXmlIndex.PrimaryXmlIndex 
		{
			get 
			{
				return this.PrimaryXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlXmlIndexReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql90TSqlXmlSchemaCollection"/>.
	/// </summary>
	public partial class TSqlXmlSchemaCollection : ISql90TSqlXmlSchemaCollection
	{		
		String ISql90TSqlXmlSchemaCollection.Expression 
		{
			get { return this.Expression;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference> ISql90TSqlXmlSchemaCollection.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql90TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlColumn"/>.
	/// </summary>
	public partial class TSqlColumn : ISql100TSqlColumn
	{		
		String ISql100TSqlColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql100TSqlColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql100TSqlColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql100TSqlColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql100TSqlColumn.IsFileStream 
		{
			get { return this.IsFileStream;}
		}
		Boolean ISql100TSqlColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql100TSqlColumn.IsIdentityNotForReplication 
		{
			get { return this.IsIdentityNotForReplication;}
		}
		Boolean ISql100TSqlColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql100TSqlColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql100TSqlColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql100TSqlColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql100TSqlColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql100TSqlColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql100TSqlColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql100TSqlColumn.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql100TSqlColumn.Sparse 
		{
			get { return this.Sparse;}
		}
		XmlStyle ISql100TSqlColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql100TSqlColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference> ISql100TSqlColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableValuedFunction"/>.
	/// </summary>
	public partial class TSqlTableValuedFunction : ISql100TSqlTableValuedFunction
	{		
		Boolean? ISql100TSqlTableValuedFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql100TSqlTableValuedFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql100TSqlTableValuedFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql100TSqlTableValuedFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql100TSqlTableValuedFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql100TSqlTableValuedFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql100TSqlTableValuedFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlTableValuedFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql100TSqlTableValuedFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql100TSqlTableValuedFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql100TSqlTableValuedFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql100TSqlTableValuedFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql100TSqlTableValuedFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql100TSqlTableValuedFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		String ISql100TSqlTableValuedFunction.ReturnTableVariableName 
		{
			get { return this.ReturnTableVariableName;}
		}
		SystemDataAccessKind? ISql100TSqlTableValuedFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql100TSqlTableValuedFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql100TSqlTableValuedFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssembly> ISql100TSqlTableValuedFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlTableValuedFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn> ISql100TSqlTableValuedFunction.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlTableValuedFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter> ISql100TSqlTableValuedFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference> ISql100TSqlTableValuedFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlTableValuedFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlClrTableOption> ISql100TSqlTableValuedFunction.TableOption 
		{
			get 
			{
				return this.TableOption.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlClrTableOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlTableValuedFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlScalarFunction"/>.
	/// </summary>
	public partial class TSqlScalarFunction : ISql100TSqlScalarFunction
	{		
		Boolean? ISql100TSqlScalarFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql100TSqlScalarFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql100TSqlScalarFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql100TSqlScalarFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql100TSqlScalarFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql100TSqlScalarFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql100TSqlScalarFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlScalarFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql100TSqlScalarFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql100TSqlScalarFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql100TSqlScalarFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql100TSqlScalarFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql100TSqlScalarFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql100TSqlScalarFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		SystemDataAccessKind? ISql100TSqlScalarFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql100TSqlScalarFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql100TSqlScalarFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssembly> ISql100TSqlScalarFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlScalarFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlScalarFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter> ISql100TSqlScalarFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference> ISql100TSqlScalarFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlScalarFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlScalarFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlClrTableOption"/>.
	/// </summary>
	public partial class TSqlClrTableOption : ISql100TSqlClrTableOption
	{		
		String ISql100TSqlClrTableOption.ClassName 
		{
			get { return this.ClassName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlClrTableOption.OrderColumns 
		{
			get 
			{
				return this.OrderColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAggregate"/>.
	/// </summary>
	public partial class TSqlAggregate : ISql100TSqlAggregate
	{		
		String ISql100TSqlAggregate.ClassName 
		{
			get { return this.ClassName;}
		}
		Format ISql100TSqlAggregate.Format 
		{
			get { return this.Format;}
		}
		Boolean? ISql100TSqlAggregate.InvariantToDuplicates 
		{
			get { return this.InvariantToDuplicates;}
		}
		Boolean? ISql100TSqlAggregate.InvariantToNulls 
		{
			get { return this.InvariantToNulls;}
		}
		Int32? ISql100TSqlAggregate.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		Boolean? ISql100TSqlAggregate.NullIfEmpty 
		{
			get { return this.NullIfEmpty;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlAggregate.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter> ISql100TSqlAggregate.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference> ISql100TSqlAggregate.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlAggregate.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlApplicationRole"/>.
	/// </summary>
	public partial class TSqlApplicationRole : ISql100TSqlApplicationRole
	{		
		String ISql100TSqlApplicationRole.Password 
		{
			get { return this.Password;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlApplicationRole.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlIndex"/>.
	/// </summary>
	public partial class TSqlIndex : ISql100TSqlIndex
	{		
		Boolean ISql100TSqlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql100TSqlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql100TSqlIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql100TSqlIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql100TSqlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql100TSqlIndex.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql100TSqlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		String ISql100TSqlIndex.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISql100TSqlIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql100TSqlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql100TSqlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql100TSqlIndex.Unique 
		{
			get { return this.Unique;}
		}
		Boolean ISql100TSqlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlIndex.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption> ISql100TSqlIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlIndex.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlIndex.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlIndex.IncludedColumns 
		{
			get 
			{
				return this.IncludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql100TSqlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAssembly"/>.
	/// </summary>
	public partial class TSqlAssembly : ISql100TSqlAssembly
	{		
		AssemblyPermissionSet ISql100TSqlAssembly.PermissionSet 
		{
			get { return this.PermissionSet;}
		}
		Boolean ISql100TSqlAssembly.Visible 
		{
			get { return this.Visible;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblySource> ISql100TSqlAssembly.AssemblySources 
		{
			get 
			{
				return this.AssemblySources.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblySource>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlAssembly.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlAssembly.ReferencedAssemblies 
		{
			get 
			{
				return this.ReferencedAssemblies.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAssemblySource"/>.
	/// </summary>
	public partial class TSqlAssemblySource : ISql100TSqlAssemblySource
	{		
		String ISql100TSqlAssemblySource.Source 
		{
			get { return this.Source;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAsymmetricKey"/>.
	/// </summary>
	public partial class TSqlAsymmetricKey : ISql100TSqlAsymmetricKey
	{		
		AsymmetricKeyAlgorithm ISql100TSqlAsymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		SymmetricKeyCreationDisposition ISql100TSqlAsymmetricKey.CreationDisposition 
		{
			get { return this.CreationDisposition;}
		}
		Boolean ISql100TSqlAsymmetricKey.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql100TSqlAsymmetricKey.ExecutableFile 
		{
			get { return this.ExecutableFile;}
		}
		String ISql100TSqlAsymmetricKey.File 
		{
			get { return this.File;}
		}
		String ISql100TSqlAsymmetricKey.Password 
		{
			get { return this.Password;}
		}
		String ISql100TSqlAsymmetricKey.ProviderKeyName 
		{
			get { return this.ProviderKeyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlAsymmetricKey.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlAsymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlAsymmetricKey.Provider 
		{
			get 
			{
				return this.Provider;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAuditAction"/>.
	/// </summary>
	public partial class TSqlAuditAction : ISql100TSqlAuditAction
	{		
		DatabaseAuditAction ISql100TSqlAuditAction.Action 
		{
			get { return this.Action;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAuditActionGroup"/>.
	/// </summary>
	public partial class TSqlAuditActionGroup : ISql100TSqlAuditActionGroup
	{		
		AuditActionGroupType ISql100TSqlAuditActionGroup.ActionGroup 
		{
			get { return this.ActionGroup;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlAuditActionSpecification"/>.
	/// </summary>
	public partial class TSqlAuditActionSpecification : ISql100TSqlAuditActionSpecification
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditAction> ISql100TSqlAuditActionSpecification.AuditActions 
		{
			get 
			{
				return this.AuditActions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditAction>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql100TSqlAuditActionSpecification.Principals 
		{
			get 
			{
				return this.Principals;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql100TSqlAuditActionSpecification.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlBrokerPriority"/>.
	/// </summary>
	public partial class TSqlBrokerPriority : ISql100TSqlBrokerPriority
	{		
		Int32 ISql100TSqlBrokerPriority.PriorityLevel 
		{
			get { return this.PriorityLevel;}
		}
		String ISql100TSqlBrokerPriority.RemoteServiceName 
		{
			get { return this.RemoteServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlBrokerPriority.ContractName 
		{
			get 
			{
				return this.ContractName;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlBrokerPriority.LocalServiceName 
		{
			get 
			{
				return this.LocalServiceName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlBuiltInServerRole"/>.
	/// </summary>
	public partial class TSqlBuiltInServerRole : ISql100TSqlBuiltInServerRole
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDataType"/>.
	/// </summary>
	public partial class TSqlDataType : ISql100TSqlDataType
	{		
		SqlDataType ISql100TSqlDataType.SqlDataType 
		{
			get { return this.SqlDataType;}
		}
		Boolean ISql100TSqlDataType.UddtIsMax 
		{
			get { return this.UddtIsMax;}
		}
		Int32 ISql100TSqlDataType.UddtLength 
		{
			get { return this.UddtLength;}
		}
		Boolean ISql100TSqlDataType.UddtNullable 
		{
			get { return this.UddtNullable;}
		}
		Int32 ISql100TSqlDataType.UddtPrecision 
		{
			get { return this.UddtPrecision;}
		}
		Int32 ISql100TSqlDataType.UddtScale 
		{
			get { return this.UddtScale;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlDataType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference> ISql100TSqlDataType.Type 
		{
			get 
			{
				return this.Type.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlCertificate"/>.
	/// </summary>
	public partial class TSqlCertificate : ISql100TSqlCertificate
	{		
		Boolean ISql100TSqlCertificate.ActiveForBeginDialog 
		{
			get { return this.ActiveForBeginDialog;}
		}
		Boolean ISql100TSqlCertificate.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql100TSqlCertificate.EncryptionPassword 
		{
			get { return this.EncryptionPassword;}
		}
		String ISql100TSqlCertificate.ExistingKeysFilePath 
		{
			get { return this.ExistingKeysFilePath;}
		}
		String ISql100TSqlCertificate.ExpiryDate 
		{
			get { return this.ExpiryDate;}
		}
		Boolean ISql100TSqlCertificate.IsExistingKeyFileExecutable 
		{
			get { return this.IsExistingKeyFileExecutable;}
		}
		String ISql100TSqlCertificate.PrivateKeyDecryptionPassword 
		{
			get { return this.PrivateKeyDecryptionPassword;}
		}
		String ISql100TSqlCertificate.PrivateKeyEncryptionPassword 
		{
			get { return this.PrivateKeyEncryptionPassword;}
		}
		String ISql100TSqlCertificate.PrivateKeyFilePath 
		{
			get { return this.PrivateKeyFilePath;}
		}
		String ISql100TSqlCertificate.StartDate 
		{
			get { return this.StartDate;}
		}
		String ISql100TSqlCertificate.Subject 
		{
			get { return this.Subject;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlCertificate.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlCertificate.ExistingKeysAssembly 
		{
			get 
			{
				return this.ExistingKeysAssembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlCheckConstraint"/>.
	/// </summary>
	public partial class TSqlCheckConstraint : ISql100TSqlCheckConstraint
	{		
		Boolean ISql100TSqlCheckConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql100TSqlCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql100TSqlCheckConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference> ISql100TSqlCheckConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlClrTypeMethod"/>.
	/// </summary>
	public partial class TSqlClrTypeMethod : ISql100TSqlClrTypeMethod
	{		
		String ISql100TSqlClrTypeMethod.MethodName 
		{
			get { return this.MethodName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter> ISql100TSqlClrTypeMethod.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataType> ISql100TSqlClrTypeMethod.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataType>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlClrTypeMethodParameter"/>.
	/// </summary>
	public partial class TSqlClrTypeMethodParameter : ISql100TSqlClrTypeMethodParameter
	{		
		Boolean ISql100TSqlClrTypeMethodParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		String ISql100TSqlClrTypeMethodParameter.ParameterName 
		{
			get { return this.ParameterName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql100TSqlClrTypeMethodParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlClrTypeProperty"/>.
	/// </summary>
	public partial class TSqlClrTypeProperty : ISql100TSqlClrTypeProperty
	{		
		String ISql100TSqlClrTypeProperty.PropertyName 
		{
			get { return this.PropertyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference> ISql100TSqlClrTypeProperty.ClrType 
		{
			get 
			{
				return this.ClrType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlContract"/>.
	/// </summary>
	public partial class TSqlContract : ISql100TSqlContract
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlContract.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlMessageTypeReference> ISql100TSqlContract.Messages 
		{
			get 
			{
				return this.Messages.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlMessageTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlCredential"/>.
	/// </summary>
	public partial class TSqlCredential : ISql100TSqlCredential
	{		
		String ISql100TSqlCredential.Identity 
		{
			get { return this.Identity;}
		}
		String ISql100TSqlCredential.Secret 
		{
			get { return this.Secret;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCryptographicProviderReference> ISql100TSqlCredential.CryptographicProvider 
		{
			get 
			{
				return this.CryptographicProvider.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCryptographicProviderReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlCryptographicProvider"/>.
	/// </summary>
	public partial class TSqlCryptographicProvider : ISql100TSqlCryptographicProvider
	{		
		String ISql100TSqlCryptographicProvider.DllPath 
		{
			get { return this.DllPath;}
		}
		Boolean ISql100TSqlCryptographicProvider.Enabled 
		{
			get { return this.Enabled;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDatabaseAuditSpecification"/>.
	/// </summary>
	public partial class TSqlDatabaseAuditSpecification : ISql100TSqlDatabaseAuditSpecification
	{		
		Boolean ISql100TSqlDatabaseAuditSpecification.WithState 
		{
			get { return this.WithState;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditActionGroup> ISql100TSqlDatabaseAuditSpecification.AuditActionGroups 
		{
			get 
			{
				return this.AuditActionGroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditActionGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditAction> ISql100TSqlDatabaseAuditSpecification.AuditActions 
		{
			get 
			{
				return this.AuditActions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditAction>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlServerAuditReference> ISql100TSqlDatabaseAuditSpecification.ServerAudit 
		{
			get 
			{
				return this.ServerAudit.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlServerAuditReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDatabaseDdlTrigger"/>.
	/// </summary>
	public partial class TSqlDatabaseDdlTrigger : ISql100TSqlDatabaseDdlTrigger
	{		
		Boolean? ISql100TSqlDatabaseDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql100TSqlDatabaseDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql100TSqlDatabaseDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql100TSqlDatabaseDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql100TSqlDatabaseDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlDatabaseDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql100TSqlDatabaseDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql100TSqlDatabaseDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql100TSqlDatabaseDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql100TSqlDatabaseDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlDatabaseDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlDatabaseDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup> ISql100TSqlDatabaseDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql100TSqlDatabaseDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlDatabaseDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlDatabaseDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDatabaseEncryptionKey"/>.
	/// </summary>
	public partial class TSqlDatabaseEncryptionKey : ISql100TSqlDatabaseEncryptionKey
	{		
		SymmetricKeyAlgorithm ISql100TSqlDatabaseEncryptionKey.Algorithm 
		{
			get { return this.Algorithm;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference> ISql100TSqlDatabaseEncryptionKey.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlDatabaseEncryptionKey.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDatabaseEventNotification"/>.
	/// </summary>
	public partial class TSqlDatabaseEventNotification : ISql100TSqlDatabaseEventNotification
	{		
		String ISql100TSqlDatabaseEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql100TSqlDatabaseEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql100TSqlDatabaseEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup> ISql100TSqlDatabaseEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql100TSqlDatabaseEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDatabaseMirroringLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlDatabaseMirroringLanguageSpecifier : ISql100TSqlDatabaseMirroringLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql100TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql100TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql100TSqlDatabaseMirroringLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		DatabaseMirroringRole ISql100TSqlDatabaseMirroringLanguageSpecifier.RoleType 
		{
			get { return this.RoleType;}
		}
		Boolean ISql100TSqlDatabaseMirroringLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql100TSqlDatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlDatabaseMirroringLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDatabaseOptions"/>.
	/// </summary>
	public partial class TSqlDatabaseOptions : ISql100TSqlDatabaseOptions
	{		
		Boolean ISql100TSqlDatabaseOptions.AllowSnapshotIsolation 
		{
			get { return this.AllowSnapshotIsolation;}
		}
		Boolean ISql100TSqlDatabaseOptions.AnsiNullDefaultOn 
		{
			get { return this.AnsiNullDefaultOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.AnsiPaddingOn 
		{
			get { return this.AnsiPaddingOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.AnsiWarningsOn 
		{
			get { return this.AnsiWarningsOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.ArithAbortOn 
		{
			get { return this.ArithAbortOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.AutoClose 
		{
			get { return this.AutoClose;}
		}
		Boolean ISql100TSqlDatabaseOptions.AutoCreateStatistics 
		{
			get { return this.AutoCreateStatistics;}
		}
		Boolean ISql100TSqlDatabaseOptions.AutoShrink 
		{
			get { return this.AutoShrink;}
		}
		Boolean ISql100TSqlDatabaseOptions.AutoUpdateStatistics 
		{
			get { return this.AutoUpdateStatistics;}
		}
		Boolean ISql100TSqlDatabaseOptions.AutoUpdateStatisticsAsync 
		{
			get { return this.AutoUpdateStatisticsAsync;}
		}
		Boolean ISql100TSqlDatabaseOptions.ChangeTrackingAutoCleanup 
		{
			get { return this.ChangeTrackingAutoCleanup;}
		}
		Boolean ISql100TSqlDatabaseOptions.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int32 ISql100TSqlDatabaseOptions.ChangeTrackingRetentionPeriod 
		{
			get { return this.ChangeTrackingRetentionPeriod;}
		}
		TimeUnit ISql100TSqlDatabaseOptions.ChangeTrackingRetentionUnit 
		{
			get { return this.ChangeTrackingRetentionUnit;}
		}
		String ISql100TSqlDatabaseOptions.Collation 
		{
			get { return this.Collation;}
		}
		Int32 ISql100TSqlDatabaseOptions.CompatibilityLevel 
		{
			get { return this.CompatibilityLevel;}
		}
		Boolean ISql100TSqlDatabaseOptions.ConcatNullYieldsNull 
		{
			get { return this.ConcatNullYieldsNull;}
		}
		Boolean ISql100TSqlDatabaseOptions.CursorCloseOnCommit 
		{
			get { return this.CursorCloseOnCommit;}
		}
		Boolean ISql100TSqlDatabaseOptions.CursorDefaultGlobalScope 
		{
			get { return this.CursorDefaultGlobalScope;}
		}
		Boolean ISql100TSqlDatabaseOptions.DatabaseStateOffline 
		{
			get { return this.DatabaseStateOffline;}
		}
		Boolean ISql100TSqlDatabaseOptions.DateCorrelationOptimizationOn 
		{
			get { return this.DateCorrelationOptimizationOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.DBChainingOn 
		{
			get { return this.DBChainingOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.FullTextEnabled 
		{
			get { return this.FullTextEnabled;}
		}
		Boolean ISql100TSqlDatabaseOptions.HonorBrokerPriority 
		{
			get { return this.HonorBrokerPriority;}
		}
		Boolean ISql100TSqlDatabaseOptions.MemoryOptimizedElevateToSnapshot 
		{
			get { return this.MemoryOptimizedElevateToSnapshot;}
		}
		Boolean ISql100TSqlDatabaseOptions.NumericRoundAbortOn 
		{
			get { return this.NumericRoundAbortOn;}
		}
		PageVerifyMode ISql100TSqlDatabaseOptions.PageVerifyMode 
		{
			get { return this.PageVerifyMode;}
		}
		ParameterizationOption ISql100TSqlDatabaseOptions.ParameterizationOption 
		{
			get { return this.ParameterizationOption;}
		}
		Boolean ISql100TSqlDatabaseOptions.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		RecoveryMode ISql100TSqlDatabaseOptions.RecoveryMode 
		{
			get { return this.RecoveryMode;}
		}
		Boolean ISql100TSqlDatabaseOptions.RecursiveTriggersOn 
		{
			get { return this.RecursiveTriggersOn;}
		}
		ServiceBrokerOption ISql100TSqlDatabaseOptions.ServiceBrokerOption 
		{
			get { return this.ServiceBrokerOption;}
		}
		Boolean ISql100TSqlDatabaseOptions.SupplementalLoggingOn 
		{
			get { return this.SupplementalLoggingOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.TornPageProtectionOn 
		{
			get { return this.TornPageProtectionOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.TransactionIsolationReadCommittedSnapshot 
		{
			get { return this.TransactionIsolationReadCommittedSnapshot;}
		}
		Boolean ISql100TSqlDatabaseOptions.Trustworthy 
		{
			get { return this.Trustworthy;}
		}
		UserAccessOption ISql100TSqlDatabaseOptions.UserAccessOption 
		{
			get { return this.UserAccessOption;}
		}
		Boolean ISql100TSqlDatabaseOptions.VardecimalStorageFormatOn 
		{
			get { return this.VardecimalStorageFormatOn;}
		}
		Boolean ISql100TSqlDatabaseOptions.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlDatabaseOptions.DefaultFilegroup 
		{
			get 
			{
				return this.DefaultFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlDatabaseOptions.DefaultFileStreamFilegroup 
		{
			get 
			{
				return this.DefaultFileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDataCompressionOption"/>.
	/// </summary>
	public partial class TSqlDataCompressionOption : ISql100TSqlDataCompressionOption
	{		
		CompressionLevel ISql100TSqlDataCompressionOption.CompressionLevel 
		{
			get { return this.CompressionLevel;}
		}
		Int32 ISql100TSqlDataCompressionOption.PartitionNumber 
		{
			get { return this.PartitionNumber;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDefault"/>.
	/// </summary>
	public partial class TSqlDefault : ISql100TSqlDefault
	{		
		String ISql100TSqlDefault.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlDefault.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlDefault.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlDefaultConstraint : ISql100TSqlDefaultConstraint
	{		
		Boolean ISql100TSqlDefaultConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql100TSqlDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql100TSqlDefaultConstraint.WithValues 
		{
			get { return this.WithValues;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference> ISql100TSqlDefaultConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlDmlTrigger"/>.
	/// </summary>
	public partial class TSqlDmlTrigger : ISql100TSqlDmlTrigger
	{		
		Boolean? ISql100TSqlDmlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql100TSqlDmlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		OrderRestriction ISql100TSqlDmlTrigger.DeleteOrderRestriction 
		{
			get { return this.DeleteOrderRestriction;}
		}
		Boolean ISql100TSqlDmlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql100TSqlDmlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql100TSqlDmlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlDmlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		OrderRestriction ISql100TSqlDmlTrigger.InsertOrderRestriction 
		{
			get { return this.InsertOrderRestriction;}
		}
		Boolean ISql100TSqlDmlTrigger.IsDeleteTrigger 
		{
			get { return this.IsDeleteTrigger;}
		}
		Boolean ISql100TSqlDmlTrigger.IsInsertTrigger 
		{
			get { return this.IsInsertTrigger;}
		}
		Boolean ISql100TSqlDmlTrigger.IsUpdateTrigger 
		{
			get { return this.IsUpdateTrigger;}
		}
		String ISql100TSqlDmlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean ISql100TSqlDmlTrigger.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		Boolean? ISql100TSqlDmlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql100TSqlDmlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		OrderRestriction ISql100TSqlDmlTrigger.UpdateOrderRestriction 
		{
			get { return this.UpdateOrderRestriction;}
		}
		Boolean ISql100TSqlDmlTrigger.WithAppend 
		{
			get { return this.WithAppend;}
		}
		Boolean ISql100TSqlDmlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlDmlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlDmlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlDmlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlDmlTrigger.TriggerObject 
		{
			get 
			{
				return this.TriggerObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlDmlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEndpoint"/>.
	/// </summary>
	public partial class TSqlEndpoint : ISql100TSqlEndpoint
	{		
		Payload ISql100TSqlEndpoint.Payload 
		{
			get { return this.Payload;}
		}
		Protocol ISql100TSqlEndpoint.Protocol 
		{
			get { return this.Protocol;}
		}
		State ISql100TSqlEndpoint.State 
		{
			get { return this.State;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlEndpoint.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier> ISql100TSqlEndpoint.PayloadSpecifier 
		{
			get 
			{
				return this.PayloadSpecifier;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier > ISql100TSqlEndpoint.ProtocolSpecifier 
		{
			get 
			{
				return this.ProtocolSpecifier;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlErrorMessage"/>.
	/// </summary>
	public partial class TSqlErrorMessage : ISql100TSqlErrorMessage
	{		
		String ISql100TSqlErrorMessage.Language 
		{
			get { return this.Language;}
		}
		Int32 ISql100TSqlErrorMessage.MessageNumber 
		{
			get { return this.MessageNumber;}
		}
		String ISql100TSqlErrorMessage.MessageText 
		{
			get { return this.MessageText;}
		}
		Int32 ISql100TSqlErrorMessage.Severity 
		{
			get { return this.Severity;}
		}
		Boolean ISql100TSqlErrorMessage.WithLog 
		{
			get { return this.WithLog;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventGroup"/>.
	/// </summary>
	public partial class TSqlEventGroup : ISql100TSqlEventGroup
	{		
		EventGroupType ISql100TSqlEventGroup.Group 
		{
			get { return this.Group;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventSession"/>.
	/// </summary>
	public partial class TSqlEventSession : ISql100TSqlEventSession
	{		
		EventRetentionMode ISql100TSqlEventSession.EventRetentionMode 
		{
			get { return this.EventRetentionMode;}
		}
		Int32 ISql100TSqlEventSession.MaxDispatchLatency 
		{
			get { return this.MaxDispatchLatency;}
		}
		Int32 ISql100TSqlEventSession.MaxEventSize 
		{
			get { return this.MaxEventSize;}
		}
		MemoryUnit ISql100TSqlEventSession.MaxEventSizeUnit 
		{
			get { return this.MaxEventSizeUnit;}
		}
		Int32 ISql100TSqlEventSession.MaxMemory 
		{
			get { return this.MaxMemory;}
		}
		MemoryUnit ISql100TSqlEventSession.MaxMemoryUnit 
		{
			get { return this.MaxMemoryUnit;}
		}
		MemoryPartitionMode ISql100TSqlEventSession.MemoryPartitionMode 
		{
			get { return this.MemoryPartitionMode;}
		}
		Boolean ISql100TSqlEventSession.StartupState 
		{
			get { return this.StartupState;}
		}
		Boolean ISql100TSqlEventSession.TrackCausality 
		{
			get { return this.TrackCausality;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlEventSession.EventDefinitions 
		{
			get 
			{
				return this.EventDefinitions;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlEventSession.EventTargets 
		{
			get 
			{
				return this.EventTargets;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventSessionAction"/>.
	/// </summary>
	public partial class TSqlEventSessionAction : ISql100TSqlEventSessionAction
	{		
		String ISql100TSqlEventSessionAction.ActionName 
		{
			get { return this.ActionName;}
		}
		String ISql100TSqlEventSessionAction.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql100TSqlEventSessionAction.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventSessionDefinitions"/>.
	/// </summary>
	public partial class TSqlEventSessionDefinitions : ISql100TSqlEventSessionDefinitions
	{		
		String ISql100TSqlEventSessionDefinitions.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql100TSqlEventSessionDefinitions.EventName 
		{
			get { return this.EventName;}
		}
		String ISql100TSqlEventSessionDefinitions.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
		String ISql100TSqlEventSessionDefinitions.WhereExpression 
		{
			get { return this.WhereExpression;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventSessionAction> ISql100TSqlEventSessionDefinitions.Actions 
		{
			get 
			{
				return this.Actions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventSessionAction>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlEventSessionDefinitions.AttributeSettings 
		{
			get 
			{
				return this.AttributeSettings;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventSessionSetting"/>.
	/// </summary>
	public partial class TSqlEventSessionSetting : ISql100TSqlEventSessionSetting
	{		
		String ISql100TSqlEventSessionSetting.SettingName 
		{
			get { return this.SettingName;}
		}
		String ISql100TSqlEventSessionSetting.SettingValue 
		{
			get { return this.SettingValue;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventSessionTarget"/>.
	/// </summary>
	public partial class TSqlEventSessionTarget : ISql100TSqlEventSessionTarget
	{		
		String ISql100TSqlEventSessionTarget.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql100TSqlEventSessionTarget.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
		String ISql100TSqlEventSessionTarget.TargetName 
		{
			get { return this.TargetName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlEventSessionTarget.ParameterSettings 
		{
			get 
			{
				return this.ParameterSettings;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlEventTypeSpecifier"/>.
	/// </summary>
	public partial class TSqlEventTypeSpecifier : ISql100TSqlEventTypeSpecifier
	{		
		EventType ISql100TSqlEventTypeSpecifier.EventType 
		{
			get { return this.EventType;}
		}
		OrderRestriction ISql100TSqlEventTypeSpecifier.Order 
		{
			get { return this.Order;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlExtendedProcedure"/>.
	/// </summary>
	public partial class TSqlExtendedProcedure : ISql100TSqlExtendedProcedure
	{		
		Boolean ISql100TSqlExtendedProcedure.ExeccuteAsCaller 
		{
			get { return this.ExeccuteAsCaller;}
		}
		Boolean ISql100TSqlExtendedProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlExtendedProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql100TSqlExtendedProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlExtendedProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter> ISql100TSqlExtendedProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlExtendedProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlExtendedProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlExtendedProperty"/>.
	/// </summary>
	public partial class TSqlExtendedProperty : ISql100TSqlExtendedProperty
	{		
		String ISql100TSqlExtendedProperty.Value 
		{
			get { return this.Value;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost> ISql100TSqlExtendedProperty.Host 
		{
			get 
			{
				return this.Host;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSqlFile"/>.
	/// </summary>
	public partial class TSqlSqlFile : ISql100TSqlSqlFile
	{		
		Int32? ISql100TSqlSqlFile.FileGrowth 
		{
			get { return this.FileGrowth;}
		}
		MemoryUnit ISql100TSqlSqlFile.FileGrowthUnit 
		{
			get { return this.FileGrowthUnit;}
		}
		String ISql100TSqlSqlFile.FileName 
		{
			get { return this.FileName;}
		}
		Boolean ISql100TSqlSqlFile.IsLogFile 
		{
			get { return this.IsLogFile;}
		}
		Int32? ISql100TSqlSqlFile.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql100TSqlSqlFile.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		Boolean ISql100TSqlSqlFile.Offline 
		{
			get { return this.Offline;}
		}
		Int32? ISql100TSqlSqlFile.Size 
		{
			get { return this.Size;}
		}
		MemoryUnit ISql100TSqlSqlFile.SizeUnit 
		{
			get { return this.SizeUnit;}
		}
		Boolean ISql100TSqlSqlFile.Unlimited 
		{
			get { return this.Unlimited;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlSqlFile.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlFilegroup"/>.
	/// </summary>
	public partial class TSqlFilegroup : ISql100TSqlFilegroup
	{		
		Boolean ISql100TSqlFilegroup.ContainsFileStream 
		{
			get { return this.ContainsFileStream;}
		}
		Boolean ISql100TSqlFilegroup.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlForeignKeyConstraint"/>.
	/// </summary>
	public partial class TSqlForeignKeyConstraint : ISql100TSqlForeignKeyConstraint
	{		
		ForeignKeyAction ISql100TSqlForeignKeyConstraint.DeleteAction 
		{
			get { return this.DeleteAction;}
		}
		Boolean ISql100TSqlForeignKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql100TSqlForeignKeyConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		ForeignKeyAction ISql100TSqlForeignKeyConstraint.UpdateAction 
		{
			get { return this.UpdateAction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlForeignKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlForeignKeyConstraint.ForeignColumns 
		{
			get 
			{
				return this.ForeignColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference> ISql100TSqlForeignKeyConstraint.ForeignTable 
		{
			get 
			{
				return this.ForeignTable.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference> ISql100TSqlForeignKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlFullTextCatalog"/>.
	/// </summary>
	public partial class TSqlFullTextCatalog : ISql100TSqlFullTextCatalog
	{		
		Boolean? ISql100TSqlFullTextCatalog.AccentSensitivity 
		{
			get { return this.AccentSensitivity;}
		}
		Boolean ISql100TSqlFullTextCatalog.IsDefault 
		{
			get { return this.IsDefault;}
		}
		String ISql100TSqlFullTextCatalog.Path 
		{
			get { return this.Path;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlFullTextCatalog.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlFullTextCatalog.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlFullTextIndex"/>.
	/// </summary>
	public partial class TSqlFullTextIndex : ISql100TSqlFullTextIndex
	{		
		ChangeTrackingOption ISql100TSqlFullTextIndex.ChangeTracking 
		{
			get { return this.ChangeTracking;}
		}
		Boolean ISql100TSqlFullTextIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql100TSqlFullTextIndex.Replicated 
		{
			get { return this.Replicated;}
		}
		Boolean ISql100TSqlFullTextIndex.StopListOff 
		{
			get { return this.StopListOff;}
		}
		Boolean ISql100TSqlFullTextIndex.UseSystemStopList 
		{
			get { return this.UseSystemStopList;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference> ISql100TSqlFullTextIndex.Catalog 
		{
			get 
			{
				return this.Catalog;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFullTextIndexColumnSpecifier> ISql100TSqlFullTextIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFullTextIndexColumnSpecifier>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlFullTextIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql100TSqlFullTextIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlFullTextIndex.StopList 
		{
			get 
			{
				return this.StopList;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlFullTextIndex.UniqueIndexName 
		{
			get 
			{
				return this.UniqueIndexName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlFullTextIndexColumnSpecifier"/>.
	/// </summary>
	public partial class TSqlFullTextIndexColumnSpecifier : ISql100TSqlFullTextIndexColumnSpecifier
	{		
		Int32? ISql100TSqlFullTextIndexColumnSpecifier.LanguageId 
		{
			get { return this.LanguageId;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlFullTextIndexColumnSpecifier.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlFullTextIndexColumnSpecifier.TypeColumn 
		{
			get 
			{
				return this.TypeColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlFullTextStopList"/>.
	/// </summary>
	public partial class TSqlFullTextStopList : ISql100TSqlFullTextStopList
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlFullTextStopList.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlHttpProtocolSpecifier"/>.
	/// </summary>
	public partial class TSqlHttpProtocolSpecifier : ISql100TSqlHttpProtocolSpecifier
	{		
		AuthenticationModes ISql100TSqlHttpProtocolSpecifier.AuthenticationMode 
		{
			get { return this.AuthenticationMode;}
		}
		String ISql100TSqlHttpProtocolSpecifier.AuthenticationRealm 
		{
			get { return this.AuthenticationRealm;}
		}
		Int32? ISql100TSqlHttpProtocolSpecifier.ClearPort 
		{
			get { return this.ClearPort;}
		}
		Boolean ISql100TSqlHttpProtocolSpecifier.CompressionEnabled 
		{
			get { return this.CompressionEnabled;}
		}
		String ISql100TSqlHttpProtocolSpecifier.DefaultLogonDomain 
		{
			get { return this.DefaultLogonDomain;}
		}
		Boolean ISql100TSqlHttpProtocolSpecifier.ListeningOnAllNoneReservedSites 
		{
			get { return this.ListeningOnAllNoneReservedSites;}
		}
		Boolean ISql100TSqlHttpProtocolSpecifier.ListeningOnAllSites 
		{
			get { return this.ListeningOnAllSites;}
		}
		String ISql100TSqlHttpProtocolSpecifier.Path 
		{
			get { return this.Path;}
		}
		HttpPorts ISql100TSqlHttpProtocolSpecifier.Ports 
		{
			get { return this.Ports;}
		}
		Int32? ISql100TSqlHttpProtocolSpecifier.SslPort 
		{
			get { return this.SslPort;}
		}
		String ISql100TSqlHttpProtocolSpecifier.Website 
		{
			get { return this.Website;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlLinkedServer"/>.
	/// </summary>
	public partial class TSqlLinkedServer : ISql100TSqlLinkedServer
	{		
		String ISql100TSqlLinkedServer.Catalog 
		{
			get { return this.Catalog;}
		}
		Boolean ISql100TSqlLinkedServer.CollationCompatible 
		{
			get { return this.CollationCompatible;}
		}
		String ISql100TSqlLinkedServer.CollationName 
		{
			get { return this.CollationName;}
		}
		Int32 ISql100TSqlLinkedServer.ConnectTimeout 
		{
			get { return this.ConnectTimeout;}
		}
		Boolean ISql100TSqlLinkedServer.DataAccess 
		{
			get { return this.DataAccess;}
		}
		String ISql100TSqlLinkedServer.DataSource 
		{
			get { return this.DataSource;}
		}
		Boolean ISql100TSqlLinkedServer.IsDistributor 
		{
			get { return this.IsDistributor;}
		}
		Boolean ISql100TSqlLinkedServer.IsPublisher 
		{
			get { return this.IsPublisher;}
		}
		Boolean ISql100TSqlLinkedServer.IsSubscriber 
		{
			get { return this.IsSubscriber;}
		}
		Boolean ISql100TSqlLinkedServer.LazySchemaValidationEnabled 
		{
			get { return this.LazySchemaValidationEnabled;}
		}
		String ISql100TSqlLinkedServer.Location 
		{
			get { return this.Location;}
		}
		String ISql100TSqlLinkedServer.ProductName 
		{
			get { return this.ProductName;}
		}
		String ISql100TSqlLinkedServer.ProviderName 
		{
			get { return this.ProviderName;}
		}
		String ISql100TSqlLinkedServer.ProviderString 
		{
			get { return this.ProviderString;}
		}
		Int32 ISql100TSqlLinkedServer.QueryTimeout 
		{
			get { return this.QueryTimeout;}
		}
		Boolean ISql100TSqlLinkedServer.RemoteProcTransactionPromotionEnabled 
		{
			get { return this.RemoteProcTransactionPromotionEnabled;}
		}
		Boolean ISql100TSqlLinkedServer.RpcEnabled 
		{
			get { return this.RpcEnabled;}
		}
		Boolean ISql100TSqlLinkedServer.RpcOutEnabled 
		{
			get { return this.RpcOutEnabled;}
		}
		Boolean ISql100TSqlLinkedServer.UseRemoteCollation 
		{
			get { return this.UseRemoteCollation;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlLinkedServerLogin"/>.
	/// </summary>
	public partial class TSqlLinkedServerLogin : ISql100TSqlLinkedServerLogin
	{		
		String ISql100TSqlLinkedServerLogin.LinkedServerLoginName 
		{
			get { return this.LinkedServerLoginName;}
		}
		String ISql100TSqlLinkedServerLogin.LinkedServerPassword 
		{
			get { return this.LinkedServerPassword;}
		}
		Boolean ISql100TSqlLinkedServerLogin.UseSelf 
		{
			get { return this.UseSelf;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLinkedServerReference> ISql100TSqlLinkedServerLogin.LinkedServer 
		{
			get 
			{
				return this.LinkedServer.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLinkedServerReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlLinkedServerLogin.LocalLogin 
		{
			get 
			{
				return this.LocalLogin.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlLogin"/>.
	/// </summary>
	public partial class TSqlLogin : ISql100TSqlLogin
	{		
		Boolean ISql100TSqlLogin.CheckExpiration 
		{
			get { return this.CheckExpiration;}
		}
		Boolean ISql100TSqlLogin.CheckPolicy 
		{
			get { return this.CheckPolicy;}
		}
		String ISql100TSqlLogin.DefaultDatabase 
		{
			get { return this.DefaultDatabase;}
		}
		String ISql100TSqlLogin.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		Boolean ISql100TSqlLogin.Disabled 
		{
			get { return this.Disabled;}
		}
		LoginEncryptionOption ISql100TSqlLogin.EncryptionOption 
		{
			get { return this.EncryptionOption;}
		}
		Boolean ISql100TSqlLogin.MappedToWindowsLogin 
		{
			get { return this.MappedToWindowsLogin;}
		}
		String ISql100TSqlLogin.Password 
		{
			get { return this.Password;}
		}
		Boolean ISql100TSqlLogin.PasswordHashed 
		{
			get { return this.PasswordHashed;}
		}
		Boolean ISql100TSqlLogin.PasswordMustChange 
		{
			get { return this.PasswordMustChange;}
		}
		String ISql100TSqlLogin.Sid 
		{
			get { return this.Sid;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference> ISql100TSqlLogin.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlLogin.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCredentialReference> ISql100TSqlLogin.Credential 
		{
			get 
			{
				return this.Credential.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCredentialReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlMasterKey"/>.
	/// </summary>
	public partial class TSqlMasterKey : ISql100TSqlMasterKey
	{		
		String ISql100TSqlMasterKey.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlMessageType"/>.
	/// </summary>
	public partial class TSqlMessageType : ISql100TSqlMessageType
	{		
		ValidationMethod ISql100TSqlMessageType.ValidationMethod 
		{
			get { return this.ValidationMethod;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlMessageType.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference> ISql100TSqlMessageType.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlPartitionFunction"/>.
	/// </summary>
	public partial class TSqlPartitionFunction : ISql100TSqlPartitionFunction
	{		
		PartitionRange ISql100TSqlPartitionFunction.Range 
		{
			get { return this.Range;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionValue> ISql100TSqlPartitionFunction.BoundaryValues 
		{
			get 
			{
				return this.BoundaryValues.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionValue>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference> ISql100TSqlPartitionFunction.ParameterType 
		{
			get 
			{
				return this.ParameterType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlPartitionScheme"/>.
	/// </summary>
	public partial class TSqlPartitionScheme : ISql100TSqlPartitionScheme
	{		
		Boolean ISql100TSqlPartitionScheme.AllToOneFilegroup 
		{
			get { return this.AllToOneFilegroup;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlPartitionScheme.Filegroups 
		{
			get 
			{
				return this.Filegroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionFunctionReference> ISql100TSqlPartitionScheme.PartitionFunction 
		{
			get 
			{
				return this.PartitionFunction.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionFunctionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlPartitionValue"/>.
	/// </summary>
	public partial class TSqlPartitionValue : ISql100TSqlPartitionValue
	{		
		String ISql100TSqlPartitionValue.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlPartitionValue.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlPermission"/>.
	/// </summary>
	public partial class TSqlPermission : ISql100TSqlPermission
	{		
		PermissionAction ISql100TSqlPermission.PermissionAction 
		{
			get { return this.PermissionAction;}
		}
		PermissionType ISql100TSqlPermission.PermissionType 
		{
			get { return this.PermissionType;}
		}
		Boolean ISql100TSqlPermission.WithAllPrivileges 
		{
			get { return this.WithAllPrivileges;}
		}
		Boolean ISql100TSqlPermission.WithGrantOption 
		{
			get { return this.WithGrantOption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlPermission.ExcludedColumns 
		{
			get 
			{
				return this.ExcludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql100TSqlPermission.Grantee 
		{
			get 
			{
				return this.Grantee;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql100TSqlPermission.Grantor 
		{
			get 
			{
				return this.Grantor;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlPermission.RevokedGrantOptionColumns 
		{
			get 
			{
				return this.RevokedGrantOptionColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql100TSqlPermission.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlPrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlPrimaryKeyConstraint : ISql100TSqlPrimaryKeyConstraint
	{		
		Boolean ISql100TSqlPrimaryKeyConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql100TSqlPrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql100TSqlPrimaryKeyConstraint.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql100TSqlPrimaryKeyConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql100TSqlPrimaryKeyConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlPrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption> ISql100TSqlPrimaryKeyConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlPrimaryKeyConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlPrimaryKeyConstraint.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlPrimaryKeyConstraint.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference> ISql100TSqlPrimaryKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlPrimaryKeyConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlPrimaryKeyConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlProcedure"/>.
	/// </summary>
	public partial class TSqlProcedure : ISql100TSqlProcedure
	{		
		Boolean? ISql100TSqlProcedure.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql100TSqlProcedure.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql100TSqlProcedure.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql100TSqlProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql100TSqlProcedure.ForReplication 
		{
			get { return this.ForReplication;}
		}
		Boolean ISql100TSqlProcedure.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql100TSqlProcedure.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql100TSqlProcedure.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql100TSqlProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql100TSqlProcedure.WithRecompile 
		{
			get { return this.WithRecompile;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlProcedure.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlProcedure.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter> ISql100TSqlProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlProcedureReference> ISql100TSqlProcedure.ParentProcedure 
		{
			get 
			{
				return this.ParentProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlProcedureReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlQueue"/>.
	/// </summary>
	public partial class TSqlQueue : ISql100TSqlQueue
	{		
		Boolean ISql100TSqlQueue.ActivationExecuteAsCaller 
		{
			get { return this.ActivationExecuteAsCaller;}
		}
		Boolean ISql100TSqlQueue.ActivationExecuteAsOwner 
		{
			get { return this.ActivationExecuteAsOwner;}
		}
		Boolean ISql100TSqlQueue.ActivationExecuteAsSelf 
		{
			get { return this.ActivationExecuteAsSelf;}
		}
		Int32? ISql100TSqlQueue.ActivationMaxQueueReaders 
		{
			get { return this.ActivationMaxQueueReaders;}
		}
		Boolean? ISql100TSqlQueue.ActivationStatusOn 
		{
			get { return this.ActivationStatusOn;}
		}
		Boolean ISql100TSqlQueue.PoisonMessageHandlingStatusOn 
		{
			get { return this.PoisonMessageHandlingStatusOn;}
		}
		Boolean ISql100TSqlQueue.RetentionOn 
		{
			get { return this.RetentionOn;}
		}
		Boolean ISql100TSqlQueue.StatusOn 
		{
			get { return this.StatusOn;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlProcedureReference> ISql100TSqlQueue.ActivationProcedure 
		{
			get 
			{
				return this.ActivationProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlProcedureReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn> ISql100TSqlQueue.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlQueue.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlQueue.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlQueue.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlQueue.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlQueue.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlQueue.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlQueueEventNotification"/>.
	/// </summary>
	public partial class TSqlQueueEventNotification : ISql100TSqlQueueEventNotification
	{		
		String ISql100TSqlQueueEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql100TSqlQueueEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql100TSqlQueueEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup> ISql100TSqlQueueEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql100TSqlQueueEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlQueueReference> ISql100TSqlQueueEventNotification.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlRemoteServiceBinding"/>.
	/// </summary>
	public partial class TSqlRemoteServiceBinding : ISql100TSqlRemoteServiceBinding
	{		
		Boolean ISql100TSqlRemoteServiceBinding.Anonymous 
		{
			get { return this.Anonymous;}
		}
		String ISql100TSqlRemoteServiceBinding.Service 
		{
			get { return this.Service;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlRemoteServiceBinding.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlRemoteServiceBinding.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlResourceGovernor"/>.
	/// </summary>
	public partial class TSqlResourceGovernor : ISql100TSqlResourceGovernor
	{		
		Boolean? ISql100TSqlResourceGovernor.Enabled 
		{
			get { return this.Enabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlResourceGovernor.ClassifierFunction 
		{
			get 
			{
				return this.ClassifierFunction;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlResourcePool"/>.
	/// </summary>
	public partial class TSqlResourcePool : ISql100TSqlResourcePool
	{		
		Int32 ISql100TSqlResourcePool.MaxCpuPercent 
		{
			get { return this.MaxCpuPercent;}
		}
		Int32 ISql100TSqlResourcePool.MaxMemoryPercent 
		{
			get { return this.MaxMemoryPercent;}
		}
		Int32 ISql100TSqlResourcePool.MinCpuPercent 
		{
			get { return this.MinCpuPercent;}
		}
		Int32 ISql100TSqlResourcePool.MinMemoryPercent 
		{
			get { return this.MinMemoryPercent;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlRole"/>.
	/// </summary>
	public partial class TSqlRole : ISql100TSqlRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlRoleMembership"/>.
	/// </summary>
	public partial class TSqlRoleMembership : ISql100TSqlRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql100TSqlRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlRoleReference> ISql100TSqlRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlRoute"/>.
	/// </summary>
	public partial class TSqlRoute : ISql100TSqlRoute
	{		
		String ISql100TSqlRoute.Address 
		{
			get { return this.Address;}
		}
		String ISql100TSqlRoute.BrokerInstance 
		{
			get { return this.BrokerInstance;}
		}
		Int32? ISql100TSqlRoute.Lifetime 
		{
			get { return this.Lifetime;}
		}
		String ISql100TSqlRoute.MirrorAddress 
		{
			get { return this.MirrorAddress;}
		}
		String ISql100TSqlRoute.ServiceName 
		{
			get { return this.ServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlRoute.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlRule"/>.
	/// </summary>
	public partial class TSqlRule : ISql100TSqlRule
	{		
		String ISql100TSqlRule.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlRule.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlRule.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSchema"/>.
	/// </summary>
	public partial class TSqlSchema : ISql100TSqlSchema
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlSchema.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServerAudit"/>.
	/// </summary>
	public partial class TSqlServerAudit : ISql100TSqlServerAudit
	{		
		String ISql100TSqlServerAudit.AuditGuid 
		{
			get { return this.AuditGuid;}
		}
		AuditTarget ISql100TSqlServerAudit.AuditTarget 
		{
			get { return this.AuditTarget;}
		}
		Boolean ISql100TSqlServerAudit.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql100TSqlServerAudit.FilePath 
		{
			get { return this.FilePath;}
		}
		Int32? ISql100TSqlServerAudit.MaxRolloverFiles 
		{
			get { return this.MaxRolloverFiles;}
		}
		Int32? ISql100TSqlServerAudit.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql100TSqlServerAudit.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		FailureAction ISql100TSqlServerAudit.OnFailure 
		{
			get { return this.OnFailure;}
		}
		Int32 ISql100TSqlServerAudit.QueueDelay 
		{
			get { return this.QueueDelay;}
		}
		Boolean ISql100TSqlServerAudit.ReserveDiskSpace 
		{
			get { return this.ReserveDiskSpace;}
		}
		Boolean ISql100TSqlServerAudit.UnlimitedFileSize 
		{
			get { return this.UnlimitedFileSize;}
		}
		Boolean ISql100TSqlServerAudit.UnlimitedMaxRolloverFiles 
		{
			get { return this.UnlimitedMaxRolloverFiles;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServerAuditSpecification"/>.
	/// </summary>
	public partial class TSqlServerAuditSpecification : ISql100TSqlServerAuditSpecification
	{		
		Boolean ISql100TSqlServerAuditSpecification.StateOn 
		{
			get { return this.StateOn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditActionGroup> ISql100TSqlServerAuditSpecification.AuditActionGroups 
		{
			get 
			{
				return this.AuditActionGroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAuditActionGroup>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlServerAuditReference> ISql100TSqlServerAuditSpecification.ServerAudit 
		{
			get 
			{
				return this.ServerAudit.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlServerAuditReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServerDdlTrigger"/>.
	/// </summary>
	public partial class TSqlServerDdlTrigger : ISql100TSqlServerDdlTrigger
	{		
		Boolean? ISql100TSqlServerDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql100TSqlServerDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql100TSqlServerDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql100TSqlServerDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql100TSqlServerDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql100TSqlServerDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql100TSqlServerDdlTrigger.IsLogon 
		{
			get { return this.IsLogon;}
		}
		String ISql100TSqlServerDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql100TSqlServerDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql100TSqlServerDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql100TSqlServerDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlServerDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlServerDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup> ISql100TSqlServerDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql100TSqlServerDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlServerDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference> ISql100TSqlServerDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServerEventNotification"/>.
	/// </summary>
	public partial class TSqlServerEventNotification : ISql100TSqlServerEventNotification
	{		
		String ISql100TSqlServerEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql100TSqlServerEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql100TSqlServerEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup> ISql100TSqlServerEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql100TSqlServerEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServerOptions"/>.
	/// </summary>
	public partial class TSqlServerOptions : ISql100TSqlServerOptions
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServerRoleMembership"/>.
	/// </summary>
	public partial class TSqlServerRoleMembership : ISql100TSqlServerRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> ISql100TSqlServerRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlRoleReference> ISql100TSqlServerRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlService"/>.
	/// </summary>
	public partial class TSqlService : ISql100TSqlService
	{		
		Boolean ISql100TSqlService.UseDefaultContract 
		{
			get { return this.UseDefaultContract;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlService.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlContractReference> ISql100TSqlService.Contracts 
		{
			get 
			{
				return this.Contracts.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlContractReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlQueueReference> ISql100TSqlService.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlServiceBrokerLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlServiceBrokerLanguageSpecifier : ISql100TSqlServiceBrokerLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql100TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql100TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql100TSqlServiceBrokerLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		Boolean ISql100TSqlServiceBrokerLanguageSpecifier.MessageForwardingEnabled 
		{
			get { return this.MessageForwardingEnabled;}
		}
		Int32 ISql100TSqlServiceBrokerLanguageSpecifier.MessageForwardSize 
		{
			get { return this.MessageForwardSize;}
		}
		Boolean ISql100TSqlServiceBrokerLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql100TSqlServiceBrokerLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlServiceBrokerLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSignature"/>.
	/// </summary>
	public partial class TSqlSignature : ISql100TSqlSignature
	{		
		Boolean ISql100TSqlSignature.IsCounterSignature 
		{
			get { return this.IsCounterSignature;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlSignature.EncryptionMechanism 
		{
			get 
			{
				return this.EncryptionMechanism;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlSignature.SignedObject 
		{
			get 
			{
				return this.SignedObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSignatureEncryptionMechanism"/>.
	/// </summary>
	public partial class TSqlSignatureEncryptionMechanism : ISql100TSqlSignatureEncryptionMechanism
	{		
		String ISql100TSqlSignatureEncryptionMechanism.Password 
		{
			get { return this.Password;}
		}
		String ISql100TSqlSignatureEncryptionMechanism.SignedBlob 
		{
			get { return this.SignedBlob;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference> ISql100TSqlSignatureEncryptionMechanism.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlSignatureEncryptionMechanism.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSoapLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlSoapLanguageSpecifier : ISql100TSqlSoapLanguageSpecifier
	{		
		Boolean ISql100TSqlSoapLanguageSpecifier.BatchesEnabled 
		{
			get { return this.BatchesEnabled;}
		}
		CharacterSet ISql100TSqlSoapLanguageSpecifier.CharacterSet 
		{
			get { return this.CharacterSet;}
		}
		String ISql100TSqlSoapLanguageSpecifier.DatabaseName 
		{
			get { return this.DatabaseName;}
		}
		Int32 ISql100TSqlSoapLanguageSpecifier.HeaderLimit 
		{
			get { return this.HeaderLimit;}
		}
		Boolean ISql100TSqlSoapLanguageSpecifier.IsDefaultDatabase 
		{
			get { return this.IsDefaultDatabase;}
		}
		Boolean ISql100TSqlSoapLanguageSpecifier.IsDefaultNamespace 
		{
			get { return this.IsDefaultNamespace;}
		}
		Boolean ISql100TSqlSoapLanguageSpecifier.IsDefaultWsdlSpName 
		{
			get { return this.IsDefaultWsdlSpName;}
		}
		SoapLoginType ISql100TSqlSoapLanguageSpecifier.LoginType 
		{
			get { return this.LoginType;}
		}
		String ISql100TSqlSoapLanguageSpecifier.Namespace 
		{
			get { return this.Namespace;}
		}
		SoapSchema ISql100TSqlSoapLanguageSpecifier.SchemaType 
		{
			get { return this.SchemaType;}
		}
		Boolean ISql100TSqlSoapLanguageSpecifier.SessionsEnabled 
		{
			get { return this.SessionsEnabled;}
		}
		Int32 ISql100TSqlSoapLanguageSpecifier.SessionTimeout 
		{
			get { return this.SessionTimeout;}
		}
		Boolean ISql100TSqlSoapLanguageSpecifier.SessionTimeoutNever 
		{
			get { return this.SessionTimeoutNever;}
		}
		String ISql100TSqlSoapLanguageSpecifier.WsdlSpName 
		{
			get { return this.WsdlSpName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSoapMethodSpecification> ISql100TSqlSoapLanguageSpecifier.WebMethods 
		{
			get 
			{
				return this.WebMethods.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSoapMethodSpecification>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSoapMethodSpecification"/>.
	/// </summary>
	public partial class TSqlSoapMethodSpecification : ISql100TSqlSoapMethodSpecification
	{		
		SoapFormat ISql100TSqlSoapMethodSpecification.Format 
		{
			get { return this.Format;}
		}
		SoapSchema ISql100TSqlSoapMethodSpecification.SchemaType 
		{
			get { return this.SchemaType;}
		}
		String ISql100TSqlSoapMethodSpecification.WebMethodAlias 
		{
			get { return this.WebMethodAlias;}
		}
		String ISql100TSqlSoapMethodSpecification.WebMethodNamespace 
		{
			get { return this.WebMethodNamespace;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlSoapMethodSpecification.RelatedMethod 
		{
			get 
			{
				return this.RelatedMethod;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSpatialIndex"/>.
	/// </summary>
	public partial class TSqlSpatialIndex : ISql100TSqlSpatialIndex
	{		
		Boolean ISql100TSqlSpatialIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql100TSqlSpatialIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql100TSqlSpatialIndex.CellsPerObject 
		{
			get { return this.CellsPerObject;}
		}
		Boolean ISql100TSqlSpatialIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32 ISql100TSqlSpatialIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Degree ISql100TSqlSpatialIndex.GridLevel1Density 
		{
			get { return this.GridLevel1Density;}
		}
		Degree ISql100TSqlSpatialIndex.GridLevel2Density 
		{
			get { return this.GridLevel2Density;}
		}
		Degree ISql100TSqlSpatialIndex.GridLevel3Density 
		{
			get { return this.GridLevel3Density;}
		}
		Degree ISql100TSqlSpatialIndex.GridLevel4Density 
		{
			get { return this.GridLevel4Density;}
		}
		Boolean ISql100TSqlSpatialIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql100TSqlSpatialIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Tessellation ISql100TSqlSpatialIndex.Tessellation 
		{
			get { return this.Tessellation;}
		}
		Boolean ISql100TSqlSpatialIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}
		Double? ISql100TSqlSpatialIndex.XMax 
		{
			get { return this.XMax;}
		}
		Double? ISql100TSqlSpatialIndex.XMin 
		{
			get { return this.XMin;}
		}
		Double? ISql100TSqlSpatialIndex.YMax 
		{
			get { return this.YMax;}
		}
		Double? ISql100TSqlSpatialIndex.YMin 
		{
			get { return this.YMin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlSpatialIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlSpatialIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql100TSqlSpatialIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlSpatialIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlSpatialIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlStatistics"/>.
	/// </summary>
	public partial class TSqlStatistics : ISql100TSqlStatistics
	{		
		String ISql100TSqlStatistics.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISql100TSqlStatistics.NoRecompute 
		{
			get { return this.NoRecompute;}
		}
		Int32 ISql100TSqlStatistics.SampleSize 
		{
			get { return this.SampleSize;}
		}
		SamplingStyle ISql100TSqlStatistics.SamplingStyle 
		{
			get { return this.SamplingStyle;}
		}
		String ISql100TSqlStatistics.StatsStream 
		{
			get { return this.StatsStream;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlStatistics.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlStatistics.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlStatistics.OnObject 
		{
			get 
			{
				return this.OnObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlParameter"/>.
	/// </summary>
	public partial class TSqlParameter : ISql100TSqlParameter
	{		
		String ISql100TSqlParameter.DefaultExpression 
		{
			get { return this.DefaultExpression;}
		}
		Boolean ISql100TSqlParameter.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql100TSqlParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		Int32 ISql100TSqlParameter.Length 
		{
			get { return this.Length;}
		}
		Int32 ISql100TSqlParameter.Precision 
		{
			get { return this.Precision;}
		}
		Boolean ISql100TSqlParameter.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		Int32 ISql100TSqlParameter.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql100TSqlParameter.Varying 
		{
			get { return this.Varying;}
		}
		XmlStyle ISql100TSqlParameter.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql100TSqlParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference> ISql100TSqlParameter.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSymmetricKey"/>.
	/// </summary>
	public partial class TSqlSymmetricKey : ISql100TSqlSymmetricKey
	{		
		SymmetricKeyAlgorithm ISql100TSqlSymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		SymmetricKeyCreationDisposition ISql100TSqlSymmetricKey.CreationDisposition 
		{
			get { return this.CreationDisposition;}
		}
		String ISql100TSqlSymmetricKey.IdentityValue 
		{
			get { return this.IdentityValue;}
		}
		String ISql100TSqlSymmetricKey.KeySource 
		{
			get { return this.KeySource;}
		}
		String ISql100TSqlSymmetricKey.ProviderKeyName 
		{
			get { return this.ProviderKeyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference> ISql100TSqlSymmetricKey.AsymmetricKeys 
		{
			get 
			{
				return this.AsymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql100TSqlSymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlSymmetricKey.Certificates 
		{
			get 
			{
				return this.Certificates.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlSymmetricKey.Passwords 
		{
			get 
			{
				return this.Passwords;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlSymmetricKey.Provider 
		{
			get 
			{
				return this.Provider;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSymmetricKeyReference> ISql100TSqlSymmetricKey.SymmetricKeys 
		{
			get 
			{
				return this.SymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSymmetricKeyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSymmetricKeyPassword"/>.
	/// </summary>
	public partial class TSqlSymmetricKeyPassword : ISql100TSqlSymmetricKeyPassword
	{		
		String ISql100TSqlSymmetricKeyPassword.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlSynonym"/>.
	/// </summary>
	public partial class TSqlSynonym : ISql100TSqlSynonym
	{		
		String ISql100TSqlSynonym.ForObjectName 
		{
			get { return this.ForObjectName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlSynonym.ForObject 
		{
			get 
			{
				return this.ForObject;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlSynonym.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTable"/>.
	/// </summary>
	public partial class TSqlTable : ISql100TSqlTable
	{		
		Boolean? ISql100TSqlTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql100TSqlTable.ChangeDataCaptureEnabled 
		{
			get { return this.ChangeDataCaptureEnabled;}
		}
		Boolean ISql100TSqlTable.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int64? ISql100TSqlTable.DataPages 
		{
			get { return this.DataPages;}
		}
		Double? ISql100TSqlTable.DataSize 
		{
			get { return this.DataSize;}
		}
		Boolean? ISql100TSqlTable.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Double? ISql100TSqlTable.IndexSize 
		{
			get { return this.IndexSize;}
		}
		Boolean ISql100TSqlTable.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		Boolean ISql100TSqlTable.LargeValueTypesOutOfRow 
		{
			get { return this.LargeValueTypesOutOfRow;}
		}
		LockEscalationMethod ISql100TSqlTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean? ISql100TSqlTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Int64? ISql100TSqlTable.RowCount 
		{
			get { return this.RowCount;}
		}
		Boolean ISql100TSqlTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}
		Int32 ISql100TSqlTable.TextInRowSize 
		{
			get { return this.TextInRowSize;}
		}
		Boolean ISql100TSqlTable.TrackColumnsUpdated 
		{
			get { return this.TrackColumnsUpdated;}
		}
		Int64? ISql100TSqlTable.UsedPages 
		{
			get { return this.UsedPages;}
		}
		Boolean ISql100TSqlTable.VardecimalStorageFormatEnabled 
		{
			get { return this.VardecimalStorageFormatEnabled;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn> ISql100TSqlTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption> ISql100TSqlTable.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlTable.FilegroupForTextImage 
		{
			get 
			{
				return this.FilegroupForTextImage.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlTable.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlTable.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableType"/>.
	/// </summary>
	public partial class TSqlTableType : ISql100TSqlTableType
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableTypeColumn> ISql100TSqlTableType.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableTypeColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint> ISql100TSqlTableType.Constraints 
		{
			get 
			{
				return this.Constraints;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlTableType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableTypeCheckConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeCheckConstraint : ISql100TSqlTableTypeCheckConstraint
	{		
		String ISql100TSqlTableTypeCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlTableTypeCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableTypeColumn"/>.
	/// </summary>
	public partial class TSqlTableTypeColumn : ISql100TSqlTableTypeColumn
	{		
		String ISql100TSqlTableTypeColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql100TSqlTableTypeColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql100TSqlTableTypeColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql100TSqlTableTypeColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql100TSqlTableTypeColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql100TSqlTableTypeColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql100TSqlTableTypeColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql100TSqlTableTypeColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql100TSqlTableTypeColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql100TSqlTableTypeColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql100TSqlTableTypeColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql100TSqlTableTypeColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql100TSqlTableTypeColumn.Scale 
		{
			get { return this.Scale;}
		}
		XmlStyle ISql100TSqlTableTypeColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql100TSqlTableTypeColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlTableTypeColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference> ISql100TSqlTableTypeColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableTypeDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeDefaultConstraint : ISql100TSqlTableTypeDefaultConstraint
	{		
		String ISql100TSqlTableTypeDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlTableTypeDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlTableTypeDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableTypePrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypePrimaryKeyConstraint : ISql100TSqlTableTypePrimaryKeyConstraint
	{		
		Int32? ISql100TSqlTableTypePrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql100TSqlTableTypePrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql100TSqlTableTypePrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql100TSqlTableTypePrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlTableTypePrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTableTypeUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeUniqueConstraint : ISql100TSqlTableTypeUniqueConstraint
	{		
		Boolean ISql100TSqlTableTypeUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql100TSqlTableTypeUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlTableTypeUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlTcpProtocolSpecifier"/>.
	/// </summary>
	public partial class TSqlTcpProtocolSpecifier : ISql100TSqlTcpProtocolSpecifier
	{		
		String ISql100TSqlTcpProtocolSpecifier.ListenerIPv4 
		{
			get { return this.ListenerIPv4;}
		}
		String ISql100TSqlTcpProtocolSpecifier.ListenerIPv6 
		{
			get { return this.ListenerIPv6;}
		}
		Int32 ISql100TSqlTcpProtocolSpecifier.ListenerPort 
		{
			get { return this.ListenerPort;}
		}
		Boolean ISql100TSqlTcpProtocolSpecifier.ListeningOnAllIPs 
		{
			get { return this.ListeningOnAllIPs;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlUniqueConstraint : ISql100TSqlUniqueConstraint
	{		
		Boolean ISql100TSqlUniqueConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql100TSqlUniqueConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql100TSqlUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql100TSqlUniqueConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql100TSqlUniqueConstraint.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql100TSqlUniqueConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql100TSqlUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql100TSqlUniqueConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql100TSqlUniqueConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption> ISql100TSqlUniqueConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlUniqueConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference> ISql100TSqlUniqueConstraint.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlUniqueConstraint.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference> ISql100TSqlUniqueConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlUniqueConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference> ISql100TSqlUniqueConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlUser"/>.
	/// </summary>
	public partial class TSqlUser : ISql100TSqlUser
	{		
		Boolean ISql100TSqlUser.WithoutLogin 
		{
			get { return this.WithoutLogin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference> ISql100TSqlUser.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference> ISql100TSqlUser.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlUser.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference> ISql100TSqlUser.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlUserDefinedType"/>.
	/// </summary>
	public partial class TSqlUserDefinedType : ISql100TSqlUserDefinedType
	{		
		Boolean? ISql100TSqlUserDefinedType.ByteOrdered 
		{
			get { return this.ByteOrdered;}
		}
		String ISql100TSqlUserDefinedType.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean? ISql100TSqlUserDefinedType.FixedLength 
		{
			get { return this.FixedLength;}
		}
		Format ISql100TSqlUserDefinedType.Format 
		{
			get { return this.Format;}
		}
		Int32? ISql100TSqlUserDefinedType.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		String ISql100TSqlUserDefinedType.ValidationMethodName 
		{
			get { return this.ValidationMethodName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference> ISql100TSqlUserDefinedType.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlUserDefinedType.Methods 
		{
			get 
			{
				return this.Methods;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql100TSqlUserDefinedType.Properties 
		{
			get 
			{
				return this.Properties;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlUserDefinedType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlView"/>.
	/// </summary>
	public partial class TSqlView : ISql100TSqlView
	{		
		Boolean? ISql100TSqlView.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISql100TSqlView.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql100TSqlView.Replicated 
		{
			get { return this.Replicated;}
		}
		String ISql100TSqlView.SelectStatement 
		{
			get { return this.SelectStatement;}
		}
		Boolean ISql100TSqlView.WithCheckOption 
		{
			get { return this.WithCheckOption;}
		}
		Boolean ISql100TSqlView.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql100TSqlView.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}
		Boolean ISql100TSqlView.WithViewMetadata 
		{
			get { return this.WithViewMetadata;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql100TSqlView.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn> ISql100TSqlView.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlView.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlWorkloadGroup"/>.
	/// </summary>
	public partial class TSqlWorkloadGroup : ISql100TSqlWorkloadGroup
	{		
		Int32 ISql100TSqlWorkloadGroup.GroupMaxRequests 
		{
			get { return this.GroupMaxRequests;}
		}
		Degree ISql100TSqlWorkloadGroup.Importance 
		{
			get { return this.Importance;}
		}
		Int32 ISql100TSqlWorkloadGroup.MaxDop 
		{
			get { return this.MaxDop;}
		}
		Int32 ISql100TSqlWorkloadGroup.RequestMaxCpuTimeSec 
		{
			get { return this.RequestMaxCpuTimeSec;}
		}
		Int32 ISql100TSqlWorkloadGroup.RequestMaxMemoryGrantPercent 
		{
			get { return this.RequestMaxMemoryGrantPercent;}
		}
		Int32 ISql100TSqlWorkloadGroup.RequestMemoryGrantTimeoutSec 
		{
			get { return this.RequestMemoryGrantTimeoutSec;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlResourcePoolReference> ISql100TSqlWorkloadGroup.ResourcePool 
		{
			get 
			{
				return this.ResourcePool.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlResourcePoolReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlXmlIndex"/>.
	/// </summary>
	public partial class TSqlXmlIndex : ISql100TSqlXmlIndex
	{		
		Boolean ISql100TSqlXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql100TSqlXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql100TSqlXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql100TSqlXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql100TSqlXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql100TSqlXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISql100TSqlXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		SecondaryXmlIndexType ISql100TSqlXmlIndex.SecondaryXmlIndexType 
		{
			get { return this.SecondaryXmlIndexType;}
		}
		Boolean ISql100TSqlXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference> ISql100TSqlXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql100TSqlXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlIndexReference> ISql100TSqlXmlIndex.PrimaryXmlIndex 
		{
			get 
			{
				return this.PrimaryXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlXmlIndexReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql100TSqlXmlSchemaCollection"/>.
	/// </summary>
	public partial class TSqlXmlSchemaCollection : ISql100TSqlXmlSchemaCollection
	{		
		String ISql100TSqlXmlSchemaCollection.Expression 
		{
			get { return this.Expression;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference> ISql100TSqlXmlSchemaCollection.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql100TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlColumn"/>.
	/// </summary>
	public partial class TSqlColumn : ISqlAzureTSqlColumn
	{		
		String ISqlAzureTSqlColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISqlAzureTSqlColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISqlAzureTSqlColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISqlAzureTSqlColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISqlAzureTSqlColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISqlAzureTSqlColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Int32 ISqlAzureTSqlColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISqlAzureTSqlColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISqlAzureTSqlColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISqlAzureTSqlColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISqlAzureTSqlColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISqlAzureTSqlColumn.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISqlAzureTSqlColumn.Sparse 
		{
			get { return this.Sparse;}
		}
		XmlStyle ISqlAzureTSqlColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureTSqlColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlXmlSchemaCollectionReference> ISqlAzureTSqlColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableValuedFunction"/>.
	/// </summary>
	public partial class TSqlTableValuedFunction : ISqlAzureTSqlTableValuedFunction
	{		
		Boolean? ISqlAzureTSqlTableValuedFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISqlAzureTSqlTableValuedFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISqlAzureTSqlTableValuedFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISqlAzureTSqlTableValuedFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureTSqlTableValuedFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureTSqlTableValuedFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureTSqlTableValuedFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureTSqlTableValuedFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureTSqlTableValuedFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		String ISqlAzureTSqlTableValuedFunction.ReturnTableVariableName 
		{
			get { return this.ReturnTableVariableName;}
		}
		Boolean ISqlAzureTSqlTableValuedFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssembly> ISqlAzureTSqlTableValuedFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlTableValuedFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumn> ISqlAzureTSqlTableValuedFunction.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference> ISqlAzureTSqlTableValuedFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter> ISqlAzureTSqlTableValuedFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference> ISqlAzureTSqlTableValuedFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlTableValuedFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlClrTableOption> ISqlAzureTSqlTableValuedFunction.TableOption 
		{
			get 
			{
				return this.TableOption.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlClrTableOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference> ISqlAzureTSqlTableValuedFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlScalarFunction"/>.
	/// </summary>
	public partial class TSqlScalarFunction : ISqlAzureTSqlScalarFunction
	{		
		Boolean? ISqlAzureTSqlScalarFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISqlAzureTSqlScalarFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISqlAzureTSqlScalarFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISqlAzureTSqlScalarFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureTSqlScalarFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureTSqlScalarFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureTSqlScalarFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureTSqlScalarFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureTSqlScalarFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		Boolean ISqlAzureTSqlScalarFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssembly> ISqlAzureTSqlScalarFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlScalarFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference> ISqlAzureTSqlScalarFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter> ISqlAzureTSqlScalarFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference> ISqlAzureTSqlScalarFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlScalarFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference> ISqlAzureTSqlScalarFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlIndex"/>.
	/// </summary>
	public partial class TSqlIndex : ISqlAzureTSqlIndex
	{		
		Int32? ISqlAzureTSqlIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureTSqlIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureTSqlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureTSqlIndex.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISqlAzureTSqlIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureTSqlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureTSqlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISqlAzureTSqlIndex.Unique 
		{
			get { return this.Unique;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlIndex.IncludedColumns 
		{
			get 
			{
				return this.IncludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureTSqlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlAssembly"/>.
	/// </summary>
	public partial class TSqlAssembly : ISqlAzureTSqlAssembly
	{		
		AssemblyPermissionSet ISqlAzureTSqlAssembly.PermissionSet 
		{
			get { return this.PermissionSet;}
		}
		Boolean ISqlAzureTSqlAssembly.Visible 
		{
			get { return this.Visible;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblySource> ISqlAzureTSqlAssembly.AssemblySources 
		{
			get 
			{
				return this.AssemblySources.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblySource>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureTSqlAssembly.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference> ISqlAzureTSqlAssembly.ReferencedAssemblies 
		{
			get 
			{
				return this.ReferencedAssemblies.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlAssemblySource"/>.
	/// </summary>
	public partial class TSqlAssemblySource : ISqlAzureTSqlAssemblySource
	{		
		String ISqlAzureTSqlAssemblySource.Source 
		{
			get { return this.Source;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlBuiltInServerRole"/>.
	/// </summary>
	public partial class TSqlBuiltInServerRole : ISqlAzureTSqlBuiltInServerRole
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlDataType"/>.
	/// </summary>
	public partial class TSqlDataType : ISqlAzureTSqlDataType
	{		
		SqlDataType ISqlAzureTSqlDataType.SqlDataType 
		{
			get { return this.SqlDataType;}
		}
		Boolean ISqlAzureTSqlDataType.UddtIsMax 
		{
			get { return this.UddtIsMax;}
		}
		Int32 ISqlAzureTSqlDataType.UddtLength 
		{
			get { return this.UddtLength;}
		}
		Boolean ISqlAzureTSqlDataType.UddtNullable 
		{
			get { return this.UddtNullable;}
		}
		Int32 ISqlAzureTSqlDataType.UddtPrecision 
		{
			get { return this.UddtPrecision;}
		}
		Int32 ISqlAzureTSqlDataType.UddtScale 
		{
			get { return this.UddtScale;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlDataType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference> ISqlAzureTSqlDataType.Type 
		{
			get 
			{
				return this.Type.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlCheckConstraint"/>.
	/// </summary>
	public partial class TSqlCheckConstraint : ISqlAzureTSqlCheckConstraint
	{		
		Boolean ISqlAzureTSqlCheckConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureTSqlCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference> ISqlAzureTSqlCheckConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlClrTypeMethod"/>.
	/// </summary>
	public partial class TSqlClrTypeMethod : ISqlAzureTSqlClrTypeMethod
	{		
		String ISqlAzureTSqlClrTypeMethod.MethodName 
		{
			get { return this.MethodName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter> ISqlAzureTSqlClrTypeMethod.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataType> ISqlAzureTSqlClrTypeMethod.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataType>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlClrTypeMethodParameter"/>.
	/// </summary>
	public partial class TSqlClrTypeMethodParameter : ISqlAzureTSqlClrTypeMethodParameter
	{		
		Boolean ISqlAzureTSqlClrTypeMethodParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		String ISqlAzureTSqlClrTypeMethodParameter.ParameterName 
		{
			get { return this.ParameterName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureTSqlClrTypeMethodParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlClrTypeProperty"/>.
	/// </summary>
	public partial class TSqlClrTypeProperty : ISqlAzureTSqlClrTypeProperty
	{		
		String ISqlAzureTSqlClrTypeProperty.PropertyName 
		{
			get { return this.PropertyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference> ISqlAzureTSqlClrTypeProperty.ClrType 
		{
			get 
			{
				return this.ClrType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlDatabaseDdlTrigger"/>.
	/// </summary>
	public partial class TSqlDatabaseDdlTrigger : ISqlAzureTSqlDatabaseDdlTrigger
	{		
		Boolean? ISqlAzureTSqlDatabaseDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISqlAzureTSqlDatabaseDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISqlAzureTSqlDatabaseDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureTSqlDatabaseDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureTSqlDatabaseDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureTSqlDatabaseDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureTSqlDatabaseDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureTSqlDatabaseDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISqlAzureTSqlDatabaseDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference> ISqlAzureTSqlDatabaseDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlDatabaseDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlEventGroup> ISqlAzureTSqlDatabaseDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISqlAzureTSqlDatabaseDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference> ISqlAzureTSqlDatabaseDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference> ISqlAzureTSqlDatabaseDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlDatabaseOptions"/>.
	/// </summary>
	public partial class TSqlDatabaseOptions : ISqlAzureTSqlDatabaseOptions
	{		
		String ISqlAzureTSqlDatabaseOptions.Collation 
		{
			get { return this.Collation;}
		}
		Boolean ISqlAzureTSqlDatabaseOptions.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlDefaultConstraint : ISqlAzureTSqlDefaultConstraint
	{		
		Boolean ISqlAzureTSqlDefaultConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureTSqlDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISqlAzureTSqlDefaultConstraint.WithValues 
		{
			get { return this.WithValues;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference> ISqlAzureTSqlDefaultConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlDmlTrigger"/>.
	/// </summary>
	public partial class TSqlDmlTrigger : ISqlAzureTSqlDmlTrigger
	{		
		Boolean? ISqlAzureTSqlDmlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISqlAzureTSqlDmlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		OrderRestriction ISqlAzureTSqlDmlTrigger.DeleteOrderRestriction 
		{
			get { return this.DeleteOrderRestriction;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		OrderRestriction ISqlAzureTSqlDmlTrigger.InsertOrderRestriction 
		{
			get { return this.InsertOrderRestriction;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.IsDeleteTrigger 
		{
			get { return this.IsDeleteTrigger;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.IsInsertTrigger 
		{
			get { return this.IsInsertTrigger;}
		}
		Boolean ISqlAzureTSqlDmlTrigger.IsUpdateTrigger 
		{
			get { return this.IsUpdateTrigger;}
		}
		String ISqlAzureTSqlDmlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureTSqlDmlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISqlAzureTSqlDmlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		OrderRestriction ISqlAzureTSqlDmlTrigger.UpdateOrderRestriction 
		{
			get { return this.UpdateOrderRestriction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference> ISqlAzureTSqlDmlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlDmlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference> ISqlAzureTSqlDmlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlDmlTrigger.TriggerObject 
		{
			get 
			{
				return this.TriggerObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference> ISqlAzureTSqlDmlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlEventGroup"/>.
	/// </summary>
	public partial class TSqlEventGroup : ISqlAzureTSqlEventGroup
	{		
		EventGroupType ISqlAzureTSqlEventGroup.Group 
		{
			get { return this.Group;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlEventTypeSpecifier"/>.
	/// </summary>
	public partial class TSqlEventTypeSpecifier : ISqlAzureTSqlEventTypeSpecifier
	{		
		EventType ISqlAzureTSqlEventTypeSpecifier.EventType 
		{
			get { return this.EventType;}
		}
		OrderRestriction ISqlAzureTSqlEventTypeSpecifier.Order 
		{
			get { return this.Order;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlForeignKeyConstraint"/>.
	/// </summary>
	public partial class TSqlForeignKeyConstraint : ISqlAzureTSqlForeignKeyConstraint
	{		
		ForeignKeyAction ISqlAzureTSqlForeignKeyConstraint.DeleteAction 
		{
			get { return this.DeleteAction;}
		}
		Boolean ISqlAzureTSqlForeignKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		ForeignKeyAction ISqlAzureTSqlForeignKeyConstraint.UpdateAction 
		{
			get { return this.UpdateAction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlForeignKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlForeignKeyConstraint.ForeignColumns 
		{
			get 
			{
				return this.ForeignColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference> ISqlAzureTSqlForeignKeyConstraint.ForeignTable 
		{
			get 
			{
				return this.ForeignTable.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference> ISqlAzureTSqlForeignKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlFullTextIndexColumnSpecifier"/>.
	/// </summary>
	public partial class TSqlFullTextIndexColumnSpecifier : ISqlAzureTSqlFullTextIndexColumnSpecifier
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlLogin"/>.
	/// </summary>
	public partial class TSqlLogin : ISqlAzureTSqlLogin
	{		
		Boolean ISqlAzureTSqlLogin.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureTSqlLogin.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlPermission"/>.
	/// </summary>
	public partial class TSqlPermission : ISqlAzureTSqlPermission
	{		
		PermissionAction ISqlAzureTSqlPermission.PermissionAction 
		{
			get { return this.PermissionAction;}
		}
		PermissionType ISqlAzureTSqlPermission.PermissionType 
		{
			get { return this.PermissionType;}
		}
		Boolean ISqlAzureTSqlPermission.WithAllPrivileges 
		{
			get { return this.WithAllPrivileges;}
		}
		Boolean ISqlAzureTSqlPermission.WithGrantOption 
		{
			get { return this.WithGrantOption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlPermission.ExcludedColumns 
		{
			get 
			{
				return this.ExcludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISqlAzureTSqlPermission.Grantee 
		{
			get 
			{
				return this.Grantee;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISqlAzureTSqlPermission.Grantor 
		{
			get 
			{
				return this.Grantor;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlPermission.RevokedGrantOptionColumns 
		{
			get 
			{
				return this.RevokedGrantOptionColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISqlAzureTSqlPermission.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlPrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlPrimaryKeyConstraint : ISqlAzureTSqlPrimaryKeyConstraint
	{		
		Int32? ISqlAzureTSqlPrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureTSqlPrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureTSqlPrimaryKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureTSqlPrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureTSqlPrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureTSqlPrimaryKeyConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlPrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference> ISqlAzureTSqlPrimaryKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlProcedure"/>.
	/// </summary>
	public partial class TSqlProcedure : ISqlAzureTSqlProcedure
	{		
		Boolean? ISqlAzureTSqlProcedure.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISqlAzureTSqlProcedure.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISqlAzureTSqlProcedure.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureTSqlProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureTSqlProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureTSqlProcedure.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureTSqlProcedure.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureTSqlProcedure.WithRecompile 
		{
			get { return this.WithRecompile;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference> ISqlAzureTSqlProcedure.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlProcedure.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference> ISqlAzureTSqlProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter> ISqlAzureTSqlProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlParameter>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference> ISqlAzureTSqlProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlRole"/>.
	/// </summary>
	public partial class TSqlRole : ISqlAzureTSqlRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureTSqlRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlRoleMembership"/>.
	/// </summary>
	public partial class TSqlRoleMembership : ISqlAzureTSqlRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISqlAzureTSqlRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlRoleReference> ISqlAzureTSqlRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlSchema"/>.
	/// </summary>
	public partial class TSqlSchema : ISqlAzureTSqlSchema
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureTSqlSchema.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlServerOptions"/>.
	/// </summary>
	public partial class TSqlServerOptions : ISqlAzureTSqlServerOptions
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlServerRoleMembership"/>.
	/// </summary>
	public partial class TSqlServerRoleMembership : ISqlAzureTSqlServerRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> ISqlAzureTSqlServerRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlRoleReference> ISqlAzureTSqlServerRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlSpatialIndex"/>.
	/// </summary>
	public partial class TSqlSpatialIndex : ISqlAzureTSqlSpatialIndex
	{		
		Int32? ISqlAzureTSqlSpatialIndex.CellsPerObject 
		{
			get { return this.CellsPerObject;}
		}
		Boolean ISqlAzureTSqlSpatialIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Degree ISqlAzureTSqlSpatialIndex.GridLevel1Density 
		{
			get { return this.GridLevel1Density;}
		}
		Degree ISqlAzureTSqlSpatialIndex.GridLevel2Density 
		{
			get { return this.GridLevel2Density;}
		}
		Degree ISqlAzureTSqlSpatialIndex.GridLevel3Density 
		{
			get { return this.GridLevel3Density;}
		}
		Degree ISqlAzureTSqlSpatialIndex.GridLevel4Density 
		{
			get { return this.GridLevel4Density;}
		}
		Boolean ISqlAzureTSqlSpatialIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureTSqlSpatialIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Tessellation ISqlAzureTSqlSpatialIndex.Tessellation 
		{
			get { return this.Tessellation;}
		}
		Double? ISqlAzureTSqlSpatialIndex.XMax 
		{
			get { return this.XMax;}
		}
		Double? ISqlAzureTSqlSpatialIndex.XMin 
		{
			get { return this.XMin;}
		}
		Double? ISqlAzureTSqlSpatialIndex.YMax 
		{
			get { return this.YMax;}
		}
		Double? ISqlAzureTSqlSpatialIndex.YMin 
		{
			get { return this.YMin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlSpatialIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureTSqlSpatialIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlStatistics"/>.
	/// </summary>
	public partial class TSqlStatistics : ISqlAzureTSqlStatistics
	{		
		String ISqlAzureTSqlStatistics.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISqlAzureTSqlStatistics.NoRecompute 
		{
			get { return this.NoRecompute;}
		}
		Int32 ISqlAzureTSqlStatistics.SampleSize 
		{
			get { return this.SampleSize;}
		}
		SamplingStyle ISqlAzureTSqlStatistics.SamplingStyle 
		{
			get { return this.SamplingStyle;}
		}
		String ISqlAzureTSqlStatistics.StatsStream 
		{
			get { return this.StatsStream;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlStatistics.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlStatistics.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlStatistics.OnObject 
		{
			get 
			{
				return this.OnObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlParameter"/>.
	/// </summary>
	public partial class TSqlParameter : ISqlAzureTSqlParameter
	{		
		String ISqlAzureTSqlParameter.DefaultExpression 
		{
			get { return this.DefaultExpression;}
		}
		Boolean ISqlAzureTSqlParameter.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISqlAzureTSqlParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		Int32 ISqlAzureTSqlParameter.Length 
		{
			get { return this.Length;}
		}
		Int32 ISqlAzureTSqlParameter.Precision 
		{
			get { return this.Precision;}
		}
		Boolean ISqlAzureTSqlParameter.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		Int32 ISqlAzureTSqlParameter.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISqlAzureTSqlParameter.Varying 
		{
			get { return this.Varying;}
		}
		XmlStyle ISqlAzureTSqlParameter.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureTSqlParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlXmlSchemaCollectionReference> ISqlAzureTSqlParameter.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlSynonym"/>.
	/// </summary>
	public partial class TSqlSynonym : ISqlAzureTSqlSynonym
	{		
		String ISqlAzureTSqlSynonym.ForObjectName 
		{
			get { return this.ForObjectName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlSynonym.ForObject 
		{
			get 
			{
				return this.ForObject;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlSynonym.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTable"/>.
	/// </summary>
	public partial class TSqlTable : ISqlAzureTSqlTable
	{		
		Boolean? ISqlAzureTSqlTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Int64? ISqlAzureTSqlTable.DataPages 
		{
			get { return this.DataPages;}
		}
		Double? ISqlAzureTSqlTable.DataSize 
		{
			get { return this.DataSize;}
		}
		Double? ISqlAzureTSqlTable.IndexSize 
		{
			get { return this.IndexSize;}
		}
		Boolean ISqlAzureTSqlTable.LargeValueTypesOutOfRow 
		{
			get { return this.LargeValueTypesOutOfRow;}
		}
		LockEscalationMethod ISqlAzureTSqlTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean? ISqlAzureTSqlTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Int64? ISqlAzureTSqlTable.RowCount 
		{
			get { return this.RowCount;}
		}
		Boolean ISqlAzureTSqlTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}
		Int32 ISqlAzureTSqlTable.TextInRowSize 
		{
			get { return this.TextInRowSize;}
		}
		Int64? ISqlAzureTSqlTable.UsedPages 
		{
			get { return this.UsedPages;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumn> ISqlAzureTSqlTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableType"/>.
	/// </summary>
	public partial class TSqlTableType : ISqlAzureTSqlTableType
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableTypeColumn> ISqlAzureTSqlTableType.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableTypeColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint> ISqlAzureTSqlTableType.Constraints 
		{
			get 
			{
				return this.Constraints;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlTableType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableTypeCheckConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeCheckConstraint : ISqlAzureTSqlTableTypeCheckConstraint
	{		
		String ISqlAzureTSqlTableTypeCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlTableTypeCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableTypeColumn"/>.
	/// </summary>
	public partial class TSqlTableTypeColumn : ISqlAzureTSqlTableTypeColumn
	{		
		String ISqlAzureTSqlTableTypeColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISqlAzureTSqlTableTypeColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISqlAzureTSqlTableTypeColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISqlAzureTSqlTableTypeColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISqlAzureTSqlTableTypeColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISqlAzureTSqlTableTypeColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Int32 ISqlAzureTSqlTableTypeColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISqlAzureTSqlTableTypeColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISqlAzureTSqlTableTypeColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISqlAzureTSqlTableTypeColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISqlAzureTSqlTableTypeColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISqlAzureTSqlTableTypeColumn.Scale 
		{
			get { return this.Scale;}
		}
		XmlStyle ISqlAzureTSqlTableTypeColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureTSqlTableTypeColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlTableTypeColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlXmlSchemaCollectionReference> ISqlAzureTSqlTableTypeColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableTypeDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeDefaultConstraint : ISqlAzureTSqlTableTypeDefaultConstraint
	{		
		String ISqlAzureTSqlTableTypeDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlTableTypeDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlTableTypeDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableTypePrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypePrimaryKeyConstraint : ISqlAzureTSqlTableTypePrimaryKeyConstraint
	{		
		Int32? ISqlAzureTSqlTableTypePrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureTSqlTableTypePrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureTSqlTableTypePrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureTSqlTableTypePrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlTableTypePrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlTableTypeUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeUniqueConstraint : ISqlAzureTSqlTableTypeUniqueConstraint
	{		
		Boolean ISqlAzureTSqlTableTypeUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureTSqlTableTypeUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlTableTypeUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlUniqueConstraint : ISqlAzureTSqlUniqueConstraint
	{		
		Boolean ISqlAzureTSqlUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureTSqlUniqueConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureTSqlUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureTSqlUniqueConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference> ISqlAzureTSqlUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference> ISqlAzureTSqlUniqueConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlUser"/>.
	/// </summary>
	public partial class TSqlUser : ISqlAzureTSqlUser
	{		
		Boolean ISqlAzureTSqlUser.WithoutLogin 
		{
			get { return this.WithoutLogin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlUser.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference> ISqlAzureTSqlUser.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlUserDefinedType"/>.
	/// </summary>
	public partial class TSqlUserDefinedType : ISqlAzureTSqlUserDefinedType
	{		
		Boolean? ISqlAzureTSqlUserDefinedType.ByteOrdered 
		{
			get { return this.ByteOrdered;}
		}
		String ISqlAzureTSqlUserDefinedType.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean? ISqlAzureTSqlUserDefinedType.FixedLength 
		{
			get { return this.FixedLength;}
		}
		Format ISqlAzureTSqlUserDefinedType.Format 
		{
			get { return this.Format;}
		}
		Int32? ISqlAzureTSqlUserDefinedType.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		String ISqlAzureTSqlUserDefinedType.ValidationMethodName 
		{
			get { return this.ValidationMethodName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference> ISqlAzureTSqlUserDefinedType.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISqlAzureTSqlUserDefinedType.Methods 
		{
			get 
			{
				return this.Methods;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISqlAzureTSqlUserDefinedType.Properties 
		{
			get 
			{
				return this.Properties;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlUserDefinedType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureTSqlView"/>.
	/// </summary>
	public partial class TSqlView : ISqlAzureTSqlView
	{		
		Boolean? ISqlAzureTSqlView.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISqlAzureTSqlView.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		String ISqlAzureTSqlView.SelectStatement 
		{
			get { return this.SelectStatement;}
		}
		Boolean ISqlAzureTSqlView.WithCheckOption 
		{
			get { return this.WithCheckOption;}
		}
		Boolean ISqlAzureTSqlView.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}
		Boolean ISqlAzureTSqlView.WithViewMetadata 
		{
			get { return this.WithViewMetadata;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureTSqlView.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumn> ISqlAzureTSqlView.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference> ISqlAzureTSqlView.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureTSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlColumn"/>.
	/// </summary>
	public partial class TSqlColumn : ISql110TSqlColumn
	{		
		String ISql110TSqlColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql110TSqlColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql110TSqlColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql110TSqlColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql110TSqlColumn.IsFileStream 
		{
			get { return this.IsFileStream;}
		}
		Boolean ISql110TSqlColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql110TSqlColumn.IsIdentityNotForReplication 
		{
			get { return this.IsIdentityNotForReplication;}
		}
		Boolean ISql110TSqlColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql110TSqlColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql110TSqlColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql110TSqlColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql110TSqlColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql110TSqlColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql110TSqlColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql110TSqlColumn.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql110TSqlColumn.Sparse 
		{
			get { return this.Sparse;}
		}
		XmlStyle ISql110TSqlColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql110TSqlColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference> ISql110TSqlColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableValuedFunction"/>.
	/// </summary>
	public partial class TSqlTableValuedFunction : ISql110TSqlTableValuedFunction
	{		
		Boolean? ISql110TSqlTableValuedFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql110TSqlTableValuedFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql110TSqlTableValuedFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql110TSqlTableValuedFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql110TSqlTableValuedFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql110TSqlTableValuedFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql110TSqlTableValuedFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlTableValuedFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql110TSqlTableValuedFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql110TSqlTableValuedFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql110TSqlTableValuedFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql110TSqlTableValuedFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql110TSqlTableValuedFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql110TSqlTableValuedFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		String ISql110TSqlTableValuedFunction.ReturnTableVariableName 
		{
			get { return this.ReturnTableVariableName;}
		}
		SystemDataAccessKind? ISql110TSqlTableValuedFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql110TSqlTableValuedFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql110TSqlTableValuedFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssembly> ISql110TSqlTableValuedFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlTableValuedFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn> ISql110TSqlTableValuedFunction.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlTableValuedFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter> ISql110TSqlTableValuedFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlTableValuedFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlTableValuedFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlClrTableOption> ISql110TSqlTableValuedFunction.TableOption 
		{
			get 
			{
				return this.TableOption.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlClrTableOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlTableValuedFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlScalarFunction"/>.
	/// </summary>
	public partial class TSqlScalarFunction : ISql110TSqlScalarFunction
	{		
		Boolean? ISql110TSqlScalarFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql110TSqlScalarFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql110TSqlScalarFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql110TSqlScalarFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql110TSqlScalarFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql110TSqlScalarFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql110TSqlScalarFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlScalarFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql110TSqlScalarFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql110TSqlScalarFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql110TSqlScalarFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql110TSqlScalarFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql110TSqlScalarFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql110TSqlScalarFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		SystemDataAccessKind? ISql110TSqlScalarFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql110TSqlScalarFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql110TSqlScalarFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssembly> ISql110TSqlScalarFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlScalarFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlScalarFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter> ISql110TSqlScalarFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlScalarFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlScalarFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlScalarFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlClrTableOption"/>.
	/// </summary>
	public partial class TSqlClrTableOption : ISql110TSqlClrTableOption
	{		
		String ISql110TSqlClrTableOption.ClassName 
		{
			get { return this.ClassName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlClrTableOption.OrderColumns 
		{
			get 
			{
				return this.OrderColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAggregate"/>.
	/// </summary>
	public partial class TSqlAggregate : ISql110TSqlAggregate
	{		
		String ISql110TSqlAggregate.ClassName 
		{
			get { return this.ClassName;}
		}
		Format ISql110TSqlAggregate.Format 
		{
			get { return this.Format;}
		}
		Boolean? ISql110TSqlAggregate.InvariantToDuplicates 
		{
			get { return this.InvariantToDuplicates;}
		}
		Boolean? ISql110TSqlAggregate.InvariantToNulls 
		{
			get { return this.InvariantToNulls;}
		}
		Int32? ISql110TSqlAggregate.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		Boolean? ISql110TSqlAggregate.NullIfEmpty 
		{
			get { return this.NullIfEmpty;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlAggregate.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter> ISql110TSqlAggregate.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlAggregate.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlAggregate.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlApplicationRole"/>.
	/// </summary>
	public partial class TSqlApplicationRole : ISql110TSqlApplicationRole
	{		
		String ISql110TSqlApplicationRole.Password 
		{
			get { return this.Password;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlApplicationRole.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlIndex"/>.
	/// </summary>
	public partial class TSqlIndex : ISql110TSqlIndex
	{		
		Boolean ISql110TSqlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql110TSqlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql110TSqlIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql110TSqlIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql110TSqlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql110TSqlIndex.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql110TSqlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		String ISql110TSqlIndex.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISql110TSqlIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql110TSqlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql110TSqlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql110TSqlIndex.Unique 
		{
			get { return this.Unique;}
		}
		Boolean ISql110TSqlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlIndex.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption> ISql110TSqlIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlIndex.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlIndex.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlIndex.IncludedColumns 
		{
			get 
			{
				return this.IncludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql110TSqlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAssembly"/>.
	/// </summary>
	public partial class TSqlAssembly : ISql110TSqlAssembly
	{		
		AssemblyPermissionSet ISql110TSqlAssembly.PermissionSet 
		{
			get { return this.PermissionSet;}
		}
		Boolean ISql110TSqlAssembly.Visible 
		{
			get { return this.Visible;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblySource> ISql110TSqlAssembly.AssemblySources 
		{
			get 
			{
				return this.AssemblySources.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblySource>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlAssembly.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlAssembly.ReferencedAssemblies 
		{
			get 
			{
				return this.ReferencedAssemblies.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAssemblySource"/>.
	/// </summary>
	public partial class TSqlAssemblySource : ISql110TSqlAssemblySource
	{		
		String ISql110TSqlAssemblySource.Source 
		{
			get { return this.Source;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAsymmetricKey"/>.
	/// </summary>
	public partial class TSqlAsymmetricKey : ISql110TSqlAsymmetricKey
	{		
		AsymmetricKeyAlgorithm ISql110TSqlAsymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		SymmetricKeyCreationDisposition ISql110TSqlAsymmetricKey.CreationDisposition 
		{
			get { return this.CreationDisposition;}
		}
		Boolean ISql110TSqlAsymmetricKey.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql110TSqlAsymmetricKey.ExecutableFile 
		{
			get { return this.ExecutableFile;}
		}
		String ISql110TSqlAsymmetricKey.File 
		{
			get { return this.File;}
		}
		String ISql110TSqlAsymmetricKey.Password 
		{
			get { return this.Password;}
		}
		String ISql110TSqlAsymmetricKey.ProviderKeyName 
		{
			get { return this.ProviderKeyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlAsymmetricKey.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlAsymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlAsymmetricKey.Provider 
		{
			get 
			{
				return this.Provider;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAuditAction"/>.
	/// </summary>
	public partial class TSqlAuditAction : ISql110TSqlAuditAction
	{		
		DatabaseAuditAction ISql110TSqlAuditAction.Action 
		{
			get { return this.Action;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAuditActionGroup"/>.
	/// </summary>
	public partial class TSqlAuditActionGroup : ISql110TSqlAuditActionGroup
	{		
		AuditActionGroupType ISql110TSqlAuditActionGroup.ActionGroup 
		{
			get { return this.ActionGroup;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlAuditActionSpecification"/>.
	/// </summary>
	public partial class TSqlAuditActionSpecification : ISql110TSqlAuditActionSpecification
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditAction> ISql110TSqlAuditActionSpecification.AuditActions 
		{
			get 
			{
				return this.AuditActions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditAction>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql110TSqlAuditActionSpecification.Principals 
		{
			get 
			{
				return this.Principals;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql110TSqlAuditActionSpecification.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlBrokerPriority"/>.
	/// </summary>
	public partial class TSqlBrokerPriority : ISql110TSqlBrokerPriority
	{		
		Int32 ISql110TSqlBrokerPriority.PriorityLevel 
		{
			get { return this.PriorityLevel;}
		}
		String ISql110TSqlBrokerPriority.RemoteServiceName 
		{
			get { return this.RemoteServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlBrokerPriority.ContractName 
		{
			get 
			{
				return this.ContractName;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlBrokerPriority.LocalServiceName 
		{
			get 
			{
				return this.LocalServiceName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlBuiltInServerRole"/>.
	/// </summary>
	public partial class TSqlBuiltInServerRole : ISql110TSqlBuiltInServerRole
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDataType"/>.
	/// </summary>
	public partial class TSqlDataType : ISql110TSqlDataType
	{		
		SqlDataType ISql110TSqlDataType.SqlDataType 
		{
			get { return this.SqlDataType;}
		}
		Boolean ISql110TSqlDataType.UddtIsMax 
		{
			get { return this.UddtIsMax;}
		}
		Int32 ISql110TSqlDataType.UddtLength 
		{
			get { return this.UddtLength;}
		}
		Boolean ISql110TSqlDataType.UddtNullable 
		{
			get { return this.UddtNullable;}
		}
		Int32 ISql110TSqlDataType.UddtPrecision 
		{
			get { return this.UddtPrecision;}
		}
		Int32 ISql110TSqlDataType.UddtScale 
		{
			get { return this.UddtScale;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlDataType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlDataType.Type 
		{
			get 
			{
				return this.Type.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlCertificate"/>.
	/// </summary>
	public partial class TSqlCertificate : ISql110TSqlCertificate
	{		
		Boolean ISql110TSqlCertificate.ActiveForBeginDialog 
		{
			get { return this.ActiveForBeginDialog;}
		}
		Boolean ISql110TSqlCertificate.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql110TSqlCertificate.EncryptionPassword 
		{
			get { return this.EncryptionPassword;}
		}
		String ISql110TSqlCertificate.ExistingKeysFilePath 
		{
			get { return this.ExistingKeysFilePath;}
		}
		String ISql110TSqlCertificate.ExpiryDate 
		{
			get { return this.ExpiryDate;}
		}
		Boolean ISql110TSqlCertificate.IsExistingKeyFileExecutable 
		{
			get { return this.IsExistingKeyFileExecutable;}
		}
		String ISql110TSqlCertificate.PrivateKeyDecryptionPassword 
		{
			get { return this.PrivateKeyDecryptionPassword;}
		}
		String ISql110TSqlCertificate.PrivateKeyEncryptionPassword 
		{
			get { return this.PrivateKeyEncryptionPassword;}
		}
		String ISql110TSqlCertificate.PrivateKeyFilePath 
		{
			get { return this.PrivateKeyFilePath;}
		}
		String ISql110TSqlCertificate.StartDate 
		{
			get { return this.StartDate;}
		}
		String ISql110TSqlCertificate.Subject 
		{
			get { return this.Subject;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlCertificate.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlCertificate.ExistingKeysAssembly 
		{
			get 
			{
				return this.ExistingKeysAssembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlCheckConstraint"/>.
	/// </summary>
	public partial class TSqlCheckConstraint : ISql110TSqlCheckConstraint
	{		
		Boolean ISql110TSqlCheckConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql110TSqlCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql110TSqlCheckConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference> ISql110TSqlCheckConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlClrTypeMethod"/>.
	/// </summary>
	public partial class TSqlClrTypeMethod : ISql110TSqlClrTypeMethod
	{		
		String ISql110TSqlClrTypeMethod.MethodName 
		{
			get { return this.MethodName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter> ISql110TSqlClrTypeMethod.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataType> ISql110TSqlClrTypeMethod.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataType>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlClrTypeMethodParameter"/>.
	/// </summary>
	public partial class TSqlClrTypeMethodParameter : ISql110TSqlClrTypeMethodParameter
	{		
		Boolean ISql110TSqlClrTypeMethodParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		String ISql110TSqlClrTypeMethodParameter.ParameterName 
		{
			get { return this.ParameterName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql110TSqlClrTypeMethodParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlClrTypeProperty"/>.
	/// </summary>
	public partial class TSqlClrTypeProperty : ISql110TSqlClrTypeProperty
	{		
		String ISql110TSqlClrTypeProperty.PropertyName 
		{
			get { return this.PropertyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlClrTypeProperty.ClrType 
		{
			get 
			{
				return this.ClrType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlColumnStoreIndex"/>.
	/// </summary>
	public partial class TSqlColumnStoreIndex : ISql110TSqlColumnStoreIndex
	{		
		Boolean ISql110TSqlColumnStoreIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql110TSqlColumnStoreIndex.Disabled 
		{
			get { return this.Disabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlColumnStoreIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption> ISql110TSqlColumnStoreIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlColumnStoreIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql110TSqlColumnStoreIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlColumnStoreIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlColumnStoreIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlContract"/>.
	/// </summary>
	public partial class TSqlContract : ISql110TSqlContract
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlContract.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlMessageTypeReference> ISql110TSqlContract.Messages 
		{
			get 
			{
				return this.Messages.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlMessageTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlCredential"/>.
	/// </summary>
	public partial class TSqlCredential : ISql110TSqlCredential
	{		
		String ISql110TSqlCredential.Identity 
		{
			get { return this.Identity;}
		}
		String ISql110TSqlCredential.Secret 
		{
			get { return this.Secret;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCryptographicProviderReference> ISql110TSqlCredential.CryptographicProvider 
		{
			get 
			{
				return this.CryptographicProvider.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCryptographicProviderReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlCryptographicProvider"/>.
	/// </summary>
	public partial class TSqlCryptographicProvider : ISql110TSqlCryptographicProvider
	{		
		String ISql110TSqlCryptographicProvider.DllPath 
		{
			get { return this.DllPath;}
		}
		Boolean ISql110TSqlCryptographicProvider.Enabled 
		{
			get { return this.Enabled;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDatabaseAuditSpecification"/>.
	/// </summary>
	public partial class TSqlDatabaseAuditSpecification : ISql110TSqlDatabaseAuditSpecification
	{		
		Boolean ISql110TSqlDatabaseAuditSpecification.WithState 
		{
			get { return this.WithState;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditActionGroup> ISql110TSqlDatabaseAuditSpecification.AuditActionGroups 
		{
			get 
			{
				return this.AuditActionGroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditActionGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditAction> ISql110TSqlDatabaseAuditSpecification.AuditActions 
		{
			get 
			{
				return this.AuditActions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditAction>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlServerAuditReference> ISql110TSqlDatabaseAuditSpecification.ServerAudit 
		{
			get 
			{
				return this.ServerAudit.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlServerAuditReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDatabaseDdlTrigger"/>.
	/// </summary>
	public partial class TSqlDatabaseDdlTrigger : ISql110TSqlDatabaseDdlTrigger
	{		
		Boolean? ISql110TSqlDatabaseDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql110TSqlDatabaseDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql110TSqlDatabaseDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql110TSqlDatabaseDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql110TSqlDatabaseDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlDatabaseDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql110TSqlDatabaseDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql110TSqlDatabaseDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql110TSqlDatabaseDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql110TSqlDatabaseDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlDatabaseDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlDatabaseDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup> ISql110TSqlDatabaseDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql110TSqlDatabaseDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlDatabaseDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlDatabaseDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDatabaseEncryptionKey"/>.
	/// </summary>
	public partial class TSqlDatabaseEncryptionKey : ISql110TSqlDatabaseEncryptionKey
	{		
		SymmetricKeyAlgorithm ISql110TSqlDatabaseEncryptionKey.Algorithm 
		{
			get { return this.Algorithm;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference> ISql110TSqlDatabaseEncryptionKey.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlDatabaseEncryptionKey.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDatabaseEventNotification"/>.
	/// </summary>
	public partial class TSqlDatabaseEventNotification : ISql110TSqlDatabaseEventNotification
	{		
		String ISql110TSqlDatabaseEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql110TSqlDatabaseEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql110TSqlDatabaseEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup> ISql110TSqlDatabaseEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql110TSqlDatabaseEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDatabaseMirroringLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlDatabaseMirroringLanguageSpecifier : ISql110TSqlDatabaseMirroringLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql110TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql110TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql110TSqlDatabaseMirroringLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		DatabaseMirroringRole ISql110TSqlDatabaseMirroringLanguageSpecifier.RoleType 
		{
			get { return this.RoleType;}
		}
		Boolean ISql110TSqlDatabaseMirroringLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql110TSqlDatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlDatabaseMirroringLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDatabaseOptions"/>.
	/// </summary>
	public partial class TSqlDatabaseOptions : ISql110TSqlDatabaseOptions
	{		
		Boolean ISql110TSqlDatabaseOptions.AllowSnapshotIsolation 
		{
			get { return this.AllowSnapshotIsolation;}
		}
		Boolean ISql110TSqlDatabaseOptions.AnsiNullDefaultOn 
		{
			get { return this.AnsiNullDefaultOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.AnsiPaddingOn 
		{
			get { return this.AnsiPaddingOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.AnsiWarningsOn 
		{
			get { return this.AnsiWarningsOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.ArithAbortOn 
		{
			get { return this.ArithAbortOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.AutoClose 
		{
			get { return this.AutoClose;}
		}
		Boolean ISql110TSqlDatabaseOptions.AutoCreateStatistics 
		{
			get { return this.AutoCreateStatistics;}
		}
		Boolean ISql110TSqlDatabaseOptions.AutoShrink 
		{
			get { return this.AutoShrink;}
		}
		Boolean ISql110TSqlDatabaseOptions.AutoUpdateStatistics 
		{
			get { return this.AutoUpdateStatistics;}
		}
		Boolean ISql110TSqlDatabaseOptions.AutoUpdateStatisticsAsync 
		{
			get { return this.AutoUpdateStatisticsAsync;}
		}
		Boolean ISql110TSqlDatabaseOptions.ChangeTrackingAutoCleanup 
		{
			get { return this.ChangeTrackingAutoCleanup;}
		}
		Boolean ISql110TSqlDatabaseOptions.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int32 ISql110TSqlDatabaseOptions.ChangeTrackingRetentionPeriod 
		{
			get { return this.ChangeTrackingRetentionPeriod;}
		}
		TimeUnit ISql110TSqlDatabaseOptions.ChangeTrackingRetentionUnit 
		{
			get { return this.ChangeTrackingRetentionUnit;}
		}
		String ISql110TSqlDatabaseOptions.Collation 
		{
			get { return this.Collation;}
		}
		Int32 ISql110TSqlDatabaseOptions.CompatibilityLevel 
		{
			get { return this.CompatibilityLevel;}
		}
		Boolean ISql110TSqlDatabaseOptions.ConcatNullYieldsNull 
		{
			get { return this.ConcatNullYieldsNull;}
		}
		Containment ISql110TSqlDatabaseOptions.Containment 
		{
			get { return this.Containment;}
		}
		Boolean ISql110TSqlDatabaseOptions.CursorCloseOnCommit 
		{
			get { return this.CursorCloseOnCommit;}
		}
		Boolean ISql110TSqlDatabaseOptions.CursorDefaultGlobalScope 
		{
			get { return this.CursorDefaultGlobalScope;}
		}
		Boolean ISql110TSqlDatabaseOptions.DatabaseStateOffline 
		{
			get { return this.DatabaseStateOffline;}
		}
		Boolean ISql110TSqlDatabaseOptions.DateCorrelationOptimizationOn 
		{
			get { return this.DateCorrelationOptimizationOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.DBChainingOn 
		{
			get { return this.DBChainingOn;}
		}
		String ISql110TSqlDatabaseOptions.DefaultFullTextLanguage 
		{
			get { return this.DefaultFullTextLanguage;}
		}
		String ISql110TSqlDatabaseOptions.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		String ISql110TSqlDatabaseOptions.FileStreamDirectoryName 
		{
			get { return this.FileStreamDirectoryName;}
		}
		Boolean ISql110TSqlDatabaseOptions.FullTextEnabled 
		{
			get { return this.FullTextEnabled;}
		}
		Boolean ISql110TSqlDatabaseOptions.HonorBrokerPriority 
		{
			get { return this.HonorBrokerPriority;}
		}
		Boolean ISql110TSqlDatabaseOptions.MemoryOptimizedElevateToSnapshot 
		{
			get { return this.MemoryOptimizedElevateToSnapshot;}
		}
		Boolean ISql110TSqlDatabaseOptions.NestedTriggersOn 
		{
			get { return this.NestedTriggersOn;}
		}
		NonTransactedFileStreamAccess ISql110TSqlDatabaseOptions.NonTransactedFileStreamAccess 
		{
			get { return this.NonTransactedFileStreamAccess;}
		}
		Boolean ISql110TSqlDatabaseOptions.NumericRoundAbortOn 
		{
			get { return this.NumericRoundAbortOn;}
		}
		PageVerifyMode ISql110TSqlDatabaseOptions.PageVerifyMode 
		{
			get { return this.PageVerifyMode;}
		}
		ParameterizationOption ISql110TSqlDatabaseOptions.ParameterizationOption 
		{
			get { return this.ParameterizationOption;}
		}
		Boolean ISql110TSqlDatabaseOptions.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		RecoveryMode ISql110TSqlDatabaseOptions.RecoveryMode 
		{
			get { return this.RecoveryMode;}
		}
		Boolean ISql110TSqlDatabaseOptions.RecursiveTriggersOn 
		{
			get { return this.RecursiveTriggersOn;}
		}
		ServiceBrokerOption ISql110TSqlDatabaseOptions.ServiceBrokerOption 
		{
			get { return this.ServiceBrokerOption;}
		}
		Boolean ISql110TSqlDatabaseOptions.SupplementalLoggingOn 
		{
			get { return this.SupplementalLoggingOn;}
		}
		Int32 ISql110TSqlDatabaseOptions.TargetRecoveryTimePeriod 
		{
			get { return this.TargetRecoveryTimePeriod;}
		}
		TimeUnit ISql110TSqlDatabaseOptions.TargetRecoveryTimeUnit 
		{
			get { return this.TargetRecoveryTimeUnit;}
		}
		Boolean ISql110TSqlDatabaseOptions.TornPageProtectionOn 
		{
			get { return this.TornPageProtectionOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.TransactionIsolationReadCommittedSnapshot 
		{
			get { return this.TransactionIsolationReadCommittedSnapshot;}
		}
		Boolean ISql110TSqlDatabaseOptions.TransformNoiseWords 
		{
			get { return this.TransformNoiseWords;}
		}
		Boolean ISql110TSqlDatabaseOptions.Trustworthy 
		{
			get { return this.Trustworthy;}
		}
		Int16 ISql110TSqlDatabaseOptions.TwoDigitYearCutoff 
		{
			get { return this.TwoDigitYearCutoff;}
		}
		UserAccessOption ISql110TSqlDatabaseOptions.UserAccessOption 
		{
			get { return this.UserAccessOption;}
		}
		Boolean ISql110TSqlDatabaseOptions.VardecimalStorageFormatOn 
		{
			get { return this.VardecimalStorageFormatOn;}
		}
		Boolean ISql110TSqlDatabaseOptions.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlDatabaseOptions.DefaultFilegroup 
		{
			get 
			{
				return this.DefaultFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlDatabaseOptions.DefaultFileStreamFilegroup 
		{
			get 
			{
				return this.DefaultFileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDataCompressionOption"/>.
	/// </summary>
	public partial class TSqlDataCompressionOption : ISql110TSqlDataCompressionOption
	{		
		CompressionLevel ISql110TSqlDataCompressionOption.CompressionLevel 
		{
			get { return this.CompressionLevel;}
		}
		Int32 ISql110TSqlDataCompressionOption.PartitionNumber 
		{
			get { return this.PartitionNumber;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDefault"/>.
	/// </summary>
	public partial class TSqlDefault : ISql110TSqlDefault
	{		
		String ISql110TSqlDefault.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlDefault.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlDefault.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlDefaultConstraint : ISql110TSqlDefaultConstraint
	{		
		Boolean ISql110TSqlDefaultConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql110TSqlDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql110TSqlDefaultConstraint.WithValues 
		{
			get { return this.WithValues;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference> ISql110TSqlDefaultConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlDmlTrigger"/>.
	/// </summary>
	public partial class TSqlDmlTrigger : ISql110TSqlDmlTrigger
	{		
		Boolean? ISql110TSqlDmlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql110TSqlDmlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		OrderRestriction ISql110TSqlDmlTrigger.DeleteOrderRestriction 
		{
			get { return this.DeleteOrderRestriction;}
		}
		Boolean ISql110TSqlDmlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql110TSqlDmlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql110TSqlDmlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlDmlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		OrderRestriction ISql110TSqlDmlTrigger.InsertOrderRestriction 
		{
			get { return this.InsertOrderRestriction;}
		}
		Boolean ISql110TSqlDmlTrigger.IsDeleteTrigger 
		{
			get { return this.IsDeleteTrigger;}
		}
		Boolean ISql110TSqlDmlTrigger.IsInsertTrigger 
		{
			get { return this.IsInsertTrigger;}
		}
		Boolean ISql110TSqlDmlTrigger.IsUpdateTrigger 
		{
			get { return this.IsUpdateTrigger;}
		}
		String ISql110TSqlDmlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean ISql110TSqlDmlTrigger.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		Boolean? ISql110TSqlDmlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql110TSqlDmlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		OrderRestriction ISql110TSqlDmlTrigger.UpdateOrderRestriction 
		{
			get { return this.UpdateOrderRestriction;}
		}
		Boolean ISql110TSqlDmlTrigger.WithAppend 
		{
			get { return this.WithAppend;}
		}
		Boolean ISql110TSqlDmlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlDmlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlDmlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlDmlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlDmlTrigger.TriggerObject 
		{
			get 
			{
				return this.TriggerObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlDmlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEndpoint"/>.
	/// </summary>
	public partial class TSqlEndpoint : ISql110TSqlEndpoint
	{		
		Payload ISql110TSqlEndpoint.Payload 
		{
			get { return this.Payload;}
		}
		Protocol ISql110TSqlEndpoint.Protocol 
		{
			get { return this.Protocol;}
		}
		State ISql110TSqlEndpoint.State 
		{
			get { return this.State;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlEndpoint.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier> ISql110TSqlEndpoint.PayloadSpecifier 
		{
			get 
			{
				return this.PayloadSpecifier;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier > ISql110TSqlEndpoint.ProtocolSpecifier 
		{
			get 
			{
				return this.ProtocolSpecifier;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlErrorMessage"/>.
	/// </summary>
	public partial class TSqlErrorMessage : ISql110TSqlErrorMessage
	{		
		String ISql110TSqlErrorMessage.Language 
		{
			get { return this.Language;}
		}
		Int32 ISql110TSqlErrorMessage.MessageNumber 
		{
			get { return this.MessageNumber;}
		}
		String ISql110TSqlErrorMessage.MessageText 
		{
			get { return this.MessageText;}
		}
		Int32 ISql110TSqlErrorMessage.Severity 
		{
			get { return this.Severity;}
		}
		Boolean ISql110TSqlErrorMessage.WithLog 
		{
			get { return this.WithLog;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventGroup"/>.
	/// </summary>
	public partial class TSqlEventGroup : ISql110TSqlEventGroup
	{		
		EventGroupType ISql110TSqlEventGroup.Group 
		{
			get { return this.Group;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventSession"/>.
	/// </summary>
	public partial class TSqlEventSession : ISql110TSqlEventSession
	{		
		EventRetentionMode ISql110TSqlEventSession.EventRetentionMode 
		{
			get { return this.EventRetentionMode;}
		}
		Int32 ISql110TSqlEventSession.MaxDispatchLatency 
		{
			get { return this.MaxDispatchLatency;}
		}
		Int32 ISql110TSqlEventSession.MaxEventSize 
		{
			get { return this.MaxEventSize;}
		}
		MemoryUnit ISql110TSqlEventSession.MaxEventSizeUnit 
		{
			get { return this.MaxEventSizeUnit;}
		}
		Int32 ISql110TSqlEventSession.MaxMemory 
		{
			get { return this.MaxMemory;}
		}
		MemoryUnit ISql110TSqlEventSession.MaxMemoryUnit 
		{
			get { return this.MaxMemoryUnit;}
		}
		MemoryPartitionMode ISql110TSqlEventSession.MemoryPartitionMode 
		{
			get { return this.MemoryPartitionMode;}
		}
		Boolean ISql110TSqlEventSession.StartupState 
		{
			get { return this.StartupState;}
		}
		Boolean ISql110TSqlEventSession.TrackCausality 
		{
			get { return this.TrackCausality;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlEventSession.EventDefinitions 
		{
			get 
			{
				return this.EventDefinitions;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlEventSession.EventTargets 
		{
			get 
			{
				return this.EventTargets;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventSessionAction"/>.
	/// </summary>
	public partial class TSqlEventSessionAction : ISql110TSqlEventSessionAction
	{		
		String ISql110TSqlEventSessionAction.ActionName 
		{
			get { return this.ActionName;}
		}
		String ISql110TSqlEventSessionAction.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql110TSqlEventSessionAction.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventSessionDefinitions"/>.
	/// </summary>
	public partial class TSqlEventSessionDefinitions : ISql110TSqlEventSessionDefinitions
	{		
		String ISql110TSqlEventSessionDefinitions.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql110TSqlEventSessionDefinitions.EventName 
		{
			get { return this.EventName;}
		}
		String ISql110TSqlEventSessionDefinitions.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
		String ISql110TSqlEventSessionDefinitions.WhereExpression 
		{
			get { return this.WhereExpression;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventSessionAction> ISql110TSqlEventSessionDefinitions.Actions 
		{
			get 
			{
				return this.Actions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventSessionAction>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlEventSessionDefinitions.AttributeSettings 
		{
			get 
			{
				return this.AttributeSettings;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventSessionSetting"/>.
	/// </summary>
	public partial class TSqlEventSessionSetting : ISql110TSqlEventSessionSetting
	{		
		String ISql110TSqlEventSessionSetting.SettingName 
		{
			get { return this.SettingName;}
		}
		String ISql110TSqlEventSessionSetting.SettingValue 
		{
			get { return this.SettingValue;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventSessionTarget"/>.
	/// </summary>
	public partial class TSqlEventSessionTarget : ISql110TSqlEventSessionTarget
	{		
		String ISql110TSqlEventSessionTarget.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql110TSqlEventSessionTarget.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
		String ISql110TSqlEventSessionTarget.TargetName 
		{
			get { return this.TargetName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlEventSessionTarget.ParameterSettings 
		{
			get 
			{
				return this.ParameterSettings;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlEventTypeSpecifier"/>.
	/// </summary>
	public partial class TSqlEventTypeSpecifier : ISql110TSqlEventTypeSpecifier
	{		
		EventType ISql110TSqlEventTypeSpecifier.EventType 
		{
			get { return this.EventType;}
		}
		OrderRestriction ISql110TSqlEventTypeSpecifier.Order 
		{
			get { return this.Order;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlExtendedProcedure"/>.
	/// </summary>
	public partial class TSqlExtendedProcedure : ISql110TSqlExtendedProcedure
	{		
		Boolean ISql110TSqlExtendedProcedure.ExeccuteAsCaller 
		{
			get { return this.ExeccuteAsCaller;}
		}
		Boolean ISql110TSqlExtendedProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlExtendedProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql110TSqlExtendedProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlExtendedProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter> ISql110TSqlExtendedProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlExtendedProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlExtendedProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlExtendedProperty"/>.
	/// </summary>
	public partial class TSqlExtendedProperty : ISql110TSqlExtendedProperty
	{		
		String ISql110TSqlExtendedProperty.Value 
		{
			get { return this.Value;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost> ISql110TSqlExtendedProperty.Host 
		{
			get 
			{
				return this.Host;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSqlFile"/>.
	/// </summary>
	public partial class TSqlSqlFile : ISql110TSqlSqlFile
	{		
		Int32? ISql110TSqlSqlFile.FileGrowth 
		{
			get { return this.FileGrowth;}
		}
		MemoryUnit ISql110TSqlSqlFile.FileGrowthUnit 
		{
			get { return this.FileGrowthUnit;}
		}
		String ISql110TSqlSqlFile.FileName 
		{
			get { return this.FileName;}
		}
		Boolean ISql110TSqlSqlFile.IsLogFile 
		{
			get { return this.IsLogFile;}
		}
		Int32? ISql110TSqlSqlFile.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql110TSqlSqlFile.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		Boolean ISql110TSqlSqlFile.Offline 
		{
			get { return this.Offline;}
		}
		Int32? ISql110TSqlSqlFile.Size 
		{
			get { return this.Size;}
		}
		MemoryUnit ISql110TSqlSqlFile.SizeUnit 
		{
			get { return this.SizeUnit;}
		}
		Boolean ISql110TSqlSqlFile.Unlimited 
		{
			get { return this.Unlimited;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlSqlFile.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlFilegroup"/>.
	/// </summary>
	public partial class TSqlFilegroup : ISql110TSqlFilegroup
	{		
		Boolean ISql110TSqlFilegroup.ContainsFileStream 
		{
			get { return this.ContainsFileStream;}
		}
		Boolean ISql110TSqlFilegroup.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlForeignKeyConstraint"/>.
	/// </summary>
	public partial class TSqlForeignKeyConstraint : ISql110TSqlForeignKeyConstraint
	{		
		ForeignKeyAction ISql110TSqlForeignKeyConstraint.DeleteAction 
		{
			get { return this.DeleteAction;}
		}
		Boolean ISql110TSqlForeignKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql110TSqlForeignKeyConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		ForeignKeyAction ISql110TSqlForeignKeyConstraint.UpdateAction 
		{
			get { return this.UpdateAction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlForeignKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlForeignKeyConstraint.ForeignColumns 
		{
			get 
			{
				return this.ForeignColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference> ISql110TSqlForeignKeyConstraint.ForeignTable 
		{
			get 
			{
				return this.ForeignTable.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference> ISql110TSqlForeignKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlFullTextCatalog"/>.
	/// </summary>
	public partial class TSqlFullTextCatalog : ISql110TSqlFullTextCatalog
	{		
		Boolean? ISql110TSqlFullTextCatalog.AccentSensitivity 
		{
			get { return this.AccentSensitivity;}
		}
		Boolean ISql110TSqlFullTextCatalog.IsDefault 
		{
			get { return this.IsDefault;}
		}
		String ISql110TSqlFullTextCatalog.Path 
		{
			get { return this.Path;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlFullTextCatalog.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlFullTextCatalog.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlFullTextIndex"/>.
	/// </summary>
	public partial class TSqlFullTextIndex : ISql110TSqlFullTextIndex
	{		
		ChangeTrackingOption ISql110TSqlFullTextIndex.ChangeTracking 
		{
			get { return this.ChangeTracking;}
		}
		Boolean ISql110TSqlFullTextIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql110TSqlFullTextIndex.Replicated 
		{
			get { return this.Replicated;}
		}
		Boolean ISql110TSqlFullTextIndex.StopListOff 
		{
			get { return this.StopListOff;}
		}
		Boolean ISql110TSqlFullTextIndex.UseSystemStopList 
		{
			get { return this.UseSystemStopList;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference> ISql110TSqlFullTextIndex.Catalog 
		{
			get 
			{
				return this.Catalog;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFullTextIndexColumnSpecifier> ISql110TSqlFullTextIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFullTextIndexColumnSpecifier>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlFullTextIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql110TSqlFullTextIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSearchPropertyListReference> ISql110TSqlFullTextIndex.SearchPropertyList 
		{
			get 
			{
				return this.SearchPropertyList.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSearchPropertyListReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlFullTextIndex.StopList 
		{
			get 
			{
				return this.StopList;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlFullTextIndex.UniqueIndexName 
		{
			get 
			{
				return this.UniqueIndexName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlFullTextIndexColumnSpecifier"/>.
	/// </summary>
	public partial class TSqlFullTextIndexColumnSpecifier : ISql110TSqlFullTextIndexColumnSpecifier
	{		
		Int32? ISql110TSqlFullTextIndexColumnSpecifier.LanguageId 
		{
			get { return this.LanguageId;}
		}
		Boolean ISql110TSqlFullTextIndexColumnSpecifier.PartOfStatisticalSemantics 
		{
			get { return this.PartOfStatisticalSemantics;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlFullTextIndexColumnSpecifier.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlFullTextIndexColumnSpecifier.TypeColumn 
		{
			get 
			{
				return this.TypeColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlFullTextStopList"/>.
	/// </summary>
	public partial class TSqlFullTextStopList : ISql110TSqlFullTextStopList
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlFullTextStopList.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlLinkedServer"/>.
	/// </summary>
	public partial class TSqlLinkedServer : ISql110TSqlLinkedServer
	{		
		String ISql110TSqlLinkedServer.Catalog 
		{
			get { return this.Catalog;}
		}
		Boolean ISql110TSqlLinkedServer.CollationCompatible 
		{
			get { return this.CollationCompatible;}
		}
		String ISql110TSqlLinkedServer.CollationName 
		{
			get { return this.CollationName;}
		}
		Int32 ISql110TSqlLinkedServer.ConnectTimeout 
		{
			get { return this.ConnectTimeout;}
		}
		Boolean ISql110TSqlLinkedServer.DataAccess 
		{
			get { return this.DataAccess;}
		}
		String ISql110TSqlLinkedServer.DataSource 
		{
			get { return this.DataSource;}
		}
		Boolean ISql110TSqlLinkedServer.IsDistributor 
		{
			get { return this.IsDistributor;}
		}
		Boolean ISql110TSqlLinkedServer.IsPublisher 
		{
			get { return this.IsPublisher;}
		}
		Boolean ISql110TSqlLinkedServer.IsSubscriber 
		{
			get { return this.IsSubscriber;}
		}
		Boolean ISql110TSqlLinkedServer.LazySchemaValidationEnabled 
		{
			get { return this.LazySchemaValidationEnabled;}
		}
		String ISql110TSqlLinkedServer.Location 
		{
			get { return this.Location;}
		}
		String ISql110TSqlLinkedServer.ProductName 
		{
			get { return this.ProductName;}
		}
		String ISql110TSqlLinkedServer.ProviderName 
		{
			get { return this.ProviderName;}
		}
		String ISql110TSqlLinkedServer.ProviderString 
		{
			get { return this.ProviderString;}
		}
		Int32 ISql110TSqlLinkedServer.QueryTimeout 
		{
			get { return this.QueryTimeout;}
		}
		Boolean ISql110TSqlLinkedServer.RemoteProcTransactionPromotionEnabled 
		{
			get { return this.RemoteProcTransactionPromotionEnabled;}
		}
		Boolean ISql110TSqlLinkedServer.RpcEnabled 
		{
			get { return this.RpcEnabled;}
		}
		Boolean ISql110TSqlLinkedServer.RpcOutEnabled 
		{
			get { return this.RpcOutEnabled;}
		}
		Boolean ISql110TSqlLinkedServer.UseRemoteCollation 
		{
			get { return this.UseRemoteCollation;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlLinkedServerLogin"/>.
	/// </summary>
	public partial class TSqlLinkedServerLogin : ISql110TSqlLinkedServerLogin
	{		
		String ISql110TSqlLinkedServerLogin.LinkedServerLoginName 
		{
			get { return this.LinkedServerLoginName;}
		}
		String ISql110TSqlLinkedServerLogin.LinkedServerPassword 
		{
			get { return this.LinkedServerPassword;}
		}
		Boolean ISql110TSqlLinkedServerLogin.UseSelf 
		{
			get { return this.UseSelf;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLinkedServerReference> ISql110TSqlLinkedServerLogin.LinkedServer 
		{
			get 
			{
				return this.LinkedServer.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLinkedServerReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlLinkedServerLogin.LocalLogin 
		{
			get 
			{
				return this.LocalLogin.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlLogin"/>.
	/// </summary>
	public partial class TSqlLogin : ISql110TSqlLogin
	{		
		Boolean ISql110TSqlLogin.CheckExpiration 
		{
			get { return this.CheckExpiration;}
		}
		Boolean ISql110TSqlLogin.CheckPolicy 
		{
			get { return this.CheckPolicy;}
		}
		String ISql110TSqlLogin.DefaultDatabase 
		{
			get { return this.DefaultDatabase;}
		}
		String ISql110TSqlLogin.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		Boolean ISql110TSqlLogin.Disabled 
		{
			get { return this.Disabled;}
		}
		LoginEncryptionOption ISql110TSqlLogin.EncryptionOption 
		{
			get { return this.EncryptionOption;}
		}
		Boolean ISql110TSqlLogin.MappedToWindowsLogin 
		{
			get { return this.MappedToWindowsLogin;}
		}
		String ISql110TSqlLogin.Password 
		{
			get { return this.Password;}
		}
		Boolean ISql110TSqlLogin.PasswordHashed 
		{
			get { return this.PasswordHashed;}
		}
		Boolean ISql110TSqlLogin.PasswordMustChange 
		{
			get { return this.PasswordMustChange;}
		}
		String ISql110TSqlLogin.Sid 
		{
			get { return this.Sid;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference> ISql110TSqlLogin.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlLogin.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCredentialReference> ISql110TSqlLogin.Credential 
		{
			get 
			{
				return this.Credential.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCredentialReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlMasterKey"/>.
	/// </summary>
	public partial class TSqlMasterKey : ISql110TSqlMasterKey
	{		
		String ISql110TSqlMasterKey.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlMessageType"/>.
	/// </summary>
	public partial class TSqlMessageType : ISql110TSqlMessageType
	{		
		ValidationMethod ISql110TSqlMessageType.ValidationMethod 
		{
			get { return this.ValidationMethod;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlMessageType.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference> ISql110TSqlMessageType.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPartitionFunction"/>.
	/// </summary>
	public partial class TSqlPartitionFunction : ISql110TSqlPartitionFunction
	{		
		PartitionRange ISql110TSqlPartitionFunction.Range 
		{
			get { return this.Range;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionValue> ISql110TSqlPartitionFunction.BoundaryValues 
		{
			get 
			{
				return this.BoundaryValues.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionValue>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlPartitionFunction.ParameterType 
		{
			get 
			{
				return this.ParameterType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPartitionScheme"/>.
	/// </summary>
	public partial class TSqlPartitionScheme : ISql110TSqlPartitionScheme
	{		
		Boolean ISql110TSqlPartitionScheme.AllToOneFilegroup 
		{
			get { return this.AllToOneFilegroup;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlPartitionScheme.Filegroups 
		{
			get 
			{
				return this.Filegroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionFunctionReference> ISql110TSqlPartitionScheme.PartitionFunction 
		{
			get 
			{
				return this.PartitionFunction.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionFunctionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPartitionValue"/>.
	/// </summary>
	public partial class TSqlPartitionValue : ISql110TSqlPartitionValue
	{		
		String ISql110TSqlPartitionValue.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlPartitionValue.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPermission"/>.
	/// </summary>
	public partial class TSqlPermission : ISql110TSqlPermission
	{		
		PermissionAction ISql110TSqlPermission.PermissionAction 
		{
			get { return this.PermissionAction;}
		}
		PermissionType ISql110TSqlPermission.PermissionType 
		{
			get { return this.PermissionType;}
		}
		Boolean ISql110TSqlPermission.WithAllPrivileges 
		{
			get { return this.WithAllPrivileges;}
		}
		Boolean ISql110TSqlPermission.WithGrantOption 
		{
			get { return this.WithGrantOption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlPermission.ExcludedColumns 
		{
			get 
			{
				return this.ExcludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql110TSqlPermission.Grantee 
		{
			get 
			{
				return this.Grantee;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql110TSqlPermission.Grantor 
		{
			get 
			{
				return this.Grantor;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlPermission.RevokedGrantOptionColumns 
		{
			get 
			{
				return this.RevokedGrantOptionColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql110TSqlPermission.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlPrimaryKeyConstraint : ISql110TSqlPrimaryKeyConstraint
	{		
		Boolean ISql110TSqlPrimaryKeyConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql110TSqlPrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql110TSqlPrimaryKeyConstraint.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql110TSqlPrimaryKeyConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql110TSqlPrimaryKeyConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlPrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption> ISql110TSqlPrimaryKeyConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlPrimaryKeyConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlPrimaryKeyConstraint.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlPrimaryKeyConstraint.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference> ISql110TSqlPrimaryKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlPrimaryKeyConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlPrimaryKeyConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlProcedure"/>.
	/// </summary>
	public partial class TSqlProcedure : ISql110TSqlProcedure
	{		
		Boolean? ISql110TSqlProcedure.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql110TSqlProcedure.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql110TSqlProcedure.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql110TSqlProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql110TSqlProcedure.ForReplication 
		{
			get { return this.ForReplication;}
		}
		Boolean ISql110TSqlProcedure.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql110TSqlProcedure.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql110TSqlProcedure.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql110TSqlProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql110TSqlProcedure.WithRecompile 
		{
			get { return this.WithRecompile;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlProcedure.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlProcedure.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter> ISql110TSqlProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlProcedureReference> ISql110TSqlProcedure.ParentProcedure 
		{
			get 
			{
				return this.ParentProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlProcedureReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlQueue"/>.
	/// </summary>
	public partial class TSqlQueue : ISql110TSqlQueue
	{		
		Boolean ISql110TSqlQueue.ActivationExecuteAsCaller 
		{
			get { return this.ActivationExecuteAsCaller;}
		}
		Boolean ISql110TSqlQueue.ActivationExecuteAsOwner 
		{
			get { return this.ActivationExecuteAsOwner;}
		}
		Boolean ISql110TSqlQueue.ActivationExecuteAsSelf 
		{
			get { return this.ActivationExecuteAsSelf;}
		}
		Int32? ISql110TSqlQueue.ActivationMaxQueueReaders 
		{
			get { return this.ActivationMaxQueueReaders;}
		}
		Boolean? ISql110TSqlQueue.ActivationStatusOn 
		{
			get { return this.ActivationStatusOn;}
		}
		Boolean ISql110TSqlQueue.PoisonMessageHandlingStatusOn 
		{
			get { return this.PoisonMessageHandlingStatusOn;}
		}
		Boolean ISql110TSqlQueue.RetentionOn 
		{
			get { return this.RetentionOn;}
		}
		Boolean ISql110TSqlQueue.StatusOn 
		{
			get { return this.StatusOn;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlProcedureReference> ISql110TSqlQueue.ActivationProcedure 
		{
			get 
			{
				return this.ActivationProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlProcedureReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn> ISql110TSqlQueue.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlQueue.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlQueue.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlQueue.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlQueue.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlQueue.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlQueue.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlQueueEventNotification"/>.
	/// </summary>
	public partial class TSqlQueueEventNotification : ISql110TSqlQueueEventNotification
	{		
		String ISql110TSqlQueueEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql110TSqlQueueEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql110TSqlQueueEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup> ISql110TSqlQueueEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql110TSqlQueueEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlQueueReference> ISql110TSqlQueueEventNotification.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlRemoteServiceBinding"/>.
	/// </summary>
	public partial class TSqlRemoteServiceBinding : ISql110TSqlRemoteServiceBinding
	{		
		Boolean ISql110TSqlRemoteServiceBinding.Anonymous 
		{
			get { return this.Anonymous;}
		}
		String ISql110TSqlRemoteServiceBinding.Service 
		{
			get { return this.Service;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlRemoteServiceBinding.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlRemoteServiceBinding.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlResourceGovernor"/>.
	/// </summary>
	public partial class TSqlResourceGovernor : ISql110TSqlResourceGovernor
	{		
		Boolean? ISql110TSqlResourceGovernor.Enabled 
		{
			get { return this.Enabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlResourceGovernor.ClassifierFunction 
		{
			get 
			{
				return this.ClassifierFunction;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlResourcePool"/>.
	/// </summary>
	public partial class TSqlResourcePool : ISql110TSqlResourcePool
	{		
		Int32 ISql110TSqlResourcePool.CapCpuPercent 
		{
			get { return this.CapCpuPercent;}
		}
		Int32 ISql110TSqlResourcePool.MaxCpuPercent 
		{
			get { return this.MaxCpuPercent;}
		}
		Int32 ISql110TSqlResourcePool.MaxMemoryPercent 
		{
			get { return this.MaxMemoryPercent;}
		}
		Int32 ISql110TSqlResourcePool.MinCpuPercent 
		{
			get { return this.MinCpuPercent;}
		}
		Int32 ISql110TSqlResourcePool.MinMemoryPercent 
		{
			get { return this.MinMemoryPercent;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlRole"/>.
	/// </summary>
	public partial class TSqlRole : ISql110TSqlRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlRoleMembership"/>.
	/// </summary>
	public partial class TSqlRoleMembership : ISql110TSqlRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql110TSqlRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlRoleReference> ISql110TSqlRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlRoute"/>.
	/// </summary>
	public partial class TSqlRoute : ISql110TSqlRoute
	{		
		String ISql110TSqlRoute.Address 
		{
			get { return this.Address;}
		}
		String ISql110TSqlRoute.BrokerInstance 
		{
			get { return this.BrokerInstance;}
		}
		Int32? ISql110TSqlRoute.Lifetime 
		{
			get { return this.Lifetime;}
		}
		String ISql110TSqlRoute.MirrorAddress 
		{
			get { return this.MirrorAddress;}
		}
		String ISql110TSqlRoute.ServiceName 
		{
			get { return this.ServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlRoute.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlRule"/>.
	/// </summary>
	public partial class TSqlRule : ISql110TSqlRule
	{		
		String ISql110TSqlRule.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlRule.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlRule.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSchema"/>.
	/// </summary>
	public partial class TSqlSchema : ISql110TSqlSchema
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlSchema.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSearchProperty"/>.
	/// </summary>
	public partial class TSqlSearchProperty : ISql110TSqlSearchProperty
	{		
		String ISql110TSqlSearchProperty.Description 
		{
			get { return this.Description;}
		}
		Int32 ISql110TSqlSearchProperty.Identifier 
		{
			get { return this.Identifier;}
		}
		String ISql110TSqlSearchProperty.PropertySetGuid 
		{
			get { return this.PropertySetGuid;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSearchPropertyListReference> ISql110TSqlSearchProperty.SearchPropertyList 
		{
			get 
			{
				return this.SearchPropertyList.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSearchPropertyListReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSearchPropertyList"/>.
	/// </summary>
	public partial class TSqlSearchPropertyList : ISql110TSqlSearchPropertyList
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlSearchPropertyList.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSequence"/>.
	/// </summary>
	public partial class TSqlSequence : ISql110TSqlSequence
	{		
		Int32? ISql110TSqlSequence.CacheSize 
		{
			get { return this.CacheSize;}
		}
		String ISql110TSqlSequence.IncrementValue 
		{
			get { return this.IncrementValue;}
		}
		Boolean ISql110TSqlSequence.IsCached 
		{
			get { return this.IsCached;}
		}
		Boolean ISql110TSqlSequence.IsCycling 
		{
			get { return this.IsCycling;}
		}
		String ISql110TSqlSequence.MaxValue 
		{
			get { return this.MaxValue;}
		}
		String ISql110TSqlSequence.MinValue 
		{
			get { return this.MinValue;}
		}
		Boolean ISql110TSqlSequence.NoMaxValue 
		{
			get { return this.NoMaxValue;}
		}
		Boolean ISql110TSqlSequence.NoMinValue 
		{
			get { return this.NoMinValue;}
		}
		String ISql110TSqlSequence.StartValue 
		{
			get { return this.StartValue;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlSequence.DataType 
		{
			get 
			{
				return this.DataType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlSequence.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServerAudit"/>.
	/// </summary>
	public partial class TSqlServerAudit : ISql110TSqlServerAudit
	{		
		String ISql110TSqlServerAudit.AuditGuid 
		{
			get { return this.AuditGuid;}
		}
		AuditTarget ISql110TSqlServerAudit.AuditTarget 
		{
			get { return this.AuditTarget;}
		}
		Boolean ISql110TSqlServerAudit.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql110TSqlServerAudit.FilePath 
		{
			get { return this.FilePath;}
		}
		Int32? ISql110TSqlServerAudit.MaxFiles 
		{
			get { return this.MaxFiles;}
		}
		Int32? ISql110TSqlServerAudit.MaxRolloverFiles 
		{
			get { return this.MaxRolloverFiles;}
		}
		Int32? ISql110TSqlServerAudit.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql110TSqlServerAudit.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		FailureAction ISql110TSqlServerAudit.OnFailure 
		{
			get { return this.OnFailure;}
		}
		String ISql110TSqlServerAudit.PredicateExpression 
		{
			get { return this.PredicateExpression;}
		}
		Int32 ISql110TSqlServerAudit.QueueDelay 
		{
			get { return this.QueueDelay;}
		}
		Boolean ISql110TSqlServerAudit.ReserveDiskSpace 
		{
			get { return this.ReserveDiskSpace;}
		}
		Boolean ISql110TSqlServerAudit.UnlimitedFileSize 
		{
			get { return this.UnlimitedFileSize;}
		}
		Boolean ISql110TSqlServerAudit.UnlimitedMaxRolloverFiles 
		{
			get { return this.UnlimitedMaxRolloverFiles;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServerAuditSpecification"/>.
	/// </summary>
	public partial class TSqlServerAuditSpecification : ISql110TSqlServerAuditSpecification
	{		
		Boolean ISql110TSqlServerAuditSpecification.StateOn 
		{
			get { return this.StateOn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditActionGroup> ISql110TSqlServerAuditSpecification.AuditActionGroups 
		{
			get 
			{
				return this.AuditActionGroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAuditActionGroup>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlServerAuditReference> ISql110TSqlServerAuditSpecification.ServerAudit 
		{
			get 
			{
				return this.ServerAudit.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlServerAuditReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServerDdlTrigger"/>.
	/// </summary>
	public partial class TSqlServerDdlTrigger : ISql110TSqlServerDdlTrigger
	{		
		Boolean? ISql110TSqlServerDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql110TSqlServerDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql110TSqlServerDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql110TSqlServerDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql110TSqlServerDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql110TSqlServerDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql110TSqlServerDdlTrigger.IsLogon 
		{
			get { return this.IsLogon;}
		}
		String ISql110TSqlServerDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql110TSqlServerDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql110TSqlServerDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql110TSqlServerDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlServerDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlServerDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup> ISql110TSqlServerDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql110TSqlServerDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlServerDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference> ISql110TSqlServerDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServerEventNotification"/>.
	/// </summary>
	public partial class TSqlServerEventNotification : ISql110TSqlServerEventNotification
	{		
		String ISql110TSqlServerEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql110TSqlServerEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql110TSqlServerEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup> ISql110TSqlServerEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql110TSqlServerEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServerOptions"/>.
	/// </summary>
	public partial class TSqlServerOptions : ISql110TSqlServerOptions
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServerRoleMembership"/>.
	/// </summary>
	public partial class TSqlServerRoleMembership : ISql110TSqlServerRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> ISql110TSqlServerRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlRoleReference> ISql110TSqlServerRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlService"/>.
	/// </summary>
	public partial class TSqlService : ISql110TSqlService
	{		
		Boolean ISql110TSqlService.UseDefaultContract 
		{
			get { return this.UseDefaultContract;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlService.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlContractReference> ISql110TSqlService.Contracts 
		{
			get 
			{
				return this.Contracts.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlContractReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlQueueReference> ISql110TSqlService.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlServiceBrokerLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlServiceBrokerLanguageSpecifier : ISql110TSqlServiceBrokerLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql110TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql110TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql110TSqlServiceBrokerLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		Boolean ISql110TSqlServiceBrokerLanguageSpecifier.MessageForwardingEnabled 
		{
			get { return this.MessageForwardingEnabled;}
		}
		Int32 ISql110TSqlServiceBrokerLanguageSpecifier.MessageForwardSize 
		{
			get { return this.MessageForwardSize;}
		}
		Boolean ISql110TSqlServiceBrokerLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql110TSqlServiceBrokerLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlServiceBrokerLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSignature"/>.
	/// </summary>
	public partial class TSqlSignature : ISql110TSqlSignature
	{		
		Boolean ISql110TSqlSignature.IsCounterSignature 
		{
			get { return this.IsCounterSignature;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlSignature.EncryptionMechanism 
		{
			get 
			{
				return this.EncryptionMechanism;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlSignature.SignedObject 
		{
			get 
			{
				return this.SignedObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSignatureEncryptionMechanism"/>.
	/// </summary>
	public partial class TSqlSignatureEncryptionMechanism : ISql110TSqlSignatureEncryptionMechanism
	{		
		String ISql110TSqlSignatureEncryptionMechanism.Password 
		{
			get { return this.Password;}
		}
		String ISql110TSqlSignatureEncryptionMechanism.SignedBlob 
		{
			get { return this.SignedBlob;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference> ISql110TSqlSignatureEncryptionMechanism.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlSignatureEncryptionMechanism.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSpatialIndex"/>.
	/// </summary>
	public partial class TSqlSpatialIndex : ISql110TSqlSpatialIndex
	{		
		Boolean ISql110TSqlSpatialIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql110TSqlSpatialIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql110TSqlSpatialIndex.CellsPerObject 
		{
			get { return this.CellsPerObject;}
		}
		CompressionLevel ISql110TSqlSpatialIndex.DataCompression 
		{
			get { return this.DataCompression;}
		}
		Boolean ISql110TSqlSpatialIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32 ISql110TSqlSpatialIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Degree ISql110TSqlSpatialIndex.GridLevel1Density 
		{
			get { return this.GridLevel1Density;}
		}
		Degree ISql110TSqlSpatialIndex.GridLevel2Density 
		{
			get { return this.GridLevel2Density;}
		}
		Degree ISql110TSqlSpatialIndex.GridLevel3Density 
		{
			get { return this.GridLevel3Density;}
		}
		Degree ISql110TSqlSpatialIndex.GridLevel4Density 
		{
			get { return this.GridLevel4Density;}
		}
		Boolean ISql110TSqlSpatialIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql110TSqlSpatialIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Tessellation ISql110TSqlSpatialIndex.Tessellation 
		{
			get { return this.Tessellation;}
		}
		Boolean ISql110TSqlSpatialIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}
		Double? ISql110TSqlSpatialIndex.XMax 
		{
			get { return this.XMax;}
		}
		Double? ISql110TSqlSpatialIndex.XMin 
		{
			get { return this.XMin;}
		}
		Double? ISql110TSqlSpatialIndex.YMax 
		{
			get { return this.YMax;}
		}
		Double? ISql110TSqlSpatialIndex.YMin 
		{
			get { return this.YMin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlSpatialIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlSpatialIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql110TSqlSpatialIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlSpatialIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlSpatialIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlStatistics"/>.
	/// </summary>
	public partial class TSqlStatistics : ISql110TSqlStatistics
	{		
		String ISql110TSqlStatistics.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISql110TSqlStatistics.NoRecompute 
		{
			get { return this.NoRecompute;}
		}
		Int32 ISql110TSqlStatistics.SampleSize 
		{
			get { return this.SampleSize;}
		}
		SamplingStyle ISql110TSqlStatistics.SamplingStyle 
		{
			get { return this.SamplingStyle;}
		}
		String ISql110TSqlStatistics.StatsStream 
		{
			get { return this.StatsStream;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlStatistics.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlStatistics.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlStatistics.OnObject 
		{
			get 
			{
				return this.OnObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlParameter"/>.
	/// </summary>
	public partial class TSqlParameter : ISql110TSqlParameter
	{		
		String ISql110TSqlParameter.DefaultExpression 
		{
			get { return this.DefaultExpression;}
		}
		Boolean ISql110TSqlParameter.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql110TSqlParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		Int32 ISql110TSqlParameter.Length 
		{
			get { return this.Length;}
		}
		Int32 ISql110TSqlParameter.Precision 
		{
			get { return this.Precision;}
		}
		Boolean ISql110TSqlParameter.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		Int32 ISql110TSqlParameter.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql110TSqlParameter.Varying 
		{
			get { return this.Varying;}
		}
		XmlStyle ISql110TSqlParameter.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql110TSqlParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference> ISql110TSqlParameter.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSymmetricKey"/>.
	/// </summary>
	public partial class TSqlSymmetricKey : ISql110TSqlSymmetricKey
	{		
		SymmetricKeyAlgorithm ISql110TSqlSymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		SymmetricKeyCreationDisposition ISql110TSqlSymmetricKey.CreationDisposition 
		{
			get { return this.CreationDisposition;}
		}
		String ISql110TSqlSymmetricKey.IdentityValue 
		{
			get { return this.IdentityValue;}
		}
		String ISql110TSqlSymmetricKey.KeySource 
		{
			get { return this.KeySource;}
		}
		String ISql110TSqlSymmetricKey.ProviderKeyName 
		{
			get { return this.ProviderKeyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference> ISql110TSqlSymmetricKey.AsymmetricKeys 
		{
			get 
			{
				return this.AsymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlSymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlSymmetricKey.Certificates 
		{
			get 
			{
				return this.Certificates.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlSymmetricKey.Passwords 
		{
			get 
			{
				return this.Passwords;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlSymmetricKey.Provider 
		{
			get 
			{
				return this.Provider;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSymmetricKeyReference> ISql110TSqlSymmetricKey.SymmetricKeys 
		{
			get 
			{
				return this.SymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSymmetricKeyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSymmetricKeyPassword"/>.
	/// </summary>
	public partial class TSqlSymmetricKeyPassword : ISql110TSqlSymmetricKeyPassword
	{		
		String ISql110TSqlSymmetricKeyPassword.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSynonym"/>.
	/// </summary>
	public partial class TSqlSynonym : ISql110TSqlSynonym
	{		
		String ISql110TSqlSynonym.ForObjectName 
		{
			get { return this.ForObjectName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlSynonym.ForObject 
		{
			get 
			{
				return this.ForObject;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlSynonym.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTable"/>.
	/// </summary>
	public partial class TSqlTable : ISql110TSqlTable
	{		
		Boolean? ISql110TSqlTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql110TSqlTable.ChangeDataCaptureEnabled 
		{
			get { return this.ChangeDataCaptureEnabled;}
		}
		Boolean ISql110TSqlTable.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int64? ISql110TSqlTable.DataPages 
		{
			get { return this.DataPages;}
		}
		Double? ISql110TSqlTable.DataSize 
		{
			get { return this.DataSize;}
		}
		Boolean? ISql110TSqlTable.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Double? ISql110TSqlTable.IndexSize 
		{
			get { return this.IndexSize;}
		}
		Boolean ISql110TSqlTable.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		Boolean ISql110TSqlTable.LargeValueTypesOutOfRow 
		{
			get { return this.LargeValueTypesOutOfRow;}
		}
		LockEscalationMethod ISql110TSqlTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean? ISql110TSqlTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Int64? ISql110TSqlTable.RowCount 
		{
			get { return this.RowCount;}
		}
		Boolean ISql110TSqlTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}
		Int32 ISql110TSqlTable.TextInRowSize 
		{
			get { return this.TextInRowSize;}
		}
		Boolean ISql110TSqlTable.TrackColumnsUpdated 
		{
			get { return this.TrackColumnsUpdated;}
		}
		Int64? ISql110TSqlTable.UsedPages 
		{
			get { return this.UsedPages;}
		}
		Boolean ISql110TSqlTable.VardecimalStorageFormatEnabled 
		{
			get { return this.VardecimalStorageFormatEnabled;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn> ISql110TSqlTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption> ISql110TSqlTable.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlTable.FilegroupForTextImage 
		{
			get 
			{
				return this.FilegroupForTextImage.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlTable.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlTable.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlFileTable"/>.
	/// </summary>
	public partial class TSqlFileTable : ISql110TSqlFileTable
	{		
		Boolean? ISql110TSqlFileTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISql110TSqlFileTable.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		String ISql110TSqlFileTable.FileTableCollateFilename 
		{
			get { return this.FileTableCollateFilename;}
		}
		String ISql110TSqlFileTable.FileTableDirectory 
		{
			get { return this.FileTableDirectory;}
		}
		Boolean ISql110TSqlFileTable.FileTableNamespaceEnabled 
		{
			get { return this.FileTableNamespaceEnabled;}
		}
		LockEscalationMethod ISql110TSqlFileTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean? ISql110TSqlFileTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql110TSqlFileTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn> ISql110TSqlFileTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption> ISql110TSqlFileTable.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlFileTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlFileTable.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlFileTable.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlFileTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlFileTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlFileTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableType"/>.
	/// </summary>
	public partial class TSqlTableType : ISql110TSqlTableType
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableTypeColumn> ISql110TSqlTableType.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableTypeColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint> ISql110TSqlTableType.Constraints 
		{
			get 
			{
				return this.Constraints;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlTableType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableTypeCheckConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeCheckConstraint : ISql110TSqlTableTypeCheckConstraint
	{		
		String ISql110TSqlTableTypeCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlTableTypeCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableTypeColumn"/>.
	/// </summary>
	public partial class TSqlTableTypeColumn : ISql110TSqlTableTypeColumn
	{		
		String ISql110TSqlTableTypeColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql110TSqlTableTypeColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql110TSqlTableTypeColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql110TSqlTableTypeColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql110TSqlTableTypeColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql110TSqlTableTypeColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql110TSqlTableTypeColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql110TSqlTableTypeColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql110TSqlTableTypeColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql110TSqlTableTypeColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql110TSqlTableTypeColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql110TSqlTableTypeColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql110TSqlTableTypeColumn.Scale 
		{
			get { return this.Scale;}
		}
		XmlStyle ISql110TSqlTableTypeColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql110TSqlTableTypeColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlTableTypeColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference> ISql110TSqlTableTypeColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableTypeDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeDefaultConstraint : ISql110TSqlTableTypeDefaultConstraint
	{		
		String ISql110TSqlTableTypeDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlTableTypeDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlTableTypeDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableTypePrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypePrimaryKeyConstraint : ISql110TSqlTableTypePrimaryKeyConstraint
	{		
		Int32? ISql110TSqlTableTypePrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql110TSqlTableTypePrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql110TSqlTableTypePrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql110TSqlTableTypePrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlTableTypePrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTableTypeUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeUniqueConstraint : ISql110TSqlTableTypeUniqueConstraint
	{		
		Boolean ISql110TSqlTableTypeUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql110TSqlTableTypeUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlTableTypeUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlTcpProtocolSpecifier"/>.
	/// </summary>
	public partial class TSqlTcpProtocolSpecifier : ISql110TSqlTcpProtocolSpecifier
	{		
		String ISql110TSqlTcpProtocolSpecifier.ListenerIPv4 
		{
			get { return this.ListenerIPv4;}
		}
		String ISql110TSqlTcpProtocolSpecifier.ListenerIPv6 
		{
			get { return this.ListenerIPv6;}
		}
		Int32 ISql110TSqlTcpProtocolSpecifier.ListenerPort 
		{
			get { return this.ListenerPort;}
		}
		Boolean ISql110TSqlTcpProtocolSpecifier.ListeningOnAllIPs 
		{
			get { return this.ListeningOnAllIPs;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlUniqueConstraint : ISql110TSqlUniqueConstraint
	{		
		Boolean ISql110TSqlUniqueConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql110TSqlUniqueConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql110TSqlUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql110TSqlUniqueConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql110TSqlUniqueConstraint.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql110TSqlUniqueConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql110TSqlUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql110TSqlUniqueConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql110TSqlUniqueConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption> ISql110TSqlUniqueConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlUniqueConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference> ISql110TSqlUniqueConstraint.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlUniqueConstraint.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference> ISql110TSqlUniqueConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlUniqueConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference> ISql110TSqlUniqueConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlUser"/>.
	/// </summary>
	public partial class TSqlUser : ISql110TSqlUser
	{		
		AuthenticationType ISql110TSqlUser.AuthenticationType 
		{
			get { return this.AuthenticationType;}
		}
		String ISql110TSqlUser.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		String ISql110TSqlUser.Password 
		{
			get { return this.Password;}
		}
		String ISql110TSqlUser.Sid 
		{
			get { return this.Sid;}
		}
		Boolean ISql110TSqlUser.WithoutLogin 
		{
			get { return this.WithoutLogin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference> ISql110TSqlUser.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference> ISql110TSqlUser.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlUser.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference> ISql110TSqlUser.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlUserDefinedServerRole"/>.
	/// </summary>
	public partial class TSqlUserDefinedServerRole : ISql110TSqlUserDefinedServerRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql110TSqlUserDefinedServerRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlUserDefinedType"/>.
	/// </summary>
	public partial class TSqlUserDefinedType : ISql110TSqlUserDefinedType
	{		
		Boolean? ISql110TSqlUserDefinedType.ByteOrdered 
		{
			get { return this.ByteOrdered;}
		}
		String ISql110TSqlUserDefinedType.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean? ISql110TSqlUserDefinedType.FixedLength 
		{
			get { return this.FixedLength;}
		}
		Format ISql110TSqlUserDefinedType.Format 
		{
			get { return this.Format;}
		}
		Int32? ISql110TSqlUserDefinedType.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		String ISql110TSqlUserDefinedType.ValidationMethodName 
		{
			get { return this.ValidationMethodName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference> ISql110TSqlUserDefinedType.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlUserDefinedType.Methods 
		{
			get 
			{
				return this.Methods;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql110TSqlUserDefinedType.Properties 
		{
			get 
			{
				return this.Properties;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlUserDefinedType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlView"/>.
	/// </summary>
	public partial class TSqlView : ISql110TSqlView
	{		
		Boolean? ISql110TSqlView.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISql110TSqlView.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql110TSqlView.Replicated 
		{
			get { return this.Replicated;}
		}
		String ISql110TSqlView.SelectStatement 
		{
			get { return this.SelectStatement;}
		}
		Boolean ISql110TSqlView.WithCheckOption 
		{
			get { return this.WithCheckOption;}
		}
		Boolean ISql110TSqlView.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql110TSqlView.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}
		Boolean ISql110TSqlView.WithViewMetadata 
		{
			get { return this.WithViewMetadata;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql110TSqlView.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn> ISql110TSqlView.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlView.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlWorkloadGroup"/>.
	/// </summary>
	public partial class TSqlWorkloadGroup : ISql110TSqlWorkloadGroup
	{		
		Int32 ISql110TSqlWorkloadGroup.GroupMaxRequests 
		{
			get { return this.GroupMaxRequests;}
		}
		Degree ISql110TSqlWorkloadGroup.Importance 
		{
			get { return this.Importance;}
		}
		Int32 ISql110TSqlWorkloadGroup.MaxDop 
		{
			get { return this.MaxDop;}
		}
		Int32 ISql110TSqlWorkloadGroup.RequestMaxCpuTimeSec 
		{
			get { return this.RequestMaxCpuTimeSec;}
		}
		Int32 ISql110TSqlWorkloadGroup.RequestMaxMemoryGrantPercent 
		{
			get { return this.RequestMaxMemoryGrantPercent;}
		}
		Int32 ISql110TSqlWorkloadGroup.RequestMemoryGrantTimeoutSec 
		{
			get { return this.RequestMemoryGrantTimeoutSec;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlResourcePoolReference> ISql110TSqlWorkloadGroup.ResourcePool 
		{
			get 
			{
				return this.ResourcePool.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlResourcePoolReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlXmlIndex"/>.
	/// </summary>
	public partial class TSqlXmlIndex : ISql110TSqlXmlIndex
	{		
		Boolean ISql110TSqlXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql110TSqlXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql110TSqlXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql110TSqlXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql110TSqlXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql110TSqlXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISql110TSqlXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		SecondaryXmlIndexType ISql110TSqlXmlIndex.SecondaryXmlIndexType 
		{
			get { return this.SecondaryXmlIndexType;}
		}
		Boolean ISql110TSqlXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql110TSqlXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlIndexReference> ISql110TSqlXmlIndex.PrimaryXmlIndex 
		{
			get 
			{
				return this.PrimaryXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlIndexReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlSelectiveXmlIndex"/>.
	/// </summary>
	public partial class TSqlSelectiveXmlIndex : ISql110TSqlSelectiveXmlIndex
	{		
		Boolean ISql110TSqlSelectiveXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql110TSqlSelectiveXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql110TSqlSelectiveXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql110TSqlSelectiveXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql110TSqlSelectiveXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql110TSqlSelectiveXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISql110TSqlSelectiveXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql110TSqlSelectiveXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference> ISql110TSqlSelectiveXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql110TSqlSelectiveXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> ISql110TSqlSelectiveXmlIndex.PrimaryPromotedPath 
		{
			get 
			{
				return this.PrimaryPromotedPath;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSelectiveXmlIndexReference> ISql110TSqlSelectiveXmlIndex.PrimarySelectiveXmlIndex 
		{
			get 
			{
				return this.PrimarySelectiveXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSelectiveXmlIndexReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> ISql110TSqlSelectiveXmlIndex.PromotedPaths 
		{
			get 
			{
				return this.PromotedPaths;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlNamespace> ISql110TSqlSelectiveXmlIndex.XmlNamespaces 
		{
			get 
			{
				return this.XmlNamespaces.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlXmlNamespace>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlXmlNamespace"/>.
	/// </summary>
	public partial class TSqlXmlNamespace : ISql110TSqlXmlNamespace
	{		
		String ISql110TSqlXmlNamespace.NamespaceUri 
		{
			get { return this.NamespaceUri;}
		}
		String ISql110TSqlXmlNamespace.Prefix 
		{
			get { return this.Prefix;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPromotedNodePathForXQueryType"/>.
	/// </summary>
	public partial class TSqlPromotedNodePathForXQueryType : ISql110TSqlPromotedNodePathForXQueryType
	{		
		Boolean ISql110TSqlPromotedNodePathForXQueryType.IsSingleton 
		{
			get { return this.IsSingleton;}
		}
		Int32? ISql110TSqlPromotedNodePathForXQueryType.MaxLength 
		{
			get { return this.MaxLength;}
		}
		String ISql110TSqlPromotedNodePathForXQueryType.NodePath 
		{
			get { return this.NodePath;}
		}
		String ISql110TSqlPromotedNodePathForXQueryType.Type 
		{
			get { return this.Type;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlPromotedNodePathForSqlType"/>.
	/// </summary>
	public partial class TSqlPromotedNodePathForSqlType : ISql110TSqlPromotedNodePathForSqlType
	{		
		Boolean ISql110TSqlPromotedNodePathForSqlType.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql110TSqlPromotedNodePathForSqlType.IsSingleton 
		{
			get { return this.IsSingleton;}
		}
		Int32 ISql110TSqlPromotedNodePathForSqlType.Length 
		{
			get { return this.Length;}
		}
		String ISql110TSqlPromotedNodePathForSqlType.NodePath 
		{
			get { return this.NodePath;}
		}
		Int32 ISql110TSqlPromotedNodePathForSqlType.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql110TSqlPromotedNodePathForSqlType.Scale 
		{
			get { return this.Scale;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference> ISql110TSqlPromotedNodePathForSqlType.DataType 
		{
			get 
			{
				return this.DataType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql110TSqlXmlSchemaCollection"/>.
	/// </summary>
	public partial class TSqlXmlSchemaCollection : ISql110TSqlXmlSchemaCollection
	{		
		String ISql110TSqlXmlSchemaCollection.Expression 
		{
			get { return this.Expression;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference> ISql110TSqlXmlSchemaCollection.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql110TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlColumn"/>.
	/// </summary>
	public partial class TSqlColumn : ISql120TSqlColumn
	{		
		String ISql120TSqlColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql120TSqlColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql120TSqlColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql120TSqlColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql120TSqlColumn.IsFileStream 
		{
			get { return this.IsFileStream;}
		}
		Boolean ISql120TSqlColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql120TSqlColumn.IsIdentityNotForReplication 
		{
			get { return this.IsIdentityNotForReplication;}
		}
		Boolean ISql120TSqlColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql120TSqlColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql120TSqlColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql120TSqlColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql120TSqlColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql120TSqlColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql120TSqlColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql120TSqlColumn.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql120TSqlColumn.Sparse 
		{
			get { return this.Sparse;}
		}
		XmlStyle ISql120TSqlColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql120TSqlColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference> ISql120TSqlColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableValuedFunction"/>.
	/// </summary>
	public partial class TSqlTableValuedFunction : ISql120TSqlTableValuedFunction
	{		
		Boolean? ISql120TSqlTableValuedFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql120TSqlTableValuedFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql120TSqlTableValuedFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql120TSqlTableValuedFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql120TSqlTableValuedFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql120TSqlTableValuedFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql120TSqlTableValuedFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlTableValuedFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql120TSqlTableValuedFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql120TSqlTableValuedFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql120TSqlTableValuedFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql120TSqlTableValuedFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql120TSqlTableValuedFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql120TSqlTableValuedFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		String ISql120TSqlTableValuedFunction.ReturnTableVariableName 
		{
			get { return this.ReturnTableVariableName;}
		}
		SystemDataAccessKind? ISql120TSqlTableValuedFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql120TSqlTableValuedFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql120TSqlTableValuedFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssembly> ISql120TSqlTableValuedFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlTableValuedFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn> ISql120TSqlTableValuedFunction.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlTableValuedFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter> ISql120TSqlTableValuedFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlTableValuedFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlTableValuedFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlClrTableOption> ISql120TSqlTableValuedFunction.TableOption 
		{
			get 
			{
				return this.TableOption.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlClrTableOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlTableValuedFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlScalarFunction"/>.
	/// </summary>
	public partial class TSqlScalarFunction : ISql120TSqlScalarFunction
	{		
		Boolean? ISql120TSqlScalarFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql120TSqlScalarFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISql120TSqlScalarFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISql120TSqlScalarFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISql120TSqlScalarFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISql120TSqlScalarFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql120TSqlScalarFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlScalarFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql120TSqlScalarFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		Boolean ISql120TSqlScalarFunction.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql120TSqlScalarFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql120TSqlScalarFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISql120TSqlScalarFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql120TSqlScalarFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		SystemDataAccessKind? ISql120TSqlScalarFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISql120TSqlScalarFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql120TSqlScalarFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssembly> ISql120TSqlScalarFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlScalarFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlScalarFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter> ISql120TSqlScalarFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlScalarFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlScalarFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlScalarFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlClrTableOption"/>.
	/// </summary>
	public partial class TSqlClrTableOption : ISql120TSqlClrTableOption
	{		
		String ISql120TSqlClrTableOption.ClassName 
		{
			get { return this.ClassName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlClrTableOption.OrderColumns 
		{
			get 
			{
				return this.OrderColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAggregate"/>.
	/// </summary>
	public partial class TSqlAggregate : ISql120TSqlAggregate
	{		
		String ISql120TSqlAggregate.ClassName 
		{
			get { return this.ClassName;}
		}
		Format ISql120TSqlAggregate.Format 
		{
			get { return this.Format;}
		}
		Boolean? ISql120TSqlAggregate.InvariantToDuplicates 
		{
			get { return this.InvariantToDuplicates;}
		}
		Boolean? ISql120TSqlAggregate.InvariantToNulls 
		{
			get { return this.InvariantToNulls;}
		}
		Int32? ISql120TSqlAggregate.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		Boolean? ISql120TSqlAggregate.NullIfEmpty 
		{
			get { return this.NullIfEmpty;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlAggregate.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter> ISql120TSqlAggregate.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlAggregate.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlAggregate.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlApplicationRole"/>.
	/// </summary>
	public partial class TSqlApplicationRole : ISql120TSqlApplicationRole
	{		
		String ISql120TSqlApplicationRole.Password 
		{
			get { return this.Password;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlApplicationRole.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlIndex"/>.
	/// </summary>
	public partial class TSqlIndex : ISql120TSqlIndex
	{		
		Boolean ISql120TSqlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql120TSqlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql120TSqlIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql120TSqlIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql120TSqlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql120TSqlIndex.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql120TSqlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		String ISql120TSqlIndex.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISql120TSqlIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql120TSqlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql120TSqlIndex.IncrementalStatistics 
		{
			get { return this.IncrementalStatistics;}
		}
		Boolean ISql120TSqlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql120TSqlIndex.Unique 
		{
			get { return this.Unique;}
		}
		Boolean ISql120TSqlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlIndex.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption> ISql120TSqlIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlIndex.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlIndex.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlIndex.IncludedColumns 
		{
			get 
			{
				return this.IncludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql120TSqlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAssembly"/>.
	/// </summary>
	public partial class TSqlAssembly : ISql120TSqlAssembly
	{		
		AssemblyPermissionSet ISql120TSqlAssembly.PermissionSet 
		{
			get { return this.PermissionSet;}
		}
		Boolean ISql120TSqlAssembly.Visible 
		{
			get { return this.Visible;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblySource> ISql120TSqlAssembly.AssemblySources 
		{
			get 
			{
				return this.AssemblySources.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblySource>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlAssembly.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlAssembly.ReferencedAssemblies 
		{
			get 
			{
				return this.ReferencedAssemblies.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAssemblySource"/>.
	/// </summary>
	public partial class TSqlAssemblySource : ISql120TSqlAssemblySource
	{		
		String ISql120TSqlAssemblySource.Source 
		{
			get { return this.Source;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAsymmetricKey"/>.
	/// </summary>
	public partial class TSqlAsymmetricKey : ISql120TSqlAsymmetricKey
	{		
		AsymmetricKeyAlgorithm ISql120TSqlAsymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		SymmetricKeyCreationDisposition ISql120TSqlAsymmetricKey.CreationDisposition 
		{
			get { return this.CreationDisposition;}
		}
		Boolean ISql120TSqlAsymmetricKey.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql120TSqlAsymmetricKey.ExecutableFile 
		{
			get { return this.ExecutableFile;}
		}
		String ISql120TSqlAsymmetricKey.File 
		{
			get { return this.File;}
		}
		String ISql120TSqlAsymmetricKey.Password 
		{
			get { return this.Password;}
		}
		String ISql120TSqlAsymmetricKey.ProviderKeyName 
		{
			get { return this.ProviderKeyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlAsymmetricKey.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlAsymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlAsymmetricKey.Provider 
		{
			get 
			{
				return this.Provider;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAuditAction"/>.
	/// </summary>
	public partial class TSqlAuditAction : ISql120TSqlAuditAction
	{		
		DatabaseAuditAction ISql120TSqlAuditAction.Action 
		{
			get { return this.Action;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAuditActionGroup"/>.
	/// </summary>
	public partial class TSqlAuditActionGroup : ISql120TSqlAuditActionGroup
	{		
		AuditActionGroupType ISql120TSqlAuditActionGroup.ActionGroup 
		{
			get { return this.ActionGroup;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlAuditActionSpecification"/>.
	/// </summary>
	public partial class TSqlAuditActionSpecification : ISql120TSqlAuditActionSpecification
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditAction> ISql120TSqlAuditActionSpecification.AuditActions 
		{
			get 
			{
				return this.AuditActions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditAction>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql120TSqlAuditActionSpecification.Principals 
		{
			get 
			{
				return this.Principals;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql120TSqlAuditActionSpecification.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlBrokerPriority"/>.
	/// </summary>
	public partial class TSqlBrokerPriority : ISql120TSqlBrokerPriority
	{		
		Int32 ISql120TSqlBrokerPriority.PriorityLevel 
		{
			get { return this.PriorityLevel;}
		}
		String ISql120TSqlBrokerPriority.RemoteServiceName 
		{
			get { return this.RemoteServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlBrokerPriority.ContractName 
		{
			get 
			{
				return this.ContractName;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlBrokerPriority.LocalServiceName 
		{
			get 
			{
				return this.LocalServiceName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlBuiltInServerRole"/>.
	/// </summary>
	public partial class TSqlBuiltInServerRole : ISql120TSqlBuiltInServerRole
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDataType"/>.
	/// </summary>
	public partial class TSqlDataType : ISql120TSqlDataType
	{		
		SqlDataType ISql120TSqlDataType.SqlDataType 
		{
			get { return this.SqlDataType;}
		}
		Boolean ISql120TSqlDataType.UddtIsMax 
		{
			get { return this.UddtIsMax;}
		}
		Int32 ISql120TSqlDataType.UddtLength 
		{
			get { return this.UddtLength;}
		}
		Boolean ISql120TSqlDataType.UddtNullable 
		{
			get { return this.UddtNullable;}
		}
		Int32 ISql120TSqlDataType.UddtPrecision 
		{
			get { return this.UddtPrecision;}
		}
		Int32 ISql120TSqlDataType.UddtScale 
		{
			get { return this.UddtScale;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlDataType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlDataType.Type 
		{
			get 
			{
				return this.Type.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlCertificate"/>.
	/// </summary>
	public partial class TSqlCertificate : ISql120TSqlCertificate
	{		
		Boolean ISql120TSqlCertificate.ActiveForBeginDialog 
		{
			get { return this.ActiveForBeginDialog;}
		}
		Boolean ISql120TSqlCertificate.EncryptedWithPassword 
		{
			get { return this.EncryptedWithPassword;}
		}
		String ISql120TSqlCertificate.EncryptionPassword 
		{
			get { return this.EncryptionPassword;}
		}
		String ISql120TSqlCertificate.ExistingKeysFilePath 
		{
			get { return this.ExistingKeysFilePath;}
		}
		String ISql120TSqlCertificate.ExpiryDate 
		{
			get { return this.ExpiryDate;}
		}
		Boolean ISql120TSqlCertificate.IsExistingKeyFileExecutable 
		{
			get { return this.IsExistingKeyFileExecutable;}
		}
		String ISql120TSqlCertificate.PrivateKeyDecryptionPassword 
		{
			get { return this.PrivateKeyDecryptionPassword;}
		}
		String ISql120TSqlCertificate.PrivateKeyEncryptionPassword 
		{
			get { return this.PrivateKeyEncryptionPassword;}
		}
		String ISql120TSqlCertificate.PrivateKeyFilePath 
		{
			get { return this.PrivateKeyFilePath;}
		}
		String ISql120TSqlCertificate.StartDate 
		{
			get { return this.StartDate;}
		}
		String ISql120TSqlCertificate.Subject 
		{
			get { return this.Subject;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlCertificate.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlCertificate.ExistingKeysAssembly 
		{
			get 
			{
				return this.ExistingKeysAssembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlCheckConstraint"/>.
	/// </summary>
	public partial class TSqlCheckConstraint : ISql120TSqlCheckConstraint
	{		
		Boolean ISql120TSqlCheckConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql120TSqlCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql120TSqlCheckConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference> ISql120TSqlCheckConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlClrTypeMethod"/>.
	/// </summary>
	public partial class TSqlClrTypeMethod : ISql120TSqlClrTypeMethod
	{		
		String ISql120TSqlClrTypeMethod.MethodName 
		{
			get { return this.MethodName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter> ISql120TSqlClrTypeMethod.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataType> ISql120TSqlClrTypeMethod.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataType>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlClrTypeMethodParameter"/>.
	/// </summary>
	public partial class TSqlClrTypeMethodParameter : ISql120TSqlClrTypeMethodParameter
	{		
		Boolean ISql120TSqlClrTypeMethodParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		String ISql120TSqlClrTypeMethodParameter.ParameterName 
		{
			get { return this.ParameterName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql120TSqlClrTypeMethodParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlClrTypeProperty"/>.
	/// </summary>
	public partial class TSqlClrTypeProperty : ISql120TSqlClrTypeProperty
	{		
		String ISql120TSqlClrTypeProperty.PropertyName 
		{
			get { return this.PropertyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlClrTypeProperty.ClrType 
		{
			get 
			{
				return this.ClrType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlColumnStoreIndex"/>.
	/// </summary>
	public partial class TSqlColumnStoreIndex : ISql120TSqlColumnStoreIndex
	{		
		Boolean ISql120TSqlColumnStoreIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql120TSqlColumnStoreIndex.Disabled 
		{
			get { return this.Disabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlColumnStoreIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption> ISql120TSqlColumnStoreIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlColumnStoreIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql120TSqlColumnStoreIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlColumnStoreIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlColumnStoreIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlContract"/>.
	/// </summary>
	public partial class TSqlContract : ISql120TSqlContract
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlContract.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlMessageTypeReference> ISql120TSqlContract.Messages 
		{
			get 
			{
				return this.Messages.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlMessageTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlCredential"/>.
	/// </summary>
	public partial class TSqlCredential : ISql120TSqlCredential
	{		
		String ISql120TSqlCredential.Identity 
		{
			get { return this.Identity;}
		}
		String ISql120TSqlCredential.Secret 
		{
			get { return this.Secret;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCryptographicProviderReference> ISql120TSqlCredential.CryptographicProvider 
		{
			get 
			{
				return this.CryptographicProvider.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCryptographicProviderReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlCryptographicProvider"/>.
	/// </summary>
	public partial class TSqlCryptographicProvider : ISql120TSqlCryptographicProvider
	{		
		String ISql120TSqlCryptographicProvider.DllPath 
		{
			get { return this.DllPath;}
		}
		Boolean ISql120TSqlCryptographicProvider.Enabled 
		{
			get { return this.Enabled;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDatabaseAuditSpecification"/>.
	/// </summary>
	public partial class TSqlDatabaseAuditSpecification : ISql120TSqlDatabaseAuditSpecification
	{		
		Boolean ISql120TSqlDatabaseAuditSpecification.WithState 
		{
			get { return this.WithState;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditActionGroup> ISql120TSqlDatabaseAuditSpecification.AuditActionGroups 
		{
			get 
			{
				return this.AuditActionGroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditActionGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditAction> ISql120TSqlDatabaseAuditSpecification.AuditActions 
		{
			get 
			{
				return this.AuditActions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditAction>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlServerAuditReference> ISql120TSqlDatabaseAuditSpecification.ServerAudit 
		{
			get 
			{
				return this.ServerAudit.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlServerAuditReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDatabaseDdlTrigger"/>.
	/// </summary>
	public partial class TSqlDatabaseDdlTrigger : ISql120TSqlDatabaseDdlTrigger
	{		
		Boolean? ISql120TSqlDatabaseDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql120TSqlDatabaseDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql120TSqlDatabaseDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql120TSqlDatabaseDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql120TSqlDatabaseDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlDatabaseDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISql120TSqlDatabaseDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql120TSqlDatabaseDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql120TSqlDatabaseDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql120TSqlDatabaseDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlDatabaseDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlDatabaseDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup> ISql120TSqlDatabaseDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql120TSqlDatabaseDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlDatabaseDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlDatabaseDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDatabaseEncryptionKey"/>.
	/// </summary>
	public partial class TSqlDatabaseEncryptionKey : ISql120TSqlDatabaseEncryptionKey
	{		
		SymmetricKeyAlgorithm ISql120TSqlDatabaseEncryptionKey.Algorithm 
		{
			get { return this.Algorithm;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference> ISql120TSqlDatabaseEncryptionKey.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlDatabaseEncryptionKey.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDatabaseEventNotification"/>.
	/// </summary>
	public partial class TSqlDatabaseEventNotification : ISql120TSqlDatabaseEventNotification
	{		
		String ISql120TSqlDatabaseEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql120TSqlDatabaseEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql120TSqlDatabaseEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup> ISql120TSqlDatabaseEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql120TSqlDatabaseEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDatabaseMirroringLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlDatabaseMirroringLanguageSpecifier : ISql120TSqlDatabaseMirroringLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql120TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql120TSqlDatabaseMirroringLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql120TSqlDatabaseMirroringLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		DatabaseMirroringRole ISql120TSqlDatabaseMirroringLanguageSpecifier.RoleType 
		{
			get { return this.RoleType;}
		}
		Boolean ISql120TSqlDatabaseMirroringLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql120TSqlDatabaseMirroringLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlDatabaseMirroringLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDatabaseOptions"/>.
	/// </summary>
	public partial class TSqlDatabaseOptions : ISql120TSqlDatabaseOptions
	{		
		Boolean ISql120TSqlDatabaseOptions.AllowSnapshotIsolation 
		{
			get { return this.AllowSnapshotIsolation;}
		}
		Boolean ISql120TSqlDatabaseOptions.AnsiNullDefaultOn 
		{
			get { return this.AnsiNullDefaultOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.AnsiPaddingOn 
		{
			get { return this.AnsiPaddingOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.AnsiWarningsOn 
		{
			get { return this.AnsiWarningsOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.ArithAbortOn 
		{
			get { return this.ArithAbortOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.AutoClose 
		{
			get { return this.AutoClose;}
		}
		Boolean ISql120TSqlDatabaseOptions.AutoCreateStatistics 
		{
			get { return this.AutoCreateStatistics;}
		}
		Boolean ISql120TSqlDatabaseOptions.AutoCreateStatisticsIncremental 
		{
			get { return this.AutoCreateStatisticsIncremental;}
		}
		Boolean ISql120TSqlDatabaseOptions.AutoShrink 
		{
			get { return this.AutoShrink;}
		}
		Boolean ISql120TSqlDatabaseOptions.AutoUpdateStatistics 
		{
			get { return this.AutoUpdateStatistics;}
		}
		Boolean ISql120TSqlDatabaseOptions.AutoUpdateStatisticsAsync 
		{
			get { return this.AutoUpdateStatisticsAsync;}
		}
		Boolean ISql120TSqlDatabaseOptions.ChangeTrackingAutoCleanup 
		{
			get { return this.ChangeTrackingAutoCleanup;}
		}
		Boolean ISql120TSqlDatabaseOptions.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int32 ISql120TSqlDatabaseOptions.ChangeTrackingRetentionPeriod 
		{
			get { return this.ChangeTrackingRetentionPeriod;}
		}
		TimeUnit ISql120TSqlDatabaseOptions.ChangeTrackingRetentionUnit 
		{
			get { return this.ChangeTrackingRetentionUnit;}
		}
		String ISql120TSqlDatabaseOptions.Collation 
		{
			get { return this.Collation;}
		}
		Int32 ISql120TSqlDatabaseOptions.CompatibilityLevel 
		{
			get { return this.CompatibilityLevel;}
		}
		Boolean ISql120TSqlDatabaseOptions.ConcatNullYieldsNull 
		{
			get { return this.ConcatNullYieldsNull;}
		}
		Containment ISql120TSqlDatabaseOptions.Containment 
		{
			get { return this.Containment;}
		}
		Boolean ISql120TSqlDatabaseOptions.CursorCloseOnCommit 
		{
			get { return this.CursorCloseOnCommit;}
		}
		Boolean ISql120TSqlDatabaseOptions.CursorDefaultGlobalScope 
		{
			get { return this.CursorDefaultGlobalScope;}
		}
		Boolean ISql120TSqlDatabaseOptions.DatabaseStateOffline 
		{
			get { return this.DatabaseStateOffline;}
		}
		Boolean ISql120TSqlDatabaseOptions.DateCorrelationOptimizationOn 
		{
			get { return this.DateCorrelationOptimizationOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.DBChainingOn 
		{
			get { return this.DBChainingOn;}
		}
		String ISql120TSqlDatabaseOptions.DefaultFullTextLanguage 
		{
			get { return this.DefaultFullTextLanguage;}
		}
		String ISql120TSqlDatabaseOptions.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		DelayedDurabilityMode ISql120TSqlDatabaseOptions.DelayedDurabilityMode 
		{
			get { return this.DelayedDurabilityMode;}
		}
		String ISql120TSqlDatabaseOptions.FileStreamDirectoryName 
		{
			get { return this.FileStreamDirectoryName;}
		}
		Boolean ISql120TSqlDatabaseOptions.FullTextEnabled 
		{
			get { return this.FullTextEnabled;}
		}
		Boolean ISql120TSqlDatabaseOptions.HonorBrokerPriority 
		{
			get { return this.HonorBrokerPriority;}
		}
		Boolean ISql120TSqlDatabaseOptions.MemoryOptimizedElevateToSnapshot 
		{
			get { return this.MemoryOptimizedElevateToSnapshot;}
		}
		Boolean ISql120TSqlDatabaseOptions.NestedTriggersOn 
		{
			get { return this.NestedTriggersOn;}
		}
		NonTransactedFileStreamAccess ISql120TSqlDatabaseOptions.NonTransactedFileStreamAccess 
		{
			get { return this.NonTransactedFileStreamAccess;}
		}
		Boolean ISql120TSqlDatabaseOptions.NumericRoundAbortOn 
		{
			get { return this.NumericRoundAbortOn;}
		}
		PageVerifyMode ISql120TSqlDatabaseOptions.PageVerifyMode 
		{
			get { return this.PageVerifyMode;}
		}
		ParameterizationOption ISql120TSqlDatabaseOptions.ParameterizationOption 
		{
			get { return this.ParameterizationOption;}
		}
		Boolean ISql120TSqlDatabaseOptions.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		RecoveryMode ISql120TSqlDatabaseOptions.RecoveryMode 
		{
			get { return this.RecoveryMode;}
		}
		Boolean ISql120TSqlDatabaseOptions.RecursiveTriggersOn 
		{
			get { return this.RecursiveTriggersOn;}
		}
		ServiceBrokerOption ISql120TSqlDatabaseOptions.ServiceBrokerOption 
		{
			get { return this.ServiceBrokerOption;}
		}
		Boolean ISql120TSqlDatabaseOptions.SupplementalLoggingOn 
		{
			get { return this.SupplementalLoggingOn;}
		}
		Int32 ISql120TSqlDatabaseOptions.TargetRecoveryTimePeriod 
		{
			get { return this.TargetRecoveryTimePeriod;}
		}
		TimeUnit ISql120TSqlDatabaseOptions.TargetRecoveryTimeUnit 
		{
			get { return this.TargetRecoveryTimeUnit;}
		}
		Boolean ISql120TSqlDatabaseOptions.TornPageProtectionOn 
		{
			get { return this.TornPageProtectionOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.TransactionIsolationReadCommittedSnapshot 
		{
			get { return this.TransactionIsolationReadCommittedSnapshot;}
		}
		Boolean ISql120TSqlDatabaseOptions.TransformNoiseWords 
		{
			get { return this.TransformNoiseWords;}
		}
		Boolean ISql120TSqlDatabaseOptions.Trustworthy 
		{
			get { return this.Trustworthy;}
		}
		Int16 ISql120TSqlDatabaseOptions.TwoDigitYearCutoff 
		{
			get { return this.TwoDigitYearCutoff;}
		}
		UserAccessOption ISql120TSqlDatabaseOptions.UserAccessOption 
		{
			get { return this.UserAccessOption;}
		}
		Boolean ISql120TSqlDatabaseOptions.VardecimalStorageFormatOn 
		{
			get { return this.VardecimalStorageFormatOn;}
		}
		Boolean ISql120TSqlDatabaseOptions.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlDatabaseOptions.DefaultFilegroup 
		{
			get 
			{
				return this.DefaultFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlDatabaseOptions.DefaultFileStreamFilegroup 
		{
			get 
			{
				return this.DefaultFileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDataCompressionOption"/>.
	/// </summary>
	public partial class TSqlDataCompressionOption : ISql120TSqlDataCompressionOption
	{		
		CompressionLevel ISql120TSqlDataCompressionOption.CompressionLevel 
		{
			get { return this.CompressionLevel;}
		}
		Int32 ISql120TSqlDataCompressionOption.PartitionNumber 
		{
			get { return this.PartitionNumber;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDefault"/>.
	/// </summary>
	public partial class TSqlDefault : ISql120TSqlDefault
	{		
		String ISql120TSqlDefault.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlDefault.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlDefault.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlDefaultConstraint : ISql120TSqlDefaultConstraint
	{		
		Boolean ISql120TSqlDefaultConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql120TSqlDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISql120TSqlDefaultConstraint.WithValues 
		{
			get { return this.WithValues;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference> ISql120TSqlDefaultConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlDmlTrigger"/>.
	/// </summary>
	public partial class TSqlDmlTrigger : ISql120TSqlDmlTrigger
	{		
		Boolean? ISql120TSqlDmlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql120TSqlDmlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		OrderRestriction ISql120TSqlDmlTrigger.DeleteOrderRestriction 
		{
			get { return this.DeleteOrderRestriction;}
		}
		Boolean ISql120TSqlDmlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql120TSqlDmlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql120TSqlDmlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlDmlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		OrderRestriction ISql120TSqlDmlTrigger.InsertOrderRestriction 
		{
			get { return this.InsertOrderRestriction;}
		}
		Boolean ISql120TSqlDmlTrigger.IsDeleteTrigger 
		{
			get { return this.IsDeleteTrigger;}
		}
		Boolean ISql120TSqlDmlTrigger.IsInsertTrigger 
		{
			get { return this.IsInsertTrigger;}
		}
		Boolean ISql120TSqlDmlTrigger.IsUpdateTrigger 
		{
			get { return this.IsUpdateTrigger;}
		}
		String ISql120TSqlDmlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean ISql120TSqlDmlTrigger.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		Boolean? ISql120TSqlDmlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql120TSqlDmlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		OrderRestriction ISql120TSqlDmlTrigger.UpdateOrderRestriction 
		{
			get { return this.UpdateOrderRestriction;}
		}
		Boolean ISql120TSqlDmlTrigger.WithAppend 
		{
			get { return this.WithAppend;}
		}
		Boolean ISql120TSqlDmlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlDmlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlDmlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlDmlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlDmlTrigger.TriggerObject 
		{
			get 
			{
				return this.TriggerObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlDmlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEndpoint"/>.
	/// </summary>
	public partial class TSqlEndpoint : ISql120TSqlEndpoint
	{		
		Payload ISql120TSqlEndpoint.Payload 
		{
			get { return this.Payload;}
		}
		Protocol ISql120TSqlEndpoint.Protocol 
		{
			get { return this.Protocol;}
		}
		State ISql120TSqlEndpoint.State 
		{
			get { return this.State;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlEndpoint.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IEndpointLanguageSpecifier> ISql120TSqlEndpoint.PayloadSpecifier 
		{
			get 
			{
				return this.PayloadSpecifier;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IProtocolSpecifier > ISql120TSqlEndpoint.ProtocolSpecifier 
		{
			get 
			{
				return this.ProtocolSpecifier;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlErrorMessage"/>.
	/// </summary>
	public partial class TSqlErrorMessage : ISql120TSqlErrorMessage
	{		
		String ISql120TSqlErrorMessage.Language 
		{
			get { return this.Language;}
		}
		Int32 ISql120TSqlErrorMessage.MessageNumber 
		{
			get { return this.MessageNumber;}
		}
		String ISql120TSqlErrorMessage.MessageText 
		{
			get { return this.MessageText;}
		}
		Int32 ISql120TSqlErrorMessage.Severity 
		{
			get { return this.Severity;}
		}
		Boolean ISql120TSqlErrorMessage.WithLog 
		{
			get { return this.WithLog;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventGroup"/>.
	/// </summary>
	public partial class TSqlEventGroup : ISql120TSqlEventGroup
	{		
		EventGroupType ISql120TSqlEventGroup.Group 
		{
			get { return this.Group;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventSession"/>.
	/// </summary>
	public partial class TSqlEventSession : ISql120TSqlEventSession
	{		
		EventRetentionMode ISql120TSqlEventSession.EventRetentionMode 
		{
			get { return this.EventRetentionMode;}
		}
		Int32 ISql120TSqlEventSession.MaxDispatchLatency 
		{
			get { return this.MaxDispatchLatency;}
		}
		Int32 ISql120TSqlEventSession.MaxEventSize 
		{
			get { return this.MaxEventSize;}
		}
		MemoryUnit ISql120TSqlEventSession.MaxEventSizeUnit 
		{
			get { return this.MaxEventSizeUnit;}
		}
		Int32 ISql120TSqlEventSession.MaxMemory 
		{
			get { return this.MaxMemory;}
		}
		MemoryUnit ISql120TSqlEventSession.MaxMemoryUnit 
		{
			get { return this.MaxMemoryUnit;}
		}
		MemoryPartitionMode ISql120TSqlEventSession.MemoryPartitionMode 
		{
			get { return this.MemoryPartitionMode;}
		}
		Boolean ISql120TSqlEventSession.StartupState 
		{
			get { return this.StartupState;}
		}
		Boolean ISql120TSqlEventSession.TrackCausality 
		{
			get { return this.TrackCausality;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlEventSession.EventDefinitions 
		{
			get 
			{
				return this.EventDefinitions;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlEventSession.EventTargets 
		{
			get 
			{
				return this.EventTargets;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventSessionAction"/>.
	/// </summary>
	public partial class TSqlEventSessionAction : ISql120TSqlEventSessionAction
	{		
		String ISql120TSqlEventSessionAction.ActionName 
		{
			get { return this.ActionName;}
		}
		String ISql120TSqlEventSessionAction.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql120TSqlEventSessionAction.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventSessionDefinitions"/>.
	/// </summary>
	public partial class TSqlEventSessionDefinitions : ISql120TSqlEventSessionDefinitions
	{		
		String ISql120TSqlEventSessionDefinitions.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql120TSqlEventSessionDefinitions.EventName 
		{
			get { return this.EventName;}
		}
		String ISql120TSqlEventSessionDefinitions.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
		String ISql120TSqlEventSessionDefinitions.WhereExpression 
		{
			get { return this.WhereExpression;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventSessionAction> ISql120TSqlEventSessionDefinitions.Actions 
		{
			get 
			{
				return this.Actions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventSessionAction>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlEventSessionDefinitions.AttributeSettings 
		{
			get 
			{
				return this.AttributeSettings;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventSessionSetting"/>.
	/// </summary>
	public partial class TSqlEventSessionSetting : ISql120TSqlEventSessionSetting
	{		
		String ISql120TSqlEventSessionSetting.SettingName 
		{
			get { return this.SettingName;}
		}
		String ISql120TSqlEventSessionSetting.SettingValue 
		{
			get { return this.SettingValue;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventSessionTarget"/>.
	/// </summary>
	public partial class TSqlEventSessionTarget : ISql120TSqlEventSessionTarget
	{		
		String ISql120TSqlEventSessionTarget.EventModuleGuid 
		{
			get { return this.EventModuleGuid;}
		}
		String ISql120TSqlEventSessionTarget.EventPackageName 
		{
			get { return this.EventPackageName;}
		}
		String ISql120TSqlEventSessionTarget.TargetName 
		{
			get { return this.TargetName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlEventSessionTarget.ParameterSettings 
		{
			get 
			{
				return this.ParameterSettings;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlEventTypeSpecifier"/>.
	/// </summary>
	public partial class TSqlEventTypeSpecifier : ISql120TSqlEventTypeSpecifier
	{		
		EventType ISql120TSqlEventTypeSpecifier.EventType 
		{
			get { return this.EventType;}
		}
		OrderRestriction ISql120TSqlEventTypeSpecifier.Order 
		{
			get { return this.Order;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlExtendedProcedure"/>.
	/// </summary>
	public partial class TSqlExtendedProcedure : ISql120TSqlExtendedProcedure
	{		
		Boolean ISql120TSqlExtendedProcedure.ExeccuteAsCaller 
		{
			get { return this.ExeccuteAsCaller;}
		}
		Boolean ISql120TSqlExtendedProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlExtendedProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql120TSqlExtendedProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlExtendedProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter> ISql120TSqlExtendedProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlExtendedProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlExtendedProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlExtendedProperty"/>.
	/// </summary>
	public partial class TSqlExtendedProperty : ISql120TSqlExtendedProperty
	{		
		String ISql120TSqlExtendedProperty.Value 
		{
			get { return this.Value;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost> ISql120TSqlExtendedProperty.Host 
		{
			get 
			{
				return this.Host;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSqlFile"/>.
	/// </summary>
	public partial class TSqlSqlFile : ISql120TSqlSqlFile
	{		
		Int32? ISql120TSqlSqlFile.FileGrowth 
		{
			get { return this.FileGrowth;}
		}
		MemoryUnit ISql120TSqlSqlFile.FileGrowthUnit 
		{
			get { return this.FileGrowthUnit;}
		}
		String ISql120TSqlSqlFile.FileName 
		{
			get { return this.FileName;}
		}
		Boolean ISql120TSqlSqlFile.IsLogFile 
		{
			get { return this.IsLogFile;}
		}
		Int32? ISql120TSqlSqlFile.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql120TSqlSqlFile.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		Boolean ISql120TSqlSqlFile.Offline 
		{
			get { return this.Offline;}
		}
		Int32? ISql120TSqlSqlFile.Size 
		{
			get { return this.Size;}
		}
		MemoryUnit ISql120TSqlSqlFile.SizeUnit 
		{
			get { return this.SizeUnit;}
		}
		Boolean ISql120TSqlSqlFile.Unlimited 
		{
			get { return this.Unlimited;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlSqlFile.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlFilegroup"/>.
	/// </summary>
	public partial class TSqlFilegroup : ISql120TSqlFilegroup
	{		
		Boolean ISql120TSqlFilegroup.ContainsFileStream 
		{
			get { return this.ContainsFileStream;}
		}
		Boolean ISql120TSqlFilegroup.ContainsMemoryOptimizedData 
		{
			get { return this.ContainsMemoryOptimizedData;}
		}
		Boolean ISql120TSqlFilegroup.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlForeignKeyConstraint"/>.
	/// </summary>
	public partial class TSqlForeignKeyConstraint : ISql120TSqlForeignKeyConstraint
	{		
		ForeignKeyAction ISql120TSqlForeignKeyConstraint.DeleteAction 
		{
			get { return this.DeleteAction;}
		}
		Boolean ISql120TSqlForeignKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql120TSqlForeignKeyConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		ForeignKeyAction ISql120TSqlForeignKeyConstraint.UpdateAction 
		{
			get { return this.UpdateAction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlForeignKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlForeignKeyConstraint.ForeignColumns 
		{
			get 
			{
				return this.ForeignColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference> ISql120TSqlForeignKeyConstraint.ForeignTable 
		{
			get 
			{
				return this.ForeignTable.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference> ISql120TSqlForeignKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlFullTextCatalog"/>.
	/// </summary>
	public partial class TSqlFullTextCatalog : ISql120TSqlFullTextCatalog
	{		
		Boolean? ISql120TSqlFullTextCatalog.AccentSensitivity 
		{
			get { return this.AccentSensitivity;}
		}
		Boolean ISql120TSqlFullTextCatalog.IsDefault 
		{
			get { return this.IsDefault;}
		}
		String ISql120TSqlFullTextCatalog.Path 
		{
			get { return this.Path;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlFullTextCatalog.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlFullTextCatalog.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlFullTextIndex"/>.
	/// </summary>
	public partial class TSqlFullTextIndex : ISql120TSqlFullTextIndex
	{		
		ChangeTrackingOption ISql120TSqlFullTextIndex.ChangeTracking 
		{
			get { return this.ChangeTracking;}
		}
		Boolean ISql120TSqlFullTextIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql120TSqlFullTextIndex.Replicated 
		{
			get { return this.Replicated;}
		}
		Boolean ISql120TSqlFullTextIndex.StopListOff 
		{
			get { return this.StopListOff;}
		}
		Boolean ISql120TSqlFullTextIndex.UseSystemStopList 
		{
			get { return this.UseSystemStopList;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference> ISql120TSqlFullTextIndex.Catalog 
		{
			get 
			{
				return this.Catalog;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFullTextIndexColumnSpecifier> ISql120TSqlFullTextIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFullTextIndexColumnSpecifier>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlFullTextIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql120TSqlFullTextIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSearchPropertyListReference> ISql120TSqlFullTextIndex.SearchPropertyList 
		{
			get 
			{
				return this.SearchPropertyList.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSearchPropertyListReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlFullTextIndex.StopList 
		{
			get 
			{
				return this.StopList;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlFullTextIndex.UniqueIndexName 
		{
			get 
			{
				return this.UniqueIndexName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlFullTextIndexColumnSpecifier"/>.
	/// </summary>
	public partial class TSqlFullTextIndexColumnSpecifier : ISql120TSqlFullTextIndexColumnSpecifier
	{		
		Int32? ISql120TSqlFullTextIndexColumnSpecifier.LanguageId 
		{
			get { return this.LanguageId;}
		}
		Boolean ISql120TSqlFullTextIndexColumnSpecifier.PartOfStatisticalSemantics 
		{
			get { return this.PartOfStatisticalSemantics;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlFullTextIndexColumnSpecifier.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlFullTextIndexColumnSpecifier.TypeColumn 
		{
			get 
			{
				return this.TypeColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlFullTextStopList"/>.
	/// </summary>
	public partial class TSqlFullTextStopList : ISql120TSqlFullTextStopList
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlFullTextStopList.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlLinkedServer"/>.
	/// </summary>
	public partial class TSqlLinkedServer : ISql120TSqlLinkedServer
	{		
		String ISql120TSqlLinkedServer.Catalog 
		{
			get { return this.Catalog;}
		}
		Boolean ISql120TSqlLinkedServer.CollationCompatible 
		{
			get { return this.CollationCompatible;}
		}
		String ISql120TSqlLinkedServer.CollationName 
		{
			get { return this.CollationName;}
		}
		Int32 ISql120TSqlLinkedServer.ConnectTimeout 
		{
			get { return this.ConnectTimeout;}
		}
		Boolean ISql120TSqlLinkedServer.DataAccess 
		{
			get { return this.DataAccess;}
		}
		String ISql120TSqlLinkedServer.DataSource 
		{
			get { return this.DataSource;}
		}
		Boolean ISql120TSqlLinkedServer.IsDistributor 
		{
			get { return this.IsDistributor;}
		}
		Boolean ISql120TSqlLinkedServer.IsPublisher 
		{
			get { return this.IsPublisher;}
		}
		Boolean ISql120TSqlLinkedServer.IsSubscriber 
		{
			get { return this.IsSubscriber;}
		}
		Boolean ISql120TSqlLinkedServer.LazySchemaValidationEnabled 
		{
			get { return this.LazySchemaValidationEnabled;}
		}
		String ISql120TSqlLinkedServer.Location 
		{
			get { return this.Location;}
		}
		String ISql120TSqlLinkedServer.ProductName 
		{
			get { return this.ProductName;}
		}
		String ISql120TSqlLinkedServer.ProviderName 
		{
			get { return this.ProviderName;}
		}
		String ISql120TSqlLinkedServer.ProviderString 
		{
			get { return this.ProviderString;}
		}
		Int32 ISql120TSqlLinkedServer.QueryTimeout 
		{
			get { return this.QueryTimeout;}
		}
		Boolean ISql120TSqlLinkedServer.RemoteProcTransactionPromotionEnabled 
		{
			get { return this.RemoteProcTransactionPromotionEnabled;}
		}
		Boolean ISql120TSqlLinkedServer.RpcEnabled 
		{
			get { return this.RpcEnabled;}
		}
		Boolean ISql120TSqlLinkedServer.RpcOutEnabled 
		{
			get { return this.RpcOutEnabled;}
		}
		Boolean ISql120TSqlLinkedServer.UseRemoteCollation 
		{
			get { return this.UseRemoteCollation;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlLinkedServerLogin"/>.
	/// </summary>
	public partial class TSqlLinkedServerLogin : ISql120TSqlLinkedServerLogin
	{		
		String ISql120TSqlLinkedServerLogin.LinkedServerLoginName 
		{
			get { return this.LinkedServerLoginName;}
		}
		String ISql120TSqlLinkedServerLogin.LinkedServerPassword 
		{
			get { return this.LinkedServerPassword;}
		}
		Boolean ISql120TSqlLinkedServerLogin.UseSelf 
		{
			get { return this.UseSelf;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLinkedServerReference> ISql120TSqlLinkedServerLogin.LinkedServer 
		{
			get 
			{
				return this.LinkedServer.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLinkedServerReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlLinkedServerLogin.LocalLogin 
		{
			get 
			{
				return this.LocalLogin.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlLogin"/>.
	/// </summary>
	public partial class TSqlLogin : ISql120TSqlLogin
	{		
		Boolean ISql120TSqlLogin.CheckExpiration 
		{
			get { return this.CheckExpiration;}
		}
		Boolean ISql120TSqlLogin.CheckPolicy 
		{
			get { return this.CheckPolicy;}
		}
		String ISql120TSqlLogin.DefaultDatabase 
		{
			get { return this.DefaultDatabase;}
		}
		String ISql120TSqlLogin.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		Boolean ISql120TSqlLogin.Disabled 
		{
			get { return this.Disabled;}
		}
		LoginEncryptionOption ISql120TSqlLogin.EncryptionOption 
		{
			get { return this.EncryptionOption;}
		}
		Boolean ISql120TSqlLogin.MappedToWindowsLogin 
		{
			get { return this.MappedToWindowsLogin;}
		}
		String ISql120TSqlLogin.Password 
		{
			get { return this.Password;}
		}
		Boolean ISql120TSqlLogin.PasswordHashed 
		{
			get { return this.PasswordHashed;}
		}
		Boolean ISql120TSqlLogin.PasswordMustChange 
		{
			get { return this.PasswordMustChange;}
		}
		String ISql120TSqlLogin.Sid 
		{
			get { return this.Sid;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference> ISql120TSqlLogin.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlLogin.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCredentialReference> ISql120TSqlLogin.Credential 
		{
			get 
			{
				return this.Credential.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCredentialReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlMasterKey"/>.
	/// </summary>
	public partial class TSqlMasterKey : ISql120TSqlMasterKey
	{		
		String ISql120TSqlMasterKey.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlMessageType"/>.
	/// </summary>
	public partial class TSqlMessageType : ISql120TSqlMessageType
	{		
		ValidationMethod ISql120TSqlMessageType.ValidationMethod 
		{
			get { return this.ValidationMethod;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlMessageType.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference> ISql120TSqlMessageType.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPartitionFunction"/>.
	/// </summary>
	public partial class TSqlPartitionFunction : ISql120TSqlPartitionFunction
	{		
		PartitionRange ISql120TSqlPartitionFunction.Range 
		{
			get { return this.Range;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionValue> ISql120TSqlPartitionFunction.BoundaryValues 
		{
			get 
			{
				return this.BoundaryValues.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionValue>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlPartitionFunction.ParameterType 
		{
			get 
			{
				return this.ParameterType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPartitionScheme"/>.
	/// </summary>
	public partial class TSqlPartitionScheme : ISql120TSqlPartitionScheme
	{		
		Boolean ISql120TSqlPartitionScheme.AllToOneFilegroup 
		{
			get { return this.AllToOneFilegroup;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlPartitionScheme.Filegroups 
		{
			get 
			{
				return this.Filegroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionFunctionReference> ISql120TSqlPartitionScheme.PartitionFunction 
		{
			get 
			{
				return this.PartitionFunction.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionFunctionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPartitionValue"/>.
	/// </summary>
	public partial class TSqlPartitionValue : ISql120TSqlPartitionValue
	{		
		String ISql120TSqlPartitionValue.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlPartitionValue.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPermission"/>.
	/// </summary>
	public partial class TSqlPermission : ISql120TSqlPermission
	{		
		PermissionAction ISql120TSqlPermission.PermissionAction 
		{
			get { return this.PermissionAction;}
		}
		PermissionType ISql120TSqlPermission.PermissionType 
		{
			get { return this.PermissionType;}
		}
		Boolean ISql120TSqlPermission.WithAllPrivileges 
		{
			get { return this.WithAllPrivileges;}
		}
		Boolean ISql120TSqlPermission.WithGrantOption 
		{
			get { return this.WithGrantOption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlPermission.ExcludedColumns 
		{
			get 
			{
				return this.ExcludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql120TSqlPermission.Grantee 
		{
			get 
			{
				return this.Grantee;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISql120TSqlPermission.Grantor 
		{
			get 
			{
				return this.Grantor;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlPermission.RevokedGrantOptionColumns 
		{
			get 
			{
				return this.RevokedGrantOptionColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISql120TSqlPermission.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlPrimaryKeyConstraint : ISql120TSqlPrimaryKeyConstraint
	{		
		Boolean ISql120TSqlPrimaryKeyConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql120TSqlPrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql120TSqlPrimaryKeyConstraint.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql120TSqlPrimaryKeyConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql120TSqlPrimaryKeyConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlPrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption> ISql120TSqlPrimaryKeyConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlPrimaryKeyConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlPrimaryKeyConstraint.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlPrimaryKeyConstraint.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference> ISql120TSqlPrimaryKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlPrimaryKeyConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlPrimaryKeyConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlProcedure"/>.
	/// </summary>
	public partial class TSqlProcedure : ISql120TSqlProcedure
	{		
		Boolean? ISql120TSqlProcedure.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql120TSqlProcedure.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql120TSqlProcedure.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql120TSqlProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql120TSqlProcedure.ForReplication 
		{
			get { return this.ForReplication;}
		}
		Boolean ISql120TSqlProcedure.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		String ISql120TSqlProcedure.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql120TSqlProcedure.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql120TSqlProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql120TSqlProcedure.WithNativeCompilation 
		{
			get { return this.WithNativeCompilation;}
		}
		Boolean ISql120TSqlProcedure.WithRecompile 
		{
			get { return this.WithRecompile;}
		}
		Boolean ISql120TSqlProcedure.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlProcedure.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlProcedure.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter> ISql120TSqlProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlProcedureReference> ISql120TSqlProcedure.ParentProcedure 
		{
			get 
			{
				return this.ParentProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlProcedureReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlQueue"/>.
	/// </summary>
	public partial class TSqlQueue : ISql120TSqlQueue
	{		
		Boolean ISql120TSqlQueue.ActivationExecuteAsCaller 
		{
			get { return this.ActivationExecuteAsCaller;}
		}
		Boolean ISql120TSqlQueue.ActivationExecuteAsOwner 
		{
			get { return this.ActivationExecuteAsOwner;}
		}
		Boolean ISql120TSqlQueue.ActivationExecuteAsSelf 
		{
			get { return this.ActivationExecuteAsSelf;}
		}
		Int32? ISql120TSqlQueue.ActivationMaxQueueReaders 
		{
			get { return this.ActivationMaxQueueReaders;}
		}
		Boolean? ISql120TSqlQueue.ActivationStatusOn 
		{
			get { return this.ActivationStatusOn;}
		}
		Boolean ISql120TSqlQueue.PoisonMessageHandlingStatusOn 
		{
			get { return this.PoisonMessageHandlingStatusOn;}
		}
		Boolean ISql120TSqlQueue.RetentionOn 
		{
			get { return this.RetentionOn;}
		}
		Boolean ISql120TSqlQueue.StatusOn 
		{
			get { return this.StatusOn;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlProcedureReference> ISql120TSqlQueue.ActivationProcedure 
		{
			get 
			{
				return this.ActivationProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlProcedureReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn> ISql120TSqlQueue.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlQueue.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlQueue.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlQueue.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlQueue.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlQueue.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlQueue.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlQueueEventNotification"/>.
	/// </summary>
	public partial class TSqlQueueEventNotification : ISql120TSqlQueueEventNotification
	{		
		String ISql120TSqlQueueEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql120TSqlQueueEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql120TSqlQueueEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup> ISql120TSqlQueueEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql120TSqlQueueEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlQueueReference> ISql120TSqlQueueEventNotification.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlRemoteServiceBinding"/>.
	/// </summary>
	public partial class TSqlRemoteServiceBinding : ISql120TSqlRemoteServiceBinding
	{		
		Boolean ISql120TSqlRemoteServiceBinding.Anonymous 
		{
			get { return this.Anonymous;}
		}
		String ISql120TSqlRemoteServiceBinding.Service 
		{
			get { return this.Service;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlRemoteServiceBinding.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlRemoteServiceBinding.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlResourceGovernor"/>.
	/// </summary>
	public partial class TSqlResourceGovernor : ISql120TSqlResourceGovernor
	{		
		Boolean? ISql120TSqlResourceGovernor.Enabled 
		{
			get { return this.Enabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlResourceGovernor.ClassifierFunction 
		{
			get 
			{
				return this.ClassifierFunction;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlResourcePool"/>.
	/// </summary>
	public partial class TSqlResourcePool : ISql120TSqlResourcePool
	{		
		Int32 ISql120TSqlResourcePool.CapCpuPercent 
		{
			get { return this.CapCpuPercent;}
		}
		Int32 ISql120TSqlResourcePool.MaxCpuPercent 
		{
			get { return this.MaxCpuPercent;}
		}
		Int32 ISql120TSqlResourcePool.MaxIopsPerVolume 
		{
			get { return this.MaxIopsPerVolume;}
		}
		Int32 ISql120TSqlResourcePool.MaxMemoryPercent 
		{
			get { return this.MaxMemoryPercent;}
		}
		Int32 ISql120TSqlResourcePool.MinCpuPercent 
		{
			get { return this.MinCpuPercent;}
		}
		Int32 ISql120TSqlResourcePool.MinIopsPerVolume 
		{
			get { return this.MinIopsPerVolume;}
		}
		Int32 ISql120TSqlResourcePool.MinMemoryPercent 
		{
			get { return this.MinMemoryPercent;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlRole"/>.
	/// </summary>
	public partial class TSqlRole : ISql120TSqlRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlRoleMembership"/>.
	/// </summary>
	public partial class TSqlRoleMembership : ISql120TSqlRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISql120TSqlRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlRoleReference> ISql120TSqlRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlRoute"/>.
	/// </summary>
	public partial class TSqlRoute : ISql120TSqlRoute
	{		
		String ISql120TSqlRoute.Address 
		{
			get { return this.Address;}
		}
		String ISql120TSqlRoute.BrokerInstance 
		{
			get { return this.BrokerInstance;}
		}
		Int32? ISql120TSqlRoute.Lifetime 
		{
			get { return this.Lifetime;}
		}
		String ISql120TSqlRoute.MirrorAddress 
		{
			get { return this.MirrorAddress;}
		}
		String ISql120TSqlRoute.ServiceName 
		{
			get { return this.ServiceName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlRoute.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlRule"/>.
	/// </summary>
	public partial class TSqlRule : ISql120TSqlRule
	{		
		String ISql120TSqlRule.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlRule.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlRule.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSchema"/>.
	/// </summary>
	public partial class TSqlSchema : ISql120TSqlSchema
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlSchema.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSearchProperty"/>.
	/// </summary>
	public partial class TSqlSearchProperty : ISql120TSqlSearchProperty
	{		
		String ISql120TSqlSearchProperty.Description 
		{
			get { return this.Description;}
		}
		Int32 ISql120TSqlSearchProperty.Identifier 
		{
			get { return this.Identifier;}
		}
		String ISql120TSqlSearchProperty.PropertySetGuid 
		{
			get { return this.PropertySetGuid;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSearchPropertyListReference> ISql120TSqlSearchProperty.SearchPropertyList 
		{
			get 
			{
				return this.SearchPropertyList.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSearchPropertyListReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSearchPropertyList"/>.
	/// </summary>
	public partial class TSqlSearchPropertyList : ISql120TSqlSearchPropertyList
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlSearchPropertyList.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSequence"/>.
	/// </summary>
	public partial class TSqlSequence : ISql120TSqlSequence
	{		
		Int32? ISql120TSqlSequence.CacheSize 
		{
			get { return this.CacheSize;}
		}
		String ISql120TSqlSequence.IncrementValue 
		{
			get { return this.IncrementValue;}
		}
		Boolean ISql120TSqlSequence.IsCached 
		{
			get { return this.IsCached;}
		}
		Boolean ISql120TSqlSequence.IsCycling 
		{
			get { return this.IsCycling;}
		}
		String ISql120TSqlSequence.MaxValue 
		{
			get { return this.MaxValue;}
		}
		String ISql120TSqlSequence.MinValue 
		{
			get { return this.MinValue;}
		}
		Boolean ISql120TSqlSequence.NoMaxValue 
		{
			get { return this.NoMaxValue;}
		}
		Boolean ISql120TSqlSequence.NoMinValue 
		{
			get { return this.NoMinValue;}
		}
		String ISql120TSqlSequence.StartValue 
		{
			get { return this.StartValue;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlSequence.DataType 
		{
			get 
			{
				return this.DataType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlSequence.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServerAudit"/>.
	/// </summary>
	public partial class TSqlServerAudit : ISql120TSqlServerAudit
	{		
		String ISql120TSqlServerAudit.AuditGuid 
		{
			get { return this.AuditGuid;}
		}
		AuditTarget ISql120TSqlServerAudit.AuditTarget 
		{
			get { return this.AuditTarget;}
		}
		Boolean ISql120TSqlServerAudit.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISql120TSqlServerAudit.FilePath 
		{
			get { return this.FilePath;}
		}
		Int32? ISql120TSqlServerAudit.MaxFiles 
		{
			get { return this.MaxFiles;}
		}
		Int32? ISql120TSqlServerAudit.MaxRolloverFiles 
		{
			get { return this.MaxRolloverFiles;}
		}
		Int32? ISql120TSqlServerAudit.MaxSize 
		{
			get { return this.MaxSize;}
		}
		MemoryUnit ISql120TSqlServerAudit.MaxSizeUnit 
		{
			get { return this.MaxSizeUnit;}
		}
		FailureAction ISql120TSqlServerAudit.OnFailure 
		{
			get { return this.OnFailure;}
		}
		String ISql120TSqlServerAudit.PredicateExpression 
		{
			get { return this.PredicateExpression;}
		}
		Int32 ISql120TSqlServerAudit.QueueDelay 
		{
			get { return this.QueueDelay;}
		}
		Boolean ISql120TSqlServerAudit.ReserveDiskSpace 
		{
			get { return this.ReserveDiskSpace;}
		}
		Boolean ISql120TSqlServerAudit.UnlimitedFileSize 
		{
			get { return this.UnlimitedFileSize;}
		}
		Boolean ISql120TSqlServerAudit.UnlimitedMaxRolloverFiles 
		{
			get { return this.UnlimitedMaxRolloverFiles;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServerAuditSpecification"/>.
	/// </summary>
	public partial class TSqlServerAuditSpecification : ISql120TSqlServerAuditSpecification
	{		
		Boolean ISql120TSqlServerAuditSpecification.StateOn 
		{
			get { return this.StateOn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditActionGroup> ISql120TSqlServerAuditSpecification.AuditActionGroups 
		{
			get 
			{
				return this.AuditActionGroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAuditActionGroup>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlServerAuditReference> ISql120TSqlServerAuditSpecification.ServerAudit 
		{
			get 
			{
				return this.ServerAudit.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlServerAuditReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServerDdlTrigger"/>.
	/// </summary>
	public partial class TSqlServerDdlTrigger : ISql120TSqlServerDdlTrigger
	{		
		Boolean? ISql120TSqlServerDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISql120TSqlServerDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISql120TSqlServerDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISql120TSqlServerDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISql120TSqlServerDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISql120TSqlServerDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		Boolean ISql120TSqlServerDdlTrigger.IsLogon 
		{
			get { return this.IsLogon;}
		}
		String ISql120TSqlServerDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISql120TSqlServerDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISql120TSqlServerDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISql120TSqlServerDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlServerDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlServerDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup> ISql120TSqlServerDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql120TSqlServerDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlServerDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference> ISql120TSqlServerDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServerEventNotification"/>.
	/// </summary>
	public partial class TSqlServerEventNotification : ISql120TSqlServerEventNotification
	{		
		String ISql120TSqlServerEventNotification.BrokerInstanceSpecifier 
		{
			get { return this.BrokerInstanceSpecifier;}
		}
		String ISql120TSqlServerEventNotification.BrokerService 
		{
			get { return this.BrokerService;}
		}
		Boolean ISql120TSqlServerEventNotification.WithFanIn 
		{
			get { return this.WithFanIn;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup> ISql120TSqlServerEventNotification.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISql120TSqlServerEventNotification.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServerOptions"/>.
	/// </summary>
	public partial class TSqlServerOptions : ISql120TSqlServerOptions
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServerRoleMembership"/>.
	/// </summary>
	public partial class TSqlServerRoleMembership : ISql120TSqlServerRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> ISql120TSqlServerRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlRoleReference> ISql120TSqlServerRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlService"/>.
	/// </summary>
	public partial class TSqlService : ISql120TSqlService
	{		
		Boolean ISql120TSqlService.UseDefaultContract 
		{
			get { return this.UseDefaultContract;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlService.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlContractReference> ISql120TSqlService.Contracts 
		{
			get 
			{
				return this.Contracts.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlContractReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlQueueReference> ISql120TSqlService.Queue 
		{
			get 
			{
				return this.Queue.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlQueueReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlServiceBrokerLanguageSpecifier"/>.
	/// </summary>
	public partial class TSqlServiceBrokerLanguageSpecifier : ISql120TSqlServiceBrokerLanguageSpecifier
	{		
		ServiceBrokerEncryptionAlgorithm ISql120TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart1 
		{
			get { return this.EncryptionAlgorithmPart1;}
		}
		ServiceBrokerEncryptionAlgorithm ISql120TSqlServiceBrokerLanguageSpecifier.EncryptionAlgorithmPart2 
		{
			get { return this.EncryptionAlgorithmPart2;}
		}
		EncryptionMode ISql120TSqlServiceBrokerLanguageSpecifier.EncryptionMode 
		{
			get { return this.EncryptionMode;}
		}
		Boolean ISql120TSqlServiceBrokerLanguageSpecifier.MessageForwardingEnabled 
		{
			get { return this.MessageForwardingEnabled;}
		}
		Int32 ISql120TSqlServiceBrokerLanguageSpecifier.MessageForwardSize 
		{
			get { return this.MessageForwardSize;}
		}
		Boolean ISql120TSqlServiceBrokerLanguageSpecifier.UseCertificateFirst 
		{
			get { return this.UseCertificateFirst;}
		}
		AuthenticationModes ISql120TSqlServiceBrokerLanguageSpecifier.WindowsAuthenticationMode 
		{
			get { return this.WindowsAuthenticationMode;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlServiceBrokerLanguageSpecifier.AuthenticationCertificate 
		{
			get 
			{
				return this.AuthenticationCertificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSignature"/>.
	/// </summary>
	public partial class TSqlSignature : ISql120TSqlSignature
	{		
		Boolean ISql120TSqlSignature.IsCounterSignature 
		{
			get { return this.IsCounterSignature;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlSignature.EncryptionMechanism 
		{
			get 
			{
				return this.EncryptionMechanism;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlSignature.SignedObject 
		{
			get 
			{
				return this.SignedObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSignatureEncryptionMechanism"/>.
	/// </summary>
	public partial class TSqlSignatureEncryptionMechanism : ISql120TSqlSignatureEncryptionMechanism
	{		
		String ISql120TSqlSignatureEncryptionMechanism.Password 
		{
			get { return this.Password;}
		}
		String ISql120TSqlSignatureEncryptionMechanism.SignedBlob 
		{
			get { return this.SignedBlob;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference> ISql120TSqlSignatureEncryptionMechanism.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlSignatureEncryptionMechanism.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSpatialIndex"/>.
	/// </summary>
	public partial class TSqlSpatialIndex : ISql120TSqlSpatialIndex
	{		
		Boolean ISql120TSqlSpatialIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql120TSqlSpatialIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISql120TSqlSpatialIndex.CellsPerObject 
		{
			get { return this.CellsPerObject;}
		}
		CompressionLevel ISql120TSqlSpatialIndex.DataCompression 
		{
			get { return this.DataCompression;}
		}
		Boolean ISql120TSqlSpatialIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32 ISql120TSqlSpatialIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Degree ISql120TSqlSpatialIndex.GridLevel1Density 
		{
			get { return this.GridLevel1Density;}
		}
		Degree ISql120TSqlSpatialIndex.GridLevel2Density 
		{
			get { return this.GridLevel2Density;}
		}
		Degree ISql120TSqlSpatialIndex.GridLevel3Density 
		{
			get { return this.GridLevel3Density;}
		}
		Degree ISql120TSqlSpatialIndex.GridLevel4Density 
		{
			get { return this.GridLevel4Density;}
		}
		Boolean ISql120TSqlSpatialIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql120TSqlSpatialIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Tessellation ISql120TSqlSpatialIndex.Tessellation 
		{
			get { return this.Tessellation;}
		}
		Boolean ISql120TSqlSpatialIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}
		Double? ISql120TSqlSpatialIndex.XMax 
		{
			get { return this.XMax;}
		}
		Double? ISql120TSqlSpatialIndex.XMin 
		{
			get { return this.XMin;}
		}
		Double? ISql120TSqlSpatialIndex.YMax 
		{
			get { return this.YMax;}
		}
		Double? ISql120TSqlSpatialIndex.YMin 
		{
			get { return this.YMin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlSpatialIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlSpatialIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql120TSqlSpatialIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlSpatialIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlSpatialIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlStatistics"/>.
	/// </summary>
	public partial class TSqlStatistics : ISql120TSqlStatistics
	{		
		String ISql120TSqlStatistics.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISql120TSqlStatistics.Incremental 
		{
			get { return this.Incremental;}
		}
		Boolean ISql120TSqlStatistics.NoRecompute 
		{
			get { return this.NoRecompute;}
		}
		Int32 ISql120TSqlStatistics.SampleSize 
		{
			get { return this.SampleSize;}
		}
		SamplingStyle ISql120TSqlStatistics.SamplingStyle 
		{
			get { return this.SamplingStyle;}
		}
		String ISql120TSqlStatistics.StatsStream 
		{
			get { return this.StatsStream;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlStatistics.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlStatistics.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlStatistics.OnObject 
		{
			get 
			{
				return this.OnObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlParameter"/>.
	/// </summary>
	public partial class TSqlParameter : ISql120TSqlParameter
	{		
		String ISql120TSqlParameter.DefaultExpression 
		{
			get { return this.DefaultExpression;}
		}
		Boolean ISql120TSqlParameter.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql120TSqlParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		Int32 ISql120TSqlParameter.Length 
		{
			get { return this.Length;}
		}
		Int32 ISql120TSqlParameter.Precision 
		{
			get { return this.Precision;}
		}
		Boolean ISql120TSqlParameter.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		Int32 ISql120TSqlParameter.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISql120TSqlParameter.Varying 
		{
			get { return this.Varying;}
		}
		XmlStyle ISql120TSqlParameter.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql120TSqlParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference> ISql120TSqlParameter.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSymmetricKey"/>.
	/// </summary>
	public partial class TSqlSymmetricKey : ISql120TSqlSymmetricKey
	{		
		SymmetricKeyAlgorithm ISql120TSqlSymmetricKey.Algorithm 
		{
			get { return this.Algorithm;}
		}
		SymmetricKeyCreationDisposition ISql120TSqlSymmetricKey.CreationDisposition 
		{
			get { return this.CreationDisposition;}
		}
		String ISql120TSqlSymmetricKey.IdentityValue 
		{
			get { return this.IdentityValue;}
		}
		String ISql120TSqlSymmetricKey.KeySource 
		{
			get { return this.KeySource;}
		}
		String ISql120TSqlSymmetricKey.ProviderKeyName 
		{
			get { return this.ProviderKeyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference> ISql120TSqlSymmetricKey.AsymmetricKeys 
		{
			get 
			{
				return this.AsymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlSymmetricKey.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlSymmetricKey.Certificates 
		{
			get 
			{
				return this.Certificates.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlSymmetricKey.Passwords 
		{
			get 
			{
				return this.Passwords;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlSymmetricKey.Provider 
		{
			get 
			{
				return this.Provider;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSymmetricKeyReference> ISql120TSqlSymmetricKey.SymmetricKeys 
		{
			get 
			{
				return this.SymmetricKeys.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSymmetricKeyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSymmetricKeyPassword"/>.
	/// </summary>
	public partial class TSqlSymmetricKeyPassword : ISql120TSqlSymmetricKeyPassword
	{		
		String ISql120TSqlSymmetricKeyPassword.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSynonym"/>.
	/// </summary>
	public partial class TSqlSynonym : ISql120TSqlSynonym
	{		
		String ISql120TSqlSynonym.ForObjectName 
		{
			get { return this.ForObjectName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlSynonym.ForObject 
		{
			get 
			{
				return this.ForObject;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlSynonym.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTable"/>.
	/// </summary>
	public partial class TSqlTable : ISql120TSqlTable
	{		
		Boolean? ISql120TSqlTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISql120TSqlTable.ChangeDataCaptureEnabled 
		{
			get { return this.ChangeDataCaptureEnabled;}
		}
		Boolean ISql120TSqlTable.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int64? ISql120TSqlTable.DataPages 
		{
			get { return this.DataPages;}
		}
		Double? ISql120TSqlTable.DataSize 
		{
			get { return this.DataSize;}
		}
		Durability ISql120TSqlTable.Durability 
		{
			get { return this.Durability;}
		}
		Boolean? ISql120TSqlTable.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Double? ISql120TSqlTable.IndexSize 
		{
			get { return this.IndexSize;}
		}
		Boolean ISql120TSqlTable.IsReplicated 
		{
			get { return this.IsReplicated;}
		}
		Boolean ISql120TSqlTable.LargeValueTypesOutOfRow 
		{
			get { return this.LargeValueTypesOutOfRow;}
		}
		LockEscalationMethod ISql120TSqlTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean ISql120TSqlTable.MemoryOptimized 
		{
			get { return this.MemoryOptimized;}
		}
		Boolean? ISql120TSqlTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Int64? ISql120TSqlTable.RowCount 
		{
			get { return this.RowCount;}
		}
		Boolean ISql120TSqlTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}
		Int32 ISql120TSqlTable.TextInRowSize 
		{
			get { return this.TextInRowSize;}
		}
		Boolean ISql120TSqlTable.TrackColumnsUpdated 
		{
			get { return this.TrackColumnsUpdated;}
		}
		Int64? ISql120TSqlTable.UsedPages 
		{
			get { return this.UsedPages;}
		}
		Boolean ISql120TSqlTable.VardecimalStorageFormatEnabled 
		{
			get { return this.VardecimalStorageFormatEnabled;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn> ISql120TSqlTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption> ISql120TSqlTable.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlTable.FilegroupForTextImage 
		{
			get 
			{
				return this.FilegroupForTextImage.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlTable.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlTable.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlFileTable"/>.
	/// </summary>
	public partial class TSqlFileTable : ISql120TSqlFileTable
	{		
		Boolean? ISql120TSqlFileTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISql120TSqlFileTable.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		String ISql120TSqlFileTable.FileTableCollateFilename 
		{
			get { return this.FileTableCollateFilename;}
		}
		String ISql120TSqlFileTable.FileTableDirectory 
		{
			get { return this.FileTableDirectory;}
		}
		Boolean ISql120TSqlFileTable.FileTableNamespaceEnabled 
		{
			get { return this.FileTableNamespaceEnabled;}
		}
		LockEscalationMethod ISql120TSqlFileTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean? ISql120TSqlFileTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql120TSqlFileTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn> ISql120TSqlFileTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption> ISql120TSqlFileTable.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlFileTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlFileTable.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlFileTable.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlFileTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlFileTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlFileTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableType"/>.
	/// </summary>
	public partial class TSqlTableType : ISql120TSqlTableType
	{		
		Boolean ISql120TSqlTableType.MemoryOptimized 
		{
			get { return this.MemoryOptimized;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableTypeColumn> ISql120TSqlTableType.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableTypeColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint> ISql120TSqlTableType.Constraints 
		{
			get 
			{
				return this.Constraints;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeIndex> ISql120TSqlTableType.Indexes 
		{
			get 
			{
				return this.Indexes;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlTableType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableTypeCheckConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeCheckConstraint : ISql120TSqlTableTypeCheckConstraint
	{		
		String ISql120TSqlTableTypeCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlTableTypeCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableTypeColumn"/>.
	/// </summary>
	public partial class TSqlTableTypeColumn : ISql120TSqlTableTypeColumn
	{		
		String ISql120TSqlTableTypeColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISql120TSqlTableTypeColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISql120TSqlTableTypeColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISql120TSqlTableTypeColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISql120TSqlTableTypeColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISql120TSqlTableTypeColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql120TSqlTableTypeColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISql120TSqlTableTypeColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISql120TSqlTableTypeColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISql120TSqlTableTypeColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISql120TSqlTableTypeColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISql120TSqlTableTypeColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql120TSqlTableTypeColumn.Scale 
		{
			get { return this.Scale;}
		}
		XmlStyle ISql120TSqlTableTypeColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISql120TSqlTableTypeColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlTableTypeColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference> ISql120TSqlTableTypeColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableTypeDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeDefaultConstraint : ISql120TSqlTableTypeDefaultConstraint
	{		
		String ISql120TSqlTableTypeDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlTableTypeDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlTableTypeDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableTypeIndex"/>.
	/// </summary>
	public partial class TSqlTableTypeIndex : ISql120TSqlTableTypeIndex
	{		
		Int32? ISql120TSqlTableTypeIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql120TSqlTableTypeIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql120TSqlTableTypeIndex.IsDisabled 
		{
			get { return this.IsDisabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlTableTypeIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableTypePrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypePrimaryKeyConstraint : ISql120TSqlTableTypePrimaryKeyConstraint
	{		
		Int32? ISql120TSqlTableTypePrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISql120TSqlTableTypePrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql120TSqlTableTypePrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISql120TSqlTableTypePrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlTableTypePrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTableTypeUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeUniqueConstraint : ISql120TSqlTableTypeUniqueConstraint
	{		
		Boolean ISql120TSqlTableTypeUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql120TSqlTableTypeUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlTableTypeUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlTcpProtocolSpecifier"/>.
	/// </summary>
	public partial class TSqlTcpProtocolSpecifier : ISql120TSqlTcpProtocolSpecifier
	{		
		String ISql120TSqlTcpProtocolSpecifier.ListenerIPv4 
		{
			get { return this.ListenerIPv4;}
		}
		String ISql120TSqlTcpProtocolSpecifier.ListenerIPv6 
		{
			get { return this.ListenerIPv6;}
		}
		Int32 ISql120TSqlTcpProtocolSpecifier.ListenerPort 
		{
			get { return this.ListenerPort;}
		}
		Boolean ISql120TSqlTcpProtocolSpecifier.ListeningOnAllIPs 
		{
			get { return this.ListeningOnAllIPs;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlUniqueConstraint : ISql120TSqlUniqueConstraint
	{		
		Boolean ISql120TSqlUniqueConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql120TSqlUniqueConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql120TSqlUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISql120TSqlUniqueConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean? ISql120TSqlUniqueConstraint.FileStreamNull 
		{
			get { return this.FileStreamNull;}
		}
		Int32? ISql120TSqlUniqueConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql120TSqlUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql120TSqlUniqueConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql120TSqlUniqueConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption> ISql120TSqlUniqueConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlUniqueConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference> ISql120TSqlUniqueConstraint.FileStreamFilegroup 
		{
			get 
			{
				return this.FileStreamFilegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlUniqueConstraint.FileStreamPartitionScheme 
		{
			get 
			{
				return this.FileStreamPartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference> ISql120TSqlUniqueConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlUniqueConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference> ISql120TSqlUniqueConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlUser"/>.
	/// </summary>
	public partial class TSqlUser : ISql120TSqlUser
	{		
		AuthenticationType ISql120TSqlUser.AuthenticationType 
		{
			get { return this.AuthenticationType;}
		}
		String ISql120TSqlUser.DefaultLanguage 
		{
			get { return this.DefaultLanguage;}
		}
		String ISql120TSqlUser.Password 
		{
			get { return this.Password;}
		}
		String ISql120TSqlUser.Sid 
		{
			get { return this.Sid;}
		}
		Boolean ISql120TSqlUser.WithoutLogin 
		{
			get { return this.WithoutLogin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference> ISql120TSqlUser.AsymmetricKey 
		{
			get 
			{
				return this.AsymmetricKey.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAsymmetricKeyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference> ISql120TSqlUser.Certificate 
		{
			get 
			{
				return this.Certificate.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlCertificateReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlUser.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference> ISql120TSqlUser.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlUserDefinedServerRole"/>.
	/// </summary>
	public partial class TSqlUserDefinedServerRole : ISql120TSqlUserDefinedServerRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISql120TSqlUserDefinedServerRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlUserDefinedType"/>.
	/// </summary>
	public partial class TSqlUserDefinedType : ISql120TSqlUserDefinedType
	{		
		Boolean? ISql120TSqlUserDefinedType.ByteOrdered 
		{
			get { return this.ByteOrdered;}
		}
		String ISql120TSqlUserDefinedType.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean? ISql120TSqlUserDefinedType.FixedLength 
		{
			get { return this.FixedLength;}
		}
		Format ISql120TSqlUserDefinedType.Format 
		{
			get { return this.Format;}
		}
		Int32? ISql120TSqlUserDefinedType.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		String ISql120TSqlUserDefinedType.ValidationMethodName 
		{
			get { return this.ValidationMethodName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference> ISql120TSqlUserDefinedType.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlUserDefinedType.Methods 
		{
			get 
			{
				return this.Methods;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISql120TSqlUserDefinedType.Properties 
		{
			get 
			{
				return this.Properties;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlUserDefinedType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlView"/>.
	/// </summary>
	public partial class TSqlView : ISql120TSqlView
	{		
		Boolean? ISql120TSqlView.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISql120TSqlView.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISql120TSqlView.Replicated 
		{
			get { return this.Replicated;}
		}
		String ISql120TSqlView.SelectStatement 
		{
			get { return this.SelectStatement;}
		}
		Boolean ISql120TSqlView.WithCheckOption 
		{
			get { return this.WithCheckOption;}
		}
		Boolean ISql120TSqlView.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISql120TSqlView.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}
		Boolean ISql120TSqlView.WithViewMetadata 
		{
			get { return this.WithViewMetadata;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISql120TSqlView.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn> ISql120TSqlView.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlView.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlWorkloadGroup"/>.
	/// </summary>
	public partial class TSqlWorkloadGroup : ISql120TSqlWorkloadGroup
	{		
		Int32 ISql120TSqlWorkloadGroup.GroupMaxRequests 
		{
			get { return this.GroupMaxRequests;}
		}
		Degree ISql120TSqlWorkloadGroup.Importance 
		{
			get { return this.Importance;}
		}
		Int32 ISql120TSqlWorkloadGroup.MaxDop 
		{
			get { return this.MaxDop;}
		}
		Int32 ISql120TSqlWorkloadGroup.RequestMaxCpuTimeSec 
		{
			get { return this.RequestMaxCpuTimeSec;}
		}
		Int32 ISql120TSqlWorkloadGroup.RequestMaxMemoryGrantPercent 
		{
			get { return this.RequestMaxMemoryGrantPercent;}
		}
		Int32 ISql120TSqlWorkloadGroup.RequestMemoryGrantTimeoutSec 
		{
			get { return this.RequestMemoryGrantTimeoutSec;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlResourcePoolReference> ISql120TSqlWorkloadGroup.ResourcePool 
		{
			get 
			{
				return this.ResourcePool.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlResourcePoolReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlXmlIndex"/>.
	/// </summary>
	public partial class TSqlXmlIndex : ISql120TSqlXmlIndex
	{		
		Boolean ISql120TSqlXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql120TSqlXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql120TSqlXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql120TSqlXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql120TSqlXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql120TSqlXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISql120TSqlXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		SecondaryXmlIndexType ISql120TSqlXmlIndex.SecondaryXmlIndexType 
		{
			get { return this.SecondaryXmlIndexType;}
		}
		Boolean ISql120TSqlXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql120TSqlXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlIndexReference> ISql120TSqlXmlIndex.PrimaryXmlIndex 
		{
			get 
			{
				return this.PrimaryXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlIndexReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlSelectiveXmlIndex"/>.
	/// </summary>
	public partial class TSqlSelectiveXmlIndex : ISql120TSqlSelectiveXmlIndex
	{		
		Boolean ISql120TSqlSelectiveXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISql120TSqlSelectiveXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISql120TSqlSelectiveXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISql120TSqlSelectiveXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISql120TSqlSelectiveXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISql120TSqlSelectiveXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISql120TSqlSelectiveXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISql120TSqlSelectiveXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference> ISql120TSqlSelectiveXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISql120TSqlSelectiveXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> ISql120TSqlSelectiveXmlIndex.PrimaryPromotedPath 
		{
			get 
			{
				return this.PrimaryPromotedPath;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSelectiveXmlIndexReference> ISql120TSqlSelectiveXmlIndex.PrimarySelectiveXmlIndex 
		{
			get 
			{
				return this.PrimarySelectiveXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSelectiveXmlIndexReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> ISql120TSqlSelectiveXmlIndex.PromotedPaths 
		{
			get 
			{
				return this.PromotedPaths;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlNamespace> ISql120TSqlSelectiveXmlIndex.XmlNamespaces 
		{
			get 
			{
				return this.XmlNamespaces.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlXmlNamespace>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlXmlNamespace"/>.
	/// </summary>
	public partial class TSqlXmlNamespace : ISql120TSqlXmlNamespace
	{		
		String ISql120TSqlXmlNamespace.NamespaceUri 
		{
			get { return this.NamespaceUri;}
		}
		String ISql120TSqlXmlNamespace.Prefix 
		{
			get { return this.Prefix;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPromotedNodePathForXQueryType"/>.
	/// </summary>
	public partial class TSqlPromotedNodePathForXQueryType : ISql120TSqlPromotedNodePathForXQueryType
	{		
		Boolean ISql120TSqlPromotedNodePathForXQueryType.IsSingleton 
		{
			get { return this.IsSingleton;}
		}
		Int32? ISql120TSqlPromotedNodePathForXQueryType.MaxLength 
		{
			get { return this.MaxLength;}
		}
		String ISql120TSqlPromotedNodePathForXQueryType.NodePath 
		{
			get { return this.NodePath;}
		}
		String ISql120TSqlPromotedNodePathForXQueryType.Type 
		{
			get { return this.Type;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlPromotedNodePathForSqlType"/>.
	/// </summary>
	public partial class TSqlPromotedNodePathForSqlType : ISql120TSqlPromotedNodePathForSqlType
	{		
		Boolean ISql120TSqlPromotedNodePathForSqlType.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISql120TSqlPromotedNodePathForSqlType.IsSingleton 
		{
			get { return this.IsSingleton;}
		}
		Int32 ISql120TSqlPromotedNodePathForSqlType.Length 
		{
			get { return this.Length;}
		}
		String ISql120TSqlPromotedNodePathForSqlType.NodePath 
		{
			get { return this.NodePath;}
		}
		Int32 ISql120TSqlPromotedNodePathForSqlType.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISql120TSqlPromotedNodePathForSqlType.Scale 
		{
			get { return this.Scale;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference> ISql120TSqlPromotedNodePathForSqlType.DataType 
		{
			get 
			{
				return this.DataType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISql120TSqlXmlSchemaCollection"/>.
	/// </summary>
	public partial class TSqlXmlSchemaCollection : ISql120TSqlXmlSchemaCollection
	{		
		String ISql120TSqlXmlSchemaCollection.Expression 
		{
			get { return this.Expression;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference> ISql120TSqlXmlSchemaCollection.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISql120TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlColumn"/>.
	/// </summary>
	public partial class TSqlColumn : ISqlAzureV12TSqlColumn
	{		
		String ISqlAzureV12TSqlColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISqlAzureV12TSqlColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISqlAzureV12TSqlColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISqlAzureV12TSqlColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISqlAzureV12TSqlColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISqlAzureV12TSqlColumn.IsIdentityNotForReplication 
		{
			get { return this.IsIdentityNotForReplication;}
		}
		Boolean ISqlAzureV12TSqlColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISqlAzureV12TSqlColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISqlAzureV12TSqlColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISqlAzureV12TSqlColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISqlAzureV12TSqlColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISqlAzureV12TSqlColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISqlAzureV12TSqlColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISqlAzureV12TSqlColumn.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISqlAzureV12TSqlColumn.Sparse 
		{
			get { return this.Sparse;}
		}
		XmlStyle ISqlAzureV12TSqlColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureV12TSqlColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlSchemaCollectionReference> ISqlAzureV12TSqlColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableValuedFunction"/>.
	/// </summary>
	public partial class TSqlTableValuedFunction : ISqlAzureV12TSqlTableValuedFunction
	{		
		Boolean? ISqlAzureV12TSqlTableValuedFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISqlAzureV12TSqlTableValuedFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISqlAzureV12TSqlTableValuedFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISqlAzureV12TSqlTableValuedFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureV12TSqlTableValuedFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		String ISqlAzureV12TSqlTableValuedFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureV12TSqlTableValuedFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISqlAzureV12TSqlTableValuedFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		String ISqlAzureV12TSqlTableValuedFunction.ReturnTableVariableName 
		{
			get { return this.ReturnTableVariableName;}
		}
		SystemDataAccessKind? ISqlAzureV12TSqlTableValuedFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISqlAzureV12TSqlTableValuedFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssembly> ISqlAzureV12TSqlTableValuedFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlTableValuedFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumn> ISqlAzureV12TSqlTableValuedFunction.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumn>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference> ISqlAzureV12TSqlTableValuedFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter> ISqlAzureV12TSqlTableValuedFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlTableValuedFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlTableValuedFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlClrTableOption> ISqlAzureV12TSqlTableValuedFunction.TableOption 
		{
			get 
			{
				return this.TableOption.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlClrTableOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference> ISqlAzureV12TSqlTableValuedFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlScalarFunction"/>.
	/// </summary>
	public partial class TSqlScalarFunction : ISqlAzureV12TSqlScalarFunction
	{		
		Boolean? ISqlAzureV12TSqlScalarFunction.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.CalledOnNullInput 
		{
			get { return this.CalledOnNullInput;}
		}
		String ISqlAzureV12TSqlScalarFunction.ClassName 
		{
			get { return this.ClassName;}
		}
		DataAccessKind? ISqlAzureV12TSqlScalarFunction.DataAccess 
		{
			get { return this.DataAccess;}
		}
		Boolean? ISqlAzureV12TSqlScalarFunction.Deterministic 
		{
			get { return this.Deterministic;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureV12TSqlScalarFunction.FillRowMethodName 
		{
			get { return this.FillRowMethodName;}
		}
		String ISqlAzureV12TSqlScalarFunction.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureV12TSqlScalarFunction.Precise 
		{
			get { return this.Precise;}
		}
		Boolean? ISqlAzureV12TSqlScalarFunction.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.ReturnsNullOnNullInput 
		{
			get { return this.ReturnsNullOnNullInput;}
		}
		SystemDataAccessKind? ISqlAzureV12TSqlScalarFunction.SystemDataAccess 
		{
			get { return this.SystemDataAccess;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISqlAzureV12TSqlScalarFunction.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssembly> ISqlAzureV12TSqlScalarFunction.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssembly>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlScalarFunction.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference> ISqlAzureV12TSqlScalarFunction.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter> ISqlAzureV12TSqlScalarFunction.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlScalarFunction.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlScalarFunction.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference> ISqlAzureV12TSqlScalarFunction.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlClrTableOption"/>.
	/// </summary>
	public partial class TSqlClrTableOption : ISqlAzureV12TSqlClrTableOption
	{		
		String ISqlAzureV12TSqlClrTableOption.ClassName 
		{
			get { return this.ClassName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlClrTableOption.OrderColumns 
		{
			get 
			{
				return this.OrderColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlAggregate"/>.
	/// </summary>
	public partial class TSqlAggregate : ISqlAzureV12TSqlAggregate
	{		
		String ISqlAzureV12TSqlAggregate.ClassName 
		{
			get { return this.ClassName;}
		}
		Format ISqlAzureV12TSqlAggregate.Format 
		{
			get { return this.Format;}
		}
		Boolean? ISqlAzureV12TSqlAggregate.InvariantToDuplicates 
		{
			get { return this.InvariantToDuplicates;}
		}
		Boolean? ISqlAzureV12TSqlAggregate.InvariantToNulls 
		{
			get { return this.InvariantToNulls;}
		}
		Int32? ISqlAzureV12TSqlAggregate.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		Boolean? ISqlAzureV12TSqlAggregate.NullIfEmpty 
		{
			get { return this.NullIfEmpty;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference> ISqlAzureV12TSqlAggregate.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter> ISqlAzureV12TSqlAggregate.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlAggregate.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlAggregate.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlApplicationRole"/>.
	/// </summary>
	public partial class TSqlApplicationRole : ISqlAzureV12TSqlApplicationRole
	{		
		String ISqlAzureV12TSqlApplicationRole.Password 
		{
			get { return this.Password;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlApplicationRole.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlIndex"/>.
	/// </summary>
	public partial class TSqlIndex : ISqlAzureV12TSqlIndex
	{		
		Boolean ISqlAzureV12TSqlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISqlAzureV12TSqlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISqlAzureV12TSqlIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureV12TSqlIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureV12TSqlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISqlAzureV12TSqlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		String ISqlAzureV12TSqlIndex.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISqlAzureV12TSqlIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureV12TSqlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureV12TSqlIndex.IncrementalStatistics 
		{
			get { return this.IncrementalStatistics;}
		}
		Boolean ISqlAzureV12TSqlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISqlAzureV12TSqlIndex.Unique 
		{
			get { return this.Unique;}
		}
		Boolean ISqlAzureV12TSqlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlIndex.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption> ISqlAzureV12TSqlIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlIndex.IncludedColumns 
		{
			get 
			{
				return this.IncludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureV12TSqlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference> ISqlAzureV12TSqlIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlAssembly"/>.
	/// </summary>
	public partial class TSqlAssembly : ISqlAzureV12TSqlAssembly
	{		
		AssemblyPermissionSet ISqlAzureV12TSqlAssembly.PermissionSet 
		{
			get { return this.PermissionSet;}
		}
		Boolean ISqlAzureV12TSqlAssembly.Visible 
		{
			get { return this.Visible;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblySource> ISqlAzureV12TSqlAssembly.AssemblySources 
		{
			get 
			{
				return this.AssemblySources.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblySource>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureV12TSqlAssembly.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference> ISqlAzureV12TSqlAssembly.ReferencedAssemblies 
		{
			get 
			{
				return this.ReferencedAssemblies.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlAssemblySource"/>.
	/// </summary>
	public partial class TSqlAssemblySource : ISqlAzureV12TSqlAssemblySource
	{		
		String ISqlAzureV12TSqlAssemblySource.Source 
		{
			get { return this.Source;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlBuiltInServerRole"/>.
	/// </summary>
	public partial class TSqlBuiltInServerRole : ISqlAzureV12TSqlBuiltInServerRole
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDataType"/>.
	/// </summary>
	public partial class TSqlDataType : ISqlAzureV12TSqlDataType
	{		
		SqlDataType ISqlAzureV12TSqlDataType.SqlDataType 
		{
			get { return this.SqlDataType;}
		}
		Boolean ISqlAzureV12TSqlDataType.UddtIsMax 
		{
			get { return this.UddtIsMax;}
		}
		Int32 ISqlAzureV12TSqlDataType.UddtLength 
		{
			get { return this.UddtLength;}
		}
		Boolean ISqlAzureV12TSqlDataType.UddtNullable 
		{
			get { return this.UddtNullable;}
		}
		Int32 ISqlAzureV12TSqlDataType.UddtPrecision 
		{
			get { return this.UddtPrecision;}
		}
		Int32 ISqlAzureV12TSqlDataType.UddtScale 
		{
			get { return this.UddtScale;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlDataType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlDataType.Type 
		{
			get 
			{
				return this.Type.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlCheckConstraint"/>.
	/// </summary>
	public partial class TSqlCheckConstraint : ISqlAzureV12TSqlCheckConstraint
	{		
		Boolean ISqlAzureV12TSqlCheckConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureV12TSqlCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISqlAzureV12TSqlCheckConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference> ISqlAzureV12TSqlCheckConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlClrTypeMethod"/>.
	/// </summary>
	public partial class TSqlClrTypeMethod : ISqlAzureV12TSqlClrTypeMethod
	{		
		String ISqlAzureV12TSqlClrTypeMethod.MethodName 
		{
			get { return this.MethodName;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter> ISqlAzureV12TSqlClrTypeMethod.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataType> ISqlAzureV12TSqlClrTypeMethod.ReturnType 
		{
			get 
			{
				return this.ReturnType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataType>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlClrTypeMethodParameter"/>.
	/// </summary>
	public partial class TSqlClrTypeMethodParameter : ISqlAzureV12TSqlClrTypeMethodParameter
	{		
		Boolean ISqlAzureV12TSqlClrTypeMethodParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		String ISqlAzureV12TSqlClrTypeMethodParameter.ParameterName 
		{
			get { return this.ParameterName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureV12TSqlClrTypeMethodParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlClrTypeProperty"/>.
	/// </summary>
	public partial class TSqlClrTypeProperty : ISqlAzureV12TSqlClrTypeProperty
	{		
		String ISqlAzureV12TSqlClrTypeProperty.PropertyName 
		{
			get { return this.PropertyName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlClrTypeProperty.ClrType 
		{
			get 
			{
				return this.ClrType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlColumnStoreIndex"/>.
	/// </summary>
	public partial class TSqlColumnStoreIndex : ISqlAzureV12TSqlColumnStoreIndex
	{		
		Boolean ISqlAzureV12TSqlColumnStoreIndex.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureV12TSqlColumnStoreIndex.Disabled 
		{
			get { return this.Disabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlColumnStoreIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption> ISqlAzureV12TSqlColumnStoreIndex.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlColumnStoreIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureV12TSqlColumnStoreIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlColumnStoreIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference> ISqlAzureV12TSqlColumnStoreIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDatabaseDdlTrigger"/>.
	/// </summary>
	public partial class TSqlDatabaseDdlTrigger : ISqlAzureV12TSqlDatabaseDdlTrigger
	{		
		Boolean? ISqlAzureV12TSqlDatabaseDdlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISqlAzureV12TSqlDatabaseDdlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISqlAzureV12TSqlDatabaseDdlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureV12TSqlDatabaseDdlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureV12TSqlDatabaseDdlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureV12TSqlDatabaseDdlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureV12TSqlDatabaseDdlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureV12TSqlDatabaseDdlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISqlAzureV12TSqlDatabaseDdlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		Boolean ISqlAzureV12TSqlDatabaseDdlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference> ISqlAzureV12TSqlDatabaseDdlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlDatabaseDdlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlEventGroup> ISqlAzureV12TSqlDatabaseDdlTrigger.EventGroup 
		{
			get 
			{
				return this.EventGroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlEventGroup>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Model.EventType> ISqlAzureV12TSqlDatabaseDdlTrigger.EventType 
		{
			get 
			{
				return this.EventType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference> ISqlAzureV12TSqlDatabaseDdlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference> ISqlAzureV12TSqlDatabaseDdlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDatabaseOptions"/>.
	/// </summary>
	public partial class TSqlDatabaseOptions : ISqlAzureV12TSqlDatabaseOptions
	{		
		Boolean ISqlAzureV12TSqlDatabaseOptions.AllowSnapshotIsolation 
		{
			get { return this.AllowSnapshotIsolation;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AnsiNullDefaultOn 
		{
			get { return this.AnsiNullDefaultOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AnsiPaddingOn 
		{
			get { return this.AnsiPaddingOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AnsiWarningsOn 
		{
			get { return this.AnsiWarningsOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.ArithAbortOn 
		{
			get { return this.ArithAbortOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AutoCreateStatistics 
		{
			get { return this.AutoCreateStatistics;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AutoCreateStatisticsIncremental 
		{
			get { return this.AutoCreateStatisticsIncremental;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AutoShrink 
		{
			get { return this.AutoShrink;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AutoUpdateStatistics 
		{
			get { return this.AutoUpdateStatistics;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.AutoUpdateStatisticsAsync 
		{
			get { return this.AutoUpdateStatisticsAsync;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.ChangeTrackingAutoCleanup 
		{
			get { return this.ChangeTrackingAutoCleanup;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int32 ISqlAzureV12TSqlDatabaseOptions.ChangeTrackingRetentionPeriod 
		{
			get { return this.ChangeTrackingRetentionPeriod;}
		}
		TimeUnit ISqlAzureV12TSqlDatabaseOptions.ChangeTrackingRetentionUnit 
		{
			get { return this.ChangeTrackingRetentionUnit;}
		}
		String ISqlAzureV12TSqlDatabaseOptions.Collation 
		{
			get { return this.Collation;}
		}
		Int32 ISqlAzureV12TSqlDatabaseOptions.CompatibilityLevel 
		{
			get { return this.CompatibilityLevel;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.ConcatNullYieldsNull 
		{
			get { return this.ConcatNullYieldsNull;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.CursorCloseOnCommit 
		{
			get { return this.CursorCloseOnCommit;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.DateCorrelationOptimizationOn 
		{
			get { return this.DateCorrelationOptimizationOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.FullTextEnabled 
		{
			get { return this.FullTextEnabled;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.NumericRoundAbortOn 
		{
			get { return this.NumericRoundAbortOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.RecursiveTriggersOn 
		{
			get { return this.RecursiveTriggersOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.VardecimalStorageFormatOn 
		{
			get { return this.VardecimalStorageFormatOn;}
		}
		Boolean ISqlAzureV12TSqlDatabaseOptions.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDataCompressionOption"/>.
	/// </summary>
	public partial class TSqlDataCompressionOption : ISqlAzureV12TSqlDataCompressionOption
	{		
		CompressionLevel ISqlAzureV12TSqlDataCompressionOption.CompressionLevel 
		{
			get { return this.CompressionLevel;}
		}
		Int32 ISqlAzureV12TSqlDataCompressionOption.PartitionNumber 
		{
			get { return this.PartitionNumber;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDefault"/>.
	/// </summary>
	public partial class TSqlDefault : ISqlAzureV12TSqlDefault
	{		
		String ISqlAzureV12TSqlDefault.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlDefault.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlDefault.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlDefaultConstraint : ISqlAzureV12TSqlDefaultConstraint
	{		
		Boolean ISqlAzureV12TSqlDefaultConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureV12TSqlDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}
		Boolean ISqlAzureV12TSqlDefaultConstraint.WithValues 
		{
			get { return this.WithValues;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference> ISqlAzureV12TSqlDefaultConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlDmlTrigger"/>.
	/// </summary>
	public partial class TSqlDmlTrigger : ISqlAzureV12TSqlDmlTrigger
	{		
		Boolean? ISqlAzureV12TSqlDmlTrigger.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISqlAzureV12TSqlDmlTrigger.ClassName 
		{
			get { return this.ClassName;}
		}
		OrderRestriction ISqlAzureV12TSqlDmlTrigger.DeleteOrderRestriction 
		{
			get { return this.DeleteOrderRestriction;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		OrderRestriction ISqlAzureV12TSqlDmlTrigger.InsertOrderRestriction 
		{
			get { return this.InsertOrderRestriction;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.IsDeleteTrigger 
		{
			get { return this.IsDeleteTrigger;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.IsInsertTrigger 
		{
			get { return this.IsInsertTrigger;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.IsUpdateTrigger 
		{
			get { return this.IsUpdateTrigger;}
		}
		String ISqlAzureV12TSqlDmlTrigger.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		Boolean? ISqlAzureV12TSqlDmlTrigger.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		TriggerType ISqlAzureV12TSqlDmlTrigger.TriggerType 
		{
			get { return this.TriggerType;}
		}
		OrderRestriction ISqlAzureV12TSqlDmlTrigger.UpdateOrderRestriction 
		{
			get { return this.UpdateOrderRestriction;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.WithAppend 
		{
			get { return this.WithAppend;}
		}
		Boolean ISqlAzureV12TSqlDmlTrigger.WithEncryption 
		{
			get { return this.WithEncryption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference> ISqlAzureV12TSqlDmlTrigger.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlDmlTrigger.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference> ISqlAzureV12TSqlDmlTrigger.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlDmlTrigger.TriggerObject 
		{
			get 
			{
				return this.TriggerObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference> ISqlAzureV12TSqlDmlTrigger.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlEventGroup"/>.
	/// </summary>
	public partial class TSqlEventGroup : ISqlAzureV12TSqlEventGroup
	{		
		EventGroupType ISqlAzureV12TSqlEventGroup.Group 
		{
			get { return this.Group;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlEventTypeSpecifier"/>.
	/// </summary>
	public partial class TSqlEventTypeSpecifier : ISqlAzureV12TSqlEventTypeSpecifier
	{		
		EventType ISqlAzureV12TSqlEventTypeSpecifier.EventType 
		{
			get { return this.EventType;}
		}
		OrderRestriction ISqlAzureV12TSqlEventTypeSpecifier.Order 
		{
			get { return this.Order;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlExtendedProperty"/>.
	/// </summary>
	public partial class TSqlExtendedProperty : ISqlAzureV12TSqlExtendedProperty
	{		
		String ISqlAzureV12TSqlExtendedProperty.Value 
		{
			get { return this.Value;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IExtendedPropertyHost> ISqlAzureV12TSqlExtendedProperty.Host 
		{
			get 
			{
				return this.Host;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlForeignKeyConstraint"/>.
	/// </summary>
	public partial class TSqlForeignKeyConstraint : ISqlAzureV12TSqlForeignKeyConstraint
	{		
		ForeignKeyAction ISqlAzureV12TSqlForeignKeyConstraint.DeleteAction 
		{
			get { return this.DeleteAction;}
		}
		Boolean ISqlAzureV12TSqlForeignKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureV12TSqlForeignKeyConstraint.NotForReplication 
		{
			get { return this.NotForReplication;}
		}
		ForeignKeyAction ISqlAzureV12TSqlForeignKeyConstraint.UpdateAction 
		{
			get { return this.UpdateAction;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlForeignKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlForeignKeyConstraint.ForeignColumns 
		{
			get 
			{
				return this.ForeignColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference> ISqlAzureV12TSqlForeignKeyConstraint.ForeignTable 
		{
			get 
			{
				return this.ForeignTable.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference> ISqlAzureV12TSqlForeignKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlFullTextCatalog"/>.
	/// </summary>
	public partial class TSqlFullTextCatalog : ISqlAzureV12TSqlFullTextCatalog
	{		
		Boolean? ISqlAzureV12TSqlFullTextCatalog.AccentSensitivity 
		{
			get { return this.AccentSensitivity;}
		}
		Boolean ISqlAzureV12TSqlFullTextCatalog.IsDefault 
		{
			get { return this.IsDefault;}
		}
		String ISqlAzureV12TSqlFullTextCatalog.Path 
		{
			get { return this.Path;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureV12TSqlFullTextCatalog.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlFullTextCatalog.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlFullTextIndex"/>.
	/// </summary>
	public partial class TSqlFullTextIndex : ISqlAzureV12TSqlFullTextIndex
	{		
		ChangeTrackingOption ISqlAzureV12TSqlFullTextIndex.ChangeTracking 
		{
			get { return this.ChangeTracking;}
		}
		Boolean ISqlAzureV12TSqlFullTextIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Boolean ISqlAzureV12TSqlFullTextIndex.StopListOff 
		{
			get { return this.StopListOff;}
		}
		Boolean ISqlAzureV12TSqlFullTextIndex.UseSystemStopList 
		{
			get { return this.UseSystemStopList;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElementReference> ISqlAzureV12TSqlFullTextIndex.Catalog 
		{
			get 
			{
				return this.Catalog;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFullTextIndexColumnSpecifier> ISqlAzureV12TSqlFullTextIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFullTextIndexColumnSpecifier>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlFullTextIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureV12TSqlFullTextIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlFullTextIndex.StopList 
		{
			get 
			{
				return this.StopList;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlFullTextIndex.UniqueIndexName 
		{
			get 
			{
				return this.UniqueIndexName;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlFullTextIndexColumnSpecifier"/>.
	/// </summary>
	public partial class TSqlFullTextIndexColumnSpecifier : ISqlAzureV12TSqlFullTextIndexColumnSpecifier
	{		
		Int32? ISqlAzureV12TSqlFullTextIndexColumnSpecifier.LanguageId 
		{
			get { return this.LanguageId;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlFullTextIndexColumnSpecifier.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlFullTextIndexColumnSpecifier.TypeColumn 
		{
			get 
			{
				return this.TypeColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlFullTextStopList"/>.
	/// </summary>
	public partial class TSqlFullTextStopList : ISqlAzureV12TSqlFullTextStopList
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureV12TSqlFullTextStopList.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlLogin"/>.
	/// </summary>
	public partial class TSqlLogin : ISqlAzureV12TSqlLogin
	{		
		Boolean ISqlAzureV12TSqlLogin.Disabled 
		{
			get { return this.Disabled;}
		}
		String ISqlAzureV12TSqlLogin.Password 
		{
			get { return this.Password;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPartitionFunction"/>.
	/// </summary>
	public partial class TSqlPartitionFunction : ISqlAzureV12TSqlPartitionFunction
	{		
		PartitionRange ISqlAzureV12TSqlPartitionFunction.Range 
		{
			get { return this.Range;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionValue> ISqlAzureV12TSqlPartitionFunction.BoundaryValues 
		{
			get 
			{
				return this.BoundaryValues.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionValue>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlPartitionFunction.ParameterType 
		{
			get 
			{
				return this.ParameterType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPartitionScheme"/>.
	/// </summary>
	public partial class TSqlPartitionScheme : ISqlAzureV12TSqlPartitionScheme
	{		
		Boolean ISqlAzureV12TSqlPartitionScheme.AllToOneFilegroup 
		{
			get { return this.AllToOneFilegroup;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlPartitionScheme.Filegroups 
		{
			get 
			{
				return this.Filegroups.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionFunctionReference> ISqlAzureV12TSqlPartitionScheme.PartitionFunction 
		{
			get 
			{
				return this.PartitionFunction.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionFunctionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPartitionValue"/>.
	/// </summary>
	public partial class TSqlPartitionValue : ISqlAzureV12TSqlPartitionValue
	{		
		String ISqlAzureV12TSqlPartitionValue.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlPartitionValue.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPermission"/>.
	/// </summary>
	public partial class TSqlPermission : ISqlAzureV12TSqlPermission
	{		
		PermissionAction ISqlAzureV12TSqlPermission.PermissionAction 
		{
			get { return this.PermissionAction;}
		}
		PermissionType ISqlAzureV12TSqlPermission.PermissionType 
		{
			get { return this.PermissionType;}
		}
		Boolean ISqlAzureV12TSqlPermission.WithAllPrivileges 
		{
			get { return this.WithAllPrivileges;}
		}
		Boolean ISqlAzureV12TSqlPermission.WithGrantOption 
		{
			get { return this.WithGrantOption;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlPermission.ExcludedColumns 
		{
			get 
			{
				return this.ExcludedColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISqlAzureV12TSqlPermission.Grantee 
		{
			get 
			{
				return this.Grantee;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurityPrincipal> ISqlAzureV12TSqlPermission.Grantor 
		{
			get 
			{
				return this.Grantor;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlPermission.RevokedGrantOptionColumns 
		{
			get 
			{
				return this.RevokedGrantOptionColumns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlSecurable> ISqlAzureV12TSqlPermission.SecuredObject 
		{
			get 
			{
				return this.SecuredObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlPrimaryKeyConstraint : ISqlAzureV12TSqlPrimaryKeyConstraint
	{		
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISqlAzureV12TSqlPrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISqlAzureV12TSqlPrimaryKeyConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISqlAzureV12TSqlPrimaryKeyConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlPrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption> ISqlAzureV12TSqlPrimaryKeyConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlPrimaryKeyConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference> ISqlAzureV12TSqlPrimaryKeyConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlPrimaryKeyConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference> ISqlAzureV12TSqlPrimaryKeyConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlProcedure"/>.
	/// </summary>
	public partial class TSqlProcedure : ISqlAzureV12TSqlProcedure
	{		
		Boolean? ISqlAzureV12TSqlProcedure.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		String ISqlAzureV12TSqlProcedure.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean ISqlAzureV12TSqlProcedure.ExecuteAsCaller 
		{
			get { return this.ExecuteAsCaller;}
		}
		Boolean ISqlAzureV12TSqlProcedure.ExecuteAsOwner 
		{
			get { return this.ExecuteAsOwner;}
		}
		Boolean ISqlAzureV12TSqlProcedure.ExecuteAsSelf 
		{
			get { return this.ExecuteAsSelf;}
		}
		String ISqlAzureV12TSqlProcedure.MethodName 
		{
			get { return this.MethodName;}
		}
		Boolean? ISqlAzureV12TSqlProcedure.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Boolean ISqlAzureV12TSqlProcedure.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISqlAzureV12TSqlProcedure.WithRecompile 
		{
			get { return this.WithRecompile;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference> ISqlAzureV12TSqlProcedure.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlProcedure.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference> ISqlAzureV12TSqlProcedure.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter> ISqlAzureV12TSqlProcedure.Parameters 
		{
			get 
			{
				return this.Parameters.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlParameter>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlProcedureReference> ISqlAzureV12TSqlProcedure.ParentProcedure 
		{
			get 
			{
				return this.ParentProcedure.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlProcedureReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlProcedure.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference> ISqlAzureV12TSqlProcedure.User 
		{
			get 
			{
				return this.User.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlUserReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlRole"/>.
	/// </summary>
	public partial class TSqlRole : ISqlAzureV12TSqlRole
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureV12TSqlRole.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlRoleMembership"/>.
	/// </summary>
	public partial class TSqlRoleMembership : ISqlAzureV12TSqlRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDatabaseSecurityPrincipal> ISqlAzureV12TSqlRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlRoleReference> ISqlAzureV12TSqlRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlRule"/>.
	/// </summary>
	public partial class TSqlRule : ISqlAzureV12TSqlRule
	{		
		String ISqlAzureV12TSqlRule.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlRule.BoundObjects 
		{
			get 
			{
				return this.BoundObjects;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlRule.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlSchema"/>.
	/// </summary>
	public partial class TSqlSchema : ISqlAzureV12TSqlSchema
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlObjectAuthorizer> ISqlAzureV12TSqlSchema.Authorizer 
		{
			get 
			{
				return this.Authorizer;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlSequence"/>.
	/// </summary>
	public partial class TSqlSequence : ISqlAzureV12TSqlSequence
	{		
		Int32? ISqlAzureV12TSqlSequence.CacheSize 
		{
			get { return this.CacheSize;}
		}
		String ISqlAzureV12TSqlSequence.IncrementValue 
		{
			get { return this.IncrementValue;}
		}
		Boolean ISqlAzureV12TSqlSequence.IsCached 
		{
			get { return this.IsCached;}
		}
		Boolean ISqlAzureV12TSqlSequence.IsCycling 
		{
			get { return this.IsCycling;}
		}
		String ISqlAzureV12TSqlSequence.MaxValue 
		{
			get { return this.MaxValue;}
		}
		String ISqlAzureV12TSqlSequence.MinValue 
		{
			get { return this.MinValue;}
		}
		Boolean ISqlAzureV12TSqlSequence.NoMaxValue 
		{
			get { return this.NoMaxValue;}
		}
		Boolean ISqlAzureV12TSqlSequence.NoMinValue 
		{
			get { return this.NoMinValue;}
		}
		String ISqlAzureV12TSqlSequence.StartValue 
		{
			get { return this.StartValue;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlSequence.DataType 
		{
			get 
			{
				return this.DataType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlSequence.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlServerOptions"/>.
	/// </summary>
	public partial class TSqlServerOptions : ISqlAzureV12TSqlServerOptions
	{		
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlServerRoleMembership"/>.
	/// </summary>
	public partial class TSqlServerRoleMembership : ISqlAzureV12TSqlServerRoleMembership
	{		

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.IServerSecurityPrincipal> ISqlAzureV12TSqlServerRoleMembership.Member 
		{
			get 
			{
				return this.Member;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlRoleReference> ISqlAzureV12TSqlServerRoleMembership.Role 
		{
			get 
			{
				return this.Role.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlRoleReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlSpatialIndex"/>.
	/// </summary>
	public partial class TSqlSpatialIndex : ISqlAzureV12TSqlSpatialIndex
	{		
		Boolean ISqlAzureV12TSqlSpatialIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISqlAzureV12TSqlSpatialIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Int32? ISqlAzureV12TSqlSpatialIndex.CellsPerObject 
		{
			get { return this.CellsPerObject;}
		}
		CompressionLevel ISqlAzureV12TSqlSpatialIndex.DataCompression 
		{
			get { return this.DataCompression;}
		}
		Boolean ISqlAzureV12TSqlSpatialIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32 ISqlAzureV12TSqlSpatialIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Degree ISqlAzureV12TSqlSpatialIndex.GridLevel1Density 
		{
			get { return this.GridLevel1Density;}
		}
		Degree ISqlAzureV12TSqlSpatialIndex.GridLevel2Density 
		{
			get { return this.GridLevel2Density;}
		}
		Degree ISqlAzureV12TSqlSpatialIndex.GridLevel3Density 
		{
			get { return this.GridLevel3Density;}
		}
		Degree ISqlAzureV12TSqlSpatialIndex.GridLevel4Density 
		{
			get { return this.GridLevel4Density;}
		}
		Boolean ISqlAzureV12TSqlSpatialIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureV12TSqlSpatialIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Tessellation ISqlAzureV12TSqlSpatialIndex.Tessellation 
		{
			get { return this.Tessellation;}
		}
		Boolean ISqlAzureV12TSqlSpatialIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}
		Double? ISqlAzureV12TSqlSpatialIndex.XMax 
		{
			get { return this.XMax;}
		}
		Double? ISqlAzureV12TSqlSpatialIndex.XMin 
		{
			get { return this.XMin;}
		}
		Double? ISqlAzureV12TSqlSpatialIndex.YMax 
		{
			get { return this.YMax;}
		}
		Double? ISqlAzureV12TSqlSpatialIndex.YMin 
		{
			get { return this.YMin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlSpatialIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlSpatialIndex.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureV12TSqlSpatialIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlSpatialIndex.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference> ISqlAzureV12TSqlSpatialIndex.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlStatistics"/>.
	/// </summary>
	public partial class TSqlStatistics : ISqlAzureV12TSqlStatistics
	{		
		String ISqlAzureV12TSqlStatistics.FilterPredicate 
		{
			get { return this.FilterPredicate;}
		}
		Boolean ISqlAzureV12TSqlStatistics.Incremental 
		{
			get { return this.Incremental;}
		}
		Boolean ISqlAzureV12TSqlStatistics.NoRecompute 
		{
			get { return this.NoRecompute;}
		}
		Int32 ISqlAzureV12TSqlStatistics.SampleSize 
		{
			get { return this.SampleSize;}
		}
		SamplingStyle ISqlAzureV12TSqlStatistics.SamplingStyle 
		{
			get { return this.SamplingStyle;}
		}
		String ISqlAzureV12TSqlStatistics.StatsStream 
		{
			get { return this.StatsStream;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlStatistics.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlStatistics.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlStatistics.OnObject 
		{
			get 
			{
				return this.OnObject;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlParameter"/>.
	/// </summary>
	public partial class TSqlParameter : ISqlAzureV12TSqlParameter
	{		
		String ISqlAzureV12TSqlParameter.DefaultExpression 
		{
			get { return this.DefaultExpression;}
		}
		Boolean ISqlAzureV12TSqlParameter.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISqlAzureV12TSqlParameter.IsOutput 
		{
			get { return this.IsOutput;}
		}
		Int32 ISqlAzureV12TSqlParameter.Length 
		{
			get { return this.Length;}
		}
		Int32 ISqlAzureV12TSqlParameter.Precision 
		{
			get { return this.Precision;}
		}
		Boolean ISqlAzureV12TSqlParameter.ReadOnly 
		{
			get { return this.ReadOnly;}
		}
		Int32 ISqlAzureV12TSqlParameter.Scale 
		{
			get { return this.Scale;}
		}
		Boolean ISqlAzureV12TSqlParameter.Varying 
		{
			get { return this.Varying;}
		}
		XmlStyle ISqlAzureV12TSqlParameter.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureV12TSqlParameter.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlSchemaCollectionReference> ISqlAzureV12TSqlParameter.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlSynonym"/>.
	/// </summary>
	public partial class TSqlSynonym : ISqlAzureV12TSqlSynonym
	{		
		String ISqlAzureV12TSqlSynonym.ForObjectName 
		{
			get { return this.ForObjectName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlSynonym.ForObject 
		{
			get 
			{
				return this.ForObject;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlSynonym.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTable"/>.
	/// </summary>
	public partial class TSqlTable : ISqlAzureV12TSqlTable
	{		
		Boolean? ISqlAzureV12TSqlTable.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean ISqlAzureV12TSqlTable.ChangeTrackingEnabled 
		{
			get { return this.ChangeTrackingEnabled;}
		}
		Int64? ISqlAzureV12TSqlTable.DataPages 
		{
			get { return this.DataPages;}
		}
		Double? ISqlAzureV12TSqlTable.DataSize 
		{
			get { return this.DataSize;}
		}
		Double? ISqlAzureV12TSqlTable.IndexSize 
		{
			get { return this.IndexSize;}
		}
		Boolean ISqlAzureV12TSqlTable.LargeValueTypesOutOfRow 
		{
			get { return this.LargeValueTypesOutOfRow;}
		}
		LockEscalationMethod ISqlAzureV12TSqlTable.LockEscalation 
		{
			get { return this.LockEscalation;}
		}
		Boolean? ISqlAzureV12TSqlTable.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		Int64? ISqlAzureV12TSqlTable.RowCount 
		{
			get { return this.RowCount;}
		}
		Boolean ISqlAzureV12TSqlTable.TableLockOnBulkLoad 
		{
			get { return this.TableLockOnBulkLoad;}
		}
		Int32 ISqlAzureV12TSqlTable.TextInRowSize 
		{
			get { return this.TextInRowSize;}
		}
		Boolean ISqlAzureV12TSqlTable.TrackColumnsUpdated 
		{
			get { return this.TrackColumnsUpdated;}
		}
		Int64? ISqlAzureV12TSqlTable.UsedPages 
		{
			get { return this.UsedPages;}
		}
		Boolean ISqlAzureV12TSqlTable.VardecimalStorageFormatEnabled 
		{
			get { return this.VardecimalStorageFormatEnabled;}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumn> ISqlAzureV12TSqlTable.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption> ISqlAzureV12TSqlTable.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlTable.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlTable.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference> ISqlAzureV12TSqlTable.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlTable.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableType"/>.
	/// </summary>
	public partial class TSqlTableType : ISqlAzureV12TSqlTableType
	{		

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableTypeColumn> ISqlAzureV12TSqlTableType.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableTypeColumn>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ITableTypeConstraint> ISqlAzureV12TSqlTableType.Constraints 
		{
			get 
			{
				return this.Constraints;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlTableTypeIndex> ISqlAzureV12TSqlTableType.Indexes 
		{
			get 
			{
				return this.Indexes;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlTableType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableTypeCheckConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeCheckConstraint : ISqlAzureV12TSqlTableTypeCheckConstraint
	{		
		String ISqlAzureV12TSqlTableTypeCheckConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlTableTypeCheckConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableTypeColumn"/>.
	/// </summary>
	public partial class TSqlTableTypeColumn : ISqlAzureV12TSqlTableTypeColumn
	{		
		String ISqlAzureV12TSqlTableTypeColumn.Collation 
		{
			get { return this.Collation;}
		}
		String ISqlAzureV12TSqlTableTypeColumn.Expression 
		{
			get { return this.Expression;}
		}
		String ISqlAzureV12TSqlTableTypeColumn.IdentityIncrement 
		{
			get { return this.IdentityIncrement;}
		}
		String ISqlAzureV12TSqlTableTypeColumn.IdentitySeed 
		{
			get { return this.IdentitySeed;}
		}
		Boolean ISqlAzureV12TSqlTableTypeColumn.IsIdentity 
		{
			get { return this.IsIdentity;}
		}
		Boolean ISqlAzureV12TSqlTableTypeColumn.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISqlAzureV12TSqlTableTypeColumn.IsRowGuidCol 
		{
			get { return this.IsRowGuidCol;}
		}
		Int32 ISqlAzureV12TSqlTableTypeColumn.Length 
		{
			get { return this.Length;}
		}
		Boolean ISqlAzureV12TSqlTableTypeColumn.Nullable 
		{
			get { return this.Nullable;}
		}
		Boolean ISqlAzureV12TSqlTableTypeColumn.Persisted 
		{
			get { return this.Persisted;}
		}
		Boolean? ISqlAzureV12TSqlTableTypeColumn.PersistedNullable 
		{
			get { return this.PersistedNullable;}
		}
		Int32 ISqlAzureV12TSqlTableTypeColumn.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISqlAzureV12TSqlTableTypeColumn.Scale 
		{
			get { return this.Scale;}
		}
		XmlStyle ISqlAzureV12TSqlTableTypeColumn.XmlStyle 
		{
			get { return this.XmlStyle;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlDataType> ISqlAzureV12TSqlTableTypeColumn.DataType 
		{
			get 
			{
				return this.DataType;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlTableTypeColumn.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlSchemaCollectionReference> ISqlAzureV12TSqlTableTypeColumn.XmlSchemaCollection 
		{
			get 
			{
				return this.XmlSchemaCollection.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlSchemaCollectionReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableTypeDefaultConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeDefaultConstraint : ISqlAzureV12TSqlTableTypeDefaultConstraint
	{		
		String ISqlAzureV12TSqlTableTypeDefaultConstraint.Expression 
		{
			get { return this.Expression;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlTableTypeDefaultConstraint.ExpressionDependencies 
		{
			get 
			{
				return this.ExpressionDependencies;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlTableTypeDefaultConstraint.TargetColumn 
		{
			get 
			{
				return this.TargetColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableTypeIndex"/>.
	/// </summary>
	public partial class TSqlTableTypeIndex : ISqlAzureV12TSqlTableTypeIndex
	{		
		Int32? ISqlAzureV12TSqlTableTypeIndex.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureV12TSqlTableTypeIndex.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureV12TSqlTableTypeIndex.IsDisabled 
		{
			get { return this.IsDisabled;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlTableTypeIndex.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableTypePrimaryKeyConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypePrimaryKeyConstraint : ISqlAzureV12TSqlTableTypePrimaryKeyConstraint
	{		
		Int32? ISqlAzureV12TSqlTableTypePrimaryKeyConstraint.BucketCount 
		{
			get { return this.BucketCount;}
		}
		Boolean ISqlAzureV12TSqlTableTypePrimaryKeyConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureV12TSqlTableTypePrimaryKeyConstraint.Hash 
		{
			get { return this.Hash;}
		}
		Boolean ISqlAzureV12TSqlTableTypePrimaryKeyConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlTableTypePrimaryKeyConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlTableTypeUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlTableTypeUniqueConstraint : ISqlAzureV12TSqlTableTypeUniqueConstraint
	{		
		Boolean ISqlAzureV12TSqlTableTypeUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureV12TSqlTableTypeUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlTableTypeUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlUniqueConstraint"/>.
	/// </summary>
	public partial class TSqlUniqueConstraint : ISqlAzureV12TSqlUniqueConstraint
	{		
		Boolean ISqlAzureV12TSqlUniqueConstraint.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISqlAzureV12TSqlUniqueConstraint.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISqlAzureV12TSqlUniqueConstraint.Clustered 
		{
			get { return this.Clustered;}
		}
		Boolean ISqlAzureV12TSqlUniqueConstraint.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISqlAzureV12TSqlUniqueConstraint.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISqlAzureV12TSqlUniqueConstraint.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureV12TSqlUniqueConstraint.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISqlAzureV12TSqlUniqueConstraint.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlUniqueConstraint.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption> ISqlAzureV12TSqlUniqueConstraint.DataCompressionOptions 
		{
			get 
			{
				return this.DataCompressionOptions.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataCompressionOption>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference> ISqlAzureV12TSqlUniqueConstraint.Filegroup 
		{
			get 
			{
				return this.Filegroup.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlFilegroupReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference> ISqlAzureV12TSqlUniqueConstraint.Host 
		{
			get 
			{
				return this.Host.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlTableReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlUniqueConstraint.PartitionColumn 
		{
			get 
			{
				return this.PartitionColumn.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference> ISqlAzureV12TSqlUniqueConstraint.PartitionScheme 
		{
			get 
			{
				return this.PartitionScheme.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlPartitionSchemeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlUser"/>.
	/// </summary>
	public partial class TSqlUser : ISqlAzureV12TSqlUser
	{		
		AuthenticationType ISqlAzureV12TSqlUser.AuthenticationType 
		{
			get { return this.AuthenticationType;}
		}
		String ISqlAzureV12TSqlUser.Password 
		{
			get { return this.Password;}
		}
		String ISqlAzureV12TSqlUser.Sid 
		{
			get { return this.Sid;}
		}
		Boolean ISqlAzureV12TSqlUser.WithoutLogin 
		{
			get { return this.WithoutLogin;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlUser.DefaultSchema 
		{
			get 
			{
				return this.DefaultSchema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference> ISqlAzureV12TSqlUser.Login 
		{
			get 
			{
				return this.Login.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlLoginReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlUserDefinedType"/>.
	/// </summary>
	public partial class TSqlUserDefinedType : ISqlAzureV12TSqlUserDefinedType
	{		
		Boolean? ISqlAzureV12TSqlUserDefinedType.ByteOrdered 
		{
			get { return this.ByteOrdered;}
		}
		String ISqlAzureV12TSqlUserDefinedType.ClassName 
		{
			get { return this.ClassName;}
		}
		Boolean? ISqlAzureV12TSqlUserDefinedType.FixedLength 
		{
			get { return this.FixedLength;}
		}
		Format ISqlAzureV12TSqlUserDefinedType.Format 
		{
			get { return this.Format;}
		}
		Int32? ISqlAzureV12TSqlUserDefinedType.MaxByteSize 
		{
			get { return this.MaxByteSize;}
		}
		String ISqlAzureV12TSqlUserDefinedType.ValidationMethodName 
		{
			get { return this.ValidationMethodName;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference> ISqlAzureV12TSqlUserDefinedType.Assembly 
		{
			get 
			{
				return this.Assembly.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlAssemblyReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISqlAzureV12TSqlUserDefinedType.Methods 
		{
			get 
			{
				return this.Methods;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.TSqlModelElement> ISqlAzureV12TSqlUserDefinedType.Properties 
		{
			get 
			{
				return this.Properties;
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlUserDefinedType.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlView"/>.
	/// </summary>
	public partial class TSqlView : ISqlAzureV12TSqlView
	{		
		Boolean? ISqlAzureV12TSqlView.AnsiNullsOn 
		{
			get { return this.AnsiNullsOn;}
		}
		Boolean? ISqlAzureV12TSqlView.QuotedIdentifierOn 
		{
			get { return this.QuotedIdentifierOn;}
		}
		String ISqlAzureV12TSqlView.SelectStatement 
		{
			get { return this.SelectStatement;}
		}
		Boolean ISqlAzureV12TSqlView.WithCheckOption 
		{
			get { return this.WithCheckOption;}
		}
		Boolean ISqlAzureV12TSqlView.WithEncryption 
		{
			get { return this.WithEncryption;}
		}
		Boolean ISqlAzureV12TSqlView.WithSchemaBinding 
		{
			get { return this.WithSchemaBinding;}
		}
		Boolean ISqlAzureV12TSqlView.WithViewMetadata 
		{
			get { return this.WithViewMetadata;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlModelElementReference> ISqlAzureV12TSqlView.BodyDependencies 
		{
			get 
			{
				return this.BodyDependencies;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumn> ISqlAzureV12TSqlView.Columns 
		{
			get 
			{
				return this.Columns.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumn>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlView.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlXmlIndex"/>.
	/// </summary>
	public partial class TSqlXmlIndex : ISqlAzureV12TSqlXmlIndex
	{		
		Boolean ISqlAzureV12TSqlXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISqlAzureV12TSqlXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISqlAzureV12TSqlXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISqlAzureV12TSqlXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISqlAzureV12TSqlXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureV12TSqlXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISqlAzureV12TSqlXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		SecondaryXmlIndexType ISqlAzureV12TSqlXmlIndex.SecondaryXmlIndexType 
		{
			get { return this.SecondaryXmlIndexType;}
		}
		Boolean ISqlAzureV12TSqlXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureV12TSqlXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlIndexReference> ISqlAzureV12TSqlXmlIndex.PrimaryXmlIndex 
		{
			get 
			{
				return this.PrimaryXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlIndexReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlSelectiveXmlIndex"/>.
	/// </summary>
	public partial class TSqlSelectiveXmlIndex : ISqlAzureV12TSqlSelectiveXmlIndex
	{		
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.AllowPageLocks 
		{
			get { return this.AllowPageLocks;}
		}
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.AllowRowLocks 
		{
			get { return this.AllowRowLocks;}
		}
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.Disabled 
		{
			get { return this.Disabled;}
		}
		Int32? ISqlAzureV12TSqlSelectiveXmlIndex.FillFactor 
		{
			get { return this.FillFactor;}
		}
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.IgnoreDuplicateKey 
		{
			get { return this.IgnoreDuplicateKey;}
		}
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.IsPrimary 
		{
			get { return this.IsPrimary;}
		}
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.RecomputeStatistics 
		{
			get { return this.RecomputeStatistics;}
		}
		Boolean ISqlAzureV12TSqlSelectiveXmlIndex.WithPadIndex 
		{
			get { return this.WithPadIndex;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference> ISqlAzureV12TSqlSelectiveXmlIndex.Column 
		{
			get 
			{
				return this.Column.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlColumnReference>();
			}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISpecifiesIndex> ISqlAzureV12TSqlSelectiveXmlIndex.IndexedObject 
		{
			get 
			{
				return this.IndexedObject;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> ISqlAzureV12TSqlSelectiveXmlIndex.PrimaryPromotedPath 
		{
			get 
			{
				return this.PrimaryPromotedPath;
			}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSelectiveXmlIndexReference> ISqlAzureV12TSqlSelectiveXmlIndex.PrimarySelectiveXmlIndex 
		{
			get 
			{
				return this.PrimarySelectiveXmlIndex.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSelectiveXmlIndexReference>();
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlPromotedNodePath> ISqlAzureV12TSqlSelectiveXmlIndex.PromotedPaths 
		{
			get 
			{
				return this.PromotedPaths;
			}
		}

		// Composing relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlNamespace> ISqlAzureV12TSqlSelectiveXmlIndex.XmlNamespaces 
		{
			get 
			{
				return this.XmlNamespaces.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlXmlNamespace>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlXmlNamespace"/>.
	/// </summary>
	public partial class TSqlXmlNamespace : ISqlAzureV12TSqlXmlNamespace
	{		
		String ISqlAzureV12TSqlXmlNamespace.NamespaceUri 
		{
			get { return this.NamespaceUri;}
		}
		String ISqlAzureV12TSqlXmlNamespace.Prefix 
		{
			get { return this.Prefix;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPromotedNodePathForXQueryType"/>.
	/// </summary>
	public partial class TSqlPromotedNodePathForXQueryType : ISqlAzureV12TSqlPromotedNodePathForXQueryType
	{		
		Boolean ISqlAzureV12TSqlPromotedNodePathForXQueryType.IsSingleton 
		{
			get { return this.IsSingleton;}
		}
		Int32? ISqlAzureV12TSqlPromotedNodePathForXQueryType.MaxLength 
		{
			get { return this.MaxLength;}
		}
		String ISqlAzureV12TSqlPromotedNodePathForXQueryType.NodePath 
		{
			get { return this.NodePath;}
		}
		String ISqlAzureV12TSqlPromotedNodePathForXQueryType.Type 
		{
			get { return this.Type;}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlPromotedNodePathForSqlType"/>.
	/// </summary>
	public partial class TSqlPromotedNodePathForSqlType : ISqlAzureV12TSqlPromotedNodePathForSqlType
	{		
		Boolean ISqlAzureV12TSqlPromotedNodePathForSqlType.IsMax 
		{
			get { return this.IsMax;}
		}
		Boolean ISqlAzureV12TSqlPromotedNodePathForSqlType.IsSingleton 
		{
			get { return this.IsSingleton;}
		}
		Int32 ISqlAzureV12TSqlPromotedNodePathForSqlType.Length 
		{
			get { return this.Length;}
		}
		String ISqlAzureV12TSqlPromotedNodePathForSqlType.NodePath 
		{
			get { return this.NodePath;}
		}
		Int32 ISqlAzureV12TSqlPromotedNodePathForSqlType.Precision 
		{
			get { return this.Precision;}
		}
		Int32 ISqlAzureV12TSqlPromotedNodePathForSqlType.Scale 
		{
			get { return this.Scale;}
		}

		// Peer relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference> ISqlAzureV12TSqlPromotedNodePathForSqlType.DataType 
		{
			get 
			{
				return this.DataType.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlDataTypeReference>();
			}
		}
	}
	/// <summary>
	/// Explicit implementation of <see cref="ISqlAzureV12TSqlXmlSchemaCollection"/>.
	/// </summary>
	public partial class TSqlXmlSchemaCollection : ISqlAzureV12TSqlXmlSchemaCollection
	{		
		String ISqlAzureV12TSqlXmlSchemaCollection.Expression 
		{
			get { return this.Expression;}
		}

		// Hierarchical relationship		
		IEnumerable<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference> ISqlAzureV12TSqlXmlSchemaCollection.Schema 
		{
			get 
			{
				return this.Schema.Cast<Microsoft.SqlServer.Dac.Extensions.Prototype.ISqlAzureV12TSqlSchemaReference>();
			}
		}
	}
}