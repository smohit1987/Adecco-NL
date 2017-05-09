using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace AdeccoNL.iOS
{
	public class Branch
	{
		
		[PrimaryKey]
		public string Id { get; set; }
		public string Address { get; set; }

		public string BranchCode { get; set; }
		public string BranchEmail { get; set; }
		public string BranchName { get; set; }

		public string City { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string Distance { get; set; }
		public string ItemUrl { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string Name { get; set; }

		public string State { get; set; }
		public string PhoneNumber { get; set; }
		public string Updated { get; set; }
		public string ZipCode { get; set; }

	}

	public class BranchRequest
	{
		public BranchRequest()
		{
		}
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string MaxResults { get; set; }
		public string Radius { get; set; }
		public string Industry { get; set; }
		public string RadiusUnits { get; set; }

	}

}
