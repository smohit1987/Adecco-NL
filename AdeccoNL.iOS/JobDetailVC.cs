using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Drawing;
using Newtonsoft.Json;
using System.Collections.Generic;

using AdeccoNL;
using System.Net;
using BigTed;

using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using System.Linq;
using SQLite.Net;
using SafariServices;

//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Google.Analytics;

namespace AdeccoNL.iOS
{
	public partial class JobDetailVC : UIViewController
{

		public string JobId { get; set; }
		public string loaction { get; set; }
		public string salary { get; set; }
		public string jobCategory { get; set; }
		public string contractType { get; set; }

		public string postingDate { get; set; }
		public string jobTitle { get; set; }
		public JobCMS aJob { get; set; }
		private string _pathToDatabase;
		private JobCIS aJobDetail { get; set;}



	public JobDetailVC(IntPtr handle) : base(handle) 
	{
			

	}


	

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

			this.baseScrollView.Hidden = true;
			this.applybuttonBGView.Hidden = true;

			GetJobDetails();

			// Perform any additional setup after loading the view, typically from a nib.

			// Figure out where the SQLite database will be.
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			_pathToDatabase = Path.Combine(documents, "db_sqlite-net.db");
			SQLiteConnection dbConnection = DbHelper.GetConnection(_pathToDatabase, new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS());


			UIImage logo = UIImage.FromBundle("adecco-logo-white");
			UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(0, 0, 80, 20)); //244 × 60
			imageView.Image = logo;
			NavigationItem.TitleView = imageView;


			/*
			 this.NavigationItem.SetRightBarButtonItem(
			new UIBarButtonItem("Apply"
				                , UIBarButtonItemStyle.Done
			, (sender, args) =>
			{
				//UIApplication.SharedApplication.OpenUrl(new NSUrl(this.aJobDetail.ApplyUri));
				// Apply button was clicked
				//var sfViewController = new SFSafariViewController(new NSUrl(this.aJobDetail.ApplyUri));
				//PresentViewControllerAsync(sfViewController, true);

				AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
				UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

				var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
				_webViewController.urlString = this.aJobDetail.ApplyUri;
				this.NavigationController.PushViewController(_webViewController, true);


			})
		, true);
*/

			//this.NavigationController.NavigationItem.BackBarButtonItem = new UIBarButtonItem("CustomTitle", UIBarButtonItemStyle.Plain, null);


			this.headerBGView.Layer.CornerRadius = 5.0f;
			this.headerBGView.Layer.BorderWidth = 1.0f;
			this.headerBGView.Layer.BorderColor = UIColor.White.CGColor;

			webView.ScrollView.ScrollEnabled = false;
			webView.LoadFinished += LoadFinished;
			webView.Opaque = false;


			//				this.NavigationController.NavigationBar.BarTintColor = UIColor.Red;

