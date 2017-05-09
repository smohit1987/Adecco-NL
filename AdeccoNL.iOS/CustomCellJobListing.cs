using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdeccoNL.iOS
	{
		public partial class CustomCellJobListing : UITableViewCell
		{


			public static readonly NSString Key = new NSString("CustomCellJobListing");
			public static readonly UINib Nib;
			

			static CustomCellJobListing()
			{
				Nib = UINib.FromName("CustomCellJobListing", NSBundle.MainBundle);
			}

			protected CustomCellJobListing(IntPtr handle) : base(handle)
			{
				// Note: this .ctor should not contain any initialization logic.
			}
			public static CustomCellJobListing Create()
			{
				return (CustomCellJobListing)Nib.Instantiate(null, null)[0];
			}

			public void UpdateCell(JobCMS aJob)
			{


			this.titleLabel.Text = aJob.JobTitle;
			this.LocationLabel.Text = aJob.JobLocation;
			//this.datePostedLabel.Hidden = true;

			if (!string.IsNullOrEmpty(aJob.Salary) && !(aJob.Salary.Equals("NoContent")))
			{
				this.ContractType.Text = aJob.Salary;
				this.ContractTypeImgView.Image = UIImage.FromBundle("eurosymbol.png");
				//
			}
			else if (!string.IsNullOrEmpty(aJob.ContractTypeTitle) && !(aJob.ContractTypeTitle.Equals("NoContent")))
			{

				this.ContractType.Text = aJob.ContractTypeTitle;
                this.ContractTypeImgView.Image = UIImage.FromBundle("calender-icon.png");

			}
			else
			{

				this.ContractType.Text = "N/A";
                this.ContractTypeImgView.Image = UIImage.FromBundle("calender-icon.png");
				//this.expLable.Hidden = true;
				//this.expImgView.Hidden = tr

			}
			if (!string.IsNullOrEmpty(aJob.JobCategoryTitle) && !(aJob.JobCategoryTitle.Equals("NoContent")))
			{

				this.CategoryLabel.Text = aJob.JobCategoryTitle;
				this.CategoryImageView.Image = UIImage.FromBundle("Truck.png");

			}
			else
			{

             	this.CategoryLabel.Text = "N/A";
                this.CategoryImageView.Image = UIImage.FromBundle("Truck.png");
				//this.expLable.Hidden = true;
				//this.expImgView.Hidden = tr

			}

			//btnFavJob.Hidden = true;

			// iPhone 5 customization
			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			if (appDelegate.Window.Frame.Size.Width == 320 && appDelegate.Window.Frame.Size.Height == 568)
			{

				this.titleLabel.Frame = new CGRect(5, 5, 270, 25);
				this.btnFavJob.Frame =  new CGRect(275, 5, 40, 40);

			}

			string endDate = "";

			if (!string.IsNullOrEmpty(aJob.PostedDate))
			{
				string[] tokens = aJob.PostedDate.Split(new[] { " " }, StringSplitOptions.None);
				endDate = tokens[0];

			}
			else if (!string.IsNullOrEmpty(aJob.PostingEndDate))
			{
				string[] tokens = aJob.PostingEndDate.Split(new[] { " " }, StringSplitOptions.None);
				endDate = tokens[0];
			}



			NSDateFormatter dateFormat = new NSDateFormatter();
			dateFormat.DateFormat = "MM/dd/yyyy";
			NSDate date = dateFormat.Parse(endDate);

			DateTime dt = ConvertNsDateToDateTime(date);

			endDate = calculateJobPostedDate(dt);


			if (!string.IsNullOrEmpty(endDate) && !(endDate.Equals("NoContent")))
			{
				this.datePostedLabel.Text = endDate;
			}
			else
			{
				this.datePostedLabel.Hidden = true;

			}

			if (Constants.JobDetailSiteName.Equals("adecco.fr"))
				{
                  this.datePostedLabel.Frame = new CGRect(273, 60, 85, 20);
				  this.CategoryLabel.Hidden = true;
                  this.CategoryImageView.Hidden = true;
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

			DateTime dtNow =  System.DateTime.Now;


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
					datePosted = Translations.Yesterday;
				}
			}
			else
			{
				datePosted = Translations.Today;
			}


				return datePosted;
				
		
		}

		partial void BtnFavJob_TouchUpInside(UIButton sender)
		{
			/*
			 
			bool isSelected = !sender.Selected;

			if (isSelected)
				btnFavJob.SetImage(UIImage.FromFile("heart-icon-selected.png"), UIControlState.Normal);
			
			else
				btnFavJob.SetImage(UIImage.FromFile("fav-icon.png"), UIControlState.Normal);
			
			btnFavJob.Selected = isSelected;

			//throw new NotImplementedException();
			*/
		}

			public override void LayoutSubviews()
			{
				base.LayoutSubviews();
				//imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
				//headingLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
				//subheadingLabel.Frame = new CGRect(100, 18, 100, 20);
			}


		}

		public class Job
		{
			public string title { get; set; }
			public string location { get; set; }
			public string salary { get; set; }
			public string exp { get; set; }
			public string skill { get; set; }
			public string cat { get; set; }
			public string job_type { get; set; }
			public string @ref { get; set; }
			public string desc { get; set; }
		}
	}





