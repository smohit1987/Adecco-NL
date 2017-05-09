	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Text;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using System.Linq;

	namespace AdeccoNL.iOS
	{
	public class ServiceManager
	{
		public List<JobLocation> latlon;

		//Dictionary<string, List<string>>


		public async System.Threading.Tasks.Task<Dictionary<string, dynamic>> AsyncJobSearch(JobRequest jobRequest)
		{

			var responseDict = new Dictionary<string, dynamic>();


			List<JobCMS> jobList = new List<JobCMS>();


			var baseAddress = new Uri(Constants.JobBaseAddress);
			var values = new Dictionary<string, string>();
			values.Add("filterUrl", jobRequest.FilterURL);
			values.Add("sfr", Constants.JobDetailSiteName);
			values.Add("facetSettingId", Constants.JobSearchFacetSettingID);
			values.Add("currentLanguage", Constants.JobDetailCurrentLanguage);
			values.Add("IsJobPage", "1");
			values.Add("clientId", "");
			values.Add("clientName", "");
			values.Add("branchId", "");
			var content = new FormUrlEncodedContent(values);
			var cookieContainer = new CookieContainer();


			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;

			HttpClientHandler handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				CookieContainer = cookieContainer
			};


			//using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(handler))
			{
				try
				{
					cookieContainer.Add(baseAddress, new Cookie("sitenameForRegister", Constants.JobDetailSiteName));
					cookieContainer.Add(baseAddress, new Cookie("ASP.NET_SessionId", "01pypyigge22sem4bd5mmjba"));
					cookieContainer.Add(baseAddress, new Cookie("Locale", Constants.JobDetailCurrentLanguage));//ja-JP
					cookieContainer.Add(baseAddress, new Cookie("userstatus", "candidate"));
					cookieContainer.Add(baseAddress, new Cookie("IsJobPage", "1"));

					var httpResponse = await client.PostAsync(Constants.JobSearchURL, content);


					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{

						string responseContent = await httpResponse.Content.ReadAsStringAsync();
						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

						//if(responseObj == null)
						//	return null;

						dynamic jobitems = responseObj["Items"];
						dynamic jobsCount = responseObj["Count"];
						dynamic TotalCount = responseObj["TotalCount"];

						Constants.TotalJobCount = TotalCount.ToObject<string>();
						Constants.JobCount = jobsCount.ToObject<string>();
						JobCMS job = null;
						jobList = new List<JobCMS>();
						foreach (JObject jcontent in jobitems.Children<JObject>())
						{
							job = new JobCMS();
							job = jcontent.ToObject<JobCMS>();

							if (Constants.JobDetailSiteName.Equals("adecco.fr"))
								{
									job.ContractTypeTitle = job.EmploymentTypeTitle;
								}
							jobList.Add(job);
						}

						responseDict.Add("jobList", jobList);

					
						dynamic presentationFacetResults = responseObj["PresentationFacetResults"];
					
						List<PresentationFacetResult> presentationFacetResultList = new List<PresentationFacetResult>();
						PresentationFacetResult _presentationFacetResult = new PresentationFacetResult();

						foreach (JObject jcontent in presentationFacetResults.Children<JObject>())
						{
							//presentationFacetResult.ListFacetValues = new List<FacetValue>();
							_presentationFacetResult = jcontent.ToObject<PresentationFacetResult>();
							presentationFacetResultList.Add(_presentationFacetResult);
						}
						//Console.WriteLine("ListPresentationFacetResult =={0}", presentationFacetResultList);
						responseDict.Add("presentationFacetResultList", presentationFacetResultList);


						dynamic _SelectedFacets = responseObj["SelectedFacets"];
						List<SelectedFacets> selectedFacetsList = new List<SelectedFacets>();
						SelectedFacets aSelectedFacets = new SelectedFacets();

						foreach (JObject jcontent in _SelectedFacets.Children<JObject>())
						{
							aSelectedFacets = jcontent.ToObject<SelectedFacets>();
							selectedFacetsList.Add(aSelectedFacets);
						}

						Console.WriteLine("selectedFacetsList =={0}", selectedFacetsList);
						responseDict.Add("selectedFacetsList", selectedFacetsList);



						return responseDict;
					}
					return responseDict;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());

				}

			}
			return responseDict;
		}

		/*
		  
		 public async System.Threading.Tasks.Task<List<JobCMS>> AsyncJobSearch(JobRequest jobRequest)
		{
			List<JobCMS> jobList = null;


			var baseAddress = new Uri(Constants.JobBaseAddress);
			var values = new Dictionary<string, string>();
			values.Add("filterUrl", jobRequest.FilterURL);
			values.Add("sfr", Constants.JobDetailSiteName);
			values.Add("facetSettingId", Constants.JobSearchFacetSettingID);
			values.Add("currentLanguage", Constants.JobDetailCurrentLanguage);
			values.Add("IsJobPage", "1");
			values.Add("clientId", "");
			values.Add("clientName", "");
			values.Add("branchId", "");
			var content = new FormUrlEncodedContent(values);
			var cookieContainer = new CookieContainer();


			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;


			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(networkHandler))
			{
				try
				{
					cookieContainer.Add(baseAddress, new Cookie("sitenameForRegister", Constants.JobDetailSiteName));
					cookieContainer.Add(baseAddress, new Cookie("ASP.NET_SessionId", "01pypyigge22sem4bd5mmjba"));
					cookieContainer.Add(baseAddress, new Cookie("Locale", Constants.JobDetailCurrentLanguage));//ja-JP
					cookieContainer.Add(baseAddress, new Cookie("userstatus", "candidate"));
					cookieContainer.Add(baseAddress, new Cookie("IsJobPage", "1"));

					var httpResponse = await client.PostAsync(Constants.JobSearchURL, content);


					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{

						string responseContent = await httpResponse.Content.ReadAsStringAsync();

						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

						//if(responseObj == null)
						//	return null;

						dynamic jobitems = responseObj["Items"];
						dynamic jobsCount = responseObj["Count"];
						dynamic TotalCount = responseObj["TotalCount"];

						Constants.TotalJobCount = TotalCount.ToObject<string>();
						Constants.JobCount = jobsCount.ToObject<string>();
						JobCMS job = null;
						jobList = new List<JobCMS>();
						foreach (JObject jcontent in jobitems.Children<JObject>())
						{
							job = new JobCMS();
							job = jcontent.ToObject<JobCMS>();
							jobList.Add(job);
						}

						// facet result.

						dynamic presentationFacetResults = responseObj["PresentationFacetResults"];

						List<FacetValue> listFacetValues = new List<FacetValue>();
						FacetValue facetValue = new FacetValue();

						List<PresentationFacetResult> ListPresentationFacetResult = new List<PresentationFacetResult>();

						PresentationFacetResult _presentationFacetResult = new PresentationFacetResult();

						foreach (JObject jcontent in presentationFacetResults.Children<JObject>())
						{
							//presentationFacetResult.ListFacetValues = new List<FacetValue>();
							_presentationFacetResult = jcontent.ToObject<PresentationFacetResult>();
							ListPresentationFacetResult.Add(_presentationFacetResult);
						}

						Console.WriteLine("ListPresentationFacetResult =={0}",ListPresentationFacetResult);

						return jobList;
					}
					return jobList;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());

				}

			}
			return null;
		}
		*/

		public async System.Threading.Tasks.Task<JobCIS> GetJobDetails(string JobId)
		{

			JobCIS jobDetail = new JobCIS();
			//var baseAddress = new Uri(Constants.JobBaseAddress);
			var cookieContainer = new CookieContainer();

			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;


			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(handler))
			{
				try
				{
					string JobURL = Constants.JobBaseAddress + Constants.JobDetailURL + JobId;
					var httpResponse = await client.GetAsync(JobURL);

					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{

						string responseContent = await httpResponse.Content.ReadAsStringAsync();
						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
						jobDetail = responseObj.ToObject<JobCIS>();

					}

				}
				catch (Exception ex)
				{
					string str = ex.Message;
				}
			}
			return jobDetail;

		}


		public async System.Threading.Tasks.Task<List<string>> AsyncJobGetKeywords(string keyword)
		{
			List<string> lstKeywords = new List<string>();
			var baseAddress = new Uri(Constants.JobBaseAddress);
			var values = new Dictionary<string, string>();
			values.Add("KeywordInput", keyword);

			var content = new FormUrlEncodedContent(values);

			var cookieContainer = new CookieContainer();

			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;


			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(handler))
			{
				try
				{
					//====

					//cookieContainer.Add(baseAddress, new Cookie("sitenameForRegister", Constants.JobDetailSiteName));
					//cookieContainer.Add(baseAddress, new Cookie("ASP.NET_SessionId", Constants.JobSearchSession));
					cookieContainer.Add(baseAddress, new Cookie("Locale", Constants.JobDetailCurrentLanguage));
					//cookieContainer.Add(baseAddress, new Cookie("userstatus", Constants.JobSearchUserStatus));
					//cookieContainer.Add(baseAddress, new Cookie("IsJobPage", jobRequest.IsjobPage));
					//======
					//cookieContainer.Add(baseAddress, new Cookie("sitenameForRegister", Constants.JobDetailSiteName));

					var httpResponse = await client.PostAsync(Constants.JobBaseAddress + Constants.JobKeywordURL, content);

					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{

						string responseContent = await httpResponse.Content.ReadAsStringAsync();

						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
						lstKeywords = responseObj.ToObject<List<string>>();
					}

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					throw ex;
				}

			}
			return lstKeywords;


		}


		public async System.Threading.Tasks.Task<List<string>> AsyncJobGetLocations(string location)
		{
			List<string> lstLocations = new List<string>();

			DbHelper.JobLocations = new List<JobLocation>();
			//var baseAddress = new Uri(Constants.JobBaseAddress);
			var cookieContainer = new CookieContainer();

			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;


			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(handler))
			{
				try
				{
					string locationURL = Constants.LocationURL + "key=" + Constants.LocationKey + "&maxResults=" + Constants.maxResults + "&countryRegion=" + Constants.countryRegion + "&c=" + Constants.JobDetailCurrentLanguage + "&locality=" + location;

					string URL = Constants.GoogleLocation.Replace("##LOCATION##", location);


					HttpResponseMessage httpResponse = new HttpResponseMessage();

					if (Constants.isGoogleLocation.Equals("1"))
						httpResponse = await client.GetAsync(URL);
					else
						httpResponse = await client.GetAsync(locationURL);

					// if we are using google locaton api then 
					if (Constants.isGoogleLocation.Equals("1") && (httpResponse.StatusCode == HttpStatusCode.OK))
					{
						string responseContent = await httpResponse.Content.ReadAsStringAsync();
						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
						dynamic predictions = responseObj["predictions"];
						//dynamic resourceList = resourceSets[0]["resources"];

						foreach (JObject resource in predictions.Children<JObject>())
						{
							foreach (JProperty property in resource.Properties())
							{
								if (property.Name == "description")
								{
									lstLocations.Add(property.Value.ToString());
								}
								if (property.Name == "point")
								{
									JobLocation obj = property.Value.ToObject<JobLocation>();
									DbHelper.JobLocations.Add(obj);
								}

							}
						}
					}

					else if (httpResponse.StatusCode == HttpStatusCode.OK)
					{

						string responseContent = await httpResponse.Content.ReadAsStringAsync();
						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
						dynamic resourceSets = responseObj["resourceSets"];
						dynamic resourceList = resourceSets[0]["resources"];

						foreach (JObject resource in resourceList.Children<JObject>())
						{
							foreach (JProperty property in resource.Properties())
							{
								if (property.Name == "name")
								{
									lstLocations.Add(property.Value.ToString());
								}
								if (property.Name == "point")
								{
									JobLocation obj = property.Value.ToObject<JobLocation>();
									DbHelper.JobLocations.Add(obj);
								}

							}
						}
					}



				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());

				}
			}
			return lstLocations;

		}


		public async System.Threading.Tasks.Task<string> AsyncGetUserCurrentLocations(string lattitude, string longitude)
		{
			string Location = "";
			var cookieContainer = new CookieContainer();

			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;

			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(handler))
			{
				try
				{
					//string URL = "https://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224,-73.961452&key=AIzaSyCORhhTXg24bQ6cmktMhXRHlB9D1AiYef4";


					string urlString = String.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key=AIzaSyCORhhTXg24bQ6cmktMhXRHlB9D1AiYef4", lattitude, longitude);



					HttpResponseMessage httpResponse = new HttpResponseMessage();
					httpResponse = await client.GetAsync(urlString);

					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{
						string responseContent = await httpResponse.Content.ReadAsStringAsync();
						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
						dynamic results = responseObj["results"];
						dynamic address = results[0];
						dynamic formatted_address = address["formatted_address"];
						dynamic geometry = address["geometry"];
						dynamic location =  geometry["location"];
						dynamic lat = location["lat"];
						dynamic lng = location["lng"];


						Constants.LocationLatLong = lat.ToObject<string>() +"%2C" + lng.ToObject<string>();

						Console.WriteLine(Constants.LocationLatLong);

						Location = formatted_address.ToObject<string>();

						return Location;

					}
					else
					{
						Console.WriteLine(httpResponse.StatusCode.ToString());

					}

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
			return Location;

		}


		public async System.Threading.Tasks.Task<string> AsyncGeocodeLocation(string _address)
		{
			string Location = "";
			var cookieContainer = new CookieContainer();

			CFNetworkHandler networkHandler = new CFNetworkHandler();
			networkHandler.UseSystemProxy = true;
			networkHandler.CookieContainer = cookieContainer;

			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
			using (var client = new HttpClient(handler))
			{
				try
				{
					//string URL = "https://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224,-73.961452&key=AIzaSyCORhhTXg24bQ6cmktMhXRHlB9D1AiYef4";


					string urlString = String.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}",_address);
					//https://maps.googleapis.com/maps/api/geocode/json

					//var sb = new StringBuilder();
					//sb.Append(Constants.GeoCodeURL);
					//sb.Append("?address=").Append(_address);

					HttpResponseMessage httpResponse = new HttpResponseMessage();
					httpResponse = await client.GetAsync(urlString);

					if (httpResponse.StatusCode == HttpStatusCode.OK)
					{
						string responseContent = await httpResponse.Content.ReadAsStringAsync();
						dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
						dynamic results = responseObj["results"];
						dynamic address = results[0];
						dynamic geometry = address["geometry"];
						dynamic location = geometry["location"];
						dynamic lat = location["lat"];
						dynamic lng = location["lng"];


						Constants.LocationLatLong = lat.ToObject<string>() + "%2C" + lng.ToObject<string>();
						Constants.Latitude = lat.ToObject<string>();
						Constants.Longitude = lng.ToObject<string>();

						Console.WriteLine(Constants.LocationLatLong);

						Location = lat.ToObject<string>() + "%2C" + lng.ToObject<string>();

						return Location;

					}
					else
					{
						Console.WriteLine(httpResponse.StatusCode.ToString());

					}

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
			return Location;

		}


		/// <summary>
		/// Posts the values.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="jobRequest">Job request.</param>
		public async System.Threading.Tasks.Task<string> AsyncCreateJobAlert(JobRequest jobRequest)
		{
			string JobAlertIdValue = "";

			var baseAddress = new Uri(Constants.JobBaseAddress);
			string filterurl = jobRequest.FilterURL;
			if (!Constants.LocationLatLong.Equals("") && jobRequest.Location.Contains("%2C"))
			{
				filterurl = filterurl + "&xy=" + Constants.LocationLatLong;
			}

			try
			{

				jobRequest.FacetSettingId = Constants.JobSearchFacetSettingID;
				jobRequest.BaseAddress = Constants.JobBaseAddress;

				var cookieContainer = new CookieContainer();
				cookieContainer.Add(baseAddress, new Cookie("sitenameForRegister", Constants.JobDetailSiteName));
				cookieContainer.Add(baseAddress, new Cookie("ASP.NET_SessionId", Constants.JobSearchSession));
				cookieContainer.Add(baseAddress, new Cookie("Locale", Constants.JobDetailCurrentLanguage));
				cookieContainer.Add(baseAddress, new Cookie("JobsBrowsebyCategoryURL", "http://www.badenochandclark.nl/vacatures?k=account&l=&pageNum=1&display=5"));
				cookieContainer.Add(baseAddress, new Cookie("SearchResultPage", "http://www.badenochandclark.nl/vacatures?k=account&l=&pageNum=1&display=5"));
				cookieContainer.Add(baseAddress, new Cookie("IsJobPage", "0"));

				CFNetworkHandler networkHandler = new CFNetworkHandler();
				networkHandler.UseSystemProxy = true;
				networkHandler.CookieContainer = cookieContainer;


				if (!Constants.LocationLatLong.Equals("") && jobRequest.Location.Contains("%2C"))
				{
					filterurl = filterurl + "&xy=" + Constants.LocationLatLong;
				}


				string result2 = "{ " + jobRequest.AlertData.Aggregate(new StringBuilder(),
			   (a, b) => a.Append(", ").Append("\"").Append(b.Key).Append("\"").Append(": ").Append("\"").Append(b.Value).Append("\""),
			   (a) => a.Remove(0, 2).ToString()) + " }";

				string data2 = "{\"filterUrl\":" + "\"" + filterurl + "\"" + "," + "\"facetSettingId\":" + "\"" + Constants.JobSearchFacetSettingID + "\"" + "," + "\"currentLanguage\":" + "\"" + Constants.JobDetailCurrentLanguage + "\"" + "," + "\"AlertData\":" + result2 + "}";

				var _content = new StringContent(data2, Encoding.UTF8, "application/json");
				using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })

				using (var client = new HttpClient(handler))
				{
					try
					{

						var httpResponse = await client.PostAsync(Constants.CreateJobAlert, _content);

						if (httpResponse.StatusCode == HttpStatusCode.OK)
						{

							string responseContent = await httpResponse.Content.ReadAsStringAsync();

							dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
							dynamic JobAlertId = responseObj["JobAlertId"];
							JobAlertIdValue = JobAlertId.ToObject<string>();

							return JobAlertIdValue;


						}
					}
					catch (Exception ex)
					{
						string error = ex.Message;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());

			}

			return JobAlertIdValue;
		}

		/// <summary>
		/// Posts the values.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="jobRequest">Job request.</param>
			public async System.Threading.Tasks.Task<List<Branch>> AsyncBranchLocator(BranchRequest BranchRequest)
			{
				List<Branch> branchList = new List<Branch>();

				try
				{

					var baseAddress = new Uri(Constants.JobBaseAddress);
					var cookieContainer = new CookieContainer();
					cookieContainer.Add(baseAddress, new Cookie("IsJobPage", "0"));

					CFNetworkHandler networkHandler = new CFNetworkHandler();
					networkHandler.UseSystemProxy = true;
					networkHandler.CookieContainer = cookieContainer;


					string data = "{\"dto\":{\"Latitude\":" + "\"" + BranchRequest.Latitude + "\"" + "," + "\"Longitude\":" + "\"" + BranchRequest.Longitude + "\"" + "," + "\"MaxResults\":" + "\"" + BranchRequest.MaxResults + "\"" + "," + "\"Radius\":" + "\"" + BranchRequest.Radius + "\"" + "," + "\"Industry\":" + "\"" + BranchRequest.Industry + "\"" + "," + "\"RadiusUnits\":" + "\"" + BranchRequest.RadiusUnits + "\"" + "}}";
					var _content = new StringContent(data, Encoding.UTF8, "application/json");
					using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })

				using (var client = new HttpClient(handler))
					{
						try
						{

						var httpResponse = await client.PostAsync(Constants.BranchLocator, _content);

							if (httpResponse.StatusCode == HttpStatusCode.OK)
							{

								string responseContent = await httpResponse.Content.ReadAsStringAsync();

								dynamic responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);


								foreach (JObject jcontent in responseObj.Children<JObject>())
								{
									Branch branch = new Branch();
									branch = jcontent.ToObject<Branch>();
									branchList.Add(branch);

								}
								return branchList;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.ToString());
						}
					}
				}
			  catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}

				return branchList;
			}
				
	}
	}

