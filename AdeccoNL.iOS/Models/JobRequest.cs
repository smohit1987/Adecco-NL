using System;
using System.Collections.Generic;

namespace AdeccoNL
{
	public class JobRequest
	{
		public JobRequest()
		{
		}
		public string JobId { get; set; }
		public string Keyword { get; set; }
		public string Location { get; set; }
		public string SitenameForRegister { get; set; }
		public string CurrentLanguage { get; set; }
		public string BaseAddress { get; set; }
		public string FilterURL { get; set; }
		public string PageNumber { get; set; }
		public string Display { get; set; }
		public string FacetSettingId { get; set; }
		public string IsjobPage { get; set; } = "1";
		public string ClientId { get; set; }
		public string ClientName { get; set; }
		public string BranchId { get; set; }
		public string AsUnAuthenticated { get; set; }
		public string JobURL { get; set; }
		public string JobHost { get; set; }

		public string jobKeyword { get; set; }

		public Dictionary<string, string> AlertData = new Dictionary<string, string>();



	}
}

