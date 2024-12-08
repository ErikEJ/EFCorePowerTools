using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;

namespace RevEng.Core
{
    public class DataverseModelFactoryExtension
    {
        private readonly ServiceClient serviceClient;

        public DataverseModelFactoryExtension(ServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        /// <summary>
        /// Checks if the connection is for a Dataverse instance and creates a new <see cref="DataverseModelFactoryExtension"/>
        /// if so.
        /// </summary>
        /// <param name="connection">The database connection to use to connect to Dataverse.</param>
        /// <param name="dataverse">Set to a new <see cref="DataverseModelFactoryExtension"/> if the <paramref name="connection"/> represents a connection to a Dataverse TDS Endpoint.</param>
        /// <returns><see langword="true"/> if the <paramref name="connection"/> represents a connection to a Dataverse TDS Endpoint, or <see langword="false"/> otherwise.</returns>
        public static bool TryCreate(DbConnection connection, out DataverseModelFactoryExtension dataverse)
        {
            ArgumentNullException.ThrowIfNull(connection);

            var connectionStringParser = new SqlConnectionStringBuilder(connection.ConnectionString);
            if (connection is SqlConnection sqlConnection &&
                connectionStringParser.DataSource.Contains("dynamics.com", StringComparison.OrdinalIgnoreCase))
            {
                connectionStringParser.Authentication = SqlAuthenticationMethod.NotSpecified;
                connection.ConnectionString = connectionStringParser.ToString();
                var serviceClient = new ServiceClient($"AuthType=OAuth;Username={connectionStringParser.UserID};Url=https://{connectionStringParser.DataSource};AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=http://localhost;LoginPrompt=Auto");
                sqlConnection.AccessTokenCallback = (_, __) => Task.FromResult(new SqlAuthenticationToken(serviceClient.CurrentAccessToken, DateTimeOffset.MaxValue));

                dataverse = new DataverseModelFactoryExtension(serviceClient);
                return true;
            }

            dataverse = null;
            return false;
        }

        /// <summary>
        /// Updates the <paramref name="tables"/> with metadata from the Dataverse instance.
        /// </summary>
        /// <param name="tables">The table definitions that have already been loaded from the Dataverse TDS Endpoint.</param>
        public void GetDataverseMetadata(List<DatabaseTable> tables)
        {
            var metadataQuery = new EntityQueryExpression
            {
                Properties = new MetadataPropertiesExpression(
                    nameof(EntityMetadata.LogicalName),
                    nameof(EntityMetadata.SchemaName),
                    nameof(EntityMetadata.PrimaryIdAttribute),
                    nameof(EntityMetadata.Attributes),
                    nameof(EntityMetadata.ManyToOneRelationships),
                    nameof(EntityMetadata.Keys)),
                AttributeQuery = new AttributeQueryExpression
                {
                    Properties = new MetadataPropertiesExpression(
                        nameof(AttributeMetadata.LogicalName),
                        nameof(AttributeMetadata.SchemaName)),
                },
                RelationshipQuery = new RelationshipQueryExpression
                {
                    Properties = new MetadataPropertiesExpression(
                        nameof(OneToManyRelationshipMetadata.SchemaName),
                        nameof(OneToManyRelationshipMetadata.ReferencingAttribute),
                        nameof(OneToManyRelationshipMetadata.ReferencedEntity),
                        nameof(OneToManyRelationshipMetadata.ReferencedAttribute),
                        nameof(OneToManyRelationshipMetadata.CascadeConfiguration)),
                },
                KeyQuery = new EntityKeyQueryExpression
                {
                    Properties = new MetadataPropertiesExpression(
                        nameof(EntityKeyMetadata.SchemaName),
                        nameof(EntityKeyMetadata.KeyAttributes)),
                },
            };
            var metadata = (RetrieveMetadataChangesResponse)serviceClient.Execute(new RetrieveMetadataChangesRequest { Query = metadataQuery });

            foreach (var entity in metadata.EntityMetadata)
            {
                // Check if the entity is in the table list
                var table = tables.SingleOrDefault(t => t.Name == entity.LogicalName);
                if (table is null)
                {
                    continue;
                }

                // Use the schema names for tables and columns instead of the default logical names for more standard .NET naming
                // Only switch to the schema names if they are the same as the logical name except in a different case.
                if (entity.SchemaName != entity.LogicalName && entity.LogicalName.Equals(entity.SchemaName, StringComparison.OrdinalIgnoreCase))
                {
                    table.Name = entity.SchemaName;
                }

                foreach (var attr in entity.Attributes)
                {
                    var col = table.Columns.SingleOrDefault(c => c.Name == attr.LogicalName);

                    if (col != null && attr.SchemaName != attr.LogicalName && attr.LogicalName.Equals(attr.SchemaName, StringComparison.OrdinalIgnoreCase))
                    {
                        col.Name = attr.SchemaName;
                    }
                }

                // Add the primary key column
                table.PrimaryKey = new DatabasePrimaryKey
                {
                    Table = table,
                    Name = entity.PrimaryIdAttribute,
                    Columns =
                    {
                        table.Columns.Single(c => c.Name.Equals(entity.PrimaryIdAttribute, StringComparison.OrdinalIgnoreCase)),
                    },
                };

                // Add the alternate keys
                foreach (var key in entity.Keys)
                {
                    var uniqueConstraint = new DatabaseUniqueConstraint
                    {
                        Table = table,
                        Name = key.SchemaName,
                    };

                    var hasAllColumns = true;

                    foreach (var attr in key.KeyAttributes)
                    {
                        var col = table.Columns.SingleOrDefault(c => c.Name.Equals(attr, StringComparison.OrdinalIgnoreCase));

                        if (col != null)
                        {
                            uniqueConstraint.Columns.Add(col);
                        }
                        else
                        {
                            hasAllColumns = false;
                        }
                    }

                    if (hasAllColumns)
                    {
                        table.UniqueConstraints.Add(uniqueConstraint);
                    }
                }

                // Add the foreign key relationships
                foreach (var relationship in entity.ManyToOneRelationships)
                {
                    var referencedTable = tables.SingleOrDefault(t => t.Name.Equals(relationship.ReferencedEntity, StringComparison.OrdinalIgnoreCase));

                    if (referencedTable is null)
                    {
                        continue;
                    }

                    var referencingColumn = table.Columns.SingleOrDefault(c => c.Name.Equals(relationship.ReferencingAttribute, StringComparison.OrdinalIgnoreCase));
                    var referencedColumn = referencedTable.Columns.SingleOrDefault(c => c.Name.Equals(relationship.ReferencedAttribute, StringComparison.OrdinalIgnoreCase));

                    if (referencingColumn is null || referencedColumn is null)
                    {
                        continue;
                    }

                    table.ForeignKeys.Add(new DatabaseForeignKey
                    {
                        Table = table,
                        PrincipalTable = referencedTable,
                        Name = relationship.SchemaName,
                        Columns =
                        {
                            referencingColumn,
                        },
                        PrincipalColumns =
                        {
                            referencedColumn,
                        },
                    });
                }
            }
        }
    }
}
