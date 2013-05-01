using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using Doit.Actions;
using SearchAPI;

namespace Doit.ActionProviders
{
	public class FindActionProvider : SingleParameterActionProvider<FileAction>
	{
		public FindActionProvider() : base("find")
		{
		}

		protected override IEnumerable<FileAction> OfferCore(string parameter)
		{
			if (!string.IsNullOrEmpty(parameter))
			{
				var files = new string[0];

				try
				{
					files = FindFiles(parameter).ToArray();
				}
				catch (Exception)
				{
				}

				foreach (var path in files)
				{
					yield return new FileAction(path);
				}
			}
		}

		private IEnumerable<string> FindFiles(string pattern)
		{
			string userQuery = " ";

			// This uses the Microsoft.Search.Interop assembly
			CSearchManager manager = new CSearchManager();

			// SystemIndex catalog is the default catalog in Windows
			ISearchCatalogManager catalogManager = manager.GetCatalog("SystemIndex");

			// Get the ISearchQueryHelper which will help us to translate AQS --> SQL necessary to query the indexer
			ISearchQueryHelper queryHelper = catalogManager.GetQueryHelper();

			// Set the number of results we want. Don't set this property if all results are needed.
			queryHelper.QueryMaxResults = 10;

			// Set list of columns we want
			queryHelper.QuerySelectColumns = "System.ItemUrl";

			// Set additional query restriction
			queryHelper.QueryWhereRestrictions = "AND scope='file:'";

			// convert file pattern if it is not '*'. Don't create restriction for '*' as it includes all files.
			if (pattern != "*")
			{
				pattern = pattern.Replace("*", "%");
				pattern = pattern.Replace("?", "_");

				if (pattern.Contains("%") || pattern.Contains("_"))
				{
					queryHelper.QueryWhereRestrictions += " AND System.FileName LIKE '" + pattern + "' ";
				}
				else
				{
					// if there are no wildcards we can use a contains which is much faster as it uses the index
					queryHelper.QueryWhereRestrictions += " AND Contains(System.FileName, '" + pattern + "') ";
				}
			}

			// Set sorting order 
			queryHelper.QuerySorting = "System.DateModified DESC";

			// Generate SQL from our parameters, converting the userQuery from AQS->WHERE clause
			string sqlQuery = queryHelper.GenerateSQLFromUserQuery(userQuery);
			Console.WriteLine(sqlQuery);

			// --- Perform the query ---
			// create an OleDbConnection object which connects to the indexer provider with the windows application
			using (var conn = new OleDbConnection(queryHelper.ConnectionString))
			{
				// open the connection
				conn.Open();

				// now create an OleDB command object with the query we built above and the connection we just opened.
				using (var command = new OleDbCommand(sqlQuery, conn))
				{
					// execute the command, which returns the results as an OleDbDataReader.
					using (var results = command.ExecuteReader())
					{
						while (results.Read())
						{
							var url = results.GetString(0);

							if (url.StartsWith("file:", StringComparison.InvariantCultureIgnoreCase))
							{
								yield return url.Substring(5).Replace('/', '\\');
							}
						}
					}
				}
			}
		}
	}
}