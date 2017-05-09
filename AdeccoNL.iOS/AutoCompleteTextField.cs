using System;
using System.Collections.Generic;
using UIKit;
using System.Linq;
using Foundation;
using CoreGraphics;

namespace AdeccoNL.iOS
{
	public static class AutoCompleteTextFieldManager
	{
		private static List<AutoCompleteTextField> autoCompleteTextFields;

		public static void Add(UIViewController viewController, UITextField textView, List<string> elements)
		{
			
			
			RemoveAll();

			if (autoCompleteTextFields == null)
			{
				autoCompleteTextFields = new List<AutoCompleteTextField>();
			}

			if (viewController == null || textView == null || elements == null)
				return;
			
			autoCompleteTextFields.Add(new AutoCompleteTextField(viewController, textView, elements));
		}


		public static void RemoveAll()
		{
			if (autoCompleteTextFields != null)
			{
				foreach (AutoCompleteTextField autoTxtfld in autoCompleteTextFields)
				{
					if (autoTxtfld.autoCompleteTableView != null)
					{
						autoTxtfld.autoCompleteTableView.Hidden = true;
						autoTxtfld.autoCompleteTableView.RemoveFromSuperview();
					}

				}

				autoCompleteTextFields.Clear();
				autoCompleteTextFields = null;
			}
		}


		public static void RemoveTable()
		{
			//objAutoCompleteTextField.RemoveTableView ();
			if (autoCompleteTextFields != null && autoCompleteTextFields.Count > 0)
				autoCompleteTextFields[0].RemoveTableView();
		}

		public static void BringTableFront()
		{
			//objAutoCompleteTextField.RemoveTableView ();
			if (autoCompleteTextFields != null && autoCompleteTextFields.Count > 0)
				autoCompleteTextFields[0].BringTableViewFront();
		}

		public static void DeleteTable()
		{
			//objAutoCompleteTextField.RemoveTableView ();
			if (autoCompleteTextFields != null && autoCompleteTextFields.Count > 0)
				autoCompleteTextFields[0].deleteTableView();
		}


		private class AutoCompleteTextField
		{
			private UIViewController viewController;
			private UITextField textField;
			private int selectedIndex = -1;
			private List<string> elements;
			private List<string> matchedElements;
			public UITableView autoCompleteTableView;
			private UITableViewSource autoCompleteTableViewSource;
			private EventHandler textFieldEditingChangedHandler;

			public AutoCompleteTextField(UIViewController viewController, UITextField textView, List<string> elements)
			{
				try
				{
					this.viewController = viewController;
					this.textField = textView;
					this.selectedIndex = -1;
					this.elements = elements;
					//this.matchedElements = this.elements.Where(e => e.ToLower().Contains(this.textField.Text.ToLower())).ToList();
					this.matchedElements = elements;
					int Count = matchedElements.Count;


						
					this.autoCompleteTableView = new UITableView(new CoreGraphics.CGRect(this.textField.Frame.X, this.textField.Frame.Y + this.textField.Frame.Height + 10, this.textField.Frame.Width, Count * 35));

					// branch locator text field layout
					if (this.textField.Tag == 1032)
						this.autoCompleteTableView = new UITableView(new CoreGraphics.CGRect(this.textField.Frame.X, this.textField.Frame.Y + this.textField.Frame.Height + 80, this.textField.Frame.Width, Count * 35));


					this.autoCompleteTableViewSource = new AutoCompleteTableViewSource(this.elements, this.SelectedElement,this.textField);
					this.autoCompleteTableView.Source = this.autoCompleteTableViewSource;

					this.autoCompleteTableView.ReloadData();
					this.autoCompleteTableView.ScrollEnabled = true;
					this.autoCompleteTableView.Hidden = false;

					this.autoCompleteTableView.BackgroundColor = UIColor.White;
					//this.autoCompleteTableView.Alpha = 0.8f;

					// branch locator text field layout
					//if (this.textField.Tag == 1032)
					//{
					//	this.autoCompleteTableView.BackgroundColor = UIColor.Black;
					//	this.autoCompleteTableView.Alpha = 0.8f;

					//}


					this.viewController.View.AddSubview(this.autoCompleteTableView);
					this.viewController.View.BringSubviewToFront(this.autoCompleteTableView);
					textField.EditingChanged -= this.GetTextFieldEditingChangedHandler();
					textField.EditingChanged += this.GetTextFieldEditingChangedHandler();
					this.autoCompleteTableView.AllowsSelection = true;


				}
				catch (Exception ex)
				{
					string str = ex.Message;
					Console.WriteLine("== AutoCompleteTextField Exception=== {0}",str);
					       
				}


			}

