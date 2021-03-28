using Microsoft.VisualStudio.Data.Framework.AdoDotNet;
using Microsoft.VisualStudio.Data.Services;
using System;
using System.Data.Common;
using System.Diagnostics;

namespace ErikEJ.SqlCeToolbox.DDEX4
{
    /// <summary>
    /// Represents a custom data source information class that is able to
    /// provide data source information values that require some form of
    /// computation, perhaps based on an active connection.
    /// </summary>
    internal class SqlCeSourceInformation : AdoDotNetSourceInformation
	{
		#region Constructors

		public SqlCeSourceInformation()
		{
			AddProperty(DataSourceName);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// RetrieveValue is called once per property that was identified
		/// as existing but without a value (specified in the constructor).
		/// For the purposes of this sample, only one property needs to be
		/// computed - DefaultSchema.  To retrieve this value a SQL statement
		/// is executed.
		/// </summary>
		protected override object RetrieveValue(string propertyName)
		{
			if (propertyName.Equals(DataSourceName,
					StringComparison.OrdinalIgnoreCase))
			{
				if (Site.State != DataConnectionState.Open)
				{
					Site.Open();
				}
				var conn = Connection;
				Debug.Assert(conn != null, "Invalid provider object.");
			    try
			    {
			        return System.IO.Path.GetFileName(conn.Database);
			    }
			    catch (DbException)
			    {
			        // We let the base class apply default behavior
			    }
			}
			return base.RetrieveValue(propertyName);
		}
		#endregion
	}
}
