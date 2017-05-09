using System;
using System.IO;
using Foundation;
using UIKit;
using Google.Analytics;

namespace AdeccoNL.iOS
{
	public partial class WebViewController : UIViewController
	{
		public string urlString { get; set; }
		public string titleString { get; set; }

		public bool isAboutUs { get; set; }
		public bool isJobApply { get; set; }


		public WebViewController(IntPtr handle) : base(handle) 
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();



			if (!string.IsNullOrEmpty(this.titleString))
			{
				this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
				{
					ForegroundColor = UIColor.White
				};

				this.Title = this.titleString;
			}
			else
			{
				UIImage logo = UIImage.FromBundle("adecco-logo-white");
				UIImageView imageView = new UIImageView(new System.Drawing.Rectangle(0, 0, 80, 20)); //244 × 60
				imageView.Image = logo;
				this.NavigationItem.TitleView = imageView;
			}






			CustomWebViewDelegate _delegate = new CustomWebViewDelegate(activityIndicator);
			webView.Delegate = _delegate;

			if (isAboutUs)
			{
				this.aboutUs();
			}
			else
			{
				
				var uri = new Uri(urlString.Trim());
				var nsurl = new NSUrl(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));

				if (nsurl == null)
				{
					webView.LoadHtmlString(string.Format("<html><center><font size=+5 color='red'>An error occurred:<br></font></center></html>"), null);
					return;
				}
				webView.LoadRequest(new NSUrlRequest(nsurl));
				webView.ScalesPageToFit = true;
				webView.BackgroundColor = UIColor.White;

				activityIndicator.HidesWhenStopped = true;

				activityIndicator.StartAnimating();

			}

			//var userAgent = webView.EvaluateJavascript("navigator.userAgent");
			if (this.isJobApply)
			{
				Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Apply Job");
				Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
			}
		}



		public void aboutUs()
		{


			activityIndicator.StopAnimating();
			activityIndicator.Hidden = true;

			//Resources
			// Note the Content folder is where we placed the files
			string fileName = "index.html"; // remember case-sensitive

			string localHtmlUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);

			NSUrl url = new NSUrl(localHtmlUrl, false);

			webView.LoadRequest(new NSUrlRequest(url));
			webView.ScalesPageToFit = false;
		}

		public override void PresentViewController(UIViewController viewControllerToPresent, bool animated, Action completionHandler)
		{
					base.PresentViewController(viewControllerToPresent, animated, completionHandler);
		}

		public override void DismissViewController(bool animated, Action completionHandler)
		{

			base.DismissViewController(true, null);

		}

			//http://stackoverflow.com/questions/5399706/how-i-can-call-a-javascript-function-with-monotouch-and-vice-versa/5407039#5407039
		//	NSString* output = [aWebView stringByEvaluatingJavaScriptFromString: @"document.body.offsetHeight;"];

	//[aWebView setFrame:CGRectMake(aWebView.frame.origin.x, aWebView.frame.origin.y, aWebView.frame.size.width, [output floatValue])];
			//string output = [webView stringByEvaluatingJavaScriptFromString: @"document.body.offsetHeight;"];

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	public class CustomWebViewDelegate : UIWebViewDelegate
	{

		public UIActivityIndicatorView activityIndicator;

		public CustomWebViewDelegate(UIActivityIndicatorView loadingIndicator) : base()
		{
			activityIndicator = loadingIndicator;
		}

		public override void LoadFailed(UIWebView webView, NSError error)
		{
			activityIndicator.StopAnimating();
			webView.LoadHtmlString(string.Format("<html><center><font size=+5 color='red'>An error occurred:<br>{0}</font></center></html>", error.LocalizedDescription), null);
			Console.WriteLine("CustomWebViewDelegate:LoadFailed - error loading url {0} with code - {1} and description - {2}", webView.Request.Url, error.Code, error.Description);
		}

		public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			Console.WriteLine("ShouldStartLoad:Schema = {0} call = {1}", request.Url.Scheme, request.Url.Host);

			return true;
		}

		[Export("webViewDidFinishLoad:")]
		public override void LoadingFinished(UIWebView webView)
		{

			Console.WriteLine("== LoadFinished == ");
			activityIndicator.StopAnimating();

			//http://stackoverflow.com/questions/5399706/how-i-can-call-a-javascript-function-with-monotouch-and-vice-versa/5407039#5407039
			//	NSString* output = [aWebView stringByEvaluatingJavaScriptFromString: @"document.body.offsetHeight;"];

			//[aWebView setFrame:CGRectMake(aWebView.frame.origin.x, aWebView.frame.origin.y, aWebView.frame.size.width, [output floatValue])];
			//string output = [webView stringByEvaluatingJavaScriptFromString: @"document.body.offsetHeight;"];
		}
	}
}

