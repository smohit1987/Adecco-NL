using System;
using System.Collections.Generic;

namespace AdeccoNL
{
	/// <summary>
	/// Job facets.
	/// </summary>
	public class JobFacets
	{
		public string FacetDisplayName { get; set; }
		public string FacetValueCode { get; set; }
		public List<JobFacetValue> ListFacetValues { get; set; }
	}
	/// <summary>
	/// Job facet value.
	/// </summary>
	public class JobFacetValue
	{
		public string ValueName { get; set; }
		public string ValueCount { get; set; }
		public string ValueChecked { get; set; }
		public string ValueUnChecked { get; set; }

	}
}

