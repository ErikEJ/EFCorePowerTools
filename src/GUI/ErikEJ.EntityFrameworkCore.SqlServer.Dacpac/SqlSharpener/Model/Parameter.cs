using dac = Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlSharpener.Model
{
    /// <summary>
    /// Represents a parameter of a stored procedure.
    /// </summary>
    [Serializable]
    public class Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <param name="isOutput">if set to <c>true</c> [is output].</param>
        public Parameter(string name, IDictionary<TypeFormat, string> dataTypes, bool isOutput, Table tableValue)
        {
            this.Name = name;
            this.DataTypes = dataTypes ?? new Dictionary<TypeFormat, string>();
            this.IsOutput = isOutput;
            this.TableValue = tableValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter" /> class.
        /// </summary>
        /// <param name="tSqlObject">The TSqlObject representing the parameter.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="foreignKeys">The foreign keys.</param>
        public Parameter(dac.TSqlObject tSqlObject)
        {
            this.Name = tSqlObject.Name.Parts.Last().Trim('@');
            this.IsOutput = dac.Parameter.IsOutput.GetValue<bool>(tSqlObject);
            var dataType = tSqlObject.GetReferenced(dac.Parameter.DataType).ToList().FirstOrDefault();
            if (dataType == null)
            {
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.DotNetFrameworkType, "Object");
            }
            else if (dataType.ObjectType.Name == "TableType")
            {
                this.TableValue = new Table(dataType);
            }
            else
            {
                var sqlDataTypeName = dataType.Name.Parts.Last();
                this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, sqlDataTypeName);
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
        /// Gets the data types for this parameter.
        /// </summary>
        /// <value>
        /// The data types.
        /// </value>
        public IDictionary<TypeFormat, string> DataTypes { get; private set; }

        /// <summary>
        /// Gets the table representing this parameter if it is a table variable parameter.
        /// </summary>
        /// <value>
        /// The table variable.
        /// </value>
        public Table TableValue { get; private set; }

        public bool IsTableValue { get { return this.TableValue != null; } }

        /// <summary>
        /// Gets a value indicating whether this instance is an output parameter.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is an output parameter; otherwise, <c>false</c>.
        /// </value>
        public bool IsOutput { get; private set; }
    }
}
