using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

namespace AdeccoNL.iOS
{
	public partial class RecentSearchesCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("RecentSearchesCell");
		public static readonly UINib Nib;

		static RecentSearchesCell()
		{
			Nib = UINib.FromName("RecentSearchesCell", NSBundle.MainBundle);
		}

		protected RecentSearchesCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}



		public static RecentSearchesCell Create()
		{
			return (RecentSearchesCell)Nib.Instantiate(null, null)[0];
		}

		public void UpdateCell(RecentSearch aRecentSearch)
		{
			this.lblKeyword.Text = aRecentSearch.Keyword;
			this.lblLocation.Text = aRecentSearch.Location;

			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			if (appDelegate.Window.Frame.Size.Width == 320 && appDelegate.Window.Frame.Size.Height == 568)
			{

				this.lblKeyword.Frame = new CGRect(5, 5, 270, 25);
				this.lblLocation.Frame = new CGRect(275, 5, 40, 40);

			}
			else if (appDelegate.Window.Frame.Size.Width == 414)
			{	// iPhone 6+
             	this.lblKeyword.Frame = new CGRect(100, 10, 290, 21);
                this.lblLocation.Frame = new CGRect(100, 35, 290, 21);

			}

		}


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			//imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
			//headingLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
			//subheadingLabel.Frame = new CGRect(100, 18, 100, 20);
		}


	}

}
