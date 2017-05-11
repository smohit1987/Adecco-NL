using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdeccoNL.iOS
{
	public partial class CustomCellBranchListing : UITableViewCell
	{


		public static readonly NSString Key = new NSString("CustomCellBranchListing");
		public static readonly UINib Nib;

		public BranchListVC _branchListVC { get; set; }
		public Branch _aBranch { get; set; }


		static CustomCellBranchListing()
		{
			Nib = UINib.FromName("CustomCellBranchListing", NSBundle.MainBundle);
		}

		protected CustomCellBranchListing(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		public static CustomCellBranchListing Create()
		{
			return (CustomCellBranchListing)Nib.Instantiate(null, null)[0];
		}

		public void UpdateCell(Branch aBranch, BranchListVC vc)
		{
			this._branchListVC = vc;
			this._aBranch = aBranch;

			this.titleLabel.Text = aBranch.BranchName;

			string phone = "Tel: " + aBranch.PhoneNumber;

			if (string.IsNullOrEmpty(aBranch.PhoneNumber))
			{
				phone = "Tel: " + "N/A";
			}

			string email = "Email: " + aBranch.BranchEmail;

			if (string.IsNullOrEmpty(aBranch.BranchEmail))
			{
				email = "Email: " + "N/A";
			}

			this.phoneButton.SetTitle(phone, UIControlState.Normal);
			this.emailButton.SetTitle(email, UIControlState.Normal);

			this.addressLabel.Text = aBranch.Address + ", " + aBranch.ZipCode + ", " + aBranch.City + " " + aBranch.CountryName;


			this.emailButton.TouchUpInside += (object sender, System.EventArgs e) =>
			{
				if (!string.IsNullOrEmpty(aBranch.BranchEmail))
				{
					this._branchListVC.sendMail(this._aBranch.BranchEmail);

				}
			};

		}

		partial void DirectionButton_TouchUpInside(UIButton sender)
		{
			this._branchListVC.showDirection(this._aBranch.Address + "," + this._aBranch.ZipCode);

		}

		void EmailButton_TouchUpInside(UIButton sender)
		{
			
		}

		partial void PhoneLabel_TouchUpInside(UIButton sender)
		{
			this._branchListVC.call(this._aBranch.PhoneNumber);

		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

            AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			if (appDelegate.Window.Frame.Size.Width == 320 && appDelegate.Window.Frame.Size.Height == 568)
			{

                this.directionButton.Frame = new CGRect(5, 5, 270, 25);

			}
			else if (appDelegate.Window.Frame.Size.Width == 414)
			{	// iPhone 6+
         		 this.directionButton.Frame = new CGRect(310, 85, 90, 23);
			}
			//imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
			//headingLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
			//subheadingLabel.Frame = new CGRect(100, 18, 100, 20);
		}


	}

}