			public void RemoveTableView()
			{

				if (this.autoCompleteTableView != null)
				{
					this.autoCompleteTableView.Hidden = true;
					this.autoCompleteTableView.RemoveFromSuperview();
				}

				//((UITableView)this.autoCompleteTableView.ViewWithTag(intTag)).RemoveFromSuperview();
			}
			public void BringTableViewFront()
			{
				this.viewController.View.BringSubviewToFront(this.autoCompleteTableView);
			}

			public void deleteTableView()
			{
				if (this.autoCompleteTableView != null)
				{
					this.autoCompleteTableView.Hidden = true;
					this.autoCompleteTableView.RemoveFromSuperview();

				}


			}

			private void SelectedElement(nint index)
			{
				this.selectedIndex = (int)index;
				string selectedElement = this.matchedElements[(int)this.selectedIndex];

				if(selectedElement.Equals("geen resultaten gevoden"))
					this.textField.Text = "";
				else 
					this.textField.Text = selectedElement;

				// need to fetch lat long
				Constants.shouldGeoCodeLocation = true;

				// Location text field 
				if (this.textField.Tag == 102 && !Constants.isGoogleLocation.Equals("1"))
				{
					
				    JobLocation _JobLocation  = DbHelper.JobLocations[(int)this.selectedIndex];
					//DbHelper.locationCordinates = _JobLocation.coordinates;
					Constants.LocationLatLong = _JobLocation.coordinates[0] + "%2C" + _JobLocation.coordinates[1];


				}


				this.autoCompleteTableView.Hidden = true;
				this.autoCompleteTableView.RemoveFromSuperview();
			}

			private EventHandler GetTextFieldEditingChangedHandler()
			{
				if (this.elements != null)
				{

					if ((this.textFieldEditingChangedHandler == null && this.textField.Text.Length > 2) && this.elements.Count > 0)
					{
						this.textFieldEditingChangedHandler = delegate (object sender, EventArgs e)
						{
							this.selectedIndex = -1;
							this.autoCompleteTableView.Hidden = false;
							//this.matchedElements = this.elements.Where(elem => elem.ToLower().Contains(this.textField.Text.ToLower())).ToList();
							this.matchedElements = this.elements;
							this.autoCompleteTableViewSource = new AutoCompleteTableViewSource(this.matchedElements, this.SelectedElement,this.textField);
							this.autoCompleteTableView.Source = this.autoCompleteTableViewSource;
							int Count = matchedElements.Count;
							this.autoCompleteTableView.Frame = new CGRect(this.autoCompleteTableView.Frame.X, this.autoCompleteTableView.Frame.Y, this.autoCompleteTableView.Frame.Size.Width, Count * 35);
							this.autoCompleteTableView.ReloadData();
						};
					}
				}
				return this.textFieldEditingChangedHandler;

			}


			private class AutoCompleteTableViewSource : UITableViewSource
			{
				private List<string> elements;
				private Action<nint> selectedElementCallback;

				UITextField textField;

				public AutoCompleteTableViewSource(List<string> elements, Action<nint> selectedElementCallback, UITextField aTextField)
				{
					this.elements = elements;
					this.selectedElementCallback = selectedElementCallback;
					this.textField = aTextField;
				}

				public override nint RowsInSection(UITableView tableview, nint section)
				{
					return elements.Count;
				}

				public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
				{
					return 35;
				}

				public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section)
				{
					return 0;
				}

				public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
				{
					string element = this.elements[indexPath.Row];

					UITableViewCell cell = new UITableViewCell();
					cell.TextLabel.Text = element;
					cell.TextLabel.Font = UIFont.SystemFontOfSize(13);
					cell.TextLabel.TextColor = UIColor.Black;

					// branch locator text field layout
					//if(textField.Tag ==1032)
					//	cell.TextLabel.TextColor = UIColor.White;


					cell.BackgroundColor = UIColor.Clear;
					cell.UserInteractionEnabled = true;

					UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer();
					tapRecognizer.NumberOfTapsRequired = 1;
					cell.ContentView.AddGestureRecognizer(tapRecognizer);
					tapRecognizer.AddTarget(() =>
					{
						this.DidSelectRow(tableView, indexPath.Row);
					});


					return cell;
				}

				public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
				{
					if (selectedElementCallback != null) selectedElementCallback(indexPath.Row);
				}

				void DidSelectRow(UITableView tableView, int selectedIndex)
				{
					NSIndexPath indexPath = NSIndexPath.FromRowSection(selectedIndex, 0);
					this.RowSelected(tableView, indexPath);

				}

			}
		}
	}
}
