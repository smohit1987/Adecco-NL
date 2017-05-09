using System;
using System.Collections.Generic;
using Foundation;
using MessageUI;
using UIKit;
using Google.Analytics;

namespace AdeccoNL.iOS
{
	public partial class BranchListVC : UIViewController
	{

		public List<Branch> _branchList { get; set; }

		public BranchListVC(IntPtr handle) : base(handle)
		{

		}
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.Title = Translations.Bl_Branches;
		}
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			this.Title = "";

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



			this.NavigationController.NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, null);


			tblView.Source = new BranchLocatorTableSource(this._branchList, this);
			tblView.ReloadData();

			this.titleLabel.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);
			this.titleLabel.Text = string.Format("  We have {0} branches for you!", _branchList.Count);

			this.NavigationItem.SetRightBarButtonItem(
			new UIBarButtonItem("Map"
								, UIBarButtonItemStyle.Done
			, (sender, args) =>
			{
				//Map button was clicked
				this.displayMapView();
			})
		, true);

			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Branch Listing");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
			
		}


		public void displayMapView()
		{
			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _aMapViewController = (MapViewController)storyboard.InstantiateViewController("MapViewController");
			_aMapViewController._branchList = _branchList;
			this.NavigationController.PushViewController(_aMapViewController, true);

		}


		public void sendMail(string email)
		{
			MFMailComposeViewController mailController;

			if (MFMailComposeViewController.CanSendMail)
			{
				// do mail operations here
				mailController = new MFMailComposeViewController();
				mailController.SetToRecipients(new string[] { email });
				mailController.SetSubject("Adecco Nederland Branch Query");
				mailController.SetMessageBody("this is a test", false);
			
				mailController.Finished += (object s, MFComposeResultEventArgs args) =>
				{
					Console.WriteLine(args.Result.ToString());
					args.Controller.DismissViewController(true, null);
				};

				this.PresentViewController(mailController, true, null);

			}

		}

		public void call(string phoneNumber)
		{
			var url = new NSUrl("tel:" + phoneNumber);

			if (!UIApplication.SharedApplication.CanOpenUrl(url))
			{
				var av = new UIAlertView("Not supported",
				  "Scheme 'tel:' is not supported on this device",
				  null,
				  "OK",
				  null);
				av.Show();
			}
			else
			{
				UIApplication.SharedApplication.OpenUrl(url);

			}
		}

		public void showDirection(string address)
		{
			string currentLocation = System.Net.WebUtility.UrlEncode("nederland");
			string branchLocation = System.Net.WebUtility.UrlEncode(address);


			//https://www.google.com/maps?saddr=nederland&daddr=Amsterdam,+Nederland
			string urlString = "https://www.google.com/maps?saddr=" + currentLocation + "&daddr=" + branchLocation;

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIStoryboard storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

			var _webViewController = (WebViewController)storyboard.InstantiateViewController("WebViewController");
			_webViewController.urlString = urlString;
			this.NavigationController.PushViewController(_webViewController, true);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	public class BranchLocatorTableSource : UITableViewSource
	{

		List<Branch> branchList;
		BranchListVC _aBranchListVC;

		public BranchLocatorTableSource(List<Branch> aBranchList, BranchListVC aBranchListVC)
		{
			branchList = aBranchList;
			_aBranchListVC = aBranchListVC;

		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return branchList.Count;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 120;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (CustomCellBranchListing)tableView.DequeueReusableCell(CustomCellBranchListing.Key);

			if (cell == null)
			{
				cell = CustomCellBranchListing.Create();
			}

			Branch aBranch = branchList[indexPath.Row];
			cell.UpdateCell(aBranch,_aBranchListVC);

			return cell;

		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			//Branch aBranch = branchList[indexPath.Row];
			tableView.DeselectRow(indexPath, true);
		}
	}
}

