using Foundation;
using UIKit;
using System.Drawing;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Google.Analytics;


namespace AdeccoNL.iOS
{

	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public string storyboard;

		public SidebarNavigation.SidebarController SidebarController { get; set; }

		public override UIWindow Window
		{
			get;
			set;
		}

		public RootViewController RootViewController { get { return Window.RootViewController as RootViewController; } }

		const string AllowTrackingKey = "AllowTracking";

		// Shared GA tracker
		public ITracker Tracker;

		// Learn how to get your own Tracking Id from:
		// https://support.google.com/analytics/answer/2614741?hl=en
		public static readonly string TrackingId = "UA-93345854-1";

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method
			// create a new window instance based on the screen size
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			// If you have defined a root view controller, set it here:
			Window.RootViewController = new RootViewController();


			var keys = new object[] { "UserAgent" };
			var objects = new object[] { "Mozilla/5.0 (iPhone; CPU iPhone OS 9_3 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Mobile/13E233" };

			var dictionnary = NSDictionary.FromObjectsAndKeys(objects, keys);

			NSUserDefaults.StandardUserDefaults.RegisterDefaults(dictionnary);
	
    
			// make the window visible
			Window.MakeKeyAndVisible();

			// We use NSUserDefaults to store a bool value if we are tracking the user or not 
			var optionsDict = NSDictionary.FromObjectAndKey(new NSString("YES"), new NSString(AllowTrackingKey));
			NSUserDefaults.StandardUserDefaults.RegisterDefaults(optionsDict);

			// User must be able to opt out of tracking
			Gai.SharedInstance.OptOut = !NSUserDefaults.StandardUserDefaults.BoolForKey(AllowTrackingKey);

			// Initialize Google Analytics with a 5-second dispatch interval (Use a higher value when in production). There is a
			// tradeoff between battery usage and timely dispatch.
			Gai.SharedInstance.DispatchInterval = 5;
			Gai.SharedInstance.TrackUncaughtExceptions = true;

			Tracker = Gai.SharedInstance.GetTracker("AdeccoNL", TrackingId);

			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
			Gai.SharedInstance.OptOut = !NSUserDefaults.StandardUserDefaults.BoolForKey(AllowTrackingKey);

		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}


