using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using BigTed;
using Foundation;
using SQLite.Net;
using UIKit;
using CoreGraphics;
using Google.Analytics;

namespace AdeccoNL.iOS
{
	public partial class SearchResultVC : UIViewController
	{
		public List<JobCMS> jobList { get; set; }
		private string _pathToDatabase;

		public Boolean isFavoriteJob { get; set; }
		public string _keyword { get; set; }
		public string _location { get; set; }
		public int CurrentpageNum = 1;
		public string LocationLatLong { get; set; }

		public Boolean isLoadingMoreData { get; set; }
		public JobRequest aJobRequest { get; set; }


		public Dictionary<string, dynamic> serverResponse { get; set;}


		public SearchResultVC(IntPtr handle) : base(handle)
		{
			
		}


		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			tblView.ReloadData();

			if (!isFavoriteJob)
			{
				this.FavoriteButtonWithCount(this.NavigationItem);
			}


			// Refine result if filter appiled.
			if (Constants.isFileterApplied && !isFavoriteJob)
			{
				this.jobList = Constants.jobSearchResponse["jobList"];
				tblView.Source = new TableSource(jobList, this, this.isFavoriteJob);
				tblView.ReloadData();
				this.headerLabel.Text = "  " + Constants.JobCount;
			}
			else if (Constants.shouldResetFilter && !isFavoriteJob)
			{
				this.GetJobSearchData(this._keyword, this._location);
				Constants.shouldResetFilter = false;
			}


			if (isFavoriteJob || this.jobList.Count < 1)
			{
				custmFooterView.Hidden = true;
				horizontalSepratorView.Hidden = true;
				verticalSeprator.Hidden = true;


			}


			if (isFavoriteJob)
			{
				CGRect frame = this.tblView.Frame;
				frame.Height += 40;
				this.tblView.Frame = frame;
			}


			if(jobList.Count < 1)
				BTProgressHUD.ShowToast("No record found.", false, 3000);


		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			_pathToDatabase = Path.Combine(documents, "db_sqlite-net.db");
			SQLiteConnection dbConnection = DbHelper.GetConnection(_pathToDatabase, new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS());
			//DbHelper.CreateDatabaseAndTables();


			this.isLoadingMoreData = false;

			UIImage logo = UIImage.FromBundle("adecco-logo-white");
			UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(0, 0, 80, 20)); //244 × 60
			imageView.Image = logo;
			NavigationItem.TitleView = imageView;

			tblView.Source = new TableSource(jobList, this, this.isFavoriteJob);

