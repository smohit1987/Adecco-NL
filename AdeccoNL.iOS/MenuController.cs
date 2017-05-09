using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using SQLite.Net;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace AdeccoNL.iOS
{
	partial class MenuController : BaseController
	{
		private string _pathToDatabase;


		public MenuController (IntPtr handle) : base (handle)
		{
			
		}

		// the navigation controller


		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

		}

		public void loadMenuData()
		{
			//List<JobCMS> favJobList = DbHelper.GetFavoriteJobs();

			string favJobs = "Favourite Jobs";

			//if (favJobList != null)
			//{
			//	if (favJobList.Count > 0)
			//		favJobs = string.Format("Favourite Jobs ({0})", favJobList.Count);

			//}

			string[] tableItems = new string[] { "Home", "Branch Locator", favJobs, "About Us","Settings" };
			string[] tableIcons = new string[] { "home-icon.png", "branch_locator.png", "fav-icon.png", "about-icon.png","setting.png"};

			tblView.Source = new TableSource(tableItems, tableIcons, this);
			tblView.TableHeaderView = new UIImageView(UIImage.FromBundle("header-180"));
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			_pathToDatabase = Path.Combine(documents, "db_sqlite-net.db");
			SQLiteConnection dbConnection = DbHelper.GetConnection(_pathToDatabase, new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS());
			DbHelper.CreateDatabaseAndTables();


			//var contentController = (ContentController)Storyboard.InstantiateViewController("ContentController");
			//var introController = (IntroController)Storyboard.InstantiateViewController("IntroController");

			//ContentButton.TouchUpInside += (o, e) => {
			//	if (NavController.TopViewController as ContentController == null)
			//		NavController.PushViewController(contentController, true);
			//	SidebarController.CloseMenu();
			//};

			this.loadMenuData();

		}

		public class TableSource : UITableViewSource 
		{

			string[] TableItems;
			string[] TableIcons;
			MenuController menuController;
			string CellIdentifier = "TableCell";

			public TableSource(string[] items, string[] icons, MenuController owner)
			{
				TableItems = items;
				TableIcons = icons;
				menuController = owner;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return TableItems.Length;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				
				UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
				string item = TableItems[indexPath.Row];

				//---- if there are no cells to reuse, create a new one
				if (cell == null)
				{ cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

				cell.TextLabel.Text = item;

				//			var imageViewLocation = new UIImageView(UIImage.FromBundle("location-icon.png"));

				cell.ImageView.Image = UIImage.FromBundle(TableIcons[indexPath.Row]);
				//cell.ImageView = new UIImageView(UIImage.FromBundle(TableIcons[indexPath.Row]));

				return cell;
			}


			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				
				tableView.DeselectRow(indexPath, true);



				if (indexPath.Row == 0)
				{


					var introController = (IntroController)menuController.Storyboard.InstantiateViewController("IntroController");

					if (menuController.NavController.TopViewController as IntroController == null)
						menuController.NavController.PushViewController(introController, true);

				}
				else if (indexPath.Row == 1)
				{
					var _branchSearchVC = (BranchSearchVC)menuController.Storyboard.InstantiateViewController("BranchSearchVC");

					if (menuController.NavController.TopViewController as BranchSearchVC == null)	
						menuController.NavController.PushViewController(_branchSearchVC, true);

				}
				else if (indexPath.Row == 2)
				{
					var searchResultVC = (SearchResultVC)menuController.Storyboard.InstantiateViewController("SearchResultVC");
					searchResultVC.jobList = DbHelper.GetFavoriteJobs();
					searchResultVC.isFavoriteJob = true;

					if (menuController.NavController.TopViewController as SearchResultVC == null)
						menuController.NavController.PushViewController(searchResultVC, true);

				}

				else if (indexPath.Row == 3)
				{
					// about us 

					var _webViewController = (WebViewController)menuController.Storyboard.InstantiateViewController("WebViewController");
					_webViewController.isAboutUs = true;


					if (menuController.NavController.TopViewController as WebViewController == null)
						menuController.NavController.PushViewController(_webViewController, true);

				}


				else if (indexPath.Row == 4)
				{
					// Setting us 
					var _settingViewContrroler = (SettingViewContrroler)menuController.Storyboard.InstantiateViewController("SettingViewContrroler");

					if (menuController.NavController.TopViewController as SettingViewContrroler == null)
						menuController.NavController.PushViewController(_settingViewContrroler, true);

				}


				menuController.SidebarController.CloseMenu();

				//ContentButton.TouchUpInside += (o, e) => {
				//	if (NavController.TopViewController as ContentController == null)
				//		NavController.PushViewController(contentController, true);
				//	SidebarController.CloseMenu();
			}
		}


	}

}
