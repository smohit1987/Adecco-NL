using System;
using Foundation;
using UIKit;

namespace AdeccoNL.iOS
{
	public partial class SettingViewContrroler : UIViewController
	{
		

		public SettingViewContrroler(IntPtr handle) : base(handle)
		{	
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
            
			this.Title = "Settings";

			this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
			{
				ForegroundColor = UIColor.White
			};

				//#EEEEEE   light Grey color 
				View.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);

				//#FFFFFF  white color 
				bgView.BackgroundColor = UIColor.Clear.FromHexString("##FFFFFF", 1.0f);
				bgView.Layer.BorderColor = UIColor.LightGray.CGColor;
				bgView.Layer.BorderWidth = 0.5f;
				bgView.Layer.CornerRadius = 2.0f;
				bgView.Layer.MasksToBounds = true;

				txtLanguage.TextColor = UIColor.DarkGray;
				txtLanguage.Font = UIFont.SystemFontOfSize(14);

				txtCountry.TextColor = UIColor.DarkGray;
				txtCountry.Font = UIFont.SystemFontOfSize(14);

				btnApply.Layer.CornerRadius = 2.0f;





			this.txtLanguage.AttributedPlaceholder = new NSAttributedString(
			"Please Select Your Language.",
							font: UIFont.SystemFontOfSize(14),
					foregroundColor: UIColor.DarkGray,
			strokeWidth: 0
			);


			this.txtCountry.AttributedPlaceholder = new NSAttributedString(
					"Please Select Your Country.",
							font: UIFont.SystemFontOfSize(14),
							foregroundColor: UIColor.DarkGray,
			strokeWidth: 0
		);

				txtCountry.ShouldBeginEditing += ShouldBeginEditing;
				txtLanguage.ShouldBeginEditing += ShouldBeginEditing;

		}


		private bool ShouldBeginEditing(UITextField textfield)
		{
			string _path = NSBundle.MainBundle.PathForResource("Country-Lang", "plist");
			var countryDictionary = NSDictionary.FromFile(_path);
			//Console.WriteLine("Country-Lang = {0}",dict);


			if (textfield == txtCountry)
			{
				this.showCountry(countryDictionary.Keys);

			}
			else
			{
				string key = this.txtCountry.Text;

				if (string.IsNullOrWhiteSpace(key))
				{
					var alert = UIAlertController.Create("Adecco", "Please select your Country first.", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
					PresentViewController(alert, animated: true, completionHandler: null);

					return false;
				}


				NSDictionary langDictionary = countryDictionary.ValueForKey((Foundation.NSString)key)  as NSDictionary;

				this.showLanguage(langDictionary.Keys);

			}

			return false;

		}
		public void showCountry( NSObject [] keys)
		{
			// Create a new Alert Controller
			UIAlertController actionSheetAlert = UIAlertController.Create("Select Country", "", UIAlertControllerStyle.ActionSheet);
			// Add Actions
			//actionSheetAlert.AddAction(UIAlertAction.Create("5 Km", UIAlertActionStyle.Default, (action) => selectedCountry("5 Km")));
			//actionSheetAlert.AddAction(UIAlertAction.Create("25 Km", UIAlertActionStyle.Default, (action) => selectedCountry("25 Km")));
			//actionSheetAlert.AddAction(UIAlertAction.Create("50 Km", UIAlertActionStyle.Default, (action) => selectedCountry("50 Km")));


			foreach (NSObject key in keys)
			{
				actionSheetAlert.AddAction(UIAlertAction.Create(key.ToString(), UIAlertActionStyle.Default, (action) => selectedCountry(key.ToString())));
	
			}
						actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));

