using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using UIKit;
using System.Runtime.CompilerServices;
using BigTed;
using CoreLocation;
using Google.Analytics;


	namespace AdeccoNL.iOS
	{

		public partial class BranchSearchVC : UIViewController
		{
			private static bool _searchingLocation = false;

			#region Computed Properties
			public static LocationManager Manager { get; set; }
			#endregion
			
			public BranchSearchVC(IntPtr handle) : base(handle)
			{

			}


			public override void ViewWillAppear(bool animated)
			{
				base.ViewWillAppear(animated);

				View.EndEditing(true);

				AutoCompleteTextFieldManager.RemoveTable();
				AutoCompleteTextFieldManager.RemoveAll();

			   this.Title = "Branch Locator";


			}

			public override void ViewWillDisappear(bool animated)
			{
				base.ViewWillDisappear(animated);
				this.Title = "Back";

			}


			public override void ViewDidLoad()
			{
				base.ViewDidLoad();
				// Perform any additional setup after loading the view, typically from a nib.

				//UIImage logo = UIImage.FromBundle("adecco-logo-white");
				//UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(0, 0, 80, 20)); //244 × 60
				//imageView.Image = logo;
				//NavigationItem.TitleView = imageView;


			this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
			{
				ForegroundColor = UIColor.White
			};


				//#EEEEEE   light Grey color 
				View.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);

				//#FFFFFF  white color 
				bgView.BackgroundColor = UIColor.Clear.FromHexString("##FFFFFF", 1.0f);
				bgView.Layer.BorderColor = UIColor.LightGray.CGColor;
				bgView.Layer.BorderWidth = 0.5f;
				bgView.Layer.CornerRadius = 2.0f;
				bgView.Layer.MasksToBounds = true;


				txtLocation.Tag = 1032;
			
				txtLocation.TextColor = UIColor.DarkGray;
				txtLocation.Font = UIFont.SystemFontOfSize(16);


				txtDistance.TextColor = UIColor.DarkGray;
				txtDistance.Font = UIFont.SystemFontOfSize(16);
				txtDistance.Text = "25 Km";

				btnsearch.Layer.CornerRadius = 2.0f;


				activityIndicatorLocation.Hidden = true;
				activityIndicatorLocation.StopAnimating();

				//this.txtLocation.LeftViewMode = UITextFieldViewMode.Always;
				//this.txtLocation.LeftView = new UIView(new RectangleF(0, 0, 35, 35)); //imageViewKeyword;

				//this.txtDistance.LeftViewMode = UITextFieldViewMode.Always;
				//this.txtDistance.LeftView = new UIView(new RectangleF(0, 0, 35, 35)); //imageViewLocation;

				this.txtLocation.AttributedPlaceholder = new NSAttributedString(
				Translations.Bl_Place,
							font: UIFont.SystemFontOfSize(16),
					foregroundColor: UIColor.DarkGray,
			strokeWidth: 0
			);


				this.txtDistance.AttributedPlaceholder = new NSAttributedString(
				Translations.Bl_Distance,
							font: UIFont.SystemFontOfSize(16),
							foregroundColor: UIColor.DarkGray,
			strokeWidth: 0
		);


				txtDistance.ShouldReturn += TextFieldShouldReturn;
				txtLocation.ShouldReturn += TextFieldShouldReturn;

				txtDistance.ShouldClear += TextFieldShoulClear;
				txtLocation.ShouldClear += TextFieldShoulClear;

				txtLocation.ShouldBeginEditing += ShouldBeginEditing;
				txtDistance.ShouldBeginEditing += ShouldBeginEditing;

				

				NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification, TextFieldTextDidChangeNotification);

			lblPlace.Text = Translations.Bl_Place;
			lblDistance.Text = Translations.Bl_Distance;

			btnsearch.SetTitle(Translations.Bl_Search, UIControlState.Normal);
			btnsearch.SetTitle(Translations.Bl_Search, UIControlState.Highlighted);

			    			//btnJobSearch.TouchUpInside += JobSearchButtonPressed;

			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Branch Locator");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());

			}

		// get user currenyt location
		partial void UIButton1070_TouchUpInside(UIButton sender)
		{
			_searchingLocation = false;

			// As soon as the app is done launching, begin generating location updates in the location manager
			if (Manager == null)
				Manager = new LocationManager();

			Manager.StartLocationUpdates();

			// It is better to handle this with notifications, so that the UI updates
			// resume when the application re-enters the foreground!
			Manager.LocationUpdated += HandleLocationChanged;
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

			void TextFieldTextDidChangeNotification(NSNotification notification)
			{

				UITextField txtField = (UITextField)notification.Object;
				string location = txtField.Text.Trim();

				if (location.Length > 2 && txtField == txtLocation)
					this.GetAutosuggestLocation(txtField);


			}


	#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
			async void GetAutosuggestLocation(UITextField textField)
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
				activityIndicatorLocation.Hidden = false;


					ServiceManager jobService = new ServiceManager();
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


			private bool ShouldBeginEditing(UITextField textfield)
			{
				if (textfield == txtLocation)
					return true;
				else
				{
					txtLocation.ResignFirstResponder();
					this.showRadius();
					return false;
				}
					
			}

			public void showRadius()
			{
			
				// Create a new Alert Controller
				UIAlertController actionSheetAlert = UIAlertController.Create("Select Radius", "", UIAlertControllerStyle.ActionSheet);

				// Add Actions
			foreach (string key in ConfigManager.BLDistance)
			{
				actionSheetAlert.AddAction(UIAlertAction.Create(key+" Km", UIAlertActionStyle.Default, (action) => selectedRadius(key + " Km")));
	
			}

				//actionSheetAlert.AddAction(UIAlertAction.Create("5 Km", UIAlertActionStyle.Default, (action) => selectedRadius("5 Km")));
				//actionSheetAlert.AddAction(UIAlertAction.Create("25 Km", UIAlertActionStyle.Default, (action) => selectedRadius("25 Km")));
				//actionSheetAlert.AddAction(UIAlertAction.Create("50 Km", UIAlertActionStyle.Default,(action) => selectedRadius("50 Km")));
				actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));

				// Display the alert
				this.PresentViewController(actionSheetAlert, true, null);

			}

			public void selectedRadius(string aRadius)
			{
				//Console.WriteLine("Selected Radius is = {0}",aRadius);

				this.txtDistance.Text = aRadius;
			}

			private bool TextFieldShouldReturn(UITextField textfield)
			{
				textfield.ResignFirstResponder();

				if (textfield == txtLocation)
					AutoCompleteTextFieldManager.RemoveTable();

				return true; 
			}

			private bool TextFieldShoulClear(UITextField textfield)
			{
				textfield.Text = "";

				if(textfield == txtLocation)
					AutoCompleteTextFieldManager.RemoveTable();

				return true;
			}


			public override void TouchesBegan(NSSet touches, UIEvent evt)
			{
				base.TouchesBegan(touches, evt);
				UITouch touch = touches.AnyObject as UITouch;
				if (touch.View == View)
				{
					txtLocation.ResignFirstResponder();
					txtDistance.ResignFirstResponder();
					AutoCompleteTextFieldManager.RemoveTable();
					txtLocation.TouchDown += delegate
					{
						AutoCompleteTextFieldManager.RemoveTable();
					};

				}
			}

			partial void Btnsearch_TouchUpInside(UIButton sender)
			{
				txtLocation.ResignFirstResponder();
				txtDistance.ResignFirstResponder();
				this.View.EndEditing(true);


			string[] distances = txtDistance.Text.Split(new[] { " " }, StringSplitOptions.None);
			ConfigManager.BLRadius = distances[0];
			// validate request first then start searching 

			if (string.IsNullOrEmpty(txtLocation.Text.Trim()))
			{
				var alert = UIAlertController.Create("", "Oops. You have not entered a city name!", UIAlertControllerStyle.Alert);

				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));

				PresentViewController(alert, animated: true, completionHandler: null);
			}
			else
			{
				this.geoCodeLocation(txtLocation.Text.Trim());
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

			BTProgressHUD.Show("Searching Branches...", -1, ProgressHUD.MaskType.Black);

				//AsyncGeocodeLocation
				ServiceManager jobService = new ServiceManager();

				string latLong = await jobService.AsyncGeocodeLocation(location);

				Console.WriteLine(latLong);

			if (!string.IsNullOrEmpty(latLong))
			{

				// validate request first then start searching 
				this.getBranchList();
			}
			else
				BTProgressHUD.Dismiss();





			}

	#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
			private async void getBranchList()
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

			 //BTProgressHUD.Show("Searching Branches...", -1, ProgressHUD.MaskType.Black);

				//Console.WriteLine("JobLocations ==> {0}",DbHelper.JobLocations);

				BranchRequest _branchRequest = new BranchRequest();
				_branchRequest.Latitude = Constants.Latitude;
				_branchRequest.Longitude = Constants.Longitude;
				_branchRequest.Radius = ConfigManager.BLRadius;

			if (!string.IsNullOrEmpty(Constants.Latitude))
				_branchRequest.Latitude = Constants.Latitude;

			if (!string.IsNullOrEmpty(Constants.Longitude))
				_branchRequest.Longitude = Constants.Longitude;

			if (!string.IsNullOrEmpty(this.txtDistance.Text))
			{
				String[] splitstring = txtDistance.Text.Split(' ');
				_branchRequest.Radius = splitstring[0];

			}

				
			_branchRequest.RadiusUnits = "KM";
			_branchRequest.Industry = "ALL";
			_branchRequest.MaxResults = ConfigManager.BLMaxResultCount;

			ServiceManager jobService = new ServiceManager();

			List<Branch> _branchList = await jobService.AsyncBranchLocator(_branchRequest);

			//Console.WriteLine(_branchList);
			BTProgressHUD.Dismiss();


			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent("Branch Search", "BranchSearch_button_press", "AppEvent", null).Build());
			Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately // just for demonstration // not much recommended 


			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _aBranchListVC = (BranchListVC)storyboard.InstantiateViewController("BranchListVC");
			_aBranchListVC._branchList = _branchList;
			this.NavigationController.PushViewController(_aBranchListVC, true);

		}

			public override void ViewDidUnload()
			{
				base.ViewDidUnload();
			}
			public override void DidReceiveMemoryWarning()
			{
				base.DidReceiveMemoryWarning();
				// Release any cached data, images, etc that aren't in use.
			}


		}

	}

