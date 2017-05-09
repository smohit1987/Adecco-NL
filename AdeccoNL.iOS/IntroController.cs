using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using AdeccoNL;
using BigTed;
using SQLite.Net;

using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using CoreGraphics;
using Google.Analytics;
using Newtonsoft.Json.Linq;

namespace AdeccoNL.iOS
	{
		partial class IntroController : BaseController
		{
		
		public bool isRecentSearch = false;
		public bool searchJobButtonPressed = false;
		private List<JobCMS> jobList;
		private List<RecentSearch> _recentSearchs;
		private string _pathToDatabase;

		private static bool _searchingLocation = false;

		#region Computed Properties

		public static LocationManager Manager { get; set; }

		#endregion

		#region Constructors
		public IntroController(IntPtr handle) : base(handle)
			{
				
			}
		#endregion
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if(jobList !=null)
				tblView.ReloadData();

			View.EndEditing(true);
			this.FavoriteButtonWithCount(NavigationItem);
			tblView.AllowsSelection = true;

			AutoCompleteTextFieldManager.RemoveTable();
			AutoCompleteTextFieldManager.RemoveAll();


			// clear filetr url used for refine 
			Constants.FilterURL = "";
			Constants.jobSearchResponse = new Dictionary<string, dynamic>();

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.SidebarController.Disabled = false;


			// Now Get latest jobs if config file changed
			if (Constants.isConigurationChanged)
			{
					this.txtKeyword.Text = "";
					this.txtLocation.Text = "";

					btnJobSearch.SetTitle(Translations.job_search, UIControlState.Normal); 					btnJobSearch.SetTitle(Translations.job_search, UIControlState.Highlighted); 
					segmentCtrl.RemoveAllSegments();
					segmentCtrl.InsertSegment(Translations.latest_job, 0, false); 					segmentCtrl.InsertSegment(Translations.recent_search, 1, false); 					segmentCtrl.SelectedSegment = 0; 
					this.txtKeyword.AttributedPlaceholder = new NSAttributedString( 										Translations.keyword_title, 								font: UIFont.SystemFontOfSize(15), 								foregroundColor: UIColor.White, 								strokeWidth: 0 						);  					this.txtLocation.AttributedPlaceholder = new NSAttributedString( 					Translations.location_title, 								font: UIFont.SystemFontOfSize(15), 								foregroundColor: UIColor.White, 								strokeWidth: 0 						); 
				DbHelper.ResetDatabase();
				DbHelper.CreateDatabaseAndTables();

				_recentSearchs = DbHelper.GetRecentSearches();

                 this.FavoriteButtonWithCount(NavigationItem);

                GetJobSearchData("", "");

				Constants.isConigurationChanged = false;

			}
				
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			// This screen name value will remain set on the tracker and sent with
			// hits until it is set to a new value or to null.
			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Home");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());

		}


		partial void BtnCurrentLoaction_TouchUpInside(UIButton sender)
		{
			_searchingLocation = false;
			// As soon as the app is done launching, begin generating location updates in the location manager
			if(Manager == null)
				Manager = new LocationManager();

			Manager.StartLocationUpdates();

			// It is better to handle this with notifications, so that the UI updates
			// resume when the application re-enters the foreground!
			Manager.LocationUpdated += HandleLocationChanged;
		}

		// Methopd will load app basic configuration from Sitecore 
		private  void LoadAppConfiguration()
		{
			JObject elements = JObject.Parse(File.ReadAllText("Config.json"));
			//Console.WriteLine(elements);
					
			ConfigManager.MaxSearchCount = elements["MaxSearchCount"].ToObject<string>();
			ConfigManager.DefaultRadius = elements["DefaultRadius"].ToObject<string>();
			ConfigManager.EnableAutoSuggest = elements["EnableAutoSuggest"].ToObject<bool>();
			ConfigManager.SearchLocationGoogleURL = elements["SearchLocationGoogleURL"].ToObject<string>();


			ConfigManager.MaxShortListCount = elements["MaxShortListCount"].ToObject<string>();
			ConfigManager.FacetSettingId = elements["FacetSettingId"].ToObject<string>();
			ConfigManager.BLMaxResultCount = elements["BLMaxResultCount"].ToObject<string>();
			ConfigManager.EnableBranchSearch = elements["EnableBranchSearch"].ToObject<string>();
			ConfigManager.SearchLocationAPIName = elements["SearchLocationAPIName"].ToObject<string>();


			ConfigManager.SearchLocationBingAPIKey = elements["SearchLocationBingAPIKey"].ToObject<string>();
			ConfigManager.BLResultSortingOrder = elements["BLResultSortingOrder"].ToObject<string>();
			ConfigManager.SiteDefaultLanguage = elements["SiteDefaultLanguage"].ToObject<string>();

			ConfigManager.JobSnippetFieldList = elements["JobSnippetFieldList"].ToObject<List<string>>();
			ConfigManager.JobDetailFieldList = elements["JobDetailFieldList"].ToObject<List<string>>();
			ConfigManager.SocialShare = elements["SocialShare"].ToObject<List<string>>();
			ConfigManager.SiteLanguages = elements["SiteLanguages"].ToObject<List<string>>();
			ConfigManager.BLDistance = elements["BLDistance"].ToObject<List<string>>();


			Console.WriteLine(ConfigManager.JobSnippetFieldList);

			// Changes Constant file here.
			Constants.JobSearchFacetSettingID = ConfigManager.FacetSettingId;

			// chose max number of result to be shown 
			int maxSearchCount = Convert.ToInt32(ConfigManager.MaxSearchCount);
			ConfigManager.MaxSearchCount = Math.Max(maxSearchCount, 10).ToString();

			Constants.displayCount = ConfigManager.MaxSearchCount;
			Constants.maxResults = ConfigManager.MaxSearchCount;
			Constants.latestJobsCount = ConfigManager.MaxSearchCount;


			// SearchLocation Api - GOOGLE/BING API 
			if (ConfigManager.SearchLocationAPIName.Equals("Google"))
				Constants.isGoogleLocation = "1";
			else				
				Constants.isGoogleLocation = "0";



			Constants.DistanceDefault = Convert.ToInt32(ConfigManager.DefaultRadius);


				//					if (Constants.isGoogleLocation.Equals("1"))



				//public static string displayCount = "10";
				//public static string maxResults = "10";
				//public static string latestJobsCount = "10";


				//
				/*
	JobSnippetFieldList
	JobDetailFieldList
	SocialShare
	SiteLanguages
	BLDistance
	*/


				return;
		}

		#region Public Methods
		public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
		{
			// Handle foreground updates
			Console.WriteLine("foreground updated");


			CLLocation location = e.Location;



			if (!_searchingLocation)
			{
				_searchingLocation = true;
				AsyncGetUserCurrentLocations(e);
			}

			Manager.StopLocationUpdates();
			//Manager = null;

		}
		public async void AsyncGetUserCurrentLocations(LocationUpdatedEventArgs e)
		{
			NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();
			if (remoteHostStatus == NetworkStatus.NotReachable)
			{

				var alert = UIAlertController.Create("Network Error", "Please check your internet connection", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				PresentViewController(alert, animated: true, completionHandler: null);

				return;
			}

				CLLocation location = e.Location;

				ServiceManager jobService = new ServiceManager();
				string currentLocation = await jobService.AsyncGetUserCurrentLocations(location.Coordinate.Latitude.ToString(), location.Coordinate.Longitude.ToString());

				Console.WriteLine("location :=  " + currentLocation);

				if (string.IsNullOrEmpty(currentLocation))
				{
					BTProgressHUD.ShowToast("Unable to track location.Please try later...", false, 3000);

				}
				else
					txtLocation.Text = currentLocation;


		}
		#endregion

		public override void ViewDidLoad()
			{

				base.ViewDidLoad();


				// Figure out where the SQLite database will be.
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				_pathToDatabase = Path.Combine(documents, "db_sqlite-net.db");
				SQLiteConnection dbConnection = DbHelper.GetConnection(_pathToDatabase,new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS());	
				//DbHelper.CreateDatabaseAndTables();


				 _recentSearchs = DbHelper.GetRecentSearches();

				this.txtKeyword.Text = "";
				this.txtLocation.Text = "";

				jobList = new List<JobCMS>();
				tblView.AllowsSelection = true;


				activityIndicatorKeyword.Hidden = true;
				activityIndicatorLocation.Hidden = true;

			// first load app basic configuration settings 
				LoadAppConfiguration();
				
				// Now Get latest jobs 
				GetJobSearchData("", "");

				// Perform any additional setup after loading the view, typically from a nib.
				btnJobSearch.BackgroundColor = UIColor.Red; //UIColor.Clear.FromHexString("#ef2e24", 1.0f);
				segmentCtrl.BackgroundColor = UIColor.White;
				searchBg.TintColor = UIColor.Red;

				UIImage logo = UIImage.FromBundle("adecco-logo-white");
				UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(0, 0, 80, 20)); //244 × 60
				imageView.Image = logo;
				NavigationItem.TitleView = imageView;



				//this.NavigationController.NavigationBar.BarTintColor = UIColor.Clear.FromHexString("#ef2e24", 1.0f);
				this.NavigationController.NavigationBar.BarTintColor = UIColor.Red;



					this.txtKeyword.LeftViewMode = UITextFieldViewMode.Always;
					this.txtKeyword.LeftView = new UIView(new RectangleF(0, 0, 35, 35)); //imageViewKeyword;

					this.txtLocation.LeftViewMode = UITextFieldViewMode.Always;
					this.txtLocation.LeftView = new UIView(new RectangleF(0, 0, 35, 35)); //imageViewLocation;


					this.txtKeyword.AttributedPlaceholder = new NSAttributedString(
					Translations.keyword_title,
								font: UIFont.SystemFontOfSize(15),
								foregroundColor: UIColor.White,
								strokeWidth: 0
						);


					this.txtLocation.AttributedPlaceholder = new NSAttributedString(
					Translations.location_title,
								font: UIFont.SystemFontOfSize(15),
								foregroundColor: UIColor.White,
								strokeWidth: 0
						);

				
					this.txtLocation.RightViewMode = UITextFieldViewMode.Always;
					this.txtLocation.RightView = new UIView(new RectangleF(0, 0, 35, 35));

				var g = new UITapGestureRecognizer(() => View.EndEditing(true));
				View.AddGestureRecognizer(g);

			btnJobSearch.SetTitle(Translations.job_search, UIControlState.Normal);
			btnJobSearch.SetTitle(Translations.job_search, UIControlState.Highlighted);


			btnJobSearch.TouchUpInside += JobSearchButtonPressed;
			segmentCtrl.ValueChanged += (sender, e) =>
				{
					if (segmentCtrl.SelectedSegment == 0)
					{
						isRecentSearch = false;

					if (jobList == null)
							jobList = new List<JobCMS>();
					
					if(jobList.Count<1)
						BTProgressHUD.ShowToast("No record found.", false, 3000);
					}
						
					else
					{

						if (_recentSearchs == null)
							_recentSearchs = new List<RecentSearch>();
					
					if (_recentSearchs.Count < 1)
							BTProgressHUD.ShowToast("No record found.", false, 3000);
						isRecentSearch = true;
					}
						

					tblView.Source = new TableSource(jobList, _recentSearchs, isRecentSearch,this);

					tblView.ReloadData();

					tblView.TableFooterView = new UIView();


				};


			txtKeyword.ShouldReturn += TextFieldShouldReturn;
			txtLocation.ShouldReturn += TextFieldShouldReturn;

			txtKeyword.ShouldClear += TextFieldShoulClear;
			txtLocation.ShouldClear += TextFieldShoulClear;



			// Check if auto suggestion is enabled then add text changed notification 
			// else do nothing not

			if (ConfigManager.EnableAutoSuggest)
				NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification, TextFieldTextDidChangeNotification);

			this.View.UserInteractionEnabled = true;
			tblView.UserInteractionEnabled = true;
			this.View.BringSubviewToFront(tblView);


			segmentCtrl.Layer.BorderColor = UIColor.Red.CGColor;
			segmentCtrl.Layer.BorderWidth = 0.5f;
			segmentCtrl.Layer.CornerRadius = 5.0f;
			segmentCtrl.Layer.MasksToBounds = true;

			segmentCtrl.RemoveAllSegments();
			segmentCtrl.InsertSegment(Translations.latest_job, 0, false);
			segmentCtrl.InsertSegment(Translations.recent_search, 1, false);
			segmentCtrl.SelectedSegment = 0;

		}



		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			UITouch touch = touches.AnyObject as UITouch;
			if (touch.View == View)
			{
				txtKeyword.ResignFirstResponder();
				txtLocation.ResignFirstResponder();
				AutoCompleteTextFieldManager.RemoveTable();
				txtKeyword.TouchDown += delegate
				{
					AutoCompleteTextFieldManager.RemoveTable();
				};

				txtLocation.TouchDown += delegate
				{
					AutoCompleteTextFieldManager.RemoveTable();
				};

			}
		}

		void TextFieldTextDidChangeNotification(NSNotification notification)
		{

			UITextField txtField = (UITextField)notification.Object;
			string txtMsg = txtField.Text.Trim();

			if(txtMsg.Length > 2)
				this.GetAutosuggestResult(txtField);

		}

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		async void GetAutosuggestResult(UITextField textField)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
			NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();
			if (remoteHostStatus == NetworkStatus.NotReachable)
			{

				var alert = UIAlertController.Create("Network Error", "Please check your internet connection", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				PresentViewController(alert, animated: true, completionHandler: null);

				return;
			}

			ServiceManager jobService = new ServiceManager();

			if (textField == txtKeyword)
			{
				activityIndicatorKeyword.StartAnimating();

				List<string> keywords = await jobService.AsyncJobGetKeywords(textField.Text.Trim());

				if (keywords != null)
				{
					if (keywords.Count == 0)
						keywords.Add("geen resultaten gevoden");

					AutoCompleteTextFieldManager.Add(this, textField, keywords);
					activityIndicatorKeyword.StopAnimating();

				}

			}
			else if (textField == txtLocation)
			{
				//Constants.LocationLatLong = "";

				activityIndicatorLocation.StartAnimating();

				List<string> locations = await jobService.AsyncJobGetLocations(textField.Text.Trim());

				if (locations != null)
				{
					if (locations.Count == 0)
						locations.Add("geen resultaten gevoden");
					
					AutoCompleteTextFieldManager.Add(this, textField, locations);
					activityIndicatorLocation.StopAnimating();

				}

			}

		}


		private bool TextFieldShouldReturn(UITextField textfield)
		{
			int nextTag = (int)textfield.Tag + 1;
			UIResponder nextResponder = this.View.ViewWithTag(nextTag);
			if (nextResponder != null)
			{
				nextResponder.BecomeFirstResponder();
				AutoCompleteTextFieldManager.RemoveTable();

			}
			else {
				// Not found, so remove keyboard.
				textfield.ResignFirstResponder();
			}

			return false; // We do not want UITextField to insert line-breaks.
		}

		private bool TextFieldShoulClear(UITextField textfield)
		{
			//if(textfield == txtKeyword)
			//	AutoCompleteTextFieldManager.RemoveTable(1, 0);

			//if (textfield == txtLocation)
			//	AutoCompleteTextFieldManager.RemoveTable(2, 1);

			AutoCompleteTextFieldManager.RemoveTable();
			AutoCompleteTextFieldManager.RemoveAll();

			return true;
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			AutoCompleteTextFieldManager.RemoveTable();

			//AutoCompleteTextFieldManager.RemoveAll();

		}

		void JobSearchButtonPressed(object sender, EventArgs ea)
		{
			NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();

			if (remoteHostStatus == NetworkStatus.NotReachable)
			{

				var alert = UIAlertController.Create("Network Error", "Please check your internet connection", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				PresentViewController(alert, animated: true, completionHandler: null);

				return;
			}

			View.EndEditing(true);

			string location = this.txtLocation.Text.Trim();

			searchJobButtonPressed = true;

			if (!string.IsNullOrEmpty(location))
			{
				Constants.shouldGeoCodeLocation = true;

				// geocode location first then search job listing 
				this.geoCodeLocation(this.txtLocation.Text.Trim());
			}
			else
			{

					// Add recent search in local DB.
				this.AddRecentSearchData();

				// Get job search result based on input keyword and location 
				GetJobSearchData(this.txtKeyword.Text.Trim(), this.txtLocation.Text.Trim());
			}

		}

		#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
			private async void geoCodeLocation(string location)
		#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void

		{
			NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();
			if (remoteHostStatus == NetworkStatus.NotReachable)
			{

				var alert = UIAlertController.Create("Network Error", "Please check your internet connection", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				PresentViewController(alert, animated: true, completionHandler: null);

				return;
			}

			BTProgressHUD.Show("Loading...", -1, ProgressHUD.MaskType.Black);

			//AsyncGeocodeLocation
			ServiceManager jobService = new ServiceManager();

			string latLong = await jobService.AsyncGeocodeLocation(location);

			Console.WriteLine(latLong);

			if (!string.IsNullOrEmpty(latLong))
			{
				Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent("Job Search", "JobSearch_button_press", "AppEvent", null).Build());
				Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately // just for demonstration // not much recommended 


				// Add recent search in local DB.
				this.AddRecentSearchData();

				// Get job search result based on input keyword and location 
				GetJobSearchData(this.txtKeyword.Text.Trim(), this.txtLocation.Text.Trim());
			}
			else
				BTProgressHUD.Dismiss();
		}


		void AddRecentSearchData()
		{
			string keyword = this.txtKeyword.Text.Trim();
			string location = this.txtLocation.Text.Trim();

			if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(location))
			{
				// Blank search so Don't save search 
				// do nothing 
			}
			else
			{
				if (string.IsNullOrEmpty(keyword))
					keyword = "N/A";

				if (string.IsNullOrEmpty(location))
					location = "N/A";


				// Add search into local data base
				RecentSearch _aRecentSearch = new RecentSearch { Keyword = keyword, Location = location, LocationLatLong = Constants.LocationLatLong };
				DbHelper.AddRecentSearch(_aRecentSearch);


				// Refresh the listing.
				_recentSearchs = DbHelper.GetRecentSearches();

				tblView.Source = new TableSource(jobList, _recentSearchs, isRecentSearch, this);
				tblView.ReloadData();
			}

		}

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		private async void GetJobSearchData(string keyword, string location)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{

			NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();
			if (remoteHostStatus == NetworkStatus.NotReachable)
			{

				var alert = UIAlertController.Create("Network Error", "Please check your internet connection", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				PresentViewController(alert, animated: true, completionHandler: null);

				return;
			}
			//var _JobAlertVC = (JobAlertVC)Storyboard.InstantiateViewController("JobAlertVC");
			//NavController.PushViewController(_JobAlertVC, true);

			//return;

			//location = "nederland";
			//Constants.LocationLatLong = "";

			if (keyword.Equals("N/A"))
				keyword = "";

			if (location.Equals("N/A"))
				location = "";

			Constants.JobKeyword = keyword.Trim();
			Constants.JobLocation = location.Trim();



			location = System.Net.WebUtility.UrlEncode(location);
			keyword = System.Net.WebUtility.UrlEncode(keyword);


			BTProgressHUD.Show("Loading...", -1, ProgressHUD.MaskType.Black);


			JobRequest jobRequest = new JobRequest();
			jobRequest.Keyword = "";
			jobRequest.Location = "";

			//jobRequest.CurrentLanguage = "nl-NL";
			//jobRequest.SitenameForRegister = Constants.JobDetailSiteName;

			jobRequest.CurrentLanguage = Constants.JobDetailCurrentLanguage;
			jobRequest.SitenameForRegister = Constants.JobDetailSiteName;


			jobRequest.FilterURL = Constants.JobSearchFilterURL + "pageNum=" + Constants.CurrentpageNum + "&display=" + Constants.displayCount + "&k=" + keyword + "&l=" + location;

			if (!Constants.LocationLatLong.Equals("") && location.Contains("%2C"))
			{
				jobRequest.FilterURL = jobRequest.FilterURL + "&xy=" + Constants.LocationLatLong;
			}


			jobRequest.FacetSettingId = Constants.JobSearchFacetSettingID;
			jobRequest.BaseAddress = Constants.JobBaseAddress;


			List<JobCMS> searchResults = new List<JobCMS>();

			ServiceManager jobService = new ServiceManager();

			Dictionary<string, dynamic> serverResponse = await jobService.AsyncJobSearch(jobRequest);


			if (searchJobButtonPressed)
			{
				if (serverResponse.ContainsKey("jobList"))
					searchResults = serverResponse["jobList"];
			}
			else
			{
				if (serverResponse.ContainsKey("jobList"))
					jobList = serverResponse["jobList"];
			}

			
			BTProgressHUD.Dismiss();


			if (jobList == null)
			{
				BTProgressHUD.ShowToast("No record found.", false, 3000);
				return;
			}

			if (searchResults == null)
			{
				BTProgressHUD.ShowToast("No record found.", false, 3000);
				return;
			}

			if (jobList == null)
				jobList = new List<JobCMS>();
			
			if (searchJobButtonPressed)
			{
				if (searchResults == null)
					searchResults = new List<JobCMS>();
				
				var searchResultVC = (SearchResultVC)Storyboard.InstantiateViewController("SearchResultVC");
				searchResultVC.jobList = searchResults;
				searchResultVC.isFavoriteJob = false;
				searchResultVC._keyword =  this.txtKeyword.Text.Trim();
				searchResultVC._location = this.txtLocation.Text.Trim();
				searchResultVC.LocationLatLong = Constants.LocationLatLong;
				searchResultVC.serverResponse = serverResponse;
				searchResultVC.aJobRequest = jobRequest;
				NavController.PushViewController(searchResultVC, true);
				searchJobButtonPressed = false;
			}
			else
			{
				tblView.Source = new TableSource(jobList, _recentSearchs, isRecentSearch,this);
				//Delegate = new TableViewDelegate(list);
				//tblView.Delegate = new TableViewDelegate(jobList);
				//tblView.WeakDelegate = this;
				tblView.ReloadData();

			}

		
			tblView.AllowsSelection = true;
			tblView.UserInteractionEnabled = true;


		}

	void FavoriteButtonWithCount(UINavigationItem  navItem)
			{

			List<JobCMS> favJobList = DbHelper.GetFavoriteJobs();
			int favCount = 0;

			if (favJobList != null)
				favCount = favJobList.Count;
			

			UIButton btn = new UIButton(UIButtonType.Custom);
			btn.Frame = new RectangleF(0, 0, 40, 40);
			btn.TouchUpInside += FavoriteButtonPressed;
					
			UILabel lbl = new UILabel();
			lbl.Frame = new RectangleF(19, 0, 20, 20);
			lbl.Text = favCount.ToString();
			lbl.TextAlignment = UITextAlignment.Center;
			lbl.TextColor = UIColor.Red;
			//lbl.Font = UIFont.SystemFontOfSize(14);

			if (favCount < 10)
			{
				lbl.Font = UIFont.BoldSystemFontOfSize(14);
			}

			else
			{
				lbl.Font = UIFont.BoldSystemFontOfSize(12);
			}

			if (favCount == 0)
				btn.SetImage(UIImage.FromBundle("favicon-unselected"), UIControlState.Normal);
			else
			{

				btn.SetImage(UIImage.FromBundle("Badge"), UIControlState.Normal);
				btn.AddSubview(lbl);
			}
			btn.ShowsTouchWhenHighlighted = true;
			UIBarButtonItem barButton2 = new UIBarButtonItem(btn);
			navItem.RightBarButtonItem = barButton2;

			IntroController.favAnimation(this, lbl, true, 0.3f, null);

		}

		static void favAnimation(IntroController instance, UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
		{
			var minAlpha = (nfloat)0.0f;
			var maxAlpha = (nfloat)1.0f;
			var minTransform = CGAffineTransform.MakeScale((nfloat)0.1, (nfloat)0.1);
			var maxTransform = CGAffineTransform.MakeScale((nfloat)1, (nfloat)1);

			view.Alpha = isIn ? minAlpha : maxAlpha;
			view.Transform = isIn ? minTransform : maxTransform;
			UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
				() =>
				{
					view.Alpha = isIn ? maxAlpha : minAlpha;
					view.Transform = isIn ? maxTransform : minTransform;
				},
				onFinished
			);
		}

		void FavoriteButtonPressed(object sender, EventArgs ea)
		{
			var searchResultVC = (SearchResultVC)Storyboard.InstantiateViewController("SearchResultVC");
			searchResultVC.jobList = DbHelper.GetFavoriteJobs();
			searchResultVC.isFavoriteJob = true;
			NavController.PushViewController(searchResultVC, true);
		}

		public override void DidReceiveMemoryWarning()
			{
				base.DidReceiveMemoryWarning();
				// Release any cached data, images, etc that aren't in use.
			}

			public class TableSource : UITableViewSource
			{

			public bool isRecentSearch;
			List<JobCMS> jobList;
			List<RecentSearch> _recentSearchs;
			IntroController _introViewCtrl;

			public TableSource(List<JobCMS> jobList1, List<RecentSearch> aRecentSearchList, Boolean latestJobSelected, IntroController aIntroVC)
				{
				//TableItems = items;
					jobList = jobList1;
					_recentSearchs = aRecentSearchList;
					isRecentSearch = latestJobSelected;
					_introViewCtrl = aIntroVC;
				}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				if (isRecentSearch)
					return _recentSearchs.Count;
				else
					return jobList.Count;
			}

			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				if (isRecentSearch)
					return 65;
				else
				{
					if (Constants.JobDetailSiteName.Equals("adecco.fr"))
						return 90;
					else
						return 120;
				}

			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{

				if (isRecentSearch)
				{

					var cell = (RecentSearchesCell)tableView.DequeueReusableCell(RecentSearchesCell.Key);
					if (cell == null)
					{
						cell = RecentSearchesCell.Create();
					}

					int index =  _recentSearchs.Count - 1 - indexPath.Row;

					cell.UpdateCell(_recentSearchs[index]);

					UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer();
					tapRecognizer.NumberOfTapsRequired = 1;
					cell.ContentView.AddGestureRecognizer(tapRecognizer);
					tapRecognizer.AddTarget(() =>
					{
						this.DidSelectRow(tableView, indexPath.Row);
					});


					return cell;

				}
				else
				{

					var cell = (CustomCellJobListing)tableView.DequeueReusableCell(CustomCellJobListing.Key);

					if (cell !=null)
						cell = null;
					
					if (cell == null)
					{
						cell = CustomCellJobListing.Create();
					}

					JobCMS aJob = jobList[indexPath.Row];


					JobCMS tempJob = DbHelper.GetFavoriteJobs().Where(x => x.JobId == aJob.JobId).SingleOrDefault<JobCMS>();
					if (tempJob == null)
					{

						aJob.isFavorote = false;
					}
					else
					{
						aJob.isFavorote = true;
					}

					cell.UpdateCell(aJob);


					foreach(UIView view in cell.ContentView.Subviews)
					{
						if (view is UIButton)
						{
							UIButton btnFav = (UIButton)view;
							btnFav.TouchUpInside += FavoriteButtonPressed;
							btnFav.Tag = (int)indexPath.Row;

							if (aJob.isFavorote)
								btnFav.SetImage(UIImage.FromFile("heart-icon-selected.png"), UIControlState.Normal);

							else
								btnFav.SetImage(UIImage.FromFile("fav-icon.png"), UIControlState.Normal);
						}
					}

					UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer();
					tapRecognizer.NumberOfTapsRequired = 1;
					cell.ContentView.AddGestureRecognizer(tapRecognizer);
					tapRecognizer.AddTarget(() =>
					{
						this.DidSelectRow(tableView, indexPath.Row);
					});


					return cell;

				}

			}
				[Export("tableView:didSelectRowAtIndexPath:")]

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
				{


				if (isRecentSearch)
				{
					int index = _recentSearchs.Count - 1 - indexPath.Row;

					RecentSearch aRecentSearch = _recentSearchs[index];

					string keyword = aRecentSearch.Keyword;
					string location = aRecentSearch.Location;

					if(keyword.Equals("N/A"))
							keyword = "";

					if (location.Equals("N/A"))
						location = "";
					
					if (!string.IsNullOrEmpty(aRecentSearch.LocationLatLong))
						Constants.LocationLatLong = aRecentSearch.LocationLatLong;


					_introViewCtrl.searchJobButtonPressed = true;
					_introViewCtrl.txtKeyword.Text = keyword;
					_introViewCtrl.txtLocation.Text = location;
					_introViewCtrl.GetJobSearchData(keyword, location);


				}

				else
				{
					string keyword = _introViewCtrl.txtKeyword.Text.Trim();
					string location = _introViewCtrl.txtLocation.Text.Trim();

					if (keyword.Equals("N/A"))
						keyword = "";

					if (location.Equals("N/A"))
						location = "";

					Constants.JobKeyword = keyword;
					Constants.JobLocation = location;



					JobCMS aJob = jobList[indexPath.Row];

					AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
					UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

					var jobDetailVC = (JobDetailVC)storyboard.InstantiateViewController("JobDetailVC");
					jobDetailVC.JobId = aJob.JobId;
					jobDetailVC.jobTitle = aJob.JobTitle;
					jobDetailVC.postingDate = aJob.PostedDate;
					jobDetailVC.salary = aJob.Salary;
					jobDetailVC.jobCategory = aJob.JobCategoryTitle;
					jobDetailVC.aJob = aJob;
					jobDetailVC.loaction = aJob.JobLocation;

					_introViewCtrl.NavController.PushViewController(jobDetailVC, true);
				}

				tableView.DeselectRow(indexPath, true);

				}


			void DidSelectRow(UITableView tableView, int selectedIndex)
			{
				NSIndexPath indexPath = NSIndexPath.FromRowSection(selectedIndex, 0);
				this.RowSelected(tableView, indexPath);
		
			}


			void FavoriteButtonPressed(object sender, EventArgs ea)
			{
				UIButton btnFav = (UIButton)sender;
				JobCMS aJob = jobList[(int)btnFav.Tag];
				bool isFavorote = !aJob.isFavorote;

				if (isFavorote)
				{
					btnFav.SetImage(UIImage.FromFile("heart-icon-selected.png"), UIControlState.Normal);
					DbHelper.AddFavoriteJob(aJob);
				}
				else
				{
					btnFav.SetImage(UIImage.FromFile("fav-icon.png"), UIControlState.Normal);
					DbHelper.DeleteFavoriteJob(aJob);
				}

				aJob.isFavorote = isFavorote;
				((UITableView)btnFav.Superview.Superview.Superview.Superview).ReloadData();
				_introViewCtrl.FavoriteButtonWithCount(_introViewCtrl.NavigationItem);
			}


		}

	}
	}


