using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace AdeccoNL
{
	public class JobSearch
	{
		public string Count { get; set; }
		public List<Job> Items { get; set; }
		public string LocationWithinRadius { get; set; }
		public List<JobFacets> PresentationFacetResults { get; set; }

	}
	[Table("RecentSearch")]
	public class RecentSearch
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }
		public string Keyword { get; set; }
		public string Location { get; set; }
		public string LocationLatLong { get; set; }

	}
}