			//#EEEEEE   light Grey color 
			this.headerLabel.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);


			if (!isFavoriteJob)
			{
				this.FavoriteButtonWithCount(this.NavigationItem);
				this.headerLabel.Layer.CornerRadius = 0.0f;
				this.headerLabel.Layer.BorderWidth = 1.0f;
				this.headerLabel.Layer.BorderColor = UIColor.Gray.CGColor;
				this.headerLabel.Text = "  " + Constants.JobCount;
				this.headerLabel.AdjustsFontSizeToFitWidth = true;
			}
			else
				this.headerLabel.Hidden = true;



			tblView.TableFooterView = new UIView();

			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Job Listing");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());


		}

		void FavoriteButtonWithCount(UINavigationItem navItem)
		{

			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent("Job Search", "Favorate_button_press", "AppEvent", null).Build());
			Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately // just for demonstration // not much recommended 



			if (isFavoriteJob)
				return;


			List<JobCMS> favJobList = DbHelper.GetFavoriteJobs();
			int favCount = 0;

			if (favJobList != null)
				favCount = favJobList.Count;


			UIButton btn = new UIButton(UIButtonType.Custom);
			btn.Frame = new RectangleF(0, 0, 40, 40);
			btn.TouchUpInside += FavoriteButtonPressed;
			btn.ShowsTouchWhenHighlighted = true;

			UILabel lbl = new UILabel();
			lbl.Frame = new RectangleF(19, 0, 20, 20);
			lbl.Text = favCount.ToString();
			lbl.TextAlignment = UITextAlignment.Center;
			lbl.TextColor = UIColor.Red;

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

			UIBarButtonItem barButton2 = new UIBarButtonItem(btn);
			navItem.RightBarButtonItem = barButton2;

			SearchResultVC.favAnimation(this, lbl, true, 0.3f, null);
		}

		static void favAnimation(SearchResultVC instance, UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
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
			this.NavigationController.PushViewController(searchResultVC, true);
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

			location = System.Net.WebUtility.UrlEncode(location);
			keyword = System.Net.WebUtility.UrlEncode(keyword);

			// Add table footer view if this is not a fav job screen 
			if(!isFavoriteJob)
				this.AddTableFooterView();

			JobRequest jobRequest = new JobRequest();
			jobRequest.Keyword = "";
			jobRequest.Location = "";
			jobRequest.CurrentLanguage = "nl-NL";
			jobRequest.SitenameForRegister = Constants.JobDetailSiteName;

			jobRequest.FilterURL = Constants.JobSearchFilterURL + "pageNum=" + this.CurrentpageNum.ToString() + "&display=" + Constants.displayCount + "&k=" + keyword + "&l=" + location;

			// Refine result if filter appiled.
			if (!string.IsNullOrEmpty(Constants.FilterURL))
				jobRequest.FilterURL = Constants.FilterURL;


			if (!this.isLoadingMoreData)
				BTProgressHUD.Show("Loading...", -1, ProgressHUD.MaskType.Black);


			if (!this.LocationLatLong.Equals("") && location.Contains("%2C"))
			{
				jobRequest.FilterURL = jobRequest.FilterURL + "&xy=" + this.LocationLatLong;
			}
			jobRequest.FacetSettingId = Constants.JobSearchFacetSettingID;
			jobRequest.BaseAddress = Constants.JobBaseAddress;


			this.aJobRequest = jobRequest;

			ServiceManager jobService = new ServiceManager();
			//List<JobCMS> jobList2 = await jobService.AsyncJobSearch(jobRequest);

			Dictionary<string, dynamic> responseDict = await jobService.AsyncJobSearch(jobRequest);

			this.serverResponse = responseDict;
			    
			List<JobCMS> jobList2  = responseDict["jobList"];


			// If we are fetching more data then append into joblist  
			if (this.isLoadingMoreData)
			{
				jobList.AddRange(jobList2);
			}
			else
			{
				// else  simple refresh the job list
				jobList = jobList2;
			}
				

			//jobList = (System.Collections.Generic.List<AdeccoNL.JobCMS>)jobList.Concat(jobList2);

			tblView.Source = new TableSource(jobList, this, this.isFavoriteJob);

			tblView.ReloadData();

			tblView.AllowsSelection = true;
			tblView.UserInteractionEnabled = true;

			// Remove table footer view  
			tblView.TableFooterView = new UIView();

			// Refine result if filter appiled.
			BTProgressHUD.Dismiss();
			this.headerLabel.Text = "  " + Constants.JobCount;

		}

		void AddTableFooterView()
		{
			UIView TableFooterView = new UIView();
			TableFooterView.Frame = new CoreGraphics.CGRect(0f, 0f, 375f, 40f);
			TableFooterView.BackgroundColor = UIColor.White;

			UIActivityIndicatorView activivityIndicator = new UIActivityIndicatorView();
			activivityIndicator.Frame = new CoreGraphics.CGRect(100f, 5f, 30f, 30f);
			activivityIndicator.StartAnimating();
			activivityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
			TableFooterView.AddSubview(activivityIndicator);


			UILabel label = new UILabel();
			label.Frame = new CoreGraphics.CGRect(140f, 05f, 375f, 30f);
			label.Text = "Loading....";
			label.TextColor = UIColor.Gray;
			TableFooterView.AddSubview(label);

			this.tblView.TableFooterView = TableFooterView;

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void JobAlertButton_TouchUpInside(UIButton sender)
		{
			var _JobAlertVC = (JobAlertVC)Storyboard.InstantiateViewController("JobAlertVC");
			this.NavigationController.PushViewController(_JobAlertVC, true);
		}

		partial void FilterButton_TouchUpInside(UIButton sender)
		{

			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent("Job Search", "Refine_button_press", "AppEvent", null).Build());
			Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately // just for demonstration // not much recommended 


			Constants.jobSearchResponse = this.serverResponse;

			this.isLoadingMoreData = false;

			var _refineViewController = (RefineViewController)Storyboard.InstantiateViewController("RefineViewController");
			_refineViewController.presentationFacetResultList = this.serverResponse["presentationFacetResultList"];
			_refineViewController.jobRequest = this.aJobRequest;
			this.NavigationController.PushViewController(_refineViewController, true);

		}

		public class TableSource : UITableViewSource
		{

			List<JobCMS> jobList;
			SearchResultVC searchResultVC;
			Boolean isFavoriteJob;
			public Boolean isLoadingData { get; set;}

			public TableSource(List<JobCMS> jobList1, SearchResultVC searchResultVcObj, Boolean isFav)
			{
				jobList = jobList1;
				searchResultVC = searchResultVcObj;
				isFavoriteJob = isFav;
				isLoadingData = false;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return jobList.Count;
			}

			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
					if (Constants.JobDetailSiteName.Equals("adecco.fr"))
						return 90;
					else
						return 120;			
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (CustomCellJobListing)tableView.DequeueReusableCell(CustomCellJobListing.Key);

				if (cell != null)
					cell = null;
				
				if (cell == null)
				{
					cell = CustomCellJobListing.Create();
				}

				JobCMS aJob = jobList[indexPath.Row];
				cell.UpdateCell(aJob);


				JobCMS tempJob = DbHelper.GetFavoriteJobs().Where(x => x.JobId == aJob.JobId).SingleOrDefault<JobCMS>();
				if (tempJob == null)
				{

					aJob.isFavorote = false;
				}
				else
				{
					aJob.isFavorote = true;
				}



				foreach (UIView view in cell.ContentView.Subviews)
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

				return cell;

			}

			void FavoriteButtonPressed(object sender, EventArgs ea)
			{
				UIButton btnFav = (UIButton)sender;

				JobCMS aJob = jobList[(int)btnFav.Tag];

			//	Console.WriteLine("FavoriteButtonPressed tag =={0}", (int)btnFav.Tag);

				//Console.WriteLine("FavoriteButtonPressed =={0}", aJob);

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

				UITableView tableView = (UITableView)btnFav.Superview.Superview.Superview.Superview;

				if (isFavoriteJob && !isFavorote)
				{
					try
					{
						// IF we are coming from Fav job then delete the record from list
						NSIndexPath indexPath = NSIndexPath.FromRowSection((int)btnFav.Tag, 0);

						if (jobList.Count > (int)btnFav.Tag)
						{
							// remove the item from the underlying data source
							jobList.RemoveAt((int)btnFav.Tag);
							// delete the row from the table
							tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Right);
						}
					
					}
					catch (Exception ex)
					{
						string str = ex.Message;
						Console.WriteLine("== This is the main entry point of the application. Main.cs Exception=== {0}", str);

					}

					}
				// manage fav button count
				searchResultVC.FavoriteButtonWithCount(searchResultVC.NavigationItem);

				//Now simply reload tableview 
				//tableView.ReloadData();
				// Invoke our method in 2 seconds
				//PerformSelector(new MonoTouch.ObjCRuntime.Selector("ReloadTableData:"), tableView, 2);
				tableView.ReloadData();

				if(jobList.Count<1)
					BTProgressHUD.ShowToast("No record found.", false, 3000);


			}

			// This registers the method "RunDemo" as responding to the selector "demo:"
			[Export("ReloadTableData:")]
		
			void ReloadTableData(UITableView tableView)
			{
				tableView.ReloadData();
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				
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
				jobDetailVC.contractType = aJob.ContractTypeTitle;
				jobDetailVC.loaction = aJob.JobLocation;

				searchResultVC.NavigationController.PushViewController(jobDetailVC, true);
				tableView.DeselectRow(indexPath, true);
			}

			public override void Scrolled(UIScrollView scrollView)
			{
				//UIScrollView scrollView = searchResultVC.tblView.Superview;
				// load more bottom
				float height = (float)scrollView.Frame.Size.Height;
				float distanceFromBottom = (float)(scrollView.ContentSize.Height - scrollView.ContentOffset.Y);
				if (distanceFromBottom < height && isLoadingData == false)
				{
					if(jobList.Count < int.Parse(Constants.TotalJobCount) && !isFavoriteJob)
					{
						// reload data here
						Console.WriteLine(" load more bottom");
						isLoadingData = true;
						searchResultVC.isLoadingMoreData = true;
						searchResultVC.CurrentpageNum++;
						searchResultVC.GetJobSearchData(searchResultVC._keyword, searchResultVC._location);
					}

				}
			}

		}
	}
}

