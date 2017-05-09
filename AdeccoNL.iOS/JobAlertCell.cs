using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdeccoNL.iOS
{
	public partial class JobAlertCell : UITableViewCell
	{


		public static readonly NSString Key = new NSString("kJobAlertCell");
		public static readonly UINib Nib;

	

		static JobAlertCell()
		{
			Nib = UINib.FromName("JobAlertCell", NSBundle.MainBundle);
		}

		protected JobAlertCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		public static JobAlertCell Create()
		{
			return (JobAlertCell)Nib.Instantiate(null, null)[0];
		}

		public void UpdateCell(JobAlert aJob)
		{

			//this.txtField.Placeholder = aJob.placeHolder;
			//this.txtField.Text = aJob.inputValue;
			//this.headingLabel.Text = aJob.heading;

		}


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

		}


	}

	public class aJobAlert 
	{
		public string heading { get; set; }
		public string placeHolder { get; set; }
		public string inputValue { get; set; }
	}
}





