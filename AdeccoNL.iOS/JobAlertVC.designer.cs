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
    [Register ("JobAlertVC")]
    partial class JobAlertVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnChckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPrivacypolicy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTermsNCondition { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        [Action ("BtnChckBox_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnChckBox_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnPrivacypolicy_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnPrivacypolicy_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnTermsNCondition_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnTermsNCondition_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnChckBox != null) {
                btnChckBox.Dispose ();
                btnChckBox = null;
            }

            if (btnPrivacypolicy != null) {
                btnPrivacypolicy.Dispose ();
                btnPrivacypolicy = null;
            }

            if (btnTermsNCondition != null) {
                btnTermsNCondition.Dispose ();
                btnTermsNCondition = null;
            }

            if (tblView != null) {
                tblView.Dispose ();
                tblView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}