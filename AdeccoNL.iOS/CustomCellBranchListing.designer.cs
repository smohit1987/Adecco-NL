// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AdeccoNL.iOS
{
    [Register ("CustomCellBranchListing")]
    partial class CustomCellBranchListing
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton directionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton emailButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton phoneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        [Action ("DirectionButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DirectionButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("PhoneLabel_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PhoneLabel_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (addressLabel != null) {
                addressLabel.Dispose ();
                addressLabel = null;
            }

            if (directionButton != null) {
                directionButton.Dispose ();
                directionButton = null;
            }

            if (emailButton != null) {
                emailButton.Dispose ();
                emailButton = null;
            }

            if (phoneButton != null) {
                phoneButton.Dispose ();
                phoneButton = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}