			this.btnApplyNow.BackgroundColor = UIColor.Red;

	}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			// This screen name value will remain set on the tracker and sent with
			// hits until it is set to a new value or to null.
			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Job Detail");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());

		}
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		private async void GetJobDetails()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{

			if (!string.IsNullOrEmpty(this.jobTitle) && !(this.jobTitle.Equals("NoContent")))
			{
				this.titleLabel.Text = this.jobTitle;
			}
			else
			{
				this.titleLabel.Hidden = true;
			}

			if (aJob.isFavorote)
			{
				favorateButton.SetImage(UIImage.FromFile("heart-icon-selected.png"), UIControlState.Normal);
			}

			else
			{
				favorateButton.SetImage(UIImage.FromFile("fav-icon.png"), UIControlState.Normal);
			}
		

			if (!string.IsNullOrEmpty(this.loaction) && !(this.loaction.Equals("NoContent")))
			{
				this.locationLabel.Text = this.loaction;
			}
			else
			{
				this.locationLabel.Text = "No Content";
				//this.locationLabel.Hidden = true;
				//this.btnLocation.Hidden = true;

			}

			//if (!string.IsNullOrEmpty(this.salary) && !(this.salary.Equals("NoContent")))
			//{
			//	this.salaryLabel.Text = this.salary;
			//}
			//else
			//{
			//	this.salaryLabel.Hidden = true;
			//	this.btnSalary.Hidden = true;

			//}
			if (!string.IsNullOrEmpty(this.jobCategory) && !(this.jobCategory.Equals("NoContent")))
			{
				this.categoryLabel.Text = this.jobCategory;
			}
			else
			{
				this.categoryLabel.Text = "N/A";
				//this.btnCategory.Hidden = true;

			}

			if (!string.IsNullOrEmpty(aJob.ContractTypeTitle) && !(aJob.ContractTypeTitle.Equals("NoContent")))
			{
				this.ContractTypeLabel.Text = aJob.ContractTypeTitle;
			}
			else
			{
                this.ContractTypeLabel.Hidden = true;
				this.btnContractType.Hidden = true;

			}

			if (Constants.JobDetailSiteName.Equals("adecco.fr"))
			{
				
			}



			try
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
						
				aJobDetail = new JobCIS();

				ServiceManager jobService = new ServiceManager();
				this.aJobDetail = await jobService.GetJobDetails(this.JobId);
				loadData(aJobDetail);
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
		}
		public DateTime ConvertNsDateToDateTime(NSDate date)
		{
			DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return newDate.AddSeconds(date.SecondsSinceReferenceDate);
		}


		private static string calculateJobPostedDate(DateTime date)
		{
			NSCalendar calendar = NSCalendar.CurrentCalendar;
			NSDateComponents comps = new NSDateComponents();
			comps.Day = date.Day;
			comps.Month = date.Month;
			comps.Year = date.Year;
			comps.Minute = date.Minute;
			comps.Hour = date.Hour;

			string datePosted = "";

			DateTime dtNow = System.DateTime.Now;


			TimeSpan age = dtNow.Date.Subtract(date.Date);

			Int32 diff = Convert.ToInt32(age.TotalDays);

			if (comps.Day < System.DateTime.Now.Day)
			{
				if (diff > 1)
				{
					datePosted = string.Format("{0} days ago", comps.Day.ToString());
				}
				else
				{
					datePosted = "Yesterday";
				}
			}
				else
				{
					datePosted = "Today";
				}


				return datePosted;
			}

		void loadData(JobCIS jobDetail)
		{

			///string htmlString = "<html><body><p style='font-family:verdana;margin-left:10px;font-size:15px;'>The MonoTouch API supports two styles of event notification: the Objective-C style that uses a delegate class or the C# style using event notifications.\n\nThe C# style allows the user to add or remove event handlers at runtime by assigning to the events of properties of this class. Event handlers can be anyone of a method, an anonymous methods or a lambda expression. Using the C# style events or properties will override any manual settings to the Objective-C Delegate or WeakDelegate settings.\n\nThe Objective-C style requires the user to create a new class derived from UIWebViewDelegate class and assign it to the UIKit.Delegate property. Alternatively, for low-level control, by creating a class derived from NSObject which has every entry point properly decorated with an [Export] attribute. The instance of this object can then be assigned to the UIWebView.WeakDelegate property.</p><p><b>Hey</b> you. My <b>name </b> is <h1> Joe </h1></p> </body></html>";
			//string htmlString = "<html><body><p style='font-family:verdana;margin-left:10px;font-size:15px;'>The MonoTouch API supports two styles of event notification: the Objective-C style that uses a delegate class or the C# style using event notifications.\n\nThe C# style allows the user to add or remove event handlers at runtime by assigning to the events of properties of this class. Event handlers can be anyone of a method, an anonymous methods or a lambda expression. Using the C# style events or properties will override any manual settings to the Objective-C Delegate or WeakDelegate settings.\n\nThe Objective-C style requires the user to create a new class derived from UIWebViewDelegate class and assign it to the UIKit.Delegate property. Alternatively, for low-level control, by creating a class derived from NSObject which has every entry point properly decorated with an [Export] attribute. The instance of this object can then be assigned to the UIWebView.WeakDelegate property.</p><p><b>Hey</b> you. My <b>name </b> is <h1> Joe </h1></p> </body></html>";

			string endDate = "";

			if (!string.IsNullOrEmpty(jobDetail.PostingEndDate))
			{
				string[] tokens = jobDetail.PostingEndDate.Split(new[] { " " }, StringSplitOptions.None);
				endDate = tokens[0];
			}
			else if (!string.IsNullOrEmpty(jobDetail.PostedDate))
			{
				string[] tokens = jobDetail.PostedDate.Split(new[] { " " }, StringSplitOptions.None);
				endDate = tokens[0];
			}


				NSDateFormatter dateFormat = new NSDateFormatter();
				dateFormat.DateFormat = "MM/dd/yyyy";
				NSDate date = dateFormat.Parse(endDate);

				DateTime dt = ConvertNsDateToDateTime(date);

				endDate = calculateJobPostedDate(dt);

			if (!string.IsNullOrEmpty(endDate) && !(endDate.Equals("NoContent")))
			{
                this.PostedDateLabel.Text = endDate;
			}
			else
			{
				this.PostedDateLabel.Hidden = true;
				this.btnPostedDate.Hidden = true;

			}

			//var htmlString = String.Format("<html><body><p style='font-family:verdana;margin-left:10px;font-size:13px;’> {0} </p></body></html>", jobDetail.Description);
			//= String.Format("<font face='Helvetica' size='2'> {0}", jobDetail.Description);

			if (!string.IsNullOrEmpty(jobDetail.Description))
			{
				var htmlString = String.Format("<font face='Helvetica' size='2'> {0}", jobDetail.Description);
			    webView.LoadHtmlString(htmlString, null);

			}
			else
				webView.LoadHtmlString(string.Format("<html><center><font size=+5 color='red'>An error occurred:<br></font></center></html>"), null);



		}

		[Export("webView:didFailLoadWithError:")]
		public void LoadFailed(UIWebView webView, NSError error)
		{
			Console.WriteLine("== LoadError == ");
			webView.LoadHtmlString(String.Format("<html><center><font size=+5 color='red'>An error occurred:<br>{0}</font></center></html>", error.LocalizedDescription), null);
		}

	public override void DidReceiveMemoryWarning()
	{
		base.DidReceiveMemoryWarning();
		// Release any cached data, images, etc that aren't in use.
	}

	void LoadFinished(object sender, EventArgs e)
	{
			float contentHeight = (float)webView.ScrollView.ContentSize.Height - 40;

			Console.WriteLine("== LoadFinished == ");


			CGRect webViewframe = this.webView.Frame;
			webViewframe.Size = new CGSize(webViewframe.Size.Width, contentHeight);
			webView.Frame = webViewframe;

			float descHeight = (float)(webViewframe.Size.Height + webViewframe.Location.Y) + 50;

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			if (appDelegate.Window.Frame.Size.Width == 320 && appDelegate.Window.Frame.Size.Height == 568)
			{
				// iphone 5
				socialNetworkingBGView.Frame = new CoreGraphics.CGRect(0, descHeight, 320, 50);

			}
			else
			{
				// iphone 6 
				socialNetworkingBGView.Frame = new CoreGraphics.CGRect(0, descHeight, 375, 50);

			}

			descHeight += 100;

			CGRect descriptionFrame = this.descriptionBGView.Frame;
			descriptionFrame.Size = new CGSize(descriptionFrame.Size.Width, descHeight);
			descriptionBGView.Frame = descriptionFrame;


			float scrollviewHeight = (float)(descriptionFrame.Size.Height + descriptionFrame.Location.Y);

			if (appDelegate.Window.Frame.Size.Width == 320 && appDelegate.Window.Frame.Size.Height == 568)
			{
				// iphone 5
				this.baseScrollView.ContentSize = new CGSize(320, scrollviewHeight);

			}
			else
			{
				// iphone 6 
				this.baseScrollView.ContentSize = new CGSize(375, scrollviewHeight);

			}



			this.baseScrollView.Hidden = false;
			this.applybuttonBGView.Hidden = false;
			BTProgressHUD.Dismiss();

	}


		partial void FavorateButton_TouchUpInside(UIButton sender)
		{
			
			bool isFavorote = !aJob.isFavorote;

			if (isFavorote)
			{
				favorateButton.SetImage(UIImage.FromFile("heart-icon-selected.png"), UIControlState.Normal);
				DbHelper.AddFavoriteJob(aJob);
			}

			else
			{
				favorateButton.SetImage(UIImage.FromFile("fav-icon.png"), UIControlState.Normal);
				DbHelper.DeleteFavoriteJob(aJob);
			}

			aJob.isFavorote = isFavorote;


		}

		partial void BtnApplyNow_TouchUpInside(UIButton sender)
		{
			
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent("Job Search", "Apply_button_press", "AppEvent", null).Build());
	        Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately // just for demonstration // not much recommended 

			//UIApplication.SharedApplication.OpenUrl(new NSUrl(this.aJobDetail.ApplyUri));

			//var sfViewController = new SFSafariViewController(new NSUrl(this.aJobDetail.ApplyUri));
			//PresentViewControllerAsync(sfViewController, true);

			UIStoryboard storyboard = UIStoryboard.FromName("Phone", null);
			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = this.aJobDetail.ApplyUri;
			_webViewController.isJobApply = true;
			this.NavigationController.PushViewController(_webViewController, true);


		}

		partial void GetDirectionButton_TouchUpInside(UIButton sender)
		{
			this.loaction = "nederland";

			string currentLocation = System.Net.WebUtility.UrlEncode("nederland");
			string JobLocation = System.Net.WebUtility.UrlEncode("Amsterdam,+Nederland");

	
			//https://www.google.com/maps?saddr=nederland&daddr=Amsterdam,+Nederland
			string urlString = "https://www.google.com/maps?saddr=" + currentLocation + "&daddr=" + JobLocation;



			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = urlString;
			this.NavigationController.PushViewController(_webViewController, true);
		}

		partial void BtnFacebook_TouchUpInside(UIButton sender)
		{
			string jobTitle = aJobDetail.JobTitle.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace(" - ", "-").Replace("--", "-").ToLower();
			string urlString = Constants.facebookURL.Replace("##TITLE##", Constants.JobBaseAddress + "/Job/" + jobTitle + "?ID=" + this.JobId);

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = urlString;
			_webViewController.titleString = "Facebook Share";
			this.NavigationController.PushViewController(_webViewController, true);	
		}

		partial void BtnTwitter_TouchUpInside(UIButton sender)
		{
			string jobTitle = aJobDetail.JobTitle.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace(" - ", "-").Replace("--", "-").ToLower();
			string urlString = Constants.twitterURL.Replace("##TITLE##", Constants.JobBaseAddress + "/Job/" + jobTitle + "?ID=" + this.JobId);

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);
		
			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = urlString;
			_webViewController.titleString = "Twitter Share";

			this.NavigationController.PushViewController(_webViewController, true);		}

		partial void BtnMail_TouchUpInside(UIButton sender)
		{
			// google plus 

			string jobTitle = aJobDetail.JobTitle.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace(" - ", "-").Replace("--", "-").ToLower();
			string urlString = Constants.googleplusURL.Replace("##TITLE##", Constants.JobBaseAddress + "/Job/" + jobTitle + "?ID=" + this.JobId);

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);


			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = urlString;
			_webViewController.titleString = "Google+ Share";

			this.NavigationController.PushViewController(_webViewController, true);
		}
		partial void BtnLinkedin_TouchUpInside(UIButton sender)
		{
			//		public static string linkedinURL = "https://www.linkedin.com/shareArticle?mini=true&url=##URL##&title=##TITLE##&summary=&source=";


			string jobTitle = aJobDetail.JobTitle.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace("&", "").Replace(".", "").Replace("/", "").Replace(" - ", "-").Replace("--", "-").ToLower();
			string urlString = Constants.linkedinURL.Replace("##URL##", Constants.JobBaseAddress + "/Job/" + jobTitle + "?ID=" + this.JobId);
			urlString = urlString.Replace("##TITLE##",aJobDetail.JobTitle);


			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = urlString;
			_webViewController.titleString = "Linkedin Share";

			this.NavigationController.PushViewController(_webViewController, true);

		}


	}

