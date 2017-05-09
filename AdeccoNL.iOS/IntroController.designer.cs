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
    [Register ("IntroController")]
    partial class IntroController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView activityIndicatorKeyword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView activityIndicatorLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCurrentLoaction { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnJobSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView searchBg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl segmentCtrl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtKeyword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtLocation { get; set; }

        [Action ("BtnCurrentLoaction_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnCurrentLoaction_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (activityIndicatorKeyword != null) {
                activityIndicatorKeyword.Dispose ();
                activityIndicatorKeyword = null;
            }

            if (activityIndicatorLocation != null) {
                activityIndicatorLocation.Dispose ();
                activityIndicatorLocation = null;
            }

            if (btnCurrentLoaction != null) {
                btnCurrentLoaction.Dispose ();
                btnCurrentLoaction = null;
            }

            if (btnJobSearch != null) {
                btnJobSearch.Dispose ();
                btnJobSearch = null;
            }

            if (searchBg != null) {
                searchBg.Dispose ();
                searchBg = null;
            }

            if (segmentCtrl != null) {
                segmentCtrl.Dispose ();
                segmentCtrl = null;
            }

            if (tblView != null) {
                tblView.Dispose ();
                tblView = null;
            }

            if (txtKeyword != null) {
                txtKeyword.Dispose ();
                txtKeyword = null;
            }

            if (txtLocation != null) {
                txtLocation.Dispose ();
                txtLocation = null;
            }
        }
    }
}