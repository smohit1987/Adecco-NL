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
    [Register ("RefineViewController")]
    partial class RefineViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnApply { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView footerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblRadius { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider radiusSlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView Table { get; set; }

        [Action ("UIButton1160_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1160_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnApply != null) {
                btnApply.Dispose ();
                btnApply = null;
            }

            if (footerView != null) {
                footerView.Dispose ();
                footerView = null;
            }

            if (lblRadius != null) {
                lblRadius.Dispose ();
                lblRadius = null;
            }

            if (radiusSlider != null) {
                radiusSlider.Dispose ();
                radiusSlider = null;
            }

            if (Table != null) {
                Table.Dispose ();
                Table = null;
            }
        }
    }
}