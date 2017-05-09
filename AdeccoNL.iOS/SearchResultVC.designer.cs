// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AdeccoNL.iOS
{
    [Register ("SearchResultVC")]
    partial class SearchResultVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView custmFooterView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton filterButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel headerLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView horizontalSepratorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton jobAlertButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView verticalSeprator { get; set; }

        [Action ("FilterButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FilterButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("JobAlertButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void JobAlertButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (custmFooterView != null) {
                custmFooterView.Dispose ();
                custmFooterView = null;
            }

            if (filterButton != null) {
                filterButton.Dispose ();
                filterButton = null;
            }

            if (headerLabel != null) {
                headerLabel.Dispose ();
                headerLabel = null;
            }

            if (horizontalSepratorView != null) {
                horizontalSepratorView.Dispose ();
                horizontalSepratorView = null;
            }

            if (jobAlertButton != null) {
                jobAlertButton.Dispose ();
                jobAlertButton = null;
            }

            if (tblView != null) {
                tblView.Dispose ();
                tblView = null;
            }

            if (verticalSeprator != null) {
                verticalSeprator.Dispose ();
                verticalSeprator = null;
            }
        }
    }
}