public class CookieWebClient : WebClient
{
	public CookieContainer CookieContainer { get; private set; }

	/// <summary>
	/// This will instanciate an internal CookieContainer.
	/// </summary>
	public CookieWebClient()
	{
		this.CookieContainer = new CookieContainer();
	}

	/// <summary>
	/// Use this if you want to control the CookieContainer outside this class.
	/// </summary>
	public CookieWebClient(CookieContainer cookieContainer)
	{
		this.CookieContainer = cookieContainer;
	}

	protected override WebRequest GetWebRequest(Uri address)
	{
		var request = base.GetWebRequest(address) as HttpWebRequest;
		if (request == null) return base.GetWebRequest(address);
		request.CookieContainer = CookieContainer;
		return request;
	}
}

	public class ApiProvider
	{
		public static System.Net.WebClient webClient;
		public static CookieWebClient webCookieClient;
		public ApiProvider()
		{
		}



	public void GetJobDetails(string JobId)
	{
		try
		{
			var cookieContainer = new CookieContainer();
			webCookieClient = new CookieWebClient();
			string JobURL = Constants.JobBaseAddress + Constants.JobDetailURL + JobId;

			webCookieClient.DownloadDataAsync(new Uri(JobURL));

			// Call authenticated resources
			//client.UploadString ("http://domain.com/ProtectedArea/MyProtectedResource", "POST", "some data");
		}
		catch (Exception ex)
		{
			string str = ex.Message;
		}

	}
	private void WebCookieClient_UploadDataCompleted1(object sender, UploadDataCompletedEventArgs e)
	{
		byte[] data = e.Result;
		string responseContent = System.Text.Encoding.UTF8.GetString(data);
	}
	}
}

