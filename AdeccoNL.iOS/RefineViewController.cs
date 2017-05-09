using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Linq;
using BigTed;
using System.Drawing;
using System.Globalization;
using Google.Analytics;

namespace AdeccoNL.iOS
{
	public partial class RefineViewController : UIViewController
	{
		public List<PresentationFacetResult> presentationFacetResultList { get; set;}
		public JobRequest jobRequest { get; set; }

		protected RefineViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
			{
				ForegroundColor = UIColor.White
			};

			this.Title = "Refine";

			/*
			 this.NavigationItem.SetLeftBarButtonItem(
			new UIBarButtonItem("Reset"
								, UIBarButtonItemStyle.Done
			, (sender, args) =>
			{
				//Apply button was clicked
				//var sfViewController = new SFSafariViewController(new NSUrl(this.aJobDetail.ApplyUri));
				//PresentViewControllerAsync(sfViewController, true);
				var alert = UIAlertController.Create("Adecco Nederland", "Are you sure ? You want to remove all the filter.", UIAlertControllerStyle.Alert);

				alert.AddAction(UIAlertAction.Create("No Thanks", UIAlertActionStyle.Default, null));
				alert.AddAction(UIAlertAction.Create("YES", UIAlertActionStyle.Default, action => resetFilterHandler()));

				PresentViewController(alert, animated: true, completionHandler: null);


			})
		, true);
		*/

			this.NavigationItem.SetRightBarButtonItem(
			new UIBarButtonItem("RESET"
								, UIBarButtonItemStyle.Done
			, (sender, args) =>
			{

				//Reset button was clicked
				var alert = UIAlertController.Create("Adecco Nederland", "Are you sure ? You want to remove all the filter.", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("No Thanks", UIAlertActionStyle.Default, null));
				alert.AddAction(UIAlertAction.Create("YES", UIAlertActionStyle.Default, action => resetFilterHandler()));
				PresentViewController(alert, animated: true, completionHandler: null);

			})
		, true);

			//#EEEEEE   light Grey color 
			//View.BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f);

			btnApply.BackgroundColor = UIColor.Red; //UIColor.Clear.FromHexString("#ef2e24", 1.0f);


			//#FFFFFF  white color 
			//Table.BackgroundColor = UIColor.Clear.FromHexString("##FFFFFF", 1.0f);
			//Table.Layer.BorderColor = UIColor.LightGray.CGColor;
			//Table.Layer.BorderWidth = 0.5f;
			//Table.Layer.CornerRadius = 2.0f;
			//Table.Layer.MasksToBounds = true;
			//Table.ClipsToBounds = true;


			Table.Source = new ExpandableTableSource(this.presentationFacetResultList, Table,this);

			this.appliedFilterList();


			// show radius slider if location exist only
			if (jobRequest.FilterURL.Contains("&xy"))
				footerView.Hidden = false;
			
			else
				footerView.Hidden = true;
		
				
		
			//Constants.selectedRadius = "&r=" + selectedRadius;


			string[] tokens = Constants.selectedRadius.Split(new[] { "=" }, StringSplitOptions.None);

			float radius =  float.Parse(tokens[1], CultureInfo.InvariantCulture.NumberFormat);

			radiusSlider.SetValue(radius, true);

			lblRadius.Text = string.Format("Distance {0} Kilometers", tokens[1]);

		    radiusSlider.Continuous = false;

			radiusSlider.TouchUpInside += (o, e) =>
			 {

				int kSliderInterval = 5;
				double sliderValue = kSliderInterval * Math.Floor((radiusSlider.Value / kSliderInterval) + 0.5);
				var selectedRadius = Convert.ToInt32(sliderValue);
				radiusSlider.SetValue((float) sliderValue, true);
				Constants.isFileterApplied = true;
				//var selectedRadius = Convert.ToInt32(radiusSlider.Value);
				lblRadius.Text = string.Format("Distance {0} Kilometers", selectedRadius);
				 // &r=25"

				this.radiusValueChanged(selectedRadius.ToString());
			};

			radiusSlider.ValueChanged += (o, e) =>
			{
				var selectedRadius = Convert.ToInt32(radiusSlider.Value);
				lblRadius.Text = string.Format("Beam {0} mileage", selectedRadius);
						
			};


			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.SidebarController.Disabled = true;

