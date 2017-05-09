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
    [Register ("SettingViewContrroler")]
    partial class SettingViewContrroler
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView bgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnApply { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtLanguage { get; set; }

        [Action ("BtnApply_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnApply_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (bgView != null) {
                bgView.Dispose ();
                bgView = null;
            }

            if (btnApply != null) {
                btnApply.Dispose ();
                btnApply = null;
            }

            if (txtCountry != null) {
                txtCountry.Dispose ();
                txtCountry = null;
            }

            if (txtLanguage != null) {
                txtLanguage.Dispose ();
                txtLanguage = null;
            }
        }
    }
}