using Microsoft.VisualStudio.Data.Framework;
using Microsoft.VisualStudio.Data.Framework.AdoDotNet;
using Microsoft.VisualStudio.Data.Services;
using Microsoft.VisualStudio.Data.Services.SupportEntities;
using System;
using System.Data.Common;
using System.Diagnostics;

namespace ErikEJ.SqlCeToolbox.DDEX4
{
    /// <summary>
    /// Represents a custom data object selector to supplement or replace
    /// the schema collections supplied by the .NET Framework Data Provider
    /// for SQL Server.  Many of the enumerations here are required for full
    /// support of the built in data design scenarios.
    /// </summary>
    internal class SqlCeObjectSelector : DataObjectSelector
	{
		#region Protected Methods

		protected override IVsDataReader SelectObjects(string typeName,
			object[] restrictions, string[] properties, object[] parameters)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
        
			// Execute a SQL statement to get the property values
			DbConnection conn = Site.GetLockedProviderObject() as DbConnection;
			Debug.Assert(conn != null, "Invalid provider object.");
			if (conn == null)
			{
				// This should never occur
				throw new NotSupportedException();
			}
			try
			{
				// Ensure the connection is open
				if (Site.State != DataConnectionState.Open)
				{
					Site.Open();
				}

				// Create a command object
				var comm = conn.CreateCommand();

				// Choose and format SQL based on the type
				if (typeName.Equals(SqlObjectTypes.Root,
						StringComparison.OrdinalIgnoreCase))
				{
					comm.CommandText = string.Format("SELECT '{0}' AS Name", System.IO.Path.GetFileName(conn.DataSource));
				}
				else
				{
					throw new NotSupportedException();
				}

				return new AdoDotNetReader(comm.ExecuteReader());
			}
			finally
			{
				Site.UnlockProviderObject();
			}
		}

		#endregion

	}
}