			Gai.SharedInstance.DefaultTracker.Set(GaiConstants.ScreenName, "Refine");
			Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());

		}

		void radiusValueChanged(string  selectedRadius)
		{
			string filterUrl = Constants.FilterURL;

			string updatedRadius = "&r=" + selectedRadius;


			if (string.IsNullOrEmpty(filterUrl))
				filterUrl = jobRequest.FilterURL + "&r=" + selectedRadius;

			else
			{
				// if &r=25 exist in filetr url then replace it

				if (filterUrl.Contains(Constants.selectedRadius))
				{
					filterUrl = filterUrl.Replace(Constants.selectedRadius,updatedRadius);
				}
				else
				{   // else simply add &r=25
					filterUrl = Constants.FilterURL + updatedRadius;
				}

			}

			Constants.selectedRadius = "&r=" + selectedRadius;
			Constants.FilterURL = filterUrl;

			this.GetJobSearchData();
		}

		void appliedFilterList()
		{
			List<SelectedFacets> selectedFacetsList = new List<SelectedFacets>();

			if (!Constants.jobSearchResponse.ContainsKey("selectedFacetsList"))
			{
				Table.TableHeaderView = new UIView();
				//Table.TableFooterView = new UIView();

				return;

			}
			else
			{
				selectedFacetsList = Constants.jobSearchResponse["selectedFacetsList"];

				if (selectedFacetsList.Count < 1)
				{
					Table.TableHeaderView = new UIView();
					//Table.TableFooterView = new UIView();

					return;
				}

			}

			float scrollheight = 50.0f;

			//if ((selectedFacetsList.Count * 40) < 120)
			//	scrollheight = selectedFacetsList.Count * 40 + 5;

			UIScrollView scrollView = new UIScrollView
			{
				Frame = new CGRect(0, 0, Table.Frame.Width, scrollheight),
				//ContentSize = new CGSize(Table.Frame.Size.Width, selectedFacetsList.Count * 40),
				BackgroundColor = UIColor.Clear.FromHexString("##EEEEEE", 1.0f),
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth
			};


			float xPos = 5.0f, yPos = 10.0f;
			int index = 0;

			float maxButtonWidth = (float)Table.Frame.Size.Width;

			foreach (SelectedFacets aSelectedFacets in selectedFacetsList)
			{
				var titleString = new NSString(aSelectedFacets.keyName);

				CGSize size = titleString.GetSizeUsingAttributes(new UIStringAttributes { Font = UIFont.SystemFontOfSize(12) });

				UIButton btn = new UIButton(UIButtonType.Custom);
				btn.SetTitle("  " + aSelectedFacets.keyName, UIControlState.Normal);
				btn.Frame = new RectangleF(xPos, yPos, (float)size.Width + 50, 30);

				maxButtonWidth = xPos + (float)size.Width + 50;

				xPos = maxButtonWidth + 5;



				btn.SetTitleColor(UIColor.Black, UIControlState.Normal);
				btn.Font = UIFont.SystemFontOfSize(12);
				btn.BackgroundColor = UIColor.Clear.FromHexString("##FFFFFF", 1.0f);
				btn.Layer.BorderColor = UIColor.LightGray.CGColor;
				btn.Layer.BorderWidth = 0.5f;
				btn.Layer.CornerRadius = 2.0f;
				btn.Layer.MasksToBounds = true;
				btn.ClipsToBounds = true;
				btn.TouchUpInside += removeSelectedFilter;
				btn.Tag = index;
				btn.ShowsTouchWhenHighlighted = true;
				btn.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

				index++;
				scrollView.AddSubview(btn);

				UIImageView imgView = new UIImageView();
				imgView.Image = UIImage.FromBundle("grey-cross-icon.png");
				imgView.ContentMode = UIViewContentMode.ScaleAspectFit;
				//imgView.Frame = new RectangleF((float)(btn.Frame.GetMaxX() - 10), (float)(btn.Frame.Y - 6), 16, 16);

				imgView.Frame = new RectangleF((float)(btn.Frame.GetMaxX() - 22), 17, 16, 16);

				scrollView.AddSubview(imgView);

				//tableHeaderView.AddSubview(btn);

				//yPos = yPos + 35;

			}

			scrollView.ContentSize = new CGSize(maxButtonWidth, scrollheight);

			//Table.TableFooterView = new UIView();
			Table.TableHeaderView = scrollView;


			CGRect tableFrame = Table.Frame;
			tableFrame.Y = 0;
			Table.Frame = tableFrame;


			Table.ReloadData();
		}

	

		public void removeSelectedFilter(object sender, EventArgs e)
		{
			UIButton filterButton = sender as UIButton;

			List<SelectedFacets> selectedFacetsList = Constants.jobSearchResponse["selectedFacetsList"];

			SelectedFacets aSelectedFacets = selectedFacetsList[(int)filterButton.Tag];
			Constants.FilterURL = Constants.JobBaseAddress + aSelectedFacets.valueName;

			this.GetJobSearchData();

		}

		void resetFilterHandler()
		{
			Constants.isFileterApplied = false;
			Constants.FilterURL = "";
			Constants.shouldResetFilter = true;
			Constants.jobSearchResponse = new Dictionary<string, dynamic>();
			Constants.selectedRadius = "&r=" + "10";

			this.NavigationController.PopViewController(true);

		}

		// Apply button code
		void UIButton928_TouchUpInside(UIButton sender)
		{

			if (!string.IsNullOrEmpty(Constants.FilterURL))
			{
				Constants.isFileterApplied = true;

			}
			else
				Constants.isFileterApplied = false;

			Constants.selectedRadius = "&r=" + radiusSlider.Value;

			this.NavigationController.PopViewController(true);
		}

		// APPLY BUTTON PRESSED
		partial void UIButton1160_TouchUpInside(UIButton sender)
		{
			if (!string.IsNullOrEmpty(Constants.FilterURL))
			{
				Constants.isFileterApplied = true;

			}
			else
				Constants.isFileterApplied = false;

			this.NavigationController.PopViewController(true);
		}

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		public async void GetJobSearchData()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void

		{

			NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();
			if (remoteHostStatus == NetworkStatus.NotReachable)
			{

				var alert = UIAlertController.Create("Network Error", "Please check your internet connection", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
				PresentViewController(alert, animated: true, completionHandler: null);

				return;
			}

			BTProgressHUD.Show("Loading...", -1, ProgressHUD.MaskType.Black);


			if (!Constants.FilterURL.Contains("&r=") && jobRequest.FilterURL.Contains("&xy"))
			{
				Constants.FilterURL = Constants.FilterURL + "&r=" + Constants.selectedRadius;

			}
			// Refine result if filter appiled.
			if (!string.IsNullOrEmpty(Constants.FilterURL))
				this.jobRequest.FilterURL = Constants.FilterURL;

			ServiceManager jobService = new ServiceManager();
			Constants.jobSearchResponse  = await jobService.AsyncJobSearch(this.jobRequest);

			this.presentationFacetResultList = Constants.jobSearchResponse["presentationFacetResultList"]; 

			Table.Source = new ExpandableTableSource(this.presentationFacetResultList, Table, this);

			this.appliedFilterList();

			Table.ReloadData();

			BTProgressHUD.Dismiss();

		}
	}

	public class ExpandableTableSource : UITableViewSource
	{
		//List<ExpandableTableModel<T>> TableItems;
		List<PresentationFacetResult> _presentationFacetResultList;

		RefineViewController _refineViewController;

		private bool[] _isSectionOpen;
		private EventHandler _headerButtonCommand;

		public ExpandableTableSource(List<PresentationFacetResult> aPresentationFacetResultList, UITableView tableView, RefineViewController aRefineViewController)
		{
			_presentationFacetResultList = aPresentationFacetResultList;
			_isSectionOpen = new bool[aPresentationFacetResultList.Count];

			_refineViewController = aRefineViewController;

			tableView.RegisterNibForCellReuse(UINib.FromName(RefineCell.Key, NSBundle.MainBundle), RefineCell.Key);
			tableView.RegisterNibForHeaderFooterViewReuse(UINib.FromName(HeaderCell.Key, NSBundle.MainBundle), HeaderCell.Key);

			_headerButtonCommand = (sender, e) =>
			{
				var button = sender as UIButton;
				var section = button.Tag;
				_isSectionOpen[(int)section] = !_isSectionOpen[(int)section];

				HeaderCell headeCell = 	(HeaderCell)button.Superview;

				if (_isSectionOpen[(int)section])
				{
					headeCell.arrowBuuton.SetImage(UIImage.FromBundle("grey_arrow-down-icon.png"), UIControlState.Normal);

				}
				else 
				{
					headeCell.arrowBuuton.SetImage(UIImage.FromBundle("grey_arrow-right-icon.png"), UIControlState.Normal);
				}


				tableView.ReloadData();

				// Animate the section cells
				var paths = new NSIndexPath[RowsInSection(tableView, section)];
				for (int i = 0; i < paths.Length; i++)
				{
					paths[i] = NSIndexPath.FromItemSection(i, section);
				}

				tableView.ReloadRows(paths, UITableViewRowAnimation.Automatic);
			};
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return _presentationFacetResultList.Count;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			PresentationFacetResult facets = _presentationFacetResultList[(int)section];

			return _isSectionOpen[(int)section] ? facets.ListFacetValues.Count : 0;
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return 44f;
		}

		public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section)
		{
			return 44f;
		}

		public override UIView GetViewForHeader(UITableView tableView, nint section)
		{
			PresentationFacetResult facet = _presentationFacetResultList[(int)section];

			HeaderCell header = tableView.DequeueReusableHeaderFooterView(HeaderCell.Key) as HeaderCell;
	  		header.label.Text = facet.FacetDisplayName;


			if (_isSectionOpen[(int)section])
				header.arrowBuuton.SetImage(UIImage.FromBundle("grey_arrow-down-icon.png"), UIControlState.Normal); 
			else
				header.arrowBuuton.SetImage(UIImage.FromBundle("grey_arrow-right-icon.png"), UIControlState.Normal);



			foreach (var view in header.Subviews)
			{
				if (view is HiddenHeaderButton)
				{
					view.RemoveFromSuperview();
				}
			}
			var hiddenButton = CreateHiddenHeaderButton(header.Bounds, section);
			header.AddSubview(hiddenButton);
			return header;
		}

		private HiddenHeaderButton CreateHiddenHeaderButton(CGRect frame, nint tag)
		{
			var button = new HiddenHeaderButton(frame);
			button.Tag = tag;
			button.TouchUpInside += _headerButtonCommand;
			//header.png
			//button.SetBackgroundImage(UIImage.FromBundle("header.png"), UIControlState.Highlighted);
			//button.SetImage(UIImage.FromBundle("header.png"), UIControlState.Selected);

			return button;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(RefineCell.Key, indexPath) as RefineCell;

			PresentationFacetResult facets = _presentationFacetResultList[(int)indexPath.Section];
			FacetValue aFacetValue = facets.ListFacetValues[(int)indexPath.Row];

			cell.label.Text = aFacetValue.ValueName + "(" + aFacetValue.ValueCount +  ")";

			// If ValueUnChecked is not null that means filter is selected. 
			if (string.IsNullOrEmpty(aFacetValue.ValueChecked) || aFacetValue.isSelected)
			{
				//cell.imageView.Image = UIImage.FromBundle("arrow-down-icon.png");
				cell.label.TextColor = UIColor.Red;
				cell.checkBoxButton.SetImage(UIImage.FromBundle("check.png"), UIControlState.Normal);

			}
			else
			{
				//cell.imageView.Image = UIImage.FromBundle("arrow-down.png");
				cell.label.TextColor = UIColor.DarkGray;
				cell.checkBoxButton.SetImage(UIImage.FromBundle("uncheck.png"), UIControlState.Normal);
			}


			return cell;
		}
		 
		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			
			var cell = tableView.CellAt(indexPath) as RefineCell;

			PresentationFacetResult facets = _presentationFacetResultList[(int)indexPath.Section];
			FacetValue aFacetValue = facets.ListFacetValues[(int)indexPath.Row];

			string filterUrl = (!string.IsNullOrEmpty(aFacetValue.ValueChecked)) ? aFacetValue.ValueChecked : aFacetValue.ValueUnChecked;

			bool isSelected = !aFacetValue.isSelected;

			if (isSelected)
			{
				//filterUrl = aFacetValue.ValueChecked;
				cell.checkBoxButton.SetImage(UIImage.FromBundle("check.png"), UIControlState.Normal);
				cell.label.TextColor = UIColor.Red;

			}
			else 
			{
				//filterUrl = aFacetValue.ValueUnChecked;
				cell.checkBoxButton.SetImage(UIImage.FromBundle("uncheck.png"), UIControlState.Normal);
				cell.label.TextColor = UIColor.DarkGray;

			}

			Constants.FilterURL = Constants.JobBaseAddress + filterUrl;
			tableView.DeselectRow(indexPath, true);

			aFacetValue.isSelected = isSelected;

			_refineViewController.GetJobSearchData();
		}

	}

	public class HiddenHeaderButton : UIButton
	{
		public HiddenHeaderButton(CGRect frame) : base(frame)
		{

		}
	}

	public class ExpandableTableModel<T> : List<T>
	{
		public string Title { get; set; }
		public ExpandableTableModel(IEnumerable<T> collection) : base(collection) { }
		public ExpandableTableModel() : base() { }
	}
}


