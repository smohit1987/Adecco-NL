using System;

using Foundation;
using UIKit;

namespace AdeccoNL.iOS
{
	public partial class RefineCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("RefineCell");
		public static readonly UINib Nib;

		static RefineCell()
		{
			Nib = UINib.FromName("RefineCell", NSBundle.MainBundle);
		}

		protected RefineCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
