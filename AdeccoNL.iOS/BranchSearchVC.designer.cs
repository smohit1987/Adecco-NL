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
    [Register ("BranchSearchVC")]
    partial class BranchSearchVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView activityIndicatorLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView bgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCurrentLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnsearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistance { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPlace { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtDistance { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtLocation { get; set; }

        [Action ("Btnsearch_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Btnsearch_TouchUpInside (UIKit.UIButton sender);

        [Action ("UIButton1070_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1070_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (activityIndicatorLocation != null) {
                activityIndicatorLocation.Dispose ();
                activityIndicatorLocation = null;
            }

            if (bgView != null) {
                bgView.Dispose ();
                bgView = null;
            }

            if (btnCurrentLocation != null) {
                btnCurrentLocation.Dispose ();
                btnCurrentLocation = null;
            }

            if (btnsearch != null) {
                btnsearch.Dispose ();
                btnsearch = null;
            }

            if (lblDistance != null) {
                lblDistance.Dispose ();
                lblDistance = null;
            }

            if (lblPlace != null) {
                lblPlace.Dispose ();
                lblPlace = null;
            }

            if (txtDistance != null) {
                txtDistance.Dispose ();
                txtDistance = null;
            }

            if (txtLocation != null) {
                txtLocation.Dispose ();
                txtLocation = null;
            }
        }
    }
}