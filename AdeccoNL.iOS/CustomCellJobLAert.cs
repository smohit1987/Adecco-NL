using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdeccoNL.iOS
{
	public partial class CustomCellJobLAert : UITableViewCell
	{


		public static readonly NSString Key = new NSString("CustomCellJobLAert");
		public static readonly UINib Nib;
		public UITextField aTxtField { get; set; }

		static CustomCellJobLAert()
		{
			Nib = UINib.FromName("CustomCellJobLAert", NSBundle.MainBundle);
		}

		protected CustomCellJobLAert(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		public static CustomCellJobLAert Create()
		{
			return (CustomCellJobLAert)Nib.Instantiate(null, null)[0];
		}

		public void UpdateCell(JobAlert aJob)
		{

			this.txtField.Placeholder = aJob.placeHolder;
			this.txtField.Text = aJob.inputValue;
			this.titleLabel.Text = aJob.heading;

			this.aTxtField = this.txtField;

		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			txtField.Layer.BorderColor = UIColor.LightGray.CGColor;
			txtField.Layer.BorderWidth = 1.0f;
			txtField.Layer.CornerRadius = 5f;
			txtField.Layer.MasksToBounds = true;

			txtField.Layer.ShadowColor = UIColor.LightGray.CGColor;
			txtField.Layer.ShadowOpacity = 0.4f;
			txtField.Layer.ShadowRadius = 0.4f;
			txtField.Layer.ShadowOffset = new SizeF(0, 0); //(2.0f, 2.0f);
			txtField.Layer.MasksToBounds = false;

			txtField.TextColor = UIColor.DarkGray;

			this.txtField.LeftViewMode = UITextFieldViewMode.Always;
			this.txtField.LeftView = new UIView(new RectangleF(0, 0, 10, 20)); //imageVie

			titleLabel.TextColor = UIColor.DarkGray;

		}


	}

	public class JobAlert
	{
		public string heading { get; set; }
		public string placeHolder { get; set; }
		public string inputValue { get; set; }
	}
}





