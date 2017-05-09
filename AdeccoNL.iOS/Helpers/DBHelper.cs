using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Interop;
namespace AdeccoNL
{
	public static class DbHelper
	{

		public static List<JobLocation> JobLocations;
		public static string[] locationCordinates { get; set; }


		#region Member Variables
		private static SQLiteConnection _dbConnection = null;
		private static string _path = string.Empty;
		private static ISQLitePlatform _platform = null;
		#endregion

		#region Properties
		public static SQLiteConnection DBConnection
		{
			get
			{
				if (_dbConnection == null)
				{
					_dbConnection	= new SQLiteConnection(_platform, _path);
				}
				return _dbConnection;
			}
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Gets the connection.
		/// </summary>
		/// <returns>The connection.</returns>
		/// <param name="path">Path.</param>
		/// <param name="platform">Platform.</param>
		public static SQLiteConnection GetConnection(string path, ISQLitePlatform platform)
		{
			_path = path;_platform = platform;


			return DBConnection;
		}

		/// <summary>
		/// Creates the database and tables.
		/// </summary>
		/// <returns><c>true</c>, if database and tables was created, <c>false</c> otherwise.</returns>
		public static void CreateDatabaseAndTables()
		{
			var db = GetConnection(_path, _platform);
           	db.CreateTable<LatestJobs>();
            db.CreateTable<JobCMS>();
			db.CreateTable<RecentSearch>();
			db.CreateTable<JobTranslation>();
			db.CreateTable<JobListLayout>();
		}

		/// <summary>
		/// Resets the database.
		/// </summary>
		public static void ResetDatabase()
		{
			var db = GetConnection(_path, _platform);
            db.CreateTable<LatestJobs>();
            db.DropTable<JobCMS>();
			db.DropTable<RecentSearch>();
			db.DropTable<JobTranslation>();
			db.DropTable<JobListLayout>();
		}

		/// <summary>
		/// Gets Favorite Jobs
		/// </summary>
		/// <returns>List of Favorite Jobs</returns>
		public static List<JobCMS> GetFavoriteJobs()
		{
			var dbConnection = GetConnection(_path, _platform);
			List<JobCMS> favJobs = dbConnection.Table<JobCMS>().ToList<JobCMS>();
			return favJobs;
		}

        /// <summary>
		/// Gets Latest Jobs
		/// </summary>
		/// <returns>List of Favorite Jobs</returns>
		public static List<LatestJobs> GetLatestJobs()
        {
            var dbConnection = GetConnection(_path, _platform);
            List<LatestJobs> latestJobs = dbConnection.Table<LatestJobs>().ToList<LatestJobs>();
            return latestJobs;
        }

        /// <summary>
		/// Gets Latest Jobs
		/// </summary>
		/// <returns>List of Favorite Jobs</returns>
		public static void AddLatestJobs(List<LatestJobs> jobs)
        {
            var dbConnection = GetConnection(_path, _platform);
            dbConnection.DeleteAll<LatestJobs>();
            foreach (LatestJobs job in jobs)
            {
                dbConnection.Insert(job);
            }
            
        }

		public static void AddFavoriteJob(JobCMS job)
		{
			var dbConnection = GetConnection(_path, _platform);
				dbConnection.Insert(job);
		}

		public static void DeleteFavoriteJob(JobCMS job)
		{
			var dbConnection = GetConnection(_path, _platform);
			dbConnection.Delete(job);
		}



        /// <summary>
        /// Gets the job translation.
        /// </summary>
        /// <returns>The job translation.</returns>
        public static List<RecentSearch> GetRecentSearches()
		{
			var dbConnection = GetConnection(_path, _platform);
			List<RecentSearch> recentSearches = dbConnection.Table<RecentSearch>().ToList<RecentSearch>();
			return recentSearches;
		}
		public static void DeleteRecentSearches()
		{
			var db = GetConnection(_path, _platform);
			db.DropTable<RecentSearch>();

			// Now create fresh table 
			db.CreateTable<RecentSearch>();
		}

		/// <summary>
		/// Updates the recent search.
		/// </summary>
		/// <param name="recentSearch">Recent search.</param>
		public static void AddRecentSearch(RecentSearch recentSearch)
		{
			List<RecentSearch> recentSearches = GetRecentSearches();
			var dbConnection = GetConnection(_path, _platform);
			if (recentSearches.Count == 10)
			{
				
				RecentSearch firstRecentSearch = dbConnection.Table<RecentSearch>().OrderBy(z => z.Id).First();
				dbConnection.Delete(firstRecentSearch);
			}
			var searchObject = dbConnection.Table<RecentSearch>().Where(x => x.Keyword == recentSearch.Keyword && x.Location == recentSearch.Location).FirstOrDefault();
			if (searchObject == null)
			{
				DBConnection.Insert(recentSearch);
			}

		}
		#endregion
	}
}