			// Display the alert
			this.PresentViewController(actionSheetAlert, true, null);

		}

		public void selectedCountry(string aCountry)
		{
					
			this.txtCountry.Text = aCountry;
            this.txtLanguage.Text = "";	

		}	

		public void showLanguage(NSObject [] keys)
		{
			// Create a new Alert Controller
			UIAlertController actionSheetAlert = UIAlertController.Create("Select Language", "", UIAlertControllerStyle.ActionSheet);
	
			foreach (NSObject key in keys)
			{
				actionSheetAlert.AddAction(UIAlertAction.Create(key.ToString(), UIAlertActionStyle.Default, (action) => selectedLanguage(key.ToString())));
	
			}
				actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine("Cancel button pressed.")));

			// Display the alert
			this.PresentViewController(actionSheetAlert, true, null);

		}

		public void selectedLanguage(string lang)
		{
			//Console.WriteLine("Selected Radius is = {0}",aRadius);
			this.txtLanguage.Text = lang;	
		}	


		partial void BtnApply_TouchUpInside(UIButton sender)
		{
			this.View.EndEditing(true);

			if (string.IsNullOrWhiteSpace(this.txtCountry.Text))
				{
					var alert = UIAlertController.Create("Adecco", "Please select your Country first.", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
					PresentViewController(alert, animated: true, completionHandler: null);

					return;
				}
			else if (string.IsNullOrWhiteSpace(this.txtLanguage.Text))
				{
					var alert = UIAlertController.Create("Adecco", "Please select your Language.", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
					PresentViewController(alert, animated: true, completionHandler: null);

					return;
				}


			string _path = NSBundle.MainBundle.PathForResource("Country-Lang", "plist");
			var countryDictionary = NSDictionary.FromFile(_path);

			string country = this.txtCountry.Text;
			NSDictionary langDictionary = countryDictionary.ValueForKey((Foundation.NSString)country) as NSDictionary;
			var language = langDictionary.ValueForKey((Foundation.NSString)txtLanguage.Text);

			Console.WriteLine("selected country = {0} & Language == {1}",country,language);

			// Set translation - localizations
			setTranslations(country,language.ToString());

			// Set api configuration  
			this.setOfflineConfigs(country, language.ToString());

			Constants.isConigurationChanged = true;

			this.NavigationController.PopViewController(true);
				
			}



		public void setOfflineConfigs(string country, string language)
		{

			/*
             * public static string JobBaseAddress = "http://www.adecco.nl";
             * public static string JobDetailCurrentLanguage = "nl-NL";
             * 
             * 
             * 
			 */
		
//ConfigManager.BLDistance = 

			string region = language;


			if (region == null)
				Constants.countryRegion = "NL";
			else
			{
				region = region.Substring(3);
				Constants.countryRegion = region;
				Constants.GoogleLocation = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=##LOCATION##&key=AIzaSyAKaf6N484SZUI6BojKztFkZGSC38uvuuw&components=country:" + Constants.countryRegion.ToLower() + "&types=(regions)";

			}

			Constants.JobBaseAddress = "http://www.adecco." + region;
			Constants.JobDetailCurrentLanguage = language;
			Constants.JobDetailSiteName = "adecco."+ region;


			if (country == null || country.Equals("Adecco Netherlands"))
			{
				//nl 
				Constants.JobBaseAddress = "http://www.adecco.nl";
				Constants.JobDetailCurrentLanguage = language;
				Constants.JobDetailSiteName = "adecco.nl";

				Constants.JobSearchFacetSettingID = "{D444BDE3-4A1D-4C4A-B3AE-D5E9A9B09FD2}";
				Constants.AboutUsURL = Constants.JobBaseAddress + "/over-adecco";
				Constants.ShareURL = Constants.JobBaseAddress + "/vacature/";
				Constants.TermsConditions = Constants.JobBaseAddress + "/terms-of-use";
				Constants.JobSearchFilterURL = Constants.JobBaseAddress + "/vacatures?";

			}
			else if (country.Equals("Adecco UK"))
			{
				Constants.JobBaseAddress = "http://www.adecco.co.uk";
			    Constants.JobDetailCurrentLanguage = language;
				Constants.JobDetailSiteName = "adecco.gb";


				Constants.JobSearchFacetSettingID = "{D08FD751-7E37-43AF-9603-CC319E01CFCC}";
				Constants.AboutUsURL = Constants.JobBaseAddress + "/about-us/";
				Constants.ShareURL = Constants.JobBaseAddress + "/Job/";
				Constants.TermsConditions = Constants.JobBaseAddress + "/terms-and-conditions";
				Constants.JobSearchFilterURL = Constants.JobBaseAddress + "/job-results?";
			}
			else if (country.Equals("Adecco France"))
			{
				Constants.JobBaseAddress = "http://www.adecco.fr";
			    Constants.JobDetailCurrentLanguage = language;
				Constants.JobDetailSiteName = "adecco.fr";

				Constants.JobSearchFacetSettingID = "{2FF62F00-3579-42AA-92D2-9139E16B1C4C}";
				Constants.AboutUsURL = Constants.JobBaseAddress + "/about-us/";
				Constants.ShareURL = Constants.JobBaseAddress + "/Job/";
				Constants.TermsConditions = Constants.JobBaseAddress + "/terms-and-conditions";
				Constants.JobSearchFilterURL = Constants.JobBaseAddress + "/resultats-offres-emploi?";
			}
			else if (country.Equals("Adecco Switzerland"))
			{

				Constants.JobBaseAddress = "http://www.adecco.ch";
			    Constants.JobDetailCurrentLanguage = language;
				Constants.JobDetailSiteName = "adecco.ch";


				Constants.countryRegion = "CH";
				Constants.JobSearchFacetSettingID = "{F96CE424-BF0F-4592-949B-8B9A6C98FE56}";
				Constants.AboutUsURL = Constants.JobBaseAddress + "/" + Constants.JobDetailCurrentLanguage + "/about-adecco/";
				Constants.ShareURL = Constants.JobBaseAddress + "/Job/";
				Constants.TermsConditions = Constants.JobBaseAddress + "/" + Constants.JobDetailCurrentLanguage + "/legal";
				Constants.JobSearchFilterURL = Constants.JobBaseAddress + "/" + Constants.JobDetailCurrentLanguage + "/job-results?";
			}

			Constants.BranchLocator = Constants.JobBaseAddress + "/globalweb/branch/branchsearch";
			Constants.CreateJobAlert = Constants.JobBaseAddress + "/AdeccoGroup.Global/api/Job/SaveJobAlert/";
			Constants.JobSearchURL = Constants.JobBaseAddress + "/AdeccoGroup.Global/api/Job/AsynchronousJobSearch/";

		 }
 
		void setTranslations(string country, string language)
		{
			Console.WriteLine("selected country ={0} and language ={1}",country,language);

			if (language.Equals("nl-NL"))
			{

				Translations.keyword_title = "Trefwoord";
				Translations.job_search = "Zoek";
				Translations.location_title = "Locatie";
				Translations.latest_job = "Laatste job";
				Translations.recent_search = "Recente zoekopdracht";

				Translations.Today = "Vandaag";
				Translations.Yesterday = "Gisteren";


				Translations.Bl_Place = "Plaats";
				Translations.Bl_Distance = "Afstand";
				Translations.Bl_Search = "Zoek";
				Translations.Bl_Branches = "Branches";
				Translations.Bl_Map = "Kaart";

			}
			else if (language.Equals("fr-CH"))
			{
				Translations.keyword_title = "Mot-clé";
				Translations.job_search = "Recherche d'emploi";
				Translations.location_title = "Ville";
				Translations.latest_job = "Dernier emploi";
				Translations.recent_search = "Recherche récente";

				Translations.Today = "Aujourd'hui";
				Translations.Yesterday = "Hier";

				Translations.Bl_Place = "Localisation";
				Translations.Bl_Distance = "Distance";
				Translations.Bl_Search = "Trouver une agence";
				Translations.Bl_Branches = "Branches";
				Translations.Bl_Map = "Carte";

			}
			else if (language.Equals("fr-FR"))
			{
				Translations.keyword_title = "Mot-clé";
				Translations.job_search = "trouver des offres";
				Translations.location_title = "Ville";
				Translations.latest_job = "Dernier emploi";
				Translations.recent_search = "Recherche récente";

				Translations.Today = "Aujourd'hui";
				Translations.Yesterday = "Hier";

				Translations.Bl_Place = "Localisation";
				Translations.Bl_Distance = "Distance";
				Translations.Bl_Search = "Trouver une agence";
				Translations.Bl_Branches = "Branches";
				Translations.Bl_Map = "Carte";


			}
			else if (language.Equals("en-GB") || language.Equals("en-us"))
			{
				Translations.keyword_title = "Keyword";
				Translations.job_search = "Find Jobs";
				Translations.location_title = "Location";
				Translations.latest_job = "Latest Jobs";
				Translations.recent_search = "Recent Search";

				Translations.Today = "Today";
				Translations.Yesterday = "Yesterday";

				Translations.Bl_Place = "Location";
				Translations.Bl_Distance = "Distance";
				Translations.Bl_Search = "Search";
				Translations.Bl_Branches = "Branches";
				Translations.Bl_Map = "Map";
			}
			else if (language.Equals("de-CH"))
			{
				Translations.keyword_title = "Stichwort?";
				Translations.job_search = "Arbeitssuche";
				Translations.location_title = "Ort";
				Translations.latest_job = "Letzter Job";
				Translations.recent_search = "Aktuelle Suche";

				Translations.Today = "Vandaag";
				Translations.Yesterday = "Gisteren";


				Translations.Bl_Place = "Plaats";
				Translations.Bl_Distance = "Afstand";
				Translations.Bl_Search = "Zoek";
				Translations.Bl_Branches = "Branches";
				Translations.Bl_Map = "Kaart";

			}
			else if (language.Equals("it-CH"))
			{
				Translations.keyword_title = "Parola chiave";
				Translations.job_search = "Trova";
				Translations.location_title = "Località";
				Translations.latest_job = "Ultimo lavoro";
				Translations.recent_search = "Ricerca recente";

				Translations.Today = "Oggi";
				Translations.Yesterday = "Ieri";

				Translations.Bl_Place = "Luogo";
				Translations.Bl_Distance = "Distanza";
				Translations.Bl_Search = "Ricerca";
				Translations.Bl_Branches = "Filiali";
				Translations.Bl_Map = "Carta geografica";
			}
		}
 

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

