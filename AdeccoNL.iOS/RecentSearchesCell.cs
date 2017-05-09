using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

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
