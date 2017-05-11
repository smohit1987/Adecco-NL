using System;
using System.Drawing;
using UIKit;
using Foundation;
using CoreGraphics;
using SidebarNavigation;

namespace AdeccoNL.iOS
{
	public partial class RootViewController : UIViewController
	{
		private UIStoryboard _storyboard;
		private Boolean isIphone6Plus { get; set; }


		// the sidebar controller for the app
		public SidebarNavigation.SidebarController SidebarController { get; private set; }

		// the navigation controller
		public NavController NavController { get; private set; }

		// the storyboard
		public override UIStoryboard Storyboard 
		{
			get {
				if (_storyboard == null)
				{
					AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

					_storyboard = UIStoryboard.FromName(appDelegate.storyboard, null);

				}
				return _storyboard;
			}
		}

		public RootViewController() : base(null, null)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;


			if (appDelegate.Window.Frame.Size.Width == 414)
				appDelegate.storyboard = "Phone6PluS";
			else if (appDelegate.Window.Frame.Size.Width == 375)
				appDelegate.storyboard = "Phone";			
			else if (appDelegate.Window.Frame.Size.Width == 320 && appDelegate.Window.Frame.Size.Height == 568)
				appDelegate.storyboard = "Phone5";

			//appDelegate.storyboard = "Phone";

			var introController = (IntroController)Storyboard.InstantiateViewController("IntroController");
			var menuController = (MenuController)Storyboard.InstantiateViewController("MenuController");

			// create a slideout navigation controller with the top navigation controller and the menu view controller
			NavController = new NavController();
			NavController.PushViewController(introController, false);
			SidebarController = new SidebarNavigation.SidebarController(this, NavController, menuController);
			SidebarController.MenuWidth = 260;
			SidebarController.ReopenOnRotate = false;
			SidebarController.MenuLocation = SidebarController.MenuLocations.Left;

			//SidebarController.Disabled = true;

			appDelegate.SidebarController = SidebarController;


			NavController.NavigationBar.BarTintColor = UIColor.Clear.FromHexString("#ef2e24", 1.0f);
			NavController.NavigationBar.TintColor = UIColor.White;


		}
	}
}


public static class UIColorExtensions
{
	public static UIColor FromHexString(this UIColor color, string hexValue, float alpha = 1.0f)
	{
		var colorString = hexValue.Replace("#", "");
		if (alpha > 1.0f)
		{
			alpha = 1.0f;
		}
		else if (alpha < 0.0f)
		{
			alpha = 0.0f;
		}

		float red, green, blue;

		switch (colorString.Length)
		{
			case 3: // #RGB
				{
					red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
					green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
					blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
					return UIColor.FromRGBA(red, green, blue, alpha);
				}
			case 6: // #RRGGBB
				{
					red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
					green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
					blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
					return UIColor.FromRGBA(red, green, blue, alpha);
				}

			default:
				throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));

		}
	}
}