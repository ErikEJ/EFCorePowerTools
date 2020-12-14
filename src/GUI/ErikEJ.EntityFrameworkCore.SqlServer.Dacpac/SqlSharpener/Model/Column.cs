﻿using dac = Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlSharpener.Model
{
    /// <summary>
    /// Represents a column in a table.
    /// </summary>
    [Serializable]
    public class Column
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Column" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <param name="isIdentity">if set to <c>true</c> [is identity].</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        /// <param name="precision">The precision.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="length">The length.</param>
        public Column(string name, IDictionary<TypeFormat, string> dataTypes, bool isIdentity, bool isNullable, int precision, int scale, int length)
        {
            this.Name = name;
            this.DataTypes = dataTypes ?? new Dictionary<TypeFormat, string>();
            this.IsIdentity = isIdentity;
            this.IsNullable = isNullable;
            this.Precision = precision;
            this.Scale = scale;
            this.Length = length;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Column" /> class.
        /// </summary>
        /// <param name="tSqlObject">The TSqlObject representing the column.</param>
        /// <param name="tSqlTable">The table or view this column belongs to.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="foreignKeys">The foreign keys.</param>
        public Column(dac.TSqlObject tSqlObject)
        {
            this.Name = tSqlObject.Name.Parts.Last();

            if (tSqlObject.ObjectType.Name == "TableTypeColumn")
            {
                var sqlDataTypeName = tSqlObject.GetReferenced(dac.TableTypeColumn.DataType).ToList().First().Name.Parts.Last();
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, sqlDataTypeName);
                this.IsIdentity = dac.TableTypeColumn.IsIdentity.GetValue<bool>(tSqlObject);
                this.IsNullable = dac.TableTypeColumn.Nullable.GetValue<bool>(tSqlObject);
                this.Precision = dac.TableTypeColumn.Precision.GetValue<int>(tSqlObject);
                this.Scale = dac.TableTypeColumn.Scale.GetValue<int>(tSqlObject);
                this.Length = dac.TableTypeColumn.Length.GetValue<int>(tSqlObject);
            }
            else
            {
                dac.ColumnType metaType = tSqlObject.GetMetadata<dac.ColumnType>(dac.Column.ColumnType);

                switch (metaType)
                {
                    case dac.ColumnType.Column:
                    case dac.ColumnType.ColumnSet:
                        SetProperties(tSqlObject);
                        break;
                    case dac.ColumnType.ComputedColumn:
                        // use the referenced column - this works for simple view referenced
                        // column but not for a computed expression like [Name] = [FirstName] + ' ' + [LastName]
                        var referenced = tSqlObject.GetReferenced().ToArray();
                        if (referenced.Length == 1)
                        {
                            var tSqlObjectReferenced = referenced[0];
                            SetProperties(tSqlObjectReferenced);
                        }
                        else
                        {
                            // TODO: how to get and evaluate the expression?
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the data types.
        /// </summary>
        /// <value>
        /// The data types.
        /// </value>
        public IDictionary<TypeFormat, string> DataTypes { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an identity column.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an identity column; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; private set; }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>
        /// The precision.
        /// </value>
        public int Precision { get; private set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public int Scale { get; private set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; private set; }

        private void SetProperties(dac.TSqlObject tSqlObject)
        {
            var sqlDataTypeName = tSqlObject.GetReferenced(dac.Column.DataType).ToList().First().Name.Parts.Last();
            this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, sqlDataTypeName);
            this.IsIdentity = dac.Column.IsIdentity.GetValue<bool>(tSqlObject);
            this.IsNullable = dac.Column.Nullable.GetValue<bool>(tSqlObject);
            this.Precision = dac.Column.Precision.GetValue<int>(tSqlObject);
            this.Scale = dac.Column.Scale.GetValue<int>(tSqlObject);
            this.Length = dac.Column.Length.GetValue<int>(tSqlObject);
        }
    }
}
