using System;
using System.Collections.Generic;

namespace AdeccoNL
{
	public static class Constants
	{



		//..............Configs Start.................................................

		public static string X_ADECCO_CONTEXT = "SelfServicePortal";
		public static string X_ADECCO_CLIENT_USER_AGENT = "Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 52.0.2743.116 Safari / 537.36";
		public static string X_ADECCO_CORRELATION_ID = "a775d628-be87-40ec-810b-ee3ac9e59773";
		public static string HeaderAccept = "application/json";
		public static string JobSearchUserStatus = "candidate";
		public static string JobCount = "0";
		public static string TotalJobCount = "0";

		public static string JobSearchSession = "01pypyigge22sem4bd5mmjba";

		// =======  Adecco NL CM enviorment ============-----------


		//public static string JobBaseAddress = "http://cm-adecco-nl.adecco.net";

		public static string JobBaseAddress = "http://www.adecco.nl";
		public static string JobDetailCurrentLanguage = "nl-NL";

		public static string JobDetailSiteName = "adecco.nl";
		public static string countryRegion = "NL";
		public static string JobSearchFacetSettingID = "{D444BDE3-4A1D-4C4A-B3AE-D5E9A9B09FD2}";
		public static string AboutUsURL = JobBaseAddress + "/over-adecco";
		public static string ShareURL = JobBaseAddress + "/vacature/";
		public static string isBranchLocator = "1";
		public static string isContact = "0";
		public static string isGoogleLocation = "1";
		public static string TermsConditions = JobBaseAddress + "/terms-of-use";
		public static int DistanceDefault = 10;
		public static int DistanceMax = 50;
		public static string JobSearchFilterURL = JobBaseAddress + "/vacatures?";


		public static string GoogleLocation = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=##LOCATION##&key=AIzaSyAKaf6N484SZUI6BojKztFkZGSC38uvuuw";
		public static string GeoCodeURL = "https://maps.googleapis.com/maps/api/geocode/json";

		public static string SearchLocationBingAPIKey = "Akwj-3zlDya41nzAxgjELKVaSlmv-jML8JLst5cwWzesvL5sQvgynF8nBPzd1esR";
		public static string SearchLocationBingURL = "http://dev.virtualearth.net/REST/v1/Locations";

		public static string SearchLocationGoogleAPIKey = "AIzaSyAKaf6N484SZUI6BojKztFkZGSC38uvuuw";
		public static string SearchLocationGoogleURL = "https://maps.googleapis.com/maps/api/js";


		public static string BranchLocator = JobBaseAddress + "/globalweb/branch/branchsearch";


		public static string SiteDefaultLanguage = "fr-FR";

		public static string FilterURL = "";
		public static string selectedRadius = "&r=10";


		//public static string isFileterApplied = "0";

		public static bool isFileterApplied = false;
		public static bool shouldResetFilter = false;

		public static bool shouldGeoCodeLocation = false;

		public static bool isConigurationChanged = false;

		public static Dictionary<string, dynamic> jobSearchResponse = new Dictionary<string, dynamic>();


		//============================


		//for B&C NL	
		//	public static string JobBaseAddress = "http://www.badenochandclark.nl";
		//	public static string JobDetailCurrentLanguage = "nl-NL";
		//	public static string JobDetailSiteName = "badenochandclark.nl";
		//	public static string countryRegion = "NL";
		//	public static string JobSearchFacetSettingID = "{0B4AD143-A7E1-4090-97C0-5CB0C017253B}";
		//	public static string TermsConditions = JobBaseAddress + "/terms-and-conditions";
		public static string CreateJobAlert = JobBaseAddress + "/AdeccoGroup.Global/api/Job/SaveJobAlert/";
		public static string JobDetailURL = "/AdeccoGroup.Global/api/Job/SearchJobDetailsById/";
		//	public static string JobSearchFilterURL = JobBaseAddress + "/vacatures?";
		public static string JobSearchURL = JobBaseAddress + "/AdeccoGroup.Global/api/Job/AsynchronousJobSearch/";
		public static string JobKeywordURL = "/AdeccoGroup.Global/api/Job/GetKeyword/false";


		// Job Location Keys
		public static string LocationURL = "http://dev.virtualearth.net/REST/v1/Locations?";
		public static string LocationKey = "Akwj-3zlDya41nzAxgjELKVaSlmv-jML8JLst5cwWzesvL5sQvgynF8nBPzd1esR";
		public static string linkedinURL = "https://www.linkedin.com/shareArticle?mini=true&url=##URL##&title=##TITLE##&summary=&source=";
		public static string facebookURL = "https://www.facebook.com/sharer.php?u=##TITLE##";
		public static string twitterURL = "https://twitter.com/intent/tweet?url=##TITLE##";
		public static string googleplusURL = "https://plus.google.com/share?url=##TITLE##";
		public static string contactUs = "http://www.badenochandclark.nl/contact";
		public static string aboutUs = "http://www.badenochandclark.nl/over-ons";

		//..............Configs End.................................................



		//..............Constants Start.................................................
		public static List<JobFacets> lstJobFacets;
		static Constants()
		{
			lstJobFacets = new List<JobFacets>();
		}
		public static string JobKeyword = "";
		public static string JobLocation = "";
		public static string LocationLatLong = "";
		public static JobCMS jobCMSSelectedForJobDetail = null;
		public static string TotalpageNum = "1";
		public static string CurrentpageNum = "1";
		public static string displayCount = "10";
		public static string maxResults = "10";
		public static string latestJobsCount = "10";

		public static string Latitude = "";
		public static string Longitude = "";

		//..............Constants End...................................
	}

	public class JobLocation
	{
		public string type { get; set; }

		public string[] coordinates { get; set; }
	}



}


