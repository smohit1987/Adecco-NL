using System;
using SQLite.Net.Attributes;

namespace AdeccoNL
{
	/// <summary>
	/// Job list layout.
	/// </summary>
	[Table("JobListLayout")]
	public class JobListLayout
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string LayoutName { get; set; }
		public Boolean Location { get; set; }
		public Boolean IndustryTypeTitle { get; set; }
		public Boolean Salary { get; set; }
		public Boolean Reference { get; set; }
		public Boolean Description { get; set; }
		public Boolean PostedDate { get; set; }
		public Boolean Title { get; set; }
		public Boolean IsFavorite { get; set; }
		public Boolean Status { get; set; }
	}
	/// <summary>
	/// Job translation.
	/// </summary>
	[Table("JobTranslation")]
	public class JobTranslation : Job
	{
		public string LocalizedFileName { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}

