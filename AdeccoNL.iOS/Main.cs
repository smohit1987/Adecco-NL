using System;
using UIKit;

namespace AdeccoNL.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			try
			{
				// if you want to use a different Application Delegate class from "AppDelegate"
				// you can specify it here.
				UIApplication.Main(args, null, "AppDelegate");
			}
			catch (Exception ex)
			{
				string str = ex.Message;
				Console.WriteLine("== This is the main entry point of the application. Main.cs Exception=== {0}", str);

			}


		}
	}
}
