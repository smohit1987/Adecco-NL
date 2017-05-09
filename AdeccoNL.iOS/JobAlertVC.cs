using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using UIKit;
using System.Runtime.CompilerServices;
using BigTed;
using CoreGraphics;
using Google.Analytics;

namespace AdeccoNL.iOS
{

	public partial class JobAlertVC : UIViewController
	{
		NSObject _keyboardObserverWillShow;
		NSObject _keyboardObserverWillHide;

		bool isSelected { get; set; }

		public List<JobAlert> _jobAlerts { get; set; }
		public bool isKeyboardOpen { get; set; }


		public JobAlertVC(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			//UIImage logo = UIImage.FromBundle("adecco-logo-white");
			//UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(0, 0, 80, 20)); //244 × 60
			//imageView.Image = logo;
			//NavigationItem.TitleView = imageView;

			//confirmButton.BackgroundColor = UIColor.Clear.FromHexString("#B93224", 1.0f);

			this.NavigationItem.SetRightBarButtonItem(
			new UIBarButtonItem("Confirm"
								, UIBarButtonItemStyle.Done
			, (sender, args) =>
			{
				//Confirm button was clicked
				this.ConfirmButton_TouchUpInside();
			})
		, true);


			btnChckBox.SetImage(UIImage.FromFile("uncheck.png"), UIControlState.Normal);


			this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
			{
				ForegroundColor = UIColor.White
			};

			this.Title = "Job Alert";
			/*
			 * tblView.Layer.BorderColor = UIColor.LightGray.CGColor;
			tblView.Layer.BorderWidth = 1.5f;
			tblView.Layer.CornerRadius = 5f;
			tblView.Layer.MasksToBounds = true;

			tblView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
			tblView.Layer.ShadowOpacity = 0.4f;
			tblView.Layer.ShadowRadius = 2.0f;
			tblView.Layer.ShadowOffset = new SizeF(0, 0); //(2.0f, 2.0f);
			tblView.Layer.MasksToBounds = false;
			*/

			tblView.Tag = 101;
			tblView.ClipsToBounds = true;


			//#EEEEEE   light Grey color 
			titleLabel.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);


			//#EEEEEE   light Grey color 
			View.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);

			//#FFFFFF  white color 
			tblView.BackgroundColor = UIColor.Clear.FromHexString("##FFFFFF", 1.0f);



			//tblView.Layer.BorderColor = UIColor.LightGray.CGColor;
			//tblView.Layer.BorderWidth = 0.5f;
			//tblView.Layer.CornerRadius = 2.0f;
			//tblView.Layer.MasksToBounds = true;




			_jobAlerts = new List<JobAlert>();

			JobAlert aJobAlert = new JobAlert { heading = "Name Job Alert", placeHolder = "B.v.technische functies", inputValue = "" };
			JobAlert bJobAlert = new JobAlert { heading = "Frequency", placeHolder = "Daily", inputValue = "Daily" };
			JobAlert cJobAlert = new JobAlert { heading = "First Name", placeHolder = "Enter your first name here.", inputValue = "" };
			JobAlert dJobAlert = new JobAlert { heading = "Surname", placeHolder = "Enter your surname here.", inputValue = "" };
			JobAlert eJobAlert = new JobAlert { heading = "Email", placeHolder = "Enter your email here.", inputValue = "" };

			_jobAlerts.Add(aJobAlert);
			_jobAlerts.Add(bJobAlert);
			_jobAlerts.Add(cJobAlert);
			_jobAlerts.Add(dJobAlert);
			_jobAlerts.Add(eJobAlert);

			tblView.Source = new TableSource(_jobAlerts, this);
			tblView.ReloadData();
			//tblView.TableFooterView = new UIView();
			//tblView.TableHeaderView = new UIView();

			tblView.ScrollEnabled = true;

			// Setup keyboard event handlers
			RegisterForKeyboardNotifications();

