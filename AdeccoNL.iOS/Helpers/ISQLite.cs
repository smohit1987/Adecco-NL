using System;
using SQLite.Net;

namespace AdeccoNL
{
	public interface ISQLite
	{
		string GetDatabasePath();

		SQLiteConnection GetConnection();

		void DropDatabase();
		bool TableExists<T>(string TableName);
	}
}