			this.appliedFilterList();

			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Job Alert");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
		}


		void appliedFilterList()
		{
			List<SelectedFacets> selectedFacetsList = new List<SelectedFacets>();

			if (!Constants.jobSearchResponse.ContainsKey("selectedFacetsList"))
			{
				tblView.TableHeaderView = new UIView();
				//tblView.TableFooterView = new UIView();

				return;

			}
			else
			{
				selectedFacetsList = Constants.jobSearchResponse["selectedFacetsList"];

				if (selectedFacetsList.Count < 1)
				{
					tblView.TableHeaderView = new UIView();
					//tblView.TableFooterView = new UIView();

					return;
				}

			}

			float scrollheight = 120.0f;

			if ((selectedFacetsList.Count * 35) < 120)
				scrollheight = selectedFacetsList.Count * 35 + 5;

			UIScrollView scrollView = new UIScrollView
			{
				Frame = new CGRect(0, 0, tblView.Frame.Width, scrollheight),
				ContentSize = new CGSize(tblView.Frame.Size.Width, selectedFacetsList.Count * 40),
				BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f),
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth
			};


			float xPos = 5.0f, yPos = 5.0f;
			int index = 0;

			foreach (SelectedFacets aSelectedFacets in selectedFacetsList)
			{
				var titleString = new NSString(aSelectedFacets.keyName);

				CGSize size = titleString.GetSizeUsingAttributes(new UIStringAttributes { Font = UIFont.SystemFontOfSize(10) });

				UIButton btn = new UIButton(UIButtonType.Custom);
				btn.SetTitle(aSelectedFacets.keyName, UIControlState.Normal);
				btn.Frame = new RectangleF(xPos, yPos, (float)size.Width + 50, 30);

				float maxButtonWidth = xPos + (float)size.Width + 50;

				if (maxButtonWidth > tblView.Frame.Size.Width)
				{
					yPos = yPos + 35;
					xPos = 5.0f;
					btn.Frame = new RectangleF(xPos, yPos, (float)size.Width + 50, 30);
					xPos = maxButtonWidth + 5;

				}
				else
				{
					xPos = maxButtonWidth + 5;
				}


				btn.SetTitleColor(UIColor.Red, UIControlState.Normal);
				btn.Font = UIFont.SystemFontOfSize(10);
				btn.BackgroundColor = UIColor.Clear.FromHexString("##FFFFFF", 1.0f);
				btn.Layer.BorderColor = UIColor.LightGray.CGColor;
				btn.Layer.BorderWidth = 0.5f;
				btn.Layer.CornerRadius = 2.0f;
				btn.Layer.MasksToBounds = true;
				btn.ClipsToBounds = true;
				//btn.TouchDown += removeSelectedFilter;
				btn.Tag = index;
				index++;
				scrollView.AddSubview(btn);
				//tableHeaderView.AddSubview(btn);

				//yPos = yPos + 35;

			}

			//tblView.TableFooterView = new UIView();
			tblView.TableHeaderView = scrollView;


			//CGRect tableFrame = Table.Frame;
			//tableFrame.Y = 0;
			//tblView.Frame = tableFrame;


			tblView.ReloadData();
		}

		void ConfirmButton_TouchUpInside()
		{




			// AlertName
			if (string.IsNullOrEmpty(_jobAlerts[0].inputValue))
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Error",
					Message = "Please enete a valid Job Alert Name."
				};
				alert.AddButton("OK");
				alert.Show();
			}// JobAlertFrequencyId
			else if (string.IsNullOrEmpty(_jobAlerts[1].inputValue))
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Error",
					Message = "Please select Job Alert Frequency."
				};
				alert.AddButton("OK");
				alert.Show();
			}//FirstName
			else if (string.IsNullOrEmpty(_jobAlerts[2].inputValue))
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Error",
					Message = "Please enete a First Name."
				};
				alert.AddButton("OK");
				alert.Show();
			}//LastName
			else if (string.IsNullOrEmpty(_jobAlerts[3].inputValue))
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Error",
					Message = "Please enete a valid Last Name."
				};
				alert.AddButton("OK");
				alert.Show();
			}// email 
			else if (!this.IsValidEmail(_jobAlerts[4].inputValue))
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Error",
					Message = "Please enete a valid email."
				};
				alert.AddButton("OK");
				alert.Show();
			}

			else if (!isSelected)
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Error",
					Message = "Please select the Privacy Policy, the terms and conditions and cookie policy."
				};
				alert.AddButton("OK");
				alert.Show();
			}
			else
			{
				this.CreateJobAlert("", "");
			}
		}

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		private async void CreateJobAlert(string keyword, string location)
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


			BTProgressHUD.Show("Loading...", -1, ProgressHUD.MaskType.Black);


			JobRequest jobRequest = new JobRequest();
			jobRequest.Keyword = "";
			jobRequest.Location = "";
			jobRequest.CurrentLanguage = "nl-NL";
			jobRequest.SitenameForRegister = Constants.JobDetailSiteName;

			jobRequest.FilterURL = Constants.JobSearchFilterURL + "pageNum=" + Constants.CurrentpageNum + "&display=" + Constants.displayCount + "&k=" + keyword + "&l=" + location;

			// Refine result if filter appiled.
			if (!string.IsNullOrEmpty(Constants.FilterURL))
				jobRequest.FilterURL = Constants.FilterURL;

			if (!Constants.LocationLatLong.Equals("") && location.Contains("%2C"))
			{
				jobRequest.FilterURL = jobRequest.FilterURL + "&xy=" + Constants.LocationLatLong;
			}


			jobRequest.FacetSettingId = Constants.JobSearchFacetSettingID;
			jobRequest.BaseAddress = Constants.JobBaseAddress;

			jobRequest.AlertData.Add("AlertName", _jobAlerts[0].inputValue);
			jobRequest.AlertData.Add("JobAlertFrequencyId", _jobAlerts[1].inputValue);
			jobRequest.AlertData.Add("FirstName", _jobAlerts[2].inputValue);
			jobRequest.AlertData.Add("LastName", _jobAlerts[3].inputValue);
			jobRequest.AlertData.Add("EmailAddress", _jobAlerts[4].inputValue);


			ServiceManager jobService = new ServiceManager();

			string JobAlertIdValue = await jobService.AsyncCreateJobAlert(jobRequest);

			if (!string.IsNullOrEmpty(JobAlertIdValue))
			{

				var alert = UIAlertController.Create("Adecco Nederland", "Your new Job Alert is created successfully.", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Thanks", UIAlertActionStyle.Default, action => JobAlertCreated()));
				PresentViewController(alert, animated: true, completionHandler: null);

			}

			BTProgressHUD.Dismiss();

		}

		public void JobAlertCreated()
		{
			isSelected = false;
			btnChckBox.SetImage(UIImage.FromFile("uncheck.png"), UIControlState.Normal);

			_jobAlerts = new List<JobAlert>();

			JobAlert aJobAlert = new JobAlert { heading = "Name Job Alert", placeHolder = "B.v.technische functies", inputValue = "" };
			JobAlert bJobAlert = new JobAlert { heading = "Frequency", placeHolder = "Daily", inputValue = "Daily" };
			JobAlert cJobAlert = new JobAlert { heading = "First Name", placeHolder = "Enter your first name here.", inputValue = "" };
			JobAlert dJobAlert = new JobAlert { heading = "Surname", placeHolder = "Enter your surname here.", inputValue = "" };
			JobAlert eJobAlert = new JobAlert { heading = "Email", placeHolder = "Enter your email here.", inputValue = "" };

			_jobAlerts.Add(aJobAlert);
			_jobAlerts.Add(bJobAlert);
			_jobAlerts.Add(cJobAlert);
			_jobAlerts.Add(dJobAlert);
			_jobAlerts.Add(eJobAlert);

			tblView.Source = new TableSource(_jobAlerts, this);
			tblView.ReloadData();
		}

		bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		partial void BtnChckBox_TouchUpInside(UIButton sender)
		{

			UIButton btnFav = (UIButton)sender;
			isSelected = !btnFav.Selected;

			if (isSelected)
			{
				btnChckBox.SetImage(UIImage.FromFile("check.png"), UIControlState.Normal);
			}
			else
			{
				btnChckBox.SetImage(UIImage.FromFile("uncheck.png"), UIControlState.Normal);
			}

			btnChckBox.Selected = isSelected;
		}

		partial void BtnPrivacypolicy_TouchUpInside(UIButton sender)
		{
			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = "http://www.adecco.nl/terms-of-use/";
			_webViewController.titleString = @"Privacy Policy";
			this.NavigationController.PushViewController(_webViewController, true);
		}


		partial void BtnTermsNCondition_TouchUpInside(UIButton sender)
		{

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = "http://www.adecco.nl/terms-of-use/";
			_webViewController.titleString = @"Terms Of Use";

			this.NavigationController.PushViewController(_webViewController, true);
		}






		public override void ViewDidUnload()
		{
			base.ViewDidUnload();

			UnregisterKeyboardNotifications();
		}

		protected virtual void RegisterForKeyboardNotifications()
		{
			_keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
			_keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
		}



		protected virtual void UnregisterKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillShow);
			NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillHide);
		}

		protected virtual UIView KeyboardGetActiveView()
		{
			return this.View.FindFirstResponder();
		}
		protected virtual void KeyboardWillShowNotification(NSNotification notification)
		{
			

			UITextField activeView = (UITextField)this.tblView.FindFirstResponder();

			if ((activeView == null))
				return;


			var keyboardHeight = UIKeyboard.FrameBeginFromNotification(notification).Height + 30;

			//var keyboardHeight = 260;

			UIEdgeInsets contentInsets = new UIEdgeInsets(64.0f, 0.0f, keyboardHeight, 0.0f);
			tblView.ContentInset = contentInsets;
			tblView.ScrollIndicatorInsets = contentInsets;

			isKeyboardOpen = true;

			CGRect Frame = new CGRect(activeView.Frame.X, activeView.Frame.Y + 50, activeView.Frame.Size.Width, activeView.Frame.Size.Height);
			// Make sure the tapped location is visible.
			this.tblView.ScrollRectToVisible(Frame, true);
		


		}

		protected virtual void KeyboardWillHideNotification(NSNotification notification)
		{
			
			// Remove the inset when the keyboard is hidden so that the
			// TableView will use the whole screen again.
			UIView.BeginAnimations("");
			{
				UIView.SetAnimationCurve(UIViewAnimationCurve.Linear);
				UIView.SetAnimationDuration(0.3);
				//this.tblView.Frame = new CGRect(0, 40, 375, 627);

				UIEdgeInsets contentInsets = new UIEdgeInsets(64.0f, 0.0f, 0.0f, 0.0f);

				this.tblView.ContentInset = contentInsets;
				this.tblView.ScrollIndicatorInsets = contentInsets;
			}
			UIView.CommitAnimations();


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}



		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			UITouch touch = touches.AnyObject as UITouch;
			if (touch.View == View)
			{
				this.View.EndEditing(true);
			}
		}

	}

	public static class ViewExtensions
	{
		/// &lt;summary&gt;
		/// Find the first responder in the &lt;paramref name=&quot;view&quot;/&gt;'s subview hierarchy
		/// &lt;/summary&gt;
		/// &lt;param name=&quot;view&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/param&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; that is the first responder or null if there is no first responder
		/// &lt;/returns&gt;
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		/// &lt;summary&gt;
		/// Find the first Superview of the specified type (or descendant of)
		/// &lt;/summary&gt;
		/// &lt;param name=&quot;view&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/param&gt;
		/// &lt;param name=&quot;stopAt&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; that indicates where to stop looking up the superview hierarchy
		/// &lt;/param&gt;
		/// &lt;param name=&quot;type&quot;&gt;
		/// A &lt;see cref=&quot;Type&quot;/&gt; to look for, this should be a UIView or descendant type
		/// &lt;/param&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; if it is found, otherwise null
		/// &lt;/returns&gt;
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}
	}

	public class TableSource : UITableViewSource
	{

		JobAlertVC _jobAlertVC;
		List<JobAlert> jobAlerts;


		public TableSource(List<JobAlert> data, JobAlertVC aJobAlertVC)
		{
			_jobAlertVC = aJobAlertVC;
			jobAlerts = data;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return jobAlerts.Count;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 85;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (CustomCellJobLAert)tableView.DequeueReusableCell(CustomCellJobListing.Key);

			//if (cell != null)
			//	cell = null;

			if (cell == null)
			{
				cell = CustomCellJobLAert.Create();
			}

			if (indexPath.Row == 1)
			{
				//arrow-down-icon.png
				UIImage arrow = UIImage.FromBundle("arrow-down");
				UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(330, 45, 21, 21)); //244 × 60
				imageView.Image = arrow;
				cell.ContentView.AddSubview(imageView);
			}

			JobAlert aJob = jobAlerts[indexPath.Row];
			cell.UpdateCell(aJob);
			cell.aTxtField.Tag = 1000 + indexPath.Row;
			//cell.aTxtField.Delegate = this;

			cell.aTxtField.ShouldReturn += TextFieldShouldReturn;
			cell.aTxtField.ShouldClear += TextFieldShoulClear;
			cell.aTxtField.ShouldBeginEditing += ShouldBeginEditing;
			cell.aTxtField.EditingDidEnd += TextFieldDidEndEditing;
			cell.aTxtField.EditingChanged += HandleTextMessageChanged;

			cell.SelectionStyle = UITableViewCellSelectionStyle.None;


			return cell;

		}

		void HandleTextMessageChanged(object sender, EventArgs e)
		{
			UITextField txtField = (UITextField)sender;
			int index = (int)txtField.Tag - 1000;
			JobAlert aJobAlert = _jobAlertVC._jobAlerts[index];
			aJobAlert.inputValue = txtField.Text;

			//UITableView tblView = (UITableView)_jobAlertVC.View.ViewWithTag(101);
			//tblView.ReloadData();
		}



		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);

			if (indexPath.Row == 1)
			{
				this.displayFrequencies();
			}


		}



		public void displayFrequencies()
		{

			// Create a new Alert Controller
			UIAlertController actionSheetAlert = UIAlertController.Create("Action Sheet", "Select an item from below", UIAlertControllerStyle.ActionSheet);

			// Add Actions
			actionSheetAlert.AddAction(UIAlertAction.Create("Daily", UIAlertActionStyle.Default, (action) => selectedFrequency("Daily")));
			actionSheetAlert.AddAction(UIAlertAction.Create("Weekly", UIAlertActionStyle.Default, (action) => selectedFrequency("Weekly")));
			actionSheetAlert.AddAction(UIAlertAction.Create("Monthly", UIAlertActionStyle.Default, (action) => selectedFrequency("Monthly")));
			actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));


			// Display the alert
			_jobAlertVC.PresentViewController(actionSheetAlert, true, null);




		}

		public void selectedFrequency(string aFrequency)
		{
			JobAlert aJobAlert = _jobAlertVC._jobAlerts[1];
			aJobAlert.inputValue = aFrequency;

			UITableView tblView = (UITableView)_jobAlertVC.View.ViewWithTag(101);
			tblView.ReloadData();
		}




		private bool TextFieldShouldReturn(UITextField textfield)
		{
			textfield.ResignFirstResponder();
			Console.WriteLine("TextFieldShouldReturn");
			return true;

		}


		private bool TextFieldShoulClear(UITextField textfield)
		{
			Console.WriteLine("TextFieldShoulClear");
			return true;

		}

		void TextFieldDidEndEditing(object sender, EventArgs e)
		//public override void TextFieldDidEndEditing(UITextField textfield)
		{
			UITextField txtField = (UITextField)sender;
			int index = (int)txtField.Tag - 1000;
			JobAlert aJobAlert = _jobAlertVC._jobAlerts[index];
			aJobAlert.inputValue = txtField.Text;

			UITableView tblView = (UITableView)_jobAlertVC.View.ViewWithTag(101);
			tblView.ReloadData();
		}

		private bool TextFieldEndEditing(UITextField textfield)
		{
			Console.WriteLine("TextFieldShoulClear");


			//if (textfield.Tag == 1004)
			//{
			//	UITableView tblView = (UITableView)_jobAlertVC.View.ViewWithTag(101);
			//	//tblView.ContentInset = contentInsets;
			//	//tblView.SetContentOffset(scrollPoint, true);

			//	CGRect frame = tblView.Frame;
			//	frame.Size = new CGSize(tblView.Frame.Size.Width, tblView.Frame.Size.Height + 200);
			//	tblView.Frame = frame;

			//}

			return true;

		}


		private bool ShouldBeginEditing(UITextField textField)
		{
			Console.WriteLine("ShouldBeginEditing");

			if (textField.Tag == 1001)
			{
				_jobAlertVC.View.EndEditing(true);

				this.displayFrequencies();
				return false;
			}


			//if (textField.Tag == 1003)
			//{
			//	//PointF scrollPoint = new PointF(0.0f, 100.0f);
			//	//// Reset the content inset of the scrollView and animate using the current keyboard animation duration
			//	//UIEdgeInsets contentInsets = new UIEdgeInsets(300.0f, 0.0f, 0.0f, 0.0f);

			//	UITableView tblView = (UITableView)_jobAlertVC.View.ViewWithTag(101);
			//	//tblView.ContentInset = contentInsets;
			//	//tblView.SetContentOffset(scrollPoint, true);

			//	CGRect frame = tblView.Frame;
			//	frame.Size = new CGSize(tblView.Frame.Size.Width, tblView.Frame.Size.Height - 200); 
			//	tblView.Frame = frame;
			//}
				

				return true;

		}

	}

}

